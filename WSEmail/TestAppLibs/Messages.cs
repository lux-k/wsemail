/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System.Diagnostics;
using System;
using System.Threading;
using WSEmailProxy;
using PennLibraries;
using System.Xml;
using System.Configuration;
using Microsoft.Web.Services2.Security;



namespace TestAppLibs
{
	public class Messages : ProbabilisticObject 
	{
		public WSEmailMessage Get() 
		{
			WSEmailMessage msg = (WSEmailMessage)base.GetObject();
			
			return msg;
		}
	}
	
	
	
	
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	/// 


	public class MessagesReader : IConfigurationSectionHandler 
	{
		public object Create(object parent, object configContext, System.Xml.XmlNode section) 
		{
			Messages msg = new Messages();
			
			string machineName = "";
			if (configContext != null) 
			{
				try 
				{
					machineName = ((string)configContext).ToLower();
				} 
				catch {}
			}

			
			int sizeOfSubject=0;
			int sizeOfBody=0;
			try 
			{  
				foreach (XmlNode child in section.ChildNodes) 
				{
					if (child.Attributes != null) 
					{
						bool add = true;
						int weight = 1;
						if (child.Attributes["weight"] != null)
							weight = int.Parse(child.Attributes["weight"].Value);
						WSEmailMessage m = new WSEmailMessage();
						Console.WriteLine("\tWeight: " + child.Attributes["weight"].Value);
						if (child.Attributes["machine"] != null) 
						{
							if (machineName.Equals("") || !machineName.Equals(child.Attributes["machine"].Value.ToLower())) 
								add = false;
						}
						foreach (XmlNode grandchild in child.ChildNodes)
						{
							switch (grandchild.LocalName)
							{
								case "Recipients" :
								{ 
									foreach (XmlNode greatgrandchild in grandchild.ChildNodes)
									{
										if (greatgrandchild.NodeType != XmlNodeType.Whitespace) 
										{
											m.Recipients.AddRange( RecipientList.ParseRecipients(greatgrandchild.InnerText) );
											Console.WriteLine("yes" + greatgrandchild.OuterXml + greatgrandchild.LocalName);
										}
									}
									break;
								}

								case "Subject" :
								{
									sizeOfSubject = int.Parse(grandchild.Attributes["size"].Value);
									m.Subject = randomMessage(sizeOfSubject);
									break;
								}

								case "Body" :
								{
									sizeOfBody = int.Parse(grandchild.Attributes["size"].Value);
									m.Body = randomMessage(sizeOfBody);
									break;
								}
							}
						}
						if (add)
							msg.AddObject(m,weight);
					}
				}
			}
			catch (System.Exception caught)
			{ 
				Console.WriteLine(caught.Message);
			}
			return msg;
		}
	
		private string randomMessage(int i)
		{
			
			System.Text.StringBuilder b = new System.Text.StringBuilder(1024);

			Random r = new Random();
			for (int j=0;j<1024;j++)
			{
				b.Append(((char)r.Next(65,90)));
			}

			System.Text.StringBuilder c = new System.Text.StringBuilder(i);
			for (int j=0; j< (i/1024); j++) 
				c.Append(b.ToString());

			if (c.ToString().Length != i)
				c.Append(b.ToString().Substring(0,i-c.ToString().Length));

			return c.ToString();
		}
	}
}