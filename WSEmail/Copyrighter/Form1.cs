/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System.Diagnostics;
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace Copyrighter
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListBox fileList;
		private System.Windows.Forms.Button getFiles;
		private System.Windows.Forms.TextBox dirSearch;
		private System.Windows.Forms.Button writeNotice;
		private System.Windows.Forms.TextBox textBox1;
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
			this.fileList = new System.Windows.Forms.ListBox();
			this.getFiles = new System.Windows.Forms.Button();
			this.dirSearch = new System.Windows.Forms.TextBox();
			this.writeNotice = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// fileList
			// 
			this.fileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.fileList.Location = new System.Drawing.Point(8, 40);
			this.fileList.Name = "fileList";
			this.fileList.Size = new System.Drawing.Size(144, 225);
			this.fileList.TabIndex = 0;
			this.fileList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fileList_KeyDown);
			this.fileList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fileList_KeyPress);
			// 
			// getFiles
			// 
			this.getFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.getFiles.Location = new System.Drawing.Point(168, 40);
			this.getFiles.Name = "getFiles";
			this.getFiles.Size = new System.Drawing.Size(88, 40);
			this.getFiles.TabIndex = 1;
			this.getFiles.Text = "Generate File List";
			this.getFiles.Click += new System.EventHandler(this.button1_Click);
			// 
			// dirSearch
			// 
			this.dirSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.dirSearch.Location = new System.Drawing.Point(8, 8);
			this.dirSearch.Name = "dirSearch";
			this.dirSearch.Size = new System.Drawing.Size(256, 20);
			this.dirSearch.TabIndex = 2;
			this.dirSearch.Text = "C:\\Inetpub\\;C:\\Documents and Settings\\luxk\\My Documents\\Visual Studio Projects\\Si" +
				"gnedRouter";
			// 
			// writeNotice
			// 
			this.writeNotice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.writeNotice.Location = new System.Drawing.Point(168, 96);
			this.writeNotice.Name = "writeNotice";
			this.writeNotice.Size = new System.Drawing.Size(88, 40);
			this.writeNotice.TabIndex = 3;
			this.writeNotice.Text = "Write Notice";
			this.writeNotice.Click += new System.EventHandler(this.writeNotice_Click);
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.Location = new System.Drawing.Point(168, 160);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(100, 104);
			this.textBox1.TabIndex = 4;
			this.textBox1.Text = "textBox1";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(280, 273);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.writeNotice);
			this.Controls.Add(this.dirSearch);
			this.Controls.Add(this.getFiles);
			this.Controls.Add(this.fileList);
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
			Application.Run(new Form1());
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			ArrayList dirs = new ArrayList();
			ArrayList files = new ArrayList();
			string[] startdirs = dirSearch.Text.Split(';');

			foreach (string s in startdirs)
				dirs.Add(new DirectoryInfo(s));

			bool added = true;
			
			while (added) 
			{
				added = false;
				foreach (DirectoryInfo d in dirs.ToArray(typeof(DirectoryInfo))) 
				{
					Debug.WriteLine("Searched " + d.FullName);
					dirs.Remove(d);
					if (d.FullName.IndexOf("/_vti_") <= 0) 
					{
						dirs.AddRange(d.GetDirectories());
						files.AddRange(d.GetFiles("*.cs"));
						files.AddRange(d.GetFiles("*.aspx"));
						added = true;
					}
				}
			}
			
			fileList.DataSource = files.ToArray(typeof(FileInfo));
			fileList.DisplayMember = "FullName";
		}

		private void fileList_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
		}

		private void fileList_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{

			if (e.KeyCode == Keys.Delete && fileList.SelectedIndex >= 0) 
			{
				int selected = fileList.SelectedIndex;
				ArrayList a = new ArrayList(fileList.Items);
				a.RemoveAt(selected);
				fileList.DataSource = a.ToArray(typeof(FileInfo));
				if (fileList.Items.Count > selected) 
					fileList.SelectedIndex = selected;
			}
		}

		private void writeNotice_Click(object sender, System.EventArgs e)
		{
			foreach (object o in fileList.Items) 
			{
				FileInfo f = (FileInfo)o;
				if (!((f.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly))
				{
					StreamReader r = f.OpenText();
					string file = "\r\n"+ r.ReadToEnd();
					r.Close();
					if (f.FullName.EndsWith(".aspx"))
						file = "<!-- " + "\r\n" + textBox1.Text + "\r\n-->" + file;
					else
						file = "/*\r\n"+ textBox1.Text + "\r\n*/\r\n" + file;


					FileStream fs = f.Open(FileMode.Truncate);
					StreamWriter w = new StreamWriter(fs);
					w.Write(file);
					w.Flush();
					w.Close();
				}
			}
		}
	}

}
