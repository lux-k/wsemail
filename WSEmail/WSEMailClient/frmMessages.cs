using WSEmailProxy;
using System.Configuration;
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using PennLibraries;

namespace WSEMailClient
{
	/// <summary>
	/// Summary description for frmMessage.
	/// </summary>
	public class frmMessages : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnCheck;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.DataGrid dgHeaders;
		private System.Windows.Forms.TextBox txtCheck;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Timer timerCheck;
		private System.ComponentModel.IContainer components;
		private Form newmail = null;

		public frmMessages()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.txtCheck.Text = Global.CheckDelay.ToString();
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
			if (newmail != null)
				newmail.Dispose();
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
			this.btnCheck = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.dgHeaders = new System.Windows.Forms.DataGrid();
			this.txtCheck = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.timerCheck = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.dgHeaders)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCheck
			// 
			this.btnCheck.Location = new System.Drawing.Point(592, 208);
			this.btnCheck.Name = "btnCheck";
			this.btnCheck.Size = new System.Drawing.Size(96, 32);
			this.btnCheck.TabIndex = 1;
			this.btnCheck.Text = "Check Again...";
			this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(8, 192);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(464, 104);
			this.textBox1.TabIndex = 2;
			this.textBox1.Text = @"Click the button to the right to check your mail.

No password is needed to check your mail because your request to the mail server will be signed with the private key of your certificate.

The server can verify your identity by checking the signature of your request against your public key.";
			// 
			// dgHeaders
			// 
			this.dgHeaders.CaptionText = "Inbox";
			this.dgHeaders.DataMember = "";
			this.dgHeaders.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgHeaders.Location = new System.Drawing.Point(8, 8);
			this.dgHeaders.Name = "dgHeaders";
			this.dgHeaders.ReadOnly = true;
			this.dgHeaders.Size = new System.Drawing.Size(792, 176);
			this.dgHeaders.TabIndex = 3;
			this.dgHeaders.Tag = "";
			this.dgHeaders.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgHeaders_KeyDown);
			this.dgHeaders.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgHeaders_KeyPress);
			this.dgHeaders.DoubleClick += new System.EventHandler(this.lstHeaders_DblClick);
			this.dgHeaders.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgHeaders_MouseUp);
			this.dgHeaders.Navigate += new System.Windows.Forms.NavigateEventHandler(this.dgHeaders_Navigate);
			// 
			// txtCheck
			// 
			this.txtCheck.Location = new System.Drawing.Point(608, 272);
			this.txtCheck.Name = "txtCheck";
			this.txtCheck.Size = new System.Drawing.Size(56, 20);
			this.txtCheck.TabIndex = 4;
			this.txtCheck.Text = "";
			this.txtCheck.TextChanged += new System.EventHandler(this.txtCheck_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(536, 272);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.TabIndex = 5;
			this.label1.Text = "Check every";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(672, 272);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 16);
			this.label2.TabIndex = 6;
			this.label2.Text = "seconds.";
			// 
			// timerCheck
			// 
			this.timerCheck.Enabled = true;
			this.timerCheck.Interval = 10000;
			this.timerCheck.Tick += new System.EventHandler(this.timerCheck_Tick);
			// 
			// frmMessages
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(808, 301);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label2,
																		  this.label1,
																		  this.txtCheck,
																		  this.dgHeaders,
																		  this.textBox1,
																		  this.btnCheck});
			this.Name = "frmMessages";
			this.Text = "Check your messages...";
			this.Load += new System.EventHandler(this.frmMessages_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgHeaders)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void btnCheck_Click(object sender, System.EventArgs e)
		{
//			frmWait frm = new frmWait("Retrieving message headers from Mail Server " + ConfigurationSettings.AppSettings["MailServer"] + ".");
			Utilities.LogToStatusWindow("Beginning to download WSEmail headers.");
			
//			Utilities.LogTransit(Global.Proxy.Url,"To");
			WSEmailHeader[] wsh = Global.Proxy.WSEmailFetchHeaders();

			if (wsh == null) 
			{
				MessageBox.Show("You have no messages.","Oops!");
				textBox1.Text = "You don't have any mail.\r\n\r\nYou can click the button to check again.";
			}
		 
			else 
			{
				int count = 0;
				int spot = dgHeaders.CurrentRowIndex;
				if (dgHeaders.DataSource != null)
					count = ((WSEmailHeader[])dgHeaders.DataSource).Length;

				if (wsh.Length != count) 
				{
					dgHeaders.TableStyles.Clear();
					dgHeaders.TableStyles.Add(GetDataGridTableStyle(wsh));
					dgHeaders.DataSource = wsh;

					if (spot >= 0 && spot < wsh.Length) 
					{
						dgHeaders.CurrentRowIndex = spot;
					}

				}

				if (wsh.Length > count && count != 0)  
				{
					if (newmail == null)
						newmail = new NewMail(this);
				
					newmail.Show();
				}

		
				//MessageBox.Show("New mail has arrived.");
				textBox1.Text = "You have mail!\r\n\r\nDouble click on any message to view its contents.\r\nPress the delete key on any selected message to delete it.";
			}
			Utilities.LogToStatusWindow("Done downloading WSEmail headers.");
			Utilities.LogTransit(Global.Proxy.Url,"Erase");
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
				bool tamper = false;

/*
				System.Windows.Forms.DialogResult r = MessageBox.Show("Do you wish to tamper with the message?","Question:",MessageBoxButtons.YesNo);
				if (r == System.Windows.Forms.DialogResult.Yes) 
					tamper = true;
*/				
				frmMessageView f = new frmMessageView((WSEmailHeader)(((WSEmailHeader[])dgHeaders.DataSource)[dgHeaders.CurrentRowIndex]),tamper);
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
				Global.CheckDelay = int.Parse(txtCheck.Text);
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
	}
}
