/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

// #define MULTI

using System;
using System.Threading;
using System.Configuration;
using WSEmailProxy;
using System.Collections;
using MailServerInterfaces;
using System.Xml;
using System.Web.Mail;
using DNSResolver;
using Microsoft.Web.Services2.Security.Tokens;
using Microsoft.Web.Services2.Security;


namespace WSEmailServer
{
	/// <summary>
	/// Message delivery queue that will attempt local and remote delivery of messages.
	/// </summary>
	public class MessageQueue : IMailQueue
	{
		System.Diagnostics.PerformanceCounter perfQueueLen;

		/// <summary>
		/// Number of delivery threads to run (currently only one is supported).
		/// </summary>
		protected static int MaxThreads = 5;



#if (MULTI)
		/// <summary>
		/// Link to the running delivery threads.
		/// </summary>
		protected Thread[] QueueThreads = new Thread[MaxThreads];
#else
		/// <summary>
		/// Link to the running delivery thread.
		/// </summary>
		protected Thread QueueThread = null;

#endif
		/// <summary>
		/// Number of messages forwarded since the queue started running.
		/// </summary>
		protected int MessagesForwarded = 0;
		/// <summary>
		/// Number of messages bounced since the queue started running.
		/// </summary>
		protected int MessagesBounced = 0;
		/// <summary>
		/// The queue that holds messages to be delivered.
		/// </summary>
		protected ArrayList MainQueue = new ArrayList();
		/// <summary>
		/// MailServer proxy used to talk to remote mail servers and routers.
		/// </summary>
		protected MailServerProxy m = new MailServerProxy();
		/// <summary>
		/// Reference to the mail server configuration.
		/// </summary>
		protected IMailServer ServerConf = null;

		/// <summary>
		/// Default constructor that doesn't do anything.
		/// </summary>
		public MessageQueue()
		{
			try 
			{
				perfQueueLen = new System.Diagnostics.PerformanceCounter("WSEmail Server","DeliveryQueueLength",false);
				perfQueueLen.RawValue = 0;
			} 
			catch {}
		}

		/// <summary>
		/// Private. Don't use me. Really. Back off.
		/// </summary>
		protected bool _init = false, _shut = false;

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
		/// Initializes the mail server configuration's location and starts up the delivery thread.
		/// </summary>
		/// <param name="c">MailServer configuration</param>
		/// <returns>True (bool)</returns>
		public bool Initialize(IMailServer c) 
		{
			ServerConf = c;
			c.Log(MailServerLogType.ServerInfo,this + " : Mail queuing starting up...");
			m.Url = ServerConf.Router;
			m.SecurityToken = new X509SecurityToken(ServerConf.Certificate);
#if MULTI
			for (int i = 0; i < MaxThreads; i++) 
			{
				QueueThreads[i] = new Thread(new ThreadStart(QueueThreadCode));
				QueueThreads[i].Start();
			}
#else
			QueueThread = new Thread(new ThreadStart(QueueThreadCode));
			QueueThread.Start();
#endif
			_init = true;
			return true;
		}

		/// <summary>
		/// Returns the number of messages in the queue and the number of messages forwarded.
		/// </summary>
		/// <returns></returns>
		public string GetStatus() 
		{
			string s = "Messages in queue: " + MainQueue.Count.ToString() + ", Messages forwarded: " + MessagesForwarded.ToString() + ", Messages bounced: " + MessagesBounced.ToString();
			s += "\nReceived external bytes: " + ServerConf.Stats.Received.External.ByteCount.ToString() + 
				", Recevied external messages: " + ServerConf.Stats.Received.External.MessageCount.ToString();
			return s;
		}

		/// <summary>
		/// Returns the plugin type.
		/// </summary>
		/// <returns>PluginType.Service</returns>
		public MailServerInterfaces.PluginType PluginType  
		{
			get 
			{
				return MailServerInterfaces.PluginType.Service;
			}
		}

		/// <summary>
		/// Delivery thread code. This is bound to a thread and run continuously to empty the delivery queue. It
		/// uses monitors to avoid busy waiting.
		/// </summary>
		protected void QueueThreadCode () 
		{
			while (true) 
			{
				//ServerConf.Log(this + " : Waiting....");
				Monitor.Enter(MainQueue);
				//ServerConf.Log(this + " : Got MainQueue lock....");
				if (MainQueue.Count == 0) 
					Monitor.Wait(MainQueue);
				// now there is a message.
				//ServerConf.Log(this + " : Was renotified....");
				WSEmailMessage w = (WSEmailMessage)MainQueue[0];
				XmlElement sig = (XmlElement)MainQueue[1];
				AuthenticatingTokenEnum t = (AuthenticatingTokenEnum)MainQueue[2];
				MainQueue.RemoveAt(0);
				MainQueue.RemoveAt(0);
				MainQueue.RemoveAt(0);

				if (perfQueueLen != null)
					perfQueueLen.IncrementBy(-1);
				Monitor.Exit(MainQueue);
				DeliverMessage(w,sig,t);
				//ServerConf.Log(this + " : Finished delivery. Messages left: " + MainQueue.Count.ToString());
				
			}
		}

		/// <summary>
		/// Enqueues a message into the delivery queue for delivery. It will be delivered as soon as the
		/// items ahead of it are processed.
		/// </summary>
		/// <param name="m">WSEmailMessage</param>
		/// <param name="sig">XmlElement signature of the message</param>
		/// <returns>WSEmailStatus indicated message was queued</returns>
		public WSEmailStatus Enqueue(WSEmailMessage m, XmlElement sig, AuthenticatingTokenEnum t) 
		{
			Monitor.Enter(MainQueue);
			if (perfQueueLen != null)
				perfQueueLen.Increment();
			MainQueue.Add(m);
			MainQueue.Add(sig);
			MainQueue.Add(t);
			Monitor.Pulse(MainQueue);
			Monitor.Exit(MainQueue);
			ServerConf.Log(MailServerLogType.ServerInfo,this + " : Message enqueued for delivery.");
			return new WSEmailStatus(200,"Message queued for delivery.");
		}


		/// <summary>
		/// Determines the next hop a message should take, based on its destination. It will query DNS and
		/// default to a hard-coded router if unable to make heads or tails of DNS responses.
		/// </summary>
		/// <param name="destination">The user@host destination</param>
		/// <returns>A string, that is the URL to send the message to or NULL if problems.</returns>
		protected string DetermineNextHop(string destination) 
		{
			/*
			string[] parts = null;
			parts = destination.Split(new char[] {'@'});

			if (parts == null || parts.Length != 2) 
			{
				ServerConf.Log(this + " : Error parsing address: " + destination + ". Can't figure out the domain.");
				return null;
			}
			*/
			// now we have a domain... try to get some records for it.
			Resolver.DNSRecord[] Records = null;
			// are we using a configured dns server or the defaults?
			if (ServerConf.DnsServer != null && ServerConf.DnsServer.Length > 0) 
			{
				ServerConf.Log(MailServerLogType.ServerDebug,this + " : DNS : Performing DNS query.\nRecord type: SRV\nQuestion: _wsemail._tcp."+destination+"\nServer: " + ServerConf.DnsServer);
				Records = Resolver.Query("_wsemail._tcp."+destination, Resolver.RecordTypes.DNS_TYPE_SRV, ServerConf.DnsServer);
			} 
			else 
			{
				ServerConf.Log(MailServerLogType.ServerDebug,this + " : DNS : Performing DNS query.\nRecord type: SRV\nQuestion: _wsemail._tcp."+destination+"\nServer: (default)");
				Records = Resolver.Query("_wsemail._tcp."+destination, Resolver.RecordTypes.DNS_TYPE_SRV);
			}

			if (Records == null || Records.Length == 0) 
			{
				ServerConf.Log(MailServerLogType.ServerWarning,this + " : DNS : No records returned from DNS.");
				return null;
			}


			try 
			{
				foreach (Resolver.DNSRecord r in Records)
					ServerConf.Log(MailServerLogType.ServerDebug, this + " : DNS (Answer) : " + r.ToString());

				for (int i = 0; i < Records.Length; i++) 
				{
					if (Records[i] is Resolver.SRVRecord) 
					{
						Resolver.SRVRecord rec = (Resolver.SRVRecord)Records[i];
						try 
						{
							Uri u = new Uri(rec.Target);
							ServerConf.Log(MailServerLogType.ServerDebug, this + " : DNS : Returning this as target: " + rec.Target);
							return rec.Target;
						} 
						catch (Exception ee) 
						{
							ServerConf.Log(MailServerLogType.ServerDebug, this + " : DNS : Target (" + rec.Target + ") not a URL! (Exception: " + ee.Message + ")");
						}
					}
				}
			} 
			catch (Exception e) 
			{
				ServerConf.Log(MailServerLogType.ServerError, this + " : DNS : Unable to recast DNS response into array of SRV records. Returning router.\nException: " + e.Message);
				
			} 
			ServerConf.Log(MailServerLogType.ServerWarning, this + " : DNS : Giving up and just returning the router.");
			//return ServerConf.Router;
			return null;
		}

		/// <summary>
		/// Bounces a message back to the sender with a reason.
		/// </summary>
		/// <param name="theMessage">Message to return</param>
		/// <param name="reason">Reason it's being returned</param>
		public void BounceMessage(WSEmailMessage theMessage, string reason) 
		{
			try 
			{
				WSEmailMessage m = new WSEmailMessage();

				m.Recipients.Add(theMessage.Sender);
				m.Sender = "postmaster@"+ServerConf.Name;
				m.Timestamp = DateTime.Now;
				m.Subject = "Delivery failure";
				m.Body = "Delivery of the following message failed.\n\n"+reason+"\n\nOriginal message:\n"+theMessage.Body;
				this.Enqueue(m,m.Sign(ServerConf.Certificate),AuthenticatingTokenEnum.Internal);
				MessagesBounced++;
			} 
			catch (Exception e) 
			{
				ServerConf.Log(MailServerLogType.ServerError,this + " : Unable to send bounce message! (Exception: " + e.Message+")");
			}

		}

		/// <summary>
		/// Attempts delivery of a message. This is called within the DeliveryThread's code. It will attempt
		/// to directly serialize the message to the local server, if that's where the message is destined instead
		/// of sending it through a router.
		/// </summary>
		/// <param name="theMessage">Message to deliver</param>
		/// <param name="sig">Signature of message</param>
		protected void DeliverMessage(WSEmailMessage theMessage,XmlElement sig, AuthenticatingTokenEnum tok) 
		{
			foreach (string recipient in theMessage.Recipients.GetDistinctDestinations()) 
			{
				if (recipient.ToLower().Equals(ServerConf.Name.ToLower())) 
				{
					ServerConf.Log(MailServerLogType.ServerDebug, this + " : Queue attempting local delivery for recipient domain '" + recipient + "'; handing off to LocalMTA.");
					WSEmailStatus res = ServerConf.LocalMTA.DeliverMessage(theMessage,sig);
					ServerConf.Log(MailServerLogType.ServerDeliveryStatus, this + " : LocalMTA delivery complete; response code: " + res.ResponseCode.ToString() + " (" + res.Message + ")");
				} 
				// only forward messages to other servers if they are local users
				// federated users using that to send email are assumed to act as their own server...
				else if (tok != AuthenticatingTokenEnum.FederatedIdentity && 
					theMessage.Sender.ToLower().EndsWith("@" + ServerConf.Name.ToLower()))
				{
					ServerConf.Log(MailServerLogType.ServerDebug, this + " : Beginning delivery of message.");
					string dest = DetermineNextHop(recipient);
					if (dest != null) 
					{
						ServerConf.Log(MailServerLogType.ServerDebug, this + " : Forwarding message through: " + dest);
						WSEmailStatus res;
						try 
						{
							m.Url = dest;
							if ( (theMessage.MessageFlags & WSEmailFlags.Precedence.EncryptedDelivery) ==
								WSEmailFlags.Precedence.EncryptedDelivery) 
							{
								MailServerProxy proxy = new MailServerProxy(dest);
								proxy.SecurityToken = m.SecurityToken;
								proxy.AddServerEncryption();
								res = proxy.WSEmailSend(theMessage,sig);
							} else 
								res = m.WSEmailSend(theMessage,sig);

							ServerConf.Stats.Sent.External.MessageCount++;
							ServerConf.Log(MailServerLogType.ServerDeliveryStatus, this + " : Forwarding message through: " + dest + "... complete (code: " + res.ResponseCode.ToString()+")");
							this.MessagesForwarded++;
						} 
						catch (Exception e) 
						{
							ServerConf.Log(MailServerLogType.ServerDeliveryStatus, this + " : Unable to deliver message to destination. (Exception: " + e.Message +")");
							BounceMessage(theMessage,e.Message);
						}
					} 
					else 
					{

						//ServerConf.Log(MailServerLogType.ServerError, this + " : Destination URL is null. Not attempting delivery.. bouncing message.");
						//BounceMessage(theMessage,"No route to host");
						if (ServerConf.SMTPRelay != null && ServerConf.SMTPRelay.Length > 0) 
						{
							MailMessage mailObj = new MailMessage();

							if(SmtpMail.SmtpServer.Length == 0)
								SmtpMail.SmtpServer= ServerConf.SMTPRelay;

							mailObj.From = "luxk@saul.cis.upenn.edu";
							mailObj.To = theMessage.Recipients.ToString();
							mailObj.Subject = theMessage.Subject;
							mailObj.Body = theMessage.Body;
							mailObj.BodyFormat = MailFormat.Html;

							// Send the email using the SmtpMail object
							SmtpMail.Send(mailObj);
						} 
						else 
						{
							BounceMessage(theMessage,"Unable to deliver to recipient because the destination was not found.");
						}
					}
				}
			}
		}
		/// <summary>
		/// Suspends the delivery threads.
		/// </summary>
		public void Suspend() 
		{
#if MULTI
			for (int i = 0; i < MaxThreads; i++) 
			{
				if (QueueThreads[i].ThreadState == ThreadState.Running) 
					QueueThreads[i].Suspend();
			}
#else
			if (QueueThread.ThreadState == ThreadState.Running)
				QueueThread.Suspend();
#endif
		}

		/// <summary>
		/// Resumes suspended delivery threads.
		/// </summary>
		public void Resume() 
		{
#if MULTI
			for (int i = 0; i < MaxThreads; i++) 
			{
				if (QueueThreads[i].ThreadState == ThreadState.Suspended)
					QueueThreads[i].Resume();
			}
#else
			if (QueueThread.ThreadState == ThreadState.Suspended)
				QueueThread.Resume();
#endif

		}

		/// <summary>
		/// Shuts down the delivery threads and cleans up resources.
		/// </summary>
		/// <returns></returns>
		public bool Shutdown() 
		{
#if MULTI
			for (int i = 0; i < MaxThreads; i++)
				QueueThreads[i].Abort();
#else
			if (QueueThread != null)
				QueueThread.Abort();
#endif
			_shut = true;
			return true;
		}
	}
}
