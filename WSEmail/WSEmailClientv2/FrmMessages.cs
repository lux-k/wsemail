/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using WSEmailProxy;
using System.Net.Sockets;
using System.Net;
using System.Configuration;
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using PennLibraries;
using System.Threading;

namespace WSEmailClientv2
{
	/// <summary>
	/// Summary description for frmMessage.
	/// </summary>
	public class FrmMessages : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnCheck;
		private ScrollableDataGrid dgHeaders;
		private System.Windows.Forms.Timer timerCheck;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.CheckBox chkServerNotification;
		private Form newmail = null;
		private int UDPPort = -1;
		private UdpClient udpclient = null;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem mnuCheck;
		private System.Windows.Forms.MenuItem mnuExit;
		private System.Windows.Forms.Label lblInfo;
		Thread UDPListenerThread = null;
		private DateTime lastchecked = DateTime.MinValue;
		//private bool disabled = false;

		public FrmMessages()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.timerCheck.Interval = Global.CheckDelay * 1000;
			this.btnCheck_Click(null,null);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}

			if (newmail != null && newmail.Disposing != true)
				newmail.Dispose();

			ShredUDPListener();
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FrmMessages));
			this.btnCheck = new System.Windows.Forms.Button();
			this.dgHeaders = new WSEmailClientv2.ScrollableDataGrid();
			this.timerCheck = new System.Windows.Forms.Timer(this.components);
			this.chkServerNotification = new System.Windows.Forms.CheckBox();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mnuCheck = new System.Windows.Forms.MenuItem();
			this.mnuExit = new System.Windows.Forms.MenuItem();
			this.lblInfo = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dgHeaders)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCheck
			// 
			this.btnCheck.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnCheck.Location = new System.Drawing.Point(592, 412);
			this.btnCheck.Name = "btnCheck";
			this.btnCheck.Size = new System.Drawing.Size(96, 32);
			this.btnCheck.TabIndex = 1;
			this.btnCheck.Text = "Check Again...";
			this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
			// 
			// dgHeaders
			// 
			this.dgHeaders.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.dgHeaders.CaptionText = "Inbox";
			this.dgHeaders.CaptionVisible = false;
			this.dgHeaders.DataMember = "";
			this.dgHeaders.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgHeaders.Name = "dgHeaders";
			this.dgHeaders.ReadOnly = true;
			this.dgHeaders.Size = new System.Drawing.Size(696, 404);
			this.dgHeaders.TabIndex = 3;
			this.dgHeaders.Tag = "";
			this.dgHeaders.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgHeaders_KeyDown);
			this.dgHeaders.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgHeaders_KeyPress);
			this.dgHeaders.Navigate += new System.Windows.Forms.NavigateEventHandler(this.dgHeaders_Navigate);
			this.dgHeaders.DoubleClick += new System.EventHandler(this.lstHeaders_DblClick);
			this.dgHeaders.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgHeaders_MouseUp);
			// 
			// timerCheck
			// 
			this.timerCheck.Enabled = true;
			this.timerCheck.Interval = 10000;
			this.timerCheck.Tick += new System.EventHandler(this.timerCheck_Tick);
			// 
			// chkServerNotification
			// 
			this.chkServerNotification.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.chkServerNotification.Location = new System.Drawing.Point(8, 412);
			this.chkServerNotification.Name = "chkServerNotification";
			this.chkServerNotification.Size = new System.Drawing.Size(192, 32);
			this.chkServerNotification.TabIndex = 4;
			this.chkServerNotification.Text = "Use Server Notification Extension";
			this.chkServerNotification.CheckedChanged += new System.EventHandler(this.chkServerNotification_CheckedChanged);
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
																					  this.mnuCheck,
																					  this.mnuExit});
			this.menuItem1.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
			this.menuItem1.Text = "File";
			// 
			// mnuCheck
			// 
			this.mnuCheck.Index = 0;
			this.mnuCheck.Text = "Check Again";
			this.mnuCheck.Click += new System.EventHandler(this.mnuCheck_Click);
			// 
			// mnuExit
			// 
			this.mnuExit.Index = 1;
			this.mnuExit.Text = "Close Inbox";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// lblInfo
			// 
			this.lblInfo.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.lblInfo.Location = new System.Drawing.Point(392, 408);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(176, 40);
			this.lblInfo.TabIndex = 5;
			this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FrmMessages
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(696, 449);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.lblInfo,
																		  this.chkServerNotification,
																		  this.dgHeaders,
																		  this.btnCheck});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.Name = "FrmMessages";
			this.Text = "Inbox";
			this.Load += new System.EventHandler(this.frmMessages_Load);
			this.Closed += new System.EventHandler(this.FrmMessages_Closed);
			((System.ComponentModel.ISupportInitialize)(this.dgHeaders)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void btnCheck_Click(object sender, System.EventArgs e)
		{
//			frmWait frm = new frmWait("Retrieving message headers from Mail Server " + ConfigurationSettings.AppSettings["MailServer"] + ".");
			Utilities.LogToStatusWindow("Beginning to download WSEmail headers.");
			
//			Utilities.LogTransit(Global.Proxy.Url,"To");
			WSEmailHeader[] wsh = null;
			bool err = false;

			try
			{
				DateTime t = DateTime.Now;
				wsh = Global.Proxy.WSEmailFetchHeaders(this.lastchecked);
				this.lastchecked = t;
			} 
			catch (Exception ex) 
			{
				new ExceptionForm(ex,"Unable to retrieve messages from remote server. Perhaps it isn't online?");
				//err = (disabled == true);
				err = true;
				lblInfo.Text = "Checking for new mail suspended; close and reopen inbox to reenable polling.";
				this.timerCheck.Enabled = false;
				
			}

			if (!err) 
			{
				try 
				{
					if (wsh == null && ((WSEmailHeader[])dgHeaders.DataSource) == null) 
					{
						MessageBox.Show("You have no messages.","Oops!");
						//				textBox1.Text = "You don't have any mail.\r\n\r\nYou can click the button to check again.";
					}
		 
					else 
					{
						int count = 0;
						int spot = dgHeaders.CurrentRowIndex;
						if (dgHeaders.DataSource != null)
							count = ((WSEmailHeader[])dgHeaders.DataSource).Length;

						if (wsh.Length > 0) 
						{
							//dgHeaders.TableStyles.Clear();
							if (dgHeaders.TableStyles.Count == 0)
								dgHeaders.TableStyles.Add(GetDataGridTableStyle(wsh));
							ArrayList heads = new ArrayList();
							if (dgHeaders.DataSource != null)
								heads.AddRange((WSEmailHeader[])dgHeaders.DataSource);
							heads.AddRange(wsh);
							dgHeaders.SetDataBinding(heads.ToArray(typeof(WSEmailHeader)), null);


							//						if (spot >= 0 && spot < wsh.Length-1) 
							//						{
							try 
							{
								if (dgHeaders.DataSource != null && (WSEmailHeader[])dgHeaders.DataSource != null) 
								{
									int i = ((WSEmailHeader[])dgHeaders.DataSource).Length-1;
									if (i > 0) 
									{
										dgHeaders.CurrentRowIndex = i;
										dgHeaders.Select(i);
									}
									dgHeaders.ScrollToBottom();
								}
							} 
							catch
							{
								Console.WriteLine("Found 0 error...");
							}
							//						}

						}

						if (wsh.Length > count && count != 0)  
						{
/*
							if (newmail == null || newmail.Disposing || newmail.IsDisposed)
								newmail = new NewMail(this);
				
							newmail.Show();
*/
							Global.AlertIcon.Visible = true;
							Global.AlertIcon.ShowBalloon("New WSEmail has arrived!","From: " + wsh[wsh.Length-1].Sender + "\r\nSubject: "+wsh[wsh.Length-1].Subject,NotifyIconEx.NotifyInfoFlags.Info,20000);
						}

		
						//MessageBox.Show("New mail has arrived.");
						//				textBox1.Text = "You have mail!\r\n\r\nDouble click on any message to view its contents.\r\nPress the delete key on any selected message to delete it.";
					}
				} 
				catch (Exception e2) 
				{
					Console.WriteLine(e2.Message + e2.StackTrace);
				}
			}
			Utilities.LogToStatusWindow("Done downloading WSEmail headers.");
//			Utilities.LogTransit(Global.Proxy.Url,"Erase");
//			frm.Dispose();
		}

		private DataGridTableStyle GetDataGridTableStyle(object o) 
		{
			DataGridTableStyle gs = new DataGridTableStyle();
			gs.MappingName = o.GetType().Name;

			DataGridTextBoxColumn cs = new DataGridTextBoxColumn();
			cs.MappingName = "MessageID";
			cs.HeaderText = "#";
			cs.Width = 30;
			cs.ReadOnly = true;
			cs.TextBox.Enabled = false;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "Sender";
			cs.HeaderText = "Sender";	
			cs.Alignment = HorizontalAlignment.Right;
			cs.Width = 120;
			cs.TextBox.Enabled = false;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "Subject";
			cs.HeaderText = "Subject";
			cs.Width = 160;
			cs.Alignment = HorizontalAlignment.Right;
			cs.TextBox.Enabled = false;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "Timestamp";
			cs.HeaderText = "Date";
			cs.Alignment = HorizontalAlignment.Right;
			cs.Width = 180;
			cs.TextBox.Enabled = false;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "Contents";
			cs.HeaderText = "Contents";
			cs.Alignment = HorizontalAlignment.Right;
			cs.Width = 250;
			cs.TextBox.Enabled = false;
			gs.GridColumnStyles.Add(cs);

			return gs;
		}

		private void lstHeaders_DblClick(object sender, System.EventArgs e)
		{
			if (dgHeaders.CurrentRowIndex >= 0) 
			{
				//bool tamper = false;

/*
				System.Windows.Forms.DialogResult r = MessageBox.Show("Do you wish to tamper with the message?","Question:",MessageBoxButtons.YesNo);
				if (r == System.Windows.Forms.DialogResult.Yes) 
					tamper = true;
*/				
				FrmMessageView f = new FrmMessageView((WSEmailHeader)(((WSEmailHeader[])dgHeaders.DataSource)[dgHeaders.CurrentRowIndex]),false);
				f.MdiParent = this.MdiParent;
				f.BringToFront();
				if (f.IsDisposed != true && f.Disposing == false)
					f.Show();
			}
		}

		private void dgHeaders_Navigate(object sender, System.Windows.Forms.NavigateEventArgs ne)
		{

		}

		private void dgHeaders_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			
		}

		private void dgHeaders_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			dgHeaders.Select(dgHeaders.CurrentRowIndex);
			if (e.KeyCode == Keys.Delete) 
			{
				ArrayList al = new ArrayList();
				ArrayList il = new ArrayList();
				for(int i = 0; i < ((WSEmailHeader[])dgHeaders.DataSource).Length ; ++i) 
				{ 
					if(dgHeaders.IsSelected(i)) 
					{
						al.Add((((WSEmailHeader[])dgHeaders.DataSource)[i]).MessageID); 
						il.Add(i); 
					}
 				} 

				Global.Proxy.WSEmailDelete((int[])al.ToArray(typeof(int)));

				ArrayList a = new ArrayList();
				a.AddRange(((WSEmailHeader[])dgHeaders.DataSource));
				int offset = 0;
				foreach (int i in il.ToArray(typeof(int)))
					a.RemoveAt(i - offset++);

				dgHeaders.DataSource = (WSEmailHeader[])a.ToArray(typeof(WSEmailHeader));
			}
		}

		private void frmMessages_Load(object sender, System.EventArgs e)
		{
		
		}

		private void dgHeaders_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			System.Drawing.Point pt = new Point(e.X, e.Y); 
			DataGrid.HitTestInfo hti = dgHeaders.HitTest(pt); 
			if(hti.Type == DataGrid.HitTestType.Cell) 
			{ 
				dgHeaders.Select(hti.Row); 
			}
		
		}

		private void txtCheck_TextChanged(object sender, System.EventArgs e)
		{
			try 
			{
				//Global.CheckDelay = int.Parse(txtCheck.Text);
				timerCheck.Interval = Global.CheckDelay * 1000;
			}
			catch (Exception) 
			{
			}
			
		}

		private void timerCheck_Tick(object sender, System.EventArgs e)
		{
			btnCheck_Click(null,null);
		}

		private void chkServerNotification_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkServerNotification.Checked) 
			{
				this.timerCheck.Enabled = false;
				lock(this) 
				{
					UDPListenerThread = new Thread(new ThreadStart(UDPListener));
					UDPListenerThread.Start();
					Monitor.Wait(this);
				}

				try 
				{
					ExtensionArgument args = new ExtensionArgument("Register");
					args["ip"] = System.Net.Dns.GetHostName();
					args["port"] = UDPPort.ToString();
					Global.Proxy.ExecuteExtensionHandler("NewMailNotifier",args.AsXmlElement());
					Utilities.LogToStatusWindow("Registered for new mail notification from server.");
				} 
				catch (Exception ex) 
				{
					new ExceptionForm(ex,"Registering for new mail notification has failed.");
					this.chkServerNotification.Checked = false;
				}
			} 
			else 
			{
				this.timerCheck.Enabled = true;
				ShredUDPListener();
			}
		}

		private void UDPListener() 
		{
			try 
			{
				if (udpclient != null) 
				{
					udpclient.Close();
					udpclient = null;
				}


				this.UDPPort = GetRandomPort();
				bool connected = false;
				while (!connected) {
					
					try {
						
						udpclient = new UdpClient(UDPPort);
						connected = true;
					} 
					catch {
						
						UDPPort = GetRandomPort();
						Console.WriteLine("Getting new random port...");
					}
				}
				lock (this) {
					
					Monitor.Pulse(this);
				}
					IPEndPoint p = new IPEndPoint(IPAddress.Any, 0);
				while (true) 
				{
					byte[] bytes = udpclient.Receive(ref p);
					Utilities.LogToStatusWindow("Received UDP notification from server of new message.");
					Utilities.LogEvent(LogType.Informational,"Received UDP notification from server of new messages.");
					this.Invoke(new EventHandler(this.btnCheck_Click), new Object [] { this.btnCheck,null });
				}
			}
			catch (Exception e) { 
				try 
				{
					Console.WriteLine(e.Message + e.StackTrace);
					udpclient.Close();
				} 
				catch {}
			}
		
		}

		private void ShredUDPListener() 
		{
			if (UDPListenerThread != null) 
			{
				try 
				{
					ExtensionArgument args = new ExtensionArgument("Unregister");
					Global.Proxy.ExecuteExtensionHandler("NewMailNotifier",args.AsXmlElement());
					Utilities.LogToStatusWindow("Registered for new mail notification from server.");
				} 
				catch (Exception e) 
				{
					new ExceptionForm(e,"Unregistering for server notification has failed.");
				}

				UDPListenerThread.Abort();
				UDPListenerThread = null;
				udpclient.Close();
			}
		}

		private int GetRandomPort() 
		{
			Random r = new Random();
			int i = 1024 + r.Next(63333);
			return i;
		}

		private void FrmMessages_Closed(object sender, System.EventArgs e)
		{
			ShredUDPListener();
		}

		private void mnuCheck_Click(object sender, System.EventArgs e)
		{
			this.btnCheck_Click(null,null);
		}

		private void mnuExit_Click(object sender, System.EventArgs e)
		{
			this.Dispose();
		}

		private void dgHeaders_Resize(object sender, System.EventArgs e)
		{
			int j = dgHeaders.ClientWidth;
			for (int i = 0; i < dgHeaders.TableStyles[0].GridColumnStyles.Count; i++)
				j -= dgHeaders.TableStyles[0].GridColumnStyles[0].Width;

			MessageBox.Show("j=" + j.ToString());

				
//			MessageBox.Show(dgHeaders.VisibleWidth.ToString());
			dgHeaders.TableStyles[0].GridColumnStyles["Subject"].Width += j;
		}

	}
}
