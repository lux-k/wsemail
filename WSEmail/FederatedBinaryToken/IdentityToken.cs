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
using Microsoft.Web.Services2.Security;
using Microsoft.Web.Services2.Security.Cryptography;

namespace FederatedBinaryToken 
{
	/// <summary>
	/// The identity information of a federated token.
	/// </summary>
	public class IdentityToken : PennLibraries.SignableObject
	{

		/// <summary>
		/// The RSA crypto provider to do signing and such.
		/// </summary>
		[System.Xml.Serialization.XmlIgnore()]
		public RSA15 RSA = null;
		/// <summary>
		/// Name of the RSA container where the keys are stored.
		/// </summary>
		[System.Xml.Serialization.XmlIgnore()]
		public string KeyContainer;
		/// <summary>
		/// The username
		/// </summary>
		private string _user = null;
		private string _validfrom, _validtill = null;
		/// <summary>
		/// The url where current validty information can be retrieved.
		/// </summary>
		private string _validityurl = null;
		/// <summary>
		/// Id of this token
		/// </summary>
		private string _uuid = null;

		/// <summary>
		/// Returns the identifier of the token.
		/// </summary>
		public string Identifer 
		{
			get 
			{
				return _uuid;
			}
			set 
			{
				_uuid  = value;
			}
		}

		/// <summary>
		/// Returns the date the token is valid from
		/// </summary>
		public string ValidFrom 
		{
			get 
			{
				return _validfrom;
			}
			set 
			{
				_validfrom = value;
			}
		}

		/// <summary>
		/// Returns the date the token is valid till
		/// </summary>
		public string ValidTill 
		{
			get 
			{
				return _validtill;
			}
			set 
			{
				_validtill = value;
			}
		}

		/// <summary>
		/// Returns the url where the token can be verified as being issued.
		/// </summary>
		public string CheckURL 
		{
			get 
			{
				return _validityurl;
			}
			set 
			{
				_validityurl = value;
			}
		}

		/// <summary>
		/// Returns the userID of the token, ie. who it's bound to.
		/// </summary>
		public string UserID 
		{
			get 
			{
				return _user;
			}
			set 
			{
				_user = value;
			}
		}

		public override XmlDocument SerializeToXmlDocument()
		{
			XmlDocument d = base.SerializeToXmlDocument();
			if (this.RSA != null) 
			{
				XmlElement key = d.CreateElement("Key");
				key.InnerText = RSA.Key.ToXmlString(false);
				key = (XmlElement)d.ImportNode(key,true);
				d.FirstChild.AppendChild(key);
			}

			return d;
		}

		/// <summary>
		/// Loads an identity token from an XmlElement
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public static IdentityToken Load(XmlElement e) 
		{
			PennLibraries.Utilities.LogEvent("Received e as " + e.OuterXml);
			XmlNodeList l = null;

			XmlElement tok = e;
			l = e.GetElementsByTagName("Key");
			XmlElement rsa = null;
			if (l != null && l.Count > 0) 
			{
				rsa = (XmlElement)l[0];
				e.RemoveChild(rsa);
			}
			PennLibraries.Utilities.LogEvent("E is now " + e.OuterXml);
			PennLibraries.Utilities.LogEvent("rsa is now " + rsa.OuterXml);
				
			XmlSerializer xs = new XmlSerializer(typeof(IdentityToken));
			MemoryStream ms = new MemoryStream();
			byte[] bytes = System.Text.Encoding.ASCII.GetBytes(tok.OuterXml);
			ms.Write(bytes,0,bytes.Length);
			ms.Position = 0;
			IdentityToken t = (IdentityToken)xs.Deserialize(ms);
			ms.Close();
			if (rsa != null) 
			{
				System.Security.Cryptography.RSACryptoServiceProvider rr = new System.Security.Cryptography.RSACryptoServiceProvider();
				rr.FromXmlString(rsa.InnerText);
				t.RSA = new RSA15(rr);
			}

			if (t == null || rsa == null)
				Log("the deserialized token or the rsa key info is null.");

			return t;

		}


		private static void Log(PennLibraries.LogType t, string s) 
		{
			PennLibraries.Utilities.LogEvent(t,s);
		}
			
		private static void Log(string s) 
		{
			PennLibraries.Utilities.LogEvent(s);
		}

		/// <summary>
		/// Checks that the dates on the token are valid. (ie. not before issued, not expired)
		/// </summary>
		/// <returns>true if valid, exceptions otherwise</returns>
		public bool IsDateValid() 
		{
			DateTime from, till;

			try 
			{
				from = DateTime.ParseExact(this.ValidFrom,"r",null);
			} 
			catch
			{
				throw new Exception("Unable to properly parse the valid from date.");
			}

			try 
			{
				till = DateTime.ParseExact(this.ValidTill,"r",null);
			} 
			catch
			{
				throw new Exception("Unable to properly parse the valid to date.");
			}

			if (DateTime.Compare(from,till) > 0)
				throw new Exception("Validity period is invalid. The beginning date is after the end date.");

			if (DateTime.Compare(from,till) == 0)
				throw new Exception("Validity period is invalid. The beginning and ending dates are the same.");

			if (DateTime.Compare(DateTime.UtcNow,from) < 0)
				throw new Exception("Current date is before validity period.");

			if (DateTime.Compare(DateTime.UtcNow,till) > 0)
				throw new Exception("Current date exceeds validity period.");

			return true;
				
		}
	}
}
