using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Merchant
{
	/// <summary>
	/// Summary description for ManageGISServers.
	/// </summary>
	public class ManageGISServers : System.Windows.Forms.Form
	{
		/// <summary>
		/// The servers that we are listing in the box
		/// </summary>
		protected System.Collections.ArrayList m_servers;
		public System.Collections.ArrayList Servers
		{
			get { return m_servers; }
			set { m_servers = value; }
		}

		/// <summary>
		/// The window to add a new GIS server
		/// </summary>
		protected Merchant.AddGIS m_addgis;

		/// <summary>
		/// Dirty bit to see if things have changed and must be saved
		/// </summary>
		protected bool m_dirty;
		public bool Dirty
		{
			get { return m_dirty; }
			set { m_dirty = value; }
		}

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox lbServers;
		private System.Windows.Forms.Button bClose;
		private System.Windows.Forms.Button bRemove;
		private System.Windows.Forms.Button bAdd;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ManageGISServers()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// add window
			this.m_addgis = new AddGIS();

			m_dirty = false;

			this.lbServers.DrawMode = DrawMode.OwnerDrawFixed;

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
			this.label1 = new System.Windows.Forms.Label();
			this.lbServers = new System.Windows.Forms.ListBox();
			this.bClose = new System.Windows.Forms.Button();
			this.bRemove = new System.Windows.Forms.Button();
			this.bAdd = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(200, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "List of known GIS Servers:";
			// 
			// lbServers
			// 
			this.lbServers.Location = new System.Drawing.Point(8, 40);
			this.lbServers.Name = "lbServers";
			this.lbServers.Size = new System.Drawing.Size(312, 186);
			this.lbServers.TabIndex = 1;
			this.lbServers.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbServers_DrawItem);
			// 
			// bClose
			// 
			this.bClose.Location = new System.Drawing.Point(240, 240);
			this.bClose.Name = "bClose";
			this.bClose.TabIndex = 2;
			this.bClose.Text = "Close";
			this.bClose.Click += new System.EventHandler(this.bClose_Click);
			// 
			// bRemove
			// 
			this.bRemove.Location = new System.Drawing.Point(96, 240);
			this.bRemove.Name = "bRemove";
			this.bRemove.TabIndex = 3;
			this.bRemove.Text = "Remove";
			this.bRemove.Click += new System.EventHandler(this.bRemove_Click);
			// 
			// bAdd
			// 
			this.bAdd.Location = new System.Drawing.Point(8, 240);
			this.bAdd.Name = "bAdd";
			this.bAdd.TabIndex = 4;
			this.bAdd.Text = "Add New...";
			this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
			// 
			// ManageGISServers
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(328, 269);
			this.Controls.Add(this.bAdd);
			this.Controls.Add(this.bRemove);
			this.Controls.Add(this.bClose);
			this.Controls.Add(this.lbServers);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ManageGISServers";
			this.Text = "GIS Servers";
			this.Load += new System.EventHandler(this.ManageGISServers_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void bRemove_Click(object sender, System.EventArgs e)
		{
			// get the selected index and delete it
			if ( this.lbServers.SelectedIndex != -1 )
			{
				this.lbServers.Items.Remove(this.lbServers.SelectedItem);
				m_dirty = true;
			}
		}

		private void bAdd_Click(object sender, System.EventArgs e)
		{
			DialogResult res;

			// show the add box and make a new one
			this.m_addgis.Server = new GISServer();
			res = this.m_addgis.ShowDialog(this);

			if ( res == DialogResult.OK )
			{
				this.lbServers.Items.Add(this.m_addgis.Server);
				m_dirty = true;
			}			
		}

		private void bClose_Click(object sender, System.EventArgs e)
		{
			// if dirty, clean out the servers and load them in
			if ( m_dirty )
			{
				this.m_servers.Clear();
				foreach ( GISServer gs in this.lbServers.Items )
				{
					this.m_servers.Add(gs);
				}
			}

			this.Close();
		}

		private void ManageGISServers_Load(object sender, System.EventArgs e)
		{
			// set the dirty bit to clean
			m_dirty = false;

			// reload the data from the stored server list into the listbox
			this.lbServers.Items.Clear();
			foreach ( GISServer gs in this.m_servers )
			{
				this.lbServers.Items.Add(gs);
			}
		}

		private void lbServers_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			if ( e.Index == -1 )
			{
				// Draw the current item text based on the above made string
				e.Graphics.DrawString("", e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);

				// If the ListBox has focus, draw a focus rectangle around the selected item.
				e.DrawFocusRectangle();		

				return;
			}


			// custom drawing for the servers in the list box

			// Draw the background of the ListBox control for each item.
			e.DrawBackground();

			// the disputes item about to be drawn
			GISServer gs = lbServers.Items[e.Index] as GISServer;

			// create a string based on the attributes
			string text = "";

			text += gs.Name;
			text += ": ";
			text += gs.Location;
			text += ": ";
			text += gs.Url;
			
			// Draw the current item text based on the above made string
			e.Graphics.DrawString(text, e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);

			// If the ListBox has focus, draw a focus rectangle around the selected item.
			e.DrawFocusRectangle();		
		}


	}
}
