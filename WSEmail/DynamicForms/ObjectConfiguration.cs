/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

namespace DynamicForms
{
	/// <summary>
	/// Defines the structure needed to load this object from a WSEmail client, when that client
	/// might know absolutely nothing about the actual data structure of the element.
	/// </summary>
	public class ObjectConfiguration 
	{
		/// <summary>
		/// Url where the class (DLL) needed to interpret this object can be found
		/// </summary>
		public string Url = null;
		/// <summary>
		/// The name of the class (DLL), without the ending .DLL
		/// </summary>
		public string DLL = null;
		/// <summary>
		/// The name of this object in the form of Namespace.Object
		/// </summary>
		[System.Xml.Serialization.XmlAttribute()]
		public string Name = null;
		/// <summary>
		/// The version of this object
		/// </summary>
		[System.Xml.Serialization.XmlAttribute()]
		public float Version = 0F;
		/// <summary>
		/// A friendly name such as "Timesheet" or "Purchase Order"
		/// </summary>
		public string FriendlyName = null;
		/// <summary>
		/// A slightly longer description of what this object does, such as "Allows you to get paid"
		/// </summary>
		public string Description = null;
		/// <summary>
		/// An array of dependencies this object might have.
		/// </summary>
		public ObjectConfiguration[] Dependencies;

		/// <summary>
		/// Default, boring constructor.
		/// </summary>
		public ObjectConfiguration() 
		{
		}

		/// <summary>
		/// Serializes this object configuration and all dependencies into an xml node. Used internally for update
		/// manifests of libraries, but made public because it might be useful.
		/// </summary>
		/// <returns>XmlNode of the configuration</returns>
		public XmlNode SerializeConfiguration () 
		{
			XmlDocument xd = ObjectLoader.MemoryStreamToXmlDocument(ObjectLoader.Serialize(this.GetType(),this));
			if (xd.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
				xd.RemoveChild(xd.FirstChild);
			return xd.FirstChild;	
		}

		/// <summary>
		/// Deserializes an Object configuration from an XmlNode
		/// </summary>
		/// <param name="x">Xml node to deserialize</param>
		/// <returns>ObjectConfiguration</returns>
		public static ObjectConfiguration Deserialize(XmlNode x) 
		{
			return (ObjectConfiguration)ObjectLoader.Deserialize(typeof(ObjectConfiguration),x.OuterXml);
		}

		/// <summary>
		/// Adds a dependency at a specified position
		/// </summary>
		/// <param name="i">0 based index to add it</param>
		/// <param name="o">Configuration to add</param>
		public void AddDependency(int i, ObjectConfiguration o) 
		{
			if (Dependencies == null)
				AddDependency(o);

			if (i < 0)
				throw new Exception("Can not add dependency at less than 0.");

			ArrayList a = new ArrayList();
			a.AddRange(Dependencies);
			a.Insert(i,o);
			Dependencies = (ObjectConfiguration[])a.ToArray(typeof(ObjectConfiguration));
		}

		/// <summary>
		/// Adds a dependency to the end of the list
		/// </summary>
		/// <param name="o">Configuration to add</param>
		public void AddDependency(ObjectConfiguration o) 
		{
			if (Dependencies == null) 
			{
				Dependencies = new ObjectConfiguration[1];
				Dependencies[0] = o;
			} 
			else 
			{
				ArrayList a = new ArrayList();
				a.AddRange(Dependencies);
				a.Add(o);
				Dependencies = (ObjectConfiguration[])a.ToArray(typeof(ObjectConfiguration));
			}
		}

		/// <summary>
		/// Removes a configuration from the dependencies
		/// </summary>
		/// <param name="i">0 based index to remove</param>
		/// <returns>True if successful, false otherwise</returns>
		public bool RemoveDependency(int i) 
		{
			if (i < 0 || Dependencies == null || i >= Dependencies.Length)
				return false;

			ArrayList a = new ArrayList();
			a.AddRange(Dependencies);
			a.RemoveAt(i);
			Dependencies = (ObjectConfiguration[])a.ToArray(typeof(ObjectConfiguration));
			return true;
		}

		/// <summary>
		/// Removes a certain configuration from the dependencies
		/// </summary>
		/// <param name="o">Configuration to remove</param>
		/// <returns>True if successful, false otherwise</returns>
		public bool RemoveDependency(ObjectConfiguration o) 
		{
			if (o == null)
				return false;

			ArrayList a = new ArrayList();
			a.AddRange(Dependencies);
			a.Remove(o);
			Dependencies = (ObjectConfiguration[])a.ToArray(typeof(ObjectConfiguration));
			return true;
		}

		/// <summary>
		/// Returns the friendly name
		/// </summary>
		/// <returns>String</returns>
		public override string ToString() 
		{
			if (this.FriendlyName != null)
				return this.FriendlyName;
			else
				return "";
		}
	}
}
