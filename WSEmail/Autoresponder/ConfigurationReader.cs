/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Collections;
using System.Configuration;
using MailServerInterfaces;
using WSEmailProxy;

namespace Autoresponder
{
	/// <summary>
	/// Handles reading the AutoResponder configuration out of a section of an XML file. Basically, the configurations
	/// look like this:<br><br>
	/// &lt;AutoResponder&gt;<br>
	///   &lt;Name&gt;Response to send for mail@name&lt;/Name&gt;<br>
	/// &lt;AutoResponder&gt;  
	/// </summary>
	public class AutoResponderConfigurationReader : IConfigurationSectionHandler 
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
				if (c.FirstChild != null)
					h.Add(c.Name.ToLower(),c.FirstChild.Value);
				else
					h.Add(c.Name,"");
			}
			return h;
		}

	}
}
