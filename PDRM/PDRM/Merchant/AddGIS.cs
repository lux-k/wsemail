using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Merchant
{
	/// <summary>
	/// Summary description for AddGIS.
	/// </summary>
	public class AddGIS : System.Windows.Forms.Form
	{
		/// <summary>
		/// The GIS Server object that we're adding
		/// </summary>
		protected GISServer m_gis_server;
		/// <summary>
		/// The GIS Server object that's being edited
		/// </summary>
		public GISServer Server
		{
			get { return m_gis_server; }
			set
			{
				m_gis_server = value;

				this.ReloadData();
			}
		}

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button bCancel;
		private System.Windows.Forms.Button bOk;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.TextBox tbLocation;
		private System.Windows.Forms.TextBox tbUrl;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AddGIS()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.m_gis_server = new GISServer();
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
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.tbName = new System.Windows.Forms.TextBox();
			this.tbUrl = new System.Windows.Forms.TextBox();
			this.tbLocation = new System.Windows.Forms.TextBox();
			this.bCancel = new System.Windows.Forms.Button();
			this.bOk = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Server Name:";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(8, 52);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(112, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Server IP Address:";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(8, 88);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "Server Location:";
			// 
			// tbName
			// 
			this.tbName.Location = new System.Drawing.Point(128, 16);
			this.tbName.Name = "tbName";
			this.tbName.Size = new System.Drawing.Size(128, 20);
			this.tbName.TabIndex = 3;
			this.tbName.Text = "";
			// 
			// tbUrl
			// 
			this.tbUrl.Location = new System.Drawing.Point(128, 52);
			this.tbUrl.Name = "tbUrl";
			this.tbUrl.Size = new System.Drawing.Size(128, 20);
			this.tbUrl.TabIndex = 4;
			this.tbUrl.Text = "";
			// 
			// tbLocation
			// 
			this.tbLocation.Location = new System.Drawing.Point(128, 88);
			this.tbLocation.Name = "tbLocation";
			this.tbLocation.Size = new System.Drawing.Size(128, 20);
			this.tbLocation.TabIndex = 5;
			this.tbLocation.Text = "";
			// 
			// bCancel
			// 
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(184, 120);
			this.bCancel.Name = "bCancel";
			this.bCancel.TabIndex = 6;
			this.bCancel.Text = "Cancel";
			this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
			// 
			// bOk
			// 
			this.bOk.Location = new System.Drawing.Point(88, 120);
			this.bOk.Name = "bOk";
			this.bOk.TabIndex = 7;
			this.bOk.Text = "Ok";
			this.bOk.Click += new System.EventHandler(this.bOk_Click);
			// 
			// AddGIS
			// 
			this.AcceptButton = this.bOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.bCancel;
			this.ClientSize = new System.Drawing.Size(272, 151);
			this.Controls.Add(this.bOk);
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.tbLocation);
			this.Controls.Add(this.tbUrl);
			this.Controls.Add(this.tbName);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AddGIS";
			this.Text = "New GIS Server";
			this.ResumeLayout(false);

		}
		#endregion

		private void bOk_Click(object sender, System.EventArgs e)
		{
			// save it and close the window
			this.m_gis_server.Name= this.tbName.Text;
			this.m_gis_server.Location = this.tbLocation.Text;
			this.m_gis_server.Url_String = this.tbUrl.Text;
			this.m_gis_server.Proxy = new Merchant.GIS.GISWse();
			this.m_gis_server.Proxy.Url = this.m_gis_server.Url_String;

			// tag on the tailing part of the url
			this.m_gis_server.Proxy.Url = this.m_gis_server.Proxy.Url + "//GIS//GIS.asmx";

			this.DialogResult = DialogResult.OK;

			// now close
			this.Close();
		}

		private void bCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;

			// close it
			this.Close();
		}

		/// <summary>
		/// Reload the data in the textboxes from the stored Server object
		/// </summary>
		private void ReloadData()
		{
			// set the text boxes appropriately
			this.tbName.Text = m_gis_server.Name;
			this.tbLocation.Text = m_gis_server.Location;
			this.tbUrl.Text = m_gis_server.Url_String;
		}
			
	}
}
