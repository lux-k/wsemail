using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace BabyObjects
{
	/// <summary>
	/// Summary description for BabyForm.
	/// </summary>
	public class BabyForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		public delegate void NullDelegate();
		public event NullDelegate Done;
		private BabyObject BO;
		private bool finishing = false;
		public BabyObject Baby
		{
			get 
			{
				return BO;
			}
			set 
			{
				BO = value;
				RedrawBaby();
			}
		}

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public BabyForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			RedrawBaby();
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
			Finish();
			base.Dispose( disposing );

		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(184, 32);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(96, 20);
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "textBox1";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(24, 184);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(72, 32);
			this.button1.TabIndex = 1;
			this.button1.Text = "button1";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(128, 184);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(72, 32);
			this.button2.TabIndex = 2;
			this.button2.Text = "button2";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// BabyForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.AddRange(new System.Windows.Forms.Control[] 
{
	this.button2,
	this.button1,
	this.textBox1});
			this.Name = "BabyForm";
			this.Text = "BabyForm";
			this.ResumeLayout(false);

		}
		#endregion

		public void SetBaby(ref BabyObject b) 
		{
			BO = b;
			RedrawBaby();

		}

		private void Finish() 
		{
			if (!finishing) 
			{
				finishing = true;
				BO.theMessage = textBox1.Text;
				if (Done != null)
					Done.DynamicInvoke(null);

			}
		}

		private void RedrawBaby() 
		{
			
			if (BO != null && BO.theMessage != null)
				textBox1.Text = BO.theMessage;
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			BO.approve = true;
			Finish();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			BO.approve = false;
			Finish();
		}
	}
}
