using System;
using WSEmailProxy;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;

namespace WSEMailClient
{
	/// <summary>
	/// Summary description for frmMessageView.
	/// </summary>
	public class frmMessageView : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtTo;
		private System.Windows.Forms.TextBox txtFrom;
		private System.Windows.Forms.TextBox txtDate;
		private System.Windows.Forms.TextBox txtSubject;
		private System.Windows.Forms.TextBox txtSignature;
		private System.Windows.Forms.TextBox txtMessage;
		private System.Windows.Forms.Label label6;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox txtSignatureStatus;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button btnReply;
		private System.Windows.Forms.Button btnDel;
		private WSEmailPackage theMessage = null;
		// private BusinessObjectsFormInterface frmForm = null;

		public frmMessageView()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
		public frmMessageView(WSEmailHeader h, bool tamper) 
		{
			InitializeComponent();
			frmWait f = new frmWait("Retrieving mail message #"+h.MessageID+" from the mail server.");
			Global.LogToStatusWindow("Beginning download of WSEmail message #" +h.MessageID.ToString() + ".");
			try 
			{
				theMessage = Global.Proxy.WSEmailRetrieve(h.MessageID);
			} 
			catch 
			{
				MessageBox.Show("There was an error downloading the message.","Oops!");
				f.Dispose();
				this.Dispose();
				return;
			}

			Global.LogToStatusWindow("Finished downloading WSEmail message #" +h.MessageID.ToString() + ".");
			txtFrom.Text = theMessage.theMessage.Sender;
			txtTo.Text = theMessage.theMessage.Recipients.ToString();
			txtSubject.Text=theMessage.theMessage.Subject;
			txtMessage.Lines = theMessage.theMessage.Body.Split(Environment.NewLine.ToCharArray());
			//MessageBox.Show(p.UnCleanWSEmailMessage(theMessage.theMessage.Body));
			//MessageBox.Show("Len=" + Environment.NewLine.Length.ToString());
			//txtMessage.Text=p.UnCleanWSEmailMessage(theMessage.theMessage.Body);
			txtDate.Text=theMessage.theMessage.Timestamp;
			if (tamper)
				((System.Xml.XmlElement)Global.Proxy.ResponseSoapContext.Envelope.GetElementsByTagName("Timestamp")[0]).InnerText = "Tampered";

			try 
			{
				Microsoft.Web.Services.Security.X509.X509Certificate c = PennLibraries.Utilities.VerifyLocalWSEmailMessageSignature(Global.Proxy.ResponseSoapContext.Envelope,theMessage.sig);
				txtSignature.Text = "Signed by: " + c.GetName();
				txtSignatureStatus.ForeColor = Color.Green;
				txtSignatureStatus.Text = "GOOD";
			} catch {
				txtSignature.Text = "Signature invalid!";
				txtSignatureStatus.ForeColor = Color.Red;
				txtSignatureStatus.Text = "BAD";
			}

			f.Dispose();

			if ( (theMessage.theMessage.MessageFlags & WSEmailFlags.Contains.DynamicForm ) == WSEmailFlags.Contains.DynamicForm) 
			{
				Global.LogToStatusWindow("WSEmail message #" +h.MessageID.ToString() + " requires signatures, asking user if they wish to process it.");
				DialogResult d = MessageBox.Show("This email has attachments. Would you like to process it now?","Question...",MessageBoxButtons.YesNo);
				if (d == System.Windows.Forms.DialogResult.Yes) {
					frmSend frm = new frmSend();
					frm.reply(theMessage.theMessage);
					frm.Show();
					this.Dispose();
				}
			}
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
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.txtTo = new System.Windows.Forms.TextBox();
			this.txtFrom = new System.Windows.Forms.TextBox();
			this.txtDate = new System.Windows.Forms.TextBox();
			this.txtSubject = new System.Windows.Forms.TextBox();
			this.txtSignature = new System.Windows.Forms.TextBox();
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txtSignatureStatus = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.btnReply = new System.Windows.Forms.Button();
			this.btnDel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(32, 16);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			this.label1.Text = "To:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(32, 80);
			this.label2.Name = "label2";
			this.label2.TabIndex = 1;
			this.label2.Text = "Date:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(32, 112);
			this.label3.Name = "label3";
			this.label3.TabIndex = 2;
			this.label3.Text = "Subject:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(32, 48);
			this.label4.Name = "label4";
			this.label4.TabIndex = 3;
			this.label4.Text = "From:";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(32, 208);
			this.label5.Name = "label5";
			this.label5.TabIndex = 4;
			this.label5.Text = "Message:";
			// 
			// txtTo
			// 
			this.txtTo.Location = new System.Drawing.Point(136, 16);
			this.txtTo.Name = "txtTo";
			this.txtTo.Size = new System.Drawing.Size(256, 20);
			this.txtTo.TabIndex = 5;
			this.txtTo.Text = "textBox1";
			// 
			// txtFrom
			// 
			this.txtFrom.Location = new System.Drawing.Point(136, 48);
			this.txtFrom.Name = "txtFrom";
			this.txtFrom.Size = new System.Drawing.Size(256, 20);
			this.txtFrom.TabIndex = 6;
			this.txtFrom.Text = "textBox2";
			// 
			// txtDate
			// 
			this.txtDate.Location = new System.Drawing.Point(136, 80);
			this.txtDate.Name = "txtDate";
			this.txtDate.Size = new System.Drawing.Size(256, 20);
			this.txtDate.TabIndex = 7;
			this.txtDate.Text = "textBox3";
			// 
			// txtSubject
			// 
			this.txtSubject.Location = new System.Drawing.Point(136, 112);
			this.txtSubject.Name = "txtSubject";
			this.txtSubject.Size = new System.Drawing.Size(256, 20);
			this.txtSubject.TabIndex = 8;
			this.txtSubject.Text = "textBox4";
			// 
			// txtSignature
			// 
			this.txtSignature.Location = new System.Drawing.Point(136, 176);
			this.txtSignature.Name = "txtSignature";
			this.txtSignature.Size = new System.Drawing.Size(512, 20);
			this.txtSignature.TabIndex = 9;
			this.txtSignature.Text = "textBox5";
			// 
			// txtMessage
			// 
			this.txtMessage.AcceptsReturn = true;
			this.txtMessage.Location = new System.Drawing.Point(136, 208);
			this.txtMessage.Multiline = true;
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.Size = new System.Drawing.Size(512, 288);
			this.txtMessage.TabIndex = 10;
			this.txtMessage.Text = "textBox6";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(32, 176);
			this.label6.Name = "label6";
			this.label6.TabIndex = 11;
			this.label6.Text = "Signature:";
			// 
			// txtSignatureStatus
			// 
			this.txtSignatureStatus.Location = new System.Drawing.Point(136, 144);
			this.txtSignatureStatus.Name = "txtSignatureStatus";
			this.txtSignatureStatus.Size = new System.Drawing.Size(256, 20);
			this.txtSignatureStatus.TabIndex = 12;
			this.txtSignatureStatus.Text = "textBox1";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(32, 144);
			this.label7.Name = "label7";
			this.label7.TabIndex = 13;
			this.label7.Text = "Signature Status:";
			// 
			// btnReply
			// 
			this.btnReply.Location = new System.Drawing.Point(528, 512);
			this.btnReply.Name = "btnReply";
			this.btnReply.Size = new System.Drawing.Size(112, 32);
			this.btnReply.TabIndex = 14;
			this.btnReply.Text = "Reply...";
			this.btnReply.Click += new System.EventHandler(this.btnReply_Click);
			// 
			// btnDel
			// 
			this.btnDel.Location = new System.Drawing.Point(400, 512);
			this.btnDel.Name = "btnDel";
			this.btnDel.Size = new System.Drawing.Size(112, 32);
			this.btnDel.TabIndex = 15;
			this.btnDel.Text = "Delete...";
			this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
			// 
			// frmMessageView
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(680, 597);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btnDel,
																		  this.btnReply,
																		  this.label7,
																		  this.txtSignatureStatus,
																		  this.label6,
																		  this.txtMessage,
																		  this.txtSignature,
																		  this.txtSubject,
																		  this.txtDate,
																		  this.txtFrom,
																		  this.txtTo,
																		  this.label5,
																		  this.label4,
																		  this.label3,
																		  this.label2,
																		  this.label1});
			this.Name = "frmMessageView";
			this.Text = "WSEmail: Message View";
			this.ResumeLayout(false);

		}
		#endregion

		private void btnReply_Click(object sender, System.EventArgs e)
		{
//			MessageBox.Show("Reply function turned off frmMessageView.cs:292");

			frmSend f = new frmSend();
			f.reply(theMessage.theMessage);
			f.Show();

		}

		private void btnDel_Click(object sender, System.EventArgs e)
		{
			Global.Proxy.WSEmailDelete(this.theMessage.MessageID);
			this.Hide();
			this.Dispose();
		}

	}
}
