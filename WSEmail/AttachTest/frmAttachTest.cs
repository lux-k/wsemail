using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.Web.Services.Security;

using DistributedAttachment;
using DynamicForms;
using WSEmailProxy;
using XACLPolicy;
using PennLibraries;

namespace AttachTest
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmAttachTest : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btAddAttach;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtTo;
		private System.Windows.Forms.TextBox txtSubject;
		private System.Windows.Forms.TextBox txtBody;
		private System.Windows.Forms.Button btSend;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ListBox listAttachs;

		
		private WSMessageAttach wsm;
		private string sender;

		public Recipients recipients = null;

		public frmAttachTest(string vsender)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			recipients = new Recipients();
			recipients.addRecipient(new Recipient("Jianqing Zhang","arrowC1@Capricorn"));
			recipients.addRecipient(new Recipient("arrow","arrowATC@Capricorn"));

			this.Text = vsender + "-" + this.Text;
			sender = vsender;

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			wsm = new WSMessageAttach();
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
			this.btAddAttach = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtTo = new System.Windows.Forms.TextBox();
			this.txtSubject = new System.Windows.Forms.TextBox();
			this.txtBody = new System.Windows.Forms.TextBox();
			this.btSend = new System.Windows.Forms.Button();
			this.listAttachs = new System.Windows.Forms.ListBox();
			this.label4 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btAddAttach
			// 
			this.btAddAttach.Location = new System.Drawing.Point(144, 360);
			this.btAddAttach.Name = "btAddAttach";
			this.btAddAttach.Size = new System.Drawing.Size(92, 32);
			this.btAddAttach.TabIndex = 0;
			this.btAddAttach.Text = "AddAttach";
			this.btAddAttach.Click += new System.EventHandler(this.btAddAttach_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(16, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = "To:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(16, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 20);
			this.label2.TabIndex = 2;
			this.label2.Text = "Subject:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(16, 156);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 20);
			this.label3.TabIndex = 3;
			this.label3.Text = "Body:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtTo
			// 
			this.txtTo.Location = new System.Drawing.Point(116, 8);
			this.txtTo.Name = "txtTo";
			this.txtTo.Size = new System.Drawing.Size(220, 20);
			this.txtTo.TabIndex = 4;
			this.txtTo.Text = "arrowC1@Capricorn,arrowATC@Capricorn";
			// 
			// txtSubject
			// 
			this.txtSubject.Location = new System.Drawing.Point(116, 40);
			this.txtSubject.Name = "txtSubject";
			this.txtSubject.Size = new System.Drawing.Size(220, 20);
			this.txtSubject.TabIndex = 5;
			this.txtSubject.Text = "Attach Test";
			// 
			// txtBody
			// 
			this.txtBody.Location = new System.Drawing.Point(116, 152);
			this.txtBody.Multiline = true;
			this.txtBody.Name = "txtBody";
			this.txtBody.Size = new System.Drawing.Size(220, 188);
			this.txtBody.TabIndex = 6;
			this.txtBody.Text = "Test for Attachment";
			// 
			// btSend
			// 
			this.btSend.Location = new System.Drawing.Point(244, 360);
			this.btSend.Name = "btSend";
			this.btSend.Size = new System.Drawing.Size(92, 32);
			this.btSend.TabIndex = 7;
			this.btSend.Text = "Send";
			this.btSend.Click += new System.EventHandler(this.btSend_Click);
			// 
			// listAttachs
			// 
			this.listAttachs.Location = new System.Drawing.Point(116, 76);
			this.listAttachs.Name = "listAttachs";
			this.listAttachs.Size = new System.Drawing.Size(220, 69);
			this.listAttachs.TabIndex = 10;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(16, 76);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(96, 20);
			this.label4.TabIndex = 9;
			this.label4.Text = "Attachments";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// frmAttachTest
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(350, 408);
			this.Controls.Add(this.txtBody);
			this.Controls.Add(this.txtSubject);
			this.Controls.Add(this.txtTo);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.listAttachs);
			this.Controls.Add(this.btSend);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btAddAttach);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "frmAttachTest";
			this.Text = "Send Mail";
			this.ResumeLayout(false);

		}
		#endregion

		private void btAddAttach_Click(object sender, System.EventArgs e)
		{
			DistributedAttachment.DistributedAttachment disAttach = new DistributedAttachment.DistributedAttachment(recipients);
			disAttach.Run();
			if (disAttach.fileattachments != null)
				for(int i=0;i< disAttach.fileattachments.fas.Length;i++)
					listAttachs.Items.Add(disAttach.fileattachments.fas[i].FileName);
			
			wsm.Fileattachments = disAttach.fileattachments;
			wsm.policy = disAttach.policy;
			/*wsm.Fileattachments = new FileAttachment[disAttach.fileattachments.numFa];
			for(int i=0;i<disAttach.fileattachments.numFa;i++)
				wsm.Fileattachments[i] = disAttach.fileattachments.fas[i];
			wsm.policy = disAttach.policy;*/

		}

		private void btSend_Click(object sender, System.EventArgs e)
		{
			//DistributedAttachment.WSMessageAttach wsm = new WSMessageAttach();
			//WSEmailProxy.WSEmailMessage wsm = new WSEmailMessage();

			wsm.Sender = this.sender;
			wsm.Body = this.txtBody.Text;
			wsm.Subject = this.txtSubject.Text;
			wsm.Timestamp = PennLibraries.Utilities.GetCurrentTime();
			wsm.Recipients.Add(recipients.recs[0].EmailAddress);
			wsm.Recipients.Add(recipients.recs[1].EmailAddress);

			//PennLibraries.Utilities.printXmlObject(wsm);

			//MessageBox.Show("The message has been print out");


			MailAttachService masproxy = new MailAttachService();

			//msproxy.SecurityToken = new UsernameToken("arrowC1","pennsec",PasswordOption.SendNone);


			WSEmailProxy.WSEmailStatus wseStatus = masproxy.WSEmailSend(wsm,null);
			MessageBox.Show(wseStatus.ToString());

			Close();


		}


	}
}
