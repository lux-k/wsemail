/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using WSEmailProxy;
using Microsoft.Web.Services2.Security.Tokens;

namespace LoadGenerator
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
		private System.Windows.Forms.Button runButton;
		private bool running = false;
		private Thread sendThread = null;
		private System.Windows.Forms.Label label1;
		private bool abort = false;
		private string[] recips = new string[] {"sandy@spongebob","patrick@spongebob","gary@spongebob","mrkrabs@spongebob" };

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.runButton.Text = "Go away";
			this.runButton.Enabled = false;
			sendThread = new Thread(new ThreadStart(threadCode));
			sendThread.Start();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public void threadCode() 
		{
			MailServerProxy p = new MailServerProxy();
			p.SecurityToken = new UsernameToken("send","send",PasswordOption.SendNone);
			WSEmailMessage m = new WSEmailMessage();
			m.Subject = "stupid, stupid mail";

			for (int i = 0; i < 64; i++)
				m.Body += "XXXXXXXXXXXXXXXX";
			Random r = new Random();
			
			this.runButton.Text = "Go...";
			this.runButton.Enabled = true;
			sendThread.Suspend();
			
			while (true) 
			{
				if (abort) 
					sendThread.Abort();

				label1.Text = DateTime.Now.ToLongTimeString() + ": last message sent";
				m.Timestamp = DateTime.Now;
				m.Recipients.AddRange( recips );
				p.WSEmailSend(m,null);
				m.Recipients.Clear();
				Thread.Sleep(new TimeSpan(0,0,1));
			}
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
			this.runButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// runButton
			// 
			this.runButton.Location = new System.Drawing.Point(80, 112);
			this.runButton.Name = "runButton";
			this.runButton.TabIndex = 0;
			this.runButton.Text = "Go...";
			this.runButton.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 200);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(240, 48);
			this.label1.TabIndex = 1;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(232, 245);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.runButton);
			this.Name = "Form1";
			this.Text = "WSEmail Load Generator";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
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

		private void button1_Click(object sender, System.EventArgs e)
		{
			if (!running) 
			{
				running = true;
				runButton.Text = "Stop...";
				sendThread.Resume();
			} 
			else 
			{
				running = false;
				runButton.Text = "Go...";
				sendThread.Suspend();
			}
		}

		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			abort = true;
			if (sendThread.ThreadState != ThreadState.Running)
				sendThread.Resume();
		}
	}
}
