using System;
using System.Xml;
using System.Xml.Serialization;


namespace XACLPolicy
{
	/// <summary>
	/// Summary description for Subject.
	/// </summary>

	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.trl.ibm.com/projects/xml/xacl")]
	[Serializable]

	public class Subject
	{
		public Subject()
		{
			//
			// TODO: Add constructor logic here
			//
			uid = null;
		}

		[System.Xml.Serialization.XmlElement]
		public string uid;

		// In the future, we need add fields for role and group
		
		/// <summary>
		/// Whether the searched uid is in the curret subject description.
		/// In the future,we shall extend to group and role
		/// </summary>
		/// <param name="uid">The searched uid</param>
		/// <returns></returns>
		public bool contains(string vuid)
		{
			if(vuid.Equals(uid))
				return true;
			else
				return false;
		}
	}
}
