/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System.IO;
using System.Reflection;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System;
using PennLibraries;

namespace DynamicForms
{
	/// <summary>
	/// Loads, serializes, deserializes and maintains dynamic classes
	/// </summary>
	public class ObjectLoader 
	{
		/// <summary>
		/// Filename where the Xml manifest of available classes will be listed
		/// </summary>
		public const string ASSEMBLYXMLFILE = "Assemblies.xml";

		/// <summary>
		/// Serializes an object of a certain type to a memory stream.
		/// </summary>
		/// <param name="t">System.Type to serialize o as (usually o.GetType())</param>
		/// <param name="o">Object to serialize</param>
		/// <returns>Memory stream holding the string of the serialized form</returns>
		public static MemoryStream Serialize(System.Type t, object o) 
		{
			XmlSerializer xs = new XmlSerializer(t);
			MemoryStream ms = new MemoryStream();
			//Console.WriteLine("This = " + o.GetType().Name);
			xs.Serialize(ms,o);
			ms.Position = 0;
			return ms;
		}

		/// <summary>
		/// Converts the contents of a memory stream into an XmlDocument
		/// </summary>
		/// <param name="ms">Memory stream</param>
		/// <returns>XmlDocument</returns>
		public static XmlDocument MemoryStreamToXmlDocument(MemoryStream ms) 
		{
			XmlDocument d = new XmlDocument();
			d.Load(ms);
			if (d.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
				d.RemoveChild(d.FirstChild);
			return d;
		}
		/// <summary>
		/// Given an array of XmlNodes, will search out the object configuration subnodes and return just them. Useful
		/// for displaying a list of the available forms in a document.
		/// </summary>
		/// <param name="x">Xml source node</param>
		/// <returns>Array of configurations</returns>
		public static ObjectConfiguration[] GetFormInventory(XmlNode[] x) 
		{
			ArrayList a = new ArrayList();
			foreach (XmlNode d in x) 
			{
				a.Add(LoadConfiguration(d));
			}
			return (ObjectConfiguration[])a.ToArray(typeof(ObjectConfiguration));
		}

		/// <summary>
		/// Converts a string into a memory stream. Useful for deserializing objects.
		/// </summary>
		/// <param name="s">String to serialize</param>
		/// <returns>Memory stream</returns>
		public static MemoryStream StringToMemoryStream(string s) 
		{
			MemoryStream ms = new MemoryStream();
			byte[] bytes = System.Text.Encoding.ASCII.GetBytes(s);
			ms.Write(bytes,0,bytes.Length);
			ms.Position = 0;
			return ms;
		}

		/// <summary>
		/// Attempts to load an object given an XmlNode/document that describes it.
		/// </summary>
		/// <param name="x">Xml source document</param>
		/// <returns>BaseObject</returns>
		public static BaseObject LoadObject(XmlDocument x) 
		{
			return LoadObject(x.OuterXml);
		}

		/// <summary>
		/// Deserializes a certain type from a string.
		/// </summary>
		/// <param name="type">Type to try and deserialize</param>
		/// <param name="s">Xml string</param>
		/// <returns>Object</returns>
		public static Object Deserialize(System.Type type, string s) 
		{
			XmlSerializer xs = new XmlSerializer(type);
			return xs.Deserialize(StringToMemoryStream(s));
		}

		/// <summary>
		/// Loads an ObjectConfiguration from a certain XPath query node
		/// </summary>
		/// <param name="i">Iterator to load from</param>
		/// <returns>ObjectConfiguration</returns>
		public static ObjectConfiguration LoadConfiguration(XPathNodeIterator i) 
		{
			ObjectConfiguration c = new ObjectConfiguration();
			do 
			{
				Console.WriteLine("Hello, in outer switch the name = " + i.Current.LocalName);
				switch (i.Current.LocalName) 
				{
					case "Configuration":
						Console.WriteLine("Landed in configuration section.");
						XPathNodeIterator x = i.Clone();
						bool go = x.Current.MoveToFirstAttribute();
						while (go) 
						{
							switch (x.Current.LocalName) 
							{
								case "Version":
									c.Version = float.Parse(x.Current.Value);
									break;
								case "Name":
									c.Name = x.Current.Value;
									break;
							}
							go = x.Current.MoveToNextAttribute();
						}
						//Console.WriteLine("Name set to: " + c.Name);
						//Console.WriteLine("Version set to: " + c.Version.ToString());
						break;
					case "Url":
						c.Url = i.Current.Value;
						break;
					case "DLL":
						c.DLL = i.Current.Value;
						break;
					case "FriendlyName":
						c.FriendlyName = i.Current.Value;
						break;
					case "Description":
						c.Description = i.Current.Value;
						break;
				}

			} while (i.MoveNext());
			return c;
		}

		/// <summary>
		/// Loads a configuration from an XmlNode
		/// </summary>
		/// <param name="d">Xml source node</param>
		/// <returns>Object configuration</returns>
		public static ObjectConfiguration LoadConfiguration(XmlNode d) 
		{
			return LoadConfiguration(d.OuterXml);
		}

		/// <summary>
		/// Loads a configuration from a string
		/// </summary>
		/// <param name="s">Xml source string</param>
		/// <returns>Object configuration</returns>
		public static ObjectConfiguration LoadConfiguration(string s) 
		{
			XPathDocument xp = new XPathDocument(StringToMemoryStream(s));
			XPathNavigator xn = xp.CreateNavigator();
			XPathNodeIterator it = xn.Select("//Configuration/descendant-or-self::*");
			
			ObjectConfiguration oc = null;

			if (it.MoveNext())
				oc = LoadConfiguration(it);

			return oc;
		}

		/// <summary>
		/// Given an xml string, this function will try to load the object configuration out of it, load the library
		/// referenced by the configuration, create an instance of that object and populate it from the rest of the
		/// xml data.
		/// </summary>
		/// <param name="s">Xml source string</param>
		/// <returns>BaseObject</returns>
		public static BaseObject LoadObject(string s) 
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
			
			ObjectConfiguration oc = LoadConfiguration(s);
			
			BaseObject baseref = null;
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
					LogEvent(LogType.Error,e.Message);
					
					DownloadAssembly(oc.Url,oc.DLL);
					VerifyAssembly(oc.DLL);
					UpdateAssemblyList(oc);
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

		private static void LogEvent(LogType t, string s) 
		{
			Utilities.LogEvent(t,s);
		}
		/// <summary>
		/// Downloads a file from url and saves it as "asmname.dll"
		/// </summary>
		/// <param name="url">Url to download from</param>
		/// <param name="asmname">Name of filename to save as</param>
		private static void DownloadAssembly(string url, string asmname) 
		{
			System.Net.WebClient wc = new System.Net.WebClient();
			Console.WriteLine("Downloading DLL " + asmname + ".dll" + " from " + url);
			wc.DownloadFile(url,asmname+".dll");
			Console.WriteLine("Download complete...");
		}

		/// <summary>
		/// Gets the current version (ie the version that the is recorded in the library manifest) for a specified configuration
		/// </summary>
		/// <param name="oc">Object configuration</param>
		/// <returns>Current version or -1 for unavailable</returns>
		public static float GetCurrentVersion(ObjectConfiguration oc) 
		{
			return GetCurrentVersion(oc.Name);
		}

		/// <summary>
		/// Gets the current version (ie the version that the is recorded in the library manifest) for a specified configuration
		/// </summary>
		/// <param name="name">Namespace.name of object to get</param>
		/// <returns>Current version or -1 for unavailable</returns>
		public static float GetCurrentVersion(string name) 
		{
			XPathDocument xp = new XPathDocument(ObjectLoader.ASSEMBLYXMLFILE);
			XPathNavigator xn = xp.CreateNavigator();
			//@*[name() !='excludeThisOne']
			XPathNodeIterator it = xn.Select("/Libraries/ObjectConfiguration[@Name='" + name + "']/@Version");
			
			if (it.MoveNext()) 
			{
				Console.WriteLine("Able to it.MoveNext().");
				Console.WriteLine("Value = " + it.Current.Value);
				it.Current.MoveToFirstAttribute();
			{
				Console.WriteLine("Moved to attribute " + it.Current.LocalName);
				if (it.Current.LocalName.Equals("Version"))
					return float.Parse(it.Current.Value);
			} while (it.Current.MoveToNextAttribute());
				return -1;
			}
			else 
			{
				Console.WriteLine("Oops! Unable to it.MoveNext().");
				return -1;
			}
		}
		/// <summary>
		/// Attempts to verify the signature on a downloaded assembly.
		/// </summary>
		/// <param name="asmname">The DLL name (sans the ending .dll)</param>
		private static void VerifyAssembly(string asmname) 
		{
			asmname += ".dll";
			CAPICOM.SignedCodeClass scc = new CAPICOM.SignedCodeClass();
			scc.FileName = asmname;
			scc.Verify(true);
		}

		/// <summary>
		/// Gets a list of available objects from the assembly xml file.
		/// </summary>
		/// <returns></returns>
		public static ObjectConfiguration[] GetAvailableObjects() 
		{
			Console.WriteLine(Directory.GetCurrentDirectory());
			if (File.Exists(ObjectLoader.ASSEMBLYXMLFILE)) 
			{
				XmlDocument xd = new XmlDocument();
				ObjectConfiguration[] objs = null;
				try 
				{
					xd.Load(ObjectLoader.ASSEMBLYXMLFILE);
					Console.WriteLine("opening " + ObjectLoader.ASSEMBLYXMLFILE);
					objs = new ObjectConfiguration[xd["Libraries"].ChildNodes.Count];
					Console.WriteLine("There are " + objs.Length.ToString() + " objs");
				} 
				catch (Exception e) 
				{
					Console.WriteLine("There was an exception: " + e.Message);
					return null;
				}
				int ptr = 0;
				foreach (XmlNode x in xd["Libraries"]) 
				{
					objs[ptr++] = ObjectConfiguration.Deserialize(x);
				}
				return objs;
			} 
			else 
			{
				return null;
			}
		}

		/// <summary>
		/// Scans a DLL and looks for any objects that conform to the spec necessary to run under this system. Reports back
		/// object configurations of those qualifying objects.
		/// </summary>
		/// <param name="f">Filename (full filename, with extensions)</param>
		/// <returns></returns>
		public static ObjectConfiguration[] ScanLibrary(string f) 
		{
			ArrayList o = new ArrayList();
			if (!File.Exists(f))
				throw new Exception("File does not exist!");
            
			Assembly a = Assembly.LoadFrom(f);
			Type[] types = a.GetTypes();
			foreach (System.Type t in types) 
			{
				//				Console.WriteLine("Type: " + t.FullName);
				//				Console.WriteLine("Is a class: " + t.IsClass.ToString());
				//				Console.WriteLine("Is an interface: " + t.IsInterface.ToString());
				if (t.BaseType != null) 
				{
					//					Console.WriteLine("Directly inherits from: " + t.BaseType.FullName);
					if (t.BaseType.FullName.Equals("DynamicForms.BaseObject")) 
					{
						Console.WriteLine("Implements the BaseObject architecture: true");
						Console.WriteLine();
						Console.WriteLine("Found new useable object: " + t.FullName);
						BaseObject b = (BaseObject)a.CreateInstance(t.FullName);
						o.Add(b.Configuration);
						Console.WriteLine("Configuration: " + b.Configuration);
						Console.WriteLine();
					}
				}
			}
			return (ObjectConfiguration[])o.ToArray(typeof(ObjectConfiguration));
		}

		/// <summary>
		/// Checks the dependencies of a configuration and attempts to download missing dependencies.
		/// </summary>
		/// <param name="c">Config to check</param>
		public static void CheckDependencies(ObjectConfiguration c) 
		{
			CheckDependencies(c.Dependencies);

			if  (!File.Exists(c.DLL + ".dll")) 
			{
				Console.WriteLine("Unsatisfied dependency: " + c.Name);
				DownloadAssembly(c.Url,c.DLL);
			}

		}

		/// <summary>
		/// Checks an array of dependencies
		/// </summary>
		/// <param name="oc"></param>
		public static void CheckDependencies(ObjectConfiguration[] oc) 
		{
			if (oc == null || oc.Length == 0)
				return;
			
			foreach (ObjectConfiguration c in oc)
				CheckDependencies(c);
				
		}

		/// <summary>
		/// Loads an instance of a particular object, given the object configuration
		/// </summary>
		/// <param name="oc"></param>
		/// <returns></returns>
		public static BaseObject LoadObject(ObjectConfiguration oc) 
		{
			LogEvent(LogType.Debug, "LoadObject(oc): " + ConfigurationSettings.AppSettings["IgnoreDynamicFormVersioning"]);
			if (ConfigurationSettings.AppSettings["IgnoreDynamicFormVersioning"] != null && ConfigurationSettings.AppSettings["IgnoreDynamicFormVersioning"].Equals("false")) 
			{
				LogEvent(LogType.Debug, "LoadObject(oc): Checking versions...");
				float f = GetCurrentVersion(oc);
				if (f < oc.Version)
					throw new Exception("There is a newer version of the current form available. Would you like to get it? (Version " + f.ToString() + " vs " + oc.Version.ToString()+")");

				CheckDependencies(oc.Dependencies);
			} else
				LogEvent(LogType.Debug, "Ignoring dynamic form versions and dependency check per configuration directive.");


			if (oc.Name == null)
				Console.WriteLine("oc.Name == null");
			else
				Console.WriteLine("oc.Name != null");

			LogEvent(LogType.Debug, "Current directory is: " + Directory.GetCurrentDirectory());
			LogEvent(LogType.Debug, "Current directory is: " + Assembly.GetExecutingAssembly().Location);
			
			Assembly a;
			if (ConfigurationSettings.AppSettings["ForceLibraryLoadDirectory"] != null)
                a = Assembly.LoadFrom(ConfigurationSettings.AppSettings["ForceLibraryLoadDirectory"] + 
					oc.DLL + ".dll");
				else
				a = Assembly.LoadFrom(oc.DLL + ".dll");


			
			if (a == null) 
			{
				Console.WriteLine("Unable to load assembly " + oc.DLL + ".dll");
				LogEvent(LogType.Error, "Unable to load " + oc.DLL + ".dll");
				return null;
			}
			//			System.Type[] ts = a.GetTypes();
			//			for (int i = 0; i < ts.Length; i++)
			//				Console.WriteLine(ts.GetType().Name);
			System.Type t = a.GetType(oc.Name);
			if (t == null) 
			{
				Console.WriteLine("Unable to load type " + oc.Name + " from " + oc.DLL);
				LogEvent(LogType.Error, "Unable to get type " + oc.Name + " fromn " + oc.DLL);
				return null;
			}
		
			BaseObject baseref = (BaseObject)Activator.CreateInstance(t);
			Console.WriteLine("Loaded object successfully.");
			return baseref;
		}

		/// <summary>
		/// Adds or updates the configurations available in the xml index file
		/// </summary>
		/// <param name="oc">Config to update or add</param>
		public static void UpdateAssemblyList(ObjectConfiguration oc) 
		{
			XmlDocument d = new XmlDocument();
			XmlNode insert = null;
			if (File.Exists(ObjectLoader.ASSEMBLYXMLFILE)) 
			{

				try 
				{
					d.Load("Assemblies.xml");
				} 
				catch {}
					
				XmlNodeList nl = d.GetElementsByTagName("Libraries");

				if (nl.Count > 0) 
				{
					// the libraries section exists
					XmlNode n = nl[0];
					foreach (XmlNode xn in n) 
					{
						if (xn.Attributes["Name"].Value == oc.Name) 
						{
							insert = xn;
							break;
						}
					}
					if (insert != null)
						insert.ParentNode.RemoveChild(insert);
				} 
				else
					// it doesnt exist, so create it.
					d.AppendChild(d.ImportNode(d.CreateNode(XmlNodeType.Element,"Libraries",""),true));
			} 
			else 
			{
				d = new XmlDocument();
				d.AppendChild(d.ImportNode(d.CreateNode(XmlNodeType.Element,"Libraries",""),true));
			}
			
			d["Libraries"].AppendChild(d.ImportNode(oc.SerializeConfiguration(),true));
			d.Save(ObjectLoader.ASSEMBLYXMLFILE);

		}
	}
}
