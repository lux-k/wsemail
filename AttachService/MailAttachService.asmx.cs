using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;


using Microsoft.Web.Services;
using Microsoft.Web.Services.Security;

using PennLibraries;
using WSEmailProxy;
using DistributedAttachment;
using XACLPolicy;

namespace AttachService
{
	/// <summary>
	/// Summary description for Service1.
	/// </summary>
	[WebService(Namespace="http://securitylab.cis.upenn.edu/WSEmail")]
	public class MailAttachService : System.Web.Services.WebService
	{
		private bool debug = true;

		
		public MailAttachService() 
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
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

		// WEB SERVICE EXAMPLE
		// The HelloWorld() example service returns the string Hello World
		// To build, uncomment the following lines then save and build the project
		// To test this web service, press F5


		[WebMethod]
		public string HelloAttach()
		{
			return "This is Attach Service";

		}

		//[XmlInclude(typeof(WSMessageAttach))]
		[WebMethod]
		public WSEmailStatus  WSEmailSend(WSMessageAttach theMessage, XmlElement theSig)
		{
			if (theMessage == null)
				return new WSEmailStatus(500,"The sent message is null?");

			// Build a new xml document and import the signature back in (if it has been signed)
			XmlDocument x = new XmlDocument();			// Dose it work?

			if(theSig != null)
			{
				theSig = (XmlElement)HttpSoapContext.RequestContext.Envelope.ImportNode(theSig,true);
			}

			// Add some codes for log
			return processWSEmail(theMessage,theSig);
		}

		
		private WSEmailStatus processWSEmail(WSMessageAttach theMessage,XmlElement theSig)
		{
			if(debug)
			{
				//AttachUtility.Log(theMessage.ToString());
				PennLibraries.Utilities.LogEvent("ARROW");
				String s = theMessage.ToString();
				PennLibraries.Utilities.LogEvent("The message is " + s);
				PennLibraries.Utilities.LogEvent(PennLibraries.LogType.ServerDebug,"****");

				if( theMessage.Fileattachments != null)
					s = theMessage.Fileattachments.ToString();
				PennLibraries.Utilities.LogEvent("The fileattachments are " + s);
				//for(int int i=0;i<theMessage.
			}
			


			// Currently the mail is stored in Application as a memory object
			int index = Application.Count + 1;
			Application.Add(index.ToString(),theMessage);
			
			return new WSEmailStatus(200,"succeed");

			// Add the parser code to parse the 
		}

		[WebMethod]
		//Currently, this method is processed in memory. It will be substituted by Database.
		public WSEmailProxy.WSEmailHeader[]  WSEmailFetchHeaders(string recipient)
		{
			int mailcount = 0;
			System.Collections.ArrayList al = new ArrayList(); // Store the message
			ArrayList alMsgID = new ArrayList();	// Store the message ID
			WSMessageAttach wsmaTemp;
			
			for(int i = 0;i<Application.Count;i++)
			{
				if (Application[i] is DistributedAttachment.WSMessageAttach)
				{
					wsmaTemp = (WSMessageAttach)Application[i];
					for (int j = 0; j < wsmaTemp.Recipients.Count; j++)
						if ((wsmaTemp.Recipients[j]).ToString().Equals(recipient))
						{
							al.Add(wsmaTemp);
							alMsgID.Add(i);
							mailcount++;
						}
				}
			}

			WSEmailHeader[] wshs = new WSEmailHeader[mailcount];
			for(int i=0;i<mailcount;i++)
			{
				wsmaTemp = (WSMessageAttach)al[i];
				wshs[i] = new WSEmailHeader();
				wshs[i].Sender = wsmaTemp.Sender;
				wshs[i].Subject = wsmaTemp.Subject;
				wshs[i].Timestamp = wsmaTemp.Timestamp;
				wshs[i].MessageID = (int)alMsgID[i];
			}
			return wshs;

		}

		[WebMethod]
		public WSMessageAttach WSEmailRetrieve(int msgID,string vrecipient)
		{
			WSMessageAttach wsmaReturn = new WSMessageAttach();
			WSMessageAttach wsmaOrigin = (WSMessageAttach)Application[msgID];


			wsmaReturn.Sender = wsmaOrigin.Sender;
			wsmaReturn.Subject = wsmaOrigin.Subject;
			for(int i=0;i<wsmaOrigin.Recipients.Count;i++)
                wsmaReturn.Recipients.Add(wsmaOrigin.Recipients[i]);
			wsmaReturn.Timestamp = wsmaOrigin.Timestamp;
			wsmaReturn.Body = wsmaOrigin.Body;

			// Process to plicy Analysis
			XACLPolicy.Subject vsubject = new Subject();
			vsubject.uid = vrecipient;
			if (wsmaOrigin.policy == null || wsmaOrigin.Fileattachments == null)
				return wsmaReturn;
			Action.PermissionType perType = wsmaOrigin.policy.getPermission(null,vsubject,Action.OperationType.read);
			if(perType == Action.PermissionType.grant)
			{
				wsmaReturn.Fileattachments = new FileAttachments();
				FileAttachment faTemp;
				FileAttachments tempFAs = wsmaOrigin.Fileattachments;
				for(int i=0;i<tempFAs.numFa;i++)
				{
					faTemp = (FileAttachment)tempFAs.fas[i].Clone();
					faTemp.FileBase64 = null;
					wsmaReturn.Fileattachments.addFileAttachment(faTemp);
				}
			}

			return wsmaReturn;
		}
		
		[WebMethod]
		public  string WSEmailDelete()
		{
			return "The Mail is deleted";
		}

		[WebMethod]
		public string RequestFederatedToken()
		{
			return "TokenFromAttachService";
		}

		[WebMethod]
		public FileAttachment WSEmailRetrieveAttach(string token,string filename,int msgid)
		{
			if (!token.Equals("TokenFromAttachService"))
				return null;
			WSMessageAttach wsma = (WSMessageAttach)Application[msgid];
			FileAttachment returnFA=null;
			for(int i=0;i<wsma.Fileattachments.numFa;i++)
			{
				returnFA = wsma.Fileattachments.fas[i];
				if (returnFA.FileName.Equals(filename))
					break;
			}
			
			return returnFA;
		}

		[WebMethod(EnableSession=true)]
		public int setValue(string s)
		{
			//Session.Add(s,s)
			//Session.Contents.Add("test","value");
			//return Session.Count;
			Application.Add("test",s);
			return Application.Count;
		}

		[WebMethod(EnableSession=true)]
		public string getValue()
		{
			//return Session.Count.ToString();
			/*if (Session == null )
				return "null";
			else
				if (Session.Count  == 0)
					return "The count is 0";
				else
					return ((string)Session.Contents[0]);*/
			return ((string)Application["test"]);
		}
	}
}
