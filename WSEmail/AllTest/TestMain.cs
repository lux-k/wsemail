using System;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using XACLPolicy;


namespace AllTest
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class TestMain
	{
		public TestMain()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void Main() 
		{

			XmlDocument doc = new XmlDocument();
			doc.LoadXml("<book genre='novel' ISBN='1-861001-57-5'>" +
				"<title>Pride And Prejudice</title>" +
				"</book>");

			XmlNode root = doc.DocumentElement;

			// OuterXml includes the markup of current node.
			Console.WriteLine("Display the OuterXml property...");
			Console.WriteLine(root.OuterXml);
            
			// InnerXml does not include the markup of the current node.
			// As a result, the attributes are not displayed.
			Console.WriteLine();
			Console.WriteLine("Display the InnerXml property...");
			Console.WriteLine(root.InnerXml);

			Book book = new Book();
			book.ISBN = "12343";
			book.ISBN1 = "5431XXX";
			book.bookname = "Learning C#";
			book.authors = new ArrayList();
			//book.authors[0] = "Arrow";
			//book.authors[1] = "Sabre";
			
			//book.authors[1] = Book.LogType.Error.ToString();
			//book.authors[2] = "xxx"; 
			
			book.authors.Add("arrow");

			XACLPolicy.Subject subject = new Subject();
			subject.uid = "arrow";
			book.authors.Add(subject);
			book.writeOut();

			Console.WriteLine("-------------------------------");

			Action xaction = new Action(Action.OperationType.read,Action.PermissionType.grant);
			
			Subject xsubject = new Subject();
			xsubject.uid = "Alice";

			Acl xxacl = new Acl();
			xxacl.actionRead = xaction;
			xxacl.subject = xsubject;

			Rule xrule = new Rule();
			xrule.acl = xxacl;

			XObject xobject = new XObject();
			xobject.href = "/contents";

			Xacl xacl = new Xacl();
			xacl.xobject = xobject;
			xacl.rule = xrule;

			Policy policy = new Policy();
			policy.xacl = xacl;

			policy.writeOut();

		}
	}
}
