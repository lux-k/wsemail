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

namespace WSEmailConfigurator
{
	/// <summary>
	/// Summary description for BrowseServers.
	/// </summary>
	public class BrowseServers : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ComboBox servers;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button btnDone;
		private string _url = "";

		public string URL 
		{
			get 
			{
				return _url;
			}
			set 
			{
//				MessageBox.Show("Url = " + this.Url);
				_url = value;
			}
		}

		public BrowseServers()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

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
			this.servers = new System.Windows.Forms.ComboBox();
			this.btnDone = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// servers
			// 
			this.servers.Location = new System.Drawing.Point(16, 8);
			this.servers.Name = "servers";
			this.servers.Size = new System.Drawing.Size(432, 21);
			this.servers.TabIndex = 0;
			this.servers.Text = "comboBox1";
			this.servers.SelectionChangeCommitted += new System.EventHandler(this.servers_SelectionChangeCommitted);
			// 
			// btnDone
			// 
			this.btnDone.Location = new System.Drawing.Point(384, 104);
			this.btnDone.Name = "btnDone";
			this.btnDone.Size = new System.Drawing.Size(72, 32);
			this.btnDone.TabIndex = 1;
			this.btnDone.Text = "Done...";
			this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
			// 
			// BrowseServers
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(464, 141);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btnDone,
																		  this.servers});
			this.Name = "BrowseServers";
			this.Text = "BrowseServers";
			this.Load += new System.EventHandler(this.BrowseServers_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void BrowseServers_Load(object sender, System.EventArgs e)
		{
			AvailableServers a = new AvailableServers();
			servers.Items.Clear();
			servers.Items.AddRange(a.GetServers());
			servers.SelectedIndex = 0;
			servers_SelectionChangeCommitted(null,null);
		}

		private void servers_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (servers.SelectedIndex >= 0)
				this.URL = ((Server)servers.Items[servers.SelectedIndex]).Url;
		}

		private void btnDone_Click(object sender, System.EventArgs e)
		{
			servers_SelectionChangeCommitted(null,null);
			this.Close();
			this.Hide();
		}
	}
}
