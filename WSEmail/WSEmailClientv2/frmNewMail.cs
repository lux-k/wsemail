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
using System.Threading;

namespace WSEmailClientv2
{
	/// <summary>
	/// Summary description for NewMail.
	/// </summary>
	public class NewMail : System.Windows.Forms.Form, IDisposable
	{
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		Thread thread;
		private System.Windows.Forms.Label label2;
		System.Random random;
		Form messages = null;

		public NewMail(Form f)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.messages = f;
			this.TopMost = true;
			this.TopLevel = true;
			random = new Random();
			thread = new Thread(new ThreadStart(ThreadCode));
			thread.Start();
		}

		~NewMail() 
		{
			Dispose();
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
			try 
			{
				thread.Resume();
				thread.Abort();
			} 
			catch (Exception) {}
			GC.SuppressFinalize(this);
			base.Dispose( disposing );
		}

		private void ThreadCode() 
		{
			while (true) 
			{
				this.Hide();
				this.Left = random.Next(SystemInformation.PrimaryMonitorSize.Width-this.Width);
				this.Top = random.Next(SystemInformation.PrimaryMonitorSize.Height-this.Height);
				this.Show();
				this.BringToFront();

				Thread.Sleep(random.Next(10) * 1000);
			}
		}
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(NewMail));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.White;
			this.label1.Image = ((System.Drawing.Bitmap)(resources.GetObject("label1.Image")));
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 40);
			this.label1.TabIndex = 0;
			this.label1.Click += new System.EventHandler(this.Clicked);
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Black;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(40, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 40);
			this.label2.TabIndex = 1;
			this.label2.Text = "New WSEmail has arrived!";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label2.Click += new System.EventHandler(this.Clicked);
			// 
			// NewMail
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(128, 40);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label2,
																		  this.label1});
			this.Name = "NewMail";
			this.ShowInTaskbar = false;
			this.VisibleChanged += new System.EventHandler(this.DoIt);
			this.ResumeLayout(false);

		}
		#endregion

		private void Clicked(object sender, System.EventArgs e)
		{
			if (messages.WindowState == FormWindowState.Minimized)
				messages.WindowState = FormWindowState.Maximized;
			messages.Show();
			messages.BringToFront();
			this.Hide();
			thread.Suspend();
			this.Dispose();
		}

		private void DoIt(object sender, System.EventArgs e)
		{
			if ((thread.ThreadState & ThreadState.Suspended) == ThreadState.Suspended) 
			{
				thread.Resume();
			}
		}
	}
}
