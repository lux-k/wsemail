/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Collections;
using System.Configuration;
using System.Xml;

namespace BizObjectsPlugin
{
	/// <summary>
	/// Handles reading the form routing stuff out of an xml config file. Basically, it expects things to look like this:
	/// &lt;SecureRoutingMapper&gt;<br>
	///   &lt;Role name="Supervisor" value="BossMan@Capricorn&gt;<br>
	/// &lt;SecureRoutingMapper&gt;  
	/// </summary>
	public class SecureRoutingMapperConfigurationReader : IConfigurationSectionHandler 
	{
		
		/// <summary>
		/// Creates a new reader in a specified node and context.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="configContext"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		public object Create(object parent, object configContext, System.Xml.XmlNode section) 
		{
			Hashtable h = new Hashtable();

			foreach (System.Xml.XmlNode c in section.ChildNodes) 
			{
				if (c.LocalName.Equals("Role")) 
					h.Add(c.Attributes["name"].Value,c.Attributes["value"].Value);
			}
			return h;
		}

	}
}
