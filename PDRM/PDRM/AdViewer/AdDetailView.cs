using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace AdViewer
{
	/// <summary>
	/// A message box to show the details of a particular AdMessage
	/// </summary>
	public class AdDetailView : System.Windows.Forms.Form
	{
		/// <summary>
		/// The Ad that we are displaying the details of
		/// </summary>
		protected CommonTypes.AdMessage m_ad;

		/// <summary>
		/// The Ad that we are displaying te details of
		/// </summary>
		public CommonTypes.AdMessage Ad
		{
			get { return m_ad; }
			set
			{
				m_ad = value;

				// reload the data to the form
				this.ReloadData();
			}
		}

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tbSource;
		private System.Windows.Forms.TextBox tbTime;
		private System.Windows.Forms.TextBox tbType;
		private System.Windows.Forms.TextBox tbText;
		private System.Windows.Forms.Button bOk;
	
		public AdDetailView()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// initialize, but set later
			this.m_ad = new CommonTypes.AdMessage();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
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
			this.tbSource = new System.Windows.Forms.TextBox();
			this.tbTime = new System.Windows.Forms.TextBox();
			this.tbType = new System.Windows.Forms.TextBox();
			this.tbText = new System.Windows.Forms.TextBox();
			this.bOk = new System.Windows.Forms.Button();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 8);
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.Text = "Source:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 50);
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.Text = "Time Stamp:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 92);
			this.label3.Size = new System.Drawing.Size(72, 16);
			this.label3.Text = "Type:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 134);
			this.label4.Size = new System.Drawing.Size(72, 16);
			this.label4.Text = "Text:";
			// 
			// tbSource
			// 
			this.tbSource.Location = new System.Drawing.Point(104, 8);
			this.tbSource.ReadOnly = true;
			this.tbSource.Text = "";
			// 
			// tbTime
			// 
			this.tbTime.Location = new System.Drawing.Point(104, 50);
			this.tbTime.ReadOnly = true;
			this.tbTime.Text = "";
			// 
			// tbType
			// 
			this.tbType.Location = new System.Drawing.Point(104, 92);
			this.tbType.ReadOnly = true;
			this.tbType.Text = "";
			// 
			// tbText
			// 
			this.tbText.Location = new System.Drawing.Point(16, 160);
			this.tbText.ReadOnly = true;
			this.tbText.Size = new System.Drawing.Size(208, 22);
			this.tbText.Text = "";
			// 
			// bOk
			// 
			this.bOk.Location = new System.Drawing.Point(80, 192);
			this.bOk.Text = "Ok";
			this.bOk.Click += new System.EventHandler(this.bOk_Click);
			// 
			// AdDetailView
			// 
			this.ClientSize = new System.Drawing.Size(234, 223);
			this.Controls.Add(this.bOk);
			this.Controls.Add(this.tbText);
			this.Controls.Add(this.tbType);
			this.Controls.Add(this.tbTime);
			this.Controls.Add(this.tbSource);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Text = "Ad Detail";
			this.Load += new System.EventHandler(this.AdDetailView_Load);

		}
		#endregion

		/// <summary>
		/// Reload the data to the window from the stored AdMesssage
		/// </summary>
		private void ReloadData()
		{
			// reload the text boxes on the window with the new ad
			this.tbSource.Text = m_ad.Source;
			this.tbText.Text = m_ad.AdText;
			this.tbTime.Text = m_ad.Time.ToString();
			this.tbType.Text = m_ad.TypeString;
		}

		private void bOk_Click(object sender, System.EventArgs e)
		{
			// close up shop
			this.Close();
		}

		private void AdDetailView_Load(object sender, System.EventArgs e)
		{
			// reload the data
			this.ReloadData();
		}
	}
}
