/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using WSEmailProxy;
using MailServerInterfaces;

namespace WSSMTPGatewayLib
{
	/// <summary>
	/// Given a TCP socket, this object attempts to get a message from a remote host using SMTP and then send
	/// that message out on WSEmail.
	/// </summary>
	public class SMTPProcessor
	{
		/// <summary>
		/// Reference to the mail server configuration object
		/// </summary>
		protected IMailServer MailServer = null;
		/// <summary>
		/// Socket that communication occurs on.
		/// </summary>
		protected Socket client;
		/// <summary>
		/// Creates a new SMTP Processor on the given socket. This is typically called first, then a new thread
		/// is created on the Run() method.
		/// </summary>
		/// <param name="c">Socket (raw socket) of the connection.</param>
		public SMTPProcessor(Socket c, IMailServer m) 
		{
			client = c;
			MailServer =m;
		}

		/// <summary>
		/// Holds the information that is received over the SMTP connection
		/// </summary>
		protected MessageDetail Detail = new MessageDetail();

		/// <summary>
		/// Begins the SMTP transactions. Opens up stream readers/writers and tries to get a message out
		/// of the server.
		/// </summary>
		public void Run() 
		{
			// open the streams and present the smtp greeting line
			NetworkStream stream = new NetworkStream(client);
			StreamReader reader = new StreamReader(stream);
			StreamWriter writer = new StreamWriter(stream);
			writer.NewLine = "\n";
			WriteLine(writer,"220 " + SMTPGateway.ServerName + " WSEMailtoSMTPGateway 1.0.0/1.0.0; " + PennLibraries.Utilities.GetCurrentTime());

			string s = "" ;
			Console.WriteLine("Received " + s);

			// negotiate the helo/ehlo introduction of the client. we currently only support helo (old sendmail spec)	
			bool res = DoHeloDialog(reader,writer);
			if (!res)
				HangUp();
			res = DoMailFromDialog(reader,writer);
			if (!res)
				HangUp();

			res = DoRcptToDialog(reader,writer);		
			if (!res)
				HangUp();
		
			res = DoDataDialog(reader,writer);
			if (!res)
				HangUp();

			s = ReadLine(reader);
			WriteLine(writer,"Goodbye...");
			client.Close();

			WSEmailMessage m = BuildWSEmailMessage();				
			MailServer.DeliveryQueue.Enqueue(m,m.Sign(MailServer.Certificate),AuthenticatingTokenEnum.Internal);
			HangUp();
			return;
		}

		/// <summary>
		/// Closes the client connection and aborts processing on the thread.
		/// </summary>
		private void HangUp() 
		{
			try 
			{
				client.Close();
			} 
			catch {}
			Thread.CurrentThread.Abort();
		}

		/// <summary>
		/// Expects to read a mail from line
		/// </summary>
		/// <param name="r"></param>
		/// <param name="w"></param>
		/// <returns></returns>
		public bool DoMailFromDialog(StreamReader r, StreamWriter w) 
		{
			string s = "";
			s = ReadLine(r);
			string sender = "";

			// get the mail from string.
			if (s.Length > 0 && s.Split(':')[0].ToLower().Equals("mail from")) 
			{
				sender = s.Split(':')[1];
				sender = sender.Trim();
				// clean up extra whitespace and extra chars
				sender = sender.Trim(new char[] {'<','>','"'});
				//sender = sender.Replace("@","!") + "@SMTPGateway"; 
					
				// ack it
				SMTPGateway.LogEvent("Mail from " + sender);
				WriteLine(w,"250 2.1.0 " + sender + "... Sender ok");
				Detail.MAILFROMResponse = sender;
				return true;
			} 
			else 
			{
				WriteLine(w,"Err. No sender specified.");
				return false;
			}
		}

		/// <summary>
		/// Expects to negotiate multiple recipients
		/// </summary>
		/// <param name="r"></param>
		/// <param name="w"></param>
		/// <returns></returns>
		public bool DoRcptToDialog(StreamReader r, StreamWriter w) 
		{
			string s = ReadLine(r);
			ArrayList recips = new ArrayList();
			string recip = "";
			while (s.Split(':')[0].ToLower().Equals("rcpt to")) 
			{
				recip = s.Split(':')[1];
				recip = recip.Trim();
				recips.Add(recip);
				SMTPGateway.LogEvent("Receipt to " + recip);
				WriteLine(w,"250 2.1.5 " + recip + "... Recipient ok");
				s = ReadLine(r);
			}

			if (recips.Count == 0)
			{
				WriteLine(w,"500 Err. No recip specified.");
				return false;
			}

			Detail.RCPTTOResponse = recips;
			
			if (s.ToLower().Equals("data")) 
				return true;
			else
				return false;


		}

		/// <summary>
		/// Expects to get the data off the SMTP connection
		/// </summary>
		/// <param name="r"></param>
		/// <param name="w"></param>
		/// <returns></returns>
		public bool DoDataDialog(StreamReader r, StreamWriter w) 
		{
			WriteLine(w,"354 Enter mail, end with \".\" on a line by itself");
			string 	s = "", message = "";
			while (s == "") 
			{
				s = ReadLine(r);
				if (s.Equals("."))
					break;
				message += s + "\r\n";
				s="";
			}
			Detail.DATAResponse = message;
			SMTPGateway.LogEvent(message);
			WriteLine(w,"250 2.0.0 h6HFmmtl018271 Message accepted for delivery");
			return true;
		}

		/// <summary>
		/// Expects to negotiate the helo dialog
		/// </summary>
		/// <param name="r"></param>
		/// <param name="w"></param>
		/// <returns></returns>
		public bool DoHeloDialog(StreamReader r, StreamWriter w) 
		{
			string s = "", remote = "";
			while (true) 
			{
				s= r.ReadLine().ToLower();
				if (s.StartsWith("helo")) 
				{
					remote = s.Split(' ')[1];
					string lookedup = "";
					try 
					{
						// try to look up the ip address of the host from the helo
						// ie. helo <hostname>; nslookup hostname
						lookedup = System.Net.Dns.GetHostByName(remote).AddressList[0].ToString();
					} 
					catch 
					{
						SMTPGateway.LogEvent("DNS lookup failed for " + remote);
					}

					SMTPGateway.LogEvent("Received HELO from " + remote + " [" + lookedup + "]");
					Detail.HELOResponse = remote;
					WriteLine(w,"250 " + SMTPGateway.ServerName + " Good day, " + remote + " ["+lookedup + "], please continue...");
					break;
				} 
				else if (s.StartsWith("ehlo"))
				{
					// complain we don't talk esmtp
					SMTPGateway.LogEvent("Informing remote server I don't understand ESMTP.");
					WriteLine(w,"502 Message type not supported.");
				}
				else 
				{
					// otherwise, fail.
					WriteLine(w,"500 Err. I was expecting a helo.");
					return false;	
				}
			}
			return true;
		}

		/// <summary>
		/// Writes a line to a stream
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="s"></param>
		public void WriteLine(StreamWriter writer, string s) 
		{
			writer.WriteLine(s);
			writer.Flush();
		}
		/// <summary>
		/// Reads a line from the socket. If it fails, it will throw an exception.
		/// </summary>
		/// <param name="reader">StreamReader to read from</param>
		/// <returns>String read</returns>
		public string ReadLine(StreamReader reader) 
		{
			string s = "";
			while (s == "") 
			{
				if (!reader.BaseStream.CanRead)
					throw new Exception("Unable to read from stream!");

				try 
				{
					s = reader.ReadLine();

				} 
				catch {}

			}
			return s;
		}
		/// <summary>
		/// Builds the WSEmailMessage from the SMTP details
		/// </summary>
		/// <returns></returns>
		public WSEmailMessage BuildWSEmailMessage() 
		{
			WSEmailMessage m= new WSEmailMessage();
			m.Sender = Detail.MAILFROMResponse;
			foreach (string r in (string[])Detail.RCPTTOResponse.ToArray(typeof(string))) 
				m.Recipients.Add(r + "@" + MailServer.Name);

			int start = Detail.DATAResponse.ToLower().IndexOf("subject: ") + "subject: ".Length;
			if (start - 9 >= 0) 
			{
				//SMTPGateway.LogEvent("start is " + start.ToString());
				m.Subject = Detail.DATAResponse.Substring(start, Detail.DATAResponse.IndexOf("\n",start+1)-start);
			} 
			else 
			{
				m.Subject = "(no subject)";
			}
				
			m.Body = Detail.DATAResponse;
			m.Timestamp = DateTime.Now;
			return m;
		}

		/// <summary>
		/// The SMTP message detail object. This is used to translate to from SMTP to WSEmail
		/// </summary>
		public class MessageDetail 
		{
			/// <summary>
			/// The helo response... mostly useless.
			/// </summary>
			public string HELOResponse;
			/// <summary>
			/// The sender
			/// </summary>
			public string MAILFROMResponse;
			/// <summary>
			/// List of recipients
			/// </summary>
			public ArrayList RCPTTOResponse = new ArrayList();
			/// <summary>
			/// The data of the message, including the headers
			/// </summary>
			public string DATAResponse;
			/// <summary>
			/// Default empty constructor
			/// </summary>
			public MessageDetail() {}
		}
	}
}
