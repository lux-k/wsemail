/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;


namespace DynamicBizObjects
{
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
