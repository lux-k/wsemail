/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Xml;
using WSEmailProxy;
using System.Web;
using Microsoft.Web.Services2.Security.X509;
using Microsoft.Web.Services2.Security.Tokens;
using Microsoft.Web.Services2;

namespace MailServerInterfaces
{
	/// <summary>
	/// Responsible for keeping a pool of database connections open, managing who gets them and making sure
	/// they are cleaned up.
	/// </summary>
	public interface IDatabaseManager : IServerPlugin, IService 
	{
		/// <summary>
		/// Gets a SQL connection from the pool.
		/// </summary>
		DatabaseConnection Connection {get;}
		/// <summary>
		/// Gets or sets the maximum number of connections that can be opened to the database server.
		/// </summary>
		int MaxConnections {get; set;}
		/// <summary>
		/// Gets the number of connections opened to the database server.
		/// </summary>
		int CurrentConnections {get;}
		/// <summary>
		/// Gets the number of open connections currently in use.
		/// </summary>
		int ActiveConnections {get;}
		/// <summary>
		/// Frees a connection with the given identity.
		/// </summary>
		/// <param name="i"></param>
		void Free(DatabaseConnection c);
	}
}
