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
	public delegate void SecureListenerTerminating(SecureListenerProcessor p);

	/// <summary>
	/// Contains the actual socket used to talk to a client, along with the setup and tear down of it.
	/// </summary>
	public class SecureListenerProcessor : IDisposable
	{
		public event SecureListenerTerminating OnTerminate = null;
		/// <summary>
		/// The listener socket.
		/// </summary>
		private SecureSocket s;
		/// <summary>
		/// The secure sockets that communications are being sent/received on.
		/// </summary>
		private SecureClient[] clients = new SecureClient[0];
		/// <summary>
		/// The port this client is listening on. (Should be the port number assigned by the ChannelRequestor.)
		/// </summary>
		public int Port;
		/// <summary>
		/// The thread that is waiting to accept new connections.
		/// </summary>
		public Thread thread;
		/// <summary>
		/// Used as a synchronization object.
		/// </summary>
		private object ClientUpdate = new object();
		/// <summary>
		/// Used to set state in the object.
		/// </summary>
		private bool IsDisposing = false;
		/// <summary>
		/// If we receive a cert request from the client
		/// </summary>
		/// <param name="socket"></param>
		/// <param name="acceptable"></param>
		/// <param name="e"></param>
		public void OnCertRequest(SecureSocket socket, DistinguishedNameList acceptable, RequestEventArgs e) 
		{
			IMBroker.LogEvent(this + " : received cert request from client?");
			return;
		}

		~SecureListenerProcessor() 
		{
			this.Dispose();
		}

		/// <summary>
		/// Creates a new secure listener on the specified port. This will setup a SSLv3/TLS1 socket.
		/// </summary>
		/// <param name="p">Port to listen on</param>
		public SecureListenerProcessor (int p) 
		{
			try 
			{
				// create a new security options object and specify that it's a server
				// require mutal authentication (ie. the client has to present a cert)
				// we'll do our own verifying.
				// create the socket and bind to an endpoint...
				SecurityOptions options = new SecurityOptions(
					SecureProtocol.Ssl3  | SecureProtocol.Tls1,	// use SSL3 or TLS1
					IMBroker.ServerCert,
					// Certificate.CreateFromX509Certificate(myCert),// do not use client authentication
					ConnectionEnd.Server,						// this is the client side
					CredentialVerification.Manual,				// let the SecureSocket verify the remote certificate automatically
					new CertVerifyEventHandler(OnVerify),		// not used with automatic certificate verification
					"",								// this is the common name of the Microsoft web server
					SecurityFlags.MutualAuthentication,						// use the default security flags
					SslAlgorithms.SECURE_CIPHERS,				// only use secure ciphers
					new CertRequestEventHandler(OnCertRequest));

				s = new SecureSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp, options);
				IPEndPoint ep = new IPEndPoint(System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0],p);
				s.Bind(ep);
				Port = p;
			} 
			catch (Exception e) 
			{
				IMBroker.LogEvent(this + " : oops in constructor/port binding: " + e.Message);
			}

		}

		/// <summary>
		/// Used to clean up this object and all the clients it created.
		/// </summary>
		public void Dispose() 
		{
			if (!IsDisposing) 
			{
				this.IsDisposing = true;
				IMBroker.LogEvent(this + " : (port: " + this.Port.ToString() + ") disposing; killing clients");
				try 
				{
					s.Close();
					s = null;
				} 
				catch (Exception e) { IMBroker.LogEvent(this + " : oops in Dispose(): " + e.Message+e.StackTrace); }
			
				foreach (SecureClient l in clients)  
				{
					l.Dispose();
					l.t.Abort();
				}
			}

			GC.SuppressFinalize(this);
		}


		/// <summary>
		/// Adds a client to the collection of created clients (so they can be cleaned up later).
		/// </summary>
		/// <param name="l">Client to add</param>
		private void AddListener(SecureClient l) 
		{
			lock(clients) 
			{
				ArrayList a = new ArrayList(this.clients);
				a.Add(l);
				this.clients = (SecureClient[])a.ToArray(typeof(SecureClient));
			}
		}

		/// <summary>
		/// Removes a secureclient from the list of clients
		/// </summary>
		/// <param name="l"></param>
		private void RemoveListener(SecureClient l) {
			lock(clients) 
			{
				//IMBroker.LogEvent("SLP clients before prune: " + this.clients.Length.ToString());
				ArrayList a = new ArrayList(this.clients);
				a.Remove(l);
				l.Dispose();
				this.clients = (SecureClient[])a.ToArray(typeof(SecureClient));
				//IMBroker.LogEvent("SLP clients after prune: " + this.clients.Length.ToString());
				if (this.clients.Length == 0) 
				{
					if (this.OnTerminate != null)
						this.OnTerminate(this);
				}
			}
		}

			
		/// <summary>
		/// Called when a client presents a certificate. Throw an exception in here or close the socket
		/// if unhappy with the client's data.
		/// </summary>
		/// <param name="socket">Client socket</param>
		/// <param name="remote">Certificate they presented</param>
		/// <param name="cc">Certificate chain built from the certificate</param>
		/// <param name="e">The arguments</param>
		protected void OnVerify(SecureSocket socket, Certificate remote, CertificateChain cc, VerifyEventArgs e) 
		{
			// lock the monitor...
			//			IMBroker.LogEvent(this + " : Received OnVerify() call...");
			lock (this) 
			{
				//IMBroker.LogEvent(this + " " +DateTime.Now.Ticks.ToString() + " : locked on OnVerify().");
				//IMBroker.LogEvent(this + " : port " + this.Port.ToString() + " has identified itself with " + remote.GetName());
				///
				/// cert check code goes here
				///
				e.Valid = true;
				// set that the verification succeeded and pulse the monitor
				foreach (SecureClient s in clients) 
				{
					try 
					{

						if (s != null && s.ss.RemoteEndPoint.Equals(socket.RemoteEndPoint)) 
						{
							lock(s) 
							{
								if (!s.Ready) 
								{
									IMBroker.LogEvent(this +" " + s.ID + " : Got to OnVerify before secureclient was ready; waiting...");
									Monitor.Wait(s);
								}
								s.cert = remote;
								Monitor.Pulse(s);
//								IMBroker.LogEvent(this + " " + s.ID +  " " + DateTime.Now.Ticks.ToString() + " : pulsing.");
								break;
							}
						}
					}
					catch (Exception ex) 
					{
						IMBroker.LogEvent(this + " : ERROR: " + ex.Message + ex.StackTrace);
					}
				}
				//IMBroker.LogEvent(this + " " +DateTime.Now.Ticks.ToString() + " : released on OnVerify().");
			} 
			return;
		}


		/// <summary>
		/// Sends a message to all attached clients.
		/// </summary>
		/// <param name="s">string to send</param>
		public void Broadcast(string username, string s) 
		{
			lock(ClientUpdate) 
			{
				foreach (SecureClient l in clients)  
				{
					if (!l.userName.Equals(username))
						l.SendLine("["+username+"] "+s);
				}
			}
		}

		/// <summary>
		/// Waits to accept a connection and then creates a new secure client with the accepted socket.
		/// </summary>
		public void Go() 
		{
		
			s.Listen(10);
			IMBroker.LogEvent(this + " : SSL v3/TLS server now listening on port " + Port.ToString() + ".");

			while(!IsDisposing) 
			{
				// accept it and send a message right back so i know the
				// thing is working.
				try 
				{
					SecureSocket ss = (SecureSocket)s.Accept();
					SecureClient l =  new SecureClient(ss,this);
					l.OnTerminate += new SecureClientTerminating(this.RemoveListener);
					Thread t = new Thread(new ThreadStart(l.DoConversation));
					this.AddListener(l);
					t.Name = "Listener" + this.clients.Length.ToString();
					IMBroker.LogEvent(this + " : Accepted a connection on port " + Port.ToString() + 
						" (total connections: " + this.clients.Length.ToString() + ") to "
						+ ss.RemoteEndPoint.Serialize().ToString());
					l.t = t;
					l.t.Start();
//					IMBroker.LogEvent(this + " : Got released on accept.");
//					IMBroker.LogEvent(this + " " +DateTime.Now.Ticks.ToString() + " : received pulse in accept.");
//					IMBroker.LogEvent(this + " " +DateTime.Now.Ticks.ToString() + " : unlocked this in accept.");

					
				} 
				catch (Exception e) 
				{
					IMBroker.LogEvent(this + " : Oops on Go().Accept() .. (probably a SLP shutdown). ("+e.Message+")");
				}
				if (!IsDisposing)
					IMBroker.LogEvent(this + " : Waiting for another connection on port " + this.Port.ToString());
			}
		}
	}
}


