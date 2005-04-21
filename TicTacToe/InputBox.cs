/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Drawing;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;


namespace TicTacToe 
{
	public class InputBox : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		private System.ComponentModel.Container components = null;

		private InputBox(string s)
		{
			InitializeComponent();
			label1.Text = s;
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			} base.Dispose( disposing );
		}

		private void InitializeComponent()
		{
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(16, 48);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(256, 20);
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "";
			this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(256, 40);
			this.label1.TabIndex = 1;
			this.label1.Text = "label1";
			// 
			// InputBox
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(282, 95);
			this.ControlBox = false;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "InputBox";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Input...";
			this.ResumeLayout(false);

		}

		private void textBox1_KeyDown(object sender, 
			System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
				this.Close();
		}

		public static string ShowInputBox(string s)
		{
			InputBox box = new InputBox(s);
			box.ShowDialog();
			return box.textBox1.Text;
		}

		public static string ShowInputBox(string s,string i)
		{
			InputBox box = new InputBox(s);
			box.textBox1.Text = i;
			box.ShowDialog();
			return box.textBox1.Text;
		}


		public static string ShowInputBox(string s,bool password) 
		{
			InputBox box = new InputBox(s);
			if (password)
				box.textBox1.PasswordChar = '*';
			box.ShowDialog();
			return box.textBox1.Text;
		}

		private void textBox1_TextChanged(object sender, System.EventArgs e)
		{
		
		} 
	}
}