using System;
using System.Xml;
using System.Xml.Serialization;

namespace XACLPolicy
{
	/// <summary>
	/// Summary description for Rule.
	/// </summary>

	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.trl.ibm.com/projects/xml/xacl")]
	[Serializable]
	public class Rule
	{

		public Acl acl;
		/* <remark/>
		 * rule element has 0 or multiple sub-element subject and 1 or multiple sub-elements action.
		 * We need extend this class.
		 * */


		public Rule()
		{
			//
			// TODO: Add constructor logic here
			//
			acl = null;
		}
	}
}
