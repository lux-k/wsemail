using System.Text;
using DynamicBizObjects;
using DynamicForms;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Data.SqlClient;
using System.Configuration;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Threading;
using Microsoft.Web.Services;
using WSEmailProxy;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;



namespace WSEMailServer
{
	/// <summary>
	/// Implements a secure mail and instant message platform for sending and receiving messages.
	/// </summary>
	/// 
	[WebService(Namespace="http://securitylab.cis.upenn.edu/WSEmail")]
	public class MailServer : System.Web.Services.WebService
	{

		/// <summary>
		/// The name of the Server. This is a domain, ie. (MailServerA)
		/// </summary>
		public string ServerName;
		/// <summary>
		/// URL this mailserver sends mail that isn't local.
		/// </summary>
		public string MailRouter;

		/// <summary>
		/// Default constructor which loads the ServerName and Router out of the configuration file.
		/// </summary>
		///
		public string LogID = "";
		public MailServer()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
			// some real basic constructors.
			ServerName = ConfigurationSettings.AppSettings["MailServerName"];
			MailRouter = ConfigurationSettings.AppSettings["MailRouter"];
			// LogID = this + "\n" + "MailServer : " + ServerName + "\n";
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
		/// Returns a SQL connection to the MailServer's database. It assume it will read
		/// the data out of a database named "ServerName"Mail. The username/password of the connection
		/// string is also in here.
		/// </summary>
		/// <returns></returns>
		// connects to the sql server.
		// unfortunately, right now, it's got a plaintext password.
		private SqlConnection DBConnect() 
		{
			SqlConnection s = new SqlConnection("data source=xiangtan;initial catalog="+ServerName+"Mail;user id=sa;password=superbuh");
			s.Open();
			return s;
		}

		/// <summary>
		/// Gets the remote email of the user out of a certificate. If the request is a message, it will give you the email address
		/// of the person sending the message, otherwise it's the last authenticated hop.
		/// </summary>
		/// <returns></returns>
		private string GetRemoteEmail() 
		{
			if (HttpSoapContext.RequestContext["MessageCertificate"] != null)
				return PennRoutingFilters.PennRoutingUtilities.GetCertEmail((Microsoft.Web.Services.Security.X509.X509Certificate)HttpSoapContext.RequestContext["MessageCertificate"]);
			else
				return PennRoutingFilters.PennRoutingUtilities.GetCertEmail((Microsoft.Web.Services.Security.X509.X509Certificate)HttpSoapContext.RequestContext["AuthenticatingCertificate"]);
		}



		/// <summary>
		///  Receives a message to be delivered. If the server can deliver the message locally, it will do so.
		///  Otherwise it will forward the message to the mailrouter.
		/// </summary>
		/// <param name="theMessage">Message to send</param>
		/// <param name="theSig">Signature of the message</param>
		/// <returns></returns>
		[WebMethod]
		public WSEmailStatus WSEmailSend(WSEmailMessage theMessage, XmlElement theSig) 
		{
			// build a new xml document and import the signature back in (ie. it'll look original again.
			XmlDocument x = new XmlDocument();
			if (theSig != null) 
			{
				theSig=(XmlElement)HttpSoapContext.RequestContext.Envelope.ImportNode(theSig,true);
			
				// we save it here because it's a common place to look for it.
				HttpSoapContext.RequestContext["MessageSignature"] = theSig;
			}
			PennRoutingFilters.PennRoutingUtilities.LogEvent("Mail server: " + ServerName +  "\nReceived message: " + theMessage); //: " + m.ToString());
			return ProcessWSEmail(theMessage);
		}

		/// <summary>
		/// Used internally by the server. Performs some checks on the message and then looks to see where it should go.
		/// </summary>
		/// <param name="theMesage">Message to deliver</param>
		/// <returns>Status message</returns>
		private WSEmailStatus ProcessWSEmail(WSEmailMessage theMesage) {
			// if the signature on the message doesn't match then trash it.
			if (!PennRoutingFilters.PennRoutingUtilities.VerifyWSEmailMessageSignature())
				return new WSEmailStatus(500,"Message signature failed to verify. Rejecting...");

			// if the message sender doesn't equal the person who signed it then trash it
			if (GetRemoteEmail() != theMesage.Sender)
				return new WSEmailStatus(500,"Message received by " + ServerName + " and discarded. Signing certificate and from address don't match! Should be: " + GetRemoteEmail());
			
			// figure out the destination server
			string dest = theMesage.Recipient.Substring(theMesage.Recipient.IndexOf("@") + 1);

			// if its me then deliver the message
			// otherwise forward it
			if (dest.ToLower() != ServerName.ToLower())
				return ForwardMessage(theMesage);
			else
				return DeliverLocally(theMesage);
		}
	
		/// <summary>
		/// Attempts to futher categorize the message for local delivery.
		/// </summary>
		/// <param name="theMessage"></param>
		/// <returns></returns>
		private WSEmailStatus DeliverLocally(WSEmailMessage theMessage) {
			PennRoutingFilters.PennRoutingUtilities.LogEvent(LogID + "Delivering locally...");
			// if it's an im then send it as an im
			// otherwise spool it
			return DeliverMessage(theMessage);
		}

		/// <summary>
		/// This is called by the server to save the message into the database for the recipient.
		/// </summary>
		/// <param name="theMessage"></param>
		/// <returns></returns>
		private WSEmailStatus DeliverMessage(WSEmailMessage theMessage) 
		{

			//if (theMessage.BusinessRequest == null)
			//	return new WSEmailStatus(500,"Can't process an empty businessrequest.");

			string recip = theMessage.Recipient;
			PennRoutingFilters.PennRoutingUtilities.LogEvent("Recip = " + recip);
			recip = recip.Split(new char[] {'@'})[0].ToLower();
			PennRoutingFilters.PennRoutingUtilities.LogEvent("Recip = " + recip);

			BaseObject o = ObjectLoader.LoadObject(theMessage.XmlAttachments[0].OuterXml);
			BizObjectsInterface bz = (BizObjectsInterface)o;
			BusinessRequest br = bz.GetBusinessRequest();

			switch (recip) 
			{

				case "pos":
				{

					PennRoutingFilters.PennRoutingUtilities.LogEvent("Automated server starting processing on PO message.");
					br.Delegates = new string[] {"BossMan@MailServerA","Biz@MailServerA"};
					br.InsertDelegatesAtFront = true;
					br.Mappings.Add(new MappedItem("Supervisor","BossMan@MailServerA",null));
					br.Mappings.Add(new MappedItem("BusinessOffice","Biz@MailServerA",null));
					br.GenerateApproval(null,true);
					theMessage.Sender = theMessage.Recipient;
					theMessage.Recipient = br.GetNextHop;
					theMessage.XmlAttachments[0] = o.AsXmlDocument();

					MailServerProxy m = new MailServerProxy(ConfigurationSettings.AppSettings["MailRouter"]);
					theMessage.Body = "This message was automatically processed by an automation server.\r\n" + theMessage.Body;
					m.WSEmailSend(theMessage,null);

					//return new WSEmailStatus(500,"Automated form processing is disabled until new release.");
					return new WSEmailStatus(200,"Message received and processed.");
				}
				case "verifier":
				{
					VerifierProxy vp = new VerifierProxy();
					return new WSEmailStatus(200,vp.PostObject(br));
				}

			}
			return new WSEmailStatus(500,"Unknown recipient.");
		}

		/// <summary>
		/// Forwards the message to the mailrouter, for further processing.
		/// </summary>
		/// <param name="theMessage"></param>
		/// <returns></returns>
		private WSEmailStatus ForwardMessage(WSEmailMessage theMessage) 
		{
			PennRoutingFilters.PennRoutingUtilities.LogEvent("Forwarding message through mailrouter: " + MailRouter);
			MailServerProxy m = new MailServerProxy(MailRouter);
			WSEmailStatus res = m.WSEmailSend(theMessage,(XmlElement)HttpSoapContext.RequestContext["MessageSignature"]);
			PennRoutingFilters.PennRoutingUtilities.LogEvent("Forwarding message through mailrouter: " + MailRouter + "... complete!");
			return res;
		}
	}
}
