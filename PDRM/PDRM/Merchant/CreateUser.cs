using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using CommonTypes;

namespace Merchant
{
	/// <summary>
	/// Summary description for CreateDevice.
	/// </summary>
	public class CreateUser : System.Windows.Forms.Form
	{
		/// <summary>
		/// The User item that we are creating here
		/// </summary>
		protected User m_user;

		/// <summary>
		/// The User item that we are creating here
		/// </summary>
		public User theUser
		{
			get { return m_user;}
			set
			{
				m_user = value;

				// reload the values to the text boxes
				ReloadData();
			}
		}

		/// <summary>
		/// The list of GIS Servers that we can select from
		/// </summary>
		protected System.Collections.ArrayList m_GISServers;
		/// <summary>
		/// The list of GIS Servers that we can select from
		/// </summary>
		public System.Collections.ArrayList GISServers
		{
			get { return m_GISServers; }
			set
			{
				m_GISServers = value;

				// add the new servers to the list
				ReloadServers();
			}
		}
		
		protected GISServer m_GIS;
		/// <summary>
		/// The GIS Server to associate with this user
		/// </summary>
		public GISServer GIS
		{
			get { return m_GIS; }
			set
			{
				m_GIS = value;

				// if this one isn't in the list yet, add it to the combobox
				if ( this.m_GISServers.Contains(m_GIS) == false )
				{
					this.cmbGIS.Items.Add(m_GIS);
				}

				// now highlight it
				this.cmbGIS.SelectedItem = m_GIS;			
			}
		}

		/// <summary>
		/// The window to request license details
		/// </summary>
		protected Request m_request;

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox cbLicense;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.TextBox tbAddress;
		private System.Windows.Forms.TextBox tbEmail;
		private System.Windows.Forms.TextBox tbEntryTime;
		private System.Windows.Forms.Button bCancel;
		private System.Windows.Forms.Button bCreate;
		private System.Windows.Forms.ComboBox cmbGIS;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CreateUser()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// details window
			this.m_request = new Request();

			// the data objects
			this.m_user = new User();
			this.m_GISServers = new ArrayList();
			this.m_GIS = new GISServer();

			// set the combobox to be owner drawn
			this.cmbGIS.DrawMode = DrawMode.OwnerDrawFixed;
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
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tbEmail = new System.Windows.Forms.TextBox();
			this.tbAddress = new System.Windows.Forms.TextBox();
			this.tbName = new System.Windows.Forms.TextBox();
			this.cbLicense = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.cmbGIS = new System.Windows.Forms.ComboBox();
			this.tbEntryTime = new System.Windows.Forms.TextBox();
			this.bCreate = new System.Windows.Forms.Button();
			this.bCancel = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Address";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "Entry Time";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 88);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(80, 16);
			this.label4.TabIndex = 3;
			this.label4.Text = "Email Address";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 24);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(64, 16);
			this.label6.TabIndex = 5;
			this.label6.Text = "GIS Server";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.tbEmail);
			this.groupBox1.Controls.Add(this.tbAddress);
			this.groupBox1.Controls.Add(this.tbName);
			this.groupBox1.Controls.Add(this.cbLicense);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(320, 152);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "User Data";
			// 
			// tbEmail
			// 
			this.tbEmail.Location = new System.Drawing.Point(112, 88);
			this.tbEmail.Name = "tbEmail";
			this.tbEmail.Size = new System.Drawing.Size(192, 20);
			this.tbEmail.TabIndex = 8;
			this.tbEmail.Text = "";
			// 
			// tbAddress
			// 
			this.tbAddress.Location = new System.Drawing.Point(112, 56);
			this.tbAddress.Name = "tbAddress";
			this.tbAddress.Size = new System.Drawing.Size(192, 20);
			this.tbAddress.TabIndex = 7;
			this.tbAddress.Text = "";
			// 
			// tbName
			// 
			this.tbName.Location = new System.Drawing.Point(112, 24);
			this.tbName.Name = "tbName";
			this.tbName.Size = new System.Drawing.Size(192, 20);
			this.tbName.TabIndex = 6;
			this.tbName.Text = "";
			// 
			// cbLicense
			// 
			this.cbLicense.Location = new System.Drawing.Point(16, 120);
			this.cbLicense.Name = "cbLicense";
			this.cbLicense.Size = new System.Drawing.Size(152, 24);
			this.cbLicense.TabIndex = 5;
			this.cbLicense.Text = "Make a license for user";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.cmbGIS);
			this.groupBox2.Controls.Add(this.tbEntryTime);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Location = new System.Drawing.Point(8, 176);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(320, 88);
			this.groupBox2.TabIndex = 7;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "GIS Data";
			// 
			// cmbGIS
			// 
			this.cmbGIS.Location = new System.Drawing.Point(88, 24);
			this.cmbGIS.Name = "cmbGIS";
			this.cmbGIS.Size = new System.Drawing.Size(216, 21);
			this.cmbGIS.TabIndex = 8;
			this.cmbGIS.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGIS_DrawItem);
			// 
			// tbEntryTime
			// 
			this.tbEntryTime.Location = new System.Drawing.Point(88, 56);
			this.tbEntryTime.Name = "tbEntryTime";
			this.tbEntryTime.Size = new System.Drawing.Size(216, 20);
			this.tbEntryTime.TabIndex = 7;
			this.tbEntryTime.Text = "";
			// 
			// bCreate
			// 
			this.bCreate.Location = new System.Drawing.Point(168, 272);
			this.bCreate.Name = "bCreate";
			this.bCreate.TabIndex = 8;
			this.bCreate.Text = "Create";
			this.bCreate.Click += new System.EventHandler(this.bCreate_Click);
			// 
			// bCancel
			// 
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(256, 272);
			this.bCancel.Name = "bCancel";
			this.bCancel.TabIndex = 9;
			this.bCancel.Text = "Cancel";
			this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
			// 
			// CreateUser
			// 
			this.AcceptButton = this.bCreate;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.bCancel;
			this.ClientSize = new System.Drawing.Size(344, 309);
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.bCreate);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CreateUser";
			this.Text = "Create a new User";
			this.Load += new System.EventHandler(this.CreateUser_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Reload the data in the textboxes from the stored User object
		/// </summary>
		private void ReloadData()
		{
			// reload the data in the text boxes from the stored user item
			this.tbName.Text = this.m_user.Name;
			this.tbAddress.Text = this.m_user.Address;
			this.tbEmail.Text = this.m_user.EmailAddress;
			this.tbEntryTime.Text = this.m_user.EntryTime.ToString();
		}

		/// <summary>
		/// Reload the data in the combobox with the server information
		/// </summary>
		private void ReloadServers()
		{
			// reload the servers into the combobox
			this.cmbGIS.Items.Clear();

			// now enter in the new ones
			foreach ( GISServer gs in this.m_GISServers )
			{
				this.cmbGIS.Items.Add(gs);
			}
		}

		private void bCreate_Click(object sender, System.EventArgs e)
		{
			// make sure that the data is all there
			if ( this.cmbGIS.SelectedIndex == -1 )
			{
				// message box error!
				MessageBox.Show(this, "Must enter a GIS Server value", "Data value needed", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			// we must take the data in the textboxes and plug it into the user
			this.m_user.Name = this.tbName.Text;
			this.m_user.Address = this.tbAddress.Text;
			this.m_user.EntryTime = System.Convert.ToDateTime(this.tbEntryTime.Text);
			this.m_user.EmailAddress = this.tbEmail.Text;

			// if the checkbox for making a license is checked, make one
			if ( this.cbLicense.Checked )
			{
				DialogResult res;

				// get the details
				this.m_request.theUser = this.m_user;
				this.m_request.Server = (this.cmbGIS.SelectedItem as GISServer).Name ;

				// show the details window
				res = this.m_request.ShowDialog();

				// if the user hit cancel, continue without the license
				if ( res == DialogResult.Cancel )
				{
					MessageBox.Show(this, "No license created!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				
				// otherwise, continue making the license				
				System.Xml.XmlNodeList list;

				// prepare a license to be sent with the license template as stored
				System.Xml.XmlDocument newLic = new XmlDocument();
				newLic.Load("C:\\pdrm docs\\Merchant_License_Template.xml");

				// now we have to add in the required fields to the license
			
				// name of the user
				list = newLic.GetElementsByTagName("commonName", "*");
				list[0].InnerXml = this.m_user.Name;

				// mobile device id
				list = newLic.GetElementsByTagName("id", "*");
				list[0].InnerXml = this.m_user.EmailAddress;

				// find the place holder
				list = newLic.GetElementsByTagName("RightsPlaceHolder", "*");

				// set the rights accordingly
				System.Xml.XmlElement right;

				// send discounts
				if ( this.m_request.SendDiscounts )
				{
					right = newLic.CreateElement("", "sendanydiscount", "priv");
					list[0].ParentNode.AppendChild(right);
				}

				// send offensive ads
				if ( this.m_request.SendOffensiveAds )
				{
					right = newLic.CreateElement("", "sendoffensiveads", "priv");
					list[0].ParentNode.AppendChild(right);
				}

				// send normal ads
				if ( this.m_request.SendNormalAds )
				{
					right = newLic.CreateElement("", "sendnormalads", "priv");
					list[0].ParentNode.AppendChild(right);
				}

				// now remove the placeholder
				list[0].ParentNode.RemoveChild(list[0]);

				// now attach the license to the user
				this.m_user.License = newLic;
			}

			// save the selected GIS Server as the server
			this.m_GIS = (GISServer) this.cmbGIS.SelectedItem;

			// set the result to be ok
			this.DialogResult = DialogResult.OK;

			this.Close();
		}

		private void bCancel_Click(object sender, System.EventArgs e)
		{
			// set the result - cancel
			this.DialogResult = DialogResult.Cancel;

			this.Close();
		}

		private void cmbGIS_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			// draw each item by just showing the name
			if ( e.Index == -1 )
			{
				// Draw the current item text based on the above made string
				e.Graphics.DrawString("", e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);

				// If the ListBox has focus, draw a focus rectangle around the selected item.
				e.DrawFocusRectangle();		

				return;
			}

			// custom drawing for the servers in the combobox

			// Draw the background of the ListBox control for each item.
			e.DrawBackground();

			// the disputes item about to be drawn
			GISServer gs = this.cmbGIS.Items[e.Index] as GISServer;

			// create a string based on the attributes
			string text = "";

			text += gs.Name;
			
			// Draw the current item text based on the above made string
			e.Graphics.DrawString(text, e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);

			// If the ListBox has focus, draw a focus rectangle around the selected item.
			e.DrawFocusRectangle();		
		}

		private void CreateUser_Load(object sender, System.EventArgs e)
		{
			// make a new user object
			this.theUser = new User();

			// reload the GIS Servers stuff
			this.ReloadServers();
		}
	}
}
