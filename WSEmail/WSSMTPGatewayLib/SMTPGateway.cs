/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using WSEmailProxy;
//using WSEmailQueueLib;
using System.Windows.Forms;
using MailServerInterfaces;

namespace WSSMTPGatewayLib
{
	/// <summary>
	/// Provides a gateway from SMTP to WSEmail. This class will assume that there is a signing certificate in the configuration file
	/// for the purposes of authenticating with the WSEmail server.
	/// </summary>
	public class SMTPGateway : IService, IServerPlugin, IDisposable
	{
	
		/// <summary>
		/// Whether or not the server is initialized
		/// </summary>
		protected bool _init = false;
		/// <summary>
		/// Whether or not the server is shutdown
		/// </summary>
		protected bool _shut = false;
		/// <summary>
		/// Reference to the mail server
		/// </summary>
		private static IMailServer MailServer = null;
		/// <summary>
		/// DNS name of this mail server.
		/// </summary>
		public static string ServerName = "";
		/// <summary>
		/// Port number to bind to considered constant
		/// </summary>
		public const int PORTNUM = 25;
		/// <summary>
		/// An object to send log messages to.
		/// </summary>
		public static object LogTo = null;
		/// <summary>
		/// Socket listener thread handle.
		/// </summary>
		private Thread listener = null;
		/// <summary>
		/// TCPListener socket.
		/// </summary>
		private TcpListener serverSock = null;
	
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
				return MailServerInterfaces.PluginType.Service;
			}
		}

		/// <summary>
		/// Initializes this plugin to use a particular mailserver instance.
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		public bool Initialize(IMailServer m) 
		{
			MailServer = m;
			_init = true;
			this.StartUp();
			return true;
		}

		/// <summary>
		/// Binds to the port and listens for connections.
		/// </summary>
		public SMTPGateway() 
		{

		}

		/// <summary>
		/// Shutsdown the plugin.
		/// </summary>
		/// <returns></returns>
		public bool Shutdown() 
		{
			Dispose();
			_shut = true;
			return true;
		}

		/// <summary>
		/// Returns whether or not this plugin is initialized
		/// </summary>
		public bool IsInitialized 
		{
			get 
			{
				return _init;
			}
		}

		/// <summary>
		/// Returns whether or not this plugin is shutdown.
		/// </summary>
		public bool IsShutdown
		{
			get 
			{
				return _shut;
			}
		}

		/// <summary>
		/// Binds to the port and listens for connections. Also, sets an object to log to.
		/// </summary>
		public SMTPGateway (object o) 
		{
			SMTPGateway.LogTo = o;
			StartUp();
		}

		/// <summary>
		/// Suspends listening for new SMTP connections
		/// </summary>
		public void Suspend() 
		{
			if (listener.ThreadState == ThreadState.Running)
				listener.Suspend();
		}

		/// <summary>
		/// Resumes listening for new SMTP connections
		/// </summary>
		public void Resume() 
		{
			if (listener.ThreadState == ThreadState.Suspended)
				listener.Resume();
		}

		/// <summary>
		/// Gets the DNS name, starts the server socket and listens.
		/// </summary>
		private void StartUp() 
		{
			try 
			{
				LogEvent("Determining hostname... ");
				IPAddress IP = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0];
				ServerName = System.Net.Dns.GetHostByAddress(IP).HostName;
				LogEvent("Hostname is " + ServerName + ", done.");
				LogEvent("Binding to listening socket " + PORTNUM.ToString() + "... ");
				serverSock = new TcpListener(PORTNUM);
				serverSock.Start();
				LogEvent("Done binding...");
				listener = new Thread(new ThreadStart(Listen));
				listener.Start();
			} 
			catch (Exception e) 
			{
				LogEvent("Unable to start SMTP gateway: " + e.Message + e.StackTrace);
			}
		}

		/// <summary>
		/// Releases the listening socket and stops the listener thread
		/// </summary>
		public void Dispose() 
		{
			serverSock.Stop();
			serverSock = null;
			// destroy the socket listener thread, and stop listening on the socket
			if (listener != null)
				listener.Abort();
			GC.SuppressFinalize(this);		
		}

		/// <summary>
		/// Default destructor which will just call the dispose method
		/// </summary>
		~SMTPGateway() 
		{
			Dispose();
		}

		/// <summary>
		/// Used as a threadstart internally. This monitors the server socket and accepts connections.
		/// </summary>
		public void Listen() 
		{
			bool listen = true;
			while (listen) 
			{
				try 
				{
					Socket newClient = serverSock.AcceptSocket();
					LogEvent("Accepting new connection from " + newClient.RemoteEndPoint.ToString());
					// set the timeout to be higher.
					newClient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 25000);
					Thread newThread = new Thread(new ThreadStart(new SMTPProcessor(newClient,MailServer).Run));
					newThread.Start();
				} 
				catch (Exception e) 
				{
					LogEvent(e.Message + "\r\n" + e.StackTrace);
					listen = false;
				}
			}
		}

		/// <summary>
		/// Logs an event to whatever logging object this object has. The time is automatically prepended to the message. This method is
		/// static.
		/// </summary>
		/// <param name="s">Message</param>
		public static void LogEvent(string s) 
		{
			MailServer.Log(s);
		}
	}
}
