/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using Org.Mentalis.Security.Ssl;
using WSEmailProxy;
using Org.Mentalis.Security.Certificates;
using System;
using System.Xml;
using System.Collections;
using System.Threading;
using MailServerInterfaces;

namespace WSSecureIMLib
{
	/// <summary>
	/// Coordinates allocating ports, opening those ports and proxying conversations between users.
	/// </summary>
	public class IMBroker : IExtensionProcessor, IDisposable
	{
		/// <summary>
		/// Holds the port allocator object.
		/// </summary>
		private ChannelRequestor Channels = new ChannelRequestor();
		/// <summary>
		/// Whether or not this object is being destroyed.
		/// </summary>
		private bool IsDisposing = false;
		/// <summary>
		/// Contains all the threads started by this object.
		/// </summary>
		private ArrayList SecureListenerProcessors  = new ArrayList();
		/// <summary>
		/// An object to log to. Currently two types are supported: null (means use console) and System.Windows.Forms.TextBox
		/// </summary>
		public static object LogTo;
		/// <summary>
		/// A certificate that identifies this chat proxy.
		/// </summary>
		public static Certificate ServerCert = null;

		protected bool _init = false, _shut = false;
		/// <summary>
		/// Holds a reference to the mailserver.
		/// </summary>
		private static MailServerInterfaces.IMailServer MailServer = null;

		/// <summary>
		/// Whether or not the plugin is initialized.
		/// </summary>
		public bool IsInitialized 
		{
			get 
			{
				return _init;
			}
		}

		/// <summary>
		/// Whether or not the plugin is in the shutdown state.
		/// </summary>
		public bool IsShutdown
		{
			get 
			{
				return _shut;
			}
		}

		/// <summary>
		/// The extension this plugin will process.
		/// </summary>
		public string Extension 
		{
			get 
			{
				return "SecureInstantMessaging";
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
		/// Returns the type of the plugin, in this case PluginType.MappedAddress
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
		/// Processes requests. Currently only implements the request direction connection method.
		/// </summary>
		/// <param name="args">XmlElement representing the arguments.</param>
		/// <param name="env">The environnment the request was called in</param>
		/// <returns>XmlElement result</returns>
		public XmlElement ProcessRequest(XmlElement args, ProcessingEnvironment env) 
		{
			if (env.IsLocalUser) 
			{
				if (args.LocalName.Equals("RequestDirectConnection")) 
				{
					if (args.ChildNodes.Count != 1)
						return null;

					IMBroker.LogEvent(this + " : granting secure chat allocation request.");
					ChannelRequest r = ChannelRequest.Deserialize(args.ChildNodes[0]);
					r = Channels.RequestChannel(r);
					OpenPorts(r);
					SendInvitations(r);
				}
			}
			return null;
		}

		/// <summary>
		/// Sends a secure chat invitation to all people in the conversation.
		/// </summary>
		/// <param name="r">The channel request to give.</param>
		private void SendInvitations(ChannelRequest r) 
		{
			WSEmailMessage wem = new WSEmailMessage();
			wem.Sender = r.Sender;
			wem.Recipients.AddRange(RecipientList.ParseRecipients(r.Recipient));
			wem.MessageFlags |= WSEmailFlags.InstantMessaging.SendAsInstantMessage | WSEmailFlags.InstantMessaging.DeleteIfNotDeliverable | WSEmailFlags.InstantMessaging.DirectConnectInvitation;
			wem.Timestamp = DateTime.Now;
			wem.Subject = "";
			wem.Body = r.Proxy + ":"+r.DestinationPort.ToString();
			MailServer.DeliveryQueue.Enqueue(wem,wem.Sign(MailServer.Certificate),AuthenticatingTokenEnum.Internal);
		}

		/// <summary>
		/// Shuts down and cleans up the plugin.
		/// </summary>
		/// <returns></returns>
		public bool Shutdown() 
		{
			Dispose();
			_shut = true;
			return true;
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
			ServerCert = Certificate.CreateFromX509Certificate(MailServer.Certificate);
			return true;
		}

		/// <summary>
		/// Logs an event, depending on what kind of logging object is setup.
		/// </summary>
		/// <param name="s">Message to log.</param>
		public static void LogEvent(string s) 
		{
			MailServer.Log(s);
		}

		/// <summary>
		/// Starts up the plugin.
		/// </summary>
		public IMBroker() 
		{
			StartUp();
		}

		/// <summary>
		/// Callled but unused.
		/// </summary>
		private void StartUp() 
		{
		}

		/// <summary>
		/// Opens the secure ports, creating new threads to watch each port.
		/// </summary>
		/// <param name="r"></param>
		private void OpenPorts(ChannelRequest r) 
		{
			SecureListenerProcessor sender = new SecureListenerProcessor(r.DestinationPort);
			sender.OnTerminate += new SecureListenerTerminating(this.TerminateListener);
			Thread t = new Thread(new ThreadStart(sender.Go));
			t.Name = "SLP-Port-"+r.DestinationPort.ToString();
			sender.thread = t;
			lock (SecureListenerProcessors) 
			{
				SecureListenerProcessors.Add(sender);
			}
			sender.thread.Start();
		}

		/// <summary>
		/// Terminates and disposes of the secure listener processor
		/// </summary>
		/// <param name="p"></param>
		private void TerminateListener(SecureListenerProcessor p) 
		{
			lock (SecureListenerProcessors) 
			{
				SecureListenerProcessors.Remove(p);
				int port = p.Port;
				p.Dispose();
				Channels.FreeChannel(port);
			}
		}
		
		~IMBroker() 
		{
			Dispose();
		}

		/// <summary>
		/// Cleans up all the threads created by this object.
		/// </summary>
		public void Dispose() 
		{
			if (!IsDisposing) 
			{
				for (int i = 0; i < SecureListenerProcessors.Count; i++) 
				{
					PennLibraries.Utilities.LogEvent(this + " : Disposing of SLP...");
					try 
					{
						((SecureListenerProcessor)SecureListenerProcessors[i]).Dispose();
						((SecureListenerProcessor)SecureListenerProcessors[i]).thread.Abort();
					} 
					catch (Exception e) 
					{
						LogEvent(this + " : Error in dispose -- " + e.Message);
					}
				}
			}
			GC.SuppressFinalize(this);
		}
	}
}
