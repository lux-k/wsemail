using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WSEmailClientv2
{
	/// <summary>
	/// Summary description for FrmAddressView.
	/// </summary>
	public class FrmAddressView : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtFirst;
		private System.Windows.Forms.TextBox txtLast;
		private System.Windows.Forms.TextBox txtEmail;
		private System.Windows.Forms.TextBox txtNotes;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtDate;
		/// <summary>
		/// If the OK button was pressed, ie. to update the entry
		/// </summary>
		public bool IsOk = false;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrmAddressView()
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
		/// Initializes the form with a particular address.
		/// </summary>
		/// <param name="i"></param>
		public FrmAddressView(AddressItem i) 
		{
			InitializeComponent();
			this.txtDate.Text = i.Address.AddDate.ToString();
			this.txtEmail.Text = i.Address.Email;
			this.txtFirst.Text = i.Address.FirstName;
			this.txtLast.Text = i.Address.LastName;
			this.txtNotes.Text = i.Address.Notes;
		}

		public string Email 
		{
			get 
			{
				return this.txtEmail.Text;
			}
		}

		/// <summary>
		/// Returns the first name of the user
		/// </summary>
		public string First 
		{
			get 
			{
				return this.txtFirst.Text;
			}
		}

		/// <summary>
		/// Returns the last name of the user.
		/// </summary>
		public string Last 
		{
			get 
			{
				return this.txtLast.Text;
			}
		}

		/// <summary>
		/// Returns the notes for the user.
		/// </summary>
		public string Notes 
		{
			get 
			{
				return this.txtNotes.Text;
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
			this.txtFirst = new System.Windows.Forms.TextBox();
			this.txtLast = new System.Windows.Forms.TextBox();
			this.txtEmail = new System.Windows.Forms.TextBox();
			this.txtNotes = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.txtDate = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 8);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			this.label1.Text = "First Name:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(0, 40);
			this.label2.Name = "label2";
			this.label2.TabIndex = 1;
			this.label2.Text = "Last Name:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(0, 72);
			this.label3.Name = "label3";
			this.label3.TabIndex = 2;
			this.label3.Text = "Email:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(0, 136);
			this.label4.Name = "label4";
			this.label4.TabIndex = 3;
			this.label4.Text = "Notes:";
			// 
			// txtFirst
			// 
			this.txtFirst.Location = new System.Drawing.Point(104, 8);
			this.txtFirst.Name = "txtFirst";
			this.txtFirst.TabIndex = 0;
			this.txtFirst.Text = "";
			// 
			// txtLast
			// 
			this.txtLast.Location = new System.Drawing.Point(104, 40);
			this.txtLast.Name = "txtLast";
			this.txtLast.TabIndex = 1;
			this.txtLast.Text = "";
			// 
			// txtEmail
			// 
			this.txtEmail.Location = new System.Drawing.Point(104, 72);
			this.txtEmail.Name = "txtEmail";
			this.txtEmail.TabIndex = 2;
			this.txtEmail.Text = "";
			// 
			// txtNotes
			// 
			this.txtNotes.Location = new System.Drawing.Point(104, 136);
			this.txtNotes.Multiline = true;
			this.txtNotes.Name = "txtNotes";
			this.txtNotes.Size = new System.Drawing.Size(160, 128);
			this.txtNotes.TabIndex = 3;
			this.txtNotes.Text = "";
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(32, 296);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(152, 296);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(0, 104);
			this.label5.Name = "label5";
			this.label5.TabIndex = 10;
			this.label5.Text = "Addition Date:";
			// 
			// txtDate
			// 
			this.txtDate.Location = new System.Drawing.Point(104, 104);
			this.txtDate.Name = "txtDate";
			this.txtDate.ReadOnly = true;
			this.txtDate.TabIndex = 11;
			this.txtDate.Text = "";
			// 
			// FrmAddressView
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(272, 333);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.txtDate,
																		  this.label5,
																		  this.btnCancel,
																		  this.btnOK,
																		  this.txtNotes,
																		  this.txtEmail,
																		  this.txtLast,
																		  this.txtFirst,
																		  this.label4,
																		  this.label3,
																		  this.label2,
																		  this.label1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmAddressView";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Address Entry";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Sets the IsOK property of the form and closes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOK_Click(object sender, System.EventArgs e)
		{
			this.IsOk = true;
			this.Close();
		}

		/// <summary>
		/// Just closes the form.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
	}
}
