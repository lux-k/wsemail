/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
//using Microsoft.Win32;
using System.Configuration;
using System.Xml;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace SysCon
{
	/// <summary>
	/// Summary description for FrmConfig.
	/// </summary>
	public class FrmConfig : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtUser;
		private System.Windows.Forms.TextBox txtUrl;
		private System.Windows.Forms.Button btnOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Config _config = null;

		public Config Configuration 
		{
			get 
			{
				return _config;
			}
			set 
			{
				_config=value;
				this.txtUrl.Text = _config.URL;
				this.txtUser.Text = _config.Username;
			}
		}

		public FrmConfig()
		{
			//
			// Required for Windows Form Designer support
			//
			_config = new Config();
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public FrmConfig(Config c) 
		{
			InitializeComponent();
			this.Configuration = c;

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
			this.txtUser = new System.Windows.Forms.TextBox();
			this.txtUrl = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			this.label1.Text = "Username:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.TabIndex = 1;
			this.label2.Text = "Server URL:";
			// 
			// txtUser
			// 
			this.txtUser.Location = new System.Drawing.Point(112, 16);
			this.txtUser.Name = "txtUser";
			this.txtUser.Size = new System.Drawing.Size(136, 20);
			this.txtUser.TabIndex = 2;
			this.txtUser.Text = "";
			// 
			// txtUrl
			// 
			this.txtUrl.Location = new System.Drawing.Point(112, 48);
			this.txtUrl.Name = "txtUrl";
			this.txtUrl.Size = new System.Drawing.Size(136, 20);
			this.txtUrl.TabIndex = 3;
			this.txtUrl.Text = "";
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(168, 112);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "Ok";
			this.btnOK.Click += new System.EventHandler(this.button1_Click);
			// 
			// FrmConfig
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(256, 149);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label1,
																		  this.label2,
																		  this.txtUser,
																		  this.btnOK,
																		  this.txtUrl});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmConfig";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Configuration";
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			if (Configuration == null)
				Configuration = new Config();

			this.Configuration.URL = txtUrl.Text;
			this.Configuration.Username = txtUser.Text;

			bool ok = true;
			try 
			{
				Uri u = new Uri(this.Configuration.URL);
			} 
			catch
			{
				MessageBox.Show("The ServerURL appears to be improperly formed.","Oops!");
				ok = false;
			}

			if (ok)
				this.Hide();
			
		}
	}
	public class Config 
	{
		public string Username;
		public string URL;
		public static Config GetConfig(string file) 
		{
			if (File.Exists(file)) 
			{
				System.Xml.Serialization.XmlSerializer x = new XmlSerializer(typeof(Config));
				return (Config)x.Deserialize(File.Open(file,FileMode.Open));
			}
			return null;
		}
		public static void SetConfig(Config c, string file) 
		{
			System.Xml.Serialization.XmlSerializer x = new XmlSerializer(c.GetType());
			x.Serialize(File.OpenWrite(file),c);
		}
	}
}
