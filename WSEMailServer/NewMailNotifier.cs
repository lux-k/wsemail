/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Xml;
using System.Collections;
using System.Configuration;
using MailServerInterfaces;
using WSEmailProxy;



namespace WSEmailServer
{
	/// <summary>
	/// Summary description for FederatedAttachmentProcessor.
	/// </summary>
	public class NewMailNotifier : IExtensionProcessor
	{
		/// <summary>
		/// Reference to the Mail Server interface
		/// </summary>
		protected IMailServer MailServer = null;
		Hashtable recips = new Hashtable();

		public NewMailNotifier()
		{
			//
			// TODO: Add constructor logic here
			//
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

		public string Extension 
		{
			get 
			{
				return "NewMailNotifier";
			}
		}

		public XmlElement ProcessRequest(XmlElement args, ProcessingEnvironment env) 
		{
			ExtensionArgument e = new ExtensionArgument(args);
			if (e.MethodName.Equals("Register")) 
			{
				if (recips[env.UserID.ToLower()] == null)
					recips.Add(env.UserID.ToLower(),e["ip"] + ":" + e["port"]);
				else
					recips[env.UserID.ToLower()] = e["ip"] + ":" + e["port"];
				MailServer.Log(MailServerLogType.ServerInfo,"Registered UDP listener " + env.UserID + " on " +
					e["ip"] + ":" + e["port"]);
			} 
			else if (e.MethodName.Equals("Unregister"))
			{
				if (recips[env.UserID.ToLower()] != null) 
				{
					recips.Remove(env.UserID.ToLower());
					MailServer.Log(MailServerLogType.ServerInfo,"Unregistered UDP listener " + env.UserID);
				}
			}
			return null;
		}
		/// <summary>
		/// Initializes the plugin and receives a reference to the mail server.
		/// </summary>
		/// <param name="m">Mail server interface reference</param>
		/// <returns>Bool if successful initialization</returns>
		public bool Initialize(IMailServer m) 
		{
			MailServer = m;
			MailServer.LocalMTA.MailDelivered += new MailDeliveredHandler(MessageDelivered);
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

		public void MessageDelivered(string s) 
		{
			MailServer.Log(MailServerLogType.Debug,"Message delivered to " + s);
			if (recips[s.ToLower()] != null) 
			{

				string[] args = ((string)recips[s.ToLower()]).Split(':');
				UdpClient c = new UdpClient(args[0], int.Parse(args[1]));
				c.Send(new byte[] {},0);
				c.Close();
				MailServer.Log(MailServerLogType.ServerInfo,"Notified " + s + " of new mail at location " +
					(string)recips[s.ToLower()]);
			}
		}
		/// <summary>
		/// Returns the type of the plugin, in this case PluginType.MappedAddress
		/// </summary>
		/// <returns></returns>
		public MailServerInterfaces.PluginType PluginType  
		{
			get 
			{
				return MailServerInterfaces.PluginType.Service;
			}
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
