using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.Web;
using Microsoft.Web.Services;
using Microsoft.Web.Services.Security;
using Microsoft.Web.Services.Security.X509;
using CommonTypes;

namespace Merchant
{
	/// <summary>
	/// The main form for the Merchant application.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		#region Policies and Policy Management
		/// <summary>
		/// The P3P privacy policy that we send to GIS servers for approval
		/// </summary>
		protected P3P.POLICY m_GISpolicy;
		/// <summary>
		/// The P3P privacy policy that we send to devices and user agents
		/// for approval
		/// </summary>
		protected P3P.POLICY m_devicepolicy;

		/// <summary>
		/// Property Page window for creating and editing P3P policies
		/// </summary>
		protected Merchant.P3PCreator m_p3pEditor;
		#endregion

		/// <summary>
		/// The devices that we have licenses for - users and GIS Server pairs
		/// </summary>
		protected System.Collections.SortedList m_users;

		/// <summary>
		/// Window for showing the devices that are in a particular GIS Server
		/// </summary>
		protected Merchant.UserList m_userWindow;

		/// <summary>
		/// Window for adding a new GIS server info to the list
		/// </summary>
		protected AddGIS m_add_gis;
		/// <summary>
		/// Window for managing the GIS servers information
		/// </summary>
		protected ManageGISServers m_manage_gis;
		/// <summary>
		/// Window for creating new user information.  For Debugging and Examples only!
		/// </summary>
		protected CreateUser m_create_user;
		
		/// <summary>
		/// List of the GIS Servers
		/// </summary>
		protected System.Collections.ArrayList m_gis_servers;

		/// <summary>
		/// Window for registering with and getting information from known GISs
		/// </summary>
		protected Merchant.GISQuery m_gis_query;

		/// <summary>
		/// Our X.509 Security Certificate
		/// </summary>
		protected Microsoft.Web.Services.Security.X509.X509Certificate m_cert;

		/// <summary>
		/// The local certificate store
		/// </summary>
		protected Microsoft.Web.Services.Security.X509.X509CertificateStore m_store;

		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem mFile;
		private System.Windows.Forms.MenuItem mEdit;
		private System.Windows.Forms.MenuItem mLists;
		private System.Windows.Forms.MenuItem mHelp;
		private System.Windows.Forms.MenuItem mListsGISDeviceList;
		private System.Windows.Forms.MenuItem mGISDeviceListReload;
		private System.Windows.Forms.MenuItem mGISDeviceListMakeOffers;
		private System.Windows.Forms.MenuItem mGISDeviceListPushAds;
		private System.Windows.Forms.MenuItem mLicenses;
		private System.Windows.Forms.MenuItem mLicensesViewAll;
		private System.Windows.Forms.MenuItem mLicensesManage;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem mDebugCreateUser;
		private System.Windows.Forms.MenuItem mDebugCreateLicense;
		private System.Windows.Forms.MenuItem mDebugCreateAd;
		private System.Windows.Forms.MenuItem mDebugCreateServer;
		private System.Windows.Forms.MenuItem mDebugViewOffer;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem mQuit;
		private System.Windows.Forms.MenuItem mCheckWSE;
		private System.Windows.Forms.MenuItem mAbout;
		private System.Windows.Forms.MenuItem mAddGIS;
		private System.Windows.Forms.MenuItem mManageGIS;
		private System.Windows.Forms.MenuItem mCheckGIS;
		private System.Windows.Forms.MenuItem mGISPrivacy;
		private System.Windows.Forms.MenuItem mGISPrivacyEdit;
		private System.Windows.Forms.MenuItem mGISPrivacyView;
		private System.Windows.Forms.MenuItem mUsersViewAll;
		private System.Windows.Forms.MenuItem mPendingOffers;
		private System.Windows.Forms.MenuItem mUserPrivacy;
		private System.Windows.Forms.MenuItem mUserPrivacyView;
		private System.Windows.Forms.MenuItem mUserPrivacyEdit;
		private System.Windows.Forms.MenuItem mGISDefaultOK;
		private System.Windows.Forms.MenuItem mGISDefaultNotOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// policies
			this.m_devicepolicy = new P3P.POLICY();
			this.m_devicepolicy.m_statements[0] = new P3P.STATEMENT();
			this.m_GISpolicy = new P3P.POLICY();
			this.m_GISpolicy.m_statements[0] = new P3P.STATEMENT();
			
			// lists
			this.m_users = new SortedList();
			this.m_gis_servers = new ArrayList();

			// child windows
			this.m_p3pEditor = new P3PCreator();
			this.m_userWindow = new UserList();
			this.m_manage_gis = new ManageGISServers();
			this.m_add_gis = new AddGIS();
			this.m_create_user = new CreateUser();
			this.m_gis_query = new GISQuery();

			// certificates
			X509CertificateCollection collection;
			this.m_cert = null;
			// current user store
			m_store = X509CertificateStore.CurrentUserStore(X509CertificateStore.MyStore);
			// open it
			m_store.OpenRead();
			// find the one called merchant
//			collection = m_store.FindCertificateBySubjectName("Merchant");
//			if ( collection.Count > 0 )
//			{
//				this.m_cert = collection[0];
//			}
//
//			// if it didn't work, pop up a window
//			if ( this.m_cert == null )
//				MessageBox.Show(this, "Didn't find certificate for Merchant", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			// for now, just take the first one that we can find
			m_cert = m_store.Certificates[0];

			// if this doesn't work, something is screwy
			if ( m_cert == null )
				MessageBox.Show(this, "Didn't find any current user certificates", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.mFile = new System.Windows.Forms.MenuItem();
			this.mCheckWSE = new System.Windows.Forms.MenuItem();
			this.mQuit = new System.Windows.Forms.MenuItem();
			this.mEdit = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.mAddGIS = new System.Windows.Forms.MenuItem();
			this.mManageGIS = new System.Windows.Forms.MenuItem();
			this.mCheckGIS = new System.Windows.Forms.MenuItem();
			this.mGISPrivacy = new System.Windows.Forms.MenuItem();
			this.mGISPrivacyEdit = new System.Windows.Forms.MenuItem();
			this.mGISPrivacyView = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.mUsersViewAll = new System.Windows.Forms.MenuItem();
			this.mPendingOffers = new System.Windows.Forms.MenuItem();
			this.mUserPrivacy = new System.Windows.Forms.MenuItem();
			this.mUserPrivacyEdit = new System.Windows.Forms.MenuItem();
			this.mUserPrivacyView = new System.Windows.Forms.MenuItem();
			this.mLists = new System.Windows.Forms.MenuItem();
			this.mListsGISDeviceList = new System.Windows.Forms.MenuItem();
			this.mGISDeviceListReload = new System.Windows.Forms.MenuItem();
			this.mGISDeviceListMakeOffers = new System.Windows.Forms.MenuItem();
			this.mGISDeviceListPushAds = new System.Windows.Forms.MenuItem();
			this.mLicenses = new System.Windows.Forms.MenuItem();
			this.mLicensesViewAll = new System.Windows.Forms.MenuItem();
			this.mLicensesManage = new System.Windows.Forms.MenuItem();
			this.mHelp = new System.Windows.Forms.MenuItem();
			this.mAbout = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mDebugCreateUser = new System.Windows.Forms.MenuItem();
			this.mDebugCreateLicense = new System.Windows.Forms.MenuItem();
			this.mDebugCreateAd = new System.Windows.Forms.MenuItem();
			this.mDebugCreateServer = new System.Windows.Forms.MenuItem();
			this.mDebugViewOffer = new System.Windows.Forms.MenuItem();
			this.mGISDefaultOK = new System.Windows.Forms.MenuItem();
			this.mGISDefaultNotOK = new System.Windows.Forms.MenuItem();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mFile,
																					  this.mEdit,
																					  this.menuItem2,
																					  this.menuItem9,
																					  this.mLists,
																					  this.mHelp,
																					  this.menuItem1});
			// 
			// mFile
			// 
			this.mFile.Index = 0;
			this.mFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				  this.mCheckWSE,
																				  this.mQuit});
			this.mFile.Text = "File";
			// 
			// mCheckWSE
			// 
			this.mCheckWSE.Index = 0;
			this.mCheckWSE.Text = "Check WSEmail";
			// 
			// mQuit
			// 
			this.mQuit.Index = 1;
			this.mQuit.Text = "Quit";
			this.mQuit.Click += new System.EventHandler(this.mQuit_Click);
			// 
			// mEdit
			// 
			this.mEdit.Index = 1;
			this.mEdit.Text = "Edit";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 2;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mAddGIS,
																					  this.mManageGIS,
																					  this.mCheckGIS,
																					  this.mGISPrivacy});
			this.menuItem2.Text = "GIS Servers";
			// 
			// mAddGIS
			// 
			this.mAddGIS.Index = 0;
			this.mAddGIS.Text = "Add new...";
			this.mAddGIS.Click += new System.EventHandler(this.mAddGIS_Click);
			// 
			// mManageGIS
			// 
			this.mManageGIS.Index = 1;
			this.mManageGIS.Text = "Manage...";
			this.mManageGIS.Click += new System.EventHandler(this.mManageGIS_Click);
			// 
			// mCheckGIS
			// 
			this.mCheckGIS.Index = 2;
			this.mCheckGIS.Text = "Check GIS Servers...";
			this.mCheckGIS.Click += new System.EventHandler(this.mCheckGIS_Click);
			// 
			// mGISPrivacy
			// 
			this.mGISPrivacy.Index = 3;
			this.mGISPrivacy.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.mGISPrivacyEdit,
																						this.mGISPrivacyView,
																						this.mGISDefaultOK,
																						this.mGISDefaultNotOK});
			this.mGISPrivacy.Text = "Privacy Policy";
			// 
			// mGISPrivacyEdit
			// 
			this.mGISPrivacyEdit.Index = 0;
			this.mGISPrivacyEdit.Text = "Edit...";
			this.mGISPrivacyEdit.Click += new System.EventHandler(this.mGISPrivacyEdit_Click);
			// 
			// mGISPrivacyView
			// 
			this.mGISPrivacyView.Index = 1;
			this.mGISPrivacyView.Text = "View";
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 3;
			this.menuItem9.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mUsersViewAll,
																					  this.mPendingOffers,
																					  this.mUserPrivacy});
			this.menuItem9.Text = "Users";
			// 
			// mUsersViewAll
			// 
			this.mUsersViewAll.Index = 0;
			this.mUsersViewAll.Text = "View All Users";
			this.mUsersViewAll.Click += new System.EventHandler(this.mUsersViewAll_Click);
			// 
			// mPendingOffers
			// 
			this.mPendingOffers.Index = 1;
			this.mPendingOffers.Text = "Pending Offers";
			// 
			// mUserPrivacy
			// 
			this.mUserPrivacy.Index = 2;
			this.mUserPrivacy.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.mUserPrivacyEdit,
																						 this.mUserPrivacyView});
			this.mUserPrivacy.Text = "Privacy Policy";
			// 
			// mUserPrivacyEdit
			// 
			this.mUserPrivacyEdit.Index = 0;
			this.mUserPrivacyEdit.Text = "Edit..";
			this.mUserPrivacyEdit.Click += new System.EventHandler(this.mUserPrivacyEdit_Click);
			// 
			// mUserPrivacyView
			// 
			this.mUserPrivacyView.Index = 1;
			this.mUserPrivacyView.Text = "View";
			// 
			// mLists
			// 
			this.mLists.Index = 4;
			this.mLists.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				   this.mListsGISDeviceList,
																				   this.mLicenses});
			this.mLists.Text = "Lists";
			// 
			// mListsGISDeviceList
			// 
			this.mListsGISDeviceList.Index = 0;
			this.mListsGISDeviceList.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								this.mGISDeviceListReload,
																								this.mGISDeviceListMakeOffers,
																								this.mGISDeviceListPushAds});
			this.mListsGISDeviceList.Text = "GIS Device List";
			// 
			// mGISDeviceListReload
			// 
			this.mGISDeviceListReload.Index = 0;
			this.mGISDeviceListReload.Text = "Reload";
			// 
			// mGISDeviceListMakeOffers
			// 
			this.mGISDeviceListMakeOffers.Index = 1;
			this.mGISDeviceListMakeOffers.Text = "Make Offers";
			// 
			// mGISDeviceListPushAds
			// 
			this.mGISDeviceListPushAds.Index = 2;
			this.mGISDeviceListPushAds.Text = "Push Ads";
			// 
			// mLicenses
			// 
			this.mLicenses.Index = 1;
			this.mLicenses.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mLicensesViewAll,
																					  this.mLicensesManage});
			this.mLicenses.Text = "Licenses";
			// 
			// mLicensesViewAll
			// 
			this.mLicensesViewAll.Index = 0;
			this.mLicensesViewAll.Text = "View All";
			// 
			// mLicensesManage
			// 
			this.mLicensesManage.Index = 1;
			this.mLicensesManage.Text = "Manage";
			// 
			// mHelp
			// 
			this.mHelp.Index = 5;
			this.mHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				  this.mAbout});
			this.mHelp.Text = "Help";
			// 
			// mAbout
			// 
			this.mAbout.Index = 0;
			this.mAbout.Text = "About";
			this.mAbout.Click += new System.EventHandler(this.mAbout_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 6;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mDebugCreateUser,
																					  this.mDebugCreateLicense,
																					  this.mDebugCreateAd,
																					  this.mDebugCreateServer,
																					  this.mDebugViewOffer});
			this.menuItem1.Text = "Debug";
			// 
			// mDebugCreateUser
			// 
			this.mDebugCreateUser.Index = 0;
			this.mDebugCreateUser.Text = "Create User";
			this.mDebugCreateUser.Click += new System.EventHandler(this.mDebugCreateUser_Click);
			// 
			// mDebugCreateLicense
			// 
			this.mDebugCreateLicense.Index = 1;
			this.mDebugCreateLicense.Text = "Create License";
			// 
			// mDebugCreateAd
			// 
			this.mDebugCreateAd.Index = 2;
			this.mDebugCreateAd.Text = "Create Ad";
			// 
			// mDebugCreateServer
			// 
			this.mDebugCreateServer.Index = 3;
			this.mDebugCreateServer.Text = "Create GIS Server";
			// 
			// mDebugViewOffer
			// 
			this.mDebugViewOffer.Index = 4;
			this.mDebugViewOffer.Text = "XML Tree View";
			this.mDebugViewOffer.Click += new System.EventHandler(this.mDebugViewOffer_Click);
			// 
			// mGISDefaultOK
			// 
			this.mGISDefaultOK.Index = 2;
			this.mGISDefaultOK.Text = "Default Ok Policy";
			this.mGISDefaultOK.Click += new System.EventHandler(this.mGISDefaultOK_Click);
			// 
			// mGISDefaultNotOK
			// 
			this.mGISDefaultNotOK.Index = 3;
			this.mGISDefaultNotOK.Text = "Default Not-Ok Policy";
			this.mGISDefaultNotOK.Click += new System.EventHandler(this.mGISDefaultNotOK_Click);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(440, 309);
			this.IsMdiContainer = true;
			this.Menu = this.mainMenu1;
			this.Name = "MainForm";
			this.Text = "PDRM Manager";

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
		}

		#region Menu Items
		private void mAbout_Click(object sender, System.EventArgs e)
		{
			// show the about window
			Merchant.About abt = new About();
			abt.ShowDialog(this);
		}

		private void mQuit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void mUserPrivacyEdit_Click(object sender, System.EventArgs e)
		{
			DialogResult res;

			// now create a window to show the P3P policy editing in
			this.m_p3pEditor.Policy = this.m_devicepolicy;

			// show it
			res = this.m_p3pEditor.ShowDialog(this);

			// if it was saved, grab it
			if ( res == DialogResult.OK )
			{
				this.m_devicepolicy = this.m_p3pEditor.Policy;
			}

			return;
		}

		private void mGISPrivacyEdit_Click(object sender, System.EventArgs e)
		{
			DialogResult res;

			// now create a window to show the P3P policy editing in
			this.m_p3pEditor.Policy = this.m_GISpolicy;

			// show it
			res = this.m_p3pEditor.ShowDialog(this);

			// if it was saved, grab it
			if ( res == DialogResult.OK )
			{
				this.m_GISpolicy = this.m_p3pEditor.Policy;
			}

			return;	
		}

		private void mUsersViewAll_Click(object sender, System.EventArgs e)
		{
			DialogResult res;

			// show the available licenses in the device list window
			this.m_userWindow.Users = this.m_users;

			// show it
			res = this.m_userWindow.ShowDialog();

			// if it's saved, then let's save the results here
			if ( res == DialogResult.OK )
			{
				this.m_users = this.m_userWindow.Users;
			}		
		}

		private void mAddGIS_Click(object sender, System.EventArgs e)
		{
			DialogResult res;
			GISServer gs;

			// show the add new gis server dialog box
			this.m_add_gis.Server = new GISServer();
			res = this.m_add_gis.ShowDialog(this);

			// if it's an ok, then save the new one
			if ( res == DialogResult.OK )
			{
				gs = m_add_gis.Server;

				this.m_gis_servers.Add(gs);
			}
		}

		private void mManageGIS_Click(object sender, System.EventArgs e)
		{
			// show the manage GIS Servers window
			this.m_manage_gis.Servers = this.m_gis_servers;

			// show it
			this.m_manage_gis.ShowDialog(this);

			// if things changed, save it
			if ( this.m_manage_gis.Dirty )
			{
				this.m_gis_servers = this.m_manage_gis.Servers;
			}
		}

		private void mDebugCreateUser_Click(object sender, System.EventArgs e)
		{
			DialogResult res;

			// we want to make up a user to play with in the program			
			m_create_user.GISServers = this.m_gis_servers;

			// now show the window
			res = m_create_user.ShowDialog(this);

			// if we have a new user
			if ( res == DialogResult.OK )
			{
				// add the user to the users list
				this.m_users.Add(m_create_user.theUser, m_create_user.GIS);
			}

			// we're done
			return;		
		}

		private void mDebugViewOffer_Click(object sender, System.EventArgs e)
		{
			// make a new XML Tree View window
			Merchant.XMLTreeView xtv = new XMLTreeView();

			// now show it
			xtv.ShowDialog(this);
		}
		#endregion

		private void mCheckGIS_Click(object sender, System.EventArgs e)
		{
			// we must show a dialog box to allow the user to query various GIS Servers
			// the list of GISs that we know about
			this.m_gis_query.GISList = this.m_gis_servers;

			// policy to send out
			this.m_gis_query.Policy = this.m_GISpolicy;

			// cert to use
			this.m_gis_query.Cert = this.m_cert;

			// show the window
			this.m_gis_query.ShowDialog(this);

			// now fetch the results if there are any
			this.AddUsers(m_gis_query.New_Users);

			// done
		}

		/// <summary>
		/// Takes a sorted list of User/GIS pairs to add to the master User list.  In case there
		/// was a User already in the system identical to one on the new list, the old one is
		/// removed and replaced with the new one.  The newList remains unchanged.
		/// </summary>
		/// <param name="newList">List of new users to add to the User list</param>
		/// <returns>true if the master user list was changed.  false otherwise</returns>
		private bool AddUsers(System.Collections.SortedList newList)
		{
			bool changed = false;

			User user;

			// we must take the users that we have here and add them to the existing list
			// make sure that we don't have doubles!

			foreach(Object obj in newList.Keys )
			{
				// check if it's null
				if ( obj == null )
					break;

				// now cast it to a user
				user = obj as User;

				// remove any old copies since the last time entered - we have a new time now
				// this won't fail, even if the user wasn't in there before
				this.m_users.Remove(user);

				// now add it with its new GIS information
				this.m_users.Add(user, newList[user]);

				// we changed something
				changed = true;
			}

			// if we added anything return true
			return changed;
		}

		private void mGISDefaultOK_Click(object sender, System.EventArgs e)
		{
			// load in a P3P policy that is rather conservative

			// this one really is just a copy from the GIS default policy
			P3P.STATEMENT s1 = new P3P.STATEMENT();
			s1.categories.computer = true;
			s1.categories.demographic = true;
			s1.categories.financial = true;
		
			s1.consequence = "College kids love pizza";

			s1.nonidentifiable = true;

			s1.purpose.admin.present = true;
			s1.purpose.admin.required = P3P.REQUIRED.always;
			s1.purpose.develop.present = true;
			s1.purpose.pseudo_analysis.present = true;

			s1.recipient._public.present = true;
			s1.recipient.other_recipient.present = true;

			s1.retention.business_practices = true;
			s1.retention.stated_purpose = true;

			// put it all together and store it
			this.m_GISpolicy.m_statements[0] = s1;

			// add one access
			this.m_GISpolicy.access.none = true;
		}

		private void mGISDefaultNotOK_Click(object sender, System.EventArgs e)
		{
			// let's make a P3P policy that is more leanient than the one the GIS will accept
			P3P.STATEMENT s1 = new P3P.STATEMENT();
			s1.categories.computer = true;
			s1.categories.demographic = true;
			s1.categories.financial = true;
			s1.categories.health = true;
		
			s1.consequence = "College kids love pizza";

			s1.purpose.admin.present = true;
			s1.purpose.admin.required = P3P.REQUIRED.always;
			s1.purpose.develop.present = true;
			s1.purpose.pseudo_analysis.present = true;
			s1.purpose.individual_decision.present = true;
			s1.purpose.individual_decision.required = P3P.REQUIRED.always;

			s1.recipient._public.present = true;
			s1.recipient.other_recipient.present = true;
			s1.recipient.unrelated.present = true;

			s1.retention.business_practices = true;
			s1.retention.stated_purpose = true;
			s1.retention.indefinitely = true;

			// put it all together and store it
			this.m_GISpolicy.m_statements[0] = s1;
		}


	}
}
