using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.IO;

using XACLPolicy;
namespace AllTest
{
	/// <summary>
	/// Summary description for Book.
	/// </summary>
	[System.Xml.Serialization.XmlTypeAttribute]
	[System.Xml.Serialization.XmlInclude(typeof(XACLPolicy.Subject))]
	[Serializable]
	public class Book
	{


		public enum LogType 
		{
			Unknown, Error, MessageSent, MessageReceived, UserAuthentication, UserAuthenticationError,
			Debug, MessageAuthentication, MessageAuthenticationError, ServerInfo, ServerDebug, Informational,
			ServerStatus, ServerDeliveryStatus, ServerError, ServerWarning, RequestStart, RequestEnd};

		[System.Xml.Serialization.XmlElement(ElementName = "TaxRate")]
		public string ISBN;

		[System.Xml.Serialization.XmlElement(ElementName = "TaxRate1")]
		public string ISBN1;

		[System.Xml.Serialization.XmlAttribute]
		public string bookname;


		public ArrayList authors;
		
		public Book()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		public void writeOut()
		{
			System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(this.GetType(),"http://mytest/");
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			xs.Serialize(ms,this);
			ms.Position = 0;

			XmlDocument d = new XmlDocument();
			d.PreserveWhitespace = false;
			d.Load(ms);
			
			Console.WriteLine("The xml doc is:");
			Console.WriteLine(d.OuterXml);
			
		}
	}
}
