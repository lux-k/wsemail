using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace BusinessObjects
{
	/// <summary>
	/// Summary description for FrmDelegate.
	/// </summary>
	public class FrmDelegates : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListBox lstDelegates;
		private System.Windows.Forms.TextBox txtAdd;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TextBox txtBlurb;
		private ArrayList dels = new ArrayList();
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrmDelegates()
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
			this.lstDelegates = new System.Windows.Forms.ListBox();
			this.txtAdd = new System.Windows.Forms.TextBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.txtBlurb = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// lstDelegates
			// 
			this.lstDelegates.Items.AddRange(new object[] {
															  ""});
			this.lstDelegates.Location = new System.Drawing.Point(176, 16);
			this.lstDelegates.Name = "lstDelegates";
			this.lstDelegates.Size = new System.Drawing.Size(200, 121);
			this.lstDelegates.TabIndex = 0;
			this.lstDelegates.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstDelegatesKey);
			// 
			// txtAdd
			// 
			this.txtAdd.Location = new System.Drawing.Point(8, 16);
			this.txtAdd.Name = "txtAdd";
			this.txtAdd.Size = new System.Drawing.Size(144, 20);
			this.txtAdd.TabIndex = 1;
			this.txtAdd.Text = "";
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(72, 56);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(80, 32);
			this.btnAdd.TabIndex = 2;
			this.btnAdd.Text = "Add...";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(272, 216);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(88, 40);
			this.button2.TabIndex = 3;
			this.button2.Text = "Done...";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// txtBlurb
			// 
			this.txtBlurb.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.txtBlurb.Enabled = false;
			this.txtBlurb.Location = new System.Drawing.Point(8, 152);
			this.txtBlurb.Multiline = true;
			this.txtBlurb.Name = "txtBlurb";
			this.txtBlurb.Size = new System.Drawing.Size(240, 144);
			this.txtBlurb.TabIndex = 4;
			this.txtBlurb.Text = "";
			// 
			// FrmDelegates
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(384, 309);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.txtBlurb,
																		  this.button2,
																		  this.btnAdd,
																		  this.txtAdd,
																		  this.lstDelegates});
			this.Name = "FrmDelegates";
			this.Text = "Delegates";
			this.Load += new System.EventHandler(this.FrmDelegates_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FrmDelegates_Load(object sender, System.EventArgs e)
		{
			txtBlurb.Text = "You can delegate the current form to other entities.\n\nYou must name a group of entities that must also sign this form in order for your signature to be valid.\n\nTo make sure that your delegation isn't altered, the delegated signature blocks are also signed when you sign the request. Verifying your signature with the delegates changed or without all the required delegate signatures will fail.";
		}

		public void DisplayDelegates (string[] del) 
		{
			lstDelegates.Items.Clear();
			if (del != null && del.Length > 0) 
				lstDelegates.Items.AddRange(del);
		}

		public string[] GetDelegates() 
		{
			return (string[])(new ArrayList(lstDelegates.Items).ToArray(typeof(string)));
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			if(!txtAdd.Text.Equals("")) 
				lstDelegates.Items.Add(txtAdd.Text);
			txtAdd.Text="";
		}

		private void lstDelegatesKey(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == System.Windows.Forms.Keys.Delete)
				lstDelegates.Items.RemoveAt(lstDelegates.SelectedIndex);
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}
	}
}
