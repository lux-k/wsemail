using System;
using WSEmailProxy;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.Web.Services.Security;

namespace SimpleSend
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button button1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

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
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(160, 32);
			this.textBox1.Name = "textBox1";
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "arrowS1@Scorpio";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(160, 64);
			this.textBox2.Name = "textBox2";
			this.textBox2.TabIndex = 1;
			this.textBox2.Text = "TestFromMe";
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(160, 96);
			this.textBox3.Name = "textBox3";
			this.textBox3.TabIndex = 2;
			this.textBox3.Text = "Just A Test";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 32);
			this.label1.Name = "label1";
			this.label1.TabIndex = 4;
			this.label1.Text = "to:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 64);
			this.label3.Name = "label3";
			this.label3.TabIndex = 6;
			this.label3.Text = "subject:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 96);
			this.label4.Name = "label4";
			this.label4.TabIndex = 7;
			this.label4.Text = "message:";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(184, 128);
			this.button1.Name = "button1";
			this.button1.TabIndex = 9;
			this.button1.Text = "button1";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(416, 333);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBox3);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.textBox1);
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

		private void button1_Click(object sender, System.EventArgs e)
		{
			WSEmailMessage m= new WSEmailMessage();
			m.Body = textBox3.Text;
			m.Subject = textBox2.Text;
			m.Recipients.Add(textBox1.Text);
			m.Timestamp = DateTime.Now.ToString();

			//MailServerProxy p = new MailServerProxy("http://tower/WSEmailServer/MailServer.asmx");
			MailServerProxy p = new MailServerProxy("http://localhost/WSEMailServer_5/MailServer.asmx");
			p.SecurityToken = new UsernameToken("kevin","superbuh",PasswordOption.SendNone);
//			p.SecurityTokenRetriever = new WSEmailProxy.TokenGrabber(ShowAuthenticationScreen);

			WSEmailStatus s = p.WSEmailSend(m,null);
			MessageBox.Show(s.ToString());
		}

		public Microsoft.Web.Services.Security.SecurityToken ShowAuthenticationScreen() 
		{
			PennLibraries.AuthenticationForm f = new PennLibraries.AuthenticationForm();
			f.ShowDialog();
			SecurityToken s = f.SecurityToken;
			if (s != null) 
			{
			} 
			else 
				MessageBox.Show("No security token was given. Things probably won't work correctly.");
			f.Dispose();
			return s;
		}
	}
}
