using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Merchant
{
	/// <summary>
	/// Summary description for CreateAd.
	/// </summary>
	public class CreateAd : System.Windows.Forms.Form
	{
		/// <summary>
		/// The new Ad that we have created
		/// </summary>
		protected GIS.AdMessage m_ad;

		/// <summary>
		/// The new Ad that we have created
		/// </summary>
		public GIS.AdMessage Ad
		{
			get { return m_ad; }
			set
			{
				m_ad = value;

				// reload
				ReloadData();
			}
		}

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbAdText;
		private System.Windows.Forms.GroupBox gAdType;
		private System.Windows.Forms.RadioButton rbNormalAd;
		private System.Windows.Forms.RadioButton rbOffensiveAd;
		private System.Windows.Forms.RadioButton rbDiscount;
		private System.Windows.Forms.Button bOk;
		private System.Windows.Forms.Button bCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CreateAd()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			m_ad = new GIS.AdMessage();
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
			this.tbAdText = new System.Windows.Forms.TextBox();
			this.gAdType = new System.Windows.Forms.GroupBox();
			this.rbDiscount = new System.Windows.Forms.RadioButton();
			this.rbOffensiveAd = new System.Windows.Forms.RadioButton();
			this.rbNormalAd = new System.Windows.Forms.RadioButton();
			this.bOk = new System.Windows.Forms.Button();
			this.bCancel = new System.Windows.Forms.Button();
			this.gAdType.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Ad text:";
			// 
			// tbAdText
			// 
			this.tbAdText.Location = new System.Drawing.Point(72, 8);
			this.tbAdText.Name = "tbAdText";
			this.tbAdText.Size = new System.Drawing.Size(496, 20);
			this.tbAdText.TabIndex = 1;
			this.tbAdText.Text = "";
			// 
			// gAdType
			// 
			this.gAdType.Controls.Add(this.rbDiscount);
			this.gAdType.Controls.Add(this.rbOffensiveAd);
			this.gAdType.Controls.Add(this.rbNormalAd);
			this.gAdType.Location = new System.Drawing.Point(8, 48);
			this.gAdType.Name = "gAdType";
			this.gAdType.Size = new System.Drawing.Size(392, 56);
			this.gAdType.TabIndex = 2;
			this.gAdType.TabStop = false;
			this.gAdType.Text = "Ad Type";
			// 
			// rbDiscount
			// 
			this.rbDiscount.Location = new System.Drawing.Point(280, 24);
			this.rbDiscount.Name = "rbDiscount";
			this.rbDiscount.TabIndex = 2;
			this.rbDiscount.Text = "Discount";
			// 
			// rbOffensiveAd
			// 
			this.rbOffensiveAd.Location = new System.Drawing.Point(152, 24);
			this.rbOffensiveAd.Name = "rbOffensiveAd";
			this.rbOffensiveAd.TabIndex = 1;
			this.rbOffensiveAd.Text = "Offensive Ad";
			// 
			// rbNormalAd
			// 
			this.rbNormalAd.Location = new System.Drawing.Point(24, 24);
			this.rbNormalAd.Name = "rbNormalAd";
			this.rbNormalAd.TabIndex = 0;
			this.rbNormalAd.Text = "Normal Ad";
			// 
			// bOk
			// 
			this.bOk.Location = new System.Drawing.Point(496, 40);
			this.bOk.Name = "bOk";
			this.bOk.TabIndex = 3;
			this.bOk.Text = "Ok";
			this.bOk.Click += new System.EventHandler(this.bOk_Click);
			// 
			// bCancel
			// 
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(496, 80);
			this.bCancel.Name = "bCancel";
			this.bCancel.TabIndex = 4;
			this.bCancel.Text = "Cancel";
			this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
			// 
			// CreateAd
			// 
			this.AcceptButton = this.bOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.bCancel;
			this.ClientSize = new System.Drawing.Size(578, 117);
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.bOk);
			this.Controls.Add(this.gAdType);
			this.Controls.Add(this.tbAdText);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CreateAd";
			this.Text = "Create a new Ad";
			this.Load += new System.EventHandler(this.CreateAd_Load);
			this.gAdType.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Reloads the data to the window from the stored AdMessage object
		/// </summary>
		private void ReloadData()
		{
			// reload the window from the local state
			tbAdText.Text = m_ad.AdText;

			switch (m_ad.Type )
			{
				case GIS.AdType.Discount:
					rbDiscount.Checked = true;
					break;

				case GIS.AdType.NormalAd:
					rbNormalAd.Checked = true;
					break;

				case GIS.AdType.OffensiveAd:
					rbOffensiveAd.Checked = true;
					break;
			}

			// done;
		}

		private void bOk_Click(object sender, System.EventArgs e)
		{
			// grab the values in the window and close up
			m_ad.AdText = tbAdText.Text;
			if ( this.rbDiscount.Checked )
			{
				m_ad.Type = GIS.AdType.Discount;
			}
			else if ( this.rbNormalAd.Checked )
			{
				m_ad.Type = GIS.AdType.NormalAd;
			}
			else if ( this.rbOffensiveAd.Checked )
			{
				m_ad.Type = GIS.AdType.OffensiveAd;
			}

			// set the ad source to be us
			m_ad.Source = "Security Lab Server";

			// ok
			this.DialogResult = DialogResult.OK;

			// close up shop
			this.Close();
		}

		private void CreateAd_Load(object sender, System.EventArgs e)
		{
			// reload the data
			ReloadData();
		}

		private void bCancel_Click(object sender, System.EventArgs e)
		{
			// cancel
			this.DialogResult = DialogResult.Cancel;

			this.Close();
		}
	}
}
