/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Web.Services;
using System.Xml;
using Microsoft.Web.Services2;
using Microsoft.Web.Services2.Security;
using Microsoft.Web.Services2.Security.Tokens;
using Microsoft.Web.Services2.Security.X509;
using WSEmailProxy;
using System.ComponentModel;
using FederatedBinaryToken;
using MailServerInterfaces;

namespace WSEmailServer
{
	/// <summary>
	/// Implements a secure messaging platform for sending and receiving messages.
	/// </summary>
	/// 
	[WebService(Namespace="http://securitylab.cis.upenn.edu/WSEmail")]
	public class MailServer : System.Web.Services.WebService
	{

		/// <summary>
		/// Returns true if the user sending a message is a locally authenticated user, false otherwise.
		/// </summary>
		public bool IsLocalSender 
		{
			get 
			{
				if (!(AuthenticatingTokenType == AuthenticatingTokenEnum.FederatedIdentity) &&
					(UserID != null && UserID.ToLower().EndsWith("@"+Global.ServerConfiguration.Name.ToLower())))
					return true;
				else
					return false;
			}
		}

		/// <summary>
		/// Returns the UserID (user@servername) of the currently authenticated user.
		/// </summary>
		public string UserID 
		{
			get 
			{
				return _uid;
			}
			set 
			{
				_uid = value;
			}
		}

		/// <summary>
		/// UID of the authenticated user for this connection
		/// </summary>
		private string _uid = null;

		private SecurityToken AuthenticatingToken = null;
		private MailServerInterfaces.AuthenticatingTokenEnum AuthenticatingTokenType = AuthenticatingTokenEnum.Unknown;
		/// <summary>
		/// Logs a message to the logfile.
		/// </summary>
		/// <param name="s"></param>
		private void Log(string s) 
		{
			Global.ServerConfiguration.Log(MailServerLogType.ServerDebug,this + " : " + s);
		}

		/// <summary>
		/// The default constructor. Verifies that that the request is unique and has a timestamp, that all requests
		/// to send email are authenticated locally, unless it's being relayed, and that the request is signed. It then
		/// adds the server's certificate to the response tokens.
		/// </summary>
		public MailServer()
		{
			InitializeComponent();

			if (RequestSoapContext.Current == null)
				Log("There is no request context. It's extremely likely that you do not have the WSE 2.0 registered in the web.config file.");
			else 
			{
				if (RequestSoapContext.Current.Addressing == null)
					Log("H.ReqCon.Addressing is null!");
				else 
				{
					if (RequestSoapContext.Current.Addressing.MessageID == null) 
						Log("The addressing messageID is null. Each and every message received by the WSEmail server is expected to contain a messageID.");
					else 
					{
						Log("Request ID is " + RequestSoapContext.Current.Addressing.MessageID.Value.ToString());
						if (RequestSoapContext.Current.Security.Timestamp != null) 
							Global.ServerConfiguration.RegisterRequest(RequestSoapContext.Current.Addressing.MessageID.Value.ToString(),RequestSoapContext.Current.Security.Timestamp.Expires);
						else
							throw new Exception("A timestamp is required on all requests.");
					}
				}
			}

			if (RequestSoapContext.Current.Security.Tokens.Count == 0)
				throw new Exception("You must give at least 1 security token");

			if (RequestSoapContext.Current.Security.Elements.Count == 0)
				throw new Exception("Local user did not attach any security elements (no signatures)");

			AuthenticatingToken = VerifySignatures();

			if (AuthenticatingToken == null)
				throw new Exception("We require that the client sign the Soap Body, routing path and timestamp elements in requests.");

			ParseToken(AuthenticatingToken);
			
			if (!RequestSoapContext.Current.Envelope.Body.FirstChild.Name.Equals("WSEmailSend") &&
				!RequestSoapContext.Current.Envelope.Body.FirstChild.Name.Equals("ExecuteExtensionHandler") &&
				!IsLocalSender)
				throw new Exception("You must authenticate with a local WSEmail account to access that function.");

			ResponseSoapContext.Current.Security.Tokens.Add(Global.ServerConfiguration.X509Token);
			ResponseSoapContext.Current.Security.Elements.Add(new MessageSignature(Global.ServerConfiguration.X509Token));
		}

		/// <summary>
		/// Verifies that the required signatures (SoapBody, Routing and Timestamp) are on the message and
		/// returns the security token (if any) that did the signatures.
		/// </summary>
		/// <returns></returns>
		private SecurityToken VerifySignatures() 
		{
			SecurityToken s = null;
			foreach (Object o in RequestSoapContext.Current.Security.Elements) 
			{
				if (o is MessageSignature) 
				{
					MessageSignature sig = (MessageSignature)o;
					if ((sig.SignatureOptions & SignatureOptions.IncludeSoapBody) == SignatureOptions.IncludeSoapBody) 
					{
						if ((sig.SignatureOptions & SignatureOptions.IncludeAddressing) == SignatureOptions.IncludeAddressing
							|| (
							((sig.SignatureOptions & SignatureOptions.IncludeAction) == SignatureOptions.IncludeAction) &&
							((sig.SignatureOptions & SignatureOptions.IncludeMessageId) == SignatureOptions.IncludeMessageId) &&
							((sig.SignatureOptions & SignatureOptions.IncludeTo) == SignatureOptions.IncludeTo)
							)) 
						{
							if ((sig.SignatureOptions & SignatureOptions.IncludeTimestamp) == SignatureOptions.IncludeTimestamp) 
							{
								s = sig.SigningToken;
								break;
							} 
							else
								Log("Could not find timestamp signature option.");
						} 
						Log("Could not find path signature.");
					} 
					else
						Log("Could not find soap body signature.");
				}
			}
			return s;
		}

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// MailServer
			// 
			this.Disposed += new System.EventHandler(this.MailServer_Disposed);

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		/// <summary>
		/// Sets the UserID and AuthenticatingTokenType of the request.
		/// </summary>
		/// <returns></returns>
		private void ParseToken(SecurityToken s) 
		{
			
				if (s is UsernameToken) 
				{
					string user = Global.ServerConfiguration.DataAccessor.ReadUserName(((UsernameToken)s).Username);
					_uid = user + "@" + Global.ServerConfiguration.Name;
					AuthenticatingTokenType = AuthenticatingTokenEnum.UsernamePassword;
				}
				else if (s is X509SecurityToken) 
				{
					_uid = PennLibraries.Utilities.GetCertEmail(((X509SecurityToken)s).Certificate);
					AuthenticatingTokenType = AuthenticatingTokenEnum.X509Certificate;
				}
				else if (s is FederatedToken) 
				{
					_uid = ((FederatedToken)s).IdentityToken.UserID;
					AuthenticatingTokenType = AuthenticatingTokenEnum.FederatedIdentity;
				}
		}

		/// <summary>
		/// Executes an extension handler with the given extension name and arguments.
		/// </summary>
		[WebMethod]
		public XmlElement ExecuteExtensionHandler(string ext, XmlElement args) 
		{
			return Global.ServerConfiguration.RunExtensionHandler(ext,args,this.GetProcessingEnvironment());
		}
		/// <summary>
		/// Allows a user to retrieve the headers of all the mail in their box.
		/// Think of it as the webservices equivalent of a POP3 list command.
		/// </summary>
		[WebMethod]
		public WSEmailHeader[] WSEmailFetchHeaders(DateTime messagesSince) 
		{
			IMessageAccess accessor = Global.ServerConfiguration.MessageAccessor.CreateInstance(this.UserID);
			WSEmailHeader[] h = accessor.FetchHeaders(messagesSince);
			accessor.Dispose();
			return h;
		}

		/// <summary>
		///  Receives a message to be delivered. If the server can deliver the message locally, it will do so.
		///  Otherwise it will forward the message to the mailrouter.
		/// </summary>
		/// <param name="theMessage">Message to send</param>
		/// <param name="theSig">Signature of the message</param>
		/// <returns></returns>
		/// 

		[WebMethod]
		public WSEmailStatus WSEmailSend(WSEmailMessage theMessage, XmlElement theSig) 
		{
			if (theMessage == null)
				return new WSEmailStatus(500,"No message sent?");

			if (theMessage.Recipients.Count == 0)
				return new WSEmailStatus(500,"No recipients?");

			Log("Mail server: " + Global.ServerConfiguration.Name +  "\nReceived message: " + theMessage); //: " + m.ToString());
			return ProcessWSEmail(theMessage,theSig);
		}

		/// <summary>
		/// Allows an enduser to delete one of their messages.
		/// </summary>
		/// <param name="messageToDelete">ID number of the message to delete.</param>
		/// <returns>Status message</returns>
		[WebMethod]
		public WSEmailStatus WSEmailDelete(int[] messagesToDelete) 
		{
			if (messagesToDelete != null && messagesToDelete.Length > 0) 
			{
				IMessageAccess a = Global.ServerConfiguration.MessageAccessor.CreateInstance(this.UserID);
				a.DeleteMessage(messagesToDelete);
				a.Dispose();
			}
			return null;
		}

		/// <summary>
		/// Allows an enduser to receive one of their messages.
		/// </summary>
		/// <param name="messageToGet">ID number of the message to get.</param>
		/// <returns>A package containing the Message ID, signature and message</returns>
		[WebMethod]
		public WSEmailPackage WSEmailRetrieve(int messageToGet) 
		{
			if (messageToGet <= 0)
				return null;

			WSEmailPackage p = Global.ServerConfiguration.MessageAccessor.CreateInstance(this.UserID).RetrieveMessage(messageToGet);

			if ((p.theMessage.MessageFlags & WSEmailFlags.Precedence.EncryptedDelivery) ==
				WSEmailFlags.Precedence.EncryptedDelivery) 
			{
				RequestSoapContext.Current.Security.Tokens.Add(this.AuthenticatingToken);
				EncryptedData enc = new EncryptedData(this.AuthenticatingToken);
				RequestSoapContext.Current.Security.Elements.Add(enc);
			}
			return p;
		}

		/// <summary>
		/// Gets the processing environment so that plugins can get access to such things as SoapRequest environments and such.
		/// </summary>
		/// <returns></returns>
		private MailServerInterfaces.ProcessingEnvironment GetProcessingEnvironment() 
		{

			return new MailServerInterfaces.ProcessingEnvironment(Server, RequestSoapContext.Current,
				ResponseSoapContext.Current, this.UserID,this.IsLocalSender, this.AuthenticatingTokenType);
		}

		/// <summary>
		/// Used internally by the server. Performs some checks on the message and then looks to see where it should go.
		/// </summary>
		/// <param name="theMesage">Message to deliver</param>
		/// <returns>Status message</returns>
		private WSEmailStatus ProcessWSEmail(WSEmailMessage theMessage, XmlElement theSig) 
		{
			// if the signature on the message doesn't match then trash it.
			Log("Sending userID = " + this.UserID);
			Log("Token type = " + this.AuthenticatingTokenType.ToString());
			Log("Is local? " + this.IsLocalSender.ToString());
			
			if (IsLocalSender || AuthenticatingTokenType == AuthenticatingTokenEnum.FederatedIdentity) 
			{
				Global.ServerConfiguration.Stats.Received.Internal.MessageCount++;
				Global.ServerConfiguration.Stats.Received.Internal.ByteCount += this.Context.Request.TotalBytes;

				Global.ServerConfiguration.RunSendingPlugins(theMessage,theSig,GetProcessingEnvironment());
				theMessage.Sender = this.UserID;
				theSig = theMessage.Sign(Global.ServerConfiguration.WSE1SecurityToken);
			} 
			else 
			{
				Global.ServerConfiguration.Stats.Received.External.MessageCount++;
				Global.ServerConfiguration.Stats.Received.External.ByteCount += this.Context.Request.TotalBytes;

				// we didn't sign it, so verify it.
				string domain = theMessage.Sender.Substring(theMessage.Sender.IndexOf("@")).ToLower();
				X509Certificate cert = theMessage.VerifyReturningWSE2Cert(theSig);//PennLibraries.Utilities.VerifyLocalWSEmailMessageSignature(RequestSoapContext.Current.Envelope,theSig);
				
				if (cert == null)
					return new WSEmailStatus(577,"Message signature doesn't match or is missing; authenticity failure.");
				
				string signer = PennLibraries.Utilities.GetCertEmail(cert).ToLower();

				if (!(signer.Equals("postmaster"+domain) ||
					signer.Equals("postmaster@" + Global.ServerConfiguration.Name.ToLower())))
					return new WSEmailStatus(577,"Message is not signed by correct sending domain (postmaster"+domain + " vs " + PennLibraries.Utilities.GetCertEmail(cert) + ")" );
			}

			// now.. users can't sign their own messages, so either the signature on the message must be
			// from the user's sending domain or this local domain (federated) anything else doesn't make
			// sense

			WSEmailStatus res = Global.ServerConfiguration.DeliveryQueue.Enqueue(theMessage,theSig,this.AuthenticatingTokenType);
			return res;
		}

		/// <summary>
		/// Cleans up resources when a particular instance of the mail server is thrown out.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MailServer_Disposed(object sender, System.EventArgs e)
		{
		}
	}
}
