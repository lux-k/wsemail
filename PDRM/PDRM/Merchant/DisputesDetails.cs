using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Merchant
{
	/// <summary>
	/// Summary description for DisputesDetails.
	/// </summary>
	public class DisputesDetails : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox tbService;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox cbCorrect;
		private System.Windows.Forms.CheckBox cbMoney;
		private System.Windows.Forms.Button bOk;
		private System.Windows.Forms.Button bCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.RadioButton rCourt;
		private System.Windows.Forms.RadioButton rIndependent;
		private System.Windows.Forms.RadioButton rLaw;
		private System.Windows.Forms.RadioButton rService;
		private System.Windows.Forms.CheckBox cbLaw;
		private System.Windows.Forms.GroupBox ResGroup;
		private System.Windows.Forms.GroupBox RemGroup;

		/// <summary>
		/// The P3P Disputes item that we are examining or modifying
		/// </summary>
		protected P3P.DISPUTES m_disputes;
		public P3P.DISPUTES DisputesItem
		{
			get { return m_disputes; }
			set
			{
				m_disputes = value;

				// fix the window display to match the new data
				UpdateDetails();
			}
		}

		public DisputesDetails()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// initialize the disputes element
			this.m_disputes = new P3P.DISPUTES();			
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
			this.tbService = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.ResGroup = new System.Windows.Forms.GroupBox();
			this.RemGroup = new System.Windows.Forms.GroupBox();
			this.cbCorrect = new System.Windows.Forms.CheckBox();
			this.cbLaw = new System.Windows.Forms.CheckBox();
			this.cbMoney = new System.Windows.Forms.CheckBox();
			this.bOk = new System.Windows.Forms.Button();
			this.bCancel = new System.Windows.Forms.Button();
			this.rCourt = new System.Windows.Forms.RadioButton();
			this.rIndependent = new System.Windows.Forms.RadioButton();
			this.rLaw = new System.Windows.Forms.RadioButton();
			this.rService = new System.Windows.Forms.RadioButton();
			this.ResGroup.SuspendLayout();
			this.RemGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// tbService
			// 
			this.tbService.Location = new System.Drawing.Point(80, 16);
			this.tbService.Name = "tbService";
			this.tbService.Size = new System.Drawing.Size(200, 20);
			this.tbService.TabIndex = 0;
			this.tbService.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Service";
			// 
			// ResGroup
			// 
			this.ResGroup.Controls.Add(this.rService);
			this.ResGroup.Controls.Add(this.rLaw);
			this.ResGroup.Controls.Add(this.rIndependent);
			this.ResGroup.Controls.Add(this.rCourt);
			this.ResGroup.Location = new System.Drawing.Point(16, 48);
			this.ResGroup.Name = "ResGroup";
			this.ResGroup.Size = new System.Drawing.Size(264, 80);
			this.ResGroup.TabIndex = 2;
			this.ResGroup.TabStop = false;
			this.ResGroup.Text = "Resolution Type";
			// 
			// RemGroup
			// 
			this.RemGroup.Controls.Add(this.cbMoney);
			this.RemGroup.Controls.Add(this.cbLaw);
			this.RemGroup.Controls.Add(this.cbCorrect);
			this.RemGroup.Location = new System.Drawing.Point(16, 144);
			this.RemGroup.Name = "RemGroup";
			this.RemGroup.Size = new System.Drawing.Size(264, 48);
			this.RemGroup.TabIndex = 3;
			this.RemGroup.TabStop = false;
			this.RemGroup.Text = "Remedies";
			// 
			// cbCorrect
			// 
			this.cbCorrect.Location = new System.Drawing.Point(16, 24);
			this.cbCorrect.Name = "cbCorrect";
			this.cbCorrect.Size = new System.Drawing.Size(64, 16);
			this.cbCorrect.TabIndex = 0;
			this.cbCorrect.Text = "Correct";
			// 
			// cbLaw
			// 
			this.cbLaw.Location = new System.Drawing.Point(104, 24);
			this.cbLaw.Name = "cbLaw";
			this.cbLaw.Size = new System.Drawing.Size(64, 16);
			this.cbLaw.TabIndex = 1;
			this.cbLaw.Text = "Law";
			// 
			// cbMoney
			// 
			this.cbMoney.Location = new System.Drawing.Point(192, 24);
			this.cbMoney.Name = "cbMoney";
			this.cbMoney.Size = new System.Drawing.Size(64, 16);
			this.cbMoney.TabIndex = 2;
			this.cbMoney.Text = "Money";
			// 
			// bOk
			// 
			this.bOk.Location = new System.Drawing.Point(120, 208);
			this.bOk.Name = "bOk";
			this.bOk.TabIndex = 4;
			this.bOk.Text = "Ok";
			this.bOk.Click += new System.EventHandler(this.bOk_Click);
			// 
			// bCancel
			// 
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(208, 208);
			this.bCancel.Name = "bCancel";
			this.bCancel.TabIndex = 5;
			this.bCancel.Text = "Cancel";
			// 
			// rCourt
			// 
			this.rCourt.Location = new System.Drawing.Point(16, 24);
			this.rCourt.Name = "rCourt";
			this.rCourt.Size = new System.Drawing.Size(88, 16);
			this.rCourt.TabIndex = 4;
			this.rCourt.Text = "Court";
			// 
			// rIndependent
			// 
			this.rIndependent.Location = new System.Drawing.Point(16, 56);
			this.rIndependent.Name = "rIndependent";
			this.rIndependent.Size = new System.Drawing.Size(88, 16);
			this.rIndependent.TabIndex = 5;
			this.rIndependent.Text = "Independent";
			// 
			// rLaw
			// 
			this.rLaw.Location = new System.Drawing.Point(144, 24);
			this.rLaw.Name = "rLaw";
			this.rLaw.Size = new System.Drawing.Size(88, 16);
			this.rLaw.TabIndex = 6;
			this.rLaw.Text = "Law";
			// 
			// rService
			// 
			this.rService.Location = new System.Drawing.Point(144, 56);
			this.rService.Name = "rService";
			this.rService.Size = new System.Drawing.Size(88, 16);
			this.rService.TabIndex = 7;
			this.rService.Text = "Service";
			// 
			// DisputesDetails
			// 
			this.AcceptButton = this.bOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.bCancel;
			this.ClientSize = new System.Drawing.Size(292, 237);
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.bOk);
			this.Controls.Add(this.RemGroup);
			this.Controls.Add(this.ResGroup);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbService);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DisputesDetails";
			this.Text = "Disputes Item";
			this.Load += new System.EventHandler(this.DisputesDetails_Load);
			this.ResGroup.ResumeLayout(false);
			this.RemGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void DisputesDetails_Load(object sender, System.EventArgs e)
		{
			// upadate all of the things in the window
			UpdateDetails();		
		}

		/// <summary>
		/// Take the data in the Disputes item and load it into the detail
		/// box
		/// </summary>
		private void UpdateDetails()
		{
			// the service description
			this.tbService.Text = this.m_disputes.service;

			// the resolution type
			switch ( this.m_disputes.resolution_type)
			{
				case P3P.RESOLUTION_TYPE.COURT:
					this.rCourt.Checked = true;
					break;

				case P3P.RESOLUTION_TYPE.INDEPENDENT:
					this.rIndependent.Checked = true;
					break;

				case P3P.RESOLUTION_TYPE.LAW:
					this.rLaw.Checked = true;
					break;

				case P3P.RESOLUTION_TYPE.SERVICE:
					this.rService.Checked = true;
					break;
			}

			// the remedy options
			this.cbCorrect.Checked = this.m_disputes.remedies.correct;
			this.cbMoney.Checked = this.m_disputes.remedies.money;
			this.cbLaw.Checked = this.m_disputes.remedies.law;
		}

		private void bOk_Click(object sender, System.EventArgs e)
		{
			// save the data as shown in the form into our Disputes item
			
			// the service description
			this.m_disputes.service = this.tbService.Text;

			// the resolution type
			if (this.rCourt.Checked == true)
			{
				this.m_disputes.resolution_type = P3P.RESOLUTION_TYPE.COURT;
			}
			else if (this.rIndependent.Checked == true)
			{
				this.m_disputes.resolution_type = P3P.RESOLUTION_TYPE.INDEPENDENT;
			}
			else if (this.rLaw.Checked == true)
			{
				this.m_disputes.resolution_type = P3P.RESOLUTION_TYPE.LAW;
			}
			else // this.rService.Checked == true
			{
				this.m_disputes.resolution_type = P3P.RESOLUTION_TYPE.SERVICE;
			}				

			// the remedy options
			this.m_disputes.remedies.correct = this.cbCorrect.Checked;
			this.m_disputes.remedies.money = this.cbMoney.Checked;
			this.m_disputes.remedies.law = this.cbLaw.Checked;

			// set the result type to OK
			this.DialogResult = DialogResult.OK;
			
			// leave
			this.Close();
		}
	}
}
