using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using System.Reflection;
using BusinessObjects;

namespace ResourceMonkey
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button1;
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
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(8, 16);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(280, 144);
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "textBox1";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(192, 232);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(88, 32);
			this.button1.TabIndex = 1;
			this.button1.Text = "Enumerate...";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(304, 301);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.button1,
																		  this.textBox1});
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			PennRoutingFilters.PennRoutingUtilities.AddPennRoutingFilters(false);
			Application.Run(new Form1());
		}

		private void button1_Click(object sender, System.EventArgs e)
		{

//			FrmTimesheet f = new FrmTimesheet();
//			f.Show();

			//"Additional information: ResourceManager base name should not end in .resources.
			//It should be similar to MyResources, which the ResourceManager can convert into 
			//MyResources.<culture>.resources; for example, MyResources.en-US.resources.
//			SignedCode
			((Form)Activator.CreateInstance(Assembly.LoadFrom("TheForm.dll").GetType("TheForm.TheForm"))).Show();


			//System.Reflection.Assembly a = System.Reflection.Assembly.LoadFrom("TheForm.Dll");
			//ResourceManager res = new ResourceManager("frmSend",a);
			// ResourceManager res = new ResourceManager("frmSend", a);

			// System.Resources.ResourceManager res = new System.Resources.ResourceManager("frmSend",this.GetType().Assembly);
			
			Object o = res.GetObject("WSEMailClient.frmSend");
			MessageBox.Show("Got the frmSend!");
			if (o == null)
				MessageBox.Show("O is null!");

			Form f = (Form)o;
			
			if (f == null)
				MessageBox.Show("F is null?");

			foreach (String Resource in a.GetManifestResourceNames())
			{
				textBox1.Text += Resource + "\r\n";
			}

			/*
			// Create a ResXResourceReader for the file items.resx.
			ResXResourceReader rsxr = new ResXResourceReader("frmSend.resx");
			// Create an IDictionaryEnumerator to iterate through the resources.
			IDictionaryEnumerator id = rsxr.GetEnumerator();       

			// Iterate through the resources and display the contents to the console.
			foreach (DictionaryEntry d in rsxr) 
			{
				if (d.Key != null &&  d.Value != null)
				textBox1.Text += (d.Key.ToString() + ":\t" + d.Value.ToString()) + "\r\n";
			}

			//Close the reader.
			rsxr.Close();
			*/
		}
	}
}
