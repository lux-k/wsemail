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

namespace WSEmailConfigurator
{
	/// <summary>
	/// Summary description for PromptingForm.
	/// </summary>
	public class PromptingForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button nextButton;
		private System.Windows.Forms.TextBox descriptionBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox inputBox1;
		private System.Windows.Forms.Button inputButton1;
		private System.Windows.Forms.Label inputLabel;
		private System.Windows.Forms.Label exampleLabel;
		private System.Windows.Forms.Button backButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PromptingForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public string Title 
		{
			set 
			{
				this.Text = value;
			}
		}

		public Label InputLabel 
		{
			get 
			{
				return this.inputLabel;
			}
		}

		public Label ExampleLabel 
		{
			get 
			{
				return this.exampleLabel;
			}
		}

		public TextBox InputBox 
		{
			get 
			{
				return this.inputBox1;
			}
		}

		public Button InputButton 
		{
		
			get 
			{
				return this.inputButton1;
			}
		}
		
		public Button NextButton 
		{
			get 
			{
				return this.nextButton;
			}
		}

		public Button BackButton 
		{
			get 
			{
				return this.backButton;
			}
		}

		public TextBox DescriptionBox 
		{
			get 
			{
				return this.descriptionBox;
			}
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PromptingForm));
			this.nextButton = new System.Windows.Forms.Button();
			this.descriptionBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.inputBox1 = new System.Windows.Forms.TextBox();
			this.inputButton1 = new System.Windows.Forms.Button();
			this.inputLabel = new System.Windows.Forms.Label();
			this.exampleLabel = new System.Windows.Forms.Label();
			this.backButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// nextButton
			// 
			this.nextButton.Location = new System.Drawing.Point(408, 384);
			this.nextButton.Name = "nextButton";
			this.nextButton.Size = new System.Drawing.Size(96, 32);
			this.nextButton.TabIndex = 2;
			this.nextButton.Text = "Next -->";
			// 
			// descriptionBox
			// 
			this.descriptionBox.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.descriptionBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.descriptionBox.Location = new System.Drawing.Point(80, 0);
			this.descriptionBox.Multiline = true;
			this.descriptionBox.Name = "descriptionBox";
			this.descriptionBox.ReadOnly = true;
			this.descriptionBox.Size = new System.Drawing.Size(448, 176);
			this.descriptionBox.TabIndex = 4;
			this.descriptionBox.Text = "textBox1";
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.White;
			this.label1.Image = ((System.Drawing.Bitmap)(resources.GetObject("label1.Image")));
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 440);
			this.label1.TabIndex = 2;
			// 
			// inputBox1
			// 
			this.inputBox1.Location = new System.Drawing.Point(192, 216);
			this.inputBox1.Name = "inputBox1";
			this.inputBox1.Size = new System.Drawing.Size(288, 20);
			this.inputBox1.TabIndex = 1;
			this.inputBox1.Text = "";
			this.inputBox1.Visible = false;
			// 
			// inputButton1
			// 
			this.inputButton1.Location = new System.Drawing.Point(280, 248);
			this.inputButton1.Name = "inputButton1";
			this.inputButton1.Size = new System.Drawing.Size(112, 32);
			this.inputButton1.TabIndex = 3;
			this.inputButton1.Text = "button1";
			this.inputButton1.Visible = false;
			// 
			// inputLabel
			// 
			this.inputLabel.Location = new System.Drawing.Point(88, 216);
			this.inputLabel.Name = "inputLabel";
			this.inputLabel.Size = new System.Drawing.Size(100, 24);
			this.inputLabel.TabIndex = 5;
			this.inputLabel.Text = "label2";
			this.inputLabel.Visible = false;
			// 
			// exampleLabel
			// 
			this.exampleLabel.Location = new System.Drawing.Point(192, 288);
			this.exampleLabel.Name = "exampleLabel";
			this.exampleLabel.Size = new System.Drawing.Size(288, 40);
			this.exampleLabel.TabIndex = 6;
			this.exampleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.exampleLabel.Visible = false;
			// 
			// backButton
			// 
			this.backButton.Enabled = false;
			this.backButton.Location = new System.Drawing.Point(296, 384);
			this.backButton.Name = "backButton";
			this.backButton.Size = new System.Drawing.Size(96, 32);
			this.backButton.TabIndex = 5;
			this.backButton.Text = "<-- Back";
			// 
			// PromptingForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(528, 437);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.backButton,
																		  this.exampleLabel,
																		  this.inputLabel,
																		  this.inputButton1,
																		  this.inputBox1,
																		  this.label1,
																		  this.descriptionBox,
																		  this.nextButton});
			this.Name = "PromptingForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "PromptingForm";
			this.ResumeLayout(false);

		}
		#endregion


	}
}
