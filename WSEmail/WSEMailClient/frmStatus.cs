using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using PennLibraries;

namespace WSEMailClient
{
	/// <summary>
	/// Summary description for frmStatus.
	/// </summary>
	public class frmStatus : System.Windows.Forms.Form, IPostMessages
	{
		private System.Windows.Forms.TextBox txtMessages;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmStatus()
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
			this.txtMessages = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtMessages
			// 
			this.txtMessages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtMessages.Multiline = true;
			this.txtMessages.Name = "txtMessages";
			this.txtMessages.ReadOnly = true;
			this.txtMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtMessages.Size = new System.Drawing.Size(408, 349);
			this.txtMessages.TabIndex = 0;
			this.txtMessages.Text = "";
			// 
			// frmStatus
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(408, 349);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.txtMessages});
			this.Name = "frmStatus";
			this.Text = "WSEmail Status Log";
			this.Resize += new System.EventHandler(this.frmStatus_Resize);
			this.ResumeLayout(false);

		}
		#endregion
		/// <summary>
		/// Posts a new message to the txtMessages buffer. It will be appended with a new line and the txtMessages object
		/// will be scrolled to the end.
		/// </summary>
		/// <param name="s">The string to post.</param>
		public void PostMessage (string s) 
		{
			s ="["+ PennLibraries.Utilities.GetCurrentTime() + "] " + s;
			if (txtMessages.Text.Length != 0) 
				txtMessages.Text += "\r\n" + s;
			else
				txtMessages.Text = s;

			txtMessages.SelectionStart=txtMessages.Text.Length;
			txtMessages.ScrollToCaret();
			this.Focus();
			this.Show();
		}

		private void frmStatus_Resize(object sender, System.EventArgs e)
		{
			txtMessages.Width = this.Width - 30;
			txtMessages.Height = this.Width - 30;
		}


	}
}
