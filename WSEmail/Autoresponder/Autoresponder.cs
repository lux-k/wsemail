/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Collections;
using System.Configuration;
using MailServerInterfaces;
using WSEmailProxy;

namespace Autoresponder 
{
	/// <summary>
	/// Will automatically send a return message for users that this plugin is set to respond to.
	/// </summary>
	public class AutoResponder : IMappedAddress
	{
		/// <summary>
		/// Reference to the Mail Server interface
		/// </summary>
		protected IMailServer MailServer = null;
		/// <summary>
		/// A hashtable holding userIDs (as keys) and responses (as values)
		/// </summary>
		protected Hashtable ResponseTable = null;
		/// <summary>
		/// Private variable for whether the plugin is intialized.
		/// </summary>
		protected bool _init = false;
		/// <summary>
		/// Private variable for whether the plugin is shutdown.
		/// </summary>
		protected bool _shut = false;

		/// <summary>
		/// Returns the plugin type of this plugin (extension)
		/// </summary>
		public MailServerInterfaces.PluginType PluginType  
		{
			get 
			{
				return MailServerInterfaces.PluginType.Extension;
			}
		}

		/// <summary>
		/// Returns true if initialized, false otherwise.
		/// </summary>
		public bool IsInitialized 
		{
			get 
			{
				return _init;
			}
		}

		/// <summary>
		/// Returns true if shutdown, false otherwise.
		/// </summary>
		public bool IsShutdown 
		{
			get 
			{
				return _shut;
			}
		}

		/// <summary>
		/// Default constructor which will attempt to load the configuration from
		/// the default configuration file.
		/// </summary>
		public AutoResponder()
		{
			object o = ConfigurationSettings.GetConfig("AutoResponder");
			if (o is Hashtable) 
			{
				ResponseTable = (Hashtable)o;
			}
		}

		/// <summary>
		/// Implementation of ProcessMessage (required via IMappedAddress interface) that will see if
		/// it can map the user name to a message. This should always work, because for the mail server to
		/// know that it should hand off processing of a message to a plugin, the plugin has to register
		/// which addresses it can service.
		/// </summary>
		/// <param name="theMessage">The WSEmail Message to respond to</param>
		/// <param name="theSig">The Signature on the WSEmail Message</param>
		/// <returns>WSEmailStatus code</returns>
		public WSEmailStatus ProcessSave(WSEmailMessage theMessage, System.Xml.XmlElement theSig, string  recip) 
		{
			WSEmailMessage m = new WSEmailMessage();
			string s = recip.ToLower().Substring(0,recip.IndexOf("@"));
			MailServer.Log(MailServerLogType.ServerDebug, "Looking up the following address in the autoresponder table : '" + s + "'");
			ArrayList a = new ArrayList(ResponseTable.Keys);
			MailServer.Log(MailServerLogType.ServerDebug, "Keys = '" + String.Join("', '",(string[])a.ToArray(typeof(string))));
			m.Recipients.Add(theMessage.Sender);
			m.Sender = "AutoResponse@" + MailServer.Name;
			m.Timestamp = DateTime.Now;
			m.Subject = "Your auto-requested information...";
			m.Body = (string)ResponseTable[s];
			MailServer.Log(MailServerLogType.ServerDebug, "Sending autoresponse back to " + theMessage.Sender);
			if (MailServer == null || MailServer.DeliveryQueue == null)
				throw new Exception("Mailserver or deliveryqueue is null!");
			MailServer.DeliveryQueue.Enqueue(m,m.Sign(MailServer.Certificate),AuthenticatingTokenEnum.Internal);
			return new WSEmailStatus(200,"Message responded to");
		}

		/// <summary>
		/// Returns a string array of the addresses this plugin will handle.
		/// </summary>
		/// <returns>String array of userIDs (with or without @server on the end)</returns>
		public string[] EnumerateMappings() 
		{
			ArrayList a = new ArrayList();
			foreach (object o in ResponseTable.Keys)
				a.Add(o);

			return (string[])(a.ToArray(typeof(string)));
		}

		/// <summary>
		/// Initializes the plugin and receives a reference to the mail server.
		/// </summary>
		/// <param name="m">Mail server interface reference</param>
		/// <returns>Bool if successful initialization</returns>
		public bool Initialize(IMailServer m) 
		{
			MailServer = m;
			_init = true;
			return true;
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
		/// Shuts down the plugin. Again, it doesn't do much here.
		/// </summary>
		/// <returns></returns>
		public bool Shutdown() 
		{
			_shut = true;
			return true;
		}
	}
}
