/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Threading;
using DNSResolver;
using DynamicForms;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using WSEmailProxy;
using System.Xml;
using PennLibraries;
using Microsoft.Web.Services2.Security;
using Microsoft.Web.Services2.Security.Tokens;
using Microsoft.Web.Services2.Dime;
using XmlAddressBook;

namespace WSEmailClientv2
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class FrmSend : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox txtSubject;
		private System.Windows.Forms.TextBox txtBody;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnSend;
		private System.Windows.Forms.CheckBox chkIM;
		private System.Windows.Forms.CheckBox chkDelIMUnD;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox cmbForms;
		private System.Windows.Forms.Button btnAddForm;
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button btnView;
		private System.Windows.Forms.StatusBar statusBar1;
	
		public delegate WSEmailStatus sendMessageDelegate(WSEmailMessage m, XmlElement sig);
		private System.Windows.Forms.ListBox lstFormsAttached;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem mnuFrmAddDLL;
		private System.Windows.Forms.MenuItem mnuFrmAddWeb;
		private System.Windows.Forms.Label stamp;
		private System.Windows.Forms.CheckBox chkSendDir;
		private System.Windows.Forms.CheckBox chkEncrypt;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnAddrBook;
		private System.Windows.Forms.TextBox txtRecipient1;
		private WSEmailClientv2.AddressBookControl addressBookControl;
		private BaseObject BO = null;
		public FrmSend()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			stamp.Image = Global.UnusedStamp;
			this.cmbForms.DataSource = ObjectLoader.GetAvailableObjects();
			addressBookControl.TheBook = AddressBook.GetInstance();
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FrmSend));
			this.txtSubject = new System.Windows.Forms.TextBox();
			this.txtRecipient1 = new System.Windows.Forms.TextBox();
			this.txtBody = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.btnSend = new System.Windows.Forms.Button();
			this.chkIM = new System.Windows.Forms.CheckBox();
			this.chkDelIMUnD = new System.Windows.Forms.CheckBox();
			this.label7 = new System.Windows.Forms.Label();
			this.cmbForms = new System.Windows.Forms.ComboBox();
			this.btnAddForm = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.btnView = new System.Windows.Forms.Button();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.lstFormsAttached = new System.Windows.Forms.ListBox();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.mnuFrmAddDLL = new System.Windows.Forms.MenuItem();
			this.mnuFrmAddWeb = new System.Windows.Forms.MenuItem();
			this.stamp = new System.Windows.Forms.Label();
			this.chkSendDir = new System.Windows.Forms.CheckBox();
			this.chkEncrypt = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnAddrBook = new System.Windows.Forms.Button();
			this.addressBookControl = new WSEmailClientv2.AddressBookControl();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtSubject
			// 
			this.txtSubject.AccessibleDescription = resources.GetString("txtSubject.AccessibleDescription");
			this.txtSubject.AccessibleName = resources.GetString("txtSubject.AccessibleName");
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
			this.txtSubject.TextChanged += new System.EventHandler(this.txtSubject_TextChanged);
			// 
			// txtRecipient1
			// 
			this.txtRecipient1.AccessibleDescription = resources.GetString("txtRecipient1.AccessibleDescription");
			this.txtRecipient1.AccessibleName = resources.GetString("txtRecipient1.AccessibleName");
			this.txtRecipient1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtRecipient1.Anchor")));
			this.txtRecipient1.AutoSize = ((bool)(resources.GetObject("txtRecipient1.AutoSize")));
			this.txtRecipient1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtRecipient1.BackgroundImage")));
			this.txtRecipient1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtRecipient1.Dock")));
			this.txtRecipient1.Enabled = ((bool)(resources.GetObject("txtRecipient1.Enabled")));
			this.txtRecipient1.Font = ((System.Drawing.Font)(resources.GetObject("txtRecipient1.Font")));
			this.txtRecipient1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtRecipient1.ImeMode")));
			this.txtRecipient1.Location = ((System.Drawing.Point)(resources.GetObject("txtRecipient1.Location")));
			this.txtRecipient1.MaxLength = ((int)(resources.GetObject("txtRecipient1.MaxLength")));
			this.txtRecipient1.Multiline = ((bool)(resources.GetObject("txtRecipient1.Multiline")));
			this.txtRecipient1.Name = "txtRecipient1";
			this.txtRecipient1.PasswordChar = ((char)(resources.GetObject("txtRecipient1.PasswordChar")));
			this.txtRecipient1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtRecipient1.RightToLeft")));
			this.txtRecipient1.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtRecipient1.ScrollBars")));
			this.txtRecipient1.Size = ((System.Drawing.Size)(resources.GetObject("txtRecipient1.Size")));
			this.txtRecipient1.TabIndex = ((int)(resources.GetObject("txtRecipient1.TabIndex")));
			this.txtRecipient1.Text = resources.GetString("txtRecipient1.Text");
			this.txtRecipient1.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtRecipient1.TextAlign")));
			this.txtRecipient1.Visible = ((bool)(resources.GetObject("txtRecipient1.Visible")));
			this.txtRecipient1.WordWrap = ((bool)(resources.GetObject("txtRecipient1.WordWrap")));
			// 
			// txtBody
			// 
			this.txtBody.AccessibleDescription = resources.GetString("txtBody.AccessibleDescription");
			this.txtBody.AccessibleName = resources.GetString("txtBody.AccessibleName");
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
			// label2
			// 
			this.label2.AccessibleDescription = resources.GetString("label2.AccessibleDescription");
			this.label2.AccessibleName = resources.GetString("label2.AccessibleName");
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
			this.label3.AccessibleDescription = resources.GetString("label3.AccessibleDescription");
			this.label3.AccessibleName = resources.GetString("label3.AccessibleName");
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
			this.label4.AccessibleDescription = resources.GetString("label4.AccessibleDescription");
			this.label4.AccessibleName = resources.GetString("label4.AccessibleName");
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
			this.btnSend.AccessibleDescription = resources.GetString("btnSend.AccessibleDescription");
			this.btnSend.AccessibleName = resources.GetString("btnSend.AccessibleName");
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
			// chkIM
			// 
			this.chkIM.AccessibleDescription = resources.GetString("chkIM.AccessibleDescription");
			this.chkIM.AccessibleName = resources.GetString("chkIM.AccessibleName");
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
			// chkDelIMUnD
			// 
			this.chkDelIMUnD.AccessibleDescription = resources.GetString("chkDelIMUnD.AccessibleDescription");
			this.chkDelIMUnD.AccessibleName = resources.GetString("chkDelIMUnD.AccessibleName");
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
			this.label7.AccessibleDescription = resources.GetString("label7.AccessibleDescription");
			this.label7.AccessibleName = resources.GetString("label7.AccessibleName");
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
			this.cmbForms.AccessibleDescription = resources.GetString("cmbForms.AccessibleDescription");
			this.cmbForms.AccessibleName = resources.GetString("cmbForms.AccessibleName");
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
			this.btnAddForm.AccessibleDescription = resources.GetString("btnAddForm.AccessibleDescription");
			this.btnAddForm.AccessibleName = resources.GetString("btnAddForm.AccessibleName");
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
			this.label8.AccessibleDescription = resources.GetString("label8.AccessibleDescription");
			this.label8.AccessibleName = resources.GetString("label8.AccessibleName");
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
			// btnView
			// 
			this.btnView.AccessibleDescription = resources.GetString("btnView.AccessibleDescription");
			this.btnView.AccessibleName = resources.GetString("btnView.AccessibleName");
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
			this.statusBar1.AccessibleDescription = resources.GetString("statusBar1.AccessibleDescription");
			this.statusBar1.AccessibleName = resources.GetString("statusBar1.AccessibleName");
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
			// lstFormsAttached
			// 
			this.lstFormsAttached.AccessibleDescription = resources.GetString("lstFormsAttached.AccessibleDescription");
			this.lstFormsAttached.AccessibleName = resources.GetString("lstFormsAttached.AccessibleName");
			this.lstFormsAttached.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lstFormsAttached.Anchor")));
			this.lstFormsAttached.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("lstFormsAttached.BackgroundImage")));
			this.lstFormsAttached.ColumnWidth = ((int)(resources.GetObject("lstFormsAttached.ColumnWidth")));
			this.lstFormsAttached.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lstFormsAttached.Dock")));
			this.lstFormsAttached.Enabled = ((bool)(resources.GetObject("lstFormsAttached.Enabled")));
			this.lstFormsAttached.Font = ((System.Drawing.Font)(resources.GetObject("lstFormsAttached.Font")));
			this.lstFormsAttached.HorizontalExtent = ((int)(resources.GetObject("lstFormsAttached.HorizontalExtent")));
			this.lstFormsAttached.HorizontalScrollbar = ((bool)(resources.GetObject("lstFormsAttached.HorizontalScrollbar")));
			this.lstFormsAttached.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lstFormsAttached.ImeMode")));
			this.lstFormsAttached.IntegralHeight = ((bool)(resources.GetObject("lstFormsAttached.IntegralHeight")));
			this.lstFormsAttached.ItemHeight = ((int)(resources.GetObject("lstFormsAttached.ItemHeight")));
			this.lstFormsAttached.Location = ((System.Drawing.Point)(resources.GetObject("lstFormsAttached.Location")));
			this.lstFormsAttached.Name = "lstFormsAttached";
			this.lstFormsAttached.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lstFormsAttached.RightToLeft")));
			this.lstFormsAttached.ScrollAlwaysVisible = ((bool)(resources.GetObject("lstFormsAttached.ScrollAlwaysVisible")));
			this.lstFormsAttached.Size = ((System.Drawing.Size)(resources.GetObject("lstFormsAttached.Size")));
			this.lstFormsAttached.TabIndex = ((int)(resources.GetObject("lstFormsAttached.TabIndex")));
			this.lstFormsAttached.Visible = ((bool)(resources.GetObject("lstFormsAttached.Visible")));
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem3});
			this.mainMenu1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mainMenu1.RightToLeft")));
			// 
			// menuItem1
			// 
			this.menuItem1.Enabled = ((bool)(resources.GetObject("menuItem1.Enabled")));
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem2});
			this.menuItem1.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
			this.menuItem1.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItem1.Shortcut")));
			this.menuItem1.ShowShortcut = ((bool)(resources.GetObject("menuItem1.ShowShortcut")));
			this.menuItem1.Text = resources.GetString("menuItem1.Text");
			this.menuItem1.Visible = ((bool)(resources.GetObject("menuItem1.Visible")));
			// 
			// menuItem2
			// 
			this.menuItem2.Enabled = ((bool)(resources.GetObject("menuItem2.Enabled")));
			this.menuItem2.Index = 0;
			this.menuItem2.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItem2.Shortcut")));
			this.menuItem2.ShowShortcut = ((bool)(resources.GetObject("menuItem2.ShowShortcut")));
			this.menuItem2.Text = resources.GetString("menuItem2.Text");
			this.menuItem2.Visible = ((bool)(resources.GetObject("menuItem2.Visible")));
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Enabled = ((bool)(resources.GetObject("menuItem3.Enabled")));
			this.menuItem3.Index = 1;
			this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem4});
			this.menuItem3.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItem3.Shortcut")));
			this.menuItem3.ShowShortcut = ((bool)(resources.GetObject("menuItem3.ShowShortcut")));
			this.menuItem3.Text = resources.GetString("menuItem3.Text");
			this.menuItem3.Visible = ((bool)(resources.GetObject("menuItem3.Visible")));
			// 
			// menuItem4
			// 
			this.menuItem4.Enabled = ((bool)(resources.GetObject("menuItem4.Enabled")));
			this.menuItem4.Index = 0;
			this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuFrmAddDLL,
																					  this.mnuFrmAddWeb});
			this.menuItem4.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItem4.Shortcut")));
			this.menuItem4.ShowShortcut = ((bool)(resources.GetObject("menuItem4.ShowShortcut")));
			this.menuItem4.Text = resources.GetString("menuItem4.Text");
			this.menuItem4.Visible = ((bool)(resources.GetObject("menuItem4.Visible")));
			// 
			// mnuFrmAddDLL
			// 
			this.mnuFrmAddDLL.Enabled = ((bool)(resources.GetObject("mnuFrmAddDLL.Enabled")));
			this.mnuFrmAddDLL.Index = 0;
			this.mnuFrmAddDLL.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuFrmAddDLL.Shortcut")));
			this.mnuFrmAddDLL.ShowShortcut = ((bool)(resources.GetObject("mnuFrmAddDLL.ShowShortcut")));
			this.mnuFrmAddDLL.Text = resources.GetString("mnuFrmAddDLL.Text");
			this.mnuFrmAddDLL.Visible = ((bool)(resources.GetObject("mnuFrmAddDLL.Visible")));
			this.mnuFrmAddDLL.Click += new System.EventHandler(this.mnuFrmAddDLL_Click);
			// 
			// mnuFrmAddWeb
			// 
			this.mnuFrmAddWeb.Enabled = ((bool)(resources.GetObject("mnuFrmAddWeb.Enabled")));
			this.mnuFrmAddWeb.Index = 1;
			this.mnuFrmAddWeb.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuFrmAddWeb.Shortcut")));
			this.mnuFrmAddWeb.ShowShortcut = ((bool)(resources.GetObject("mnuFrmAddWeb.ShowShortcut")));
			this.mnuFrmAddWeb.Text = resources.GetString("mnuFrmAddWeb.Text");
			this.mnuFrmAddWeb.Visible = ((bool)(resources.GetObject("mnuFrmAddWeb.Visible")));
			this.mnuFrmAddWeb.Click += new System.EventHandler(this.mnuFrmAddWeb_Click);
			// 
			// stamp
			// 
			this.stamp.AccessibleDescription = resources.GetString("stamp.AccessibleDescription");
			this.stamp.AccessibleName = resources.GetString("stamp.AccessibleName");
			this.stamp.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("stamp.Anchor")));
			this.stamp.AutoSize = ((bool)(resources.GetObject("stamp.AutoSize")));
			this.stamp.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("stamp.Dock")));
			this.stamp.Enabled = ((bool)(resources.GetObject("stamp.Enabled")));
			this.stamp.Font = ((System.Drawing.Font)(resources.GetObject("stamp.Font")));
			this.stamp.Image = ((System.Drawing.Image)(resources.GetObject("stamp.Image")));
			this.stamp.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("stamp.ImageAlign")));
			this.stamp.ImageIndex = ((int)(resources.GetObject("stamp.ImageIndex")));
			this.stamp.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("stamp.ImeMode")));
			this.stamp.Location = ((System.Drawing.Point)(resources.GetObject("stamp.Location")));
			this.stamp.Name = "stamp";
			this.stamp.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("stamp.RightToLeft")));
			this.stamp.Size = ((System.Drawing.Size)(resources.GetObject("stamp.Size")));
			this.stamp.TabIndex = ((int)(resources.GetObject("stamp.TabIndex")));
			this.stamp.Text = resources.GetString("stamp.Text");
			this.stamp.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("stamp.TextAlign")));
			this.stamp.Visible = ((bool)(resources.GetObject("stamp.Visible")));
			// 
			// chkSendDir
			// 
			this.chkSendDir.AccessibleDescription = resources.GetString("chkSendDir.AccessibleDescription");
			this.chkSendDir.AccessibleName = resources.GetString("chkSendDir.AccessibleName");
			this.chkSendDir.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkSendDir.Anchor")));
			this.chkSendDir.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkSendDir.Appearance")));
			this.chkSendDir.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkSendDir.BackgroundImage")));
			this.chkSendDir.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkSendDir.CheckAlign")));
			this.chkSendDir.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkSendDir.Dock")));
			this.chkSendDir.Enabled = ((bool)(resources.GetObject("chkSendDir.Enabled")));
			this.chkSendDir.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkSendDir.FlatStyle")));
			this.chkSendDir.Font = ((System.Drawing.Font)(resources.GetObject("chkSendDir.Font")));
			this.chkSendDir.Image = ((System.Drawing.Image)(resources.GetObject("chkSendDir.Image")));
			this.chkSendDir.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkSendDir.ImageAlign")));
			this.chkSendDir.ImageIndex = ((int)(resources.GetObject("chkSendDir.ImageIndex")));
			this.chkSendDir.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkSendDir.ImeMode")));
			this.chkSendDir.Location = ((System.Drawing.Point)(resources.GetObject("chkSendDir.Location")));
			this.chkSendDir.Name = "chkSendDir";
			this.chkSendDir.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkSendDir.RightToLeft")));
			this.chkSendDir.Size = ((System.Drawing.Size)(resources.GetObject("chkSendDir.Size")));
			this.chkSendDir.TabIndex = ((int)(resources.GetObject("chkSendDir.TabIndex")));
			this.chkSendDir.Text = resources.GetString("chkSendDir.Text");
			this.chkSendDir.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkSendDir.TextAlign")));
			this.chkSendDir.Visible = ((bool)(resources.GetObject("chkSendDir.Visible")));
			// 
			// chkEncrypt
			// 
			this.chkEncrypt.AccessibleDescription = resources.GetString("chkEncrypt.AccessibleDescription");
			this.chkEncrypt.AccessibleName = resources.GetString("chkEncrypt.AccessibleName");
			this.chkEncrypt.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkEncrypt.Anchor")));
			this.chkEncrypt.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkEncrypt.Appearance")));
			this.chkEncrypt.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkEncrypt.BackgroundImage")));
			this.chkEncrypt.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkEncrypt.CheckAlign")));
			this.chkEncrypt.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkEncrypt.Dock")));
			this.chkEncrypt.Enabled = ((bool)(resources.GetObject("chkEncrypt.Enabled")));
			this.chkEncrypt.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkEncrypt.FlatStyle")));
			this.chkEncrypt.Font = ((System.Drawing.Font)(resources.GetObject("chkEncrypt.Font")));
			this.chkEncrypt.Image = ((System.Drawing.Image)(resources.GetObject("chkEncrypt.Image")));
			this.chkEncrypt.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkEncrypt.ImageAlign")));
			this.chkEncrypt.ImageIndex = ((int)(resources.GetObject("chkEncrypt.ImageIndex")));
			this.chkEncrypt.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkEncrypt.ImeMode")));
			this.chkEncrypt.Location = ((System.Drawing.Point)(resources.GetObject("chkEncrypt.Location")));
			this.chkEncrypt.Name = "chkEncrypt";
			this.chkEncrypt.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkEncrypt.RightToLeft")));
			this.chkEncrypt.Size = ((System.Drawing.Size)(resources.GetObject("chkEncrypt.Size")));
			this.chkEncrypt.TabIndex = ((int)(resources.GetObject("chkEncrypt.TabIndex")));
			this.chkEncrypt.Text = resources.GetString("chkEncrypt.Text");
			this.chkEncrypt.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkEncrypt.TextAlign")));
			this.chkEncrypt.Visible = ((bool)(resources.GetObject("chkEncrypt.Visible")));
			// 
			// groupBox1
			// 
			this.groupBox1.AccessibleDescription = resources.GetString("groupBox1.AccessibleDescription");
			this.groupBox1.AccessibleName = resources.GetString("groupBox1.AccessibleName");
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("groupBox1.Anchor")));
			this.groupBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox1.BackgroundImage")));
			this.groupBox1.Controls.Add(this.chkEncrypt);
			this.groupBox1.Controls.Add(this.chkIM);
			this.groupBox1.Controls.Add(this.chkSendDir);
			this.groupBox1.Controls.Add(this.chkDelIMUnD);
			this.groupBox1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("groupBox1.Dock")));
			this.groupBox1.Enabled = ((bool)(resources.GetObject("groupBox1.Enabled")));
			this.groupBox1.Font = ((System.Drawing.Font)(resources.GetObject("groupBox1.Font")));
			this.groupBox1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("groupBox1.ImeMode")));
			this.groupBox1.Location = ((System.Drawing.Point)(resources.GetObject("groupBox1.Location")));
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("groupBox1.RightToLeft")));
			this.groupBox1.Size = ((System.Drawing.Size)(resources.GetObject("groupBox1.Size")));
			this.groupBox1.TabIndex = ((int)(resources.GetObject("groupBox1.TabIndex")));
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = resources.GetString("groupBox1.Text");
			this.groupBox1.Visible = ((bool)(resources.GetObject("groupBox1.Visible")));
			// 
			// btnAddrBook
			// 
			this.btnAddrBook.AccessibleDescription = resources.GetString("btnAddrBook.AccessibleDescription");
			this.btnAddrBook.AccessibleName = resources.GetString("btnAddrBook.AccessibleName");
			this.btnAddrBook.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnAddrBook.Anchor")));
			this.btnAddrBook.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAddrBook.BackgroundImage")));
			this.btnAddrBook.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnAddrBook.Dock")));
			this.btnAddrBook.Enabled = ((bool)(resources.GetObject("btnAddrBook.Enabled")));
			this.btnAddrBook.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnAddrBook.FlatStyle")));
			this.btnAddrBook.Font = ((System.Drawing.Font)(resources.GetObject("btnAddrBook.Font")));
			this.btnAddrBook.Image = ((System.Drawing.Image)(resources.GetObject("btnAddrBook.Image")));
			this.btnAddrBook.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnAddrBook.ImageAlign")));
			this.btnAddrBook.ImageIndex = ((int)(resources.GetObject("btnAddrBook.ImageIndex")));
			this.btnAddrBook.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnAddrBook.ImeMode")));
			this.btnAddrBook.Location = ((System.Drawing.Point)(resources.GetObject("btnAddrBook.Location")));
			this.btnAddrBook.Name = "btnAddrBook";
			this.btnAddrBook.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnAddrBook.RightToLeft")));
			this.btnAddrBook.Size = ((System.Drawing.Size)(resources.GetObject("btnAddrBook.Size")));
			this.btnAddrBook.TabIndex = ((int)(resources.GetObject("btnAddrBook.TabIndex")));
			this.btnAddrBook.Text = resources.GetString("btnAddrBook.Text");
			this.btnAddrBook.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnAddrBook.TextAlign")));
			this.btnAddrBook.Visible = ((bool)(resources.GetObject("btnAddrBook.Visible")));
			this.btnAddrBook.Click += new System.EventHandler(this.btnAddrBook_Click);
			// 
			// addressBookControl
			// 
			this.addressBookControl.AccessibleDescription = resources.GetString("addressBookControl.AccessibleDescription");
			this.addressBookControl.AccessibleName = resources.GetString("addressBookControl.AccessibleName");
			this.addressBookControl.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("addressBookControl.Anchor")));
			this.addressBookControl.AutoScroll = ((bool)(resources.GetObject("addressBookControl.AutoScroll")));
			this.addressBookControl.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("addressBookControl.AutoScrollMargin")));
			this.addressBookControl.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("addressBookControl.AutoScrollMinSize")));
			this.addressBookControl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("addressBookControl.BackgroundImage")));
			this.addressBookControl.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("addressBookControl.Dock")));
			this.addressBookControl.Enabled = ((bool)(resources.GetObject("addressBookControl.Enabled")));
			this.addressBookControl.Font = ((System.Drawing.Font)(resources.GetObject("addressBookControl.Font")));
			this.addressBookControl.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("addressBookControl.ImeMode")));
			this.addressBookControl.Location = ((System.Drawing.Point)(resources.GetObject("addressBookControl.Location")));
			this.addressBookControl.Name = "addressBookControl";
			this.addressBookControl.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("addressBookControl.RightToLeft")));
			this.addressBookControl.Size = ((System.Drawing.Size)(resources.GetObject("addressBookControl.Size")));
			this.addressBookControl.TabIndex = ((int)(resources.GetObject("addressBookControl.TabIndex")));
			this.addressBookControl.Visible = ((bool)(resources.GetObject("addressBookControl.Visible")));
			// 
			// FrmSend
			// 
			this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
			this.AccessibleName = resources.GetString("$this.AccessibleName");
			this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
			this.Controls.Add(this.addressBookControl);
			this.Controls.Add(this.btnAddrBook);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.stamp);
			this.Controls.Add(this.lstFormsAttached);
			this.Controls.Add(this.statusBar1);
			this.Controls.Add(this.btnView);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.btnAddForm);
			this.Controls.Add(this.cmbForms);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.btnSend);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtBody);
			this.Controls.Add(this.txtRecipient1);
			this.Controls.Add(this.txtSubject);
			this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
			this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
			this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
			this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
			this.Menu = this.mainMenu1;
			this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
			this.Name = "FrmSend";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
			this.Text = resources.GetString("$this.Text");
			this.Load += new System.EventHandler(this.frmSend_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		/// 
		private WSEmailMessage theMessage = null;
		private DimeAttachment[] DimeAttachments = null;
		public void reply(WSEmailMessage w) 
		{
			theMessage = w;
			UpdateFormStatus();
		}			
		private void frmSend_Load(object sender, System.EventArgs e)
		{
//			if (this.cmbForms.DataSource == null)
//				MessageBox.Show("For whatever reason, there are not available objects.");

			if (theMessage != null ) 
			{
				// TODO fix for multiple recipients later
				// txtSender.Text = theMessage.Recipient;
				addressBookControl.Text = theMessage.Sender;
				txtSubject.Text = "Re: " + theMessage.Subject;
				string[] aa = new string[]{"", "", "In reply to the following, written " + theMessage.Timestamp, ""};
				ArrayList a = new ArrayList();
				a.AddRange(aa);
				a.AddRange(theMessage.Body.Split(Environment.NewLine.ToCharArray()));
				txtBody.Lines = (string[])a.ToArray(typeof(string));
					
				txtBody.Focus();
			}
			else
			{
				// txtSender.Text = PennRoutingUtilities.GetCertEmail(PennRoutingUtilities.GetSecurityToken(PennRoutingUtilities.MyIdentifer,false).Certificate);
				addressBookControl.Text = "";
				txtSubject.Text = "Testing...";
				txtBody.Text = "This is a test message!";
			}
		}

		/*
		private void SendingCallback(IAsyncResult r) 
		{
			Global.Proxy.RequestSoapContext.Attachments.Clear();
			// TODO : This is messy. Clean up the proxies some how... either by locking or cloning.


			try 
			{
				WSEmailStatus s = Global.Proxy.EndWSEmailSend(r);
				Utilities.LogToStatusWindow(s.Message);
				statusBar1.Text = "Reply from server: " + s.ToString();
				stamp.Image = Global.CancelledStamp;
			} 
			catch (Exception e) 
			{
				new ExceptionForm(e,"An error occurred while sending the message.");
			}
		}
		*/
		public class SendingThread 
		{
			WSEmailMessage Message = null;
			FrmSend frm = null;
			public SendingThread(WSEmailMessage m, FrmSend f) 
			{
				this.Message = m;
				frm = f;
			}

			public void Send() 
			{
				try 
				{
					WSEmailMessage m = Message;
					frm.statusBar1.Text = "Sending message...";
					frm.stamp.Image = Global.UnusedStamp;
					frm.btnSend.Enabled = false;
					MailServerProxy p = new MailServerProxy(Global.Proxy.Url);
					p.SecurityToken = Global.Proxy.SecurityToken;

					if (frm.DimeAttachments != null)
						p.RequestSoapContext.Attachments.AddRange(frm.DimeAttachments);

					frm.DimeAttachments = null;

					WSEmailStatus s = null;
					Global.LogToStatusWindow("Beginning to send message entitled '"+m.Subject+"' to " + m.Recipients+".");

					if ((m.MessageFlags & WSEmailFlags.Precedence.DirectDelivery) ==
						WSEmailFlags.Precedence.DirectDelivery)
						s = SendDirectly(m);
					else 
					{
						if ((m.MessageFlags & WSEmailFlags.Precedence.EncryptedDelivery) == 
							WSEmailFlags.Precedence.EncryptedDelivery) 
							p.AddServerEncryption();
						s = p.WSEmailSend(m,null);	
					}
		
					frm.statusBar1.Text = s.ToString();

					if (s != null && s.ResponseCode == 200) 
					{
						frm.stamp.Image = Global.CancelledStamp;
						Global.LogToStatusWindow(s.ToString());
					} 
					else
						Global.LogToStatusWindow("No status returned.");

					frm.btnSend.Enabled = true;
				} 
				catch (Exception e) 
				{
					new ExceptionForm(e,"An error occurred while sending the message.");
				}
			}



			private WSEmailStatus SendDirectly(WSEmailMessage m) 
			{
				string ret = "";
				int res = 200;
				foreach (string r in m.Recipients.GetDistinctDestinations()) 
				{
					string target = "";

					try 
					{
						target = Global.GetServerUrl(r);
					} 
					catch {}

					if (target.Equals(""))
						return null;

					Global.LogToStatusWindow("Sending directly to " + target);

					MailServerProxy proxy = new MailServerProxy(target);
					proxy.SecurityToken = Global.FederatedTokenManager.Token;

					if ((m.MessageFlags & WSEmailFlags.Precedence.EncryptedDelivery) == 
						WSEmailFlags.Precedence.EncryptedDelivery) 
							proxy.AddServerEncryption();
					
					WSEmailStatus s = proxy.WSEmailSend(m,null);
					ret += "Results for " + target+"\n" + s.ToString()+"\n\n";
					res = s.ResponseCode;
				}

				//MessageBox.Show(ret,"Sending results...");
				return new WSEmailStatus(res,ret);
			}

		}

		private void btnSend_Click(object sender, System.EventArgs e)
		{

			WSEmailMessage wem = new WSEmailMessage();

			wem.Recipients.AddRange(RecipientList.ParseRecipients(addressBookControl.Text));
			if (wem.Recipients.ToString().Equals("")) 
			{
				MessageBox.Show("Please type in recipients for your message!!","Oops...");
				return;
			}

			wem.Subject = txtSubject.Text;
			wem.Body = txtBody.Text;
			wem.Timestamp = DateTime.Now;
			if (chkIM.Checked)
				wem.MessageFlags |= WSEmailFlags.InstantMessaging.SendAsInstantMessage;
			if (chkDelIMUnD.Checked)
				wem.MessageFlags |= WSEmailFlags.InstantMessaging.DeleteIfNotDeliverable;
			if (theMessage != null)
				wem.XmlAttachments = theMessage.XmlAttachments;
			if (wem != null && wem.XmlAttachments != null && wem.XmlAttachments.Length > 0)
				wem.MessageFlags |= WSEmailFlags.Contains.DynamicForm;
			if (chkEncrypt.Checked)
				wem.MessageFlags |= WSEmailFlags.Precedence.EncryptedDelivery;
			if (chkSendDir.Checked)
				wem.MessageFlags |= WSEmailFlags.Precedence.DirectDelivery;
			
			(new Thread(new ThreadStart((new SendingThread(wem,this)).Send))).Start();

			/*
			statusBar1.Text = "Sending message...";
			
			if (this.chkSendDir.Checked) 
			{
				if (SendDirectly(wem))
					statusBar1.Text = "Sent successfully";
				else
					statusBar1.Text = "Message not sent";
			} 
			else 
			{
				AsyncCallback callback = new AsyncCallback(SendingCallback);
				sendMessageDelegate AsyncDelegate = new sendMessageDelegate(Global.Proxy.WSEmailSend);

				Global.LogToStatusWindow("Beginning to send message entitled '"+wem.Subject+"' to " + wem.Recipients+".");
				Global.Proxy.BeginWSEmailSend(wem,null,callback,null);
			}
			*/
		}

		private void UpdateFormStatus() 
		{
			if (theMessage.XmlAttachments!= null && theMessage.XmlAttachments.Length > 0) 
			{
				btnView.Enabled = true;
				addressBookControl.Enabled = false;
				lstFormsAttached.Items.Clear();
				lstFormsAttached.Items.AddRange(ObjectLoader.GetFormInventory(theMessage.XmlAttachments));
			} 
			else 
			{
				btnView.Enabled = false;
			}
		}

		private void btnAddForm_Click(object sender, System.EventArgs e)
		{
			if (this.cmbForms.SelectedItem == null) 
			{
				MessageBox.Show("Please select an object from the dropdown box to attach.","Oops!");
				return;
			}
			if (theMessage == null)
				theMessage = new WSEmailMessage();

			BO = ObjectLoader.LoadObject((ObjectConfiguration)this.cmbForms.SelectedItem);
			BO.DoneEditing += new BaseObject.BaseObjectDelegate(FormDone);
			BO.UpdateAction = BaseObject.UpdateActions.ADD;
			BO.NextHopChanged += new BaseObject.StringDelegate(NextHopChanged);
			BO.TokenRequest += new BaseObject.RequestTokenDelegate(TokenRequested);
			BO.Run();
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
		private void NextHopChanged (string s) 
		{
			this.addressBookControl.Text = s;
		}

		private void FormDone(BaseObject b) 
		{
			if (b.UpdateAction == BaseObject.UpdateActions.ADD) 
			{
				if (theMessage.XmlAttachments != null) 
				{
					ArrayList a = new ArrayList();
					a.AddRange(theMessage.XmlAttachments);
					a.Add(b.AsXmlDocument());
					theMessage.XmlAttachments = (XmlDocument[])a.ToArray(typeof(XmlDocument));
				} 
				else 
				{
					theMessage.XmlAttachments = new XmlDocument[1];
					theMessage.XmlAttachments[0] = b.AsXmlDocument();
				}
			} 
			else if (b.UpdateAction == BaseObject.UpdateActions.REPLACE) 
			{
				theMessage.XmlAttachments[b.Identifier] = b.AsXmlDocument();
			}

			if (b.DimeAttachments != null) 
			{
				DialogResult r = MessageBox.Show(
					"The applet has files which it would like to attachment to your message. Is this acceptable?",
					"Security question...", MessageBoxButtons.YesNo,MessageBoxIcon.Question,
					MessageBoxDefaultButton.Button2);
				if (r == DialogResult.Yes)
					this.DimeAttachments = (DimeAttachment[])b.DimeAttachments.Clone();
			}

			b.Dispose();
			UpdateFormStatus();
		}

		private void FormDone() 
		{
			BO.Dispose();
		}

		private void btnView_Click(object sender, System.EventArgs e)
		{
			try 
			{
				if (theMessage.XmlAttachments != null && theMessage.XmlAttachments.Length > 0) 
				{
					if (lstFormsAttached.SelectedItem == null)
						MessageBox.Show("Please select which form you wish to view!","Oops!");
					else 
					{
					
						BO = ObjectLoader.LoadObject(theMessage.XmlAttachments[lstFormsAttached.SelectedIndex].OuterXml);
						BO.Identifier = lstFormsAttached.SelectedIndex;
						BO.DoneEditing += new BaseObject.BaseObjectDelegate(FormDone);
						BO.UpdateAction = BaseObject.UpdateActions.REPLACE;
						BO.NextHopChanged += new BaseObject.StringDelegate(NextHopChanged);
						BO.TokenRequest += new BaseObject.RequestTokenDelegate(TokenRequested);
						BO.Run();
					}
				}
			} 
			catch (Exception ex) 
			{
				new ExceptionForm(ex,"Unable to run viewing plugin.");
			}
		}

		private void mnuFrmAddWeb_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show("Feature not yet available. (frmSend.cs:917)","Oops!");
		}

		private void mnuFrmAddDLL_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog f = new OpenFileDialog();
			f.DefaultExt = "DLL";
			f.Filter = "DLLs (*.dll)|*.dll|All files (*.*)|*.*";
			f.FilterIndex = 0;
			f.CheckFileExists = true;
			f.Title = "Select DLL to scan...";
			f.InitialDirectory = ".";
			System.Windows.Forms.DialogResult r = f.ShowDialog();
			if (r == DialogResult.OK) 
			{
				ObjectConfiguration[] objs = ObjectLoader.ScanLibrary(f.FileName);
				if (objs != null && objs.Length > 0) 
				{
					foreach (ObjectConfiguration o in objs) 
					{
						r = MessageBox.Show("Would you like to add the following object to your available forms?\r\n\r\nName: " + o.FriendlyName + "\r\nVersion: " + o.Version.ToString() + "\r\nDescription: " + o.Description,"Question...",MessageBoxButtons.YesNo);
						if (r == DialogResult.Yes)
							ObjectLoader.UpdateAssemblyList(o);
					}
				} else
					MessageBox.Show("No suitable objects were found in the given library.","Oops!");
			}
			this.cmbForms.DataSource = ObjectLoader.GetAvailableObjects();
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnAddrBook_Click(object sender, System.EventArgs e)
		{
			FrmAddressBook f = new FrmAddressBook();
			f.ShowDialog();
			if (f.SelectedEmails != null) 
			{
				if (addressBookControl.Text.Length > 0) 
				{
					addressBookControl.Text += f.SelectedEmails + ", ";
				}
				else
					addressBookControl.Text = f.SelectedEmails + ", ";
			}
			this.Activate();
			addressBookControl.Focus();
		}

		private void txtSubject_TextChanged(object sender, System.EventArgs e)
		{
			this.Text = "New Message '" + this.txtSubject.Text +"'";
		}
	}
}
