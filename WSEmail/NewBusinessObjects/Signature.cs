/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using Microsoft.Web.Services2.Security.X509;
using System;
using System.Xml;

namespace DynamicBizObjects
{

	/// <summary>
	/// Defines a signature object which is used to delegate responsibilty or
	/// to approve of something.
	/// </summary>
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securitylab.cis.upenn.edu/AuthorizedRouting")]
	public class Signature : ICloneable, IComparable
	{
		string _usr, _prereq, _addedby, _datetime;
		/// <summary>
		/// The signature in XML form.
		/// </summary>
		XmlElement _sig;

		/// <summary>
		/// Creates a copy of a particular instance of the class.
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			// Create a new Person object with the name.
			Signature s = new Signature();
			s.User = this.User;
			s.Value = this.Value;
			s.Timestamp = this.Timestamp;
			s.Prerequisite = this.Prerequisite;
			s.AddedBy = this.AddedBy;
			return s;
		}

		/// <summary>
		/// Compares two signature objects for sorting and such.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public int CompareTo(object obj) 
		{
			if (obj is Signature) 
			{
				Signature s = (Signature)obj;
				return this.User.CompareTo(s.User);
			} 
			else
				throw new ArgumentException("Can't compare a signature to another object that isn't a signature!");
		}

		/// <summary>
		/// Creates a new signature with the specified required signature and who it was
		/// added by.
		/// </summary>
		/// <param name="user"></param>
		/// <param name="addedby"></param>
		public Signature (string user, string addedby) 
		{
			User = user;
			AddedBy = addedby;
		}

		/// <summary>
		/// Creates a new signature for a particular user.
		/// </summary>
		/// <param name="user"></param>
		public Signature (string user) 
		{
			User = user;
		}

		public Signature () {} 

		/// <summary>
		/// The "value" or actual signature of the signature.
		/// </summary>
		[System.Xml.Serialization.XmlElement("")]
		public XmlElement Value 
		{
			get 
			{
				return _sig;
			}
			set 
			{
				_sig = value;
			}
		}

		/// <summary>
		/// The userID who owns the signature.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("user")]
		public string User 
		{
			get 
			{
				return _usr;
			}
			set 
			{
				_usr = value;
			}
		}

		/// <summary>
		/// The date/time the signature was made.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("timeStamp")]
		public string Timestamp 
		{
			get 
			{
				return _datetime;
			}
			set 
			{
				_datetime = value;
			}
		}

		/// <summary>
		/// A userID who is a prerequisite for this signature.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("preReq")]
		public string Prerequisite 
		{
			get 
			{
				return _prereq;
			}
			set 
			{
				_prereq = value;
			}
		}

		/// <summary>
		/// Who requested this signature be added.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("addedBy")]
		public string AddedBy 
		{
			get 
			{
				return _addedby;
			}
			set 
			{
				_addedby = value;
			}
		}

		/// <summary>
		/// Gets a descriptive string for this object.
		/// </summary>
		/// <returns></returns>
		public override string ToString() 
		{
			string s = this.GetType().FullName + "\n\tUser: " + User + "\n\tPrerequisite: " + Prerequisite + "\n\tTimestamp: " + Timestamp;
			return s;
		}

		/// <summary>
		/// Returns the certificate used to generate the signature.
		/// </summary>
		/// <returns></returns>
		public X509Certificate GetCertificate () 
		{
			if (Value != null) 
			{
				X509Certificate cert = new X509Certificate( Convert.FromBase64String((Value.GetElementsByTagName("X509Certificate")[0]).InnerText));
				return cert;
			}
			return null;
		}
	}
}
