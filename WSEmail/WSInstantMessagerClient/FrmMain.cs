using System;
using System.Drawing;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using WSInstantMessagingLibraries;
using PennLibraries;
using System.Threading;
using WSEmailProxy;


namespace WSInstantMessagingClient
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	/// 
	
    
	public class FrmMain : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		//WSInstantMessagingServer _wsims = null;
		WSInstantMessagingManager _wsimm = null;
		MessageBuffer _mb = null;
		public WSInstantMessagingConfig InstantMessagingConfig = new WSInstantMessagingConfig();
		private Thread messageProcessor;

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtLog;
		private System.Windows.Forms.MenuItem mnuItmNewIM;
		private System.Windows.Forms.MenuItem mnuExit;
		private System.Windows.Forms.MainMenu menuItems;
		private System.Windows.Forms.MenuItem menuItem2;

		private System.ComponentModel.Container components = null;

		public FrmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			//InstantMessageConfig.
			_wsimm = new WSInstantMessagingManager();
			//_wsims = new WSInstantMessagingServer(_wsimm);
			_mb = new MessageBuffer();
			_wsimm.mainForm = this;
			// start remoting server.
			const string publishedName = "IM";
			Global.Proxy.SecurityTokenRetriever = new WSEmailProxy.TokenGrabber(ShowAuthenticationScreen);

			InstantMessagingConfig.loadConfig();		
			HttpChannel channel = new HttpChannel( InstantMessagingConfig.RemotingPort );
			ChannelServices.RegisterChannel( channel );
	
			//RemotingConfiguration.ApplicationName = publishedName;
			// RemotingServices.Marshal(_wsims,publishedName);
			RemotingServices.Marshal(_mb,publishedName);
			// tell the communications server about it.
			InstantMessagingConfig.RemotingUrl = "http://" + System.Net.Dns.GetHostName() + ":"+ InstantMessagingConfig.RemotingPort +"/"+publishedName;
			LogMessage("Instant Messaging Client starting up...");
			LogMessage("Configured to use Communications Server: " + InstantMessagingConfig.MailServerUrl);
			LogMessage("My user ID is: " + InstantMessagingConfig.UserID);

			Console.WriteLine("Bringing up message processor thread...");
			ThreadStart messageProcessorTS = new ThreadStart(ProcessInstantMessages);
			messageProcessor = new Thread(messageProcessorTS);
			messageProcessor.Start();

			_wsimm.updateCommServer(InstantMessagingConfig.RemotingUrl);
			_wsimm.InstantMessagingConfig = InstantMessagingConfig;
		}

		public Microsoft.Web.Services.Security.SecurityToken ShowAuthenticationScreen() 
		{
			AuthenticationForm f = new AuthenticationForm();
			f.ShowDialog();
			Microsoft.Web.Services.Security.SecurityToken s = f.SecurityToken;
			f.Dispose();
			return s;
		}

		public void LogMessage(string s) 
		{
			if (txtLog.Text.Length == 0) 
				txtLog.Text = s;
			else 
				txtLog.Text += "\r\n" + s;
		}
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}

				messageProcessor.Abort();
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
			this.txtLog = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.mnuItmNewIM = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.mnuExit = new System.Windows.Forms.MenuItem();
			this.menuItems = new System.Windows.Forms.MainMenu();
			this.SuspendLayout();
			// 
			// txtLog
			// 
			this.txtLog.Location = new System.Drawing.Point(8, 32);
			this.txtLog.Multiline = true;
			this.txtLog.Name = "txtLog";
			this.txtLog.Size = new System.Drawing.Size(432, 328);
			this.txtLog.TabIndex = 2;
			this.txtLog.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.TabIndex = 1;
			this.label1.Text = "Log:";
			// 
			// mnuItmNewIM
			// 
			this.mnuItmNewIM.Index = 0;
			this.mnuItmNewIM.Text = "Send IM...";
			this.mnuItmNewIM.Click += new System.EventHandler(this.newIM);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuItmNewIM,
																					  this.mnuExit});
			this.menuItem2.Text = "File";
			// 
			// mnuExit
			// 
			this.mnuExit.Index = 1;
			this.mnuExit.Text = "Exit....";
			this.mnuExit.Click += new System.EventHandler(this.Click_Exit);
			// 
			// menuItems
			// 
			this.menuItems.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem2});
			// 
			// FrmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(456, 373);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label1,
																		  this.txtLog});
			this.Menu = this.menuItems;
			this.Name = "FrmMain";
			this.Text = "WSInstantMessaging Client";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			//PennRoutingUtilities.AddPennRoutingFilters(false);
			Utilities.AddPennLoggingFilters();
			// PennRoutingFilters.PennRoutingUtilities.AddWSEMailSignatureFilters(true);
			Application.Run(new FrmMain());
		}

		private void Click_Exit(object sender, System.EventArgs e) 
		{
			Dispose();
		}
	

		private void newIM(object sender, System.EventArgs e)
		{

			string to = PennLibraries.InputBox.ShowInputBox("Who do you want to send an Instant Message to?");
			if (to.Length > 0)
				_wsimm.MakeNew(to);
		}

		private void ProcessInstantMessages() 
		{
			Console.WriteLine("Process instant messages thread started.");
			while (true) 
			{
				WSEmailMessage m = null;
				// this will block (hopefully) on the message buffer's mutex
				m = _mb.getMessage();
				Console.WriteLine(m);
				//MessageBox.Show(m.ToString());
				_wsimm.ProcessMessage(m);
			}
		}
	}
}
