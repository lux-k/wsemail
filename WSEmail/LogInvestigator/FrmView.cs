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

namespace LogInvestigator
{
	/// <summary>
	/// Summary description for FrmView.
	/// </summary>
	public class FrmView : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel viewPanel;
		private System.Windows.Forms.RichTextBox txtMessage;
		private System.Windows.Forms.TextBox txtXmlMessage;
		private System.Windows.Forms.TextBox txtLogType;
		private System.Windows.Forms.TextBox txtDate;
		private System.Windows.Forms.TextBox txtEntity;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private Entry _e;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private bool onTop = false;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrmView()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

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
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		public Entry LogEntry 
		{
			set 
			{
				_e = value;
				this.txtDate.Text = _e.Date;
				this.txtMessage.Text = _e.Message;
				this.txtEntity.Text = _e.Entity;
				this.txtXmlMessage.Text = _e.XmlMessage;
				this.txtLogType.Text = _e.LogType;
			}
			get 
			{
				return _e;
			}
		}

				

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.viewPanel = new System.Windows.Forms.Panel();
			this.txtMessage = new System.Windows.Forms.RichTextBox();
			this.txtXmlMessage = new System.Windows.Forms.TextBox();
			this.txtLogType = new System.Windows.Forms.TextBox();
			this.txtDate = new System.Windows.Forms.TextBox();
			this.txtEntity = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.viewPanel.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// viewPanel
			// 
			this.viewPanel.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.viewPanel.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.viewPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.txtMessage,
																					this.txtXmlMessage,
																					this.txtLogType,
																					this.txtDate,
																					this.txtEntity,
																					this.panel1});
			this.viewPanel.Name = "viewPanel";
			this.viewPanel.Size = new System.Drawing.Size(440, 504);
			this.viewPanel.TabIndex = 6;
			// 
			// txtMessage
			// 
			this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtMessage.Location = new System.Drawing.Point(72, 80);
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.ReadOnly = true;
			this.txtMessage.Size = new System.Drawing.Size(288, 96);
			this.txtMessage.TabIndex = 22;
			this.txtMessage.Text = "";
			// 
			// txtXmlMessage
			// 
			this.txtXmlMessage.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtXmlMessage.BackColor = System.Drawing.Color.White;
			this.txtXmlMessage.Location = new System.Drawing.Point(72, 176);
			this.txtXmlMessage.Multiline = true;
			this.txtXmlMessage.Name = "txtXmlMessage";
			this.txtXmlMessage.ReadOnly = true;
			this.txtXmlMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtXmlMessage.Size = new System.Drawing.Size(352, 328);
			this.txtXmlMessage.TabIndex = 15;
			this.txtXmlMessage.Text = "";
			// 
			// txtLogType
			// 
			this.txtLogType.BackColor = System.Drawing.Color.White;
			this.txtLogType.Location = new System.Drawing.Point(72, 56);
			this.txtLogType.Name = "txtLogType";
			this.txtLogType.ReadOnly = true;
			this.txtLogType.Size = new System.Drawing.Size(288, 20);
			this.txtLogType.TabIndex = 13;
			this.txtLogType.Text = "";
			// 
			// txtDate
			// 
			this.txtDate.BackColor = System.Drawing.Color.White;
			this.txtDate.Location = new System.Drawing.Point(72, 32);
			this.txtDate.Name = "txtDate";
			this.txtDate.ReadOnly = true;
			this.txtDate.Size = new System.Drawing.Size(288, 20);
			this.txtDate.TabIndex = 12;
			this.txtDate.Text = "";
			// 
			// txtEntity
			// 
			this.txtEntity.BackColor = System.Drawing.Color.White;
			this.txtEntity.Location = new System.Drawing.Point(72, 8);
			this.txtEntity.Name = "txtEntity";
			this.txtEntity.ReadOnly = true;
			this.txtEntity.Size = new System.Drawing.Size(288, 20);
			this.txtEntity.TabIndex = 11;
			this.txtEntity.Text = "";
			// 
			// panel1
			// 
			this.panel1.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.label4,
																				 this.label5,
																				 this.label3,
																				 this.label2,
																				 this.label1});
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(136, 504);
			this.panel1.TabIndex = 21;
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(0, 88);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 23);
			this.label4.TabIndex = 19;
			this.label4.Text = "Message:";
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(0, 184);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(72, 23);
			this.label5.TabIndex = 20;
			this.label5.Text = "XML Data:";
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(0, 64);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 23);
			this.label3.TabIndex = 18;
			this.label3.Text = "Log Type:";
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(0, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 23);
			this.label2.TabIndex = 17;
			this.label2.Text = "Date:";
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(0, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 23);
			this.label1.TabIndex = 16;
			this.label1.Text = "Entity:";
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem2,
																					  this.menuItem3});
			this.menuItem1.Text = "File";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.Text = "Always On Top";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.Text = "Close";
			this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
			// 
			// FrmView
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(432, 501);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.viewPanel});
			this.Menu = this.mainMenu1;
			this.Name = "FrmView";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Log Detail";
			this.viewPanel.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			this.Hide();
			this.Close();
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			onTop = !onTop;
			menuItem2.Checked = onTop;
			this.TopMost = onTop;
			
		}
	}
}
