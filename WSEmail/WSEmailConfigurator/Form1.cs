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
using System.Data;

namespace WSEmailConfigurator
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		string cookie;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button3;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
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
				if (components != null) 
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.button3 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(248, 248);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(120, 40);
			this.button1.TabIndex = 0;
			this.button1.Text = "Configure Server";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(248, 312);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(120, 40);
			this.button2.TabIndex = 1;
			this.button2.Text = "Configure Client";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textBox1.Location = new System.Drawing.Point(80, 0);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(432, 224);
			this.textBox1.TabIndex = 2;
			this.textBox1.Text = "This program will help you setup the basic XML configuration files for the variou" +
				"s program associated with WS Email.\r\n\r\nAdvanced options and all plugin configura" +
				"tion will still need to be done manually.";
			this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
			this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.White;
			this.label1.Image = ((System.Drawing.Bitmap)(resources.GetObject("label1.Image")));
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 440);
			this.label1.TabIndex = 3;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(248, 376);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(120, 40);
			this.button3.TabIndex = 4;
			this.button3.Text = "Configure Router";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(512, 429);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.button3,
																		  this.label1,
																		  this.textBox1,
																		  this.button2,
																		  this.button1});
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "WS Email Configuration Utility";
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			WSServerConfiguration s = new WSServerConfiguration();
			s.Configure();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			WSClientConfiguration s = new WSClientConfiguration();
			s.Configure();
		
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			WSRouterConfiguration s = new WSRouterConfiguration();
			s.Configure();
		}

		private void Form1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
		}

		private void textBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			cookie += e.KeyData;
			if (cookie.Equals("KEVIN")) 
			{
				label1.Image = Image.FromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType().Namespace + ".caesar.gif"));
				cookie = "";
			}
		}

	}
}
