/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Configuration;

namespace PennLibraries
{
	public class DatabaseConfiguration 
	{
		public string connstr = "";
		public int maxconnections = 5;
	}

	public class DatabaseConfigurationReader : IConfigurationSectionHandler 
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
			DatabaseConfiguration cf = new DatabaseConfiguration();

			foreach (System.Xml.XmlNode c in section.ChildNodes) 
			{
				switch (c.Name.ToLower())
				{
					case "server":
						cf.connstr += "data source=" + c.Attributes["value"].Value + ";";
						break;
					case "username":
						cf.connstr += "user id=" + c.Attributes["value"].Value + ";";
						break;
					case "database":
						cf.connstr += "initial catalog=" + c.Attributes["value"].Value + ";";
						break;
					case "password":
						cf.connstr += "password=" + c.Attributes["value"].Value + ";";
						break;
					case "connections":
						cf.maxconnections = int.Parse(c.Attributes["value"].Value);
						cf.connstr += "max pool size= " +cf.maxconnections.ToString() +";";
						break;
				}
			}
			cf.connstr +="pooling=true;";
			if (cf.connstr.EndsWith(";"))
				cf.connstr = cf.connstr.TrimEnd(';');

			return cf;
		}
	}
}
