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

namespace PennLibraries
{
	/// <summary>
	/// Summary description for SerializableObject.
	/// </summary>
	public abstract class SerializableObject
	{
		/// <summary>
		/// Serializes an object of a certain type to a memory stream.
		/// </summary>
		/// <param name="t">System.Type to serialize o as (usually o.GetType())</param>
		/// <param name="o">Object to serialize</param>
		/// <returns>Memory stream holding the string of the serialized form</returns>
		public MemoryStream Serialize() 
		{
			XmlSerializer xs = new XmlSerializer(this.GetType());
			MemoryStream ms = new MemoryStream();
			//Console.WriteLine("This = " + o.GetType().Name);
			xs.Serialize(ms,this);
			ms.Position = 0;
			return ms;
		}

		public virtual XmlDocument SerializeToXmlDocument() 
		{
			return MemoryStreamToXmlDocument(Serialize());
		}

		public string SerializeToBase64String() 
		{
			return Convert.ToBase64String(Serialize().ToArray());
		}

		/// <summary>
		/// Converts the contents of a memory stream into an XmlDocument
		/// </summary>
		/// <param name="ms">Memory stream</param>
		/// <returns>XmlDocument</returns>
		public XmlDocument MemoryStreamToXmlDocument(MemoryStream ms) 
		{
			XmlDocument d = new XmlDocument();
			d.Load(ms);
			if (d.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
				d.RemoveChild(d.FirstChild);
			return d;
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
	}
}
