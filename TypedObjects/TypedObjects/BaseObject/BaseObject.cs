using System.Resources;
using System.Xml.Serialization;
using System.Reflection;
using System;
using System.Net;
using System.Text;
using System.IO;
using System.Runtime.Remoting;
using System.Xml.XPath;
using System.Xml;

namespace BaseObject
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public abstract class Base
	{
		public Base()
		{
			this.Configuration = new ObjectConfiguration();
			this.Configuration.Name = this.GetType().FullName;
		}

		public ObjectConfiguration Configuration;
		public abstract string DoYourThing();

		public XmlDocument AsXmlDocument() 
		{
			return StringToXmlDocument(AsXmlString());
		}

		public Base LoadFrom(string s) 
		{
			XmlSerializer xs = new XmlSerializer(this.GetType());
			return (Base)xs.Deserialize(ObjectLoader.StringToMemoryStream(s));
		}
			
		public XmlDocument StringToXmlDocument(string s) 
		{
			XmlDocument xd = new XmlDocument();
			xd.LoadXml(s);
			if (xd.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
				xd.RemoveChild(xd.FirstChild);

			return xd;
		}

		public string AsXmlString() 
		{
			XmlSerializer xs = new XmlSerializer(this.GetType());
			MemoryStream ms = new MemoryStream();
			xs.Serialize(ms,this);
			return Encoding.ASCII.GetString(ms.ToArray());
		}
	}

	public class ObjectConfiguration 
	{
		public string Url = null;
		public float Version = 0F;
		public string Name = null;

		public ObjectConfiguration() 
		{
		}
	}

	public class ObjectLoader 
	{
		public static MemoryStream StringToMemoryStream(string s) 
		{
			MemoryStream ms = new MemoryStream();
			byte[] bytes = Encoding.ASCII.GetBytes(s);
			ms.Write(bytes,0,bytes.Length);
			ms.Position = 0;
			return ms;
		}

		public static Base LoadObject(XmlDocument x) 
		{
			return LoadObject(x.OuterXml);
		}

		public static ObjectConfiguration LoadConfiguration(XPathNodeIterator i) 
		{
			Console.WriteLine("Landed in LoadConfiguration");
			ObjectConfiguration c = new ObjectConfiguration();
			do 
			{
				switch (i.Current.LocalName) 
				{
					case "Url":
						c.Url = i.Current.Value;
						break;
					case "Name":
						c.Name = i.Current.Value;
						break;
					case "Version":
						//c.Version = float.Parse(i.Current.Value);
						c.Version = float.Parse(i.Current.Value);
						break;
				}
			} while (i.MoveNext());
			return c;
		}

		public static Base LoadObject(string s) 
		{
			// load the config
			// try to load the lib specified by the name
			// if that fail, try to download the lib
			// try to load the lib, again
			// if it loads, bind it to the base type
			// use baseclass methods to populate it
			// execute it?

			// XmlSerializer xs = new XmlSerializer(this.GetType());
			Console.WriteLine("Hello.");
			
			
			XPathDocument xp = new XPathDocument(StringToMemoryStream(s));
			XPathNavigator xn = xp.CreateNavigator();
			XPathNodeIterator it = xn.Select("//Configuration/*");
			
			ObjectConfiguration oc = null;

			if (it.MoveNext())
				oc = LoadConfiguration(it);

			Base baseref = null;
			if (oc != null) 
			{
				// try to load the object
				try 
				{
					baseref = LoadObject(oc);
				} 
				catch (Exception e) 
				{
					Console.WriteLine("Oops! Loading instance failed, presumably because the DLL can't be found.");
					Console.WriteLine(e.Message);
					DownloadAssembly(oc.Url,oc.Name);
					VerifyAssembly(oc.Name);
					Console.WriteLine("Attempting to reload...");
					baseref = LoadObject(oc);
				}
				baseref = baseref.LoadFrom(s);
			} 
			else 
			{
				Console.WriteLine("The configuration is null. That's bad.");
			}


			return baseref;
		}

		private static void DownloadAssembly(string url, string asmname) 
		{
			WebClient wc = new WebClient();
			Console.WriteLine("Downloading DLL " + asmname + ".dll" + " from " + url);
			wc.DownloadFile(url,asmname+".dll");
			Console.WriteLine("Download complete...");
		}

		private static void VerifyAssembly(string asmname) 
		{
			asmname += ".dll";
			CAPICOM.SignedCodeClass scc = new CAPICOM.SignedCodeClass();
			scc.FileName = asmname;
			scc.Verify(true);
		}

		private static Base LoadObject(ObjectConfiguration oc) 
		{
			Base baseref = (Base)Activator.CreateInstance(Assembly.LoadFrom(oc.Name + ".dll").GetType(oc.Name));
			Console.WriteLine("Loaded object successfully.");
			return baseref;
		}
	}
}
