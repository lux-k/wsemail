using System;
using System.Xml;
using System.Xml.Serialization;

namespace XACLPolicy
{
	/// <summary>
	/// Summary description for XObject.
	/// </summary>
	public class XObject
	{
		[System.Xml.Serialization.XmlAttribute]
		public string href;	// href is an expression of XPath

		/*
		 * <remark/>
		 * We need a more flexible method to indicate the XPath experssion of href.
		 */
		
		public XObject()
		{
			//
			// TODO: Add constructor logic here
			//
		}

	}
}
