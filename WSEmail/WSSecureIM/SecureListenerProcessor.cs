using System.Collections;
using System.Text;
using System.Windows.Forms;
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
	/// <summary>
	/// Contains the actual socket used to talk to a client, along with the setup and tear down of it.
	/// </summary>
	public class SecureListenerProcessor 
	{
		/// <summary>
		/// The listener socket.
		/// </summary>
		private SecureSocket s;
		/// <summary>
		/// The secure socket that communications are being sent/received on.
		/// </summary>
		public SecureSocket ss;
		/// <summary>
		/// The port this client is listening on. (Should be the port number assigned by the ChannelRequestor.
		/// </summary>
		private int port;
		/// <summary>
		/// The peer this client is talking to.
		/// </summary>
		public SecureListenerProcessor Peer;
		/// <summary>
		/// Used to signal other parts of the program that the client certificate has been received and validated. (It's a monitor.)
		/// </summary>
		private object ClientAuthenticated = new object();
		/// <summary>
		/// Creates a new secure listener on the specified port. This will setup a SSLv3/TLS1 socket.
		/// </summary>
		/// <param name="p">Port to listen on</param>
		public SecureListenerProcessor (int p) 
		{
			try 
			{
				// create a new security options object and specify that it's a server
				SecurityOptions so = new SecurityOptions(SecureProtocol.Ssl3, IMBroker.ServerCert, Org.Mentalis.Security.Ssl.ConnectionEnd.Server);
				// require mutal authentication (ie. the client has to present a cert)
				so.Flags = SecurityFlags.MutualAuthentication;
				// we'll do our own verifying.
				so.Verifier = new CertVerifyEventHandler(OnVerify);
				so.VerificationType = CredentialVerification.Manual;
				// create the socket and bind to an endpoint...
				s = new SecureSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp, so);
				IPEndPoint ep = new IPEndPoint(System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].Address,p);
				s.Bind(ep);
				port = p;
			} 
			catch (Exception e) 
			{
				IMBroker.LogEvent("Error in SecureListenerProcessor: " + e.Message);
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
			lock(ClientAuthenticated) 
			{
				IMBroker.LogEvent("Port " + this.port.ToString() + " has identified itself with " + remote.GetName());
				///
				/// cert check code goes here
				///
				e.Valid = true;
				// set that the verification succeeded and pulse the monitor
				Monitor.Pulse(ClientAuthenticated);
			}
			return;
		}

		/// <summary>
		/// Sends a line out on a secure socket.
		/// </summary>
		/// <param name="sock">Socket to send on</param>
		/// <param name="st">String to send out</param>
		public void SendLine(SecureSocket sock, string st) 
		{
			int ret = 0;
			while(ret != st.Length) 
			{
				try 
				{
					ret += sock.Send(Encoding.ASCII.GetBytes(st), ret, st.Length - ret, SocketFlags.None);
				}
				catch (Org.Mentalis.Security.SecurityException e) 
				{

					IMBroker.LogEvent(e.Message);
				}
			}
		}

		/// <summary>
		/// Waits to accept a connection and then relays messages back and forth between 2 parties. This should
		/// check client certificates and that the socket hasn't been reused but doesn't at the moment.
		/// </summary>
		public void Go() 
		{
		
			s.Listen(1);
			IMBroker.LogEvent("SSL v3/TLS server now listening on port " + port.ToString() + ".");
			// buffer of whats received
			string query = "";
			// the byte version (needed for sockets)
			byte[] buffer = new byte[1024];
			// bytes read
			int ret;
			// loop probably unnecessary; we dont want to accept multiple connections, right?
			while(true) {
				// accept it and send a message right back so i know the
				// thing is working.
				ss = (SecureSocket)s.Accept();
				IMBroker.LogEvent("Connection to port " + port.ToString() + " established.");
				lock(ClientAuthenticated) 
				{
					IMBroker.LogEvent("Waiting for client certificate on port " + port.ToString());
					Monitor.Wait(ClientAuthenticated);
				}

				IMBroker.LogEvent("Received certificate on port " + port.ToString());

				ret = 0;
				if (ss.RemoteCertificate != null)
					query = "Hello!" + ss.RemoteCertificate.GetDistinguishedName();
				else
					query = "Welcome to secure chat.";

				SendLine(ss,query);
				query = "";

				while(true) { // just keep looping
					try {
						//IMBroker.LogEvent("Trying to read at top of loop.");
						ret = ss.Receive(buffer, 0, buffer.Length, SocketFlags.None);
					} catch (Exception e) {
						IMBroker.LogEvent("Error while receiving data from client [" + e.Message + "].");
						IMBroker.LogEvent(e.StackTrace.ToString());
						break;
					}
					if (ret == 0) {
						IMBroker.LogEvent("Client closed connection too soon.");
						ss.Close();
						break;
					}
					query += Encoding.ASCII.GetString(buffer, 0, ret);
					IMBroker.LogEvent("Received " + query);
					if (Peer.ss != null && Peer.ss.Connected) {
						SendLine(Peer.ss,query);
						IMBroker.LogEvent("Sent " + query + " out on peer.");
						query = "";
					}

					if (ss.RemoteCertificate != null)
						IMBroker.LogEvent(ss.RemoteCertificate.GetName());
					if (s.RemoteCertificate != null)
						IMBroker.LogEvent(s.RemoteCertificate.GetName());


				}
				IMBroker.LogEvent("Waiting for another connection...");
			}
		}
	}
}
