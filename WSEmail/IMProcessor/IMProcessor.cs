/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using WSEmailProxy;
using MailServerInterfaces;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.Xml;
using System.Collections;
using System.Configuration;
using WSInstantMessagingLibraries;

namespace IMProcessor
{
	/// <summary>
	/// Supplies instant messaging capabilities to a WSEmail server.
	/// </summary>
	public class IMProcessor : IExtensionProcessor, IDeliveryProcessor
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public IMProcessor()
		{
		}
		protected bool _init = false, _shut = false;

		/// <summary>
		/// Handle to the mail server.
		/// </summary>
		private MailServerInterfaces.IMailServer MailServer = null;
		private Hashtable Cache = null;
		private int iter = 0;

		/// <summary>
		/// Returns true if initialized.
		/// </summary>
		public bool IsInitialized 
		{
			get 
			{
				return _init;
			}
		}

		/// <summary>
		/// Returns true if shutdown.
		/// </summary>
		public bool IsShutdown
		{
			get 
			{
				return _shut;
			}
		}

		/// <summary>
		/// Returns the extension this plugin manages, "InstantMessaging"
		/// </summary>
		public string Extension 
		{
			get 
			{
				return "InstantMessaging";
			}
		}

		/// <summary>
		/// Initializes the plugin and receives a reference to the mail server.
		/// </summary>
		/// <param name="m">Mail server interface reference</param>
		/// <returns>Bool if successful initialization</returns>
		public bool Initialize(IMailServer m) 
		{
			Cache = new Hashtable();
			MailServer = m;
			m.CleanUpTick += new EventHandler(ClearCache);
			_init = true;

			return true;
		}

		private void ClearCache(object o, EventArgs e) 
		{
			iter++;
			if (iter == 2) 
			{
				Cache.Clear();
				iter = 0;
			}
		}

		/// <summary>
		/// Gets the current status from the plugin. Doesn't do much of anything.
		/// </summary>
		/// <returns></returns>
		public string GetStatus() 
		{
			return "Hello!";
		}

		/// <summary>
		/// Returns the type of the plugin, in this case PluginType.Extension
		/// </summary>
		/// <returns></returns>
		public MailServerInterfaces.PluginType PluginType  
		{
			get 
			{
				return MailServerInterfaces.PluginType.Extension;
			}
		}

		/// <summary>
		/// Processes requests from the ExecuteExtensionHandler server method. Currently only allows
		/// IMLocation updates.
		/// </summary>
		/// <param name="args">XmlElement of arguments</param>
		/// <param name="env">Environment on the server</param>
		/// <returns>XmlElement response</returns>
		public XmlElement ProcessRequest(XmlElement args, ProcessingEnvironment env) 
		{
			ExtensionArgument e = new ExtensionArgument(args);
			if (env.IsLocalUser) 
			{
				if (e.MethodName.Equals("UpdateIMLocation"))
					UpdateIMLocation(e,env);
			}
			return null;
		}

		/// <summary>
		/// Updates the IM remoting location of a particular user.
		/// </summary>
		/// <param name="args">XmlElement of args</param>
		/// <param name="env">Executing environment</param>
		private void UpdateIMLocation(ExtensionArgument args, ProcessingEnvironment env) 
		{
			if (args["Location"] == null) 
			{
				MailServer.Log(MailServerLogType.ServerError,this + " : Unable to find location node in IM location update!");
				return;
			}

			DatabaseConnection c = MailServer.Database.Connection;
			SqlCommand command = new SqlCommand(null, c.Connection);
			command.CommandText = "WSEmailIMUpdateUserLocation";
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add("@userid",env.UserID);
			command.Parameters.Add("@remotingurl",args["Location"]);
			command.ExecuteNonQuery();

			MailServer.Database.Free(c);
			MailServer.Log(MailServerLogType.ServerInfo, this + " : updated the IM location of " + env.UserID + " to " + args["Location"]);
		}
		/// <summary>
		/// Shuts down the plugin. Again, it doesn't do much here.
		/// </summary>
		/// <returns></returns>
		public bool Shutdown() 
		{
			_shut = true;
			return true;
		}

		/// <summary>
		/// Handles delivering messages via instant message and deals with errors if unable to do so. Called via
		/// server interfaces.
		/// </summary>
		/// <param name="theMessage">The WSEmail message</param>
		/// <param name="theSig">Signature on message</param>
		/// <param name="recip">The recipient of the message</param>
		/// <returns>true if successful</returns>
		public bool ProcessDeliver(WSEmailMessage theMessage, XmlElement theSig, string recip) 
		{
			if ( (theMessage.MessageFlags & WSEmailFlags.InstantMessaging.SendAsInstantMessage) == WSEmailFlags.InstantMessaging.SendAsInstantMessage)
				return !SendIM(theMessage,recip);
			return true;
		}

		/// <summary>
		/// Returns the IM location of a particular user.
		/// </summary>
		/// <param name="user">user@host to lookup</param>
		/// <returns></returns>
		private string GetIMLocation(string user) 
		{
			DatabaseConnection c = MailServer.Database.Connection;
			SqlCommand sqlCmd = new SqlCommand("WSEmailIMGetRemotingUrl",c.Connection);
			sqlCmd.CommandType = CommandType.StoredProcedure;
			sqlCmd.Parameters.Add("@userid",user);
			SqlDataReader rows=sqlCmd.ExecuteReader();
			// used to detect a delivery error
			WSEmailStatus status = new WSEmailStatus();
			string rUrl = null;
			if (rows.Read()) 
			{
				// ok there is a remote location! send to it
				rUrl = rows.GetString(0);
			}
			rows.Close();
			MailServer.Database.Free(c);
			return rUrl;
		}

		private bool AttemptMailDelivery(WSEmailMessage theMessage, string recipient) 
		{
			if ((theMessage.MessageFlags & WSEmailFlags.InstantMessaging.DeleteIfNotDeliverable) == WSEmailFlags.InstantMessaging.DeleteIfNotDeliverable) 
			{
				MailServer.Log(MailServerLogType.ServerDeliveryStatus, this + " : " + new WSEmailStatus(541,"Unable to send instant message to user -- failed to connect to user -- throwing out."));
				return true;
			}
			else 
			{
				MailServer.Log(MailServerLogType.ServerDeliveryStatus, this + " : " + new WSEmailStatus(541,"Unable to send instant message to user -- failed to connect to user -- spooling."));
				return false;
			}
		}
		/// <summary>
		/// Sends an im to a user, or deals with errors if it can't 
		/// </summary>
		/// <param name="theMessage"></param>
		/// <returns></returns>
		private bool SendIM(WSEmailMessage theMessage, string recipient) 
		{
			// return false if a message wasn't sent
			// return true if it was sent
			if (Cache[recipient] == null) 
			{
				string remoteUrl = this.GetIMLocation(recipient);
				if (remoteUrl == null) 
				{
					if (Cache[recipient] == null)
						Cache.Add(recipient,false);

					if ((theMessage.MessageFlags & WSEmailFlags.InstantMessaging.DeleteIfNotDeliverable) == WSEmailFlags.InstantMessaging.DeleteIfNotDeliverable) 
					{
						MailServer.Log(MailServerLogType.ServerDeliveryStatus, this + " : " + new WSEmailStatus(544,"Unable to find user, but flags specify messages should be thrown out."));
						// just throw it out if undeliverable.
						return true;
					}				
					else 
					{
						MailServer.Log(MailServerLogType.ServerDeliveryStatus, this + " : " + new WSEmailStatus(544,"Unable to find user."));
						// no where to send it to and delete flag isn't present
						return false;
					}
				}
			
				try 
				{
					IMPosting ser = (IMPosting)Activator.GetObject(typeof(IMPosting),remoteUrl);
					ser.postMessage(theMessage.SerializeToBase64String());
					MailServer.Log(MailServerLogType.ServerDeliveryStatus, this + " : " + new WSEmailStatus(201,"Message received by " + MailServer.Name + " for " + recipient + " and sent as an Instant Message to " + remoteUrl));
					return true;
				} 
				catch (Exception ex) 
				{
					MailServer.Log(MailServerLogType.ServerDebug, this + " : Error while attempting to send instant message (" + ex.Message +")");
					if (Cache[recipient]==null)
						Cache.Add(recipient,false);

					return AttemptMailDelivery(theMessage,recipient);
				}
			} 
			else 
			{
				return AttemptMailDelivery(theMessage,recipient);
			}
		}
	}
}
