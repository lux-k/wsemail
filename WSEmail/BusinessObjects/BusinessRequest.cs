using DynamicForms;
using System;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using PennRoutingFilters;
using System.Windows.Forms;
using Microsoft.Web.Services;
using Microsoft.Web.Services.Security;
using Microsoft.Web.Services.Security.X509;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;


namespace BusinessObjects
{
	/// <summary>
	/// Summary description for BusinessRequest.
	/// </summary>
	///
	[Serializable]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securitylab.cis.upenn.edu/AuthorizedRouting")]
	[XmlInclude(typeof(Signature)), XmlInclude(typeof(Timesheet)), XmlInclude(typeof(MappedItem)), XmlInclude(typeof(PurchaseOrder))]
	public class BusinessRequest : BaseObject
	{
		private BusinessObject _bo = null;
		private Approvals _approvals = null;
		private const string Namespace = "http://securitylab.cis.upenn.edu/AuthorizedRouting/";
		private string[] _del = null;
		private string MyID = "";
		private bool _idaf = false;

		public ArrayList Mappings = new ArrayList();

		public override void Run() 
		{
			if (_bo is Timesheet) 
			{
				FrmTimesheet f = new FrmTimesheet();
				f.LoadBusinessRequestAndShow(this);
				myForm = f;
			} 
			else 
			{
				FrmPurchaseOrder f = new FrmPurchaseOrder();
				f.LoadBusinessRequestAndShow(this);
				myForm = f;
			}
		}

		public override string DebugToScreen() 
		{
			return "HEY!";
		}

		private Form myForm;

		
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

		public string GetNextHop 
		{
			get 
			{
				if (HasMoreHops) 
				{
					return ((Signature)Approvals.Required[0]).User;
				}
				return "Verifier@AutomatedServer";
			}
		}

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

		public BusinessRequest()
		{
			_approvals = new Approvals();
			this.Configuration.DLL = "BusinessObjects";
			this.Configuration.Version = 1.0F;
		}

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
					PennRoutingUtilities.LogEvent("Calling RecusivelyVerifySignature on " + s);
					res &= RecursivelyVerifySignature(s);
				}
				if (!res)
					break;
			}
			return res;
		}
		
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

		public MappedItem[] GetNullMappings() 
		{
			return GetMappingsByID("");
		}

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
						PennRoutingUtilities.LogEvent("Verification failed for: " + sigs[i].ToString());
						break;
					}
				}
			}
			return res;
		}

		public string[] GetMappingEntities() 
		{
			ArrayList a = new ArrayList();
			for (int i = 0; i < Mappings.Count; i++) 
				if (a.IndexOf( ((MappedItem)Mappings[i]).MappedBy) < 0)
					a.Add(((MappedItem)Mappings[i]).MappedBy);

			return (string[])a.ToArray(typeof(string));
		}

		public bool VerifySignature(Signature s) 
		{
			PennRoutingUtilities.LogEvent("Verifying signature on " + s);
			if (s.Value == null)
				return false;

			MySignedXml m = new MySignedXml();
			XmlDocument d = new XmlDocument();
			XmlElement e = s.Value;
			XmlElement f = (new System.Security.Cryptography.Xml.DataObject("BusinessObject","","",GetSerializedBusinessObject(false,s.Timestamp,s.User))).GetXml();
			d.AppendChild(d.ImportNode(e,true));
			d.ChildNodes[0].AppendChild(d.ImportNode(f,true));
			m.LoadXml((XmlElement)d.GetElementsByTagName("Signature")[0]);

			return m.CheckSignature();
		}

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

			PennRoutingUtilities.LogEvent(this + " Signed...\nWith certificate where CN = " + MyID +  "\nUsing machine store: " +  "\nOutput: "+toAdd.OuterXml);
			return toAdd;
		}

		public void GenerateApproval(string CertCN, bool UseMachineStore) 
		{
			if (CertCN == null)
				CertCN = ConfigurationSettings.AppSettings["SigningCertificate"];

			X509SecurityToken cert = PennRoutingUtilities.GetSecurityToken(CertCN,UseMachineStore);
			MyID = PennRoutingUtilities.GetCertEmail(cert.Certificate);


			if (Delegates != null && Delegates.Length > 0) 
			{
				PennRoutingUtilities.LogEvent("Adding delegation for " + Delegates);
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


			string datetime = PennRoutingUtilities.GetCurrentTime();
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
	public class Approvals 
	{
		SignatureList _req, _rec;
		
		public Approvals() 
		{
			_req = new SignatureList();
			_rec = new SignatureList();
		}
		
		public SignatureList Required 
		{
			get 
			{
				return _req;
			}

			set 
			{
				_req = value;
			}
		}

		public SignatureList Received 
		{
			get 
			{
				return _rec;
			}
			set 
			{
				_rec = value;
			}
		}

		public Signature[] GetSignatureByName(string name) 
		{
			ArrayList a = new ArrayList();
			for (int j = 0; j < Received.Count; j++) 
			{
				Signature s = (Signature)((Signature)Received[j]).Clone();
				if (s.User != null && s.User.Equals(name))
					a.Add(s);
			}
			for (int j = 0; j < Required.Count; j++) 
			{
				Signature s = (Signature)((Signature)Required[j]).Clone();
				if (s.User != null && s.User.Equals(name))
					a.Add(s);
			}

			return (Signature[])a.ToArray(typeof(Signature));
		}

		public Signature[] GetDelegates(string addingID)
		{
			ArrayList a = new ArrayList();
			for (int j = 0; j < Received.Count; j++) 
			{
					Signature s = (Signature)((Signature)Received[j]).Clone();
					if (s.AddedBy != null && s.AddedBy.Equals(addingID))
						a.Add(s);
			}
			for (int j = 0; j < Required.Count; j++) 
			{
				Signature s = (Signature)((Signature)Required[j]).Clone();
				if (s.AddedBy != null && s.AddedBy.Equals(addingID))
					a.Add(s);
			}

			return (Signature[])a.ToArray(typeof(Signature));
		}

		public Signature[] GetDelegates(string[] delegates, string addingID) 
		{
			if (delegates == null || delegates.Length == 0)
				return new Signature[0];
			Signature[] sigs = new Signature[delegates.Length];
			for (int i = 0; i < delegates.Length; i++) 
			{
				for (int j = 0; j < Received.Count; j++) 
				{
					Signature s = (Signature)((Signature)Received[j]).Clone();
					if (s.AddedBy != null && s.AddedBy.Equals(addingID) && s.User.Equals(delegates[i]))
						sigs[i] = s;
				}

				for (int j = 0; j < Required.Count; j++) 
				{
					Signature s = (Signature)((Signature)Required[j]).Clone();
					if (s.AddedBy != null && s.AddedBy.Equals(addingID) && s.User.Equals(delegates[i]))
						sigs[i] = s;
				}

			}
			return sigs;
		}

	}

	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securitylab.cis.upenn.edu/AuthorizedRouting")]
	public class Signature : ICloneable, IComparable
	{
		string _usr, _prereq, _addedby, _datetime;
		XmlElement _sig;

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

		public Signature (string user, string addedby) 
		{
			User = user;
			AddedBy = addedby;
		}

		public Signature (string user) 
		{
			User = user;
		}

		public Signature () {} 

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

		public override string ToString() 
		{
			string s = this.GetType().FullName + "\n\tUser: " + User + "\n\tPrerequisite: " + Prerequisite + "\n\tTimestamp: " + Timestamp;
			return s;
		}

		public Microsoft.Web.Services.Security.X509.X509Certificate GetCertificate () 
		{
			if (Value != null) 
			{
				Microsoft.Web.Services.Security.X509.X509Certificate cert = new Microsoft.Web.Services.Security.X509.X509Certificate( Convert.FromBase64String((Value.GetElementsByTagName("X509Certificate")[0]).InnerText));
				return cert;
			}
			return null;
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

	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securitylab.cis.upenn.edu/AuthorizedRouting")]
	public class MappedItem 
	{

		public event MappingChangedEvent MappingChanged;
		public delegate void MappingChangedEvent(MappedItem m);

		private string _usr = null;
		private string _mpb = null;
		private string _name;
		private Signature _sig = null;

		[System.Xml.Serialization.XmlIgnore()]
		public Signature Signature 
		{
			get 
			{
				return _sig;
			}
			set 
			{
				_sig = value;
				if (MappingChanged != null)
					MappingChanged.DynamicInvoke(new object[] {this});
			}
		}

		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string User 
		{
			get 
			{
				return _usr;
			}
			set 
			{
				_usr = value;
				if (MappingChanged != null)
					MappingChanged.DynamicInvoke(new object[] {this});
			}
		}

		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Name
		{
			get 
			{
				return _name;
			}
			set 
			{
				_name = value;
			}
		}


		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string MappedBy
		{
			get 
			{
				return _mpb;
			}
			set 
			{
				_mpb = value;
				if (MappingChanged != null)
					MappingChanged.DynamicInvoke(new object[] {this});
			}
		}

		[System.Xml.Serialization.XmlIgnore()]
		public string Empty 
		{
			get 
			{
				return "";
			}
			set 
			{
			}
		}

		public MappedItem() {}
		public MappedItem (string name, string user, string mappedby) 
		{
			Name = name;
			User = user;
			MappedBy = mappedby;
		}

		public override string ToString() 
		{
			string s= this + "\nName=" + Name + "\nUser=" + User + "\nMappedBy=" + MappedBy + "Signature=" + Signature;
			return s;
		}
	}
}

