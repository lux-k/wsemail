/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Collections;
using System.Web;
using System.Web.Services;
using System.Xml;

using Microsoft.Web.Services2;
using Microsoft.Web.Services2.Security;

using WSSecureIMLib;
using WSEmailProxy;
using WSInstantMessagingLibraries;

using System.Data.SqlClient;
using System.ComponentModel;
using System.Data;

namespace WSEmailServer
{
	/// <summary>
	/// Summary description for AvailableServers.
	/// </summary>
	/// 
	public class Server 
	{
		public string Url;
		public string ServerName;
		public string Owner;
		public Server() {}
		public Server(string name, string url, string owner) 
		{
			Url = url;
			ServerName = name;
			Owner = owner;
		}
	}

	[WebService(Namespace="http://securitylab.cis.upenn.edu/WSEmail")]
	public class AvailableServers : System.Web.Services.WebService
	{
		public AvailableServers()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		[WebMethod]
		public Server[] GetServers() 
		{
			MailServerInterfaces.DatabaseConnection c = Global.ServerConfiguration.Database.Connection;
			
			SqlCommand command = new SqlCommand("WSEmailShowServers",c.Connection );
			command.CommandType = CommandType.StoredProcedure;
			SqlDataReader rows=command.ExecuteReader();
			
			ArrayList Servers = new ArrayList();
			while (rows.Read()) 
			{
				Server s = new Server(rows.GetString(0),rows.GetString(2),rows.GetString(1));
				Servers.Add(s);
			}
			rows.Close();
			Global.ServerConfiguration.Database.Free(c);
			return (Server[])Servers.ToArray(typeof(Server));	

		}
	}
}
