/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using DynamicForms;
using WSEmailProxy;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using XmlAddressBook;
using Microsoft.Web.Services2.Security.Tokens;
using Microsoft.Web.Services2.Security.X509;


namespace WSEmailClientv2
{
	/// <summary>
	/// Summary description for frmMessageView.
	/// </summary>
	public class FrmMessageView : System.Windows.Forms.Form
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
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ListBox lstFormsAttached;
		private System.Windows.Forms.Button btnView;
		private System.Windows.Forms.Button btnFromPull;
		private BaseObject BO = null;

		public FrmMessageView()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		private void UpdateFormStatus() 
		{
			if (theMessage.theMessage.XmlAttachments!= null && theMessage.theMessage.XmlAttachments.Length > 0) 
			{
				btnView.Enabled = true;
				lstFormsAttached.Items.Clear();
				lstFormsAttached.Items.AddRange(ObjectLoader.GetFormInventory(theMessage.theMessage.XmlAttachments));
			} 
			else 
			{
				btnView.Enabled = false;
			}
		}

		public FrmMessageView(WSEmailHeader h, bool tamper) 
		{
			InitializeComponent();
			FrmWait f = new FrmWait("Retrieving mail message #"+h.MessageID+" from the mail server.");
			Global.LogToStatusWindow("Beginning download of WSEmail message #" +h.MessageID.ToString() + ".");
			try 
			{
				theMessage = Global.Proxy.WSEmailRetrieve(h.MessageID);
			} 
			catch (Exception e)
			{
				new PennLibraries.ExceptionForm(e,"There was an error downloading the message.");
				f.Dispose();
				this.Dispose();
				return;
			}
			if (theMessage.MessageID != h.MessageID) 
			{
				new PennLibraries.ExceptionForm(new Exception(),"Server did not send the requested message.");
				return;
			}

			Global.LogToStatusWindow("Finished downloading WSEmail message #" +h.MessageID.ToString() + ".");
			txtFrom.Text = theMessage.theMessage.Sender;
			txtTo.Text = theMessage.theMessage.Recipients.ToString();
			txtSubject.Text=theMessage.theMessage.Subject;
			txtMessage.Lines = theMessage.theMessage.Body.Split(Environment.NewLine.ToCharArray());
			this.Text = "Message '"+txtSubject.Text +"'";
			txtDate.Text=theMessage.theMessage.Timestamp.ToLongTimeString();
			if (tamper)
				((System.Xml.XmlElement)Global.Proxy.ResponseSoapContext.Envelope.GetElementsByTagName("Timestamp")[0]).InnerText = "Tampered";

			try 
			{
				System.Security.Cryptography.X509Certificates.X509Certificate c = theMessage.theMessage.VerifyReturningWSE1Cert(theMessage.sig);//PennLibraries.Utilities.VerifyLocalWSEmailMessageSignature(Global.Proxy.ResponseSoapContext.Envelope,theMessage.sig);
				txtSignature.Text = "Signed by: " + c.GetName();
				txtSignatureStatus.ForeColor = Color.Green;
				txtSignatureStatus.Text = "GOOD";
			} catch {
				txtSignature.Text = "Signature invalid!";
				txtSignatureStatus.ForeColor = Color.Red;
				txtSignatureStatus.Text = "BAD";
			}
			UpdateFormStatus();
			f.Dispose();

/*			if ( (theMessage.theMessage.MessageFlags & WSEmailFlags.Contains.DynamicForm ) == WSEmailFlags.Contains.DynamicForm) 
			{
				Global.LogToStatusWindow("WSEmail message #" +h.MessageID.ToString() + " requires signatures, asking user if they wish to process it.");
				DialogResult d = MessageBox.Show("This email has attachments. Would you like to process it now?","Question...",MessageBoxButtons.YesNo);
				if (d == System.Windows.Forms.DialogResult.Yes) {
					FrmSend frm = new FrmSend();
					frm.reply(theMessage.theMessage);
					frm.Show();
					this.Dispose();
				}
			}
*/		}


		private void FormDone() 
		{
			BO.Dispose();
		}

		private void FormDone(BaseObject b) 
		{
			BO.Dispose();
		}

		private void btnView_Click(object sender, System.EventArgs e)
		{

			if (theMessage.theMessage.XmlAttachments != null && theMessage.theMessage.XmlAttachments.Length > 0) 
			{
				if (lstFormsAttached.SelectedItem == null)
					MessageBox.Show("Please select which form you wish to view!","Oops!");
				else 
				{
					
					BO = ObjectLoader.LoadObject(theMessage.theMessage.XmlAttachments[lstFormsAttached.SelectedIndex].OuterXml);
					BO.Identifier = lstFormsAttached.SelectedIndex;
					BO.DoneEditing += new BaseObject.BaseObjectDelegate(FormDone);
					BO.UpdateAction = BaseObject.UpdateActions.REPLACE;
					BO.TokenRequest += new BaseObject.RequestTokenDelegate(TokenRequested);
					BO.Run();
				}
			}
		}

		private SecurityToken TokenRequested(BaseObject.TokenType t) 
		{
			if (t == BaseObject.TokenType.FederatedToken) 
			{
				DialogResult result = MessageBox.Show(
					"The running applet is requesting access to your federated token.\n\n" + 
					"Do you grant permission to access the token?","Security question...",
					MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
				
				if (result == DialogResult.Yes)
					return Global.FederatedTokenManager.Token;
				else
					return null;
			}
			return null;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FrmMessageView));
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
			this.label8 = new System.Windows.Forms.Label();
			this.lstFormsAttached = new System.Windows.Forms.ListBox();
			this.btnView = new System.Windows.Forms.Button();
			this.btnFromPull = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			this.label1.Text = "To:";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(8, 72);
			this.label2.Name = "label2";
			this.label2.TabIndex = 1;
			this.label2.Text = "Date:";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(8, 104);
			this.label3.Name = "label3";
			this.label3.TabIndex = 2;
			this.label3.Text = "Subject:";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(8, 40);
			this.label4.Name = "label4";
			this.label4.TabIndex = 3;
			this.label4.Text = "From:";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(8, 264);
			this.label5.Name = "label5";
			this.label5.TabIndex = 4;
			this.label5.Text = "Message:";
			// 
			// txtTo
			// 
			this.txtTo.Location = new System.Drawing.Point(112, 8);
			this.txtTo.Name = "txtTo";
			this.txtTo.Size = new System.Drawing.Size(256, 20);
			this.txtTo.TabIndex = 5;
			this.txtTo.Text = "textBox1";
			// 
			// txtFrom
			// 
			this.txtFrom.Location = new System.Drawing.Point(112, 40);
			this.txtFrom.Name = "txtFrom";
			this.txtFrom.Size = new System.Drawing.Size(256, 20);
			this.txtFrom.TabIndex = 6;
			this.txtFrom.Text = "textBox2";
			// 
			// txtDate
			// 
			this.txtDate.Location = new System.Drawing.Point(112, 72);
			this.txtDate.Name = "txtDate";
			this.txtDate.Size = new System.Drawing.Size(256, 20);
			this.txtDate.TabIndex = 7;
			this.txtDate.Text = "textBox3";
			// 
			// txtSubject
			// 
			this.txtSubject.Location = new System.Drawing.Point(112, 104);
			this.txtSubject.Name = "txtSubject";
			this.txtSubject.Size = new System.Drawing.Size(256, 20);
			this.txtSubject.TabIndex = 8;
			this.txtSubject.Text = "textBox4";
			// 
			// txtSignature
			// 
			this.txtSignature.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtSignature.Location = new System.Drawing.Point(112, 224);
			this.txtSignature.Multiline = true;
			this.txtSignature.Name = "txtSignature";
			this.txtSignature.Size = new System.Drawing.Size(544, 32);
			this.txtSignature.TabIndex = 9;
			this.txtSignature.Text = "textBox5";
			// 
			// txtMessage
			// 
			this.txtMessage.AcceptsReturn = true;
			this.txtMessage.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtMessage.Location = new System.Drawing.Point(112, 264);
			this.txtMessage.Multiline = true;
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.Size = new System.Drawing.Size(544, 288);
			this.txtMessage.TabIndex = 10;
			this.txtMessage.Text = "textBox6";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label6.Location = new System.Drawing.Point(8, 224);
			this.label6.Name = "label6";
			this.label6.TabIndex = 11;
			this.label6.Text = "Signature:";
			// 
			// txtSignatureStatus
			// 
			this.txtSignatureStatus.Location = new System.Drawing.Point(112, 192);
			this.txtSignatureStatus.Name = "txtSignatureStatus";
			this.txtSignatureStatus.Size = new System.Drawing.Size(256, 20);
			this.txtSignatureStatus.TabIndex = 12;
			this.txtSignatureStatus.Text = "textBox1";
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label7.Location = new System.Drawing.Point(8, 192);
			this.label7.Name = "label7";
			this.label7.TabIndex = 13;
			this.label7.Text = "Signature Status:";
			// 
			// btnReply
			// 
			this.btnReply.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnReply.Location = new System.Drawing.Point(544, 560);
			this.btnReply.Name = "btnReply";
			this.btnReply.Size = new System.Drawing.Size(112, 32);
			this.btnReply.TabIndex = 14;
			this.btnReply.Text = "Reply...";
			this.btnReply.Click += new System.EventHandler(this.btnReply_Click);
			// 
			// btnDel
			// 
			this.btnDel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnDel.Location = new System.Drawing.Point(416, 560);
			this.btnDel.Name = "btnDel";
			this.btnDel.Size = new System.Drawing.Size(112, 32);
			this.btnDel.TabIndex = 15;
			this.btnDel.Text = "Delete...";
			this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label8.Location = new System.Drawing.Point(8, 144);
			this.label8.Name = "label8";
			this.label8.TabIndex = 16;
			this.label8.Text = "Attachments:";
			// 
			// lstFormsAttached
			// 
			this.lstFormsAttached.Location = new System.Drawing.Point(112, 136);
			this.lstFormsAttached.Name = "lstFormsAttached";
			this.lstFormsAttached.Size = new System.Drawing.Size(256, 43);
			this.lstFormsAttached.TabIndex = 17;
			// 
			// btnView
			// 
			this.btnView.Location = new System.Drawing.Point(376, 152);
			this.btnView.Name = "btnView";
			this.btnView.Size = new System.Drawing.Size(75, 24);
			this.btnView.TabIndex = 18;
			this.btnView.Text = "View...";
			this.btnView.Click += new System.EventHandler(this.btnView_Click);
			// 
			// btnFromPull
			// 
			this.btnFromPull.Location = new System.Drawing.Point(376, 40);
			this.btnFromPull.Name = "btnFromPull";
			this.btnFromPull.TabIndex = 19;
			this.btnFromPull.Text = "Add...";
			this.btnFromPull.Click += new System.EventHandler(this.btnFromPull_Click);
			// 
			// FrmMessageView
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.DarkKhaki;
			this.ClientSize = new System.Drawing.Size(672, 605);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btnFromPull,
																		  this.btnView,
																		  this.lstFormsAttached,
																		  this.label8,
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
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FrmMessageView";
			this.Text = "WSEmail: Message View";
			this.ResumeLayout(false);

		}
		#endregion

		private void btnReply_Click(object sender, System.EventArgs e)
		{
			FrmSend f = new FrmSend();
			f.MdiParent = this.MdiParent;
			f.reply(theMessage.theMessage);
			f.Show();
		}

		private void btnDel_Click(object sender, System.EventArgs e)
		{
			Global.Proxy.WSEmailDelete(this.theMessage.MessageID);
			this.Hide();
			this.Dispose();
		}

		private void btnFromPull_Click(object sender, System.EventArgs e)
		{
			AddressBook book = AddressBook.GetInstance();
			AddressBookEntry b=new AddressBookEntry();
			b.AddDate = DateTime.Now;
			b.Email = this.txtFrom.Text;
			if (book.AddEntry(b)) 
				MessageBox.Show("Added " + b.Email + " to your address book.");
			else
				MessageBox.Show("The email address " + b.Email + " is already in your address book.");

		}
	}
}
