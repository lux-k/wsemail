using WSSecureIMLib;
using WSSMTPGatewayLib;
using WSEmailQueueLib;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace WSCommunicationsServer
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabSecureConnections;
		private System.Windows.Forms.TextBox txtSecureIM;

		// secure IM proxy...
//		private ChannelRequestor cr = null;
//		private RemoteableChannelRequest RReqChan = null;
		private IMBroker broker = null;
//		private WSEmailQueue emailqueue = null;
		private SMTPGateway smtp = null;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TabPage tabSMTP;
		private System.Windows.Forms.TextBox txtSMTPLog;
		private HttpChannel myChannel = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public void Launcher() 
		{
			myChannel = new HttpChannel(2525);
			ChannelServices.RegisterChannel(myChannel);
			//secure IM
			broker = new IMBroker();//this.txtSecureIM);
			// emailqueue = new WSEmailQueue(this.txtEmailQueue);
			smtp = new SMTPGateway(this.txtSMTPLog);
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
			ChannelServices.UnregisterChannel(myChannel);
			base.Dispose( disposing );
			broker.Dispose();
			//emailqueue.CleanUp();
			smtp.CleanUp();

		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabSecureConnections = new System.Windows.Forms.TabPage();
			this.txtSecureIM = new System.Windows.Forms.TextBox();
			this.tabSMTP = new System.Windows.Forms.TabPage();
			this.txtSMTPLog = new System.Windows.Forms.TextBox();
			this.tabControl1.SuspendLayout();
			this.tabSecureConnections.SuspendLayout();
			this.tabSMTP.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.tabControl1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.tabSecureConnections,
																					  this.tabSMTP});
			this.tabControl1.Location = new System.Drawing.Point(8, 8);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(448, 368);
			this.tabControl1.TabIndex = 0;
			// 
			// tabSecureConnections
			// 
			this.tabSecureConnections.Controls.AddRange(new System.Windows.Forms.Control[] {
																							   this.txtSecureIM});
			this.tabSecureConnections.Location = new System.Drawing.Point(4, 22);
			this.tabSecureConnections.Name = "tabSecureConnections";
			this.tabSecureConnections.Size = new System.Drawing.Size(440, 342);
			this.tabSecureConnections.TabIndex = 1;
			this.tabSecureConnections.Text = "Secure IMs";
			// 
			// txtSecureIM
			// 
			this.txtSecureIM.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtSecureIM.Location = new System.Drawing.Point(8, 8);
			this.txtSecureIM.Multiline = true;
			this.txtSecureIM.Name = "txtSecureIM";
			this.txtSecureIM.Size = new System.Drawing.Size(424, 328);
			this.txtSecureIM.TabIndex = 1;
			this.txtSecureIM.Text = "";
			// 
			// tabSMTP
			// 
			this.tabSMTP.Controls.AddRange(new System.Windows.Forms.Control[] {
																				  this.txtSMTPLog});
			this.tabSMTP.Location = new System.Drawing.Point(4, 22);
			this.tabSMTP.Name = "tabSMTP";
			this.tabSMTP.Size = new System.Drawing.Size(440, 342);
			this.tabSMTP.TabIndex = 2;
			this.tabSMTP.Text = "SMTP Gateway";
			// 
			// txtSMTPLog
			// 
			this.txtSMTPLog.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtSMTPLog.Location = new System.Drawing.Point(8, 8);
			this.txtSMTPLog.Multiline = true;
			this.txtSMTPLog.Name = "txtSMTPLog";
			this.txtSMTPLog.Size = new System.Drawing.Size(424, 328);
			this.txtSMTPLog.TabIndex = 0;
			this.txtSMTPLog.Text = "";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(464, 381);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl1});
			this.Name = "Form1";
			this.Text = "WS Communications Log";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabSecureConnections.ResumeLayout(false);
			this.tabSMTP.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Form1 f = new Form1();
			f.Launcher();
			Application.Run(f);
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
		
		}
	}
}



