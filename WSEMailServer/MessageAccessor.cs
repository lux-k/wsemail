/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Collections;
using MailServerInterfaces;
using WSEmailProxy;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using System.Xml;

namespace WSEmailServer
{
	/// <summary>
	/// Essentially the POP3 or IMAP arm of the mail processing system. Depends upon a working database manager running.
	/// </summary>
	public class MessageAccessor : IMessageAccess, IDisposable
	{

		/// <summary>
		/// Reference to the mail server interfaces.
		/// </summary>
		protected IMailServer server = null;
		/// <summary>
		/// The userID we are processing messages for.
		/// </summary>
		protected string _user;

		/// <summary>
		/// Default constructor does nothing.
		/// </summary>
		public MessageAccessor()
		{
		}

		protected bool _init = false, _shut = false;

		int _ins = 0;
		/// <summary>
		/// Is this plugin initialized?
		/// </summary>
		public bool IsInitialized 
		{
			get 
			{
				return _init;
			}
		}

		/// <summary>
		/// Is this plugin marked as being shutdown?
		/// </summary>
		public bool IsShutdown
		{
			get 
			{
				return _shut;
			}
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
			server.Log(MailServerLogType.ServerInfo, this + " : Local message accessor running...");
			_init = true;
			return true;
		}

		/// <summary>
		/// The already authenticated ID of the user whose email will be accessed.
		/// </summary>
		public string UserID 
		{
			get 
			{
				return _user;
			}
			set 
			{
				_user = value;
			}
		}

		/// <summary>
		/// Clones this object
		/// </summary>
		/// <returns>Object</returns>
		public object Clone() 
		{
			this._ins++;
			return this.MemberwiseClone();
		}

		/// <summary>
		/// Creates a clone of this instance and sets the new instance to the username specified. It's assumed one
		/// instance will always be in memory and each sub instance will get created.
		/// </summary>
		/// <param name="s">UserID to use</param>
		/// <returns>IMessageAccess object</returns>
		public IMessageAccess CreateInstance(string s) 
		{
			IMessageAccess i = (IMessageAccess)this.Clone();
			i.UserID = s;
			return i;
		}

		/// <summary>
		/// Deletes an array of integer message numbers from the user's store.
		/// </summary>
		/// <param name="messagesToDelete">Array of integer representing message numbers</param>
		/// <returns>WSEmailStatus giving number of messages deleted</returns>
		public WSEmailStatus DeleteMessage(int[] messagesToDelete) 
		{
			return server.DataAccessor.DeleteMessage(UserID,messagesToDelete);
		}

		/// <summary>
		/// Fetches all the headers in a user's inbox
		/// </summary>
		/// <returns>Array of WSEmailHeader or null</returns>
		public WSEmailHeader[] FetchHeaders(DateTime t) 
		{
			return server.DataAccessor.ReadHeaders(this.UserID,t);
		}

		/// <summary>
		/// Retrieves a WSEmail message from the user's store
		/// </summary>
		/// <param name="messageToGet">Message number to get</param>
		/// <returns>WSEmailPackage containing message id, message and signature or null</returns>
		public WSEmailPackage RetrieveMessage(int messageToGet) 
		{
			return server.DataAccessor.ReadMessage(this.UserID,messageToGet);
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
		/// Returns the status of the object. (not implemented)
		/// </summary>
		/// <returns></returns>
		public string GetStatus() 
		{
			return "Instantiations: " + _ins.ToString();
		}

		/// <summary>
		/// Required so that database connections can be freed in an efficient manner.
		/// </summary>
		public void Dispose() 
		{
		}

		/// <summary>
		/// Cleans up any left over database connections.
		/// </summary>
		~MessageAccessor() 
		{
			Dispose();
		}
	}
}
