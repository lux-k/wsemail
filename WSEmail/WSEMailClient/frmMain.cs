using System.Threading;
using System.Configuration;
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Web.Services.Security;
using WSEmailProxy;

using FederatedBinaryToken;
using System.Security.Cryptography;


namespace WSEMailClient
{
	/// <summary>
	/// Summary description for frmMain.
	/// </summary>		
	/// 
	

	public class frmMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnSend;
		private System.Windows.Forms.Button btnCheck;
		private frmSend formSend = null;
		private frmMessages formMessages = null;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Label label2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		[STAThread]
		static void Main() 
		{
			// PennRoutingFilters.PennRoutingUtilities.AddPennRoutingFilters(false);
			PennLibraries.Utilities.AddPennLoggingFilters();
			//PennRoutingFilters.PennRoutingUtilities.AddWSEMailSignatureFilters(true);
			MailServerProxy p = Global.Proxy;
			Application.Run(new frmMain());
		}

		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Global.Proxy.SecurityTokenRetriever = new WSEmailProxy.TokenGrabber(ShowAuthenticationScreen);
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			string blurb = "Welcome to a demonstration of the Penn Security Lab WSEmail Client!\r\n\r\n";
			blurb += "This mail client and server system is based entirely off of Microsoft .Net Web Services. ";
			blurb += "By leveraging the additional features contained in the WSE (Web Services Enhancement) package such as Web Services Routing and Security, this system can run seamlessly and securely over the web. ";
			blurb += "Custom routing filters provide complete end-to-end security.";
 
			this.textBox1.Text = blurb;
			blurb = "User: " + ConfigurationSettings.AppSettings["SigningCertificate"]	+ "\r\n\r\nWeb Service: " + ConfigurationSettings.AppSettings["MailServer"];
			this.textBox2.Text=blurb;
		}

		public Microsoft.Web.Services.Security.SecurityToken ShowAuthenticationScreen() 
		{
			PennLibraries.AuthenticationForm f = new PennLibraries.AuthenticationForm();
			f.ShowDialog();
			SecurityToken s = f.SecurityToken;
			f.Dispose();
			return s;
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
			if (formMessages != null)
				formMessages.Dispose();
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmMain));
			this.btnSend = new System.Windows.Forms.Button();
			this.btnCheck = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnSend
			// 
			this.btnSend.BackColor = System.Drawing.Color.Gainsboro;
			this.btnSend.Location = new System.Drawing.Point(520, 128);
			this.btnSend.Name = "btnSend";
			this.btnSend.Size = new System.Drawing.Size(112, 40);
			this.btnSend.TabIndex = 0;
			this.btnSend.Text = "Send Message...";
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			// 
			// btnCheck
			// 
			this.btnCheck.BackColor = System.Drawing.Color.Gainsboro;
			this.btnCheck.Location = new System.Drawing.Point(520, 216);
			this.btnCheck.Name = "btnCheck";
			this.btnCheck.Size = new System.Drawing.Size(112, 40);
			this.btnCheck.TabIndex = 1;
			this.btnCheck.Text = "Check Mail...";
			this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Bitmap)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(480, 56);
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			// 
			// textBox1
			// 
			this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textBox1.Location = new System.Drawing.Point(16, 104);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(472, 312);
			this.textBox1.TabIndex = 3;
			this.textBox1.Text = "";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(184, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(304, 32);
			this.label1.TabIndex = 4;
			this.label1.Text = "WSEmail Client v1.5";
			// 
			// textBox2
			// 
			this.textBox2.BackColor = System.Drawing.Color.White;
			this.textBox2.Location = new System.Drawing.Point(504, 312);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.ReadOnly = true;
			this.textBox2.Size = new System.Drawing.Size(152, 104);
			this.textBox2.TabIndex = 5;
			this.textBox2.Text = "textBox2";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(504, 296);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(136, 16);
			this.label2.TabIndex = 6;
			this.label2.Text = "Configuration:";
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(664, 421);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label2,
																		  this.textBox2,
																		  this.label1,
																		  this.textBox1,
																		  this.pictureBox1,
																		  this.btnCheck,
																		  this.btnSend});
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Penn Security Lab WSEmail Client";
			this.ResumeLayout(false);

		}
		#endregion

		private void btnSend_Click(object sender, System.EventArgs e)
		{
			if (formSend == null || formSend.IsDisposed)
				formSend = new frmSend();
			formSend.Show();
			formSend.BringToFront();
		}

		private void btnCheck_Click(object sender, System.EventArgs e)
		{
			if (formMessages == null || formMessages.IsDisposed) 
				formMessages = new frmMessages();
			formMessages.Show();
			formMessages.BringToFront();
		}
	}
}
