using System;
using System.Xml;
using WSEmailProxy;
using DistributedAttachment;

namespace WSEmailProxy
{
	/// <summary>
	/// Summary description for WSMessageAttach.
	/// </summary>
	/// 
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securitylab.cis.upenn.edu/WSEmail")]
	[Serializable]
	public class WSMessageAttach : WSEmailMessage
	{
		public WSMessageAttach() 
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// The Attachments of the message
		/// </summary>
		public FileAttachment[] Fileattachments;

		public DistributedAttachment.Policy policy;


	}
}
