/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Diagnostics;
using System.Reflection;
using System.Xml;
using System.Configuration;
using System.Drawing;
using System.Collections;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Data;
using TestAppLibs;

namespace TestApp 
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.DataGrid dgMessages;
		public MailMessage[] Messages;
		private Controller _co = null;
		private ClientEventsProxy _cp = null;
		private System.Windows.Forms.TextBox txtSetup;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnLink;
		private TcpChannel chan = null;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem mnuPerf;
		private System.Windows.Forms.MenuItem mnuExit;
		private ModelObjects mo = new ModelObjects();
		//private System.Diagnostics.PerformanceCounter coun;
		private EventHandler evt = null;
		private UDPMulticastListener umcl = null;
		private Thread multicastThread = null;
		private string machineName = "";
		private DateTime testStartTime = DateTime.MinValue;
		
		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			IntializeRemotingStuff();
			
			mo.Logger += new EventHandler(Log);
			mo.TestFinished += new EventHandler(TestFinished);
			multicastThread = new Thread(new ThreadStart(MulticastListen));
			multicastThread.Start();
		
			machineName = System.Net.Dns.GetHostName();
			//
			// TODO: Add any constructor code after InitializeComponent call
			//

		}

		private void MulticastListen() 
		{
			Debug.WriteLine("Multicast listener thread started.");
			umcl = new UDPMulticastListener();
			umcl.MessageReceived += new MessageReceivedHandler(UdpLink);
			umcl.StartListener();
			
		}

		private void UdpLink(string s) 
		{
			Log("Received broadcast link request to connect to " + s);
			Link(s);
		}

		private void IntializeRemotingStuff() 
		{	
			BinaryServerFormatterSinkProvider clientProv = new BinaryServerFormatterSinkProvider();
			clientProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

			IDictionary props = new Hashtable();
			props["port"] = 0;

			chan = new TcpChannel(props, null, clientProv);        
			ChannelServices.RegisterChannel( chan );
		}

		private void UnregisterClient() 
		{
			try 
			{
				if (_co != null) 
				{
					if (this.evt != null)
						_co.BeginTest -= evt;

					_co.UnregisterSlave(machineName);
				}

			} 
			catch (Exception e)
			{
				Log(e.Message);
				Log(e.StackTrace);
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{

			if (umcl != null) 
			{
				try 
				{
					umcl.StopListener();
				} catch {}
			}

			if (multicastThread != null) 
			{
				try 
				{
					multicastThread.Abort();
				} 
				catch {}
			}


			UnregisterClient();

			if (chan != null) 
			{
				ChannelServices.UnregisterChannel(chan);
				chan = null;
			}

			if (mo != null) 
			{
				mo.Dispose();
			}

			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.dgMessages = new System.Windows.Forms.DataGrid();
			this.btnLink = new System.Windows.Forms.Button();
			this.txtSetup = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mnuPerf = new System.Windows.Forms.MenuItem();
			this.mnuExit = new System.Windows.Forms.MenuItem();
			((System.ComponentModel.ISupportInitialize)(this.dgMessages)).BeginInit();
			this.SuspendLayout();
			// 
			// dgMessages
			// 
			this.dgMessages.DataMember = "";
			this.dgMessages.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgMessages.Location = new System.Drawing.Point(8, 376);
			this.dgMessages.Name = "dgMessages";
			this.dgMessages.Size = new System.Drawing.Size(632, 208);
			this.dgMessages.TabIndex = 24;
			this.dgMessages.Visible = false;
			// 
			// btnLink
			// 
			this.btnLink.Location = new System.Drawing.Point(16, 40);
			this.btnLink.Name = "btnLink";
			this.btnLink.TabIndex = 25;
			this.btnLink.Text = "Link";
			this.btnLink.Click += new System.EventHandler(this.btnLink_Click);
			// 
			// txtSetup
			// 
			this.txtSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSetup.Location = new System.Drawing.Point(128, 40);
			this.txtSetup.Multiline = true;
			this.txtSetup.Name = "txtSetup";
			this.txtSetup.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtSetup.Size = new System.Drawing.Size(392, 281);
			this.txtSetup.TabIndex = 27;
			this.txtSetup.Text = "";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(128, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(240, 23);
			this.label1.TabIndex = 28;
			this.label1.Text = "Status Information:";
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuPerf,
																					  this.mnuExit});
			this.menuItem1.Text = "File";
			// 
			// mnuPerf
			// 
			this.mnuPerf.Index = 0;
			this.mnuPerf.Text = "Register Performance Counters";
			this.mnuPerf.Click += new System.EventHandler(this.mnuPerf_Click);
			// 
			// mnuExit
			// 
			this.mnuExit.Index = 1;
			this.mnuExit.Text = "Exit";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(536, 334);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtSetup);
			this.Controls.Add(this.btnLink);
			this.Controls.Add(this.dgMessages);
			this.Menu = this.mainMenu1;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "TestBed Client v1.0";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgMessages)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}


		private void Link(string s) 
		{
			mo.StopThreads();
			Log("Registering with server...");
			if (_co != null) 
			{
				UnregisterClient();
			}

			_co = (Controller)Activator.GetObject(typeof(Controller),s);
			_co.RegisterSlave(machineName);
			Log("Registered with server.");

			_cp = new ClientEventsProxy();
			_cp.StartTest += new EventHandler(this.StartTest);

			evt = new EventHandler(_cp.LocallyHandleMessage);
			_co.BeginTest += evt;
			GetConfig();
		}

		private void btnLink_Click(object sender, System.EventArgs e)
		{
			Link(ConfigurationSettings.AppSettings["Server"]);
		}

		public void Log(string s) 
		{
			this.Invoke(new BUH(LogLocal),new object[] {s});
		}

		private delegate void BUH (string s);

		private void LogLocal(string s) 
		{
			txtSetup.Text += DateTime.Now.ToLongTimeString() + ": " + s + "\r\n";
			txtSetup.SelectionStart = txtSetup.TextLength;
			txtSetup.ScrollToCaret();
		}

		public void Log(object o, EventArgs e) 
		{
			Log((string)o);
		}

		public void StartTest(object o, EventArgs e) 
		{
			if (testStartTime == DateTime.MinValue) 
			{
				testStartTime = (DateTime)o;

			}
			else 
			{
				if (testStartTime == (DateTime)o) 
				{
					Log("Rejected extra test start request.");
					return;
				}
				testStartTime = (DateTime)o;
				Log("Set test start time");
			}

			Log("Received test start signal from server.");
			try 
			{
				mo.Run();
			} 
			catch (Exception ex) 
			{
				Log(ex.Message);
				Log(ex.StackTrace);
			}
			
		}

		private void TestFinished(object o, EventArgs e) 
		{
			Log("Sending results to server...");
			_co.PushResults(mo.Results);
			Log("Done sending results.");

		}

		private void GetConfig()
		{
			Log("Beginning download of configuration...");
			GetXmlConfigs();
			GetThreadCount();
			GetCycles();
			GetServer();
			Log("Configuration download done.");
		}

		private void GetXmlConfigs() 
		{
			Log("Configuration:\r\n");
			string sum = "Summary: ";
			foreach (string sect in Enum.GetNames(typeof(SECTIONTYPES))) 
			{
				string s = _co.RetrieveConfigurationSection((SECTIONTYPES)Enum.Parse(typeof(SECTIONTYPES),sect));
				if (s != null) 
				{
					XmlDocument d = new XmlDocument();
					Log(s);
					d.LoadXml(s);
					
					Assembly a = Assembly.GetAssembly(typeof(ProbabilisticObject));
					try 
					{
						IConfigurationSectionHandler cfg = (IConfigurationSectionHandler)a.CreateInstance(a.GetName().Name+"."+sect + "Reader");
						ProbabilisticObject o = (ProbabilisticObject)cfg.Create(null,machineName,d.FirstChild);
						sum += "\r\n" + sect + ": " + o.Count.ToString() + " object(s)";
						FieldInfo f = mo.GetType().GetField(sect.ToLower(),BindingFlags.Public | BindingFlags.Instance);
						f.SetValue(mo,o);
					} 
					catch
					{
						Log("Error loading configuration for " + sect);
					}

				}
			}
			Log(sum);
			
		}

		private void GetThreadCount() 
		{
			mo.threads = _co.Threads;
			Log("Server requests " + mo.threads.ToString() + " threads.");
		}

		private void GetCycles() 
		{
			mo.cycles = _co.Cycles;
			Log("Server requests " + mo.cycles.ToString() + " cycles.");
		}

		private void GetServer() 
		{
			mo.server = _co.Server;
			Log("Server requests target to be " + mo.server);
		}

		private void mnuPerf_Click(object sender, System.EventArgs e)
		{

			if (!System.Diagnostics.PerformanceCounterCategory.Exists(PerformanceConstants.ClientCategory))
			{
				System.Diagnostics.CounterCreationDataCollection CounterDatas = 
					new System.Diagnostics.CounterCreationDataCollection();
				// Create the counters and set their properties.
				System.Diagnostics.CounterCreationData cdCounter1 = 
					new System.Diagnostics.CounterCreationData();
				cdCounter1.CounterName = PerformanceConstants.ClientLatency;
				cdCounter1.CounterHelp = PerformanceConstants.ClientLatencyDesc;
				cdCounter1.CounterType = System.Diagnostics.PerformanceCounterType.NumberOfItems32;
				// Add both counters to the collection.
				CounterDatas.Add(cdCounter1);
				// Create the category and pass the collection to it.
				System.Diagnostics.PerformanceCounterCategory.Create(
					PerformanceConstants.ClientCategory, PerformanceConstants.ClientCategory + " Statistics", CounterDatas);
			}
		}

		private void mnuExit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			Log("Listening on UDP multicast: " + umcl.ToString());		
		}
	}
}
