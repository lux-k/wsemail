using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Http;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using WSEmailProxy;
using WSInstantMessagingLibraries;

namespace WSEmailSMTPGateway
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>

	class SMTPGateway
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		///

		public static string ServerName = "";
		public delegate void ProcessorDelegate (TcpClient c);
		public MessageBuffer theBuffer = new MessageBuffer();
		private HttpChannel RemotingChannel = null;
		private static string MailRouter = "http://tower/WSEMailRouter/Router.ashx";
/*
		private void ProcessMessages() 
		{
			Console.WriteLine("Process messages thread started.");
			while (true) 
			{
				WSEmailMessage m = null;
				// this will block (hopefully) on the message buffer's mutex
				m = theBuffer.getMessage();
				Console.WriteLine(m);
				ForwardMessage(m);
			}
		}

		public static string ForwardMessage(WSEmailMessage theMessage) 
		{
			PennRoutingFilters.PennRoutingUtilities.LogEvent("Forwarding message through mailrouter: " + MailRouter);
			MailServerProxy m = new MailServerProxy(Global.MailRouter);
			string res = m.WSEmailSend(theMessage,(XmlElement)HttpSoapContext.RequestContext["MessageSignature"]);
			PennRoutingFilters.PennRoutingUtilities.LogEvent("Forwarding message through mailrouter: " + MailRouter + "... complete!");
			return res;
		}
*/
		[STAThread]
		static void Main(string[] args)
		{
			const int PORTNUM = 25;
			const int QUEUECHANNEL = 26;

			
/*		
			RemotingChannel = new HttpChannel( REMOTINGCHANNEL );

			ChannelServices.RegisterChannel( RemotingChannel );
			RemotingServices.Marshal(theBuffer,"MessageQueue");

			ThreadStart messageProcessorTS = new ThreadStart(ProcessMessages);
			messageProcessor = new Thread(messageProcessorTS);
			messageProcessor.Start();
*/
			Console.Write("Determining hostname... ");
			IPAddress IP = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0];
			ServerName = System.Net.Dns.GetHostByAddress(IP).HostName;
			Console.WriteLine("(" + ServerName + ") done.");
			Console.Write("Binding to listening socket " + PORTNUM.ToString() + "... ");
			PennRoutingFilters.PennRoutingUtilities.AddPennRoutingFilters(false);
			PennRoutingFilters.PennRoutingUtilities.AddWSEMailSignatureFilters(true,false);
			TcpListener serverSock = new TcpListener(PORTNUM);
			serverSock.Start();
			Console.WriteLine("done.");
			while (true) {
				try 
				{
					Socket newClient = serverSock.AcceptSocket();
					Console.WriteLine("Accepting new connection from " + newClient.RemoteEndPoint.ToString());
					newClient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 25000);
					Thread newThread = new Thread(new ThreadStart(new SMTPProcessor(newClient).Run));
					newThread.Start();
				} 
				catch (Exception e) 
				{
					Console.WriteLine(e.Message + "\r\n" + e.StackTrace);
				}
			}
		}

		public class SMTPProcessor
		{
			protected Socket client;
			public SMTPProcessor(Socket c) 
			{
				client = c;
			}
			public void Run() 
			{
				NetworkStream stream = new NetworkStream(client);
				StreamReader reader = new StreamReader(stream);
				StreamWriter writer = new StreamWriter(stream);
				writer.NewLine = "\n";
				writer.WriteLine("220 " + SMTPGateway.ServerName + " WSEMailtoSMTPGateway 1.0.0/1.0.0; " + PennRoutingFilters.PennRoutingUtilities.GetCurrentTime());
				writer.Flush();

				string s = "" ;
				string remote = "";
				string lookedup = "";
				Console.WriteLine("Received " + s);
				
				while (true) 
				{
					s= reader.ReadLine().ToLower();
					if (s.StartsWith("helo")) 
					{
						remote = s.Split(' ')[1];
						lookedup = "";
						try 
						{
							lookedup = System.Net.Dns.GetHostByName(remote).AddressList[0].ToString();
						} 
						catch 
						{
							Console.WriteLine("DNS lookup failed for " + remote);
						}

						Console.WriteLine("Received HELO from " + remote + " [" + lookedup + "]");
						break;
					} 
					else if (s.StartsWith("ehlo"))
					{
						Console.WriteLine("Informing remote server I don't understand ESMTP.");
						writer.WriteLine("502 Message type not supported.");
						writer.Flush();
					}
					else 
					{
						writer.WriteLine("Err. I was expecting a helo.");
						writer.Flush();
						writer.Close();
						return;	
					}
				}
				/*
									if (!lookedup.ToLower().Equals(remote.ToLower())) 
									{
										Console.WriteLine("DNS Mismatch (" + lookedup + " != " +  + ")");
										writer.WriteLine("Err. DNS doesn't match!");
										writer.Flush();
										client.Close();
									}
				*/
				writer.WriteLine("250 " + SMTPGateway.ServerName + " Good day, " + remote + " ["+lookedup + "], please continue...");
				writer.Flush();
				s = "";
				s = ReadLine(reader);
				string sender = "";

				if (s.Split(':')[0].ToLower().Equals("mail from")) 
				{
					sender = s.Split(':')[1];
					sender = sender.Trim();
					sender = sender.Trim(new char[] {'<','>','"'});
					sender = sender.Replace("@","!") + "@SMTPGateway"; 
					
					Console.WriteLine("Mail from " + sender);
					writer.WriteLine("250 2.1.0 " + sender + "... Sender ok");
					writer.Flush();

				} 
				else 
				{
					writer.WriteLine("Err. No sender specified.");
					writer.Flush();
					client.Close();
				}

				
				s = ReadLine(reader);
				string recip = "";

				if (s.Split(':')[0].ToLower().Equals("rcpt to")) 
				{
					recip = s.Split(':')[1];
					recip = recip.Trim();
					Console.WriteLine("Receipt to " + recip);
					writer.WriteLine("250 2.1.5 " + recip + "... Recipient ok");
					writer.Flush();
				}
				else 
				{
					writer.WriteLine("Err. No recip specified.");
					writer.Flush();
					client.Close();
				}

				s = ReadLine(reader);
				string message = "";

				if (s.ToLower().Equals("data")) 
				{
					writer.WriteLine("354 Enter mail, end with \".\" on a line by itself");
					writer.Flush();
				}
				else 
				{
					writer.WriteLine("Err. Expected data.");
					writer.Flush();
					client.Close();
				}
				s = "";
				while (s == "") 
				{
					s = ReadLine(reader);
					if (s.Equals("."))
						break;
					message += s + "\r\n";
					s="";
				}
				Console.WriteLine(message);
				writer.WriteLine("250 2.0.0 h6HFmmtl018271 Message accepted for delivery");
				writer.Flush();

				s = ReadLine(reader);
				if (s.ToLower().Equals("quit")) 
				{
					writer.WriteLine("Goodbye...");
					writer.Flush();
					client.Close();
				}
				
				recip = recip.Trim(new char[] {'<','>'});
				string r = recip.Split('@')[0];
				if (r.IndexOf(".") > 0)
					r = r.Replace(".","@");


				recip = r;
				WSEmailMessage m= new WSEmailMessage();
				m.Sender = sender; // "Kevin@MailServerA";
				m.Recipient = recip;
				int start = message.ToLower().IndexOf("subject: ") + "subject: ".Length;
				m.Subject = message.Substring(start, message.IndexOf("\n",start+1)-start);
				Console.WriteLine("subject = " + m.Subject);
				m.Body = message;
				m.Timestamp = PennRoutingFilters.PennRoutingUtilities.GetCurrentTime();
				MailServerProxy wsep = new MailServerProxy();
				try 
				{
					wsep.WSEmailSend(m,null);
				} 
				catch (Exception e) {
					Console.WriteLine(e.Message);
				}
				

			}
			public string ReadLine(StreamReader reader) 
			{
				string s = "";
				while (s == "") 
				{
					try 
					{
						s = reader.ReadLine();
					} 
					catch {}
				}
				return s;
			}
		}
	}
}
