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

///<summary>
/// These are the interfaces that will need to be implemented to create plugins and
/// services that will seamlessly fit in to the WS Email Server.
///</summary>
namespace MailServerInterfaces
{
	public enum MailServerLogType 
	{	Unknown, Error, MessageSent, MessageReceived, UserAuthentication, UserAuthenticationError,
		Debug, MessageAuthentication, MessageAuthenticationError, ServerInfo, ServerDebug, Informational,
		ServerStatus, ServerDeliveryStatus, ServerError, ServerWarning, RequestStart, RequestEnd};
	public enum AuthenticatingTokenEnum {Unknown, UsernamePassword, X509Certificate, FederatedIdentity, Internal};

	public delegate void MailDeliveredHandler(string recipient);

	public class ProcessingEnvironment 
	{
		public readonly HttpServerUtility Server = null;
		public readonly bool IsLocalUser = false;
		public readonly string UserID = null;
		public readonly SoapContext RequestContext = null;
		public SoapContext ResponseContext = null;
		public readonly string Recipient = null;
		public readonly AuthenticatingTokenEnum AuthenticatingTokenType = AuthenticatingTokenEnum.Unknown;

		public ProcessingEnvironment(HttpServerUtility t, SoapContext req, SoapContext res, string UserID, bool IsLocalUser, AuthenticatingTokenEnum AuthTokenType) 
		{
			this.Server = t;
			this.RequestContext = req;
			this.ResponseContext = res;
			this.UserID = UserID;
			this.IsLocalUser = IsLocalUser;
			this.AuthenticatingTokenType = AuthTokenType;
			
		}
	}
	/// <summary>
	/// Determines the type of the plugin. Either it's a service that runs independent of addresses or is tied to a specific address.
	/// </summary>
	public enum PluginType {Extension, Service};

	/// <summary>
	/// Interface for all plugins.
	/// </summary>
	public interface IServerPlugin
	{
		/// <summary>
		/// Initializes the plugin with a reference to the server running it. (So the plugin can access
		/// log files, databases, etc).
		/// </summary>
		/// <param name="m">MailServer reference</param>
		/// <returns></returns>
		bool Initialize(IMailServer m);
		bool IsInitialized {get;}
		/// <summary>
		/// Shuts down the plugin.
		/// </summary>
		/// <returns></returns>
		///
		bool IsShutdown {get;}
		bool Shutdown();
		PluginType PluginType {get;}
	}

	public interface ISendingProcessor : IServerPlugin
	{
		/// <summary>
		/// Called by the server to process the given message through the plugin.
		/// </summary>
		void ProcessSend(WSEmailMessage theMessage, XmlElement theSig, ProcessingEnvironment env);
	}

	public interface IDeliveryProcessor : IServerPlugin
	{
		/// <summary>
		/// Called by the server to process the given message through the plugin.
		/// </summary>
		bool ProcessDeliver(WSEmailMessage theMessage, XmlElement theSig, string recip);
	}

	public interface IExtensionProcessor : IServerPlugin 
	{
		string Extension {get;}
		XmlElement ProcessRequest(XmlElement arguments, ProcessingEnvironment env);
	}

	/// <summary>
	/// Interface for all plugins that map addresses.
	/// </summary>
	public interface IMappedAddress : IServerPlugin
	{
		/// <summary>
		/// Returns a list of email addresses to the server that this plugin will handle.
		/// </summary>
		string[] EnumerateMappings();
		/// <summary>
		/// Called by the server to process the given message through the plugin.
		/// </summary>
		WSEmailStatus ProcessSave(WSEmailMessage theMessage, XmlElement theSig, string recip);
	}

	/// <summary>
	/// Interface for all services (such as the mail queue, database access, etc).
	/// </summary>
	public interface IService 
	{
		/// <summary>
		/// Returns a status string from the service.
		/// </summary>
		/// <returns></returns>
		string GetStatus();
		/// <summary>
		/// Tells the service not to process any more requests.
		/// </summary>
		void Suspend();
		/// <summary>
		/// Tells the service to resume processing requests.
		/// </summary>
		void Resume();
	}

	/// <summary>
	/// Interface for the mail queue. It holds messages waiting to be processed.
	/// </summary>
	public interface IMailQueue : IServerPlugin, IService 
	{
		/// <summary>
		/// Enqueues a message in to the queue.
		/// </summary>
		WSEmailStatus Enqueue(WSEmailMessage m, XmlElement x, AuthenticatingTokenEnum t);
		/// <summary>
		/// Bounce a message back into the queue.
		/// </summary>
		void BounceMessage(WSEmailMessage theMessage, string reason) ;
	}


	/// <summary>
	/// Controls local message delivery.
	/// </summary>
	public interface ILocalMTA : IServerPlugin, IService 
	{
		/// <summary>
		/// Delivers a message to the local database.
		/// </summary>
		WSEmailStatus DeliverMessage(WSEmailMessage m, XmlElement sig);
		event MailDeliveredHandler MailDelivered;
	}

	/// <summary>
	/// Retrieves and deletes messages.
	/// </summary>
	public interface IMessageAccess : IServerPlugin, IService, ICloneable, IDisposable
	{
		/// <summary>
		/// The userID of the messages to get/delete.
		/// </summary>
		string UserID {get; set;}
		/// <summary>
		/// Deletes a list of messages.
		/// </summary>
		WSEmailStatus DeleteMessage(int[] list);
		/// <summary>
		/// Retrieves a messages.
		/// </summary>
		WSEmailPackage RetrieveMessage(int i);
		/// <summary>
		/// Fetches the headers of the mailbox.
		/// </summary>
		WSEmailHeader[] FetchHeaders(DateTime t);
		/// <summary>
		/// Clones an instance of the object.
		/// </summary>
		IMessageAccess CreateInstance(string uid);
	}

	public interface IDataAccessor : IServerPlugin, IService, IDisposable
	{
		WSEmailStatus DeleteMessage(string userID, int[] messagesToDelete);
		WSEmailStatus WriteMessage(string recip, WSEmailMessage theMessage, XmlElement x);
		WSEmailPackage ReadMessage(string user, int number);
		WSEmailHeader[] ReadHeaders(string user, DateTime t);
		string ReadUserPassword(string userID); 
		string ReadUserName(string userID);
	}
	/// <summary>
	/// Holds information about a database connection.
	/// </summary>
	public class DatabaseConnection 
	{
		/// <summary>
		/// The actual SqlConnection
		/// </summary>
		public System.Data.SqlClient.SqlConnection Connection = null;
		/// <summary>
		/// The database manager's identifer for the connection.
		/// </summary>
		public int Identifier = 0;

		public string TraceInfo = "";

		/// <summary>
		/// Constructor which creates a new database connection with the given connection and identifer.
		/// </summary>
		/// <param name="c">Connection</param>
		/// <param name="i">Identifier</param>
		public DatabaseConnection(System.Data.SqlClient.SqlConnection c, int i) 
		{
			this.Connection = c;
			this.Identifier = i;
		}
	}


}
