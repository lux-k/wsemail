using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace AdViewer
{
	/// <summary>
	/// Summary description for GLSInfo.
	/// </summary>
	public class GLSInfo : System.Windows.Forms.Form
	{
		/// <summary>
		/// IP address for the GLS
		/// </summary>
		protected System.Uri m_gls_ip;

		/// <summary>
		/// IP address for the GIS that we got from the GLS
		/// </summary>
		protected System.Uri m_gis_ip;

		/// <summary>
		/// Physical location information gotten from the GLS
		/// </summary>
		protected string m_gls_loc;

		#region Properties
		/// <summary>
		/// Get or set the GLS IP address
		/// </summary>
		public System.Uri GLS_IP
		{
			get { return this.m_gls_ip; }
			set
			{
				m_gls_ip = value;

				// reload the data
				this.ReloadData();
			}
		}

		/// <summary>
		/// Get or set the GIS IP address
		/// </summary>
		public System.Uri GIS_IP
		{
			get { return this.m_gis_ip; }
			set
			{
				this.m_gis_ip = value;
				
				// reload the data
				this.ReloadData();			
			}
		}

		/// <summary>
		/// Get or set the GLS physical location information
		/// </summary>
		public string GLS_LOC
		{
			get { return this.m_gls_loc; }
			set
			{
				m_gls_loc = value;

				// reload the data
				this.ReloadData();
			}
		}
		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbGLSip;
		private System.Windows.Forms.TextBox tbGLSloc;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbGISip;
	
		public GLSInfo()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// some default values to be set later
			this.m_gis_ip = new Uri("localhost");
			this.m_gls_ip = new Uri("localhost");
			this.m_gls_loc = "";

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
			this.tbGLSip = new System.Windows.Forms.TextBox();
			this.tbGLSloc = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tbGISip = new System.Windows.Forms.TextBox();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Size = new System.Drawing.Size(88, 16);
			this.label1.Text = "GLS IP Address";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 56);
			this.label2.Size = new System.Drawing.Size(88, 16);
			this.label2.Text = "GLS Location";
			// 
			// tbGLSip
			// 
			this.tbGLSip.Location = new System.Drawing.Point(104, 8);
			this.tbGLSip.ReadOnly = true;
			this.tbGLSip.Text = "";
			// 
			// tbGLSloc
			// 
			this.tbGLSloc.Location = new System.Drawing.Point(104, 56);
			this.tbGLSloc.ReadOnly = true;
			this.tbGLSloc.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 104);
			this.label3.Size = new System.Drawing.Size(88, 16);
			this.label3.Text = "GIS Address";
			// 
			// tbGISip
			// 
			this.tbGISip.Location = new System.Drawing.Point(104, 104);
			this.tbGISip.ReadOnly = true;
			this.tbGISip.Text = "";
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			// 
			// GLSInfo
			// 
			this.ClientSize = new System.Drawing.Size(218, 143);
			this.Controls.Add(this.tbGISip);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.tbGLSloc);
			this.Controls.Add(this.tbGLSip);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Text = "About the current GLS";
			this.Load += new System.EventHandler(this.GLSInfo_Load);

		}
		#endregion

		/// <summary>
		/// Reload the data to the textboxes from the saved data
		/// </summary>
		private void ReloadData()
		{
			// reload everything from the saved data
			this.tbGISip.Text = this.m_gis_ip.Host;
			this.tbGLSip.Text = this.m_gls_ip.Host;
			this.tbGLSloc.Text = this.m_gls_loc;
		}

		private void GLSInfo_Load(object sender, System.EventArgs e)
		{
			// reload the data on showing the form
			this.ReloadData();
		}
	}
}
