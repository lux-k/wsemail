/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;

namespace TestAppLibs
{
	public class ModelObjects : IDisposable
	{
		public event EventHandler TestFinished;
		public event EventHandler Logger;
		public System.Diagnostics.PerformanceCounter PerfLatency;

		ArrayList _res = new ArrayList();
		public Actions actions;
		public SecurityTokens securitytokens;
		public Messages messages;

		public int total = 0;

		public DateTime starttime;
		private int succeeded = 0;
		private int failed = 0;
		public string server;
		public DateTime lastheader = DateTime.MinValue;

		private int _tot=0, _cycles=0;
		public bool PerfRec = false;
		const string publishedName = "IM";

		public HttpChannel channel = null;
		WSInstantMessagingLibraries.MessageBuffer _mb = null;
		private Thread messageProcessor;
		public Hashtable mesnums = new Hashtable();
		Thread[] thethreads = null;

		public ModelObjects() 
		{
			RemoteBuffer();
			
			try 
			{
				PerfLatency = new PerformanceCounter(PerformanceConstants.ClientCategory,PerformanceConstants.ClientLatency,false);
				PerfRec = true;

			} 
			catch (Exception e) {
				Log(e.Message);
				Log(e.StackTrace);
			}
		}

		public void Dispose() {
			StopThreads();

			if (messageProcessor != null) 
			{
				try 
				{
					messageProcessor.Abort();
				} 
				catch {}
			}


			if (channel != null) 
			{
				try 
				{
					RemotingServices.Disconnect(_mb);
					ChannelServices.UnregisterChannel(channel);
				} 
				catch {}
			}

		}
			

		public void StopThreads() 
		{
			if (thethreads != null) 
			{
				foreach (Thread t in thethreads) 
				{
					if (t != null && t.ThreadState == System.Threading.ThreadState.Running) 
					{
						try 
						{
							t.Abort();
						} 
						catch {}
					}
				}
			}
			thethreads = null;
		}

		protected void RemoteBuffer() 
		{
			_mb = new WSInstantMessagingLibraries.MessageBuffer();
			channel = new HttpChannel( 6789 );
			ChannelServices.RegisterChannel( channel );
			RemotingServices.Marshal(_mb,publishedName);

			ThreadStart messageProcessorTS = new ThreadStart(ProcessInstantMessages);
			messageProcessor = new Thread(messageProcessorTS);
			messageProcessor.Start();
		}


		public int threads 
		{
			get 
			{
				return _tot;
			}
			set 
			{
				_tot = value;
				total = _tot * cycles;
			}
		}
		public int cycles 
		{
			get 
			{
				return _cycles;
			}
			set 
			{
				_cycles = value;
				total = _tot * cycles;
			}
		}

		public void AddResult(TestResult t) 
		{
			_res.Add(t);
		}

		public TestResult[] Results 
		{
			get 
			{
				return (TestResult[])_res.ToArray(typeof(TestResult));
			}
		}

		public void AddSuccess() 
		{
			lock(this) 
			{
				succeeded += 1;
			}
			CheckDone();
		}

		public void AddFailure() 
		{
			lock(this) 
			{
				failed += 1;
			}
			CheckDone();
		}

		private void CheckDone() 
		{
			if (failed + succeeded == total) 
			{
				Log("Test run done.");
				float sec = (float)((DateTime.Now.ToFileTime() - starttime.ToFileTime()) / 10000000);
				Log(((float)total/sec).ToString() + " attempts/sec");
				if (TestFinished != null)
					TestFinished(this,EventArgs.Empty);
			}
		}
		public void Run() 
		{
			Log("Run initializing...");
			
			bool again = true;
			int count = 0;
			while (again) 
			{
				try 
				{
					WSEmailProxy.ExtensionArgument args = new WSEmailProxy.ExtensionArgument("UpdateIMLocation");
					string remUrl = "http://" + System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString() + ":6789/"+publishedName;
					args.AddArgument("Location",remUrl);
					WSEmailProxy.MailServerProxy p = new WSEmailProxy.MailServerProxy(server);
					p.SecurityToken = securitytokens.Get();
					p.ExecuteExtensionHandler("InstantMessaging",args.AsXmlElement());
					again = false;
				} 
				catch
				{
					count++;
					if (count == 5)
						again = false;
				}
			}


			try 
			{
				StopThreads();
				mesnums.Clear();
				succeeded = 0;
				failed = 0;
				_res.Clear();
				thethreads = new Thread[this.threads];
				for (int i = 0; i < this.threads; i++) 
				{
					thethreads[i] = new Thread(new ThreadStart( (new TheThread(this,i)).ThreadCode ) );
				}
				starttime = DateTime.Now;
				Log("Run starting...");
				for (int i = 0; i < this.threads; i++) 
				{
					thethreads[i].Start();
				}
				Log("Run started.");
			} 
			catch (Exception e) 
			{
				Log(e.Message);
				Log(e.StackTrace);
			}
		}

		private void ProcessInstantMessages() 
		{
			Console.WriteLine("Process instant messages thread started.");
			while (true) 
			{
				try 
				{
					WSEmailProxy.WSEmailMessage m = null;
					Console.WriteLine("Process instant messages thread running.");
					// this will block (hopefully) on the message buffer's mutex
					m = _mb.getMessage();
					Console.WriteLine("Process instant messages received message.");
					DateTime dt = new DateTime(long.Parse(m.Subject));
					TimeSpan time = DateTime.Now.Subtract(dt);
					TestResult t = new TestResult();
					t.Action = ACTIONS.SendIM;;
					bool res = true;
					t.Successful = res;
					t.Token = "Nayan = cheeseboy";
					t.Latency = time.TotalMilliseconds/2.0;

					if (PerfRec)
						PerfLatency.RawValue = (long)(time.TotalMilliseconds/1000.0000);

					AddResult(t);
					
					if (res) 
					{
						AddSuccess();
					}
					else 
					{
						AddFailure();
					}

				} 
				catch (Exception e) 
				{
					try 
					{
						Console.WriteLine("Oops in ProcessInstantMessages: " + e.StackTrace);
					} 
					catch {}
				}
			}
		}

		public void Log(string s) 
		{
			Debug.WriteLine(s);
			if (Logger != null) 
				Logger(s,EventArgs.Empty);
		}
		// used so that this object (when it's remoted) doesn't time out.

	}
}
