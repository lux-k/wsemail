/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using MailServerInterfaces;

namespace WSEmailServer
{
	/// <summary>
	/// Summary description for PluginsConfigurationReader.
	/// </summary>
	public class PluginsConfigurationReader : IConfigurationSectionHandler
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
			ArrayList a = new ArrayList();

			foreach (System.Xml.XmlNode c in section.ChildNodes) 
			{
				a.Add(c.Attributes["class"].Value);
			}

			return (string[])a.ToArray(typeof(string));
		}

	}
}
