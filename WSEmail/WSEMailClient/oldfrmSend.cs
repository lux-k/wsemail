using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using WSEmailProxy;
using Microsoft.Web.Services.Timestamp;
using System.Configuration;
using Microsoft.Web.Services.Security.X509;
using System.Security.Cryptography;
using System.Runtime.Remoting.Messaging;
using Microsoft.Web.Services;
using Microsoft.Web.Services.Security;
using BusinessObjects;
using Microsoft.Web.Services.Configuration;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using PennRoutingFilters;

namespace WSEMailClient
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class oldfrmSend : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox txtSender;
		private System.Windows.Forms.TextBox txtSubject;
		private System.Windows.Forms.TextBox txtRecipient;
		private System.Windows.Forms.TextBox txtBody;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnSend;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox chkIM;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox chkDelIMUnD;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox cmbForms;
		private System.Windows.Forms.Button btnAddForm;
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private BusinessObjectsFormInterface frmForm = null;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button btnView;
		private System.Windows.Forms.Label lblFormAttached;
		private System.Windows.Forms.StatusBar statusBar1;
	
		public delegate WSEmailStatus sendMessageDelegate(WSEmailMessage m, XmlElement sig);
		MailServerProxy m = new MailServerProxy(ConfigurationSettings.AppSettings["MailServer"]);

		public oldfrmSend()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			cmbForms.DataSource = BusinessObjects.AvailableForms.GetForms();
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmSend));
			this.txtSender = new System.Windows.Forms.TextBox();
			this.txtSubject = new System.Windows.Forms.TextBox();
			this.txtRecipient = new System.Windows.Forms.TextBox();
			this.txtBody = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.btnSend = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.chkIM = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.chkDelIMUnD = new System.Windows.Forms.CheckBox();
			this.label7 = new System.Windows.Forms.Label();
			this.cmbForms = new System.Windows.Forms.ComboBox();
			this.btnAddForm = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.lblFormAttached = new System.Windows.Forms.Label();
			this.btnView = new System.Windows.Forms.Button();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.SuspendLayout();
			// 
			// txtSender
			// 
			this.txtSender.AccessibleDescription = ((string)(resources.GetObject("txtSender.AccessibleDescription")));
			this.txtSender.AccessibleName = ((string)(resources.GetObject("txtSender.AccessibleName")));
			this.txtSender.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtSender.Anchor")));
			this.txtSender.AutoSize = ((bool)(resources.GetObject("txtSender.AutoSize")));
			this.txtSender.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtSender.BackgroundImage")));
			this.txtSender.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtSender.Dock")));
			this.txtSender.Enabled = ((bool)(resources.GetObject("txtSender.Enabled")));
			this.txtSender.Font = ((System.Drawing.Font)(resources.GetObject("txtSender.Font")));
			this.txtSender.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtSender.ImeMode")));
			this.txtSender.Location = ((System.Drawing.Point)(resources.GetObject("txtSender.Location")));
			this.txtSender.MaxLength = ((int)(resources.GetObject("txtSender.MaxLength")));
			this.txtSender.Multiline = ((bool)(resources.GetObject("txtSender.Multiline")));
			this.txtSender.Name = "txtSender";
			this.txtSender.PasswordChar = ((char)(resources.GetObject("txtSender.PasswordChar")));
			this.txtSender.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtSender.RightToLeft")));
			this.txtSender.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtSender.ScrollBars")));
			this.txtSender.Size = ((System.Drawing.Size)(resources.GetObject("txtSender.Size")));
			this.txtSender.TabIndex = ((int)(resources.GetObject("txtSender.TabIndex")));
			this.txtSender.Text = resources.GetString("txtSender.Text");
			this.txtSender.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtSender.TextAlign")));
			this.txtSender.Visible = ((bool)(resources.GetObject("txtSender.Visible")));
			this.txtSender.WordWrap = ((bool)(resources.GetObject("txtSender.WordWrap")));
			// 
			// txtSubject
			// 
			this.txtSubject.AccessibleDescription = ((string)(resources.GetObject("txtSubject.AccessibleDescription")));
			this.txtSubject.AccessibleName = ((string)(resources.GetObject("txtSubject.AccessibleName")));
			this.txtSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtSubject.Anchor")));
			this.txtSubject.AutoSize = ((bool)(resources.GetObject("txtSubject.AutoSize")));
			this.txtSubject.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtSubject.BackgroundImage")));
			this.txtSubject.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtSubject.Dock")));
			this.txtSubject.Enabled = ((bool)(resources.GetObject("txtSubject.Enabled")));
			this.txtSubject.Font = ((System.Drawing.Font)(resources.GetObject("txtSubject.Font")));
			this.txtSubject.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtSubject.ImeMode")));
			this.txtSubject.Location = ((System.Drawing.Point)(resources.GetObject("txtSubject.Location")));
			this.txtSubject.MaxLength = ((int)(resources.GetObject("txtSubject.MaxLength")));
			this.txtSubject.Multiline = ((bool)(resources.GetObject("txtSubject.Multiline")));
			this.txtSubject.Name = "txtSubject";
			this.txtSubject.PasswordChar = ((char)(resources.GetObject("txtSubject.PasswordChar")));
			this.txtSubject.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtSubject.RightToLeft")));
			this.txtSubject.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtSubject.ScrollBars")));
			this.txtSubject.Size = ((System.Drawing.Size)(resources.GetObject("txtSubject.Size")));
			this.txtSubject.TabIndex = ((int)(resources.GetObject("txtSubject.TabIndex")));
			this.txtSubject.Text = resources.GetString("txtSubject.Text");
			this.txtSubject.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtSubject.TextAlign")));
			this.txtSubject.Visible = ((bool)(resources.GetObject("txtSubject.Visible")));
			this.txtSubject.WordWrap = ((bool)(resources.GetObject("txtSubject.WordWrap")));
			// 
			// txtRecipient
			// 
			this.txtRecipient.AccessibleDescription = ((string)(resources.GetObject("txtRecipient.AccessibleDescription")));
			this.txtRecipient.AccessibleName = ((string)(resources.GetObject("txtRecipient.AccessibleName")));
			this.txtRecipient.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtRecipient.Anchor")));
			this.txtRecipient.AutoSize = ((bool)(resources.GetObject("txtRecipient.AutoSize")));
			this.txtRecipient.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtRecipient.BackgroundImage")));
			this.txtRecipient.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtRecipient.Dock")));
			this.txtRecipient.Enabled = ((bool)(resources.GetObject("txtRecipient.Enabled")));
			this.txtRecipient.Font = ((System.Drawing.Font)(resources.GetObject("txtRecipient.Font")));
			this.txtRecipient.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtRecipient.ImeMode")));
			this.txtRecipient.Location = ((System.Drawing.Point)(resources.GetObject("txtRecipient.Location")));
			this.txtRecipient.MaxLength = ((int)(resources.GetObject("txtRecipient.MaxLength")));
			this.txtRecipient.Multiline = ((bool)(resources.GetObject("txtRecipient.Multiline")));
			this.txtRecipient.Name = "txtRecipient";
			this.txtRecipient.PasswordChar = ((char)(resources.GetObject("txtRecipient.PasswordChar")));
			this.txtRecipient.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtRecipient.RightToLeft")));
			this.txtRecipient.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtRecipient.ScrollBars")));
			this.txtRecipient.Size = ((System.Drawing.Size)(resources.GetObject("txtRecipient.Size")));
			this.txtRecipient.TabIndex = ((int)(resources.GetObject("txtRecipient.TabIndex")));
			this.txtRecipient.Text = resources.GetString("txtRecipient.Text");
			this.txtRecipient.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtRecipient.TextAlign")));
			this.txtRecipient.Visible = ((bool)(resources.GetObject("txtRecipient.Visible")));
			this.txtRecipient.WordWrap = ((bool)(resources.GetObject("txtRecipient.WordWrap")));
			// 
			// txtBody
			// 
			this.txtBody.AccessibleDescription = ((string)(resources.GetObject("txtBody.AccessibleDescription")));
			this.txtBody.AccessibleName = ((string)(resources.GetObject("txtBody.AccessibleName")));
			this.txtBody.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtBody.Anchor")));
			this.txtBody.AutoSize = ((bool)(resources.GetObject("txtBody.AutoSize")));
			this.txtBody.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtBody.BackgroundImage")));
			this.txtBody.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtBody.Dock")));
			this.txtBody.Enabled = ((bool)(resources.GetObject("txtBody.Enabled")));
			this.txtBody.Font = ((System.Drawing.Font)(resources.GetObject("txtBody.Font")));
			this.txtBody.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtBody.ImeMode")));
			this.txtBody.Location = ((System.Drawing.Point)(resources.GetObject("txtBody.Location")));
			this.txtBody.MaxLength = ((int)(resources.GetObject("txtBody.MaxLength")));
			this.txtBody.Multiline = ((bool)(resources.GetObject("txtBody.Multiline")));
			this.txtBody.Name = "txtBody";
			this.txtBody.PasswordChar = ((char)(resources.GetObject("txtBody.PasswordChar")));
			this.txtBody.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtBody.RightToLeft")));
			this.txtBody.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtBody.ScrollBars")));
			this.txtBody.Size = ((System.Drawing.Size)(resources.GetObject("txtBody.Size")));
			this.txtBody.TabIndex = ((int)(resources.GetObject("txtBody.TabIndex")));
			this.txtBody.Text = resources.GetString("txtBody.Text");
			this.txtBody.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtBody.TextAlign")));
			this.txtBody.Visible = ((bool)(resources.GetObject("txtBody.Visible")));
			this.txtBody.WordWrap = ((bool)(resources.GetObject("txtBody.WordWrap")));
			// 
			// label1
			// 
			this.label1.AccessibleDescription = ((string)(resources.GetObject("label1.AccessibleDescription")));
			this.label1.AccessibleName = ((string)(resources.GetObject("label1.AccessibleName")));
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label1.Anchor")));
			this.label1.AutoSize = ((bool)(resources.GetObject("label1.AutoSize")));
			this.label1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label1.Dock")));
			this.label1.Enabled = ((bool)(resources.GetObject("label1.Enabled")));
			this.label1.Font = ((System.Drawing.Font)(resources.GetObject("label1.Font")));
			this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
			this.label1.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label1.ImageAlign")));
			this.label1.ImageIndex = ((int)(resources.GetObject("label1.ImageIndex")));
			this.label1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label1.ImeMode")));
			this.label1.Location = ((System.Drawing.Point)(resources.GetObject("label1.Location")));
			this.label1.Name = "label1";
			this.label1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label1.RightToLeft")));
			this.label1.Size = ((System.Drawing.Size)(resources.GetObject("label1.Size")));
			this.label1.TabIndex = ((int)(resources.GetObject("label1.TabIndex")));
			this.label1.Text = resources.GetString("label1.Text");
			this.label1.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label1.TextAlign")));
			this.label1.Visible = ((bool)(resources.GetObject("label1.Visible")));
			// 
			// label2
			// 
			this.label2.AccessibleDescription = ((string)(resources.GetObject("label2.AccessibleDescription")));
			this.label2.AccessibleName = ((string)(resources.GetObject("label2.AccessibleName")));
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label2.Anchor")));
			this.label2.AutoSize = ((bool)(resources.GetObject("label2.AutoSize")));
			this.label2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label2.Dock")));
			this.label2.Enabled = ((bool)(resources.GetObject("label2.Enabled")));
			this.label2.Font = ((System.Drawing.Font)(resources.GetObject("label2.Font")));
			this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
			this.label2.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label2.ImageAlign")));
			this.label2.ImageIndex = ((int)(resources.GetObject("label2.ImageIndex")));
			this.label2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label2.ImeMode")));
			this.label2.Location = ((System.Drawing.Point)(resources.GetObject("label2.Location")));
			this.label2.Name = "label2";
			this.label2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label2.RightToLeft")));
			this.label2.Size = ((System.Drawing.Size)(resources.GetObject("label2.Size")));
			this.label2.TabIndex = ((int)(resources.GetObject("label2.TabIndex")));
			this.label2.Text = resources.GetString("label2.Text");
			this.label2.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label2.TextAlign")));
			this.label2.Visible = ((bool)(resources.GetObject("label2.Visible")));
			// 
			// label3
			// 
			this.label3.AccessibleDescription = ((string)(resources.GetObject("label3.AccessibleDescription")));
			this.label3.AccessibleName = ((string)(resources.GetObject("label3.AccessibleName")));
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label3.Anchor")));
			this.label3.AutoSize = ((bool)(resources.GetObject("label3.AutoSize")));
			this.label3.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label3.Dock")));
			this.label3.Enabled = ((bool)(resources.GetObject("label3.Enabled")));
			this.label3.Font = ((System.Drawing.Font)(resources.GetObject("label3.Font")));
			this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
			this.label3.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label3.ImageAlign")));
			this.label3.ImageIndex = ((int)(resources.GetObject("label3.ImageIndex")));
			this.label3.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label3.ImeMode")));
			this.label3.Location = ((System.Drawing.Point)(resources.GetObject("label3.Location")));
			this.label3.Name = "label3";
			this.label3.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label3.RightToLeft")));
			this.label3.Size = ((System.Drawing.Size)(resources.GetObject("label3.Size")));
			this.label3.TabIndex = ((int)(resources.GetObject("label3.TabIndex")));
			this.label3.Text = resources.GetString("label3.Text");
			this.label3.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label3.TextAlign")));
			this.label3.Visible = ((bool)(resources.GetObject("label3.Visible")));
			// 
			// label4
			// 
			this.label4.AccessibleDescription = ((string)(resources.GetObject("label4.AccessibleDescription")));
			this.label4.AccessibleName = ((string)(resources.GetObject("label4.AccessibleName")));
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label4.Anchor")));
			this.label4.AutoSize = ((bool)(resources.GetObject("label4.AutoSize")));
			this.label4.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label4.Dock")));
			this.label4.Enabled = ((bool)(resources.GetObject("label4.Enabled")));
			this.label4.Font = ((System.Drawing.Font)(resources.GetObject("label4.Font")));
			this.label4.Image = ((System.Drawing.Image)(resources.GetObject("label4.Image")));
			this.label4.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label4.ImageAlign")));
			this.label4.ImageIndex = ((int)(resources.GetObject("label4.ImageIndex")));
			this.label4.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label4.ImeMode")));
			this.label4.Location = ((System.Drawing.Point)(resources.GetObject("label4.Location")));
			this.label4.Name = "label4";
			this.label4.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label4.RightToLeft")));
			this.label4.Size = ((System.Drawing.Size)(resources.GetObject("label4.Size")));
			this.label4.TabIndex = ((int)(resources.GetObject("label4.TabIndex")));
			this.label4.Text = resources.GetString("label4.Text");
			this.label4.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label4.TextAlign")));
			this.label4.Visible = ((bool)(resources.GetObject("label4.Visible")));
			// 
			// btnSend
			// 
			this.btnSend.AccessibleDescription = ((string)(resources.GetObject("btnSend.AccessibleDescription")));
			this.btnSend.AccessibleName = ((string)(resources.GetObject("btnSend.AccessibleName")));
			this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnSend.Anchor")));
			this.btnSend.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSend.BackgroundImage")));
			this.btnSend.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnSend.Dock")));
			this.btnSend.Enabled = ((bool)(resources.GetObject("btnSend.Enabled")));
			this.btnSend.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnSend.FlatStyle")));
			this.btnSend.Font = ((System.Drawing.Font)(resources.GetObject("btnSend.Font")));
			this.btnSend.Image = ((System.Drawing.Image)(resources.GetObject("btnSend.Image")));
			this.btnSend.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnSend.ImageAlign")));
			this.btnSend.ImageIndex = ((int)(resources.GetObject("btnSend.ImageIndex")));
			this.btnSend.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnSend.ImeMode")));
			this.btnSend.Location = ((System.Drawing.Point)(resources.GetObject("btnSend.Location")));
			this.btnSend.Name = "btnSend";
			this.btnSend.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnSend.RightToLeft")));
			this.btnSend.Size = ((System.Drawing.Size)(resources.GetObject("btnSend.Size")));
			this.btnSend.TabIndex = ((int)(resources.GetObject("btnSend.TabIndex")));
			this.btnSend.Text = resources.GetString("btnSend.Text");
			this.btnSend.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnSend.TextAlign")));
			this.btnSend.Visible = ((bool)(resources.GetObject("btnSend.Visible")));
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			// 
			// label5
			// 
			this.label5.AccessibleDescription = ((string)(resources.GetObject("label5.AccessibleDescription")));
			this.label5.AccessibleName = ((string)(resources.GetObject("label5.AccessibleName")));
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label5.Anchor")));
			this.label5.AutoSize = ((bool)(resources.GetObject("label5.AutoSize")));
			this.label5.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label5.Dock")));
			this.label5.Enabled = ((bool)(resources.GetObject("label5.Enabled")));
			this.label5.Font = ((System.Drawing.Font)(resources.GetObject("label5.Font")));
			this.label5.Image = ((System.Drawing.Image)(resources.GetObject("label5.Image")));
			this.label5.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label5.ImageAlign")));
			this.label5.ImageIndex = ((int)(resources.GetObject("label5.ImageIndex")));
			this.label5.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label5.ImeMode")));
			this.label5.Location = ((System.Drawing.Point)(resources.GetObject("label5.Location")));
			this.label5.Name = "label5";
			this.label5.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label5.RightToLeft")));
			this.label5.Size = ((System.Drawing.Size)(resources.GetObject("label5.Size")));
			this.label5.TabIndex = ((int)(resources.GetObject("label5.TabIndex")));
			this.label5.Text = resources.GetString("label5.Text");
			this.label5.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label5.TextAlign")));
			this.label5.Visible = ((bool)(resources.GetObject("label5.Visible")));
			// 
			// chkIM
			// 
			this.chkIM.AccessibleDescription = ((string)(resources.GetObject("chkIM.AccessibleDescription")));
			this.chkIM.AccessibleName = ((string)(resources.GetObject("chkIM.AccessibleName")));
			this.chkIM.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkIM.Anchor")));
			this.chkIM.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkIM.Appearance")));
			this.chkIM.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkIM.BackgroundImage")));
			this.chkIM.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkIM.CheckAlign")));
			this.chkIM.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkIM.Dock")));
			this.chkIM.Enabled = ((bool)(resources.GetObject("chkIM.Enabled")));
			this.chkIM.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkIM.FlatStyle")));
			this.chkIM.Font = ((System.Drawing.Font)(resources.GetObject("chkIM.Font")));
			this.chkIM.Image = ((System.Drawing.Image)(resources.GetObject("chkIM.Image")));
			this.chkIM.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkIM.ImageAlign")));
			this.chkIM.ImageIndex = ((int)(resources.GetObject("chkIM.ImageIndex")));
			this.chkIM.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkIM.ImeMode")));
			this.chkIM.Location = ((System.Drawing.Point)(resources.GetObject("chkIM.Location")));
			this.chkIM.Name = "chkIM";
			this.chkIM.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkIM.RightToLeft")));
			this.chkIM.Size = ((System.Drawing.Size)(resources.GetObject("chkIM.Size")));
			this.chkIM.TabIndex = ((int)(resources.GetObject("chkIM.TabIndex")));
			this.chkIM.Text = resources.GetString("chkIM.Text");
			this.chkIM.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkIM.TextAlign")));
			this.chkIM.Visible = ((bool)(resources.GetObject("chkIM.Visible")));
			// 
			// label6
			// 
			this.label6.AccessibleDescription = ((string)(resources.GetObject("label6.AccessibleDescription")));
			this.label6.AccessibleName = ((string)(resources.GetObject("label6.AccessibleName")));
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label6.Anchor")));
			this.label6.AutoSize = ((bool)(resources.GetObject("label6.AutoSize")));
			this.label6.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label6.Dock")));
			this.label6.Enabled = ((bool)(resources.GetObject("label6.Enabled")));
			this.label6.Font = ((System.Drawing.Font)(resources.GetObject("label6.Font")));
			this.label6.Image = ((System.Drawing.Image)(resources.GetObject("label6.Image")));
			this.label6.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label6.ImageAlign")));
			this.label6.ImageIndex = ((int)(resources.GetObject("label6.ImageIndex")));
			this.label6.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label6.ImeMode")));
			this.label6.Location = ((System.Drawing.Point)(resources.GetObject("label6.Location")));
			this.label6.Name = "label6";
			this.label6.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label6.RightToLeft")));
			this.label6.Size = ((System.Drawing.Size)(resources.GetObject("label6.Size")));
			this.label6.TabIndex = ((int)(resources.GetObject("label6.TabIndex")));
			this.label6.Text = resources.GetString("label6.Text");
			this.label6.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label6.TextAlign")));
			this.label6.Visible = ((bool)(resources.GetObject("label6.Visible")));
			// 
			// chkDelIMUnD
			// 
			this.chkDelIMUnD.AccessibleDescription = ((string)(resources.GetObject("chkDelIMUnD.AccessibleDescription")));
			this.chkDelIMUnD.AccessibleName = ((string)(resources.GetObject("chkDelIMUnD.AccessibleName")));
			this.chkDelIMUnD.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkDelIMUnD.Anchor")));
			this.chkDelIMUnD.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkDelIMUnD.Appearance")));
			this.chkDelIMUnD.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkDelIMUnD.BackgroundImage")));
			this.chkDelIMUnD.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkDelIMUnD.CheckAlign")));
			this.chkDelIMUnD.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkDelIMUnD.Dock")));
			this.chkDelIMUnD.Enabled = ((bool)(resources.GetObject("chkDelIMUnD.Enabled")));
			this.chkDelIMUnD.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkDelIMUnD.FlatStyle")));
			this.chkDelIMUnD.Font = ((System.Drawing.Font)(resources.GetObject("chkDelIMUnD.Font")));
			this.chkDelIMUnD.Image = ((System.Drawing.Image)(resources.GetObject("chkDelIMUnD.Image")));
			this.chkDelIMUnD.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkDelIMUnD.ImageAlign")));
			this.chkDelIMUnD.ImageIndex = ((int)(resources.GetObject("chkDelIMUnD.ImageIndex")));
			this.chkDelIMUnD.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkDelIMUnD.ImeMode")));
			this.chkDelIMUnD.Location = ((System.Drawing.Point)(resources.GetObject("chkDelIMUnD.Location")));
			this.chkDelIMUnD.Name = "chkDelIMUnD";
			this.chkDelIMUnD.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkDelIMUnD.RightToLeft")));
			this.chkDelIMUnD.Size = ((System.Drawing.Size)(resources.GetObject("chkDelIMUnD.Size")));
			this.chkDelIMUnD.TabIndex = ((int)(resources.GetObject("chkDelIMUnD.TabIndex")));
			this.chkDelIMUnD.Text = resources.GetString("chkDelIMUnD.Text");
			this.chkDelIMUnD.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkDelIMUnD.TextAlign")));
			this.chkDelIMUnD.Visible = ((bool)(resources.GetObject("chkDelIMUnD.Visible")));
			// 
			// label7
			// 
			this.label7.AccessibleDescription = ((string)(resources.GetObject("label7.AccessibleDescription")));
			this.label7.AccessibleName = ((string)(resources.GetObject("label7.AccessibleName")));
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label7.Anchor")));
			this.label7.AutoSize = ((bool)(resources.GetObject("label7.AutoSize")));
			this.label7.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label7.Dock")));
			this.label7.Enabled = ((bool)(resources.GetObject("label7.Enabled")));
			this.label7.Font = ((System.Drawing.Font)(resources.GetObject("label7.Font")));
			this.label7.Image = ((System.Drawing.Image)(resources.GetObject("label7.Image")));
			this.label7.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label7.ImageAlign")));
			this.label7.ImageIndex = ((int)(resources.GetObject("label7.ImageIndex")));
			this.label7.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label7.ImeMode")));
			this.label7.Location = ((System.Drawing.Point)(resources.GetObject("label7.Location")));
			this.label7.Name = "label7";
			this.label7.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label7.RightToLeft")));
			this.label7.Size = ((System.Drawing.Size)(resources.GetObject("label7.Size")));
			this.label7.TabIndex = ((int)(resources.GetObject("label7.TabIndex")));
			this.label7.Text = resources.GetString("label7.Text");
			this.label7.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label7.TextAlign")));
			this.label7.Visible = ((bool)(resources.GetObject("label7.Visible")));
			// 
			// cmbForms
			// 
			this.cmbForms.AccessibleDescription = ((string)(resources.GetObject("cmbForms.AccessibleDescription")));
			this.cmbForms.AccessibleName = ((string)(resources.GetObject("cmbForms.AccessibleName")));
			this.cmbForms.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("cmbForms.Anchor")));
			this.cmbForms.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cmbForms.BackgroundImage")));
			this.cmbForms.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("cmbForms.Dock")));
			this.cmbForms.Enabled = ((bool)(resources.GetObject("cmbForms.Enabled")));
			this.cmbForms.Font = ((System.Drawing.Font)(resources.GetObject("cmbForms.Font")));
			this.cmbForms.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("cmbForms.ImeMode")));
			this.cmbForms.IntegralHeight = ((bool)(resources.GetObject("cmbForms.IntegralHeight")));
			this.cmbForms.ItemHeight = ((int)(resources.GetObject("cmbForms.ItemHeight")));
			this.cmbForms.Location = ((System.Drawing.Point)(resources.GetObject("cmbForms.Location")));
			this.cmbForms.MaxDropDownItems = ((int)(resources.GetObject("cmbForms.MaxDropDownItems")));
			this.cmbForms.MaxLength = ((int)(resources.GetObject("cmbForms.MaxLength")));
			this.cmbForms.Name = "cmbForms";
			this.cmbForms.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("cmbForms.RightToLeft")));
			this.cmbForms.Size = ((System.Drawing.Size)(resources.GetObject("cmbForms.Size")));
			this.cmbForms.Sorted = true;
			this.cmbForms.TabIndex = ((int)(resources.GetObject("cmbForms.TabIndex")));
			this.cmbForms.Text = resources.GetString("cmbForms.Text");
			this.cmbForms.Visible = ((bool)(resources.GetObject("cmbForms.Visible")));
			// 
			// btnAddForm
			// 
			this.btnAddForm.AccessibleDescription = ((string)(resources.GetObject("btnAddForm.AccessibleDescription")));
			this.btnAddForm.AccessibleName = ((string)(resources.GetObject("btnAddForm.AccessibleName")));
			this.btnAddForm.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnAddForm.Anchor")));
			this.btnAddForm.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAddForm.BackgroundImage")));
			this.btnAddForm.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnAddForm.Dock")));
			this.btnAddForm.Enabled = ((bool)(resources.GetObject("btnAddForm.Enabled")));
			this.btnAddForm.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnAddForm.FlatStyle")));
			this.btnAddForm.Font = ((System.Drawing.Font)(resources.GetObject("btnAddForm.Font")));
			this.btnAddForm.Image = ((System.Drawing.Image)(resources.GetObject("btnAddForm.Image")));
			this.btnAddForm.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnAddForm.ImageAlign")));
			this.btnAddForm.ImageIndex = ((int)(resources.GetObject("btnAddForm.ImageIndex")));
			this.btnAddForm.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnAddForm.ImeMode")));
			this.btnAddForm.Location = ((System.Drawing.Point)(resources.GetObject("btnAddForm.Location")));
			this.btnAddForm.Name = "btnAddForm";
			this.btnAddForm.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnAddForm.RightToLeft")));
			this.btnAddForm.Size = ((System.Drawing.Size)(resources.GetObject("btnAddForm.Size")));
			this.btnAddForm.TabIndex = ((int)(resources.GetObject("btnAddForm.TabIndex")));
			this.btnAddForm.Text = resources.GetString("btnAddForm.Text");
			this.btnAddForm.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnAddForm.TextAlign")));
			this.btnAddForm.Visible = ((bool)(resources.GetObject("btnAddForm.Visible")));
			this.btnAddForm.Click += new System.EventHandler(this.btnAddForm_Click);
			// 
			// label8
			// 
			this.label8.AccessibleDescription = ((string)(resources.GetObject("label8.AccessibleDescription")));
			this.label8.AccessibleName = ((string)(resources.GetObject("label8.AccessibleName")));
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label8.Anchor")));
			this.label8.AutoSize = ((bool)(resources.GetObject("label8.AutoSize")));
			this.label8.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label8.Dock")));
			this.label8.Enabled = ((bool)(resources.GetObject("label8.Enabled")));
			this.label8.Font = ((System.Drawing.Font)(resources.GetObject("label8.Font")));
			this.label8.Image = ((System.Drawing.Image)(resources.GetObject("label8.Image")));
			this.label8.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label8.ImageAlign")));
			this.label8.ImageIndex = ((int)(resources.GetObject("label8.ImageIndex")));
			this.label8.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label8.ImeMode")));
			this.label8.Location = ((System.Drawing.Point)(resources.GetObject("label8.Location")));
			this.label8.Name = "label8";
			this.label8.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label8.RightToLeft")));
			this.label8.Size = ((System.Drawing.Size)(resources.GetObject("label8.Size")));
			this.label8.TabIndex = ((int)(resources.GetObject("label8.TabIndex")));
			this.label8.Text = resources.GetString("label8.Text");
			this.label8.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label8.TextAlign")));
			this.label8.Visible = ((bool)(resources.GetObject("label8.Visible")));
			// 
			// lblFormAttached
			// 
			this.lblFormAttached.AccessibleDescription = ((string)(resources.GetObject("lblFormAttached.AccessibleDescription")));
			this.lblFormAttached.AccessibleName = ((string)(resources.GetObject("lblFormAttached.AccessibleName")));
			this.lblFormAttached.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblFormAttached.Anchor")));
			this.lblFormAttached.AutoSize = ((bool)(resources.GetObject("lblFormAttached.AutoSize")));
			this.lblFormAttached.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblFormAttached.Dock")));
			this.lblFormAttached.Enabled = ((bool)(resources.GetObject("lblFormAttached.Enabled")));
			this.lblFormAttached.Font = ((System.Drawing.Font)(resources.GetObject("lblFormAttached.Font")));
			this.lblFormAttached.ForeColor = System.Drawing.Color.Yellow;
			this.lblFormAttached.Image = ((System.Drawing.Image)(resources.GetObject("lblFormAttached.Image")));
			this.lblFormAttached.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblFormAttached.ImageAlign")));
			this.lblFormAttached.ImageIndex = ((int)(resources.GetObject("lblFormAttached.ImageIndex")));
			this.lblFormAttached.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblFormAttached.ImeMode")));
			this.lblFormAttached.Location = ((System.Drawing.Point)(resources.GetObject("lblFormAttached.Location")));
			this.lblFormAttached.Name = "lblFormAttached";
			this.lblFormAttached.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblFormAttached.RightToLeft")));
			this.lblFormAttached.Size = ((System.Drawing.Size)(resources.GetObject("lblFormAttached.Size")));
			this.lblFormAttached.TabIndex = ((int)(resources.GetObject("lblFormAttached.TabIndex")));
			this.lblFormAttached.Text = resources.GetString("lblFormAttached.Text");
			this.lblFormAttached.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblFormAttached.TextAlign")));
			this.lblFormAttached.Visible = ((bool)(resources.GetObject("lblFormAttached.Visible")));
			// 
			// btnView
			// 
			this.btnView.AccessibleDescription = ((string)(resources.GetObject("btnView.AccessibleDescription")));
			this.btnView.AccessibleName = ((string)(resources.GetObject("btnView.AccessibleName")));
			this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnView.Anchor")));
			this.btnView.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnView.BackgroundImage")));
			this.btnView.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnView.Dock")));
			this.btnView.Enabled = ((bool)(resources.GetObject("btnView.Enabled")));
			this.btnView.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnView.FlatStyle")));
			this.btnView.Font = ((System.Drawing.Font)(resources.GetObject("btnView.Font")));
			this.btnView.Image = ((System.Drawing.Image)(resources.GetObject("btnView.Image")));
			this.btnView.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnView.ImageAlign")));
			this.btnView.ImageIndex = ((int)(resources.GetObject("btnView.ImageIndex")));
			this.btnView.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnView.ImeMode")));
			this.btnView.Location = ((System.Drawing.Point)(resources.GetObject("btnView.Location")));
			this.btnView.Name = "btnView";
			this.btnView.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnView.RightToLeft")));
			this.btnView.Size = ((System.Drawing.Size)(resources.GetObject("btnView.Size")));
			this.btnView.TabIndex = ((int)(resources.GetObject("btnView.TabIndex")));
			this.btnView.Text = resources.GetString("btnView.Text");
			this.btnView.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnView.TextAlign")));
			this.btnView.Visible = ((bool)(resources.GetObject("btnView.Visible")));
			this.btnView.Click += new System.EventHandler(this.btnView_Click);
			// 
			// statusBar1
			// 
			this.statusBar1.AccessibleDescription = ((string)(resources.GetObject("statusBar1.AccessibleDescription")));
			this.statusBar1.AccessibleName = ((string)(resources.GetObject("statusBar1.AccessibleName")));
			this.statusBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("statusBar1.Anchor")));
			this.statusBar1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("statusBar1.BackgroundImage")));
			this.statusBar1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("statusBar1.Dock")));
			this.statusBar1.Enabled = ((bool)(resources.GetObject("statusBar1.Enabled")));
			this.statusBar1.Font = ((System.Drawing.Font)(resources.GetObject("statusBar1.Font")));
			this.statusBar1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("statusBar1.ImeMode")));
			this.statusBar1.Location = ((System.Drawing.Point)(resources.GetObject("statusBar1.Location")));
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("statusBar1.RightToLeft")));
			this.statusBar1.Size = ((System.Drawing.Size)(resources.GetObject("statusBar1.Size")));
			this.statusBar1.TabIndex = ((int)(resources.GetObject("statusBar1.TabIndex")));
			this.statusBar1.Text = resources.GetString("statusBar1.Text");
			this.statusBar1.Visible = ((bool)(resources.GetObject("statusBar1.Visible")));
			// 
			// frmSend
			// 
			this.AccessibleDescription = ((string)(resources.GetObject("$this.AccessibleDescription")));
			this.AccessibleName = ((string)(resources.GetObject("$this.AccessibleName")));
			this.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("$this.Anchor")));
			this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.statusBar1,
																		  this.btnView,
																		  this.lblFormAttached,
																		  this.label8,
																		  this.btnAddForm,
																		  this.cmbForms,
																		  this.label7,
																		  this.chkDelIMUnD,
																		  this.label6,
																		  this.chkIM,
																		  this.label5,
																		  this.btnSend,
																		  this.label4,
																		  this.label3,
																		  this.label2,
																		  this.label1,
																		  this.txtBody,
																		  this.txtRecipient,
																		  this.txtSubject,
																		  this.txtSender});
			this.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("$this.Dock")));
			this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
			this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
			this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
			this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
			this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
			this.Name = "frmSend";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
			this.Text = resources.GetString("$this.Text");
			this.Visible = ((bool)(resources.GetObject("$this.Visible")));
			this.Load += new System.EventHandler(this.frmSend_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		/// 
		private WSEmailMessage theMessage = null;
		public void reply(WSEmailMessage w) 
		{
			theMessage = w;
			UpdateFormStatus();
		}			
		private void frmSend_Load(object sender, System.EventArgs e)
		{
			if (theMessage != null ) 
			{
				txtSender.Text = theMessage.Recipient;
				if (theMessage.BusinessRequest == null)
					txtRecipient.Text = theMessage.Sender;
				txtSubject.Text = "Re: " + theMessage.Subject;
				txtBody.Text = "\r\n"+"In reply to the following, written " + theMessage.Timestamp+"--\r\n\r\n"+theMessage.Body;
				txtBody.Focus();
			}
			else
			{
				txtSender.Text = PennRoutingUtilities.GetCertEmail(PennRoutingUtilities.GetSecurityToken(PennRoutingUtilities.MyIdentifer,false).Certificate);
				txtRecipient.Text = "Kevin@MailServerA";
				txtSubject.Text = "Testing...";
				txtBody.Text = "This is a test message!";
			}
		}

		private void SendingCallback(IAsyncResult r) 
		{
			WSEmailStatus s = m.EndWSEmailSend(r);
			PennRoutingUtilities.LogToStatusWindow(s.Message);
			statusBar1.Text = "Reply from server: " + s.ToString();
		}

		private void btnSend_Click(object sender, System.EventArgs e)
		{

			WSEmailMessage wem = new WSEmailMessage();
			wem.Sender = txtSender.Text;
			wem.Recipient = txtRecipient.Text;
			wem.Subject = txtSubject.Text;
			wem.Body = txtBody.Text;
			wem.Timestamp = PennRoutingFilters.PennRoutingUtilities.GetCurrentTime();
			if (chkIM.Checked)
				wem.MessageFlags |= WSEmailFlags.InstantMessaging.SendAsInstantMessage;
			if (chkDelIMUnD.Checked)
				wem.MessageFlags |= WSEmailFlags.InstantMessaging.DeleteIfNotDeliverable;
			if (theMessage != null && theMessage.BusinessRequest != null) {
				wem.BusinessRequest = theMessage.BusinessRequest;
				wem.MessageFlags |= WSEmailFlags.Contains.Form;
			}

			// frmWait f = new frmWait("Sending your message.");
			

			AsyncCallback callback = new AsyncCallback(SendingCallback);
			sendMessageDelegate AsyncDelegate = new sendMessageDelegate(m.WSEmailSend);

			PennRoutingUtilities.LogToStatusWindow("Beginning to send message entitled '"+wem.Subject+"' to " + wem.Recipient+".");
			statusBar1.Text = "Sending message...";
			m.BeginWSEmailSend(wem,null,callback,null);
			//MessageBox.Show("Message sent asynchronously...");
			//MessageBox.Show("Results:\n\n" + m.WSEmailSend(wem,null),"Message sending results...");
			// PennRoutingFilters.PennRoutingUtilities.RemoveWSEMailSignatureFilters();
			//f.Dispose();

		}

		private void UpdateFormStatus() 
		{
			if (theMessage.BusinessRequest != null) 
			{
				lblFormAttached.Text = "Yes";
				lblFormAttached.ForeColor = Color.Green;
				btnView.Enabled = true;
				txtRecipient.Text = theMessage.BusinessRequest.GetNextHop;
				txtRecipient.Enabled = false;
			} 
			else 
			{
				lblFormAttached.Text = "No";
				lblFormAttached.ForeColor = Color.Yellow;
				btnView.Enabled = false;
			}
		}

		private void FormDone() 
		{
			try 
			{
				theMessage.BusinessRequest = frmForm.GetBusinessRequest();
				frmForm.ThrowOut();
			} 
			catch {}
			UpdateFormStatus();
			frmForm = null;
		}

		private void btnAddForm_Click(object sender, System.EventArgs e)
		{
			if (theMessage == null)
				theMessage = new WSEmailMessage();

			if (theMessage.BusinessRequest != null)
				if (MessageBox.Show("Do you want to destroy the current form?" , "Question...",MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
					return;
			frmForm = BusinessObjects.AvailableForms.LoadForm((string)cmbForms.SelectedItem);
			frmForm.UserDone += new BusinessObjectsDelegates.NullDelegate(FormDone);
			
			frmForm.LoadBusinessRequestAndShow(null);
		}

		private void btnView_Click(object sender, System.EventArgs e)
		{
			if (theMessage.BusinessRequest == null) 
				return;

			frmForm = BusinessObjects.AvailableForms.LoadForm(theMessage.BusinessRequest.BusinessObject.FormName);
			frmForm.UserDone += new BusinessObjectsDelegates.NullDelegate(FormDone);
			frmForm.LoadBusinessRequestAndShow(theMessage.BusinessRequest);
		}
	}
}
