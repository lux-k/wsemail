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
using System.Data;
using System.Runtime.InteropServices;
using System.Configuration;

namespace DynamicBizObjects
{
	/// <summary>
	/// Summary description for FrmPurchaseOrder.
	/// </summary>
	public class FrmPurchaseOrder : System.Windows.Forms.Form, BusinessObjectsFormInterface
	{
		private System.Windows.Forms.DataGrid dgItems;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private BusinessRequest b = null;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.TextBox txtTotal;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.TextBox txtVendor;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtBiz;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtAcct;
		private System.Windows.Forms.Label label4;
		private PurchaseOrder p = null;
		public event BusinessObjectsDelegates.NullDelegate UserDone;
		const string FORMNAME = "Purchase Order";
		private System.Windows.Forms.Button btnSubmit;
		private System.Windows.Forms.Label lblSigner;
		const string FORMVER = "1.0.1";

		public FrmPurchaseOrder()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			p = new PurchaseOrder();
			p.AddItem(new POItem("Spongebob toy",5,(float)5.0));
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			InsertSecurityBox();
			InitializeObjects();
		}

		private void InitializeObjects() 
		{
			this.dgItems.TableStyles.Clear();
			this.dgItems.TableStyles.Add(p.GetDataGridTableStyle());
			this.dgItems.DataSource = p.Items;
			txtVendor.Text = p.Vendor;
			txtAcct.Text = p.Account;
			if (b != null && b.BusinessObject != null) 
			{
				b.RefreshMappings();
				MappedItem m = b.GetMappedItem("BusinessOffice");
				string myID = PennLibraries.Utilities.GetCertEmail(PennLibraries.Utilities.GetSecurityToken(ConfigurationSettings.AppSettings["SigningCertificate"],false).Certificate);
				//MessageBox.Show(myID + b.Approvals.Required.FindID(myID).ToString());
				if (b.Approvals.Required.FindID(myID) >= 0)
					btnSubmit.Text = "Approve...";
				else 
				{
					btnSubmit.Enabled = false;
					btnSubmit.Visible = false;
				}
				if (m != null)
					txtBiz.Text = m.User;
				m = b.GetMappedItem("Claimant");
				if (m != null) 
				{
					txtBiz.Enabled = false;
					txtAcct.Enabled = false;
					txtVendor.Enabled = false;
					dgItems.ReadOnly = true;
					btnAdd.Enabled = false;
					btnRemove.Enabled = false;
					lblSigner.Text = "Digitally submitted on " + m.Signature.Timestamp + " by " + m.User+".";
					lblSigner.Visible = true;
				}
			}
			p.TotalChanged += new BusinessObjectsDelegates.NullDelegate(this.UpdateTotal);
			UpdateTotal();
		}

		void UpdateTotal() 
		{
			txtTotal.Text = p.Total.ToString("C");
//String.Format("c{0}",p.Total.ToString());
		}

		#region BusinessObjectsFormInterface
		public void ThrowOut() 
		{
			this.Hide();
			this.Dispose();
		}


		public BusinessRequest GetBusinessRequest() 
		{
			b.BusinessObject = p;
			return b;
		}


		public BusinessRequest BusinessRequest 
		{
			get 
			{
				return b;
			}
			set 
			{
				b = value;
				
			}
		}


		public void LoadBusinessRequestAndShow(BusinessRequest b) 
		{
			if (b != null)
				this.b = b;

			if (this.b != null)
				p = (PurchaseOrder)this.b.BusinessObject;
			this.InitializeObjects();
			this.Show();
		}
		#endregion

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

		#region SecurityBox
		[DllImport("user32.dll")]
		private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

		[DllImport("user32.dll")]
		private static extern bool AppendMenu (IntPtr hMenu, Int32 wFlags, Int32 
			wIDNewItem, string lpNewItem);

		public const Int32 WM_SYSCOMMAND = 0x112;
		public const Int32 MF_SEPARATOR = 0x800;
		public const Int32 MF_STRING = 0x0;
		public const Int32 IDM_ABOUT= 1000;
		public const Int32 IDM_SIGS = 1001;
		public const Int32 IDM_DEL  = 1002;
		public const Int32 IDM_MAPS = 1003;

		protected override void WndProc(ref Message m)
		{
			
			if (b == null) 
				b = new BusinessRequest();

			if(m.Msg == WM_SYSCOMMAND)
				switch(m.WParam.ToInt32())
				{
					case IDM_ABOUT : 
						MessageBox.Show("Programmed by: Kevin Lux\nPenn Security Lab\n\nForm: " + FORMNAME + " v" + FORMVER,"About this form...",MessageBoxButtons.OK,MessageBoxIcon.Information);
						return;
					case IDM_SIGS : 
						FrmSignatures form = new FrmSignatures();
						form.DisplaySignatures(b);
						form.ShowDialog(this);
						form.Dispose();
						return;
					case IDM_DEL : 
						FrmDelegates form2 = new FrmDelegates();
						form2.DisplayDelegates(b.Delegates);
						form2.ShowDialog(this);
						b.Delegates = form2.GetDelegates();
						form2.Dispose();
						return;
					case IDM_MAPS : 
						FrmMappings form3 = new FrmMappings();
						form3.ShowMappings(b);
						form3.ShowDialog(this);
						form3.Dispose();
						return;
					default:
						break;
				} base.WndProc(ref m);
		}


		protected void InsertSecurityBox() 
		{
			IntPtr sysMenuHandle = GetSystemMenu(this.Handle, false);
			AppendMenu(sysMenuHandle, MF_SEPARATOR, 0, string.Empty);
			AppendMenu(sysMenuHandle, MF_STRING, IDM_SIGS, "Signatures...");
			AppendMenu(sysMenuHandle, MF_STRING, IDM_DEL, "Delegate...");
			AppendMenu(sysMenuHandle, MF_STRING, IDM_MAPS, "Mappings...");
			AppendMenu(sysMenuHandle, MF_STRING, IDM_ABOUT, "About...");
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.dgItems = new System.Windows.Forms.DataGrid();
			this.btnAdd = new System.Windows.Forms.Button();
			this.txtTotal = new System.Windows.Forms.TextBox();
			this.btnRemove = new System.Windows.Forms.Button();
			this.txtVendor = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtBiz = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtAcct = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.btnSubmit = new System.Windows.Forms.Button();
			this.lblSigner = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dgItems)).BeginInit();
			this.SuspendLayout();
			// 
			// dgItems
			// 
			this.dgItems.CaptionText = "Items to purchase:";
			this.dgItems.DataMember = "";
			this.dgItems.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgItems.Location = new System.Drawing.Point(8, 120);
			this.dgItems.Name = "dgItems";
			this.dgItems.Size = new System.Drawing.Size(536, 208);
			this.dgItems.TabIndex = 9;
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(8, 336);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(72, 32);
			this.btnAdd.TabIndex = 9;
			this.btnAdd.Text = "Add...";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// txtTotal
			// 
			this.txtTotal.Location = new System.Drawing.Point(408, 336);
			this.txtTotal.Name = "txtTotal";
			this.txtTotal.ReadOnly = true;
			this.txtTotal.Size = new System.Drawing.Size(136, 20);
			this.txtTotal.TabIndex = 20;
			this.txtTotal.Text = "textBox1";
			// 
			// btnRemove
			// 
			this.btnRemove.Location = new System.Drawing.Point(96, 336);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(72, 32);
			this.btnRemove.TabIndex = 9;
			this.btnRemove.Text = "Remove...";
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// txtVendor
			// 
			this.txtVendor.Location = new System.Drawing.Point(96, 88);
			this.txtVendor.Name = "txtVendor";
			this.txtVendor.Size = new System.Drawing.Size(168, 20);
			this.txtVendor.TabIndex = 2;
			this.txtVendor.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 88);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 24);
			this.label1.TabIndex = 5;
			this.label1.Text = "Vendor:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(328, 336);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 24);
			this.label2.TabIndex = 6;
			this.label2.Text = "Total:";
			// 
			// txtBiz
			// 
			this.txtBiz.Location = new System.Drawing.Point(96, 48);
			this.txtBiz.Name = "txtBiz";
			this.txtBiz.Size = new System.Drawing.Size(168, 20);
			this.txtBiz.TabIndex = 1;
			this.txtBiz.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 48);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 32);
			this.label3.TabIndex = 8;
			this.label3.Text = "Business Office:";
			// 
			// txtAcct
			// 
			this.txtAcct.Location = new System.Drawing.Point(96, 8);
			this.txtAcct.Name = "txtAcct";
			this.txtAcct.Size = new System.Drawing.Size(168, 20);
			this.txtAcct.TabIndex = 0;
			this.txtAcct.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 8);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 24);
			this.label4.TabIndex = 10;
			this.label4.Text = "Account:";
			// 
			// btnSubmit
			// 
			this.btnSubmit.Location = new System.Drawing.Point(440, 408);
			this.btnSubmit.Name = "btnSubmit";
			this.btnSubmit.Size = new System.Drawing.Size(104, 40);
			this.btnSubmit.TabIndex = 3;
			this.btnSubmit.Text = "Submit...";
			this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
			// 
			// lblSigner
			// 
			this.lblSigner.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblSigner.Location = new System.Drawing.Point(8, 400);
			this.lblSigner.Name = "lblSigner";
			this.lblSigner.Size = new System.Drawing.Size(216, 48);
			this.lblSigner.TabIndex = 12;
			this.lblSigner.Text = "label5";
			this.lblSigner.Visible = false;
			// 
			// FrmPurchaseOrder
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(576, 461);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.lblSigner,
																		  this.btnSubmit,
																		  this.label4,
																		  this.txtAcct,
																		  this.label3,
																		  this.txtBiz,
																		  this.label2,
																		  this.label1,
																		  this.txtVendor,
																		  this.btnRemove,
																		  this.txtTotal,
																		  this.btnAdd,
																		  this.dgItems});
			this.Name = "FrmPurchaseOrder";
			this.Text = "Purchase Order";
			((System.ComponentModel.ISupportInitialize)(this.dgItems)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			p.Items.Add(new POItem());
			PingDataGrid();
		}

		private void PingDataGrid() 
		{
			this.dgItems.DataSource = new ArrayList();
			this.dgItems.DataSource = p.Items;
		}

		private void btnRemove_Click(object sender, System.EventArgs e)
		{
			if (dgItems.CurrentRowIndex >= 0) 
			{
				POItem m = p.Items[dgItems.CurrentRowIndex];
				if (MessageBox.Show("Are you sure you want to remove '" +m.Item+"' from the order?","Question...",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2,MessageBoxOptions.DefaultDesktopOnly) == DialogResult.Yes) 
				{
					p.Items.RemoveAt(dgItems.CurrentRowIndex);
					PingDataGrid();
				}

			}
		}

		private void btnSubmit_Click(object sender, System.EventArgs e)
		{
			if (txtAcct.Text.Equals("")) 
			{
				MessageBox.Show("Don't forget to enter your account information.","Oops!");
				return;
			}
			if (txtBiz.Text.Equals("")) 
			{
				MessageBox.Show("Don't forget to enter your business office's information.","Oops!");
				return;
			}
			if (txtVendor.Text.Equals("")) 
			{
				MessageBox.Show("Don't forget to enter the vendor to buy from.","Oops!");
				return;
			}
			if (p.Items.Count == 0) 
			{
				MessageBox.Show("Don't forget to enter some things to buy.","Oops!");
				return;
			}
			if (b == null)
				b = new BusinessRequest();

			p.Account = txtAcct.Text;
			p.Vendor = txtVendor.Text;
			
			if (b.BusinessObject == null) {
				p.FormName = FORMNAME;
				b.Approvals.Required.Add(new Signature(ConfigurationSettings.AppSettings["POs-Address"],null));
				b.Approvals.Required.Add(new Signature(ConfigurationSettings.AppSettings["Purchasing-Address"],null));
				b.Mappings.Add(new MappedItem("Claimant",null,null));
				b.Mappings.Add(new MappedItem("Purchasing",ConfigurationSettings.AppSettings["Purchasing-Address"],null));
			}

			b.BusinessObject = p;
			b.GenerateApproval(ConfigurationSettings.AppSettings["SigningCertificate"],false);

			if (UserDone != null)
				UserDone.DynamicInvoke(null);

		}

	}
}
