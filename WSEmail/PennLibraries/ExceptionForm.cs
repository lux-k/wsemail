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

namespace PennLibraries
{
	/// <summary>
	/// Summary description for ExceptionForm.
	/// </summary>
	public class ExceptionForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.RichTextBox txtMessage;
		private System.Windows.Forms.RichTextBox txtDetail;
		private System.Windows.Forms.Button btnDetail;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnReport;
		private bool more = false;
		//private int count = 0;
		private int enterCount = 0;
		private string str = "";

		public ExceptionForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public ExceptionForm(Exception e,string Message)
		{
			this.InitializeComponent();
			this.txtDetail.Top = this.btnDetail.Top + this.btnDetail.Height + 10;
			this.txtDetail.Height = 290;
			this.Text = "An error has occurred";
			this.txtMessage.Text = Message;
			this.txtDetail.Text = "Application: " + Application.ProductName + "\r\n";
			this.txtDetail.Text += "Version: " + Application.ProductVersion + "\r\n\r\n";
			this.txtDetail.Text += "Exception: " + e.Message + "\r\n\r\n" + "Stack trace: " + e.StackTrace;

			try
			{

				if (e.Message !=null)
				{
					
					String [] str1 = new string[10];
					enterCount = e.Message.IndexOf("\n");
					str = e.Message.Substring(0,enterCount+1);
					char ch1 = ':';
					str1 = str.Split(ch1);
					
					if (str1.Length >1)
					{
						str=str1[1];
					}
					else 
					{
						str = str1[0];
					}                         
				}
			}
			catch (System.Exception ex) 
			{
				this.txtMessage.Text +="\r\n\r\n" + ex.Message + "\r\n\r\n";
			}
			switch (str.Trim())
			{
				case "The operation has timed-out." :
					this.txtMessage.Text += "\r\n\r\nOur server may be experiencing heavy load. Please try again later. r\n\r\n";
					break;
				case "The underlying connection was closed: An unexpected error occurred on a receive." :
					this.txtMessage.Text += "\r\n\r\nPlease try again the procedure \r\n\r\n";
					break;
				case "Client found response content type of 'text/html; charset=utf-8', but expected 'text/xml'.The request failed with the error message:" :
					this.txtMessage.Text += "\r\n\r\nThe server seems to be improperly configured. Please contact the system administrator. We are sorry for the inconvenience \r\n\r\n";
					break;
				case "Server unavailable, please try later --> Bad Key." :
					this.txtMessage.Text += "\r\n\r\nOur server may be experiencing heavy load. We are looking into it. Please try again later. \r\n\r\n";
					break;
				case "The underlying connection was closed: Unable to connect to the remote server." :
					this.txtMessage.Text += "\r\n\r\nContact your administrator and check the firewall settings. \r\n\r\n";
					break;
				case "Message Expired" :
					this.txtMessage.Text += "\r\n\r\nPlease ensure that your clock is syncronised and resend the message \r\n\r\n";
					break;
				case "The security token could not be authenticated or authorized" :
					this.txtMessage.Text += "\r\n\r\nPlease enter the correct Username and password and try again \r\n\r\n";
					break;
				}
			
			if (str.IndexOf("-")>0)
			{
				str = str.Substring(0,str.IndexOf("-"));
				//MessageBox.Show(str);
				switch(str.Trim())
				{
					case "Server unavailable, please try later":
						this.txtMessage.Text += "\r\n\r\n Please check the username password and try again. If further failures contact the system administrator \r\n\r\n";
						break;
					case "Server was unable to process request.":
						this.txtMessage.Text += "\r\n\r\n We are experiencing some technical difficulties. Please try again after some time. \r\n\r\n";
						break;
					default:
						this.txtMessage.Text += "\r\n\r\n Contact your administrator urgently. \r\n\r\n";
						break;
				}
			}


			if (e.InnerException != null)
				this.txtDetail.Text += "\r\n\r\nInner Exception: " + e.InnerException.Message + "\r\n\r\n" + "Inner stack trace: " + e.InnerException.StackTrace;
			Utilities.LogToStatusWindow(Message);
			this.ShowDialog();
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
			this.btnDetail = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.txtMessage = new System.Windows.Forms.RichTextBox();
			this.txtDetail = new System.Windows.Forms.RichTextBox();
			this.btnReport = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnDetail
			// 
			this.btnDetail.Location = new System.Drawing.Point(192, 144);
			this.btnDetail.Name = "btnDetail";
			this.btnDetail.Size = new System.Drawing.Size(112, 32);
			this.btnDetail.TabIndex = 0;
			this.btnDetail.Text = "<<< More Detail";
			this.btnDetail.Click += new System.EventHandler(this.btnDetail_Click);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(320, 144);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(88, 32);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// txtMessage
			// 
			this.txtMessage.Location = new System.Drawing.Point(8, 8);
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.Size = new System.Drawing.Size(400, 128);
			this.txtMessage.TabIndex = 2;
			this.txtMessage.Text = "txtMessage";
			// 
			// txtDetail
			// 
			this.txtDetail.Location = new System.Drawing.Point(8, 168);
			this.txtDetail.Name = "txtDetail";
			this.txtDetail.Size = new System.Drawing.Size(400, 264);
			this.txtDetail.TabIndex = 3;
			this.txtDetail.Text = "txtDetail";
			this.txtDetail.Visible = false;
			// 
			// btnReport
			// 
			this.btnReport.Location = new System.Drawing.Point(8, 144);
			this.btnReport.Name = "btnReport";
			this.btnReport.Size = new System.Drawing.Size(112, 32);
			this.btnReport.TabIndex = 4;
			this.btnReport.Text = "Report Problem...";
			this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
			// 
			// ExceptionForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(418, 183);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btnReport,
																		  this.txtDetail,
																		  this.txtMessage,
																		  this.btnOK,
																		  this.btnDetail});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExceptionForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ExceptionForm";
			this.ResumeLayout(false);

		}
		#endregion

		private void btnDetail_Click(object sender, System.EventArgs e)
		{
			more = !more;

			if (more) 
			{
				this.Height += 300;
				this.txtDetail.Visible = true;
				this.btnDetail.Text = " >>> Less Detail";
			} 
			else 
			{
				this.txtDetail.Visible = false;
				this.btnDetail.Text = " <<< More Detail";
				this.Height -= 300;
			}
		
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			this.Hide();
			this.Dispose();
		}

		private void btnReport_Click(object sender, System.EventArgs e)
		{
			DialogResult rst = MessageBox.Show("This will anonymously send the error report to the WSEmail development team.\r\n\r\n" +
				"The only information that will be transmitted is the version of this client and the exception" +
				" traces from the 'More Information' button.\r\n\r\nIs that Ok?","Error Reporting",
				MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
			if (rst == DialogResult.Yes) 
			{
				WebReporter r = new WebReporter();
				bool res = r.ReportError(this.txtDetail.Text);

				if (res) 
					MessageBox.Show("Successfully reported the error to the development team.\r\n\r\n" +
						"Thank you for taking the time to help us improve our project!","Thank you!");
				else
					MessageBox.Show("Guess what... We couldn't even report the error. This is bad, eh?",
						"Ooops...");
			}
		}
	}
}
