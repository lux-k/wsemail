/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System.IO;
using System.Configuration;
using System.Xml;
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Web.Services2.Security.X509;

namespace DynamicBizObjects
{
	/// <summary>
	/// Summary description for FrmTimesheet.
	/// </summary>
	/// 

	public class FrmTimesheet : System.Windows.Forms.Form, BusinessObjectsFormInterface
	{

		private const string FORMNAME = "Timesheet";
		private const string FORMVER = "1.1.3";
		private System.Windows.Forms.DataGrid dgHours;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label lblName;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox grpBasic;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button btnAutoFill;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.TextBox txtDept;
		private System.Windows.Forms.TextBox txtHours;
		private System.Windows.Forms.TextBox txtIDNum;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtRegHrs;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtOvrHrs;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox txtPrmOvr;
		private System.Windows.Forms.GroupBox groupBox1;
		Timesheet t = new Timesheet();
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Button btnSubmitEmp;
		private System.Windows.Forms.Label lblEmpSign;
		private System.Windows.Forms.Label lblSupSign;
		ToolTip toolTip = new ToolTip();
		private System.Windows.Forms.Button btnSubmitSup;
		private System.Windows.Forms.Button btnApprove;
		BusinessRequest b = null;
		public event BusinessObjectsDelegates.NullDelegate UserDone;

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

		public void ThrowOut() 
		{
			this.Hide();
			this.Dispose();
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

		public void LoadBusinessRequestAndShow(BusinessRequest br) 
		{
			if (br == null && b != null)
				br = b;

			if (br != null) 
			{
				b = br;
				t = (Timesheet)br.BusinessObject;
				UpdateEmployeeInformation(t.EmployeeInformation);
				SetMappings();
				MappedItem m = this.b.GetMappedItem("Employee");
				if (m.Signature != null) 
				{
					this.btnSubmitEmp.Enabled = false;
					this.dgHours.ReadOnly = true;
					this.btnAutoFill.Enabled = false;
					this.txtName.Enabled = false;
					this.txtTitle.Enabled = false;
					this.txtDept.Enabled = false;
					this.textBox3.Enabled = false;
					this.txtHours.Enabled = false;
					this.txtIDNum.Enabled = false;
					this.txtRegHrs.Enabled = false;
					this.txtOvrHrs.Enabled = false;
					this.txtPrmOvr.Enabled = false;
				}
				
				string myID = PennLibraries.Utilities.GetCertEmail(PennLibraries.Utilities.GetSecurityToken(ConfigurationSettings.AppSettings["SigningCertificate"],false).Certificate);
				int i = b.Approvals.Required.FindID(myID);
				if (i == -1)
					MessageBox.Show("Your signature is not required on the form!");
				else 
				{

					m = this.b.GetMappedItem("Supervisor");
					
					if ((m.Signature.Value == null || m.Signature.Value.OuterXml.Equals("")) && m.User.Equals(myID))
						btnSubmitSup.Enabled = true;
					else 
					{
						btnSubmitSup.Enabled = false;
						btnApprove.Visible = true;
					}
				}			
			} 
			else 
			{
				t = new Timesheet();
			}
			InitTimeSheetHours();
			this.Show();
		}
		private void FormClosing(object sender, System.ComponentModel.CancelEventArgs e) 
		
		{
			if (UserDone != null)
				UserDone.DynamicInvoke(null);
		}

		private void InitTimeSheetHours() 
		{
			t.TotalHoursChanged += new Timesheet.TotalChangedEvent(UpdateHours);
			dgHours.TableStyles.Clear();
			dgHours.TableStyles.Add(t.GetDataGridTableStyle());
			dgHours.DataSource= t.WeeklyHours;
			UpdateHours();

		}
		public FrmTimesheet()
		{
			InitializeComponent();
			this.Closing += new CancelEventHandler(FormClosing);
			InitTimeSheetHours();
			toolTip.AutoPopDelay = 5000;
			toolTip.InitialDelay = 250;
			toolTip.ReshowDelay = 500;
			// Force the ToolTip text to be displayed whether or not the form is active.
			toolTip.ShowAlways = true;
			InsertSecurityBox();
		}

		public BusinessRequest GetBusinessRequest() 
		{
			return b;
		}

		public void UpdateHours() 
		{
			txtRegHrs.Text = t.CalculatedRegular.ToString();
			txtOvrHrs.Text = t.CalculatedOvertime.ToString();
			txtPrmOvr.Text = t.PremiumOvertime.ToString();
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
			this.dgHours = new System.Windows.Forms.DataGrid();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.btnAutoFill = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtDept = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtHours = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtIDNum = new System.Windows.Forms.TextBox();
			this.grpBasic = new System.Windows.Forms.GroupBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.txtRegHrs = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.txtOvrHrs = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.txtPrmOvr = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnSubmitEmp = new System.Windows.Forms.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.lblSupSign = new System.Windows.Forms.Label();
			this.lblEmpSign = new System.Windows.Forms.Label();
			this.btnSubmitSup = new System.Windows.Forms.Button();
			this.btnApprove = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgHours)).BeginInit();
			this.grpBasic.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dgHours
			// 
			this.dgHours.AllowSorting = false;
			this.dgHours.CaptionText = "Hours worked:";
			this.dgHours.DataMember = "";
			this.dgHours.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgHours.Location = new System.Drawing.Point(40, 208);
			this.dgHours.Name = "dgHours";
			this.dgHours.RowHeadersVisible = false;
			this.dgHours.Size = new System.Drawing.Size(532, 161);
			this.dgHours.TabIndex = 0;
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(128, 24);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(168, 20);
			this.txtName.TabIndex = 3;
			this.txtName.Text = "";
			// 
			// lblName
			// 
			this.lblName.Location = new System.Drawing.Point(8, 24);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(80, 24);
			this.lblName.TabIndex = 4;
			this.lblName.Text = "Name:";
			this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnAutoFill
			// 
			this.btnAutoFill.Location = new System.Drawing.Point(520, 152);
			this.btnAutoFill.Name = "btnAutoFill";
			this.btnAutoFill.Size = new System.Drawing.Size(80, 24);
			this.btnAutoFill.TabIndex = 5;
			this.btnAutoFill.Text = "Autofill...";
			this.btnAutoFill.Click += new System.EventHandler(this.btnAutoFill_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 56);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 24);
			this.label1.TabIndex = 7;
			this.label1.Text = "Title:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtTitle
			// 
			this.txtTitle.Location = new System.Drawing.Point(128, 56);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(168, 20);
			this.txtTitle.TabIndex = 6;
			this.txtTitle.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 88);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 24);
			this.label2.TabIndex = 9;
			this.label2.Text = "Department:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtDept
			// 
			this.txtDept.Location = new System.Drawing.Point(128, 88);
			this.txtDept.Name = "txtDept";
			this.txtDept.Size = new System.Drawing.Size(168, 20);
			this.txtDept.TabIndex = 8;
			this.txtDept.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 120);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 24);
			this.label3.TabIndex = 11;
			this.label3.Text = "Pay Period Ending:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(128, 120);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(168, 20);
			this.textBox3.TabIndex = 10;
			this.textBox3.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 152);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(112, 24);
			this.label4.TabIndex = 13;
			this.label4.Text = "Scheduled Hours:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtHours
			// 
			this.txtHours.Location = new System.Drawing.Point(128, 152);
			this.txtHours.Name = "txtHours";
			this.txtHours.Size = new System.Drawing.Size(168, 20);
			this.txtHours.TabIndex = 12;
			this.txtHours.Text = "";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(312, 24);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(112, 24);
			this.label5.TabIndex = 15;
			this.label5.Text = "ID Number:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtIDNum
			// 
			this.txtIDNum.Location = new System.Drawing.Point(432, 24);
			this.txtIDNum.Name = "txtIDNum";
			this.txtIDNum.Size = new System.Drawing.Size(168, 20);
			this.txtIDNum.TabIndex = 14;
			this.txtIDNum.Text = "";
			// 
			// grpBasic
			// 
			this.grpBasic.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.label6,
																				   this.label1,
																				   this.label5,
																				   this.label4,
																				   this.txtHours,
																				   this.label3,
																				   this.txtName,
																				   this.textBox3,
																				   this.txtTitle,
																				   this.lblName,
																				   this.label2,
																				   this.txtIDNum,
																				   this.txtDept,
																				   this.btnAutoFill});
			this.grpBasic.Location = new System.Drawing.Point(8, 8);
			this.grpBasic.Name = "grpBasic";
			this.grpBasic.Size = new System.Drawing.Size(608, 184);
			this.grpBasic.TabIndex = 16;
			this.grpBasic.TabStop = false;
			this.grpBasic.Text = "Basic Information";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(312, 56);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(288, 88);
			this.label6.TabIndex = 16;
			this.label6.Text = "You may automatically download your information by pressing the \"Autofill\" button" +
				" below. We\'ll use your X509 certificate to download the information.";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(16, 24);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(160, 24);
			this.label7.TabIndex = 19;
			this.label7.Text = "Regular Hours:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtRegHrs
			// 
			this.txtRegHrs.Location = new System.Drawing.Point(184, 24);
			this.txtRegHrs.Name = "txtRegHrs";
			this.txtRegHrs.Size = new System.Drawing.Size(40, 20);
			this.txtRegHrs.TabIndex = 18;
			this.txtRegHrs.Text = "";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(16, 56);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(160, 24);
			this.label8.TabIndex = 21;
			this.label8.Text = "Straight-Time Overtime Hours:";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtOvrHrs
			// 
			this.txtOvrHrs.Location = new System.Drawing.Point(184, 56);
			this.txtOvrHrs.Name = "txtOvrHrs";
			this.txtOvrHrs.Size = new System.Drawing.Size(40, 20);
			this.txtOvrHrs.TabIndex = 20;
			this.txtOvrHrs.Text = "";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(16, 88);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(160, 24);
			this.label9.TabIndex = 23;
			this.label9.Text = "Premium Overtime Hours:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtPrmOvr
			// 
			this.txtPrmOvr.Location = new System.Drawing.Point(184, 88);
			this.txtPrmOvr.Name = "txtPrmOvr";
			this.txtPrmOvr.Size = new System.Drawing.Size(40, 20);
			this.txtPrmOvr.TabIndex = 22;
			this.txtPrmOvr.Text = "";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.txtOvrHrs,
																					this.label8,
																					this.label9,
																					this.txtPrmOvr,
																					this.label7,
																					this.txtRegHrs});
			this.groupBox1.Location = new System.Drawing.Point(384, 384);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(232, 120);
			this.groupBox1.TabIndex = 24;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Totals:";
			// 
			// btnSubmitEmp
			// 
			this.btnSubmitEmp.Location = new System.Drawing.Point(216, 584);
			this.btnSubmitEmp.Name = "btnSubmitEmp";
			this.btnSubmitEmp.Size = new System.Drawing.Size(80, 32);
			this.btnSubmitEmp.TabIndex = 25;
			this.btnSubmitEmp.Text = "Sign...";
			this.btnSubmitEmp.Click += new System.EventHandler(this.Employee_Click);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(32, 576);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(104, 24);
			this.label10.TabIndex = 26;
			this.label10.Text = "Employee";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(336, 576);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(104, 24);
			this.label11.TabIndex = 27;
			this.label11.Text = "Supervisor";
			// 
			// lblSupSign
			// 
			this.lblSupSign.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblSupSign.Location = new System.Drawing.Point(336, 536);
			this.lblSupSign.Name = "lblSupSign";
			this.lblSupSign.Size = new System.Drawing.Size(232, 40);
			this.lblSupSign.TabIndex = 28;
			// 
			// lblEmpSign
			// 
			this.lblEmpSign.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblEmpSign.Location = new System.Drawing.Point(32, 536);
			this.lblEmpSign.Name = "lblEmpSign";
			this.lblEmpSign.Size = new System.Drawing.Size(264, 40);
			this.lblEmpSign.TabIndex = 29;
			// 
			// btnSubmitSup
			// 
			this.btnSubmitSup.Enabled = false;
			this.btnSubmitSup.Location = new System.Drawing.Point(488, 584);
			this.btnSubmitSup.Name = "btnSubmitSup";
			this.btnSubmitSup.Size = new System.Drawing.Size(80, 32);
			this.btnSubmitSup.TabIndex = 30;
			this.btnSubmitSup.Text = "Sign...";
			this.btnSubmitSup.Click += new System.EventHandler(this.Supervisor_Click);
			// 
			// btnApprove
			// 
			this.btnApprove.Location = new System.Drawing.Point(512, 640);
			this.btnApprove.Name = "btnApprove";
			this.btnApprove.Size = new System.Drawing.Size(80, 40);
			this.btnApprove.TabIndex = 31;
			this.btnApprove.Text = "Approve...";
			this.btnApprove.Visible = false;
			this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
			// 
			// FrmTimesheet
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(624, 685);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btnApprove,
																		  this.btnSubmitSup,
																		  this.lblEmpSign,
																		  this.lblSupSign,
																		  this.label11,
																		  this.label10,
																		  this.btnSubmitEmp,
																		  this.groupBox1,
																		  this.grpBasic,
																		  this.dgHours});
			this.Name = "FrmTimesheet";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Timesheet";
			((System.ComponentModel.ISupportInitialize)(this.dgHours)).EndInit();
			this.grpBasic.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void UpdateEmployeeInformation(EmployeeInformation e) 
		{
			txtName.Text = e.Name;
			txtDept.Text = e.Department;
			txtHours.Text = e.HoursPerWeek.ToString();
			txtTitle.Text = e.Title;
			txtIDNum.Text = e.IDNum;
			t.EmployeeInformation = e;
			this.Text = e.Name + "'s Timesheet";
		}
		private void btnAutoFill_Click(object sender, System.EventArgs f)
		{
			EmployeeInfoProxy p = new EmployeeInfoProxy();
			EmployeeInformation e = p.GetEmployeeInformation();
			UpdateEmployeeInformation(e);
		}

		private void monthCalendar1_DateChanged(object sender, System.Windows.Forms.DateRangeEventArgs e)
		{
			
		}

		private void UpdateSignatureInfo(Label l, MappedItem m) 
		{
			if (m.Signature != null) 
			{
				X509Certificate cert = m.Signature.GetCertificate();
				if (cert != null) 
				{
					l.Text = "Digitally signed on " + m.Signature.Timestamp;
					toolTip.SetToolTip(l, "Signed by " + cert.GetName());
				}
			}
			
		}

		private void EmployeeUpdated(MappedItem m) 
		{
			UpdateSignatureInfo(lblEmpSign,m);
		}

		private void SupervisorUpdated(MappedItem m) 
		{
			UpdateSignatureInfo(lblSupSign,m);
		}

		private void SetMappings() 
		{
			MappedItem m = b.GetMappedItem("Employee");
			m.MappingChanged += new MappedItem.MappingChangedEvent(EmployeeUpdated);
			m = b.GetMappedItem("Supervisor");
			m.MappingChanged += new MappedItem.MappingChangedEvent(SupervisorUpdated);
			b.RefreshMappings();
		}

		private void Sign() 
		{
			b.GenerateApproval(ConfigurationSettings.AppSettings["SigningCertificate"],false);
			if (UserDone != null) 
				UserDone.DynamicInvoke(null);
		}

		private void Supervisor_Click(object sender, System.EventArgs f) 
		{
			b.Mappings.Add(new MappedItem("BusinessOffice",t.EmployeeInformation.BusinessOffice,null));
			Sign();
		}

		private void Employee_Click(object sender, System.EventArgs f)
		{
			
/*
			if (t.EmployeeInformation.IDNum == null || t.EmployeeInformation.IDNum == "") 
			{
				MessageBox.Show("Please make sure you at least have your IDNumber on the request!","Oops!");
				return;
			}

			if (t.EmployeeInformation.HoursPerWeek == 0)
			{
				MessageBox.Show("Please make sure you at least have your hours on the request!","Oops!");
				return;
			}
*/			

			if (b == null)
				b = new BusinessRequest();

			b.BusinessObject = t;
			b.BusinessObject.FormName = FORMNAME;

			b.Mappings.Add(new MappedItem("Employee",null,null));
			b.Mappings.Add(new MappedItem("Supervisor",t.EmployeeInformation.Supervisor,null));
			
			SetMappings();

			b.Approvals.Required.Add(new Signature(t.EmployeeInformation.Supervisor));
			b.Approvals.Required.Add(new Signature(t.EmployeeInformation.BusinessOffice));
			Sign();			
			
/*
			

			EmployeeInfoProxy e = new EmployeeInfoProxy("http://localhost/BusinessObjectsService/BizTest.asmx");
			b = e.MakeRequest(b);			
*/			
			//b.Delegates = new String[] {t.EmployeeInformation.Supervisor, t.EmployeeInformation.BusinessOffice};
//			b.Delegates = new String[] {"MailServerA@securitylab.cis.upenn.edu"};
			// b.Approvals.Required.Add(new Signature("MailServerA@securitylab.cis.upenn.edu"));

/*
			if (b.VerifySignature((Signature)b.Approvals.Received[0]))
				MessageBox.Show("Partial verification succeeded.");
			else
				MessageBox.Show("Partial verification failed.");

			if (b.RecursivelyVerifySignature((Signature)b.Approvals.Received[0]))
				MessageBox.Show("Complete verification succeeded.");
			else
				MessageBox.Show("Complete verification failed.");
*/
/*			if (b2.VerifyAllSignatures())
				MessageBox.Show("All signatures are verified!");
			else
				MessageBox.Show("Failed to verify all signatures!");
*/

//			EmployeeInfoProxy e = new EmployeeInfoProxy("http://localhost/BusinessObjectsService/BizTest.asmx");
//			BusinessRequest b2 = e.MakeRequest(b);

//			b=b2;
//			Sign();			
		}

		private void btnApprove_Click(object sender, System.EventArgs e)
		{
			Sign();
		}
	}
}
