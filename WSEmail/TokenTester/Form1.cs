using System;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Web.Services;
using Microsoft.Web.Services.Trust;
using Microsoft.Web.Services.Security;
using Microsoft.Web.Services.Security.X509;
using Microsoft.Web.Services.Security.SecureConversation;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;

namespace TokenTester
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(80, 104);
			this.button1.Name = "button1";
			this.button1.TabIndex = 0;
			this.button1.Text = "button1";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.button1});
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		public string AttachmentName = null;
		public string AttachmentContent = null;
		public XmlNode FederatedToken = null;

		private void button1_Click(object sender, System.EventArgs e)
		{
			SecurityTokenServiceClient client = new SecurityTokenServiceClient(new Uri( "http://tower.cis.upenn.edu/TokenIssuer/Issuer.ashx" ));
			X509SecurityToken token = GetSecurityToken("gBfo0147lM6cKnTbbMSuMVvmFY4=");		
			//					SecurityTokenCache.GlobalCache.Add( token );
			client.RequestSoapContext.Security.Tokens.Add( token );
			client.RequestSoapContext.Security.Elements.Add( new Signature( token ) );
			RequestSecurityTokenResponse response = client.RequestSecurityToken(new RequestSecurityToken( token ));
			MessageBox.Show(response.RequestedSecurityToken.SecurityToken.GetXml(new XmlDocument()).OuterXml);

			XmlSerializer xs = new XmlSerializer(typeof(Microsoft.Web.Services.Security.SecurityToken));
			MemoryStream ms = new MemoryStream();

			xs.Serialize(ms,response.RequestedSecurityToken.SecurityToken);
			ms.Position = 0;

			
			MessageBox.Show(Convert.ToString(ms.ToArray()));


			return;

			if (AttachmentContent == null) 
			{
				//MessageBox.Show("No message is attached, assuming first run.");
				OpenFileDialog f = new OpenFileDialog();
				f.Title = "Select which file you want to attach...";
				f.Filter = "All files (*.*)|*.*";
				f.FilterIndex = 0;

				System.Windows.Forms.DialogResult r = f.ShowDialog();

				if (r == DialogResult.OK) 
				{
					this.AttachmentName = f.FileName;

					StreamReader sr = new StreamReader(f.FileName);
					this.AttachmentContent = Convert.ToBase64String( System.Text.ASCIIEncoding.ASCII.GetBytes(sr.ReadToEnd()));


					/*
					 * 
						 <policy>
	  <send>
		<cache name="..\..\policyCache.xml" />
	  </send>
	</policy>
*/
					// string secureConvEndpoint = "http://tower.cis.upenn.edu/TokenValidater/Validater.asmx";
					string secureConvEndpoint = "http://tower.cis.upenn.edu/TokenIssuer/Issuer.ashx";
					

					// Sign the token request

					// Request the token, use the signing token as the Base
					//					try 
					//					{
					//						RequestSecurityToken rst = new RequestSecurityToken( token );
					//						RequestSecurityTokenResponse response = client.RequestSecurityToken(rst);
					//this.FederatedToken = response.RequestedSecurityToken.SecurityToken.GetXml(new XmlDocument());
					//						MessageBox.Show("I worked.");
					//					} 
					//					catch (SecurityFault s) 
					//					{
					//						MessageBox.Show(s.Message);
					//					}
					// Add the signature element to a security section on the request
					// to sign and encrypt the request with the security context token we just received
					
					//					requestContext.Security.Tokens.Add( response.RequestedSecurityToken.SecurityToken );
					//					requestContext.Security.Elements.Add( new Signature( response.RequestedSecurityToken.SecurityToken ) );
					// requestContext.Security.Elements.Add( new EncryptedData( response.RequestedSecurityToken.SecurityToken ) );
					
				
				}

			}
		}
		/*			form = new FrmDemo();
						form.Moves = this.Moves;
						form.Players = this.Players;
						form.CurrentPlayer = this.CurrentPlayer;
						form.Run();
						form.UserDone += new FrmDemo.NullDelegate(FormDone);
			*/
		/*		private void FormDone() 
					{
						Moves = form.Moves;
						CurrentPlayer = form.CurrentPlayer;
						Players = form.Players;
						base.Done(this);
					}
			*/
		public static X509SecurityToken GetSecurityToken(string ClientBase64KeyId)
		{	
			X509SecurityToken token = null;

			// Open the CurrentUser Certificate Store
			X509CertificateStore store;
			store = X509CertificateStore.CurrentUserStore( X509CertificateStore.MyStore );
			store.OpenRead();
			X509CertificateCollection certs = store.FindCertificateByKeyIdentifier( Convert.FromBase64String( ClientBase64KeyId ) );

			if (certs.Count > 0)
			{
				// Get the first certificate in the collection
				token = new X509SecurityToken( ((X509Certificate) certs[0]) );
			}
			store.Close();
			return token;
		}

		public static X509SecurityToken GetSecurityToken(string certKeyID, bool MachineStore)
		{            
			X509SecurityToken securityToken;  
			//
			// open the current user's certificate store
			//
			X509CertificateStore store;
			//LogEvent("Fetching cert with CN = " + certKeyID + ", Machinestore: " + MachineStore.ToString());
			if (!MachineStore) 
				store = X509CertificateStore.CurrentUserStore(X509CertificateStore.MyStore);
			else
				store = X509CertificateStore.LocalMachineStore(X509CertificateStore.MyStore);

			bool open = store.OpenRead();

			try 
			{
				// try to find the certificate.
				Microsoft.Web.Services.Security.X509.X509Certificate cert = null;
				X509CertificateCollection matchingCerts = store.FindCertificateBySubjectString(certKeyID);

				if (matchingCerts.Count == 0)
				{
					throw new ApplicationException("No matching certificates were found for the key ID provided in " + store.Location.ToString()+" store. Please run the configuration utility to guide you through client and server setup.");
				}
				else
				{
					//					LogEvent("Multiple certificates found for CN = " + certKeyID + ". ("+matchingCerts.Count.ToString() + ")");
					// pick the first one arbitrarily
					foreach (Microsoft.Web.Services.Security.X509.X509Certificate c in matchingCerts) 
					{
						//						if (GetCertCN(c).Equals(certKeyID))
						cert = c;
					}
				}
				
				if (cert == null) 
				{
					//	throw new ApplicationException("You chose not to select an X509 certificate for signing your messages.");
					//}
					Console.WriteLine("No cert selected.");
					return null;
				}
					// we need the digital sign and a private key.
				else if (!cert.SupportsDigitalSignature || cert.Key == null ) 
				{
					throw new ApplicationException("The certificate must support digital signatures and have a private key available.");
				}
				else 
				{
					byte[] keyId = cert.GetKeyIdentifier();
					Console.WriteLine("Key Name                       : {0}", cert.GetName());
					Console.WriteLine("Key ID of Certificate selected : {0}", Convert.ToBase64String(keyId));
					securityToken = new X509SecurityToken(cert);
				}
			} 
			finally 
			{
				if (store != null) { store.Close(); }
			}            

			return securityToken;            
		}
	}
}

