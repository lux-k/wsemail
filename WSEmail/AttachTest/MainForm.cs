using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace AttachTest
{
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btSendMail;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtSender;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox txtRecipient;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MainForm()
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

		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btSendMail = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.txtSender = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.txtRecipient = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btSendMail
			// 
			this.btSendMail.Location = new System.Drawing.Point(288, 28);
			this.btSendMail.Name = "btSendMail";
			this.btSendMail.Size = new System.Drawing.Size(160, 28);
			this.btSendMail.TabIndex = 0;
			this.btSendMail.Text = "Send Mail";
			this.btSendMail.Click += new System.EventHandler(this.btSendMail_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(68, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(68, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = "Sender:";
			// 
			// txtSender
			// 
			this.txtSender.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtSender.Location = new System.Drawing.Point(140, 28);
			this.txtSender.Multiline = true;
			this.txtSender.Name = "txtSender";
			this.txtSender.Size = new System.Drawing.Size(140, 28);
			this.txtSender.TabIndex = 2;
			this.txtSender.Text = "arrowS1@Scorpio";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(68, 88);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(68, 20);
			this.label2.TabIndex = 3;
			this.label2.Text = "Recipient:";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(288, 84);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(160, 28);
			this.button1.TabIndex = 4;
			this.button1.Text = "Check Mail";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// txtRecipient
			// 
			this.txtRecipient.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtRecipient.Location = new System.Drawing.Point(140, 84);
			this.txtRecipient.Multiline = true;
			this.txtRecipient.Name = "txtRecipient";
			this.txtRecipient.Size = new System.Drawing.Size(140, 28);
			this.txtRecipient.TabIndex = 5;
			this.txtRecipient.Text = "arrowC1@Capricorn";
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(476, 154);
			this.Controls.Add(this.txtRecipient);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtSender);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btSendMail);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "On-Demand Attachment-Simple Demo";
			this.ResumeLayout(false);

		}
		#endregion

		private void btSendMail_Click(object sender, System.EventArgs e)
		{
			frmAttachTest frmat = new frmAttachTest(this.txtSender.Text);
			frmat.ShowDialog();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			frmMailList frml = new frmMailList(this.txtRecipient.Text);
			frml.ShowDialog();
		}
	}
}
