/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Threading;
using System.Xml;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using WSInstantMessagingLibraries;
using WSEmailProxy;
using PennLibraries;
using WSSecureIMLib;
using WSEmailClientConfig;
using Microsoft.Web.Services2.Security.Tokens;
using Microsoft.Web.Services2.Security.X509;


namespace WSInstantMessagingLibraries
{
	/// <summary>
	/// An Instant Messaging form which can be used to send and view instant messages.
	/// </summary>
	public class IMForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// The scrolling box of messages that have been sent and received.
		/// </summary>
		private System.Windows.Forms.TextBox txtMessages;
		/// <summary>
		/// The box where a user types in a reply. The user can send it by pushing the send button or
		/// by simply pushing enter.
		/// </summary>
		private System.Windows.Forms.TextBox txtToSend;
		/// <summary>
		/// The button they can push to send the message.
		/// </summary>
		private System.Windows.Forms.Button btnSend;
		/// <summary>
		/// A handle to the configuration being used for all the instant messaging sessions.
		/// </summary>
		private WSInstantMessagingConfig InstantMessagingConfig = null;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// The intended recipient for the messages being sent.
		/// </summary>
		private string _recipient;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.Button button1;

		private ClientChatter securePeer = null;
		/// <summary>
		/// Holds the recipient for the messages send.
		/// </summary>
		public string Recipient 
		{
			set { _recipient = value; }
			get { return _recipient; }
		}

		public delegate WSEmailStatus sendMessageDelegate(WSEmailMessage m, XmlElement sig);
//		MailServerProxy msp;

		/// <summary>
		/// Default constructor that draws all the items on the form.
		/// </summary>
		public IMForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		/// <summary>
		/// Sets the handle to the configuration this window should use to communicate.
		/// </summary>
		/// <param name="w">The configuration object</param>
		public void setConfig(WSInstantMessagingConfig w) 
		{
			InstantMessagingConfig = w;
		}

		/// <summary>
		/// Creates a new IM form, draws the components and initializes the recipient to a certain person.
		/// </summary>
		/// <param name="caption">The address of the person who should receive the messages.</param>
		public IMForm(string caption) 
		{
			InitializeComponent();
			Recipient = caption;
			this.Text = "Message from: " + caption;
			txtToSend.Focus();
		}

		/// <summary>
		/// Posts a new message to the txtMessages buffer. It will be appended with a new line and the txtMessages object
		/// will be scrolled to the end.
		/// </summary>
		/// <param name="s">The string to post.</param>
		public void PostMessage (string s) 
		{
			if (!this.Disposing) 
			{
				lock (txtMessages) 
				{
					if (txtMessages.Text.Length != 0) 
						txtMessages.Text += "\r\n" + s;
					else
						txtMessages.Text = s;

					txtMessages.SelectionStart=txtMessages.Text.Length;
					txtMessages.ScrollToCaret();
					this.Focus();
					this.Show();
				}
			}
		}
		/// <summary>
		/// Posts a WSEmailMessage to the window. It basically just calls the other PostMessage with a more formatted string.
		/// </summary>
		/// <param name="m">Message object to post</param>
		public void PostMessage (WSEmailMessage m)
		{
			if ((m.MessageFlags & WSEmailFlags.InstantMessaging.DirectConnectInvitation) == WSEmailFlags.InstantMessaging.DirectConnectInvitation) 
			{
				PostMessage("NOTICE: Securely connecting to: " + m.Body);
				string[] tmp = m.Body.Split(':');
				InitChatter(tmp[0],int.Parse(tmp[1]));
				PostMessage("Connected.");
			} else				
			PostMessage("["+m.Sender+"] " + m.Body);
		}

		/// <summary>
		/// Cleans up any GUI stuff when this window is being thrown out.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose( bool disposing )
		{
			
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
			if (securePeer != null)
				securePeer.CleanUp();
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(IMForm));
			this.txtMessages = new System.Windows.Forms.TextBox();
			this.txtToSend = new System.Windows.Forms.TextBox();
			this.btnSend = new System.Windows.Forms.Button();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtMessages
			// 
			this.txtMessages.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtMessages.BackColor = System.Drawing.Color.White;
			this.txtMessages.Location = new System.Drawing.Point(8, 8);
			this.txtMessages.Multiline = true;
			this.txtMessages.Name = "txtMessages";
			this.txtMessages.ReadOnly = true;
			this.txtMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtMessages.Size = new System.Drawing.Size(272, 208);
			this.txtMessages.TabIndex = 0;
			this.txtMessages.Text = "";
			this.txtMessages.Enter += new System.EventHandler(this.focusTxtToSend);
			// 
			// txtToSend
			// 
			this.txtToSend.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtToSend.Location = new System.Drawing.Point(8, 224);
			this.txtToSend.Name = "txtToSend";
			this.txtToSend.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtToSend.Size = new System.Drawing.Size(272, 20);
			this.txtToSend.TabIndex = 1;
			this.txtToSend.Text = "";
			this.txtToSend.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtToSend_KeyPress);
			// 
			// btnSend
			// 
			this.btnSend.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnSend.Location = new System.Drawing.Point(200, 256);
			this.btnSend.Name = "btnSend";
			this.btnSend.Size = new System.Drawing.Size(80, 24);
			this.btnSend.TabIndex = 2;
			this.btnSend.Text = "Send...";
			this.btnSend.Click += new System.EventHandler(this.sendMessage);
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
																					  this.menuItem2});
			this.menuItem1.Text = "File";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.Text = "Request Direct Connection...";
			this.menuItem2.Click += new System.EventHandler(this.mnuDirectConnect);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(112, 256);
			this.button1.Name = "button1";
			this.button1.TabIndex = 3;
			this.button1.Text = "button1";
			this.button1.Visible = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// IMForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(288, 285);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.button1,
																		  this.btnSend,
																		  this.txtToSend,
																		  this.txtMessages});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.Name = "IMForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "IMForm";
			this.ResumeLayout(false);

		}
		#endregion

		private void SendingCallback(IAsyncResult r) 
		{
			//MessageBox.Show();
			ClientConfiguration.Proxy.EndWSEmailSend(r);
			//msp.EndWSEmailSend(r);
		}

		/// <summary>
		/// Used as an event on the message send button. If the button is clicked, this will
		/// formulate a WSEmailMessage, create a new proxy and send it to the recipient. It also
		/// displays the sent message in the GUI.
		/// </summary>
		/// <param name="sender">Internal GUI use</param>
		/// <param name="e">Internal GUI use</param>
		private void sendMessage(object sender, System.EventArgs e) 
		{
			if (securePeer != null) 
			{
				securePeer.SendMessage(txtToSend.Text);
			} 
			else 
			{
//				if (msp == null)
//					msp = new MailServerProxy(InstantMessagingConfig.MailServerUrl);
				WSEmailMessage m = new WSEmailMessage();

				m.Sender = ClientConfiguration.UserID;
				m.Recipients.AddRange(RecipientList.ParseRecipients(Recipient));
				m.Recipients.Remove(ClientConfiguration.UserID);
				// add the send as instant message flag, otherwise it would be 
				// be sent as an email
				m.MessageFlags |= WSEmailFlags.InstantMessaging.SendAsInstantMessage;
				m.Body = txtToSend.Text;
				// add the date.. then send.
				m.Timestamp = DateTime.Now;
				m.Subject = "";

				PostMessage("[" + ClientConfiguration.UserID + "] " + m.Body);
				AsyncCallback callback = new AsyncCallback(SendingCallback);
 
				sendMessageDelegate AsyncDelegate = new sendMessageDelegate(ClientConfiguration.Proxy.WSEmailSend);
				ClientConfiguration.Proxy.BeginWSEmailSend(m,null,callback,null);
//				msp.BeginWSEmailSend(m,null,callback,null);

//				MailServerProxy p = new MailServerProxy(InstantMessagingConfig.MailServerUrl);
//				p.WSEmailSend(m,null);
			}
			txtToSend.Text = "";
			txtToSend.Focus();
		}


 		delegate void doneSendingDelegate (System.IAsyncResult asyncResult);
		public void doneSending(System.IAsyncResult asyncResult) 
		{
			this.EndInvoke(asyncResult);
		}

		/// <summary>
		/// If a user presses enter, this method will be fired. It basically called the sendMessage method.
		/// </summary>
		/// <param name="sender">Internal GUI use</param>
		/// <param name="e">Internal GUI use</param>
		private void txtToSend_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			// if it's enter.. then post it.
			if (e.KeyChar == (char)13)
			{
				e.Handled = true;
				sendMessage(null,null);
			}

		}
		/// <summary>
		/// Sets the focus on where the user can enter a new message to send.
		/// </summary>
		/// <param name="a">Internal GUI use</param>
		/// <param name="whatever">Internal GUI use</param>
		private void focusTxtToSend(object a, System.EventArgs whatever) 
		{
			txtToSend.Focus();
		}

		private void InitChatter(string host, int port) 
		{
			if (securePeer != null)
				securePeer.CleanUp();
			securePeer = new ClientChatter();

			X509Certificate c = ClientConfiguration.Certificate;
			if (c == null) 
			{
				PostMessage("Notice: Unable to properly load client certificate!");
				PostMessage("\tMake sure that the certificate with CN = '"  + ClientConfiguration.CertCN + "'  is in your personal store.");
			} 
			else 
			{
				securePeer.myCert = c;
			}
			securePeer.MessageAvailable += new ClientChatter.ClientChatterEvent(PostMessage);
			securePeer.Connect(host,port);
			
		}

		private void mnuDirectConnect(object sender, System.EventArgs e)
		{
//			if (msp == null)
//				msp = new MailServerProxy(InstantMessagingConfig.MailServerUrl);
		
			//TODO fix me
			// ChannelRequest cr = Global.Proxy.RequestDirectConnect(InstantMessagingConfig.UserID,Recipient);
			
//			InitChatter(cr.Proxy,cr.DestinationPort);
			XmlDocument d = new XmlDocument();
			XmlElement root = d.CreateElement("RequestDirectConnection");
			ChannelRequest cr = new ChannelRequest(ClientConfiguration.UserID,Recipient);
			root.AppendChild(d.ImportNode(cr.Serialize(),true));
			ClientConfiguration.Proxy.ExecuteExtensionHandler("SecureInstantMessaging",root);

			PostMessage("NOTICE: Request sent; wait for server invitation.");

		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			Thread t = new Thread(new ThreadStart(StressTest));
			t.Start();
		}
		public void StressTest() 
		{
			Random r = new Random();
			while (true) 
			{
				mnuDirectConnect(null,null);
				Thread.Sleep(1000 + r.Next(2500));				
			}
		}
	}
}
