using System;
using System.Xml;
using System.Xml.Serialization;

namespace XACLPolicy
{
	/// <summary>
	/// Summary description for Xacl.
	/// </summary>

	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.trl.ibm.com/projects/xml/xacl")]
	[Serializable]

	public class Xacl
	{

		/* <remark/>
		 * xacl element has 1 or multiple sub-element of object and rule.
		 * We need extend this class.
		 * */


		[System.Xml.Serialization.XmlElement(ElementName="object")]
		public XObject xobject;
		public Rule rule;


		public Xacl()
		{
			//
			// TODO: Add constructor logic here
			//
			//actionRead.name = 
		}
	}
}
