using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace XACLPolicy
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.trl.ibm.com/projects/xml/xacl")]
	[Serializable]
	public class Policy
	{
		[System.Xml.Serialization.XmlElement]
		public Xacl xacl;

		//<remark/>
		/*
		 * There should be another property element.
		 * We will extend it later
		 */

		public Policy()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		/// <summary>
		/// This method is used for debuging. Print out the Policy document
		/// </summary>
		public void writeOut()
		{
			System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(this.GetType());
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			xs.Serialize(ms,this);
			ms.Position = 0;

			XmlDocument d = new XmlDocument();
			d.PreserveWhitespace = false;
			d.Load(ms);
			
			Console.WriteLine("The xml doc is:");
			Console.WriteLine(d.OuterXml);
			
		}
		/// <summary>
		/// Anaylize the policy, give the result of the action permission
		/// </summary>
		/// <param name="xObject">The Object of the Access Control</param>
		/// <param name="xSubject">The Subject of the Access Control</param>
		/// <param name="operType">Type of the operation that the client requests</param>
		/// <returns>The Result of the Action request</returns>
		public Action.PermissionType getPermission(XObject xObject, Subject xSubject, Action.OperationType operType)
		{
			// Currently, The xObject is fixed.So we only concern the Subject
			if (!xacl.rule.acl.subject.contains(xSubject.uid))
				return Action.PermissionType.deny;
			else
				if (xacl.rule.acl.actionRead.permission.Equals(Action.PermissionType.grant.ToString()))
					return Action.PermissionType.grant;
				else
					return Action.PermissionType.deny;
		}


	}
}
