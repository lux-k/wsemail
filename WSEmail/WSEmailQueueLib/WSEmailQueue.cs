using System;
using System.Windows.Forms;
using System.Xml;
using System.Configuration;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Threading;
using PennLibraries;
using WSEmailProxy;

namespace WSEmailQueueLib
{
	/// <summary>
	/// The parent library for a WS Email Queue. It is expected that there is already a remoting
	/// channel open which this library will use.
	/// </summary>
	public class WSEmailQueue
	{

		/// <summary>
		/// Holds a reference to the actual buffer object.
		/// </summary>
		private MessageBuffer theBuffer = new MessageBuffer();
		/// <summary>
		/// Holds a reference to the message buffer watcher thread
		/// </summary>
		private Thread messageProcessor = null;
		/// <summary>
		/// Holds the URL to the mail router (loaded automatically via configuration file).
		/// </summary>
		private string MailRouter = "";
		/// <summary>
		/// Holds a reference to an object that can be used for sending log messages.
		/// </summary>
		public static object LogTo = null;

		/// <summary>
		/// Default constructor. Initializes the buffer and processes messages as they come in.
		/// </summary>
		public WSEmailQueue()
		{
			StartUp();
		}

		/// <summary>
		/// Same as default, but specifies where to log messages to.
		/// </summary>
		public WSEmailQueue(object LogObject) 
		{
			WSEmailQueue.LogTo = LogObject;
			StartUp();
		}

		/// <summary>
		/// Private to the class; starts the message processor thread, remotes the queue and loads config info.
		/// </summary>
		private void StartUp() 
		{
			// load the config
			MailRouter = ConfigurationSettings.AppSettings["MailRouter"];
			LogEvent("MailRouter is set to: " + MailRouter + " (from configuration file).");
			Utilities.AddPennRoutingFilters(false);
			// remote the object.
			LogEvent("Message queue has been remoted.");
			RemotingServices.Marshal(theBuffer,"MessageQueue");

			// start the thread to process messages
			ThreadStart messageProcessorTS = new ThreadStart(ProcessMessages);
			messageProcessor = new Thread(messageProcessorTS);
			messageProcessor.Start();
		}

		/// <summary>
		/// Cleans up resources created by this object, such as threads. Also called automatically by destroying
		/// the object.
		/// </summary>
		public void CleanUp() 
		{
			if (messageProcessor != null) 
				messageProcessor.Abort();
		}

		~WSEmailQueue() 
		{
			CleanUp();
		}

		public static void LogEvent(string s) 
		{
			s = Utilities.GetCurrentTime() + ": " + s;
			if (LogTo is System.Windows.Forms.TextBox) 
			{
				System.Windows.Forms.TextBox t = (System.Windows.Forms.TextBox)LogTo;
				t.Text += s + "\r\n";
				t.SelectionStart=t.Text.Length-1;
				t.ScrollToCaret();
			}
			else
				Console.WriteLine(s);
		}

		private void ProcessMessages() 
		{
			LogEvent("Buffer processing thread has started...");
			while (true) 
			{
				//WSEmailAndSig m = null;
				// this will block (hopefully) on the message buffer's mutex
				WSEmailPackage m = theBuffer.getMessage();
				Console.WriteLine("ProcessMessages processing...");
				LogEvent("Received message to process.");
				ForwardMessage(m);
			}
		}
		public WSEmailStatus ForwardMessage(WSEmailPackage p)
		{
			LogEvent("Forwarding message through mailrouter: " + MailRouter);
			MailServerProxy m = new MailServerProxy(MailRouter);

//			XmlDocument x = new XmlDocument();
//			x.LoadXml(p.sig);
//			XmlElement xe = (XmlElement)x.ChildNodes[0];

			WSEmailStatus res = null;
			try 
			{
				res = m.WSEmailSend(p.theMessage,p.sig);
			}
			catch (Exception e) 
			{
				LogEvent(e.Message);
			}
			LogEvent("Forwarding message through mailrouter: " + MailRouter + "... complete!");
			return res;
		}
	}
}
