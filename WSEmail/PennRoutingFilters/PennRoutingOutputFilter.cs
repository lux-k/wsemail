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

namespace PennLibraries
{
	/// <summary>
	/// Provides the logic of the Penn Routing scheme going out to the wire.
	/// </summary>
	public class PennRoutingOutputFilter :  SoapOutputFilter
	{
		/// <summary>
		/// Holds the common name of the certificate to sign the message with.
		/// </summary>
		private string _signingCert = "";
		/// <summary>
		/// Specifies whether or not we should look for certificates in the machine store. Default is to not use it.
		/// </summary>
		private bool _useMachineStore = false;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public PennRoutingOutputFilter()
		{
		}


		/// <summary>
		/// Creates a new PennRoutingFilter which will sign messages with the named certificate. By default
		/// it will assume the Personal store instead of the machine store.
		/// </summary>
		/// <param name="Cert_Common_Name">String representing the CN of the certificate.</param>
		public PennRoutingOutputFilter(String Cert_Common_Name) 
		{
			SetSigningCert(Cert_Common_Name);
		}

		/// <summary>
		/// Creates a new PennRoutingFilter which will sign messages with the name certificate, but also
		/// explicitly tells the filter where to look for the certificate.
		/// </summary>
		/// <param name="Cert_Common_Name">CN of the certificate to use</param>
		/// <param name="Use_Machine_Store">True: use machine store, false: use personal store</param>
		public PennRoutingOutputFilter(String Cert_Common_Name, bool Use_Machine_Store) 
		{
			SetSigningCert(Cert_Common_Name);
			UseMachineStore=Use_Machine_Store;
		}

		/// <summary>
		/// Sets the signing Certificate to this name.
		/// </summary>
		/// <param name="Cert_Common_Name">CN of certificate</param>
		public void SetSigningCert (String Cert_Common_Name) 
		{
			_signingCert = Cert_Common_Name;
		}

		/// <summary>
		/// Sets whether or not the filter should look for certificates in the machine store.
		/// </summary>
		public bool UseMachineStore 
		{
			get { return _useMachineStore; }
			set { _useMachineStore = value; }
		}

		/// <summary>
		/// Processes the message. This will find the certificate, sign the entire message and
		/// add the certificate to the message.
		/// </summary>
		/// <param name="env">Reference to the SoapEnvelope</param>
		public override void ProcessMessage ( Microsoft.Web.Services.SoapEnvelope env ) 
		{
			PennRoutingUtilities.LogEvent(this + " : outgoing message\n"+env.OuterXml);
			return;

			//http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpref/html/frlrfsystemsecuritycryptographyxmldataobjectclasstopic.asp
			X509SecurityToken cert = PennRoutingUtilities.GetSecurityToken(_signingCert,UseMachineStore);
			// create a new root node and import it into the message
			XmlNode root = env.CreateNode(XmlNodeType.Element,"","PennSignature","http://securitylab.cis.upenn.edu/RoutingSigner/");
			env.ImportNode(root,true);
			// create another node, but use all the data in message
			XmlNode toSign = env.CreateNode(XmlNodeType.Element,"","SignMe","http://securitylab.cis.upenn.edu/RoutingSignature/");
			toSign.InnerText = env.InnerXml;
			// PennRoutingUtilities.LogEvent(this + " Signing...\nWith certificate where CN = " + _signingCert + "\nUsing machine store: " + UseMachineStore + "\nInput message:"+env.OuterXml);
			// Create the SignedXml message.
			Microsoft.Web.Services.Security.SignedXml signedXml = new Microsoft.Web.Services.Security.SignedXml();

			signedXml.SigningKey = cert.Certificate.Key;
			// Create a data object to hold the data to sign.
			signedXml.AddObject(new DataObject("SoapBody","","",(XmlElement)toSign));
			// mandatory reference
			signedXml.AddReference(new Microsoft.Web.Services.Security.Reference("#SoapBody"));
			
			KeyInfo keyInfo = new KeyInfo();

			// Include the certificate raw data with the signed xml
			keyInfo.AddClause(new KeyInfoX509Data(cert.Certificate));
			// PennRoutingUtilities.LogEvent(this + " Signed...\nWith certificate where CN = " + _signingCert + "\nUsing machine store: " + UseMachineStore+"\nValue signed:"+ toSign.OuterXml);
			// add the keyinfo
			signedXml.KeyInfo = keyInfo;

			// sign it and add it
			signedXml.ComputeSignature();
			env.Body.AppendChild(env.ImportNode(root,true));
			XmlElement toAdd = signedXml.GetXml();
			XmlElement temp = (XmlElement)env.Body.GetElementsByTagName("PennSignature")[0];

			// remove the data object. we'll check it later using the message we receive.
			toAdd.RemoveChild(toAdd.GetElementsByTagName("Object")[0]);
			temp.AppendChild(env.ImportNode(toAdd,true));
			PennRoutingUtilities.LogEvent(this + " : signed outgoing message\nWith certificate where CN = " + _signingCert + "\nUsing machine store: " + UseMachineStore + "\n"+env.OuterXml);
		}
	}
}