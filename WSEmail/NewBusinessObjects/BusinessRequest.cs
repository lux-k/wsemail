/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using DynamicForms;
using System;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Windows.Forms;
using PennLibraries;
using Microsoft.Web.Services2;
using Microsoft.Web.Services2.Security;
using Microsoft.Web.Services2.Security.X509;
using Microsoft.Web.Services2.Security.Tokens;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;


namespace DynamicBizObjects
{

	/// <summary>
	/// Summary description for BusinessRequest.
	/// </summary>
	///
	[Serializable]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securitylab.cis.upenn.edu/AuthorizedRouting")]
	[XmlInclude(typeof(Signature)), XmlInclude(typeof(Timesheet)), XmlInclude(typeof(MappedItem)), XmlInclude(typeof(PurchaseOrder))]
	public class BusinessRequest
	{
		/// <summary>
		/// The handle to the actual business object.
		/// </summary>
		private BusinessObject _bo = null;
		/// <summary>
		/// An array of approvals (both received and pending) for this request.
		/// </summary>
		private Approvals _approvals = null;
		/// <summary>
		/// Namespace of this object.
		/// </summary>
		private const string Namespace = "http://securitylab.cis.upenn.edu/AuthorizedRouting/";
		/// <summary>
		/// Array of delegates.
		/// </summary>
		private string[] _del = null;
		private string MyID = "";
		/// <summary>
		/// Whether or not to add delegates to the front (true) or the end (false).
		/// </summary>
		private bool _idaf = false;

		/// <summary>
		/// A list of mappings. Maps a user to a role.
		/// </summary>
		public ArrayList Mappings = new ArrayList();


		/// <summary>
		/// Set to true then add delegates for them to show up at the front. Otherwise, delegates get added to the end.
		/// </summary>
		[System.Xml.Serialization.XmlIgnore()]
		public bool InsertDelegatesAtFront 
		{
			get 
			{
				return _idaf;
			}
			set 
			{
				_idaf = value;
			}
		}

		/// <summary>
		/// Returns the next person the form should be sent to.
		/// </summary>
		public string GetNextHop 
		{
			get 
			{
				if (HasMoreHops) 
				{
					return ((Signature)Approvals.Required[0]).User;
				}
				return "Verifier@" + ConfigurationSettings.AppSettings["ServerName"];
			}
		}

		/// <summary>
		/// Whether or not more people must approve the form for it to be complete.
		/// </summary>
		public bool HasMoreHops 
		{
			get 
			{
				if (Approvals.Required.Count >= 1)
					return true;
				else
					return false;
			}
		}

		/// <summary>
		/// An array of delegated people.
		/// </summary>
		[System.Xml.Serialization.XmlIgnore()]
		public string[] Delegates 
		{
			get 
			{
				return _del;
			}
			set 
			{
				_del = value;
			}
		}
		
		/// <summary>
		/// The actual business object.
		/// </summary>
		public BusinessObject BusinessObject 
		{
			get 
			{
				return _bo;
			}
			set 
			{
				_bo = value;
			}
		}


		/// <summary>
		/// The approvals -- both pending and received.
		/// </summary>
		public Approvals Approvals 
		{
			get 
			{
				return _approvals;
			}

			set 
			{
				_approvals = value;
			}
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public BusinessRequest()
		{
			_approvals = new Approvals();
		}

		/// <summary>
		/// Reassociates signatures and mappings. For use after deserialization and such.
		/// </summary>
		public void RefreshMappings() 
		{
			Signature[] sigs = null;
			for (int i = 0; i < Mappings.Count; i++) 
			{
				sigs = Approvals.GetSignatureByName(((MappedItem)Mappings[i]).User);
				if (sigs.Length > 0)
					((MappedItem)Mappings[i]).Signature = sigs[0];
			}
		}

		/// <summary>
		/// Logs a message
		/// </summary>
		/// <param name="s"></param>
		public static void Log(LogType t, string s) 
		{
			Utilities.LogEvent(t,s);
		}

		/// <summary>
		/// Verifies all the signatures received thus far.
		/// </summary>
		/// <returns></returns>
		public bool VerifyAllSignatures() 
		{
			if (Approvals.Received.Count == 0) 
				return true;
			
			Signature s;
			bool res = true;
			for (int i = 0; i < Approvals.Received.Count;  i++) 
			{
				s = (Signature)Approvals.Received[i];
				if (s.AddedBy == null || s.AddedBy.Equals("")) 
				{
					Log(LogType.Debug, "Calling RecusivelyVerifySignature on " + s);
					res &= RecursivelyVerifySignature(s);
				}
				if (!res)
					break;
			}
			return res;
		}

		/// <summary>
		/// Returns the mapped item structure for a particular item.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public MappedItem GetMappedItem (string s) 
		{
			for (int i = 0; i < Mappings.Count; i++) 
			{
				if (((MappedItem)Mappings[i]).Name.Equals(s))
					return (MappedItem)Mappings[i];
			}
			return null;

			/*
			int i = Mappings.IndexOf(s);
			if (i >= 0)
				return (MappedItem)Mappings[i];
			else
				return null;
			*/
		}

		/// <summary>
		/// Gets all the mapping for a particular user ID.
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		public MappedItem[] GetMappingsByID(string ID) 
		{
			ArrayList a = new ArrayList();
			for (int i = 0; i < Mappings.Count; i++) 
			{
				MappedItem m = (MappedItem)Mappings[i];
				if (ID.Length != 0) 
				{
					if (m.User != null && m.MappedBy != null && m.MappedBy.Equals(ID))
						a.Add(m);
				}
				else  
				{
					if (m.User == null ||  m.MappedBy == null || m.User.Length == 0 || m.MappedBy.Length == 0)
						a.Add(m);
				}
			}
			return (MappedItem[])a.ToArray(typeof(MappedItem));
		}

		/// <summary>
		/// Gets mappings that have a null user ID.
		/// </summary>
		/// <returns></returns>
		public MappedItem[] GetNullMappings() 
		{
			return GetMappingsByID("");
		}

		/// <summary>
		/// Verifies a signature and all child signatures it contains.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public bool RecursivelyVerifySignature(Signature s) 
		{
			Signature[] sigs = Approvals.GetDelegates(s.User);

			bool res = true;
			// verify the signature
			res &= VerifySignature(s);
			// and all the children, if there are any.
			if (sigs.Length > 0 && res) 
			{
				for (int i = 0; i < sigs.Length; i++) 
				{
					res &= RecursivelyVerifySignature(sigs[i]);
					if (!res) 
					{
						Log(LogType.Debug,"Verification failed for: " + sigs[i].ToString());
						break;
					}
				}
			}
			return res;
		}

		/// <summary>
		/// Gets all the distinct entities that are mapped.
		/// </summary>
		/// <returns></returns>
		public string[] GetMappingEntities() 
		{
			ArrayList a = new ArrayList();
			for (int i = 0; i < Mappings.Count; i++) 
				if (a.IndexOf( ((MappedItem)Mappings[i]).MappedBy) < 0)
					a.Add(((MappedItem)Mappings[i]).MappedBy);

			return (string[])a.ToArray(typeof(string));
		}

		/// <summary>
		/// Verifies a signature. Don't know how/why.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public bool VerifySignature(Signature s) 
		{
			Log(LogType.Debug,"Verifying signature on " + s);
			if (s.Value == null)
				return false;
			
			// TODO I have no clue about this.

			MySignedXml m = new MySignedXml();
			XmlDocument d = new XmlDocument();
			XmlElement e = s.Value;
			XmlElement f = (new System.Security.Cryptography.Xml.DataObject("BusinessObject","","",GetSerializedBusinessObject(false,s.Timestamp,s.User))).GetXml();
			d.AppendChild(d.ImportNode(e,true));
			d.ChildNodes[0].AppendChild(d.ImportNode(f,true));
			m.LoadXml((XmlElement)d.GetElementsByTagName("Signature")[0]);
			
			return m.CheckSignature();
		}

		/// <summary>
		/// Serializes an object to xml.
		/// </summary>
		/// <param name="RootAttribute"></param>
		/// <param name="Namespace"></param>
		/// <param name="Type"></param>
		/// <param name="o"></param>
		/// <returns></returns>

		private string GetSerializedObject(string RootAttribute, string Namespace, System.Type Type, Object o) 
		{
			XmlRootAttribute root = new XmlRootAttribute(RootAttribute);
			root.DataType = o.GetType().Name;
			root.Namespace = Namespace;
			XmlSerializer x = new XmlSerializer(Type,root);
			MemoryStream m = new MemoryStream();
			x.Serialize(m,o);
			return (System.Text.Encoding.ASCII.GetString(m.ToArray())).Trim('\0').Replace("<?xml version=\"1.0\"?>","");
		}

		private void CleanSignatureArray(ref Signature[] sigs) 
		{
			if (sigs == null || sigs.Length == 0)
				return;
			for (int i = 0; i < sigs.Length; i++) 
			{
				sigs[i].Value = null;
				sigs[i].Timestamp = "";
			}
			Array.Sort(sigs);
		}

		public XmlElement GetSerializedBusinessObject(bool ForSigning,string datetime,string id) 
		{
			string s = GetSerializedObject("Timestamp",Namespace+"Timestamp",datetime.GetType(),datetime);
			s += GetSerializedObject("BusinessObject",Namespace+"BusinessObject",BusinessObject.GetType(),BusinessObject);

		{
			Signature[] sigs;
			if (ForSigning)
				// sign only those delegates we have specified
				sigs = Approvals.GetDelegates(Delegates,id);
			else
				// get them all
				sigs = Approvals.GetDelegates(id);

			if (sigs.Length > 0) 
			{
				CleanSignatureArray(ref sigs);
				string s2 = GetSerializedObject("Delegates",Namespace + "delegates",sigs.GetType(),sigs);

				s += s2;
			}
		}

			{
				MappedItem[] maps;
					// get them all
				maps = this.GetMappingsByID(id);

				if (maps.Length > 0) 
				{
					string s2 = GetSerializedObject("Mappings",Namespace + "mappings",maps.GetType(),maps);

					s += s2;
				}
			}

			// MessageBox.Show(s);
			
			int i = s.IndexOf("<BusinessObject ");
			s = s.Replace(s.Substring(i+16,s.IndexOf(">",i+1) - i - 16),"xsi:type=\""+BusinessObject.GetType().Name+"\"");
//			MessageBox.Show(s);
			XmlDocument doc = new XmlDocument();
			XmlElement ele = (XmlElement)doc.CreateNode(XmlNodeType.Element,"","BusinessObject","http://securitylab.cis.upenn.edu/BusinessObject/");
			ele.InnerText = s;

			return ele;
		}

		private XmlElement SignRequest (X509SecurityToken cert, string datetime) 
		{
			// TODO fix this.
			/*
			Microsoft.Web.Services.Security.SignedXml signedXml = new Microsoft.Web.Services.Security.SignedXml();

			signedXml.SigningKey = cert.Certificate.Key;
			// Create a data object to hold the data to sign.
			signedXml.AddObject(new System.Security.Cryptography.Xml.DataObject("BusinessObject","","",GetSerializedBusinessObject(true,datetime,MyID)));
			// mandatory reference
			signedXml.AddReference(new Microsoft.Web.Services.Security.Reference("#BusinessObject"));
			
			KeyInfo keyInfo = new KeyInfo();

			// Include the certificate raw data with the signed xml
			keyInfo.AddClause(new KeyInfoX509Data(cert.Certificate));
			// PennRoutingUtilities.LogEvent(this + " Signed...\nWith certificate where CN = " + _signingCert + "\nUsing machine store: " + UseMachineStore+"\nValue signed:"+ toSign.OuterXml);
			// add the keyinfo
			signedXml.KeyInfo = keyInfo;

			// sign it and add it
			signedXml.ComputeSignature();
			
			XmlElement toAdd = signedXml.GetXml();
			toAdd.RemoveChild(toAdd.GetElementsByTagName("Object")[0]);

			Log(LogType.Debug, this + " Signed...\nWith certificate where CN = " + MyID +  "\nUsing machine store: " +  "\nOutput: "+toAdd.OuterXml);
			return toAdd;
			*/
			return null;

		}

		public void GenerateApproval(string CertCN, bool UseMachineStore) 
		{
			if (CertCN == null)
				CertCN = ConfigurationSettings.AppSettings["SigningCertificate"];

			X509SecurityToken cert = Utilities.GetSecurityToken(CertCN,UseMachineStore);
			MyID = Utilities.GetCertEmail(cert.Certificate);


			if (Delegates != null && Delegates.Length > 0) 
			{
				Log(LogType.Debug, "Adding delegation for " + Delegates);
				for (int i = 0; i < Delegates.Length; i++)
					if (InsertDelegatesAtFront)
						Approvals.Required.Insert(0+i,new Signature(Delegates[i],MyID));
					else
						Approvals.Required.Add(new Signature(Delegates[i],MyID));

			}


			MappedItem[] m = GetNullMappings();
			if (m != null && m.Length > 0) 
			{
				for (int i = 0; i < m.Length; i++) 
				{
					if (m[i].MappedBy == null)
						m[i].MappedBy = MyID;
					if (m[i].User == null) 
					{
						m[i].User = MyID;
					}
				}
			}


			string datetime = Utilities.GetCurrentTime();
			XmlElement signature = SignRequest(cert,datetime);
			int spot = Approvals.Required.FindID(MyID);
			Signature s;
			// if it's in the required list, then get it out.
			if (spot >= 0) 
			{
				s = (Signature)Approvals.Required[spot];
				Approvals.Required.RemoveAt(spot);
			} 
			else
				s = new Signature(MyID);
            			
			s.Value = signature;
			s.Timestamp = datetime;

			m = GetMappingsByID(MyID);
			if (m != null && m.Length > 0) 
			{
				for (int i = 0; i < m.Length; i++) 
					m[i].Signature = s;
			}

			
			Approvals.Received.Add(s);


		}
	}





	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securitylab.cis.upenn.edu/AuthorizedRouting")]
	public class SignatureList : ArrayList 
	{
		public SignatureList(int i) 
		{
			
		}

		public SignatureList() { }

		public int FindID(string ID) 
		{
			ID = ID.ToLower();
			for (int i = 0; i < this.Count; i++) 
			{
				if (((Signature)this[i]).User.ToLower().Equals(ID))
					return i;
			}
			return -1;
		}
	}


}

