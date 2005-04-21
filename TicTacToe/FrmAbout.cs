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

namespace TicTacToe
{
	/// <summary>
	/// Summary description for FrmAbout.
	/// </summary>
	public class FrmAbout : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.RichTextBox rtBlurb;
		private System.Windows.Forms.Button button1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrmAbout()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.rtBlurb.Rtf= @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fswiss\fcharset0 Arial;}}\viewkind4\uc1\pard\qc\f0\fs20\par Network Tic-Tac-Toe " + GetVersion() +
				@" \par\par by Kevin Lux\par mailto:luxk@saul.cis.upenn.edu\par and David Birtwell\par mailto:birtwell@seas.upenn.edu" + 
				@"\par\par Built for the course project in CIS 505 / Distributed Systems,\par University of Pennsylvania.}";
			

		}

		private string GetVersion() 
		{
			return "v" + Application.ProductVersion;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FrmAbout));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.rtBlurb = new System.Windows.Forms.RichTextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(200, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(32, 32);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			// 
			// rtBlurb
			// 
			this.rtBlurb.BackColor = System.Drawing.SystemColors.Control;
			this.rtBlurb.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtBlurb.Location = new System.Drawing.Point(0, 48);
			this.rtBlurb.Name = "rtBlurb";
			this.rtBlurb.ReadOnly = true;
			this.rtBlurb.Size = new System.Drawing.Size(440, 168);
			this.rtBlurb.TabIndex = 2;
			this.rtBlurb.Text = "richTextBox1";
			this.rtBlurb.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtBlurb_LinkClicked);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(192, 224);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(56, 32);
			this.button1.TabIndex = 3;
			this.button1.Text = "OK";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// FrmAbout
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(440, 267);
			this.ControlBox = false;
			this.Controls.Add(this.button1);
			this.Controls.Add(this.rtBlurb);
			this.Controls.Add(this.pictureBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmAbout";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "About Network Tic-Tac-Toe";
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void rtBlurb_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(e.LinkText);
		}
	}
}
