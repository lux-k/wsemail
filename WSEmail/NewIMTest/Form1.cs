/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using WSEmailProxy;
using System.Xml;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.Web.Services2.Messaging;
using Microsoft.Web.Services2.Addressing;
using Microsoft.Web.Services2;

namespace NewIMTest
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
		private System.Windows.Forms.Button button1;
		EndpointReference EPR = new EndpointReference(new Uri( 
			"soap.tcp://" + System.Net.Dns.GetHostName() + "/MyReceiver" ));
		MyReceiver m = new MyReceiver();

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			SoapReceivers.Add(EPR, m);

			
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			SoapReceivers.Remove(EPR);
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
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(104, 96);
			this.button1.Name = "button1";
			this.button1.TabIndex = 0;
			this.button1.Text = "button1";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.button1);
			this.Name = "Form1";
			this.Text = "Form1";
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

		private void button1_Click(object dfs, System.EventArgs e)
		{
			SoapSender sender = new SoapSender(EPR);
			SoapEnvelope s = new SoapEnvelope();
			WSEmailMessage m = new WSEmailMessage();
			m.Body = "HELLO!!";
			m.Recipients.Add("moo@cow");
			m.Subject = "The cow.";
			m.Timestamp = DateTime.Now;
			m.Sender = "moo@cow";
			s.SetBodyObject(m);
			s.Context.Addressing.Action = new Action("soap.tcp://SomeNamespaceURI/sendIM");
			sender.Send(s);
		}
	}

	class MyReceiver : SoapReceiver
	{
		public delegate void NewMessageReceivedDelegate(SoapEnvelope e);
		public event NewMessageReceivedDelegate NewMessageReceived;

		protected override void Receive( SoapEnvelope message )
		{
			MessageBox.Show(message.OuterXml);
			MessageBox.Show(message.Context.Addressing.Action.Value);
			//Processing of message goes here
		}		
	}

}
