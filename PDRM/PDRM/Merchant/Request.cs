using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using CommonTypes;

namespace Merchant
{
	/// <summary>
	/// Summary description for Request.
	/// </summary>
	public class Request : System.Windows.Forms.Form
	{
		protected CommonTypes.User m_user;
		public User theUser
		{
			get { return m_user; }
			set 
			{
				m_user = value;

				// reload the values to the screen
				ReloadData();
			}
		}

		/// <summary>
		/// The GIS Server for this device
		/// </summary>
		public string Server
		{
			get { return this.tbGISServer.Text; }
			set { this.tbGISServer.Text = value; }
		}

		/// <summary>
		/// Request the right to send normal ads to the device
		/// </summary>
		public bool SendNormalAds
		{
			get { return this.cbNormalAds.Checked; }
			set { this.cbNormalAds.Checked = value; }
		}

		/// <summary>
		/// Request the right to send offensive ads to the device
		/// </summary>
		public bool SendOffensiveAds
		{
			get { return this.cbOffensiveAds.Checked; }
			set { this.cbOffensiveAds.Checked = value; }
		}

		/// <summary>
		/// Request the right to send discounts to the device
		/// </summary>
		public bool SendDiscounts
		{
			get { return this.cbDiscounts.Checked; }
			set { this.cbDiscounts.Checked = value; }
		}

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.TextBox tbGISServer;
		private System.Windows.Forms.TextBox tbEntry;
		private System.Windows.Forms.GroupBox RightsGroup;
		private System.Windows.Forms.CheckBox cbNormalAds;
		private System.Windows.Forms.Button bSend;
		private System.Windows.Forms.Button bCancel;
		private System.Windows.Forms.CheckBox cbOffensiveAds;
		private System.Windows.Forms.CheckBox cbDiscounts;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Request()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.m_user = new User();
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.tbName = new System.Windows.Forms.TextBox();
			this.tbGISServer = new System.Windows.Forms.TextBox();
			this.tbEntry = new System.Windows.Forms.TextBox();
			this.RightsGroup = new System.Windows.Forms.GroupBox();
			this.cbDiscounts = new System.Windows.Forms.CheckBox();
			this.cbOffensiveAds = new System.Windows.Forms.CheckBox();
			this.cbNormalAds = new System.Windows.Forms.CheckBox();
			this.bSend = new System.Windows.Forms.Button();
			this.bCancel = new System.Windows.Forms.Button();
			this.RightsGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "GIS Server";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 96);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "Time of Entry:";
			// 
			// tbName
			// 
			this.tbName.Location = new System.Drawing.Point(96, 16);
			this.tbName.Name = "tbName";
			this.tbName.ReadOnly = true;
			this.tbName.Size = new System.Drawing.Size(160, 20);
			this.tbName.TabIndex = 3;
			this.tbName.Text = "";
			// 
			// tbGISServer
			// 
			this.tbGISServer.Location = new System.Drawing.Point(96, 56);
			this.tbGISServer.Name = "tbGISServer";
			this.tbGISServer.ReadOnly = true;
			this.tbGISServer.Size = new System.Drawing.Size(160, 20);
			this.tbGISServer.TabIndex = 4;
			this.tbGISServer.Text = "";
			// 
			// tbEntry
			// 
			this.tbEntry.Location = new System.Drawing.Point(96, 96);
			this.tbEntry.Name = "tbEntry";
			this.tbEntry.ReadOnly = true;
			this.tbEntry.Size = new System.Drawing.Size(160, 20);
			this.tbEntry.TabIndex = 5;
			this.tbEntry.Text = "";
			// 
			// RightsGroup
			// 
			this.RightsGroup.Controls.Add(this.cbDiscounts);
			this.RightsGroup.Controls.Add(this.cbOffensiveAds);
			this.RightsGroup.Controls.Add(this.cbNormalAds);
			this.RightsGroup.Location = new System.Drawing.Point(16, 128);
			this.RightsGroup.Name = "RightsGroup";
			this.RightsGroup.Size = new System.Drawing.Size(240, 136);
			this.RightsGroup.TabIndex = 6;
			this.RightsGroup.TabStop = false;
			this.RightsGroup.Text = "Rights to request";
			// 
			// cbDiscounts
			// 
			this.cbDiscounts.Location = new System.Drawing.Point(16, 64);
			this.cbDiscounts.Name = "cbDiscounts";
			this.cbDiscounts.Size = new System.Drawing.Size(128, 24);
			this.cbDiscounts.TabIndex = 2;
			this.cbDiscounts.Text = "Send Any Discount";
			// 
			// cbOffensiveAds
			// 
			this.cbOffensiveAds.Location = new System.Drawing.Point(16, 104);
			this.cbOffensiveAds.Name = "cbOffensiveAds";
			this.cbOffensiveAds.Size = new System.Drawing.Size(128, 24);
			this.cbOffensiveAds.TabIndex = 1;
			this.cbOffensiveAds.Text = "Send Offensive Ads";
			// 
			// cbNormalAds
			// 
			this.cbNormalAds.Location = new System.Drawing.Point(16, 24);
			this.cbNormalAds.Name = "cbNormalAds";
			this.cbNormalAds.Size = new System.Drawing.Size(128, 24);
			this.cbNormalAds.TabIndex = 0;
			this.cbNormalAds.Text = "Send Normal Ads";
			// 
			// bSend
			// 
			this.bSend.Location = new System.Drawing.Point(184, 280);
			this.bSend.Name = "bSend";
			this.bSend.TabIndex = 7;
			this.bSend.Text = "Send";
			this.bSend.Click += new System.EventHandler(this.bSend_Click);
			// 
			// bCancel
			// 
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(96, 280);
			this.bCancel.Name = "bCancel";
			this.bCancel.TabIndex = 8;
			this.bCancel.Text = "Cancel";
			this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
			// 
			// Request
			// 
			this.AcceptButton = this.bSend;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.bCancel;
			this.ClientSize = new System.Drawing.Size(272, 309);
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.bSend);
			this.Controls.Add(this.RightsGroup);
			this.Controls.Add(this.tbEntry);
			this.Controls.Add(this.tbGISServer);
			this.Controls.Add(this.tbName);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Request";
			this.Text = "Details of License Request";
			this.Load += new System.EventHandler(this.Request_Load);
			this.RightsGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Reload the textboxes for the window with the data from the stored
		/// Device object.
		/// </summary>
		private void ReloadData()
		{
			// reset the text boxes to match the stored device data
			this.tbName.Text = this.m_user.Name;
			this.tbEntry.Text = this.m_user.EntryTime.ToString();
		}

		private void Request_Load(object sender, System.EventArgs e)
		{
			// reload the data first
			ReloadData();

			// and reset the checkboxes
			this.SendDiscounts = false;
			this.SendNormalAds = false;
			this.SendOffensiveAds = false;
		}

		private void bCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;

			this.Close();
		}

		private void bSend_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;

			this.Close();
		}
	}
}
