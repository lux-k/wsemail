using System.Threading;
using System;
using System.Text;
using WSSecureIMLib;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Org.Mentalis.Security.Ssl;
using Org.Mentalis.Security.Certificates;
using System.Xml;
using System.Drawing;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;
using PennRoutingFilters;

namespace WSSecureIMLib
{
	/// <summary>
	/// The client side of the secure IM connection. This takes care of connecting, sending and receiving messages from a 
	/// user to a secure proxy.
	/// </summary>
	public class ClientChatter
	{

		/// <summary>
		/// Delegate that is used for specifying which routine to execute on new messages.
		/// </summary>
		public delegate void ClientChatterEvent(string s);
		/// <summary>
		/// An event that is fire when new messages are available.
		/// </summary>
		public event ClientChatterEvent MessageAvailable;
		
		/// <summary>
		/// The SSL enabled socket that talks to the proxy.
		/// </summary>
		private SecureSocket SecureClient = null;
		/// <summary>
		/// The thread that listens to the socket for new messages.
		/// </summary>
		private Thread ListenerThread = null;
		/// <summary>
		/// The certificate identifying the user. (For SSL client authentication.)
		/// </summary>
		public System.Security.Cryptography.X509Certificates.X509Certificate myCert = null;

		private object Authenticated = new object();
		/// <summary>
		/// Default constructor, does nothing.
		/// </summary>
		public ClientChatter()
		{
		}

		/// <summary>
		/// Cleans up threads and other resources.
		/// </summary>
		~ClientChatter() 
		{
			CleanUp();
		}

		/// <summary>
		/// Cleans up threads and closes the socket.
		/// </summary>
		public void CleanUp() 
		{
			if (ListenerThread != null)
				ListenerThread.Abort();
			try 
			{
				if (SecureClient.Connected)
					SecureClient.Close();
			} 
			catch {}
		}

		/// <summary>
		/// Connects to a secure proxy.
		/// </summary>
		/// <param name="host">Hostname/IP of the proxy</param>
		/// <param name="port">Port to connect on</param>
		public void Connect(string host, int port) 
		{
			try 
			{
				SecurityOptions options = new SecurityOptions(
					SecureProtocol.Ssl3,	// use SSL3 or TLS1
					null,
					// Certificate.CreateFromX509Certificate(myCert),// do not use client authentication
					ConnectionEnd.Client,						// this is the client side
					CredentialVerification.Manual,				// let the SecureSocket verify the remote certificate automatically
					new CertVerifyEventHandler(OnVerify),		// not used with automatic certificate verification
					"MailServerA",								// this is the common name of the Microsoft web server
					SecurityFlags.Default,						// use the default security flags
					SslAlgorithms.SECURE_CIPHERS,				// only use secure ciphers
					new CertRequestEventHandler(OnCertRequest));										// do not process certificate requests.
				SecureClient = new SecureSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp, options);
				
				SecureClient.Connect(new IPEndPoint(Dns.Resolve(host).AddressList[0], port));
				InitializeSecureConnection();
			} 
			catch (Exception e) 
			{
				PostMessage(e.Message);
			}
		}

		public void OnCertRequest(SecureSocket socket, DistinguishedNameList acceptable, RequestEventArgs e) 
		{
			lock (Authenticated) 
			{
				PostMessage("Received certificate request from server.");
				e.Certificate = Certificate.CreateFromX509Certificate(myCert);
				PostMessage("Sent certificate");
				Monitor.Pulse(Authenticated);
			}
			return;
		}

		/// <summary>
		/// When new messages are received, this method gets them. It basically fires the MessageAvailable event
		/// with the given message.
		/// </summary>
		/// <param name="s">The message</param>
		private void PostMessage(string s) 
		{
			if (this.MessageAvailable != null)
				this.MessageAvailable.DynamicInvoke(new object[] {s});
		}

		/// <summary>
		/// Sends a message out to the proxy.
		/// </summary>
		/// <param name="s">Message to send.</param>
		public void SendMessage(string s) 
		{
			// posts it to the local window.
			PostMessage(">> " + s);

			PostMessage("Send: Client connected? " + SecureClient.Connected.ToString());
			if (SecureClient != null && SecureClient.Connected == true) 
			{
				if (s != "") 
				{
					// only send if the socket exists, it's connected and the message exists.
					byte[] reqBytes = Encoding.ASCII.GetBytes(s);
					int sent = SecureClient.Send(reqBytes, 0, reqBytes.Length, SocketFlags.None);
					while(sent != reqBytes.Length) 
					{
						sent += SecureClient.Send(reqBytes, sent, reqBytes.Length - sent, SocketFlags.None);
					}
					PostMessage("Sent " + sent.ToString() + " bytes.");
				}
			} 
		}


		/// <summary>
		/// Creates a thread to watch the socket input.
		/// </summary>
		private void InitializeSecureConnection() 
		{
			if (ListenerThread != null)
				ListenerThread.Abort();
			ListenerThread = new Thread(new ThreadStart(WatchInput));
			ListenerThread.Start();
//			SecureClient.Name = this.myCert.GetName();
		}

		/// <summary>
		/// The threadstart routine used on the socket input watching thread.
		/// </summary>
		private void WatchInput() 
		{
			string s = "";
			lock (Authenticated) 
			{
				Monitor.Wait(Authenticated);
			}
			while (true) 
			{
				try 
				{
					Console.WriteLine("Trying to read...");
					byte[] buffer = new byte[4096];
					int ret = 0;
					ret = SecureClient.Receive(buffer);
					Console.WriteLine("Ret = " + ret.ToString());
					s += Encoding.ASCII.GetString(buffer, 0, ret);
					if (s != "") 
					{
						Console.WriteLine("So="+s);
						PostMessage(s);
						s = "";
					}
				}
				catch (Exception e)
				{
					Console.WriteLine("Oops!");
					Console.WriteLine(e.Message);

				}
			}
		}

		/// <summary>
		/// The OnVerify call from the socket. This is called to verify the server. Basically it just shows
		/// certificate information.
		/// </summary>
		/// <param name="socket">SecureSocket calling this routine.</param>
		/// <param name="remote">Remote server cert</param>
		/// <param name="cc">Remote server certificate chain</param>
		/// <param name="e">Any extra arguments</param>
		protected void OnVerify(SecureSocket socket, Certificate remote, CertificateChain cc, VerifyEventArgs e) 
		{
			PostMessage("\r\nServer Certificate:\r\n-------------------");
			PostMessage(remote.ToString(true));
			PostMessage("\r\nServer Certificate Verification:\r\n--------------------------------\r\n    -> ");
			PostMessage(cc.VerifyChain(socket.CommonName, AuthType.Server).ToString() + "\r\n");
			return;
		}


	}
}
