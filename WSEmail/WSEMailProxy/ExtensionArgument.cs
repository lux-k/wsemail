/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Collections;
using System.Xml;

namespace WSEmailProxy
{
	/// <summary>
	/// Creates a hopefully easier to use front-end.
	/// </summary>
	public class ExtensionArgument
	{
		private string _metname;
		private XmlDocument doc = new XmlDocument();
		private XmlElement root = null;
		private XmlElement args = null;
		private Hashtable hash = new Hashtable();

		/// <summary>
		/// The methodname, or action, to execute in the plugin.
		/// </summary>
		public string MethodName 
		{
			get 
			{
				if (_metname == null)
					return "";
				else
					return _metname;
				
			}
			set 
			{
				_metname = value;
				if (root != null) 
				{
					XmlElement temp = root;
					root = doc.CreateElement(_metname);
					foreach (XmlElement e in temp.ChildNodes)
						root.AppendChild(e);
				} 
				else 
					root = doc.CreateElement(_metname);
			}
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ExtensionArgument()
		{

		}

		/// <summary>
		/// Gets or sets the parameter and its value.
		/// </summary>
		public string this[string key] 
		{
			get 
			{
				return GetArgument(key);
			}
			set 
			{
				this.AddArgument(key,value);
			}
		}

		/// <summary>
		/// Creates an extension argument from an XML element and parses it into keys and values.
		/// </summary>
		/// <param name="x"></param>
		public ExtensionArgument(XmlElement x) 
		{
			if (x != null) 
			{
				this.MethodName = x.LocalName;
				XmlNodeList l = x.GetElementsByTagName("Arguments");
				if (l.Count > 0) 
				{
					XmlElement args = (XmlElement)l[0];

					foreach (XmlNode c in args.ChildNodes)
						this.AddArgument(c.LocalName,c.InnerText);
				}
			}
		}

		/// <summary>
		/// Adds a key and value to the extension.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="val"></param>
		public void AddArgument(string key, string val) 
		{
			if (hash[key] != null)
				hash[key] = val;
			else
				hash.Add(key,val);

		}

		/// <summary>
		/// Gets a value for a specific key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string GetArgument(string key) 
		{
			return (string)hash[key];
		}

		/// <summary>
		/// Sets the extension arguments
		/// </summary>
		/// <param name="extname"></param>
		public ExtensionArgument(string extname) 
		{
			this.MethodName=extname;
		}

		public ICollection Arguments 
		{
			get 
			{
				return hash.Keys;
			}
		}

		/// <summary>
		/// Returns the XML representation of the current object
		/// </summary>
		/// <returns></returns>
		public XmlElement AsXmlElement() 
		{
			if (args != null)
				args.ParentNode.RemoveChild(args);

			args = doc.CreateElement("Arguments");
		
			foreach (object o in hash.Keys) 
			{
				string s = (string)o;
				XmlElement temp = doc.CreateElement(s);
				if (((string)hash[o]) != null && !( (string)hash[o] ).Equals(""))
					temp.InnerText = (string)hash[o];
				args.AppendChild(temp);
			}
			root.AppendChild(args);
			return root;
		}
	}
}
