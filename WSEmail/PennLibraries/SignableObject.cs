/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System.Security.Cryptography.Xml;
using System.Configuration;
using System;
using Microsoft.Web.Services2.Security.Tokens;
using Microsoft.Web.Services.Security;
using Microsoft.Web.Services.Security.X509;
using System.Security.Cryptography;
using System.Xml;

namespace PennLibraries
{
	/// <summary>
	/// Summary description for SignableObject.
	/// </summary>
	public abstract class SignableObject : SerializableObject
	{
		[System.Xml.Serialization.XmlIgnore()]
		public string SerializationUri = "";
		
		/// <summary>
		/// Initializes the Serialization URI.
		/// </summary>
		public SignableObject() 
		{
			this.SerializationUri = "http://securitylab.cis.upenn.edu/"+this.GetType().Name;
		}
	
		/// <summary>
		/// Signs the object using a WSE v2 x509 security token.
		/// </summary>
		/// <param name="ct">X509 security token (WSE v2)</param>
		/// <returns>XmlElement representing the XML DSIG signature of the object</returns>
		public XmlElement Sign(Microsoft.Web.Services2.Security.Tokens.X509SecurityToken ct) 
		{
			Microsoft.Web.Services.Security.X509SecurityToken cert =
				new Microsoft.Web.Services.Security.X509SecurityToken(new Microsoft.Web.Services.Security.X509.X509Certificate(ct.Certificate.GetRawCertData()));
			return Sign(cert);
		}

		/// <summary>
		/// Signs the object using a WSE v2 x509 certificate.
		/// </summary>
		/// <param name="ct">X509 certificate (WSE v2)</param>
		/// <returns>XmlElement representing the XML DSIG signature of the object</returns>
		public XmlElement Sign(Microsoft.Web.Services2.Security.X509.X509Certificate ct) 
		{
			Microsoft.Web.Services.Security.X509SecurityToken cert = new Microsoft.Web.Services.Security.X509SecurityToken(new Microsoft.Web.Services.Security.X509.X509Certificate(ct.GetRawCertData()));
			return Sign(cert);
		}

		/// <summary>
		/// Signs the object using a WSE v1 x509 security token.
		/// </summary>
		/// <param name="ct">X509 certificate (WSE v1)</param>
		/// <returns>XmlElement representing the XML DSIG signature of the object</returns>
		public XmlElement Sign(Microsoft.Web.Services.Security.X509.X509Certificate ct) 
		{
			return Sign(new Microsoft.Web.Services.Security.X509SecurityToken(ct));
		}

		/// <summary>
		/// Signs the object using a WSE v1 x509 security token.
		/// </summary>
		/// <param name="ct">X509 security token (WSE v1)</param>
		/// <returns>XmlElement representing the XML DSIG signature of the object</returns>
		public XmlElement Sign(Microsoft.Web.Services.Security.X509SecurityToken cert) 
		{
			XmlDocument d = this.SerializeToXmlDocument();
			
			XmlNode toSign = d.CreateNode(XmlNodeType.Element,"",this.GetType().Name+"Container",SerializationUri);
			LogEvent("OuterXml = " + d.OuterXml);

			XmlNode ww = d.ImportNode(d.FirstChild,true);
			toSign.AppendChild(ww);

			Utilities.LogEvent(this + " : signed message.\nWith certificate where CN = " + cert.Certificate + "\n"+ toSign.OuterXml);
			// Create the SignedXml message.
			Microsoft.Web.Services.Security.SignedXml signedXml = new Microsoft.Web.Services.Security.SignedXml();
			signedXml.SigningKey = cert.Certificate.Key;
			// Create a data object to hold the data to sign.
			signedXml.AddObject(new System.Security.Cryptography.Xml.DataObject(this.GetType().Name,"","",(XmlElement)toSign));
			signedXml.AddReference(new Microsoft.Web.Services.Security.Reference("#"+this.GetType().Name));
				
			System.Security.Cryptography.Xml.KeyInfo keyInfo = new System.Security.Cryptography.Xml.KeyInfo();

			// Include the certificate raw data with the signed xml
			keyInfo.AddClause(new System.Security.Cryptography.Xml.KeyInfoX509Data(cert.Certificate));
			//TODO take me out.
			signedXml.KeyInfo = keyInfo;

			signedXml.ComputeSignature();
			Utilities.LogEvent(PennLibraries.LogType.Debug,"Sign() = " + signedXml.GetXml().OuterXml);
			XmlElement toAdd = signedXml.GetXml();
			toAdd.RemoveChild(toAdd.GetElementsByTagName("Object")[0]);
			//Verify((XmlElement)toAdd); //(for debugging)
			return (XmlElement)toAdd;
		}


		/// <summary>
		/// Verifies a signature by serializing the current object to XML and comparing to the given signature.
		/// </summary>
		/// <param name="xe">XmlDSIG signature in the form of XML element</param>
		/// <returns>True if good, bad otherwise</returns>
		public bool Verify(XmlElement xe) 
		{
			if (xe == null)
				throw new Exception(this + ".Verify() can not verify a null signature.");

			string res = this.VerifyReturningBase64Cert(xe);
			if (res == null || res.Equals("")) 
			{
				Utilities.LogEvent(LogType.MessageAuthenticationError,"VerifySignature() : is done, but unable to verify signature!");
				return false;
			}
			else 
			{

				Utilities.LogEvent(LogType.Debug,"VerifySignature() : is done!");
				return true;
			}
		}

		/// <summary>
		/// Verifies a signature by serializing the current object to XML and comparing to the given signature.
		/// </summary>
		/// <param name="ct">XmlDSIG signature in the form of XML element</param>
		/// <returns>Base64 encoding of the certificate that generated the certificate, null if the signature is invalid.</returns>
		public string VerifyReturningBase64Cert(XmlElement xe) 
		{
			Utilities.LogEvent(LogType.Debug,"VerifySignature() : Verifying signature...\nMy identity is CN = " + ConfigurationSettings.AppSettings["SigningCertificate"] + "\nInput: "+xe.OuterXml+"\n"+"Onward to verifying...");
			// get a handle to the penn signature.
			// and the signature element within that.

			// recreate the original signme node (this will be used in the signature verification.
			XmlDocument d = new XmlDocument();
			XmlElement toSend = (XmlElement)xe.Clone();

			XmlElement toCheck = (XmlElement)d.CreateNode(XmlNodeType.Element,"",this.GetType().Name+"Container",SerializationUri);
			
			XmlNode ww = d.ImportNode(this.SerializeToXmlDocument().FirstChild,true);
			toCheck.AppendChild(ww);
			
			DataObject dO = new DataObject(this.GetType().Name,"","",(XmlElement)toCheck);

			// since tosend is a clone of the penn signature,
			// find the signature element within that,
			// append the data object
			XmlNode n = d.ImportNode(toSend,true);
			d.AppendChild(n);
			n.AppendChild(d.ImportNode(dO.GetXml(),true));

			//(toSend.GetElementsByTagName("Signature")[0]).AppendChild(env.ImportNode(dO.GetXml(),true));

			// at this point, the toSend xml has been recreated as it was on the routing output filter.
			//LogEvent("Sending to VerifyInt():\n" + n.OuterXml);
			return VerifyInt((XmlElement)n);
			

			// verify the signature and return the signing cert
		}

		/// <summary>
		/// Verifies a signature by serializing the current object to XML and comparing to the given signature.
		/// </summary>
		/// <param name="ct">XmlDSIG signature in the form of XML element</param>
		/// <returns>X509 certificate in WSE v1 format.</returns>
		public Microsoft.Web.Services.Security.X509.X509Certificate VerifyReturningWSE1Cert(XmlElement xe)
		{
			string b64 = this.VerifyReturningBase64Cert(xe);
			Microsoft.Web.Services.Security.X509.X509Certificate cert = new Microsoft.Web.Services.Security.X509.X509Certificate(Convert.FromBase64String(b64));
			return cert;
		}

		/// <summary>
		/// Verifies a signature by serializing the current object to XML and comparing to the given signature.
		/// </summary>
		/// <param name="ct">XmlDSIG signature in the form of XML element</param>
		/// <returns>X509 certificate in WSE v2 format.</returns>
		public Microsoft.Web.Services2.Security.X509.X509Certificate VerifyReturningWSE2Cert(XmlElement xe)
		{
			string b64 = this.VerifyReturningBase64Cert(xe);
			Microsoft.Web.Services2.Security.X509.X509Certificate cert = new Microsoft.Web.Services2.Security.X509.X509Certificate(Convert.FromBase64String(b64));
			return cert;
		}

		private void LogEvent (LogType t, string s) 
		{
			Utilities.LogEvent(t,s);
		}

		private void LogEvent(string s) 
		{
			Utilities.LogEvent(s);
		}

		/// <summary>
		/// Internally used for verifying.
		/// </summary>
		/// <param name="toVerify"></param>
		/// <returns></returns>
		private string VerifyInt(XmlElement toVerify) 
		{
			// load it..

			MySignedXml sx = new MySignedXml();
			//TODO takeout...
			sx.LoadXml(toVerify);
			bool good = false;

			try 
			{ 
				good = sx.CheckSignature();

			} 
			catch (Exception e) 
			{
				LogEvent(LogType.Error,e.Message);
			}
			// check the signature
			if(!good) 
			{
				LogEvent(LogType.Debug,"VerifySignature() xml = " + sx.GetXml().OuterXml);
				LogEvent(LogType.Error,this + ".VerifyInt(): Verifying signature.\r\n\r\n\tVerification failure.");
				throw new Exception ("Verification failed.");
			}
			else 
			{
				// if it's good, return the certificate.
				string b64cert = ((XmlElement)toVerify.GetElementsByTagName("X509Certificate")[0]).InnerText;

				if (Utilities.VerifyCertificate(b64cert))
					return b64cert;
				else 
					return null;
			}
		}
	}
}
