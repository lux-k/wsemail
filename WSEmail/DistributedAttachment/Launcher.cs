/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.IO;
using System.Xml;
using System.Security.Cryptography;
using System.Windows.Forms;
using DynamicForms;
using Microsoft.Web.Services2;
using Microsoft.Web.Services2.Dime;
using Microsoft.Win32;
using Microsoft.Web.Services2.Security;
using Microsoft.Web.Services2.Security.Tokens;


namespace DistributedAttachment
{

	/// <summary>
	/// Defines a distributed attachment which uses federated tokens to retrieve attachments. The idea is
	/// you send an attachment as a distributed attachment, your local server saves a copy and forwards a link
	/// to the recipients. They can then download the file if/when they want it and the sender can update the file
	/// to newer versions.
	/// </summary>
	public class DistributedAttachment : BaseObject
	{

		/// <summary>
		/// The file attachments for this message.
		/// </summary>
		public FileAttachment[] Attachments;

		/// <summary>
		/// Default constructor. Initializes the dynamic configuration.
		/// </summary>
		public DistributedAttachment()
		{
			InitializeDynamicConfiguration();
		}

		/// <summary>
		/// Stupid message.
		/// </summary>
		/// <returns></returns>
		public override string DebugToScreen() 
		{
			return "I am a distributed attachment downloader.";
		}

		/// <summary>
		/// Called when the attachment is going away.
		/// </summary>
		public override void Dispose()
		{
			if (!this.IsDisposed)
			{
				this.IsDisposed = true;
			}
		}

		/// <summary>
		/// The configuration for this object is set.
		/// </summary>
		public override void InitializeDynamicConfiguration() 
		{
			this.LockEmail = false;
			this.Configuration.DLL = "DistributedAttachment";
			this.Configuration.Name = "DistributedAttachment.DistributedAttachment";
			this.Configuration.Url = "http://tower.cis.upenn.edu/classes/DistributedAttachment.dll";
			this.Configuration.Version = 1.0F;
			this.Configuration.FriendlyName = "Distributed Attachment";
			this.Configuration.Description = "Allows servers to save bandwith by using Federated Identities.";
		}
		private void LoadAttachment() 
		{
			// create proxy
			SecurityToken s = this.GetSecurityToken(BaseObject.TokenType.FederatedToken);
			if (s == null) 
			{
				MessageBox.Show("Access to the federated token was denied or there was an error obtaining the token.","Error downloading file");
				return;
			}
			try 
			{
				XmlDocument d = new XmlDocument();
				XmlElement root = d.CreateElement("FederatedAttachmentRequest");
				XmlElement key = d.CreateElement("FileKey");
				key.InnerText = this.Attachments[0].FileKey;
				root.AppendChild(key);
				FileRetrieverProxy frp = new FileRetrieverProxy(this.Attachments[0].ServerUrl);
				frp.SecurityToken = s;
				frp.ExecuteExtensionHandler("FederatedAttachment",root);
				if (frp.ResponseSoapContext.Attachments.Count > 0) 
				{
					DialogResult r = MessageBox.Show("A file was retrieved. Save and view it now?","Question...",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
					if (r == DialogResult.Yes) 
					{
						//System.Text.UTF8Encoding.UTF8.GetString(((MemoryStream)frp.ResponseSoapContext.Attachments[0].Stream).ToArray());
						if (!Directory.Exists("attachments"))
							Directory.CreateDirectory("attachments");
						string hash = "";
						SHA1 sha = SHA1.Create();
						try 
						{
							hash = Convert.ToBase64String(sha.ComputeHash((MemoryStream)frp.ResponseSoapContext.Attachments[0].Stream));
						} 
						catch 
						{
							MessageBox.Show("Unable to verify the authenticity of the attachment!!");
						}

						sha.Clear();
						
						if (Attachments[0].FileHash.Equals(""))
							MessageBox.Show("Missing hash value on attachment.. authenticity is questionable.");

						if (!Attachments[0].FileHash.Equals(hash))
							MessageBox.Show("Attachment hash is different from original.");

						FileStream f = new FileStream(@"attachments\"+Attachments[0].FileName,FileMode.Create);
						((MemoryStream)frp.ResponseSoapContext.Attachments[0].Stream).WriteTo(f);
						f.Close();
						System.Diagnostics.Process.Start(@"attachments\"+Attachments[0].FileName);
					}
				} 
				else 
					MessageBox.Show("The server did not attach a file to its response. Perhaps a security error exists?","Oops!");
						
			} 
			catch (Exception e) 
			{
				MessageBox.Show(e.Message);
			}

		}

		private void GetAttachment() 
		{
			// add files!
			OpenFileDialog f = new System.Windows.Forms.OpenFileDialog();
			f.Title = "Select file to attach...";
			f.CheckFileExists = true;
			f.DereferenceLinks = true;
			f.ValidateNames = true;
			f.RestoreDirectory = true;
			DialogResult r = f.ShowDialog();
			if (DialogResult.OK == r) 
			{
				if (f.FileName != null && f.FileName.Length > 0) 
				{
					int i = f.FileName.LastIndexOf('.');
					string ext = "";
					if (i > 0)
						ext = f.FileName.Substring(i);;
						
					string filetype;
					// MessageBox.Show("Ext = " + ext);
				
					try 
					{

						RegistryKey k = Registry.ClassesRoot.OpenSubKey(ext);
						filetype = (string)k.GetValue("Content Type");
					} 
					catch
					{
						filetype="unknown";
					}

					// MessageBox.Show("type = " + filetype);
					try 
					{
						SHA1 sha = System.Security.Cryptography.SHA1.Create();
						FileStream fs = new FileStream(f.FileName,FileMode.Open);
						string hash = Convert.ToBase64String(sha.ComputeHash(fs));
						fs.Close();
						sha.Clear();

						DimeAttachment a = new DimeAttachment(filetype,TypeFormat.MediaType,f.FileName);
						a.ChunkSize = 10000;
						this.DimeAttachments = new DimeAttachment[] {a};
						string justname = f.FileName;

						justname = justname.Substring(justname.LastIndexOf(@"\")+1);
						this.Attachments = new FileAttachment[] { new FileAttachment(justname,hash) };

					} 
					catch (Exception e) 
					{
						MessageBox.Show(e.Message);
					}

				}
			}
			this.FormDone();
		}

		/// <summary>
		///  Runs the form.
		/// </summary>
		public override void Run() 
		{
			// just assume one attachment for simplicity sake.

			if (this.Attachments != null && this.Attachments.Length > 0) 
			{
				LoadAttachment();
			} 
			else 
			{
				GetAttachment();
			}
		}

		/// <summary>
		/// Called when the form is done.
		/// </summary>
		private void FormDone() 
		{
			/*			Moves = form.Moves;
						CurrentPlayer = form.CurrentPlayer;
						Players = form.Players;
			*/
			base.Done(this);

		}
	}
}
