using Org.Mentalis.Security.Ssl;
using Org.Mentalis.Security.Certificates;
using System;
using System.Runtime.Remoting;
using System.Collections;
using System.Threading;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels;

namespace WSSecureIMLib
{
	/// <summary>
	/// Coordinates allocating ports, opening those ports and proxying conversations between users. For the
	/// remoting stuff to work, it's expected that a named HTTP remoting channel has already been setup.
	/// </summary>
	public class IMBroker 
	{


		/// <summary>
		/// Holds the port allocator object.
		/// </summary>
		private ChannelRequestor Channels = new ChannelRequestor();
		/// <summary>
		/// Holds all the channel requests (after they are pulled off the remoted object).
		/// </summary>
		private ChannelRequestQueue ChannelRequests = new ChannelRequestQueue();
		/// <summary>
		/// The queue that is remoted so that a mail server can request channels on behalf of a client.
		/// </summary>
		private RemoteableChannelRequest Remoteable = new RemoteableChannelRequest();
		/// <summary>
		/// Object used as a holding tank for a monitor.
		/// </summary>
		private object QueueSignal = new object();
		/// <summary>
		/// Thread that watches the remoting queue and processes incoming requests.
		/// </summary>
		private Thread QueueWatcherThread;
		/// <summary>
		/// Contains all the threads started by this object.
		/// </summary>
		private ArrayList WorkerThreads = new ArrayList();
		/// <summary>
		/// An object to log to. Currently two types are supported: null (means use console) and System.Windows.Forms.TextBox
		/// </summary>
		public static object LogTo;
		/// <summary>
		/// A certificate that identifies this chat proxy.
		/// </summary>
		public static Certificate ServerCert = null;

		//		public delegate void LogEvent(string t);

		/// <summary>
		/// Logs an event, depending on what kind of logging object is setup.
		/// </summary>
		/// <param name="s">Message to log.</param>
		public static void LogEvent(string s) 
		{
			if (LogTo is System.Windows.Forms.TextBox) 
			{
				System.Windows.Forms.TextBox t = (System.Windows.Forms.TextBox)LogTo;
				t.Text += s + "\r\n";
				t.SelectionStart=t.Text.Length;
				t.ScrollToCaret();
			}
			else
				Console.WriteLine(s);
		}

		public IMBroker() 
		{
			StartUp();
		}

		public IMBroker(object o) 
		{
			IMBroker.LogTo = o;
			StartUp();
		}

		private void StartUp() 
		{
			// remotes the remoteable queue
			RemotingServices.Marshal(Remoteable,"RequestChannel");

			// adds an event handler to the remoteable buffer. this copies over items to the local buffer as fast
			// as possible.
			Remoteable.RequestAdded += new RemoteableChannelRequest.VoidDelegate(RemoteRequestAvailable);

			// starts a new thread to watch the local queue
			ThreadStart ts = new ThreadStart(QueueWatcher);
			QueueWatcherThread = new Thread(ts);
			QueueWatcherThread.Start();
			
			/*
			CertificateStore cs = new CertificateStore("MY");
			string certID = "204C83892C0DB644402E0F231C042AD489B22020";
			byte[] theID = new byte[certID.Length / 2];
			for (int i = 0; i < certID.Length; i +=2)
			theID[i/2] = Convert.ToByte(certID.Substring(i,2),16);
			*/

			// loads a copy of the server cert
			ServerCert = Certificate.CreateFromX509Certificate(PennRoutingFilters.PennRoutingUtilities.GetSecurityToken("SecureChatA",true).Certificate);
			LogEvent("Server certificate loaded: " + ServerCert.GetName());
		}

		/// <summary>
		/// The thread start for the Queue Watcher thread. It watches the channel requests (local) and then
		/// processes the queue when new stuff is placed in there. It will wait for monitor signals if it's empty.
		/// </summary>
		private void QueueWatcher() 
		{
			while (true) 
			{
				// if there are no requests
				if (ChannelRequests.PendingRequests == 0) 
				{
					// wait for a monitor signal
					lock (QueueSignal) 
					{
						Monitor.Wait(QueueSignal,Timeout.Infinite);
					}
				}
				// otherwise process a request
				ProcessChannelRequest(ChannelRequests.GetRequest());
			}
		}

		/// <summary>
		/// Processes one channel request and then puts it back out on the remoteable object for the client to retrieve.
		/// </summary>
		/// <param name="r">Request to process</param>
		private void ProcessChannelRequest(ChannelRequest r) 
		{
			r = Channels.RequestChannel(r);
			Remoteable.Request = r;
			OpenPorts(r);
			lock(Remoteable) 
			{
				Monitor.Pulse(Remoteable);
			}

		}

		/// <summary>
		/// Opens the secure ports, creating new threads to watch each port.
		/// </summary>
		/// <param name="r"></param>
		private void OpenPorts(ChannelRequest r) 
		{
			SecureListenerProcessor sender = new SecureListenerProcessor(r.SenderPort);
			Thread t = new Thread(new ThreadStart(sender.Go));
			WorkerThreads.Add(t);
			t.Start();

			SecureListenerProcessor receiver = new SecureListenerProcessor(r.RecipientPort);
			t = new Thread(new ThreadStart(receiver.Go));
			WorkerThreads.Add(t);
			t.Start();
		
			sender.Peer = receiver;
			receiver.Peer = sender;

			LogEvent("Connecting " + r.Sender + " to port " + r.SenderPort);
			LogEvent("Connecting " + r.Recipient + " to port " + r.RecipientPort);

		}

		/// <summary>
		/// The routine that is executed when new remote requests are available. Basically just pulls the request
		/// off the remoted object and signals the local queue to process it.
		/// </summary>
		private void RemoteRequestAvailable() 
		{
			ChannelRequests.AddRequest(Remoteable.Request);
			lock(QueueSignal) 
			{
				Monitor.Pulse(QueueSignal);
			}
		}

		~IMBroker() 
		{
			CleanUp();
		}

		/// <summary>
		/// Cleans up all the threads created by this object.
		/// </summary>
		public void CleanUp() 
		{
			if (QueueWatcherThread != null)
				QueueWatcherThread.Abort();
			if (WorkerThreads.Count > 0) 
			{
				for (int i = 0; i < WorkerThreads.Count; i++) 
				{
					((Thread)WorkerThreads[i]).Abort();
				}
			}
		}
	}
}
