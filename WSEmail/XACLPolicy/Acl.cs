using System;

namespace XACLPolicy
{
	/// <summary>
	/// Summary description for Acl.
	/// </summary>
	public class Acl
	{
		[System.Xml.Serialization.XmlElement]
		public Subject subject;

		[System.Xml.Serialization.XmlElement(ElementName = "action")]
		public Action actionRead;
		

		/*
		 * <remark/>
		 * rule element has 1 or multiple sub-elements acl.We need extend this class.
		 * We need extend this class.
		 * */



		/*[System.Xml.Serialization.XmlElement(ElementName = "action")]
		public Action actionWrite;


		[System.Xml.Serialization.XmlElement(ElementName = "action")]
		public Action actionCreate;

		[System.Xml.Serialization.XmlElement(ElementName = "action")]
		public Action actionDetele;*/

		public Acl()
		{
			//
			// TODO: Add constructor logic here
			//
			actionRead = null;
			subject = null;
			//actionWrite = null;
			
		}
	}
}
