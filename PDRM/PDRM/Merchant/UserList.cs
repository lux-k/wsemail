using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using CommonTypes;

namespace Merchant
{
	/// <summary>
	/// Summary description for GISUserList.
	/// </summary>
	public class UserList : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// The devices that we have in this GIS - User, GIS Server
		/// </summary>
		protected System.Collections.SortedList m_users;
		public System.Collections.SortedList Users
		{
			get { return m_users; }
			set 
			{
				m_users = value;

				// now reload the checkbox list
				ReloadData();
			}
		}

		/// <summary>
		/// Default title for the window
		/// </summary>
		private static string defaultText = "Available Users at GIS #";

		/// <summary>
		/// Window to get rights request details for the license request
		/// </summary>
		protected Request m_request_details;

		protected CreateAd m_createAd;

		private System.Windows.Forms.Button bClose;
		private System.Windows.Forms.Button bRequestLicense;
		private System.Windows.Forms.Button bSendAd;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckedListBox clbUsers;

		/// <summary>
		/// The ID of the GIS server that this list represents
		/// </summary>
		private string m_gis_ID;
		public string GIS_ID
		{
			get { return m_gis_ID;}
			set
			{
				m_gis_ID = value;

				this.Text = defaultText + m_gis_ID;
			}
		}

		public UserList()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// our list is initially empty
			this.m_users = new SortedList();

			// the window to get the rights request in
			this.m_request_details = new Request();

			// initialize the text of the window
			GIS_ID = "Unknown";	
	
			m_createAd = new CreateAd();
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
			this.bClose = new System.Windows.Forms.Button();
			this.bRequestLicense = new System.Windows.Forms.Button();
			this.bSendAd = new System.Windows.Forms.Button();
			this.clbUsers = new System.Windows.Forms.CheckedListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// bClose
			// 
			this.bClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bClose.Location = new System.Drawing.Point(352, 272);
			this.bClose.Name = "bClose";
			this.bClose.TabIndex = 1;
			this.bClose.Text = "Close";
			this.bClose.Click += new System.EventHandler(this.bClose_Click);
			// 
			// bRequestLicense
			// 
			this.bRequestLicense.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bRequestLicense.Location = new System.Drawing.Point(96, 272);
			this.bRequestLicense.Name = "bRequestLicense";
			this.bRequestLicense.Size = new System.Drawing.Size(96, 23);
			this.bRequestLicense.TabIndex = 2;
			this.bRequestLicense.Text = "Request License";
			this.bRequestLicense.Click += new System.EventHandler(this.bRequestLicense_Click);
			// 
			// bSendAd
			// 
			this.bSendAd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bSendAd.Location = new System.Drawing.Point(8, 272);
			this.bSendAd.Name = "bSendAd";
			this.bSendAd.TabIndex = 3;
			this.bSendAd.Text = "Send Ad";
			this.bSendAd.Click += new System.EventHandler(this.bSendAd_Click);
			// 
			// clbUsers
			// 
			this.clbUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.clbUsers.Location = new System.Drawing.Point(8, 56);
			this.clbUsers.Name = "clbUsers";
			this.clbUsers.Size = new System.Drawing.Size(424, 199);
			this.clbUsers.Sorted = true;
			this.clbUsers.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(424, 40);
			this.label1.TabIndex = 5;
			this.label1.Text = "List of available users and their locations.  Users for whom a license is on file" +
				" are indicated with a check mark.";
			// 
			// UserList
			// 
			this.AcceptButton = this.bClose;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(442, 319);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.clbUsers);
			this.Controls.Add(this.bSendAd);
			this.Controls.Add(this.bRequestLicense);
			this.Controls.Add(this.bClose);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(448, 344);
			this.Name = "UserList";
			this.Text = "Available Users at GIS#";
			this.Load += new System.EventHandler(this.GISUserList_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void GISUserList_Load(object sender, System.EventArgs e)
		{
			this.ReloadData();
		}
		
		/// <summary>
		/// Takes the data in the stored SortedList and adds it to the
		/// CheckedListBox according to whether we have a license for the
		/// device or not.
		/// </summary>
		private void ReloadData()
		{
			User u;
			bool hasLic = false;

			// clear out the old data
			this.clbUsers.Items.Clear();

			// take the sorted list that we have load the data into the
			// checkbox list
			foreach ( Object obj in this.m_users.Keys )
			{
				// cast if ok
				if ( obj == null )
					break;
				else
					u = obj as User;

				// get the license status
				if ( u.License != null )
				{
					hasLic = true;
				}
				else
				{
					hasLic = false;
				}

				// add it to the checked list box accordingly
				this.clbUsers.Items.Add(u, hasLic);
			}
		}

		private void bClose_Click(object sender, System.EventArgs e)
		{
			// nothing should have changed, so let's close up shop
			this.DialogResult = DialogResult.OK;
			
			this.Close();
		}

		private void bRequestLicense_Click(object sender, System.EventArgs e)
		{
			DialogResult res;

			// if there's no selected element, just leave
			if ( this.clbUsers.SelectedIndex == -1 )
				return;

			System.Xml.XmlNodeList list;

			// the device that we are going to query
			User dev = this.clbUsers.SelectedItem as User;

			// if the user is already licensed, put up a warning window
			if ( dev.License != null )
			{
				res = MessageBox.Show(this, "We already have a license for this user.  Request a new one?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				// if no, quit
				if ( res == DialogResult.No )
					return;
			}

			// prepare a license to be sent with the license template as stored
			System.Xml.XmlDocument newLic = new XmlDocument();
			newLic.Load("C:\\pdrm docs\\Merchant_License_Template.xml");

			// now we have to add in the required fields to the license
			
			// name of the user
			list = newLic.GetElementsByTagName("commonName", "*");
			list[0].InnerXml = dev.Name;

			// mobile device id
			list = newLic.GetElementsByTagName("id", "*");
			list[0].InnerXml = dev.Address;

			// the rights that we want
			// prepare the window
			this.m_request_details.Server = this.m_gis_ID;
			this.m_request_details.theUser = dev;
			
			// show the window
			res = this.m_request_details.ShowDialog(this);

			// if the User wants to continue, go on, otherwise quit
			if ( res == DialogResult.Cancel )
			{
				return;
			}

			// find the place holder
			list = newLic.GetElementsByTagName("RightsPlaceHolder", "*");

			// set the rights accordingly
			System.Xml.XmlElement right;

			// send discounts
			if ( this.m_request_details.SendDiscounts )
			{
				right = newLic.CreateElement("", "sendanydiscount", "priv");
				list[0].ParentNode.AppendChild(right);
			}

			// send offensive ads
			if ( this.m_request_details.SendOffensiveAds )
			{
				right = newLic.CreateElement("", "sendoffensiveads", "priv");
				list[0].ParentNode.AppendChild(right);
			}

			// send normal ads
			if ( this.m_request_details.SendNormalAds )
			{
				right = newLic.CreateElement("", "sendnormalads", "priv");
				list[0].ParentNode.AppendChild(right);
			}

			// now remove the placeholder
			list[0].ParentNode.RemoveChild(list[0]);

			// now send it to the user at the requesting location
			// until we have the WSEmail client working, this message box will just 
			// have to take the place of actually sending it
			MessageBox.Show(this, "When the WSEmail plug-in is ready, the license will have been sent to the user at the given WSEmail address", "Place Holder", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void bSendAd_Click(object sender, System.EventArgs e)
		{
			// make sure that there is somebody selected
			// if there's no selected element, just leave
			if ( this.clbUsers.SelectedIndex == -1 )
				return;

			System.Xml.XmlNodeList list;

			// the device that we are going to query
			User user = this.clbUsers.SelectedItem as User;

			Merchant.GISServer theGis;
			
			// show the create an ad window
			m_createAd.Ad = new Merchant.GIS.AdMessage();

			DialogResult res;
			// show it
			res = m_createAd.ShowDialog(this);

			// now get the information if it was an OK
			if ( res == DialogResult.OK )
			{
				// take the new ad and send it along

				// the user's GIS
				theGis = m_users[user] as Merchant.GISServer;

				// send along the ad
				if ( theGis.Proxy.SendAd(Convert.ToLocal(user), m_createAd.Ad) )
				{
					// success
					MessageBox.Show(this, user.Name + " was successfully sent an Ad", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					// failure
					MessageBox.Show(this, "Error: Ad wasn't posted to " + user.Name, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			}		

			
		}			
		
	}
}
#region Code for using a DataGrid object in the window
// Deprecated 22 Aug 03
/*
 * From InitComponents()
 * 			((System.ComponentModel.ISupportInitialize)(this.m_devicesGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// m_devicesGrid
			// 
			this.m_devicesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_devicesGrid.DataMember = "";
			this.m_devicesGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.m_devicesGrid.Location = new System.Drawing.Point(0, 0);
			this.m_devicesGrid.Name = "m_devicesGrid";
			this.m_devicesGrid.Size = new System.Drawing.Size(364, 336);
			this.m_devicesGrid.TabIndex = 0;	
 * 
 * 	/// <summary>
 		/// The DataGrid view in the window
 		/// </summary>
		private System.Windows.Forms.DataGrid m_devicesGrid;
		/// <summary>
		/// User data in this table
		/// </summary>
		protected System.Data.DataSet m_devicesSet;
		/// <summary>
		/// The actual list of Users as passed from the MDI window
		/// </summary>
		private System.Collections.SortedList m_devicesList;
		public System.Collections.SortedList DevicesList
		{
			get { return m_devicesList; }
			set
			{
				m_devicesList = value;

				// set the DataTable to reflect the new User list
				UpdateDataSet();
			}
		}
		
	public DeviceList()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			// the dataset that the datagrid is showing us
			this.m_devicesSet = new System.Data.DataSet("Devices");

			// create the data table for use
			MakeDataSet();
			// initialize the text of the window
			GIS_ID = "Unknown";			
		}
		
		private void GISUserList_Load(object sender, System.EventArgs e)
		{
			// set up the dataset appropriately
			this.UpdateDataSet();

			// we must make a relationship between the DataTable and
			// the DataGrid
			m_devicesGrid.SetDataBinding(this.m_devicesSet, "Devices");
		}
		
		/// <summary>
		/// Go through the DataSet and update the information in the device
		/// DataTable for proper showing the DataGrid
		/// </summary>
		private void UpdateDataSet()
		{
			// clear out the current data from the data set
			this.m_devicesSet.Clear();

			// now load in the new user information into the dataset
			DataRow newRow;
			foreach( GIS.User u in this.m_devicesList.Keys )
			{
				// add the user to the table

				// make a new row for entry
				newRow = this.m_devicesSet.Tables["Devices"].NewRow();

				// put the user and time data in
				newRow["Name"] = u.Name;
				newRow["Address"] = u.Address;
				newRow["Entry Time"] = this.m_devicesList[u];

				this.m_devicesSet.Tables["Devices"].Rows.Add(newRow);
			}

			// the dataset now reflects the new data list information
		}

		/// <summary>
		/// Initialize the m_deviceSet DataSet to have the columns that
		/// we need
		/// </summary>
		private void MakeDataSet()
		{
			DataTable devicesTable = new DataTable("Devices");

			// Create columns and add them to the table.
			DataColumn cName = new DataColumn("Name");
			DataColumn cAddress = new DataColumn("Address", typeof(string));
			DataColumn cTime = new DataColumn("Entry Time", typeof(System.DateTime));
			devicesTable.Columns.Add(cName);
			devicesTable.Columns.Add(cAddress);
			devicesTable.Columns.Add(cTime);
		
			// Add the table to the deviceDataSet.
			this.m_devicesSet.Tables.Add(devicesTable);

			// Create a DataRelation, and add it to the DataSet.
		}
		
		private void bReset_Click(object sender, System.EventArgs e)
		{
			DialogResult res;

			// make sure that the user wants to do this
			res = MessageBox.Show(this, "Are you sure you want to reset the data grid?\nAll changes will be lost!", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

			// if yes, do it
			if ( res == DialogResult.Yes )
			{
				// reload the data from the sorted list as provided
				this.UpdateDataSet();
			}
		}
*/
#endregion