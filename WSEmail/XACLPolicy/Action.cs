using System;
using System.Xml;
using System.Xml.Serialization;

namespace XACLPolicy
{
	/// <summary>
	/// Summary description for Action.
	/// </summary>
	

	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.trl.ibm.com/projects/xml/xacl")]
	[Serializable]
	public class Action
	{

		public enum OperationType
		{
			read,write,create,delete
		};

		public enum PermissionType
		{
			grant,deny
		};

		[System.Xml.Serialization.XmlAttribute]
		public string name;

		[System.Xml.Serialization.XmlAttribute]
		public string permission;
		public Action()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public Action(Action.OperationType oper,Action.PermissionType pt)
		{
			name = oper.ToString();
			permission = pt.ToString();
		}
	}
}
