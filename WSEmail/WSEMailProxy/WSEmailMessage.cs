/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using Microsoft.Web.Services2.Security;
using Microsoft.Web.Services2.Security.X509;
using Microsoft.Web.Services2.Security.Tokens;


namespace WSEmailProxy
{
	/// <summary>
	/// The fundamental parts that create a WSEmailMessage.
	/// </summary>
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securitylab.cis.upenn.edu/WSEmail")]
	[Serializable]
	public class WSEmailMessage : PennLibraries.SignableObject, ICloneable
	{
		/// <summary>
		/// Clones this object
		/// </summary>
		/// <returns>Object</returns>
		public object Clone() 
		{
			return this.MemberwiseClone();
		}

		/// <summary>
		/// Default empty constructor.
		/// </summary>
		public WSEmailMessage() 
		{
			Recipients = new RecipientList();
		}

		/// <summary>
		/// Holds the message body.
		/// </summary>
		public string Body ;

		/// <summary>
		/// Holds the subject of the message.
		/// </summary>
		public string Subject ;

		/// <summary>
		/// Holds the intended recipient of the message.
		/// </summary>
		public RecipientList Recipients;

		/// <summary>
		/// Holds the sender of a message. Should be in user@host form.
		/// </summary>
		public string Sender ;

		/// <summary>
		/// Holds a timestamp
		/// </summary>
		public DateTime Timestamp 
		{
			get 
			{
				return _time;
			}
			set 
			{
				// Sql server of extra ticks
				_time = value;
				_time = _time.AddTicks(-1 * _time.Ticks % 100000);
				
			}
		}


		private DateTime _time;


		/// <summary>
		/// Holds XmlAttachments in our DyanmicForms.BaseObject format.
		/// </summary>
		[System.Xml.Serialization.XmlArrayItemAttribute("XmlNode")]
		public XmlNode[] XmlAttachments;

		/// <summary>
		/// Holds a number that describes how the message should be handled. <see cref="#WSEmailFlags"/>
		/// </summary>
		public int MessageFlags = 0;

		/// <summary>
		/// Provides a way to print the message out nicely.
		/// </summary>
		/// <returns></returns>
		public override string ToString() 
		{
			string s = "WSEmail message:\nFrom: " + Sender + "\nTo: " + Recipients.ToString() + "\nSubject: " + Subject + "\nDate: " + Timestamp.ToLongDateString() + "\nMessage: " + Body + "\n";
			return s;
		}

		public static WSEmailMessage Deserialize(string s) 
		{
			XmlSerializer xs = new XmlSerializer(typeof(WSEmailMessage));
			MemoryStream ms = new MemoryStream();

			byte[] bytes = Convert.FromBase64String(s);

			ms.Write(bytes,0,bytes.Length);
			ms.Position = 0;

			WSEmailMessage m = (WSEmailMessage)xs.Deserialize(ms);
			return m;
		}
	}
		/// <summary>
		/// Signs a messages using XML DSig with the given certificate and returns the certificate.
		/// </summary>
		/// <param name="cert">x509 certificate</param>
		/// <returns></returns>
	/*
		public XmlElement SignMessage(X509Certificate cert) 
		{
			return SignMessage(new X509SecurityToken(cert));
			


			/*
			XmlNode root = d.CreateNode(XmlNodeType.Element,"","MessageSignature","http://securitylab.cis.upenn.edu/RoutingSigner/");
			d.ImportNode(root,true);
			XmlNode toSign = d.CreateNode(XmlNodeType.Element,"","SignMe","http://securitylab.cis.upenn.edu/RoutingSignature/");
			toSign.InnerText = d.GetElementsByTagName("WSEmailMessage")[0].InnerXml;;
			*/
			// PennRoutingFilters.PennRoutingUtilities.LogEvent(this + " : signed message.\nWith certificate where CN = " + cert.Certificate + "\n"+ toSign.OuterXml);
			// Create the SignedXml message.
			//TODO fix this, it's ugly.
/*			X509SecurityToken tok = new X509SecurityToken(cert);
			Signature signedXml = new Signature(tok);
			
			
			// signedXml.SigningKey = tok.k;
			// Create a data object to hold the data to sign.
			signedXml.AddObject(new System.Security.Cryptography.Xml.DataObject("MessageSignature","","",(XmlElement)toSign));
			signedXml.AddReference(new Reference("#MessageSignature"));
				
			//System.Security.Cryptography.Xml.KeyInfo keyInfo = new System.Security.Cryptography.Xml.KeyInfo();

			// Include the certificate raw data with the signed xml
			//keyInfo.AddClause(new System.Security.Cryptography.Xml.KeyInfoX509Data(cert));
			//TODO take me out.
			//signedXml.KeyInfo = keyInfo;

			signedXml.ComputeSignature();
//			PennLibraries.Utilities.LogEvent(PennLibraries.LogType.Debug,"Sign() = " + signedXml.GetXml(.OuterXml);
			XmlElement toAdd = signedXml.GetXml();
			toAdd.RemoveChild(toAdd.GetElementsByTagName("Object")[0]);
			root.AppendChild(d.ImportNode(toAdd,true));
w			return (XmlElement)root;
*/
//		}

		/// <summary>
		/// Signs a messages with an X509 security token (basically it just extracts the certificate and calls other
		/// signmessage method).
		/// </summary>
		/// <param name="cert"></param>
		/// <returns></returns>
/*
		public XmlElement SignMessage(X509SecurityToken ct) 
		{
			Microsoft.Web.Services.Security.X509SecurityToken cert = new Microsoft.Web.Services.Security.X509SecurityToken(new Microsoft.Web.Services.Security.X509.X509Certificate(ct.Certificate.GetRawCertData()));

			XmlSerializer xs = new XmlSerializer(this.GetType());
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			xs.Serialize(ms,this);
			ms.Position = 0;

			XmlDocument d = new XmlDocument();
			d.PreserveWhitespace = false;
			d.Load(ms);
			
			XmlNode root = d.CreateNode(XmlNodeType.Element,"","MessageSignature","http://securitylab.cis.upenn.edu/RoutingSigner/");
			d.ImportNode(root,true);
			XmlNode toSign = d.CreateNode(XmlNodeType.Element,"","SignMe","http://securitylab.cis.upenn.edu/RoutingSignature/");
			toSign.InnerText = d.GetElementsByTagName("WSEmailMessage")[0].InnerXml;;
			// PennRoutingFilters.PennRoutingUtilities.LogEvent(this + " : signed message.\nWith certificate where CN = " + cert.Certificate + "\n"+ toSign.OuterXml);
			// Create the SignedXml message.
			Microsoft.Web.Services.Security.SignedXml signedXml = new Microsoft.Web.Services.Security.SignedXml();

			
			signedXml.SigningKey = cert.Certificate.Key;
			// Create a data object to hold the data to sign.
			signedXml.AddObject(new System.Security.Cryptography.Xml.DataObject("MessageSignature","","",(XmlElement)toSign));
			signedXml.AddReference(new Microsoft.Web.Services.Security.Reference("#MessageSignature"));
				
			System.Security.Cryptography.Xml.KeyInfo keyInfo = new System.Security.Cryptography.Xml.KeyInfo();

			// Include the certificate raw data with the signed xml
			keyInfo.AddClause(new System.Security.Cryptography.Xml.KeyInfoX509Data(cert.Certificate));
			//TODO take me out.
			signedXml.KeyInfo = keyInfo;

			signedXml.ComputeSignature();
			PennLibraries.Utilities.LogEvent(PennLibraries.LogType.Debug,"Sign() = " + signedXml.GetXml().OuterXml);
			XmlElement toAdd = signedXml.GetXml();
			toAdd.RemoveChild(toAdd.GetElementsByTagName("Object")[0]);
			root.AppendChild(d.ImportNode(toAdd,true));
			return (XmlElement)root;

			/*
			
			XmlSerializer xs = new XmlSerializer(this.GetType());
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			xs.Serialize(ms,this);
			ms.Position = 0;

			XmlDocument d = new XmlDocument();
			d.PreserveWhitespace = false;
			d.Load(ms);
			
			PennLibraries.Utilities.LogEvent(PennLibraries.LogType.Debug,"Sign() = " + d.ChildNodes[1].OuterXml);


			CspParameters cp = new CspParameters();
			cp.KeyContainerName = "HI";
			cp.KeyNumber = 1;
			RSACryptoServiceProvider rsaCSP = new RSACryptoServiceProvider(cp);
			
			rsaCSP.FromXmlString(cert.Certificate.Key.ToXmlString(true));
			SignedXml signedXml = new SignedXml();
			signedXml.SigningKey = rsaCSP;
			signedXml.AddObject(new DataObject("Message","","",(XmlElement)d.ChildNodes[1]));
			signedXml.AddReference(new System.Security.Cryptography.Xml.Reference("#Message"));

			signedXml.ComputeSignature();
			XmlElement e = signedXml.GetXml();
			PennLibraries.Utilities.LogEvent(PennLibraries.LogType.Debug,"Sign() = " + e.OuterXml);
			return e;
/*			

			SignedXml s = new SignedXml();

			RSA r = cert.Certificate.Key;
			
			if (r == null) 
				PennLibraries.Utilities.LogEvent("r is null.");

			s.SigningKey = r;

			s.AddObject(new DataObject("Message","","",(XmlElement)d.ChildNodes[1]));
			s.AddReference(new System.Security.Cryptography.Xml.Reference("#Message"));

			KeyInfo keyInfo = new KeyInfo();
			keyInfo.AddClause(new RSAKeyValue(r));
			s.KeyInfo = keyInfo;

			s.ComputeSignature();
			XmlElement e = s.Signature.GetXml();

			PennLibraries.Utilities.LogEvent(PennLibraries.LogType.Debug,"Sign() = " + e.OuterXml);
			return e;
			/* <MessageSignature xmlns="http://securitylab.cis.upenn.edu/RoutingSigner/">
				<Signature xmlns="http://www.w3.org/2000/09/xmldsig#">
					<SignedInfo>
						<CanonicalizationMethod Algorithm="http://www.w3.org/2001/10/xml-exc-c14n#" />
						<SignatureMethod Algorithm="http://www.w3.org/2000/09/xmldsig#rsa-sha1" />
						<Reference URI="#MessageSignature">
							<DigestMethod Algorithm="http://www.w3.org/2000/09/xmldsig#sha1" />
							<DigestValue>/SHYfUfx ... 5ZkO2QA=</DigestValue>
						</Reference>
					</SignedInfo>
					<SignatureValue>lj/xMpP8 ... qF3rCg==</SignatureValue>
					<KeyInfo>
					<X509Data xmlns="http://www.w3.org/2000/09/xmldsig#">
						<X509Certificate>MIIFMDCC ... LyBl7UcW</X509Certificate>
					</X509Data>
					</KeyInfo>
				</Signature>
              </MessageSignature>
				*/
			/*
			
			MessageSignature sig = new MessageSignature(cert);
			sig.Document = d;XmlElement el = null;
			try 
			{
				sig.ComputeSignature();
				 el = sig.GetXml(d);
			} 
			catch (System.Security.Cryptography.CryptographicException e) 
			{
				PennLibraries.Utilities.LogEvent(e.StackTrace);
				PennLibraries.Utilities.LogEvent(e.Message);
			}
			
			PennLibraries.Utilities.LogEvent(PennLibraries.LogType.Debug,"Sign() = " + el.OuterXml);
			return sig.GetXml(d);
			*/
		//}
		
/*		public string Serialize() 
		{
			XmlSerializer xs = new XmlSerializer(this.GetType());
			MemoryStream ms = new MemoryStream();

			xs.Serialize(ms,this);
			ms.Position = 0;

			return Convert.ToBase64String(ms.ToArray());
		}
*/

}
