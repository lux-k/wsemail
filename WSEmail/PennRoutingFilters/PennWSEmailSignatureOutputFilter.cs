using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using Microsoft.Web.Services;
using Microsoft.Web.Services.Security;
using Microsoft.Web.Services.Security.X509;
using System.Xml;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
//using Interop.CAPICOM;
//using System.Runtime.InteropServices;


namespace PennLibraries
{
	/// <summary>
	/// An output filter that takes care of signing new messages. This filter only works when the webmethod called is
	/// WSEmailSend. It has null functionality on every other call.
	/// </summary>
	public class PennWSEmailSignatureOutputFilter :  SoapOutputFilter
	{

		/// <summary>
		/// The CN of the signing certificate
		/// </summary>
		private String _signingCert = "";
		/// <summary>
		/// Whether or not messages should be signed (default: false)
		/// </summary>
		private bool _signMessage = false;
		/// <summary>
		/// Whether or not to use the machine store (default: false)
		/// </summary>
		private bool _useMachineStore = false;

		/// <summary>
		/// Default constructor that does nothing.
		/// </summary>
		public PennWSEmailSignatureOutputFilter()
		{
		}

		/// <summary>
		/// Creates a new filter that will use the given cert CN to sign with.
		/// </summary>
		/// <param name="Cert_Common_Name">Cert CN</param>
		public PennWSEmailSignatureOutputFilter(String Cert_Common_Name) 
		{
			SetSigningCert(Cert_Common_Name);
		}


		/// <summary>
		/// Creates a new filter that will use the given cert CN and explicitly defines
		/// whether or not to sign messages.
		/// </summary>
		/// <param name="Cert_Common_Name">Cert CN</param>
		/// <param name="Sign_Email_Message">Whether or not to sign messages</param>
		public PennWSEmailSignatureOutputFilter(String Cert_Common_Name, bool Sign_Email_Message) 
		{
			SetSigningCert(Cert_Common_Name);
			SignMessage=Sign_Email_Message;
		}
		public PennWSEmailSignatureOutputFilter(String Cert_Common_Name, bool Sign_Email_Message, bool Use_Machine_Store) 
		{
			SetSigningCert(Cert_Common_Name);
			SignMessage=Sign_Email_Message;
			PennRoutingUtilities.LogEvent("Setting usemachinestore to: " + Use_Machine_Store.ToString());
			UseMachineStore = Use_Machine_Store;
											   
		}

		/// <summary>
		/// Sets the signing certificate CN.
		/// </summary>
		/// <param name="Cert_Common_Name">Cert CN</param>
		public void SetSigningCert (String Cert_Common_Name) 
		{
			_signingCert = Cert_Common_Name;
		}

		/// <summary>
		/// Sets whether or not to use the machine store.
		/// </summary>
		public bool UseMachineStore 
		{
			get { return _useMachineStore; }
			set { _useMachineStore = value; }
		}

		/// <summary>
		/// Sets whether or not to sign messages.
		/// </summary>
		public bool SignMessage 
		{
			get { return _signMessage; }
			set { _signMessage = value; }
		}


		/// <summary>
		/// Processes the message. It will only function if the web action is WSEmailSend and there is
		/// an unsigned message.
		/// </summary>
		/// <param name="env">SoapEnvelope handle</param>
		public override void ProcessMessage ( Microsoft.Web.Services.SoapEnvelope env ) 
		{
			return;
			// first, get the action of the message and see if it
			// it wsemail send
			XmlNodeList theList = env.Header.GetElementsByTagName("wsrp:action");
			XmlElement theAction = null;
			if (theList.Count > 0)
				theAction = (XmlElement)theList[0];
			//PennRoutingFilters.PennRoutingUtilities.LogEvent("theAction's inner text = " + theAction.InnerText);
			if (theAction != null && theAction.InnerText.Equals("http://securitylab.cis.upenn.edu/WSEmail/WSEmailSend") && SignMessage) {
				// if it is, we can assume we are trying to send a message.
				// so, we will sign the message.

				// load the cert
				X509SecurityToken cert = PennRoutingUtilities.GetSecurityToken(_signingCert,UseMachineStore);

				XmlNodeList xnl = env.Body.GetElementsByTagName("theSig");
				XmlElement parent;
				if (xnl.Count > 0) 
				{
					parent = (XmlElement)xnl[0];
					if (!parent.InnerText.Equals("")) 
						return;
				}
				else 
				{
					parent = (XmlElement)env.CreateNode(XmlNodeType.Element,"theSig","http://securitylab.cis.upenn.edu/WSEmail");
					env.ImportNode(parent,true);
					XmlNodeList xnl2 = env.Body.GetElementsByTagName("WSEmailSend");
					((XmlElement)(xnl2[0])).AppendChild(parent);
				}

				// and create a root xml element
				XmlNode root = env.CreateNode(XmlNodeType.Element,"","MessageSignature","http://securitylab.cis.upenn.edu/RoutingSigner/");
				env.ImportNode(root,true);
				XmlNode toSign = env.CreateNode(XmlNodeType.Element,"","SignMe","http://securitylab.cis.upenn.edu/RoutingSignature/");
				toSign.InnerText = env.Body.GetElementsByTagName("theMessage")[0].InnerXml;
				PennRoutingUtilities.LogEvent(this + " activated : Request is to send a message and no message signature is present.");

				// Create the SignedXml message.
				Microsoft.Web.Services.Security.SignedXml signedXml = new Microsoft.Web.Services.Security.SignedXml();

				signedXml.SigningKey = cert.Certificate.Key;
				// Create a data object to hold the data to sign.
				signedXml.AddObject(new DataObject("MessageSignature","","",(XmlElement)toSign));
				signedXml.AddReference(new Microsoft.Web.Services.Security.Reference("#MessageSignature"));
				
				KeyInfo keyInfo = new KeyInfo();

				// Include the certificate raw data with the signed xml
				keyInfo.AddClause(new KeyInfoX509Data(cert.Certificate));
				PennRoutingUtilities.LogEvent(this + " : signed message.\nWith certificate where CN = " + _signingCert + "\nUsing machine store: " + UseMachineStore+"\n"+ toSign.OuterXml);
				signedXml.KeyInfo = keyInfo;

				signedXml.ComputeSignature();
				//PennRoutingUtilities.LogEvent(root.OuterXml);
				//env.Body.AppendChild(env.ImportNode(root,true));
				XmlElement toAdd = signedXml.GetXml();
				// XmlElement temp = (XmlElement)env.Body.GetElementsByTagName("MessageSignature")[0];

				toAdd.RemoveChild(toAdd.GetElementsByTagName("Object")[0]);
				root.AppendChild(env.ImportNode(toAdd,true));
				/// here root is the entire signature.
				/// so what we want to do is take it and put it in the signature element, if necessary
				
				/*
				 * Add the signature to the right place in the requesting object.
				 */
				
				XmlElement spotToAdd=(XmlElement)env.Body.GetElementsByTagName("theSig")[0];
				spotToAdd.AppendChild(root);
				//spotToAdd.InnerText=root.OuterXml;
				PennRoutingUtilities.LogEvent(this + " : done.\n\n"+env.OuterXml);
			}
		}
	}
}