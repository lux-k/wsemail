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
	/// Pools database connections to remove latency for connecting and authenticating to the database server each
	/// time a database query is needed.
	/// </summary>
	public class DatabaseManager : IDatabaseManager
	{
		protected SqlConnection[] connections;
		protected IMailServer MailServer = null;
		protected int _max;
		protected bool[] inUse;
		protected string connectionstr;

		/// <summary>
		/// Returns the plugin type of this object.
		/// </summary>
		/// <returns></returns>
		public MailServerInterfaces.PluginType PluginType  
		{
			get 
			{
				return MailServerInterfaces.PluginType.Service;
			}
		}

		protected bool _init = false, _shut = false;

		public bool IsInitialized 
		{
			get 
			{
				return _init;
			}
		}
		public bool IsShutdown
		{
			get 
			{
				return _shut;
			}
		}


		/// <summary>
		/// Returns a string of status information about the number of connections and such.
		/// </summary>
		/// <returns></returns>
		public string GetStatus() 
		{
			string s = "Max connections: " + _max.ToString() + ", currently connected: " + CurrentConnections.ToString()+ ", active connections: " + ActiveConnections.ToString();
			DatabaseConnection c = this.Connection;
			s += "\r\nServer version: " + c.Connection.ServerVersion + ", database name = "+ c.Connection.Database + ", data source: " + c.Connection.DataSource;
			this.Free(c);
			return s;
		}

		/// <summary>
		/// Resumes giving out database connections (no implemented).
		/// </summary>
		public void Resume() {}
		/// <summary>
		/// Suspends giving out database connections (no implemented).
		/// </summary>
		public void Suspend() {}

		/// <summary>
		/// Returns the number of open and in use connections.
		/// </summary>
		public int ActiveConnections 
		{
			get 
			{
				int c = 0;
				foreach (bool b in inUse) 
				{
					if (b == true)
						c++;
				}
				return c;
			}
		}

		/// <summary>
		/// Gets or sets the maximum number of connections that can be opened to the database server.
		/// </summary>
		public int MaxConnections 
		{
			get 
			{
				return _max;
			}
			set 
			{
				_max = value;
			}
		}

		/// <summary>
		/// Default constructor which will try to read the database configuration out of the
		/// config file for the running application. It expects to find this setup in the "Database" section.
		/// </summary>
		public DatabaseManager()
		{
			object o = ConfigurationSettings.GetConfig("Database");
			if (o is PennLibraries.DatabaseConfiguration) 
			{
				this.connectionstr = ((PennLibraries.DatabaseConfiguration)o).connstr;
				this.MaxConnections = ((PennLibraries.DatabaseConfiguration)o).maxconnections;
			}
			else
				throw new Exception("Unable to read database configuration!");
		}

		/// <summary>
		/// Returns the number of connections open to the database server.
		/// </summary>
		public int CurrentConnections 
		{
			get 
			{
				int j = 0;
				for (int i = 0; i < MaxConnections; i++) 
				{
					if (connections[i].State == System.Data.ConnectionState.Open)
						j++;
				}
				return j;
			}
		}

		/// <summary>
		/// Opens the initial connections to the database server.
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		public bool Initialize(IMailServer m) 
		{
			MailServer = m;
			connections = new SqlConnection[MaxConnections];
			inUse = new bool[MaxConnections];
			for (int i = 0; i < MaxConnections; i++) 
			{
				connections[i] = OpenConnection();
				inUse[i] = false;
			}
			connections[0].Open();
			connections[0].Close();
			m.Log(MailServerLogType.ServerStatus,this + " : Database manager coming online...\r\n"+this.GetStatus());
			_init = true;
			return true;
		}

		/// <summary>
		/// Opens a connection to the database server.
		/// </summary>
		/// <returns></returns>
		private SqlConnection OpenConnection() 
		{
			SqlConnection s = new SqlConnection(connectionstr);
			return s;
		}

		/// <summary>
		/// Closes all open database connections.
		/// </summary>
		/// <returns></returns>
		public bool Shutdown() 
		{
			
			foreach (SqlConnection s in connections) 
			{
				if (s != null && s.State == System.Data.ConnectionState.Open)
					s.Close();
			}
			_shut = true;
			return true;
		}

		/// <summary>
		/// Releases a database connection back in to the pool of available connections.
		/// </summary>
		/// <param name="i">The DBConnection.Identifer to release</param>
		public void Free(DatabaseConnection c) 
		{
			if (c.Identifier >= 0 && c.Identifier < MaxConnections) 
			{
				inUse[c.Identifier] = false;
				connections[c.Identifier].Close();
				MailServer.Log(MailServerLogType.ServerDebug,this + " : Database manager freeing ID - " + c.Identifier.ToString() + " for use.");
			} 
			else 
			{
				c.Connection.Close();
			}
		}

		/// <summary>
		/// Retrieves a database connection from the pool of available connections.
		/// </summary>
		public DatabaseConnection Connection 
		{
			get 
			{

				System.Threading.Monitor.Enter(this);
				for (int i = 0; i < MaxConnections; i++) 
				{
					if (inUse[i] == false) 
					{
						if (connections[i].State != System.Data.ConnectionState.Open) 
						{
							MailServer.Log(MailServerLogType.ServerDebug,this + " : Reopening database connection - ID " + i.ToString());
							// connections[i] = OpenConnection();
							connections[i].Open();
						} else
							MailServer.Log(MailServerLogType.ServerDebug,this + " : Handing off pooled database connection - ID " + i.ToString());
						
						inUse[i] = true;
						System.Threading.Monitor.Pulse(this);
						System.Threading.Monitor.Exit(this);		
						return new DatabaseConnection(connections[i],i);

					}
				}
				MailServer.Log(MailServerLogType.ServerStatus,this + " : Out of available database connections, creating additional.");
				SqlConnection s = OpenConnection();
				s.Open();
				return new DatabaseConnection(s,-1);
			}
		}
	}
}
