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
	/// The main interface to the mail server which holds references to all services and plugins.
	/// </summary>
	public interface IMailServer 
	{
		/// <summary>
		/// The URL the mail server is at.
		/// </summary>
		string Url {get; set;}
		/// <summary>
		/// The date the server was started.
		/// </summary>
		DateTime StartTime {get;}
		/// <summary>
		/// The number of seconds the server has been running.
		/// </summary>
		long Uptime {get;}
		/// <summary>
		/// The DNS Server the mail server will query for routing information.
		/// </summary>
		string DnsServer {get; set;}
		/// <summary>
		/// The name of the server. (ie. user@Server returns Server)
		/// </summary>
		string Name {get; set;}
		/// <summary>
		/// The URL of the default router.
		/// </summary>
		string Router {get; set;}
		string LastError {get;}
		event EventHandler CleanUpTick;
		string SMTPRelay {get; set;}
		/// <summary>
		/// The certificate of the server.
		/// </summary>
		X509Certificate Certificate {get;set;}
		X509SecurityToken X509Token {get;set;}
		Microsoft.Web.Services.Security.X509SecurityToken WSE1SecurityToken  {get;set;}
		Statistics Stats {get;set;}
		bool DiscardMessages {get;set;}
		/// <summary>
		/// Logs a message to the server's log file.
		/// </summary>
		/// <param name="s"></param>
		void Log(string s);
		/// <summary>
		/// Logs a message with a type.
		/// </summary>
		/// <param name="t"></param>
		/// <param name="s"></param>
		void Log(MailServerLogType t, string s);
		/// <summary>
		/// Gets a reference to the delivery queue.
		/// </summary>
		IMailQueue DeliveryQueue {get; set;}
		/// <summary>
		/// Delivers messages to local recipients.
		/// </summary>
		ILocalMTA LocalMTA {get; set;}
		/// <summary>
		/// Manages database access.
		/// </summary>
		IDatabaseManager Database {get; set;}
		IDataAccessor DataAccessor {get; set;}
		/// <summary>
		/// Retrieves and deletes existing messages.
		/// </summary>
		IMessageAccess MessageAccessor {get; set;}
		/// <summary>
		/// Registers a new request to avoid replay attacks.
		/// </summary>
		/// <param name="id">Request ID</param>
		/// <param name="t">Expiration time</param>
		void RegisterRequest(string id, System.DateTime t);
		void RunSendingPlugins(WSEmailMessage m, XmlElement theSig, ProcessingEnvironment p);
		bool RunSavingPlugins(WSEmailMessage m, XmlElement theSig, string recip);
		bool RunDeliveryPlugins(WSEmailMessage m, XmlElement theSig, string recip);
		XmlElement RunExtensionHandler(string ext, XmlElement args, ProcessingEnvironment p);
	}
}
