using System;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using DynamicForms;
using Microsoft.Web.Services.Trust;
using Microsoft.Web.Services.Security;
using Microsoft.Web.Services.Security.X509;
using Microsoft.Web.Services.Security.SecureConversation;

namespace FederatedAttachment
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Launcher : BaseObject
	{
		public string AttachmentName = null;
		public string AttachmentContent = null;
		public XmlNode FederatedToken = null;

//		private FrmDemo form = null;

		public Launcher()
		{
			InitializeDynamicConfiguration();
		}

		public override string DebugToScreen() 
		{
			return "I am a formlauncher.";
		}

		public override void Destroy() 
		{
//			form.ThrowOut();
//			form = null;
		}

		public override void InitializeDynamicConfiguration() 
		{
			this.LockEmail = false;
			this.Configuration.DLL = "FederatedAttachment";
			this.Configuration.Name = "FederatedAttachment.Launcher";
			this.Configuration.Url = "http://tower.cis.upenn.edu/classes/FederatedAttachment.dll";
			this.Configuration.Version = 1.0F;
			this.Configuration.FriendlyName = "Federated Attachment";
			this.Configuration.Description = "Verifies trust on attachments";
		}

		public override void Run() 
		{
			if (AttachmentContent == null) 
			{
				MessageBox.Show("No message is attached, assuming first run.");
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


					string secureConvEndpoint = "http://tower.cis.upenn.edu/TokenIssuer/Issuer.ashx";
					SecurityTokenServiceClient client = new SecurityTokenServiceClient(new Uri( secureConvEndpoint ));

					// Sign the token request
					X509SecurityToken token = GetSecurityToken("KevinA",false);						
					client.RequestSoapContext.Security.Tokens.Add( token );
					client.RequestSoapContext.Security.Elements.Add( new Signature( token ) );

					// Request the token, use the signing token as the Base
					RequestSecurityTokenResponse response = client.RequestSecurityToken( new RequestSecurityToken( token ) );

					// Add the signature element to a security section on the request
					// to sign and encrypt the request with the security context token we just received
					this.FederatedToken = response.RequestedSecurityToken.SecurityToken.GetXml(new XmlDocument());
//					requestContext.Security.Tokens.Add( response.RequestedSecurityToken.SecurityToken );
//					requestContext.Security.Elements.Add( new Signature( response.RequestedSecurityToken.SecurityToken ) );
					// requestContext.Security.Elements.Add( new EncryptedData( response.RequestedSecurityToken.SecurityToken ) );
					base.Done(this);
				
				}

			}

/*			form = new FrmDemo();
			form.Moves = this.Moves;
			form.Players = this.Players;
			form.CurrentPlayer = this.CurrentPlayer;
			form.Run();
			form.UserDone += new FrmDemo.NullDelegate(FormDone);
*/
		}

/*		private void FormDone() 
		{
			Moves = form.Moves;
			CurrentPlayer = form.CurrentPlayer;
			Players = form.Players;
			base.Done(this);
		}
*/
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
