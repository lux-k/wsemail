/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.IO;
using System.Configuration;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Cryptography.Xml;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Web.Services2.Policy;
using Microsoft.Web.Services2.Security;
using Microsoft.Web.Services2.Security.Cryptography;
using Microsoft.Web.Services2.Security.Tokens;
using Microsoft.Web.Services2.Security.Utility;
using WSEmailProxy;

namespace FederatedBinaryToken
{

	/// <summary>
	/// Federated token which can be used in the WSE v2.0
	/// </summary>
	[SecurityPermission(SecurityAction.Demand, Flags= SecurityPermissionFlag.UnmanagedCode)]
	public class FederatedToken : BinarySecurityToken
	{
		/// <summary>
		/// Namespace this token belongs in.
		/// </summary>
		public static readonly string NamespaceURI  =
			"http://securitylab.cis.upenn.edu/FederatedBinaryToken";
		/// <summary>
		/// Name of the token.
		/// </summary>
		public static readonly XmlQualifiedName TokenValueType = new
			XmlQualifiedName("FederatedBinaryToken", NamespaceURI);


		/// <summary>
		/// Whether or not there is a private key available
		/// </summary>
		private Boolean privateKeyAvailable;
		/// <summary>
		/// The signature that the token is valid (from the issuer)
		/// </summary>
		public XmlElement IdentityTokenSignature = null;
		/// <summary>
		/// The identity information.
		/// </summary>
		public IdentityToken IdentityToken = null;

		private static void Log(PennLibraries.LogType t, string s) 
		{
			PennLibraries.Utilities.LogEvent(t,s);
		}

		/// <summary>
		/// This is the raw (byte) data stored in the certificate. Getting it returns a serialized
		/// form of the IdentityToken, IdentityTokenSignature and other goodies in byte form. Setting
		/// forces a reparse of whatever data is passed in.
		/// </summary>
		public override byte[] RawData 
		{
			get 
			{
				// whats needed? the signature, the identity token and the key info
				XmlDocument d = new XmlDocument();

				XmlElement root = d.CreateElement("RawData");

				if (IdentityToken != null) 
				{
					XmlElement temp = (XmlElement)IdentityToken.SerializeToXmlDocument().FirstChild;
					temp = (XmlElement)d.ImportNode(temp,true);
					root.AppendChild(temp);
				}
				if (IdentityTokenSignature != null) 
				{
					XmlElement temp = (XmlElement)d.ImportNode(this.IdentityTokenSignature,true);
					root.AppendChild(temp);
				}

				Log(PennLibraries.LogType.Debug,"Returning:\n\n " + root.OuterXml + "\n\nas FederatedToken innards.");
				return System.Text.Encoding.UTF8.GetBytes(root.OuterXml);
			}
		}

		public override bool IsCurrent
		{
			get
			{
				return !this.Expired;
			}
		}

		public override int GetHashCode()
		{
			return IdentityToken.GetHashCode();
		}

		public override bool Equals(SecurityToken token)
		{
			if (this.GetHashCode() == token.GetHashCode())
				return true;

			return false;
		}

		/// <summary>
		///  This constructor is also used by the DecryptionKeyProvider,
		/// to retrieve the decryption key stored in the crypto key
		/// container on the receiver's computer.  Additionally, this
		/// constructor is used by a developer for the sender computer to
		/// obtain an XML string of their public key.  For this use, call
		/// GetXmlString after creating the instance of BinaryToken.
		/// </summary>
		/// <param name="keyContainer">The name of the RSA key container</param>
		public FederatedToken(string keyContainer) : base(TokenValueType.ToString()) 
		{
			// Get the RSA key from the crypto key container on the local
			// computer.
			IdentityToken.RSA = new RSA15(GetKeyFromContainer(keyContainer));

			//  Assume that if the key is retrieved from the a crypto key
			// container on the local computer, the private and public keys
			// are available.
			privateKeyAvailable = true;

			// Set the RawData property to the public key. 
			// This value is placed in the WS-Security header and is used
			// as the authentication key by the receiver side.
		}

		/// <summary>
		/// This constructor is called by a sender when it is
		/// encrypting a SOAP message using the BinaryToken.  The cyrpto
		/// key container on the receiver's computer and an XML string
		/// holding the public key for the receiver must be exchanged out
		/// of band.  A developer for the sender uses the GetXmlString
		/// method to get an XML string for their public key.
		/// </summary>
		/// <param name="receiverKeyContainer">Name of the RSA key container on the recipient</param>
		/// <param name="receiverXmlString">The receiver's xml string</param>
		public FederatedToken(string receiverKeyContainer, string
			receiverXmlString) : base(TokenValueType.ToString())
		{
			// Regenerate the receiver's public key from the XML string.
			// A sender can then encrypt the SOAP message using that key.
			this.IdentityToken = new IdentityToken();

			System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider();
			rsa.FromXmlString(receiverXmlString);

			this.IdentityToken.RSA = new RSA15(rsa);
			privateKeyAvailable = false;

			// Store the receiver's crypto key container.  This is passed
			// in the SOAP message in a KeyInfo when the WSE retrieves
			// the EncryptionKey property for this class.
			this.IdentityToken.KeyContainer = receiverKeyContainer;
		}

		/// <summary>
		/// This constructor is called by the WSE when a SOAP message is
		/// received containing a Binary Security Token with a ValueType
		/// element equal the ValueType property for this class.  The
		/// ValueType property is a Server-side constructor. Invokes the
		/// base class, which then loads all properties from the supplied
		/// XmlElement using LoadXml(XmlElement)
		/// </summary>
		/// <param name="element"></param>
		/// 
		public FederatedToken(XmlElement element) : base(FederatedToken.TokenValueType.Name)
		{
			
			privateKeyAvailable = false;
			LoadXml(element);
		}


		/// <summary>
		/// Nicely format the contents of the token to a string.
		/// </summary>
		/// <returns>Formatted string</returns>
		public override string ToString() 
		{
			if (this.IdentityToken == null)
				return base.ToString();

			string s = "Federated Identity Token\n\nToken identifier: " + this.IdentityToken.Identifer + 
				"\nIssued to: " + this.IdentityToken.UserID + "\nValid from: " + this.IdentityToken.ValidFrom +
				"\nValid until: " + this.IdentityToken.ValidTill + "\nVerifiable at: " + 
				this.IdentityToken.CheckURL;

			string t = this.GetIssuerName();
			if (t != null)
				s += "\n\nIssued by: " + t;

			return s;
		}

		/// <summary>
		/// The default implementation parses the required elements
		/// for the BinarySecurityToken element and assigns them to
		/// their respective properties in the BinarySecurityToken
		/// class. These properties are RawData, ValueType and
		/// EncodingType.
		/// </summary>
		/// <param name="element"></param>
		public override void LoadXml(XmlElement element)
		{
			base.LoadXml(element);

			// Check to see whether there were contents in the RawData
			// element that were assigned to the RawData property by the
			// base class LoadXml method.  If RawData is not null, then
			// the sender's public key is assigned to it.
			
			if (base.RawData != null)
			{
				string s = System.Text.Encoding.UTF8.GetString(base.RawData);
				Log(PennLibraries.LogType.Debug,"RawData is " + s);

				XmlDocument d = new XmlDocument();
				d.LoadXml(s);

				XmlNodeList l = d.GetElementsByTagName("IdentityToken");
				if (l.Count == 1) 
				{
					this.IdentityToken = IdentityToken.Load((XmlElement)l[0]);
				}

				l = d.GetElementsByTagName("Signature");
				if (l.Count == 1)
					this.IdentityTokenSignature = (XmlElement)l[0];
			}
		}

		public override Microsoft.Web.Services2.Security.Cryptography.KeyAlgorithm Key
		{
			get
			{
				if (IdentityToken.RSA == null)
					throw new InvalidOperationException("Authentication key is unavailable.");
	
				return this.IdentityToken.RSA;
			}
		}

		/// <summary>
		/// This class supports XML digital signatures. 
		/// </summary>
		public override bool SupportsDigitalSignature
		{
			get
			{
				return IdentityToken.RSA != null;
			}
		}

		/// <summary>
		/// This class supports encryption. 
		/// </summary>
		public override bool SupportsDataEncryption
		{
			get
			{
				return IdentityToken.RSA != null;
			}
		}
    
		/// <summary>
		/// Verifies the token.
		/// </summary>
		public bool Verify(){
			return this.IdentityToken.Verify(this.IdentityTokenSignature);
		}

		/// <summary>
		/// Loads the signing certificate out of the token signature.
		/// </summary>
		/// <returns>Signing Certificate</returns>
		private Microsoft.Web.Services.Security.X509.X509Certificate LoadCertificate() 
		{
			if (this.IdentityTokenSignature == null)
				return null;

			XmlNodeList l = this.IdentityTokenSignature.GetElementsByTagName("X509Certificate");
			if (l == null || l.Count != 1)
				return null;

			Microsoft.Web.Services.Security.X509.X509Certificate c = new Microsoft.Web.Services.Security.X509.X509Certificate(System.Convert.FromBase64String(l[0].InnerText));
			return c;
		}

		/// <summary>
		/// Returns true/false depending on whether or not the certificate has expired.
		/// </summary>
		public bool Expired 
		{
			get 
			{
				if (this.IdentityToken == null)
					return true;

				try 
				{
					if (this.IdentityToken.IsDateValid() == true)
						return false;
				} 
				catch
				{
					return true;
				}
				return true;
			}
		}

		/// <summary>
		/// Gets the name of the issuer (distinguished name)
		/// </summary>
		/// <returns></returns>
		public string GetIssuerName() 
		{
			Microsoft.Web.Services.Security.X509.X509Certificate c = LoadCertificate();
			if (c != null)
				return c.GetName();
			else
				return null;
		}

		/// <summary>
		/// Gets the issuer certificate
		/// </summary>
		/// <returns></returns>
		public Microsoft.Web.Services.Security.X509.X509Certificate GetIssuerCertificate() 
		{
			return LoadCertificate();
		}

		/// <summary>
		/// Sets the RSA crypto provider to a certain provider
		/// </summary>
		/// <param name="p"></param>
		public void SetCryptoProvider(System.Security.Cryptography.RSACryptoServiceProvider p) 
		{
			if (IdentityToken == null)
				IdentityToken = new IdentityToken();
			this.IdentityToken.RSA = new RSA15(p);
			this.privateKeyAvailable = true;
		}

		/// <summary>
		/// Is a private key available?
		/// </summary>
		public Boolean PrivateKeyAvailable
		{
			get
			{ return privateKeyAvailable;}
		}

		/// <summary>
		/// Gets a RSA crypto provider from a container name
		/// </summary>
		/// <param name="ContainerName">Where to load the crypto provider</param>
		/// <returns>RSA provider</returns>
		private System.Security.Cryptography.RSACryptoServiceProvider GetKeyFromContainer(string
			ContainerName)
		{

			// Create the CspParameters object, and set the key container 
			// name used to store the RSA key pair.
			CspParameters cp = new CspParameters();

			//Sets the key number to AT_KEYEXCHANGE.
			cp.KeyNumber = 1; 
			// Sets the provide type to the default rsa provider.
			cp.ProviderType = 1;

			// Get the keys from the local computer key store, when the
			// sender and receiver reside on the same computer and
			// in different process identities.  The keys are
			// stored by default in a store associated with the processes
			// identity.
			// cp.Flags = CspProviderFlags.UseMachineKeyStore;
			cp.KeyContainerName = ContainerName;

			// Create a new instance of RSACryptoServiceProvider based on
			// the instance of CspParameters just populated.

			System.Security.Cryptography.RSACryptoServiceProvider rsa = new
				System.Security.Cryptography.RSACryptoServiceProvider(cp);

			return rsa;
		}

	}


}
