using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Web;
using Microsoft.Web.Services;
using Microsoft.Web.Services.Security;
using Microsoft.Web.Services.Security.X509;

namespace Merchant
{
	/// <summary>
	/// Window to register with and query GIS Servers that are already on the
	/// server list
	/// </summary>
	public class GISQuery : System.Windows.Forms.Form
	{
		/// <summary>
		/// The GISes that we have to show, both registered and unregistered
		/// </summary>
		protected ArrayList m_GISList;

		/// <summary>
		/// The GIS Servers that we have to work with, both registered
		/// and unregistered
		/// </summary>
		public ArrayList GISList
		{
			get { return m_GISList; }
			set
			{
				m_GISList = value;

				// reload the data
				ReloadData();
			}
		}

		/// <summary>
		/// The P3P policy that we are feeding to the GIS Servers to ask
		/// for permission to register
		/// </summary>
		protected P3P.POLICY m_policy;

		/// <summary>
		/// The P3P policy that we are feeding to the GIS Servers to ask
		/// for permission to register
		/// </summary>
		public P3P.POLICY Policy
		{
			get { return m_policy; }
			set
			{
				m_policy = value;
			}
		}

		/// <summary>
		/// The new users that are retrieved from the GISs
		/// </summary>
		protected System.Collections.SortedList m_new_users;

		/// <summary>
		/// Indicates whether the user list has been added to and not read
		/// from yet.
		/// </summary>
		protected bool m_new_users_dirty;

		/// <summary>
		/// The new users that are retrieved from the GISs.  The user information
		/// is guaranteed not to be lost until it is retrieved at least once.  After
		/// that, there is no guarantee that the user data will still be there.
		/// </summary>
		public System.Collections.SortedList New_Users
		{
			get 
			{
				// no longer dirty
				m_new_users_dirty = false;

				return m_new_users;
			}
		}

		/// <summary>
		/// The X.509 Certificate to use for signing the requests
		/// </summary>
		protected Microsoft.Web.Services.Security.X509.X509Certificate m_cert;

		/// <summary>
		/// The X.509 Certificate to use for signing the requests
		/// </summary>
		public Microsoft.Web.Services.Security.X509.X509Certificate Cert
		{
			get { return m_cert; }
			set { m_cert = value; }
		}

		private System.Windows.Forms.ListBox lUnregistered;
		private System.Windows.Forms.Button bRegister;
		private System.Windows.Forms.ListBox lRegistered;
		private System.Windows.Forms.Button bClose;
		private System.Windows.Forms.GroupBox gUnregistered;
		private System.Windows.Forms.GroupBox gRegistered;
		private System.Windows.Forms.Button bQuery;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GISQuery()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// initialize
			this.m_GISList = new ArrayList();
			this.m_new_users = new SortedList();
			m_new_users_dirty = true;
			m_cert = null;
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
			this.lUnregistered = new System.Windows.Forms.ListBox();
			this.bRegister = new System.Windows.Forms.Button();
			this.lRegistered = new System.Windows.Forms.ListBox();
			this.bQuery = new System.Windows.Forms.Button();
			this.bClose = new System.Windows.Forms.Button();
			this.gUnregistered = new System.Windows.Forms.GroupBox();
			this.gRegistered = new System.Windows.Forms.GroupBox();
			this.gUnregistered.SuspendLayout();
			this.gRegistered.SuspendLayout();
			this.SuspendLayout();
			// 
			// lUnregistered
			// 
			this.lUnregistered.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lUnregistered.Location = new System.Drawing.Point(16, 24);
			this.lUnregistered.Name = "lUnregistered";
			this.lUnregistered.Size = new System.Drawing.Size(408, 173);
			this.lUnregistered.TabIndex = 0;
			// 
			// bRegister
			// 
			this.bRegister.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bRegister.Location = new System.Drawing.Point(352, 208);
			this.bRegister.Name = "bRegister";
			this.bRegister.Size = new System.Drawing.Size(72, 24);
			this.bRegister.TabIndex = 2;
			this.bRegister.Text = "Register";
			this.bRegister.Click += new System.EventHandler(this.bRegister_Click);
			// 
			// lRegistered
			// 
			this.lRegistered.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lRegistered.Location = new System.Drawing.Point(16, 24);
			this.lRegistered.Name = "lRegistered";
			this.lRegistered.Size = new System.Drawing.Size(408, 173);
			this.lRegistered.TabIndex = 3;
			// 
			// bQuery
			// 
			this.bQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bQuery.Location = new System.Drawing.Point(312, 208);
			this.bQuery.Name = "bQuery";
			this.bQuery.Size = new System.Drawing.Size(112, 23);
			this.bQuery.TabIndex = 5;
			this.bQuery.Text = "Query for User List";
			this.bQuery.Click += new System.EventHandler(this.bQuery_Click);
			// 
			// bClose
			// 
			this.bClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bClose.Location = new System.Drawing.Point(368, 512);
			this.bClose.Name = "bClose";
			this.bClose.TabIndex = 6;
			this.bClose.Text = "Close";
			this.bClose.Click += new System.EventHandler(this.bClose_Click);
			// 
			// gUnregistered
			// 
			this.gUnregistered.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.gUnregistered.Controls.Add(this.bRegister);
			this.gUnregistered.Controls.Add(this.lUnregistered);
			this.gUnregistered.Location = new System.Drawing.Point(8, 8);
			this.gUnregistered.Name = "gUnregistered";
			this.gUnregistered.Size = new System.Drawing.Size(440, 240);
			this.gUnregistered.TabIndex = 7;
			this.gUnregistered.TabStop = false;
			this.gUnregistered.Text = "GIS Servers that we haven\'t registered with";
			// 
			// gRegistered
			// 
			this.gRegistered.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.gRegistered.Controls.Add(this.bQuery);
			this.gRegistered.Controls.Add(this.lRegistered);
			this.gRegistered.Location = new System.Drawing.Point(8, 264);
			this.gRegistered.Name = "gRegistered";
			this.gRegistered.Size = new System.Drawing.Size(440, 240);
			this.gRegistered.TabIndex = 8;
			this.gRegistered.TabStop = false;
			this.gRegistered.Text = "GIS Servers that we have registered with";
			// 
			// GISQuery
			// 
			this.AcceptButton = this.bClose;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(456, 541);
			this.Controls.Add(this.gRegistered);
			this.Controls.Add(this.gUnregistered);
			this.Controls.Add(this.bClose);
			this.Name = "GISQuery";
			this.Text = "GIS Options";
			this.gUnregistered.ResumeLayout(false);
			this.gRegistered.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Reload the GIS information in the list boxes from the stored user list
		/// </summary>
		private void ReloadData()
		{
			// clear out the registered and unregistered lists
			this.lRegistered.Items.Clear();
			this.lUnregistered.Items.Clear();

			// we must parse the GIS Server list and update the list boxes according
			// to whether the servers are registered or not
			foreach ( GISServer gis in this.m_GISList )
			{
				// if it's null, quit
				if ( gis == null )
					break;

				// now check its status
				if ( gis.Registered == true )
				{
					this.lRegistered.Items.Add(gis);
				}
				else
				{
					this.lUnregistered.Items.Add(gis);
				}
			}

			// anything else?
		}

		private void bRegister_Click(object sender, System.EventArgs e)
		{
			// make sure that there is something selected
			if ( this.lUnregistered.SelectedIndex == -1 )
				return;

			// now try to register with it
			bool result;
			GISServer gis = this.lUnregistered.SelectedItem as GISServer;

			// sign the request with our cert	
			SoapContext requestContext = gis.Proxy.RequestSoapContext;
			// Get our security token
			X509SecurityToken token = new Microsoft.Web.Services.Security.X509SecurityToken(m_cert);
			if (token == null)
				throw new ApplicationException("No key provided for signature.");

			// Add the signature element to a security section on the request
			// to sign the request
			requestContext.Security.Tokens.Add(token);
			requestContext.Security.Elements.Add(new Signature(token));

			result = gis.Proxy.RegisterMerchant(convertPolicy(m_policy));

			// run this insecurely
			// GIS.Merchant merch = new Merchant.GIS.Merchant();
			// merch.Name = "Michael";
			// result = gis.Proxy.InsecureRegisterMerchant(merch, convertPolicy(m_policy));

			// if it fails, show an error message
			if ( result == false )
			{
				MessageBox.Show(this, "Registration refused by GIS.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				// change the GIS registration status
				gis.Registered = true;

				// change it to the registered box
				this.lUnregistered.Items.Remove(this.lUnregistered.SelectedItem);
				this.lRegistered.Items.Add(gis);
			}
		}

		private void bQuery_Click(object sender, System.EventArgs e)
		{
			// make sure that there is something selected
			if ( this.lRegistered.SelectedIndex == -1 )
				return;

			// the gis we are talking to 
			GISServer gis = this.lRegistered.SelectedItem as GISServer;			

			// sign the request with our cert	
			SoapContext requestContext = gis.Proxy.RequestSoapContext;
			// Get our security token
			X509SecurityToken token = new Microsoft.Web.Services.Security.X509SecurityToken(m_cert);
			if (token == null)
				throw new ApplicationException("No key provided for signature.");

			// Add the signature element to a security section on the request
			// to sign the request
			requestContext.Security.Tokens.Add(token);
			requestContext.Security.Elements.Add(new Signature(token));

			// now try to request the user list from it
			object[] res = null;
            res = gis.Proxy.getUsers();
			
			// do this insecurely now
			//GIS.Merchant merch = new Merchant.GIS.Merchant();
			//merch.Name = "Michael";			

			//try
			//{
			//	res = gis.Proxy.InsecureGetUsers(merch);
			//}
			//catch (System.Exception ex)
			//{
			//	// show an error window, don't make this fail
			//	MessageBox.Show(this, "Error: Can't establish connection with GIS at " + gis.Url_String, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//	return;
			//}

			// the users that we have fetched
			ArrayList users;

			if ( res != null )
			{
				users = new ArrayList(res);
			}
			else
			{
				users = new ArrayList();
			}

			// if it's empty, give a message
			if ( users.Count == 0 )
			{
				MessageBox.Show(this, "User List empty.  No users added to the list", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				// we must add the users to the application's users list

				// if the list isn't dirty, clear it
				if ( m_new_users_dirty == false )
				{
					m_new_users.Clear();
				}

				// now add the users to the list
				foreach ( Merchant.GIS.User u in users )
				{
					// make sure it's not empty
					if ( u == null )
						break;

					// first convert it to a local representation of user - using
					// the CommonTypes
					CommonTypes.User user = Convert.ToCommon(u);

					// add the user with the associated GIS

					// set the time entered to now
					user.EntryTime = System.DateTime.Now;

					// if the user is already there, just change the GIS
					if ( m_new_users[user] != null )
					{
						m_new_users[user] = gis;
					}
					else
					{
						// make a new one
						m_new_users.Add(user, gis);
					}

				}

				// and mark it dirty
				m_new_users_dirty = true;
			}

			// show a message
			MessageBox.Show(this, "Added " + users.Count + " new users to the user list", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);

			// done
		}

		private Merchant.GIS.POLICY convertPolicy(P3P.POLICY Old)
		{
			// take the whole policy and make a new one out of it
			Merchant.GIS.POLICY New = new Merchant.GIS.POLICY();
			New.m_statements = new Merchant.GIS.STATEMENT[32];
			New.m_statements[0] = new Merchant.GIS.STATEMENT();

			#region ACCESS
			// ACCESS
			New.access = new Merchant.GIS.ACCESS();
			New.access.all = Old.access.all;
			New.access.contact_and_other = Old.access.contact_and_other;
			New.access.ident_contact = Old.access.ident_contact;
			New.access.none = Old.access.none;
			New.access.nonident = Old.access.nonident;
			New.access.other_ident = Old.access.other_ident;
			#endregion

			#region CATEGORIES
			// CATEGORIES
			New.m_statements[0].categories = new Merchant.GIS.CATEGORIES();
			New.m_statements[0].categories.computer = Old.m_statements[0].categories.computer;
			New.m_statements[0].categories.content = Old.m_statements[0].categories.content;
			New.m_statements[0].categories.demographic = Old.m_statements[0].categories.demographic;
			New.m_statements[0].categories.financial = Old.m_statements[0].categories.financial;
			New.m_statements[0].categories.government = Old.m_statements[0].categories.government;
			New.m_statements[0].categories.health = Old.m_statements[0].categories.health;
			New.m_statements[0].categories.interactive = Old.m_statements[0].categories.interactive;
			New.m_statements[0].categories.location = Old.m_statements[0].categories.location;
			New.m_statements[0].categories.navigation = Old.m_statements[0].categories.navigation;
			New.m_statements[0].categories.online = Old.m_statements[0].categories.online;
			New.m_statements[0].categories.other_category = Old.m_statements[0].categories.other_category;
			New.m_statements[0].categories.physical = Old.m_statements[0].categories.physical;
			New.m_statements[0].categories.political = Old.m_statements[0].categories.political;
			New.m_statements[0].categories.purchase = Old.m_statements[0].categories.purchase;
			New.m_statements[0].categories.state = Old.m_statements[0].categories.state;
			New.m_statements[0].categories.uniqueid = Old.m_statements[0].categories.uniqueid;
			#endregion

			#region DISPUTES
			// DISPUTES
			New.m_disputes = new Merchant.GIS.DISPUTES[32];
			int i = 0;
			foreach ( P3P.DISPUTES disp in Old.m_disputes )
			{
				// make sure it's not null
				if ( disp == null )
					break;

				New.m_disputes[i] = new Merchant.GIS.DISPUTES();

				// remedies
				New.m_disputes[i].remedies = new Merchant.GIS.REMEDIES();
				New.m_disputes[i].remedies.correct = disp.remedies.correct;
				New.m_disputes[i].remedies.law = disp.remedies.law;
				New.m_disputes[i].remedies.money = disp.remedies.money;
				
				// resolution type
				switch (disp.resolution_type )
				{
					case P3P.RESOLUTION_TYPE.COURT:
						New.m_disputes[i].resolution_type = Merchant.GIS.RESOLUTION_TYPE.COURT;
						break;

					case P3P.RESOLUTION_TYPE.INDEPENDENT:
						New.m_disputes[i].resolution_type = Merchant.GIS.RESOLUTION_TYPE.INDEPENDENT;
						break;

					case P3P.RESOLUTION_TYPE.LAW:
						New.m_disputes[i].resolution_type = Merchant.GIS.RESOLUTION_TYPE.LAW;
						break;

					case P3P.RESOLUTION_TYPE.SERVICE:
						New.m_disputes[i].resolution_type = Merchant.GIS.RESOLUTION_TYPE.SERVICE;
						break;
				}

				// service
				New.m_disputes[i].service = disp.service;

				// increment the counter
				i++;
				// if we're at 32 or over, break, it's too much
				if ( i >= 32 ) break;
			}
			#endregion

			#region PURPOSE
			// PURPOSE
			New.m_statements[0].purpose = new Merchant.GIS.PURPOSE();

			// admin
			New.m_statements[0].purpose.admin = new Merchant.GIS.Element_with_Required();
			New.m_statements[0].purpose.admin.present = Old.m_statements[0].purpose.admin.present;
			switch ( Old.m_statements[0].purpose.admin.required )
			{
				case P3P.REQUIRED.always:
					New.m_statements[0].purpose.admin.required = Merchant.GIS.REQUIRED.always;
					break;

				case P3P.REQUIRED.opt_out:
					New.m_statements[0].purpose.admin.required = Merchant.GIS.REQUIRED.opt_out;
					break;

				case P3P.REQUIRED.opt_in:
					New.m_statements[0].purpose.admin.required = Merchant.GIS.REQUIRED.opt_in;
					break;
			}

			// contact
			New.m_statements[0].purpose.contact = new Merchant.GIS.Element_with_Required();
			New.m_statements[0].purpose.contact.present = Old.m_statements[0].purpose.contact.present;
			switch ( Old.m_statements[0].purpose.contact.required )
			{
				case P3P.REQUIRED.always:
					New.m_statements[0].purpose.contact.required = Merchant.GIS.REQUIRED.always;
					break;

				case P3P.REQUIRED.opt_out:
					New.m_statements[0].purpose.contact.required = Merchant.GIS.REQUIRED.opt_out;
					break;

				case P3P.REQUIRED.opt_in:
					New.m_statements[0].purpose.contact.required = Merchant.GIS.REQUIRED.opt_in;
					break;
			}

			// current
			New.m_statements[0].purpose.current = Old.m_statements[0].purpose.current;

			// develop
			New.m_statements[0].purpose.develop = new Merchant.GIS.Element_with_Required();
			New.m_statements[0].purpose.develop.present = Old.m_statements[0].purpose.develop.present;
			switch ( Old.m_statements[0].purpose.develop.required )
			{
				case P3P.REQUIRED.always:
					New.m_statements[0].purpose.develop.required = Merchant.GIS.REQUIRED.always;
					break;

				case P3P.REQUIRED.opt_out:
					New.m_statements[0].purpose.develop.required = Merchant.GIS.REQUIRED.opt_out;
					break;

				case P3P.REQUIRED.opt_in:
					New.m_statements[0].purpose.develop.required = Merchant.GIS.REQUIRED.opt_in;
					break;
			}
			// historical
			New.m_statements[0].purpose.historical = new Merchant.GIS.Element_with_Required();
			New.m_statements[0].purpose.historical.present = Old.m_statements[0].purpose.historical.present;
			switch ( Old.m_statements[0].purpose.historical.required )
			{
				case P3P.REQUIRED.always:
					New.m_statements[0].purpose.historical.required = Merchant.GIS.REQUIRED.always;
					break;

				case P3P.REQUIRED.opt_out:
					New.m_statements[0].purpose.historical.required = Merchant.GIS.REQUIRED.opt_out;
					break;

				case P3P.REQUIRED.opt_in:
					New.m_statements[0].purpose.historical.required = Merchant.GIS.REQUIRED.opt_in;
					break;
			}
			// individual_analysis
			New.m_statements[0].purpose.individual_analysis = new Merchant.GIS.Element_with_Required();
			New.m_statements[0].purpose.individual_analysis.present = Old.m_statements[0].purpose.individual_analysis.present;
			switch ( Old.m_statements[0].purpose.individual_analysis.required )
			{
				case P3P.REQUIRED.always:
					New.m_statements[0].purpose.individual_analysis.required = Merchant.GIS.REQUIRED.always;
					break;

				case P3P.REQUIRED.opt_out:
					New.m_statements[0].purpose.individual_analysis.required = Merchant.GIS.REQUIRED.opt_out;
					break;

				case P3P.REQUIRED.opt_in:
					New.m_statements[0].purpose.individual_analysis.required = Merchant.GIS.REQUIRED.opt_in;
					break;
			}
			// individual_decision
			New.m_statements[0].purpose.individual_decision = new Merchant.GIS.Element_with_Required();
			New.m_statements[0].purpose.individual_decision.present = Old.m_statements[0].purpose.individual_decision.present;
			switch ( Old.m_statements[0].purpose.individual_decision.required )
			{
				case P3P.REQUIRED.always:
					New.m_statements[0].purpose.individual_decision.required = Merchant.GIS.REQUIRED.always;
					break;

				case P3P.REQUIRED.opt_out:
					New.m_statements[0].purpose.individual_decision.required = Merchant.GIS.REQUIRED.opt_out;
					break;

				case P3P.REQUIRED.opt_in:
					New.m_statements[0].purpose.individual_decision.required = Merchant.GIS.REQUIRED.opt_in;
					break;
			}

			// other_purpose
			New.m_statements[0].purpose.other_purpose = Old.m_statements[0].purpose.other_purpose;
			switch ( Old.m_statements[0].purpose.other_purpose_required )
			{
				case P3P.REQUIRED.always:
					New.m_statements[0].purpose.other_purpose_required = Merchant.GIS.REQUIRED.always;
					break;

				case P3P.REQUIRED.opt_out:
					New.m_statements[0].purpose.other_purpose_required = Merchant.GIS.REQUIRED.opt_out;
					break;

				case P3P.REQUIRED.opt_in:
					New.m_statements[0].purpose.other_purpose_required = Merchant.GIS.REQUIRED.opt_in;
					break;
			}

			// pseudo_analysis
			New.m_statements[0].purpose.pseudo_analysis = new Merchant.GIS.Element_with_Required();
			New.m_statements[0].purpose.pseudo_analysis.present = Old.m_statements[0].purpose.pseudo_analysis.present;
			switch ( Old.m_statements[0].purpose.pseudo_analysis.required )
			{
				case P3P.REQUIRED.always:
					New.m_statements[0].purpose.pseudo_analysis.required = Merchant.GIS.REQUIRED.always;
					break;

				case P3P.REQUIRED.opt_out:
					New.m_statements[0].purpose.pseudo_analysis.required = Merchant.GIS.REQUIRED.opt_out;
					break;

				case P3P.REQUIRED.opt_in:
					New.m_statements[0].purpose.pseudo_analysis.required = Merchant.GIS.REQUIRED.opt_in;
					break;
			}

			// pseudo_decision
			New.m_statements[0].purpose.pseudo_decision = new Merchant.GIS.Element_with_Required();
			New.m_statements[0].purpose.pseudo_decision.present = Old.m_statements[0].purpose.pseudo_decision.present;
			switch ( Old.m_statements[0].purpose.pseudo_decision.required )
			{
				case P3P.REQUIRED.always:
					New.m_statements[0].purpose.pseudo_decision.required = Merchant.GIS.REQUIRED.always;
					break;

				case P3P.REQUIRED.opt_out:
					New.m_statements[0].purpose.pseudo_decision.required = Merchant.GIS.REQUIRED.opt_out;
					break;

				case P3P.REQUIRED.opt_in:
					New.m_statements[0].purpose.pseudo_decision.required = Merchant.GIS.REQUIRED.opt_in;
					break;
			}

			// tailoring
			New.m_statements[0].purpose.tailoring = new Merchant.GIS.Element_with_Required();
			New.m_statements[0].purpose.tailoring.present = Old.m_statements[0].purpose.tailoring.present;
			switch ( Old.m_statements[0].purpose.tailoring.required )
			{
				case P3P.REQUIRED.always:
					New.m_statements[0].purpose.tailoring.required = Merchant.GIS.REQUIRED.always;
					break;

				case P3P.REQUIRED.opt_out:
					New.m_statements[0].purpose.tailoring.required = Merchant.GIS.REQUIRED.opt_out;
					break;

				case P3P.REQUIRED.opt_in:
					New.m_statements[0].purpose.tailoring.required = Merchant.GIS.REQUIRED.opt_in;
					break;
			}

			// telemarketing
			New.m_statements[0].purpose.telemarketing = new Merchant.GIS.Element_with_Required();
			New.m_statements[0].purpose.telemarketing.present = Old.m_statements[0].purpose.telemarketing.present;
			switch ( Old.m_statements[0].purpose.telemarketing.required )
			{
				case P3P.REQUIRED.always:
					New.m_statements[0].purpose.telemarketing.required = Merchant.GIS.REQUIRED.always;
					break;

				case P3P.REQUIRED.opt_out:
					New.m_statements[0].purpose.telemarketing.required = Merchant.GIS.REQUIRED.opt_out;
					break;

				case P3P.REQUIRED.opt_in:
					New.m_statements[0].purpose.telemarketing.required = Merchant.GIS.REQUIRED.opt_in;
					break;
			}
			#endregion

			#region RECIPIENT
			// RECIPIENT
			New.m_statements[0].recipient = new Merchant.GIS.RECIPIENT();

			// public
			New.m_statements[0].recipient._public = new Merchant.GIS.Element_with_Required();
			New.m_statements[0].recipient._public.present = Old.m_statements[0].recipient._public.present;
			switch ( Old.m_statements[0].recipient._public.required )
			{
				case P3P.REQUIRED.always:
					New.m_statements[0].recipient._public.required = Merchant.GIS.REQUIRED.always;
					break;

				case P3P.REQUIRED.opt_in:
					New.m_statements[0].recipient._public.required = Merchant.GIS.REQUIRED.opt_in;
					break;

				case P3P.REQUIRED.opt_out:
					New.m_statements[0].recipient._public.required = Merchant.GIS.REQUIRED.opt_out;
					break;
			}

			// delivery
			New.m_statements[0].recipient.delivery = new Merchant.GIS.Element_with_Required();
			New.m_statements[0].recipient.delivery.present = Old.m_statements[0].recipient.delivery.present;
			switch ( Old.m_statements[0].recipient.delivery.required )
			{
				case P3P.REQUIRED.always:
					New.m_statements[0].recipient.delivery.required = Merchant.GIS.REQUIRED.always;
					break;

				case P3P.REQUIRED.opt_in:
					New.m_statements[0].recipient.delivery.required = Merchant.GIS.REQUIRED.opt_in;
					break;

				case P3P.REQUIRED.opt_out:
					New.m_statements[0].recipient.delivery.required = Merchant.GIS.REQUIRED.opt_out;
					break;
			}

			// other_recipient
			New.m_statements[0].recipient.other_recipient = new Merchant.GIS.Element_with_Required();
			New.m_statements[0].recipient.other_recipient.present = Old.m_statements[0].recipient.other_recipient.present;
			switch ( Old.m_statements[0].recipient.other_recipient.required )
			{
				case P3P.REQUIRED.always:
					New.m_statements[0].recipient.other_recipient.required = Merchant.GIS.REQUIRED.always;
					break;

				case P3P.REQUIRED.opt_in:
					New.m_statements[0].recipient.other_recipient.required = Merchant.GIS.REQUIRED.opt_in;
					break;

				case P3P.REQUIRED.opt_out:
					New.m_statements[0].recipient.other_recipient.required = Merchant.GIS.REQUIRED.opt_out;
					break;
			}

			// ours
			New.m_statements[0].recipient.ours = Old.m_statements[0].recipient.ours;

			// same
			New.m_statements[0].recipient.same = new Merchant.GIS.Element_with_Required();
			New.m_statements[0].recipient.same.present = Old.m_statements[0].recipient.same.present;
			switch ( Old.m_statements[0].recipient.same.required )
			{
				case P3P.REQUIRED.always:
					New.m_statements[0].recipient.same.required = Merchant.GIS.REQUIRED.always;
					break;

				case P3P.REQUIRED.opt_in:
					New.m_statements[0].recipient.same.required = Merchant.GIS.REQUIRED.opt_in;
					break;

				case P3P.REQUIRED.opt_out:
					New.m_statements[0].recipient.same.required = Merchant.GIS.REQUIRED.opt_out;
					break;
			}

			// unrelated
			New.m_statements[0].recipient.unrelated = new Merchant.GIS.Element_with_Required();
			New.m_statements[0].recipient.unrelated.present = Old.m_statements[0].recipient.unrelated.present;
			switch ( Old.m_statements[0].recipient.unrelated.required )
			{
				case P3P.REQUIRED.always:
					New.m_statements[0].recipient.unrelated.required = Merchant.GIS.REQUIRED.always;
					break;

				case P3P.REQUIRED.opt_in:
					New.m_statements[0].recipient.unrelated.required = Merchant.GIS.REQUIRED.opt_in;
					break;

				case P3P.REQUIRED.opt_out:
					New.m_statements[0].recipient.unrelated.required = Merchant.GIS.REQUIRED.opt_out;
					break;
			}
			#endregion

			#region RETENTION
			// Retention
			New.m_statements[0].retention = new Merchant.GIS.RETENTION();
			New.m_statements[0].retention.business_practices = Old.m_statements[0].retention.business_practices;
			New.m_statements[0].retention.indefinitely = Old.m_statements[0].retention.indefinitely;
			New.m_statements[0].retention.legal_requirement = Old.m_statements[0].retention.legal_requirement;
			New.m_statements[0].retention.no_retention = Old.m_statements[0].retention.no_retention;
			New.m_statements[0].retention.stated_purpose = Old.m_statements[0].retention.stated_purpose;
			#endregion

			#region Non-Identifiable
			// non-identifiable
			New.m_statements[0].nonidentifiable = Old.m_statements[0].nonidentifiable;
			#endregion

			// now are we done?

			return New;
		}

		private void bClose_Click(object sender, System.EventArgs e)
		{
			// we accepted whatever is done here
			this.DialogResult = DialogResult.OK;

			//close the window
			this.Close();
		}
	}
}
