using System;
using Microsoft.Web.Services.Security;
using Microsoft.Web.Services.Security.X509;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WSEMailClient
{
	/// <summary>
	/// Summary description for AuthenticationForm.
	/// </summary>
	public class AuthenticationForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtUser;
		private System.Windows.Forms.TextBox txtPass;
		private System.Windows.Forms.Button btnUserPassOK;
		private System.Windows.Forms.ComboBox cmbStore;
		private System.Windows.Forms.ComboBox cmbCert;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnCertOK;

		private SecurityToken _sectok;

		public SecurityToken SecurityToken 
		{
			get 
			{
				return _sectok;
			}
			set 
			{
				_sectok = value;
			}
		}

		public AuthenticationForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.txtPass.Text = "superbuh";
			this.txtUser.Text = "kevin";
			this.btnUserPassOK.PerformClick();

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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.btnUserPassOK = new System.Windows.Forms.Button();
			this.txtPass = new System.Windows.Forms.TextBox();
			this.txtUser = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.btnCertOK = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbCert = new System.Windows.Forms.ComboBox();
			this.cmbStore = new System.Windows.Forms.ComboBox();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.tabPage1,
																					  this.tabPage2});
			this.tabControl1.Location = new System.Drawing.Point(8, 8);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(424, 328);
			this.tabControl1.TabIndex = 0;
			this.tabControl1.TabStop = false;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.btnUserPassOK,
																				   this.txtPass,
																				   this.txtUser,
																				   this.label2,
																				   this.label1});
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(416, 302);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Username / Password";
			// 
			// btnUserPassOK
			// 
			this.btnUserPassOK.Location = new System.Drawing.Point(312, 240);
			this.btnUserPassOK.Name = "btnUserPassOK";
			this.btnUserPassOK.Size = new System.Drawing.Size(80, 40);
			this.btnUserPassOK.TabIndex = 4;
			this.btnUserPassOK.Text = "OK...";
			this.btnUserPassOK.Click += new System.EventHandler(this.btnUserPassOK_Click);
			// 
			// txtPass
			// 
			this.txtPass.Location = new System.Drawing.Point(128, 56);
			this.txtPass.Name = "txtPass";
			this.txtPass.PasswordChar = '*';
			this.txtPass.Size = new System.Drawing.Size(200, 20);
			this.txtPass.TabIndex = 3;
			this.txtPass.Text = "";
			this.txtPass.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUser_KeyDown);
			// 
			// txtUser
			// 
			this.txtUser.Location = new System.Drawing.Point(128, 24);
			this.txtUser.Name = "txtUser";
			this.txtUser.Size = new System.Drawing.Size(200, 20);
			this.txtUser.TabIndex = 2;
			this.txtUser.Text = "";
			this.txtUser.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUser_KeyDown);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 56);
			this.label2.Name = "label2";
			this.label2.TabIndex = 1;
			this.label2.Text = "Password:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			this.label1.Text = "Username:";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.btnCertOK,
																				   this.label4,
																				   this.label3,
																				   this.cmbCert,
																				   this.cmbStore});
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(416, 302);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "X509 Certificate";
			// 
			// btnCertOK
			// 
			this.btnCertOK.Location = new System.Drawing.Point(312, 240);
			this.btnCertOK.Name = "btnCertOK";
			this.btnCertOK.Size = new System.Drawing.Size(80, 40);
			this.btnCertOK.TabIndex = 4;
			this.btnCertOK.Text = "OK...";
			this.btnCertOK.Click += new System.EventHandler(this.btnCertOK_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 64);
			this.label4.Name = "label4";
			this.label4.TabIndex = 3;
			this.label4.Text = "Certificate:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 24);
			this.label3.Name = "label3";
			this.label3.TabIndex = 2;
			this.label3.Text = "Certificate store:";
			// 
			// cmbCert
			// 
			this.cmbCert.Location = new System.Drawing.Point(128, 64);
			this.cmbCert.Name = "cmbCert";
			this.cmbCert.Size = new System.Drawing.Size(240, 21);
			this.cmbCert.TabIndex = 1;
			this.cmbCert.Text = "Select...";
			// 
			// cmbStore
			// 
			this.cmbStore.Items.AddRange(new object[] {
														  "User",
														  "Machine"});
			this.cmbStore.Location = new System.Drawing.Point(128, 24);
			this.cmbStore.Name = "cmbStore";
			this.cmbStore.Size = new System.Drawing.Size(240, 21);
			this.cmbStore.TabIndex = 0;
			this.cmbStore.Text = "Select...";
			this.cmbStore.SelectedIndexChanged += new System.EventHandler(this.cmbStore_SelectedIndexChanged);
			// 
			// AuthenticationForm
			// 
			this.AutoScale = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(448, 341);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl1});
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AuthenticationForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "WSEmail Authentication information...";
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnUserPassOK_Click(object sender, System.EventArgs e)
		{
			if (!(txtUser.Text.Length > 0 && txtPass.Text.Length > 0)) 
			{
				MessageBox.Show("Please fill in your username and password!");
				return;
			}

			this.SecurityToken = new UsernameToken(txtUser.Text,txtPass.Text,PasswordOption.SendNone);
			Done();
		}

		private void Done() 
		{
			// MessageBox.Show("Do you want to save your security token to disk?");
			// MessageBox.Show(SecurityToken.ToString());
			// MessageBox.Show(SecurityToken.GetXml(new System.Xml.XmlDocument()).OuterXml);
			this.Hide();
		}

		private void cmbStore_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			X509CertificateStore store = null;
			switch (((string)cmbStore.SelectedItem).ToLower()) 
			{
				case "user":
					store = X509CertificateStore.CurrentUserStore(X509CertificateStore.MyStore);
					break;
				case "machine":
					store = X509CertificateStore.LocalMachineStore(X509CertificateStore.MyStore);
					break;
			}
			store.Open();
			cmbCert.Items.Clear();
			foreach (X509Certificate c in store.Certificates) 
				cmbCert.Items.Add(PennRoutingFilters.PennRoutingUtilities.GetCertCN(c));

			store.Close();
		}

		private void btnCertOK_Click(object sender, System.EventArgs e)
		{
			if (cmbStore.SelectedIndex < 0 || cmbCert.SelectedIndex < 0) 
			{
				MessageBox.Show("Please select a certificate to use!");
				return;
			}

			bool machinestore = false;
			if (((string)cmbStore.SelectedItem).ToLower().Equals("machine"))
				machinestore = true;
			
			if (cmbCert.SelectedIndex >= 0)
			{
				this.SecurityToken = new X509SecurityToken(PennRoutingFilters.PennRoutingUtilities.GetSecurityToken((string)cmbCert.SelectedItem,machinestore).Certificate);
				Done();
			}
		}

		private void txtUser_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
				this.btnUserPassOK.PerformClick();
		}
	}
}
