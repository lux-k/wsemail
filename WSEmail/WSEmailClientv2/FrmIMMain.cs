/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

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
using WSEmailClientConfig;


namespace WSEmailClientv2
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	/// 
	
    
	public class FrmIMMain : System.Windows.Forms.Form
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
		private HttpChannel channel = null;
		private bool registered = false;

		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.MenuItem menuItem1;
		const string publishedName = "IM";

		protected void RemoteBuffer() 
		{
			channel = new HttpChannel( InstantMessagingConfig.RemotingPort );
			ChannelServices.RegisterChannel( channel );
			RemotingServices.Marshal(_mb,publishedName);
		}

		public FrmIMMain()
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

			RemoteBuffer();
			// tell the communications server about it.
			InstantMessagingConfig.RemotingUrl = "http://" + System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString() + ":"+ InstantMessagingConfig.RemotingPort +"/"+publishedName;
			LogMessage("Instant Messaging Client starting up...");
			LogMessage("Configured to use Communications Server: " + ClientConfiguration.MailServerUrl);
			
			Console.WriteLine("Bringing up message processor thread...");
			ThreadStart messageProcessorTS = new ThreadStart(ProcessInstantMessages);
			messageProcessor = new Thread(messageProcessorTS);
			messageProcessor.Start();

			_wsimm.InstantMessagingConfig = InstantMessagingConfig;
			Register();
			LogMessage("My user ID is: " + ClientConfiguration.UserID);
		}

		private void Register() 
		{
			try 
			{
				_wsimm.updateCommServer(InstantMessagingConfig.RemotingUrl);
				registered = true;
				string r = "Registered current location with IM server.";
				LogMessage(r);
				Utilities.LogToStatusWindow(r);
			} 
			catch (Exception e) 
			{
				new ExceptionForm(e,"Unable to update server of our present location.");
			}

			if (!registered)
				LogMessage("Warning: You are NOT registered on the server. You must reregister by using the option in the file menu.");
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

				RemotingServices.Disconnect(_mb);

				if (channel != null) 
					ChannelServices.UnregisterChannel( channel );


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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FrmIMMain));
			this.txtLog = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.mnuItmNewIM = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.mnuExit = new System.Windows.Forms.MenuItem();
			this.menuItems = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// txtLog
			// 
			this.txtLog.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
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
			this.mnuItmNewIM.Text = "Send IM";
			this.mnuItmNewIM.Click += new System.EventHandler(this.newIM);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuItmNewIM,
																					  this.menuItem1,
																					  this.mnuExit});
			this.menuItem2.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
			this.menuItem2.Text = "File";
			// 
			// mnuExit
			// 
			this.mnuExit.Index = 2;
			this.mnuExit.Text = "Close IM Client";
			this.mnuExit.Click += new System.EventHandler(this.Click_Exit);
			// 
			// menuItems
			// 
			this.menuItems.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem2});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 1;
			this.menuItem1.Text = "Reregister";
			this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
			// 
			// FrmIMMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(448, 369);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label1,
																		  this.txtLog});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.menuItems;
			this.Name = "FrmIMMain";
			this.Text = "Instant Messaging Client";
			this.ResumeLayout(false);

		}
		#endregion


		private void Click_Exit(object sender, System.EventArgs e) 
		{
			Dispose();
		}
	

		private void newIM(object sender, System.EventArgs e)
		{

			string to = PennLibraries.InputBox.ShowInputBox("Who do you want to send an Instant Message to?");

			ArrayList a = new ArrayList();
			a.Add(ClientConfiguration.UserID);
			a.AddRange(RecipientList.ParseRecipients(to));
			a.Sort();

			if (to.Length > 0)
				_wsimm.MakeNew(RecipientList.FormatRecipients((string[])a.ToArray(typeof(string))).ToLower());
		}

		private void ProcessInstantMessages() 
		{
			Console.WriteLine("Process instant messages thread started.");
			while (true) 
			{
				try 
				{
					WSEmailMessage m = null;
					// this will block (hopefully) on the message buffer's mutex
					m = _mb.getMessage();
					Console.WriteLine(m);
					//MessageBox.Show(m.ToString());
					_wsimm.ProcessMessage(m);
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

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			Register();
		}
	}
}
