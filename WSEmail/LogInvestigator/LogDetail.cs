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
	/// Summary description for LogDetail.
	/// </summary>
	public class LogDetail : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox txtEntity;
		private System.Windows.Forms.TextBox txtDate;
		private System.Windows.Forms.TextBox txtLogType;
		private System.Windows.Forms.TextBox txtMessage;
		private System.Windows.Forms.TextBox txtXmlMessage;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Panel panel1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public LogDetail()
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtEntity = new System.Windows.Forms.TextBox();
			this.txtDate = new System.Windows.Forms.TextBox();
			this.txtLogType = new System.Windows.Forms.TextBox();
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.txtXmlMessage = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// txtEntity
			// 
			this.txtEntity.Location = new System.Drawing.Point(112, 8);
			this.txtEntity.Name = "txtEntity";
			this.txtEntity.ReadOnly = true;
			this.txtEntity.Size = new System.Drawing.Size(376, 20);
			this.txtEntity.TabIndex = 0;
			this.txtEntity.Text = "textBox1";
			// 
			// txtDate
			// 
			this.txtDate.Location = new System.Drawing.Point(112, 32);
			this.txtDate.Name = "txtDate";
			this.txtDate.ReadOnly = true;
			this.txtDate.Size = new System.Drawing.Size(376, 20);
			this.txtDate.TabIndex = 1;
			this.txtDate.Text = "textBox2";
			// 
			// txtLogType
			// 
			this.txtLogType.Location = new System.Drawing.Point(112, 56);
			this.txtLogType.Name = "txtLogType";
			this.txtLogType.ReadOnly = true;
			this.txtLogType.Size = new System.Drawing.Size(376, 20);
			this.txtLogType.TabIndex = 2;
			this.txtLogType.Text = "textBox3";
			// 
			// txtMessage
			// 
			this.txtMessage.Location = new System.Drawing.Point(112, 80);
			this.txtMessage.Multiline = true;
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.ReadOnly = true;
			this.txtMessage.Size = new System.Drawing.Size(376, 96);
			this.txtMessage.TabIndex = 3;
			this.txtMessage.Text = "textBox3";
			// 
			// txtXmlMessage
			// 
			this.txtXmlMessage.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtXmlMessage.Location = new System.Drawing.Point(112, 176);
			this.txtXmlMessage.Multiline = true;
			this.txtXmlMessage.Name = "txtXmlMessage";
			this.txtXmlMessage.ReadOnly = true;
			this.txtXmlMessage.Size = new System.Drawing.Size(376, 304);
			this.txtXmlMessage.TabIndex = 4;
			this.txtXmlMessage.Text = "textBox3";
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.TabIndex = 5;
			this.label1.Text = "Entity:";
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.TabIndex = 6;
			this.label2.Text = "Date:";
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(8, 56);
			this.label3.Name = "label3";
			this.label3.TabIndex = 7;
			this.label3.Text = "Log Type";
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(8, 80);
			this.label4.Name = "label4";
			this.label4.TabIndex = 8;
			this.label4.Text = "Message:";
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(8, 184);
			this.label5.Name = "label5";
			this.label5.TabIndex = 9;
			this.label5.Text = "XML Data:";
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left);
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(112, 488);
			this.panel1.TabIndex = 10;
			// 
			// LogDetail
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(496, 485);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label5,
																		  this.label4,
																		  this.label3,
																		  this.label2,
																		  this.label1,
																		  this.txtXmlMessage,
																		  this.txtMessage,
																		  this.txtLogType,
																		  this.txtDate,
																		  this.txtEntity,
																		  this.panel1});
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LogDetail";
			this.ShowInTaskbar = false;
			this.Text = "LogDetail";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
