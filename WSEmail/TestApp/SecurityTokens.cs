using System;
using System.Threading;
using WSEmailProxy;
using PennLibraries;
using System.Xml;
using System.Configuration;
using Microsoft.Web.Services.Security;

using System.Collections;

namespace TestApp
{
	public class SecurityTokens : ProbabilisticObject 
	{
		public SecurityToken Get() 
		{
			SecurityToken s = (SecurityToken)base.GetObject();
			if (s is UsernameToken)
				s = new UsernameToken( ((UsernameToken)s).Username,((UsernameToken)s).Password,PasswordOption.SendNone);

			//Console.WriteLine("Returning a " + s.ToString());

			return s;
		}
	}

	public class SecurityTokensReader : IConfigurationSectionHandler 
	{
		public object Create(object parent, object configContext, System.Xml.XmlNode section) 
		{
			SecurityTokens sectoks = new SecurityTokens();
			Console.WriteLine("SecurityTokensReader reading config...");
			try 
			{
				foreach (XmlNode child in section.ChildNodes) 
				{
					if (child.Attributes != null) 
					{
						int weight = 1;
						//						Console.WriteLine(child.Attributes.Count.ToString());
						//						foreach (XmlNode x in child.ChildNodes)
						//							Console.WriteLine(x.Name + " " + x.Value);
						Console.WriteLine("Loading..." + child.Attributes["name"].Value + " token...");

						if (child.Attributes["weight"] != null)
							weight = int.Parse(child.Attributes["weight"].Value);

						Console.WriteLine("\tWeight: " + child.Attributes["weight"].Value);
						if (child.Attributes["type"] != null) 
						{
							SecurityToken tok = null;
							switch (child.Attributes["type"].Value) 
							{
								case "UsernamePassword":
								{
									string user = "", pass = "";
									XmlElement el = (XmlElement)child;
									XmlNodeList l = el.GetElementsByTagName("Username");
									if (l.Count > 0)
										user = l[0].InnerText;
									l = el.GetElementsByTagName("Password");
									if (l.Count > 0)
										pass = l[0].InnerText;
									tok = new UsernameToken(user,pass,PasswordOption.SendPlainText);
									Console.WriteLine("\tUsername: " + user);
									break;
								}
								case "X509":
								{
									string certcn = "";
									bool mach = false;
									XmlElement el = (XmlElement)child;
									XmlNodeList l = el.GetElementsByTagName("CertCN");
									if (l.Count > 0)
										certcn = l[0].InnerText;

									l = el.GetElementsByTagName("Store");
									if (l.Count > 0) 
									{
										string s = l[0].InnerText;
										if (s.Equals("System"))
											mach = true;
									}
									try 
									{
										tok = Utilities.GetSecurityToken(certcn,mach);
										Console.WriteLine("\tCertCN: " + certcn);
										Console.WriteLine("\tMachine Store: " + mach.ToString());
									} 
									catch (Exception e) 
									{
										Console.WriteLine(e.Message);
									}
									break;
								}
							}
							if (tok != null) 
							{
								Console.WriteLine("\tToken added to collection.");
								sectoks.AddObject(tok,weight);
							}

						}
					}
				}
				return sectoks;
			} 
			catch (Exception e) 
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
			}
			return null;
		}

	}
}
