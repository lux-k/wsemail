/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Configuration;
using System.ComponentModel;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels;
using MailServerInterfaces;

namespace WSEmailServer 
{
	/// <summary>
	/// Holds global constants that need to be available to the WSEmailServer namespace.
	/// </summary>
	public class Global : System.Web.HttpApplication
	{
		/// <summary>
		/// Remoting channel for Instant Messaging and such.
		/// </summary>
		private static HttpChannel REMOTINGCHANNEL;
		/// <summary>
		/// The mail server interface.
		/// </summary>
		public static MailServerConfiguration ServerConfiguration = null;

		public static string LastError 
		{
			get 
			{
				if (ServerConfiguration != null)
					return ServerConfiguration.LastError;
				else 
					return "";
			}
		}

		public Global()
		{
			InitializeComponent();
		}	
		
		/// <summary>
		/// Loads the filters, starts up remoting channels and loads/configures plug-ins.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_Start(Object sender, EventArgs e)
		{

			// add our custom input and output filters.
			// PennRoutingFilters.PennRoutingUtilities.AddPennRoutingFilters(true);
			PennLibraries.Utilities.AddPennLoggingFilters();
			// PennRoutingFilters.PennRoutingUtilities.AddWSEMailSignatureFilters(false);
			REMOTINGCHANNEL = new HttpChannel();
			ChannelServices.RegisterChannel(REMOTINGCHANNEL);

			ServerConfiguration = new MailServerConfiguration();
			ServerConfiguration.Log(MailServerLogType.ServerStatus,this + " : Mail server starting...");
			if (!ServerConfiguration.Initialize())
				ServerConfiguration.Log(MailServerLogType.ServerError,this + " : Server initialization failed.");
		}
 
		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
			
			if (ServerConfiguration != null && !ServerConfiguration.IsInitialized)
				ServerConfiguration.Shutdown();

			if (ServerConfiguration == null || !ServerConfiguration.IsInitialized) 
			{
				ServerConfiguration = null;
				ServerConfiguration = new MailServerConfiguration();
				ServerConfiguration.Initialize();
			}

			if (ServerConfiguration.Url == null) 
			{
				string s = Request.Url.AbsolutePath;
				s=s.Substring(0,s.LastIndexOf("/")+1) + "MailServer.asmx";
				ServerConfiguration.Url = "http://" + Server.MachineName + s;
				ServerConfiguration.Log(MailServerLogType.ServerInfo,this + " : Mail server is attached to : " +
					ServerConfiguration.Url);
			}

//			if (Request.FilePath.ToLower().EndsWith(".aspx"))
//			{
			if (ServerConfiguration == null || !ServerConfiguration.IsInitialized)
				Server.Transfer("Error.aspx");
//			}

		}

		/// <summary>
		/// Shuts down the plugins and closes the remoting channel.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_End(Object sender, EventArgs e)
		{
			ChannelServices.UnregisterChannel(REMOTINGCHANNEL);
			ServerConfiguration.Log(MailServerLogType.ServerStatus,this + " : Server shutdown initiated.");
			ServerConfiguration.Shutdown();
		}
		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			// 
			// Global
			// 
			this.Error += new System.EventHandler(this.Global_Error);
		}
		#endregion

		private void Global_Error(object sender, System.EventArgs e)
		{
			if (ServerConfiguration != null) 
				ServerConfiguration.Log(MailServerLogType.ServerError,Server.GetLastError().Message);
		}

	}
}

