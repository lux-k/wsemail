/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System.Collections;
using System.Text;
using System.IO;
using Org.Mentalis.Security.Ssl;
using Org.Mentalis.Security.Cryptography;
using Org.Mentalis.Security.Certificates;
using System.Threading;
using System;
using System.Net.Sockets;
using System.Net;

namespace WSSecureIMLib
{

	public delegate void SecureClientTerminating(SecureClient l);

	/// <summary>
	/// Talks to a client and relays messages back to the securelistenerprocessor for broadcasting.
	/// </summary>
	public class SecureClient : IDisposable
	{
		public event SecureClientTerminating OnTerminate=null;
		public Certificate cert = null;
		/// <summary>
		/// The socket we're talking over
		/// </summary>
		public SecureSocket ss = null;
		/// <summary>
		/// The parent processor that created this (used for relaying messages).
		/// </summary>
		private SecureListenerProcessor parent = null;
		/// <summary>
		/// Reference to the thread this object is running on.
		/// </summary>
		public Thread t = null;
		public string ID = Guid.NewGuid().ToString();
		private bool IsDisposing = false;
		public string userName = "";
		public bool Ready = false;
		/// <summary>
		/// Create a new secure client with the socket is should talk to and the parent it should 
		/// ask to relay with.
		/// </summary>
		/// <param name="s">SecureSocket to talk on</param>
		/// <param name="p">SecureListenerProcessor to relay messages through</param>
		public SecureClient(SecureSocket s, SecureListenerProcessor p) 
		{
			this.ss = s;
			this.parent = p;
			//PennRoutingFilters.PennRoutingUtilities.LogEvent("Secure client created. ID: " + id);
		}

		/// <summary>
		/// Cleanup code
		/// </summary>
		~SecureClient() 
		{
			Dispose();
		}

		/// <summary>
		/// Cleaup code
		/// </summary>
		public void Dispose() 
		{
			if (!IsDisposing) 
			{
				IsDisposing = true;
				try 
				{
					IMBroker.LogEvent(this + " : (username: " + this.userName + ", port: " + parent.Port.ToString() + ") disposing.");
					lock(ss) 
					{
						ss.Close();
						Monitor.Pulse(ss);
					}
				} 
				catch (Exception e) 
				{
					IMBroker.LogEvent(this + " : oops in Dispose() : " + e.Message);
				}
			}
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// This is where the new thread begins executing... 
		/// </summary>
		public void DoConversation() 
		{
			// buffer of whats received
			string query = "";
			// the byte version (needed for sockets)
			byte[] buffer = new byte[1024];
			// bytes read
			bool zeroPacket = false;

			int ret;
//			IMBroker.LogEvent(this + " : Before socket lock");
			lock(this) 
			{
				this.Ready = true;
//				IMBroker.LogEvent(this + " " +DateTime.Now.Ticks.ToString() + " : locked this.");
//				IMBroker.LogEvent(this + " " + ID +" " + DateTime.Now.Ticks.ToString()+" : Waiting for client certificate on port " + parent.Port.ToString());
//				IMBroker.LogEvent(this + " " +DateTime.Now.Ticks.ToString() + " : entering wait.");
				Monitor.Pulse(this);
				Monitor.Wait(this);
			}

			if (this.IsDisposing) 
			{
				IMBroker.LogEvent(this + " " + ID +  " : I am disposing??");
				return;
			} else
				userName = cert.GetName();

			IMBroker.LogEvent(this + " : authenticated (port: " + parent.Port.ToString()+", username: " + userName +")");
			try 
			{
				ret = 0;

				SendLine("[Server] Welcome to secure chat, " + userName); // + cert.GetName().ToString());
				query = "";

				while(true && ss.Connected) 
				{	
					// just keep looping
					try 
					{
						//IMBroker.LogEvent("Trying to read at top of loop.");
						ret = ss.Receive(buffer, 0, buffer.Length, SocketFlags.None);
					} 
					catch (Exception e) 
					{
						IMBroker.LogEvent(this + " : Receive() : " + e.Message);
						//	IMBroker.LogEvent("Error while receiving data from client [" + e.Message + "].");
						//	IMBroker.LogEvent(e.StackTrace.ToString());
						break;
					}
					if (ret == 0) 
					{
						if (!zeroPacket)
							zeroPacket = true;
						else 
						{
							IMBroker.LogEvent("Client closed connection too soon.");
							ss.Close();
							break;
						}
					}
					query += Encoding.ASCII.GetString(buffer, 0, ret);
					//IMBroker.LogEvent("Received " + query);
					parent.Broadcast(userName,query);

					query = "";

				}
				IMBroker.LogEvent(this + " : client abort (socket closed or disposing).");
			} 
			catch (Exception eee) 
			{
				IMBroker.LogEvent(this + " : Exception: " + eee.Message + eee.StackTrace);
			}

			if (OnTerminate != null)
				OnTerminate(this);
		}

		/// <summary>
		/// Sends a line out on a secure socket.
		/// </summary>
		/// <param name="sock">Socket to send on</param>
		/// <param name="st">String to send out</param>
		public void SendLine(string st) 
		{
			
			if (ss == null || !ss.Connected) 
			{
				IMBroker.LogEvent(this + " : SendLine() : Send on unconnected socket.");
				return;
			}

			int ret = 0;
			while(ret != st.Length) 
			{
				//st =  st;
				try 
				{
					ret += this.ss.Send(Encoding.ASCII.GetBytes(st), ret, st.Length - ret, SocketFlags.None);
					IMBroker.LogEvent(this + " : sent out '" + st + "'");
				}
				catch (Org.Mentalis.Security.SecurityException e) 
				{

					IMBroker.LogEvent(this + " : SendLine() : " + e.Message);
				}
			}
		}

	}
}
