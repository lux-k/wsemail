/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using MailServerInterfaces;
using WSEmailProxy;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using System.Xml;

namespace WSEmailServer
{
	/// <summary>
	/// This service is responsible for taking messages and serializing them to a database. At present, this server
	/// is SQL based (SQL Server 2000), but could easily be upgraded to an XML store or something else by editing
	/// all the serializing services (such as this) and the database manager.
	/// </summary>
	public class LocalMTA : ILocalMTA
	{
		/// <summary>
		/// Holds the number of messages delivered by this MTA.
		/// </summary>
		int messageCount = 0;
		/// <summary>
		/// Holds a reference to the mail server.
		/// </summary>
		IMailServer server = null;

		protected bool _init = false, _shut = false;
		
		public event MailDeliveredHandler MailDelivered;
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
		/// Default constructor which does nothing.
		/// </summary>
		public LocalMTA()
		{
		}
		
		/// <summary>
		/// Resumes processing of messages (currently unimplemented).
		/// </summary>
		public void Resume() {}
		/// <summary>
		/// Suspends processing of messages (currently unimplemented).
		/// </summary>
		public void Suspend() {}

		/// <summary>
		/// Shutdowns this service (currently unimplemented).
		/// </summary>
		/// <returns>True (boolean)</returns>
		public bool Shutdown() {_shut = true; return true;}

		/// <summary>
		/// Initializes the reference to the mail server configuration object.
		/// </summary>
		/// <param name="m">MailServer reference</param>
		/// <returns>True (boolean)</returns>
		public bool Initialize(IMailServer m) 
		{
			server = m;
			server.Log(MailServerLogType.ServerStatus,this + " : Local delivery agent running...");
			_init = true;
			return true;
		}

		/// <summary>
		/// Returns the plugin type of this plugin.
		/// </summary>
		/// <returns>PluginType.Service</returns>
		public MailServerInterfaces.PluginType PluginType  
		{
			get 
			{
				return MailServerInterfaces.PluginType.Service;
			}
		}

		/// <summary>
		/// Attempts to deliver a message to the local database. It will ask the database manager for a connection
		/// then massage the message in to something that can be written into and retrieved out of the database.
		/// </summary>
		/// <param name="theMessage">WSEmailMessage to save</param>
		/// <param name="x">XmlElement that is the signature on the message</param>
		/// <returns>WSEmailStatus giving indication of result</returns>
		public WSEmailStatus DeliverMessage(WSEmailMessage theMessage, XmlElement x) 
		{
			// get a connection from the database server
			try 
			{
				string[] Recipients = theMessage.Recipients.LocalRecipients(server.Name);
				
				if  (Recipients == null || Recipients.Length == 0)
					return new WSEmailStatus(500,"No recipients specified.");

				foreach (string recip in Recipients) 
				{
					WSEmailMessage localMessage = (WSEmailMessage)theMessage.Clone();
					XmlElement localSig = (XmlElement)x.Clone();
					bool go = server.RunDeliveryPlugins(localMessage,localSig,recip);

					if (go && localMessage != null) 
					{
						go = server.RunSavingPlugins(localMessage,localSig,recip);
						if (go && localMessage != null)
							SaveToDB(recip,localMessage,localSig);
						else
							server.Log(MailServerLogType.ServerDeliveryStatus,this + " : Message delivered to plugin.");
					}
					else 
						server.Log(MailServerLogType.ServerDeliveryStatus,this + " : Message delivered via delivery plugin.");
				}
			}
			catch (Exception e) 
			{
				// oops, an error. write that in the status.
				server.Log(MailServerLogType.ServerError,this + " : Exception was throw in LocalMTA: " + e.Message + "\r\n" + e.StackTrace);
				return new WSEmailStatus(500,"Internal error while saving message...");
			}
			return new WSEmailStatus(200,"Message saved.");
			// otherwise increment the message count and send acknowledgement

			
		}

		private WSEmailStatus SaveToDB(string recip, WSEmailMessage theMessage, XmlElement x) 
		{
			if (server.DiscardMessages)
				return new WSEmailStatus(200,"Message thrown out by policy.");
			
			WSEmailStatus s = server.DataAccessor.WriteMessage(recip,theMessage,x);

			if (s.ResponseCode == 200) 
			{
				messageCount++;
				if (MailDelivered != null)
					MailDelivered(recip);
			}
			return s;
		}

		/// <summary>
		/// Returns the number of messages delivered locally by this MTA.
		/// </summary>
		/// <returns></returns>
		public string GetStatus() 
		{
			return "Local messages delivered: " + messageCount.ToString();
		}

	}
}
