using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using DistributedAttachment;
using WSEmailProxy;

namespace AttachTest
{
	/// <summary>
	/// Summary description for frmMailList.
	/// </summary>
	public class frmMailList : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListView lvMsgList;
		private System.Windows.Forms.Button btOpen;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ColumnHeader ID;
		private System.Windows.Forms.ColumnHeader Subject;
		private System.Windows.Forms.ColumnHeader Date;
		private System.Windows.Forms.ColumnHeader Senders;
		private System.Windows.Forms.Button btCheck;
		private System.Windows.Forms.Button btClose;

		private string user;

		public frmMailList(string vuser)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			user = vuser;
			this.Text = user + "-" + this.Text;

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lvMsgList = new System.Windows.Forms.ListView();
			this.ID = new System.Windows.Forms.ColumnHeader();
			this.Subject = new System.Windows.Forms.ColumnHeader();
			this.Date = new System.Windows.Forms.ColumnHeader();
			this.Senders = new System.Windows.Forms.ColumnHeader();
			this.btOpen = new System.Windows.Forms.Button();
			this.btCheck = new System.Windows.Forms.Button();
			this.btClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lvMsgList
			// 
			this.lvMsgList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.ID,
																						this.Subject,
																						this.Date,
																						this.Senders});
			this.lvMsgList.FullRowSelect = true;
			this.lvMsgList.Location = new System.Drawing.Point(8, 8);
			this.lvMsgList.Name = "lvMsgList";
			this.lvMsgList.Size = new System.Drawing.Size(556, 268);
			this.lvMsgList.TabIndex = 0;
			this.lvMsgList.View = System.Windows.Forms.View.Details;
			// 
			// ID
			// 
			this.ID.Text = "ID";
			// 
			// Subject
			// 
			this.Subject.Text = "Subject";
			this.Subject.Width = 130;
			// 
			// Date
			// 
			this.Date.Text = "Date";
			this.Date.Width = 220;
			// 
			// Senders
			// 
			this.Senders.Text = "Sender";
			this.Senders.Width = 140;
			// 
			// btOpen
			// 
			this.btOpen.Location = new System.Drawing.Point(208, 292);
			this.btOpen.Name = "btOpen";
			this.btOpen.Size = new System.Drawing.Size(112, 32);
			this.btOpen.TabIndex = 3;
			this.btOpen.Text = "Open Mail";
			this.btOpen.Click += new System.EventHandler(this.btOpen_Click);
			// 
			// btCheck
			// 
			this.btCheck.Location = new System.Drawing.Point(330, 292);
			this.btCheck.Name = "btCheck";
			this.btCheck.Size = new System.Drawing.Size(112, 32);
			this.btCheck.TabIndex = 4;
			this.btCheck.Text = "Check Again";
			this.btCheck.Click += new System.EventHandler(this.btCheck_Click);
			// 
			// btClose
			// 
			this.btClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btClose.Location = new System.Drawing.Point(452, 292);
			this.btClose.Name = "btClose";
			this.btClose.Size = new System.Drawing.Size(112, 32);
			this.btClose.TabIndex = 5;
			this.btClose.Text = "Close";
			this.btClose.Click += new System.EventHandler(this.btClose_Click);
			// 
			// frmMailList
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btClose;
			this.ClientSize = new System.Drawing.Size(574, 336);
			this.Controls.Add(this.btClose);
			this.Controls.Add(this.btCheck);
			this.Controls.Add(this.btOpen);
			this.Controls.Add(this.lvMsgList);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmMailList";
			this.Text = "MessageList";
			this.Load += new System.EventHandler(this.frmMailList_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void frmMailList_Load(object sender, System.EventArgs e)
		{
			checkMail();
		}

		private void btClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void checkMail()
		{
			DistributedAttachment.MailAttachService mas = new MailAttachService();
			WSEmailProxy.WSEmailHeader[] wsehs = mas.WSEmailFetchHeaders(this.user);
			
			ListViewItem[] livs = new ListViewItem[wsehs.Length];
			for(int i=0;i<wsehs.Length;i++)
			{
				livs[i] = new ListViewItem(wsehs[i].MessageID.ToString());
				//livs[i].SubItems.Add(wsehs[i].MessageID.ToString());
				livs[i].SubItems.Add(wsehs[i].Subject);
				livs[i].SubItems.Add(wsehs[i].Timestamp);
				livs[i].SubItems.Add(wsehs[i].Sender);
			}
			lvMsgList.Items.Clear();
			this.lvMsgList.Items.AddRange(livs);
		}

		private void btCheck_Click(object sender, System.EventArgs e)
		{
			checkMail();
		}

		private void btOpen_Click(object sender, System.EventArgs e)
		{
			if (this.lvMsgList.SelectedItems.Count == 0)
			{
				MessageBoxButtons buttons = MessageBoxButtons.OK;
				string caption = "No mail selected";
				string msg = "You must select a message from Message List!";
				MessageBox.Show(this,msg,caption,buttons,MessageBoxIcon.Exclamation);
				return;
			}
			int msgid = Convert.ToInt32(lvMsgList.SelectedItems[0].SubItems[0].Text);
			FrmMessageView fmv = new FrmMessageView(msgid,this.user);
			fmv.ShowDialog();
		}
	}
}
