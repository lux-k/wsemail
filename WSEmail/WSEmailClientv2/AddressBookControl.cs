/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using XmlAddressBook;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace WSEmailClientv2
{
	/// <summary>
	/// Summary description for AddressBookControl.
	/// </summary>
	public class AddressBookControl : System.Windows.Forms.UserControl
	{
		[DefaultValue(""), Category("Data"), Description("The addresses."), Browsable(true)]
		public override string Text 
		{
			get 
			{
				return textBox.Text;
			}
			set 
			{
				addys = value;
				textBox.Text = value;
			}
		}

		private string addys = "";
		private bool init = false;
		private bool HandlersAdded = false;
		private bool dontreset = false;
		private System.Windows.Forms.TextBox textBox;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		
		public AddressBook TheBook;
		private System.Windows.Forms.ContextMenu menu;
		private string Lookup = "";
		private FrmPopupAddress f = new FrmPopupAddress();

		public AddressBookControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			textBox.Text = addys;
			f.list.Click += new EventHandler(list_SelectedIndexChanged);
			// TODO: Add any initialization after the InitializeComponent call

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.textBox = new System.Windows.Forms.TextBox();
			this.menu = new System.Windows.Forms.ContextMenu();
			this.SuspendLayout();
			// 
			// textBox
			// 
			this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBox.Location = new System.Drawing.Point(0, 0);
			this.textBox.Multiline = true;
			this.textBox.Name = "textBox";
			this.textBox.Size = new System.Drawing.Size(150, 20);
			this.textBox.TabIndex = 0;
			this.textBox.Text = "textBox1";
			this.textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
			this.textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
			this.textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
			this.textBox.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// AddressBookControl
			// 
			this.Controls.Add(this.textBox);
			this.Name = "AddressBookControl";
			this.Size = new System.Drawing.Size(152, 24);
			this.ResumeLayout(false);

		}
		#endregion

		private void Init() 
		{
			if (!init) 
			{

				int left = this.ParentForm.ParentForm.DesktopLocation.X + (this.ParentForm.ParentForm.DesktopBounds.Width - this.ParentForm.DesktopBounds.Width) + this.Left + this.ParentForm.DesktopBounds.Width - this.ParentForm.ClientSize.Width;
				int top= this.ParentForm.ParentForm.DesktopLocation.Y + (this.ParentForm.ParentForm.DesktopBounds.Height - this.ParentForm.DesktopBounds.Height) + this.Top + this.Height + this.ParentForm.DesktopBounds.Height - this.ParentForm.ClientSize.Height;

				if (!HandlersAdded) 
				{
					this.ParentForm.Deactivate += new EventHandler(textBox_Leave);
					this.ParentForm.LostFocus += new EventHandler(textBox_Leave);
					if (this.ParentForm.ParentForm != null)
						this.ParentForm.ParentForm.Deactivate += new EventHandler(textBox_Leave);
					this.ParentForm.KeyUp += new KeyEventHandler(ParentForm_KeyUp);
					this.ParentForm.KeyDown += new KeyEventHandler(ParentForm_KeyUp);
					this.ParentForm.KeyPreview = true;
					HandlersAdded = true;
				}

				Point p = new Point(left,top);
				f.DesktopLocation = p;
				//init = true;
				//f.Show();
				//MessageBox.Show(p.ToString());
				//MessageBox.Show(f.DesktopLocation.ToString());
			}
		}

		private void textBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Back) 
			{
				if (Lookup.Length > 0)
					Lookup = Lookup.Substring(0,Lookup.Length - 1);
				if (Lookup.Length > 0)
					DoLookup();
				else
					f.Hide();
				e.Handled=true;
			}

			if (e.KeyCode == Keys.Down && f.Visible) 
			{
				if (f.Visible && f.list.SelectedIndex + 1 < f.list.Items.Count)
					f.list.SelectedIndex += 1;
				e.Handled = true;
			}

			if (e.KeyCode == Keys.Enter && f.Visible) 
			{
				if (f.Visible && f.list.SelectedIndex >= 0)
					list_SelectedIndexChanged(null,null);
				e.Handled = true;
			}

			if (e.KeyCode == Keys.Up && f.Visible) 
			{
				if (f.Visible && f.list.SelectedIndex - 1 >= 0)
					f.list.SelectedIndex -= 1;
				e.Handled = true;
			}

			if (e.KeyCode == Keys.Tab) 
			{
				if (f.Visible && f.list.SelectedIndex >= 0)
					list_SelectedIndexChanged(null,null);
					e.Handled = true;
			}
		}

		private void textBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == ' ' || e.KeyChar == ',' || e.KeyChar == ';' )
			{
				Lookup = "";
				f.Hide();
				return;
			}

			if (e.KeyChar == '\r') 
			{
				e.Handled = true;
				f.Hide();
				return;
			}

			if (e.KeyChar == 8) 
			{
				return;
			}

			Lookup += e.KeyChar;
			DoLookup();
		}

		private void DoLookup() 
		{
			Console.WriteLine("Lookup = '" + Lookup + "'");
			f.list.Items.Clear();
			AddressBookEntry[] entries = TheBook.GetEntries(Lookup);
			if (entries.Length > 0) 
			{
				foreach (AddressBookEntry a in entries) 
				{
					f.list.Items.Add(a.Email);
				}

				if (!init)
					Init();
				dontreset = true;
				f.Show();

				f.TopMost = true;
			} 
			else
				f.Hide();

			textBox.Focus();
		}

		private void FillInAddress(object o, EventArgs e) 
		{
			if (o is MenuItem) 
			{
				MenuItem m = ((MenuItem)o);
				int i = textBox.Text.LastIndexOf(" ");
				if (i > 0) 
				{
					textBox.Text = textBox.Text.Substring(0,i-1);
					textBox.Text += ", ";
				} else
					textBox.Text = "";
				
				textBox.Text += ((MenuItem)o).Text + ", ";
				textBox.SelectionStart=textBox.Text.Length;
				textBox.ScrollToCaret();
				Lookup = "";
			}

		}
		private void textBox_TextChanged(object sender, System.EventArgs e)
		{
			if (textBox.Text.Length == 0) 
			{
				Console.WriteLine("Reset lookup");
				Lookup = "";
			}
		}

		private void list_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (f.list.SelectedIndex >= 0)  
			{
//				MenuItem m = ((MenuItem)o);
				string id = (string)f.list.SelectedItem;
				int i = textBox.Text.LastIndexOf(" ");
				if (i > 0) 
				{
					textBox.Text = textBox.Text.Substring(0,i-1);
					textBox.Text += ", ";
				} 
				else
					textBox.Text = "";
				
//				textBox.Text += ((MenuItem)o).Text + ", ";
				textBox.Text += id + ", ";
				textBox.SelectionStart=textBox.Text.Length;
				textBox.ScrollToCaret();
				Lookup = "";
			}
			f.Hide();

		}

		private void textBox_Leave(object sender, System.EventArgs e)
		{
			if (dontreset)
				dontreset = false;
			else 
			{
				if (f.Visible)
					f.Hide();
			}
		}

		private void ParentForm_KeyUp(object sender, KeyEventArgs e)
		{
			
		}
	}
}
