using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Merchant
{
	/// <summary>
	/// Summary description for XMLDisplay.
	/// </summary>
	public class XMLDisplay : System.Windows.Forms.Form
	{
		/// <summary>
		/// The document that we are displaying
		/// </summary>
		protected System.Xml.XmlDocument m_doc;

		/// <summary>
		/// The document to display
		/// </summary>
		public System.Xml.XmlDocument Doc
		{
			get { return m_doc; }
			set
			{
				m_doc = value;

				// reload
				ReloadData();
			}
		}

		private System.Windows.Forms.TextBox tbText;
		private System.Windows.Forms.Button bOk;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public XMLDisplay()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// default initialize
			m_doc = new System.Xml.XmlDocument();
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
			this.tbText = new System.Windows.Forms.TextBox();
			this.bOk = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tbText
			// 
			this.tbText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tbText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tbText.Location = new System.Drawing.Point(8, 8);
			this.tbText.Multiline = true;
			this.tbText.Name = "tbText";
			this.tbText.ReadOnly = true;
			this.tbText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbText.Size = new System.Drawing.Size(272, 248);
			this.tbText.TabIndex = 0;
			this.tbText.Text = "";
			// 
			// bOk
			// 
			this.bOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bOk.Location = new System.Drawing.Point(205, 272);
			this.bOk.Name = "bOk";
			this.bOk.TabIndex = 1;
			this.bOk.Text = "OK";
			this.bOk.Click += new System.EventHandler(this.bOk_Click);
			// 
			// XMLDisplay
			// 
			this.AcceptButton = this.bOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 301);
			this.Controls.Add(this.bOk);
			this.Controls.Add(this.tbText);
			this.Name = "XMLDisplay";
			this.Text = "XML Source";
			this.ResumeLayout(false);

		}
		#endregion

		private void bOk_Click(object sender, System.EventArgs e)
		{
			// leave
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void ReloadData()
		{
			// reload the text window to have the text from the document in it
			//this.tbText.Text = 
		}


	}
}
