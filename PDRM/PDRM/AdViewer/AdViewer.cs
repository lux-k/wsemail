using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using OpenNETCF;
using OpenNETCF.Notification;
using CommonTypes;

namespace AdViewer
{
	/// <summary>
	/// Application to view ads and the like on a PDA
	/// </summary>
	public class AdWindow: System.Windows.Forms.Form
	{
		/// <summary>
		/// The port that we will talk to the Ad Server on
		/// </summary>
		public const int AD_PORT = 8080;

		/// <summary>
		/// The client object for listening to a TCP connection
		/// </summary>
		protected System.Net.Sockets.TcpClient m_tcp; 

		/// <summary>
		/// The adapter information for this device
		/// </summary>
		protected IP_ADAPTER_INFO m_adapter_info;

		/// <summary>
		/// The GLS web service that we can query to get GIS information
		/// </summary>
		protected AdViewer.GLS.GLS m_gls;

		/// <summary>
		/// The GIS that we are currently registered in
		/// </summary>
		protected AdViewer.GIS.GIS m_gis;

		/// <summary>
		/// The User item that describes us
		/// </summary>
		protected CommonTypes.User m_user;

		/// <summary>
		/// Window to show the GLS information
		/// </summary>
		protected AdViewer.GLSInfo m_gls_info;

		/// <summary>
		/// The Ads that we have
		/// </summary>
		protected System.Collections.ArrayList m_ads;

		/// <summary>
		/// The notification engine we use to show message bubbles
		/// </summary>
		protected OpenNETCF.Notification.NotificationEngine m_engine;

		/// <summary>
		/// The id number for the messages that we have displayed
		/// </summary>
		protected int m_msg_counter;

		/// <summary>
		/// Message window to view the details of an ad
		/// </summary>
		protected AdViewer.AdDetailView m_ad_detail;

		/// <summary>
		/// The About window
		/// </summary>
		protected AdViewer.About m_about;

		private System.Windows.Forms.MainMenu mMain;
		private System.Windows.Forms.ListView lAds;
		private System.Windows.Forms.ColumnHeader clText;
		private System.Windows.Forms.ColumnHeader clDate;
		private System.Windows.Forms.ColumnHeader clSource;
		private System.Windows.Forms.MenuItem mFile;
		private System.Windows.Forms.MenuItem mQuit;
		private System.Windows.Forms.MenuItem mDisplay;
		private System.Windows.Forms.MenuItem mRefresh;
		private System.Windows.Forms.MenuItem mHideOld;
		private System.Windows.Forms.MenuItem mViewAll;
		private System.Windows.Forms.MenuItem mItems;
		private System.Windows.Forms.MenuItem mRemoveSelected;
		private System.Windows.Forms.MenuItem mDetails;
		private System.Windows.Forms.MenuItem mLeave;
		private System.Windows.Forms.MenuItem mEnter;
		private System.Windows.Forms.MenuItem mHelp;
		private System.Windows.Forms.MenuItem mAbout;
		private System.Windows.Forms.MenuItem mShowGLS;
		private System.Windows.Forms.Timer tAdTimer;
		private System.Windows.Forms.ColumnHeader clType;

		public AdWindow()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// the tcp client object
			m_tcp = new TcpClient();

			// gls info window
			m_gls_info = new GLSInfo();

			// ad detail window
			this.m_ad_detail = new AdDetailView();

			// acquire the default gateway information
			m_adapter_info = this.getAdapters();

			// now from the first default gateway, setup to query the GLS webservice
			this.m_gls = this.SetupGLS(m_adapter_info);
		
			// now setup the GIS information for the AdViewer
			this.m_gis = new AdViewer.GIS.GIS();			

			// set the user data to be us
			m_user = new CommonTypes.User();
			m_user.Name = "Joe Klein";
			m_user.Address = "JoeKlein@proxy.isp.net";
			m_user.EmailAddress = "JoeKlein@wsemail.cis.upenn.edu";

			// the ads we have
			this.m_ads = new ArrayList(256);

			// The notification engine initialization to some silly Guid
			m_engine = new NotificationEngine(new Guid("12345678901234567890123456789012"));

			// number of messsages that we have sent so far
			this.m_msg_counter = 0;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mMain = new System.Windows.Forms.MainMenu();
			this.mFile = new System.Windows.Forms.MenuItem();
			this.mEnter = new System.Windows.Forms.MenuItem();
			this.mLeave = new System.Windows.Forms.MenuItem();
			this.mQuit = new System.Windows.Forms.MenuItem();
			this.mDisplay = new System.Windows.Forms.MenuItem();
			this.mRefresh = new System.Windows.Forms.MenuItem();
			this.mHideOld = new System.Windows.Forms.MenuItem();
			this.mViewAll = new System.Windows.Forms.MenuItem();
			this.mItems = new System.Windows.Forms.MenuItem();
			this.mRemoveSelected = new System.Windows.Forms.MenuItem();
			this.mDetails = new System.Windows.Forms.MenuItem();
			this.mHelp = new System.Windows.Forms.MenuItem();
			this.mShowGLS = new System.Windows.Forms.MenuItem();
			this.mAbout = new System.Windows.Forms.MenuItem();
			this.lAds = new System.Windows.Forms.ListView();
			this.clText = new System.Windows.Forms.ColumnHeader();
			this.clSource = new System.Windows.Forms.ColumnHeader();
			this.clDate = new System.Windows.Forms.ColumnHeader();
			this.clType = new System.Windows.Forms.ColumnHeader();
			this.tAdTimer = new System.Windows.Forms.Timer();
			// 
			// mMain
			// 
			this.mMain.MenuItems.Add(this.mFile);
			this.mMain.MenuItems.Add(this.mDisplay);
			this.mMain.MenuItems.Add(this.mItems);
			this.mMain.MenuItems.Add(this.mHelp);
			// 
			// mFile
			// 
			this.mFile.MenuItems.Add(this.mEnter);
			this.mFile.MenuItems.Add(this.mLeave);
			this.mFile.MenuItems.Add(this.mQuit);
			this.mFile.Text = "File";
			this.mFile.Click += new System.EventHandler(this.mFile_Click);
			// 
			// mEnter
			// 
			this.mEnter.Text = "Enter GIS";
			this.mEnter.Click += new System.EventHandler(this.mEnter_Click);
			// 
			// mLeave
			// 
			this.mLeave.Enabled = false;
			this.mLeave.Text = "Leave GIS";
			this.mLeave.Click += new System.EventHandler(this.mLeave_Click);
			// 
			// mQuit
			// 
			this.mQuit.Text = "Quit";
			this.mQuit.Click += new System.EventHandler(this.mQuit_Click);
			// 
			// mDisplay
			// 
			this.mDisplay.MenuItems.Add(this.mRefresh);
			this.mDisplay.MenuItems.Add(this.mHideOld);
			this.mDisplay.MenuItems.Add(this.mViewAll);
			this.mDisplay.Text = "Display";
			// 
			// mRefresh
			// 
			this.mRefresh.Text = "Refresh Ads";
			// 
			// mHideOld
			// 
			this.mHideOld.Text = "Hide Old";
			// 
			// mViewAll
			// 
			this.mViewAll.Text = "View All";
			// 
			// mItems
			// 
			this.mItems.MenuItems.Add(this.mRemoveSelected);
			this.mItems.MenuItems.Add(this.mDetails);
			this.mItems.Text = "Items";
			// 
			// mRemoveSelected
			// 
			this.mRemoveSelected.Text = "Remove Selected";
			// 
			// mDetails
			// 
			this.mDetails.Text = "Details";
			this.mDetails.Click += new System.EventHandler(this.mDetails_Click);
			// 
			// mHelp
			// 
			this.mHelp.MenuItems.Add(this.mShowGLS);
			this.mHelp.MenuItems.Add(this.mAbout);
			this.mHelp.Text = "Help";
			// 
			// mShowGLS
			// 
			this.mShowGLS.Text = "Show GLS Info";
			this.mShowGLS.Click += new System.EventHandler(this.mShowGLS_Click);
			// 
			// mAbout
			// 
			this.mAbout.Text = "About";
			this.mAbout.Click += new System.EventHandler(this.mAbout_Click);
			// 
			// lAds
			// 
			this.lAds.Columns.Add(this.clText);
			this.lAds.Columns.Add(this.clSource);
			this.lAds.Columns.Add(this.clDate);
			this.lAds.Columns.Add(this.clType);
			this.lAds.FullRowSelect = true;
			this.lAds.Location = new System.Drawing.Point(8, 8);
			this.lAds.Size = new System.Drawing.Size(272, 256);
			this.lAds.View = System.Windows.Forms.View.Details;
			// 
			// clText
			// 
			this.clText.Text = "Text";
			this.clText.Width = 60;
			// 
			// clSource
			// 
			this.clSource.Text = "Source";
			this.clSource.Width = 91;
			// 
			// clDate
			// 
			this.clDate.Text = "Time";
			this.clDate.Width = 59;
			// 
			// clType
			// 
			this.clType.Text = "Type";
			this.clType.Width = 60;
			// 
			// tAdTimer
			// 
			this.tAdTimer.Interval = 10000;
			this.tAdTimer.Tick += new System.EventHandler(this.tAdTimer_Tick);
			// 
			// AdWindow
			// 
			this.ClientSize = new System.Drawing.Size(290, 270);
			this.Controls.Add(this.lAds);
			this.Menu = this.mMain;
			this.Text = "Mobile Ads";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.AdWindow_Closing);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>

		static void Main() 
		{
			Application.Run(new AdViewer.AdWindow());
		}

		#region Initialization
		/// <summary>
		/// Use the AdapterInfo class to retrieve the AdapterInfo object for the device
		/// </summary>
		/// <returns>AdapterInfo object for the class.  null if there's an error.</returns>
		protected AdViewer.IP_ADAPTER_INFO getAdapters()
		{
			AdViewer.IP_ADAPTER_INFO res;

			// get the result
			res = AdViewer.AdapterInfo.GetInfo();

			// if it's null, there was an error, so show it
			if ( res == null )
			{
				MessageBox.Show("Error: Can't get adapter information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
			
			// return it anyway
			return res;
		}
		/// <summary>
		/// Setup a GLS Web Proxy object to query based on the default gateway information
		/// that we get from the AdapaterInfo object passed in.  If no GLS web service can
		/// be found, an error message is raised
		/// </summary>
		/// <param name="info">The AdapaterInfo object that describes the
		/// properties for this device.</param>
		/// <returns>A GLS object that has its URL property set to the first non-empty
		/// address in the default gateway list.  If no such address can be found, the
		/// proxy object defaults to http://localhost which is guaranteed not to work
		/// unless the GLS is running on the device itself.</returns>
		protected AdViewer.GLS.GLS SetupGLS(AdViewer.IP_ADAPTER_INFO info)
		{
			AdViewer.GLS.GLS gls = new AdViewer.GLS.GLS();

			// extract the default gateway information from the AdapterInfo structure
			IP_ADDR_STRING str;
			str = info.GatewayList;

			// find the first non-zero gateway
			// and non-empty gateway - 192.168.131.254 is fake
			while ( str != null && str.IpAddress != null && str.IpAddress.String != "" )
			{
				// if the default gateway listed isn't fake
				try
				{
					System.Net.Dns.GetHostByAddress(str.IpAddress.String);
				}
				catch ( System.Exception e )
				{
					// something didn't work here, so try again
					str = str.Next;
					continue;
				}

				// if it's non-fake,
				if ( !str.IpAddress.String.StartsWith("192.168") )
				{
					// make it the url for the GLS
					gls.Url = "http://" + str.IpAddress.String + "//GLS//getGIS";

					// now return it
					return gls;
				}
				else
				{
					// move on
					str = str.Next;
				}
			}

			// we didn't get a non-zero one, so we have an error
			MessageBox.Show("Error: Couldn't find GLS on default gateway.", "Can't find GLS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
			return gls;
		}
		#endregion

		private void mEnter_Click(object sender, System.EventArgs e)
		{
			// the current gis address
			string g = m_gls.GetGIS();

			// we want to enter the GIS as listed here
			m_gis.Url = "http://" + g;
			m_gis.Url += "/GIS/GIS.asmx";

			bool result;

			// now register ourselves with the GIS
			result = m_gis.RegisterUser(Convert.ToLocal(this.m_user));

			// if we aren't added, put up a notice
			if ( result == false )
			{
				MessageBox.Show("Error: Not added to User list on GIS", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
			}

			// now activate the Leave GIS menu item
			this.mLeave.Enabled = true;
			// and deactive this one
			this.mEnter.Enabled = false;
	
			// Deprecated - using TCP to connect to 
			// make a TCP connection to the the GIS to collect ads and discounts			
			/* System.Net.IPHostEntry info = System.Net.Dns.GetHostByAddress(g);
			* this.m_tcp.Connect(info.AddressList[0], AD_PORT);
			*
			// now send out our information to the ad server
			*System.Net.Sockets.NetworkStream outStream;
			*Byte[] outBytes = new Byte[256];
			*outStream = this.m_tcp.GetStream();
			*outBytes = System.Text.Encoding.ASCII.GetBytes(this.m_user.EmailAddress + " " + this.m_user.Address);
			*outStream.Write(outBytes, 0, outBytes.Length);
			*
			// get the first batch of ads
			*this.RefreshAdsByTCP();
			*/

			// Update ads by Web Service
			this.RefreshAdsByWebService();

			// now set the timer to wake up and refresh the ads every so often
			this.tAdTimer.Enabled = true;
		}

		private void mShowGLS_Click(object sender, System.EventArgs e)
		{
			// show a message box with the GLS information that we have
			m_gls_info.GLS_IP = new Uri(this.m_gls.Url);
			m_gls_info.GLS_LOC = this.m_gls.GetLoc();
			m_gls_info.GIS_IP = new Uri("http://" + this.m_gls.GetGIS());

			// show it
			m_gls_info.ShowDialog();
		}

		private void mLeave_Click(object sender, System.EventArgs e)
		{
			// we want to leave the GIS that we are currently in
			this.m_gis.RemoveUser(Convert.ToLocal(this.m_user));

			// now disable this item
			this.mLeave.Enabled = false;
			// and reenble entry
			this.mEnter.Enabled = true;
		}

		private void tAdTimer_Tick(object sender, System.EventArgs e)
		{
			// refresh the ads that we see
			this.RefreshAdsByWebService();
		}

		/// <summary>
		/// Refresh the ads in the window by querying a Web Service at the Ad Server (GIS
		/// as of now).  Adds the new ads to the Ad List and to the listview.
		/// </summary>
		private void RefreshAdsByWebService()
		{
			// we must connect to the GIS server which is acting as our ad proxy
			// and get the new ads
			ArrayList ads = new ArrayList(this.m_gis.GetNewAds(Convert.ToLocal(this.m_user)));

			CommonTypes.AdMessage ad;
			AdViewer.GIS.AdMessage Gad;

			// if there are no new ads to show leave
			if ( ads.Count == 0 )
			{
				return;
			}
				// otherwise show them!
			else
			{
				// add them to the views
				foreach (Object obj in ads )
				{
					// make sure it's not null
					if ( obj == null )
						break;

					// get it as a GIS version
					Gad = obj as AdViewer.GIS.AdMessage;

					// convert it
					ad = Convert.ToCommon(Gad);

					// set the time
					ad.Time = DateTime.Now;

					// add the new Ad to the array
					this.m_ads.Add(ad);

					// add it to the listview
					this.lAds.Items.Add(
						new ListViewItem(
						new string[] {	ad.AdText,
										 ad.Source,
										 ad.Time.ToString(),
										 ad.TypeString}));
				}

				// show a user notification window to announce the new ads

				// create it
				OpenNETCF.Notification.Notification notif = new Notification();
				// 30 seconds. should this be longer?
				notif.Duration = 30;
				// notification blurb says:
				notif.HTML = "<b>New ads for you!</b>";
				// unique id number
				notif.ID = this.m_msg_counter;
				this.m_msg_counter++;
				// priority - maybe make this something else?
				notif.Priority = OpenNETCF.Notification.Priority.Inform;
				// title to display
				notif.Title = "New Ads from GIS";

				// now post it
				this.m_engine.Add(notif);			
			}

			// we have the new ads, now we are done
			return;
		}

		/// <summary>
		/// Converts a GIS version of an AdMessage to a local version
		/// </summary>
		/// <param name="msg">The GIS version of the ad</param>
		/// <returns>The local version of the ad</returns>
/*		private CommonTypes.AdMessage ConvertAd(GIS.AdMessage msg)
		{
			AdViewer.AdMessage newMsg = new AdMessage();

			newMsg.AdText = msg.AdText;
			newMsg.Source = msg.Source;
			switch (msg.Type)
			{
				case GIS.AdType.Discount:
					newMsg.Type = AdViewer.AdType.Discount;
					break;

				case GIS.AdType.NormalAd:
					newMsg.Type = AdViewer.AdType.NormalAd;
					break;

				case GIS.AdType.OffensiveAd:
					newMsg.Type = AdViewer.AdType.OffensiveAd;
					break;
			}

			return newMsg;
		}*/



		/// <summary>
		/// Read the ads from the TCP connection and put them into the
		/// listview to be shown
		/// </summary>
		private void RefreshAdsByTCP()
		{
			// the stream of data coming in
			System.Net.Sockets.NetworkStream inStream;
			// byte array of incoming data
			Byte[] bytesIn = new Byte[2048];
			// converted the bytes to this string
			string inString = "";
			// temps
			string chunk;
			int i;
			// new ad to add to the collections
			AdMessage newAd;
			// whether we have any new data or not
			bool haveNew = false;

			// we must read from the socket all of the ads that we are getting
			inStream = this.m_tcp.GetStream();

			// if there is data to read
			while ( this.m_tcp.GetStream().DataAvailable )
			{
				// read it in pieces of 2048 bytes max
				// chunk
				inStream.Read(bytesIn, 0, bytesIn.Length);
				
				// turn the chunk into a string and tack it onto the end
				inString += System.Text.Encoding.ASCII.GetString(bytesIn, 0, bytesIn.Length);
			}

			// now parse the string with the data
			while ( inString != "" )
			{
				// read until the first double end of line
				i = inString.IndexOf("\n\n", 0);
				chunk = inString.Substring(0, i);

				// turn the chunk into an Ad
				newAd = new AdMessage(chunk);

				// add the new Ad to the array
				this.m_ads.Add(newAd);

				// add it to the listview
				this.lAds.Items.Add(
					new ListViewItem(
					new string[] {
									 newAd.AdText,
									 newAd.Source,
									 newAd.Time.ToString(),
									 newAd.TypeString}));

				// now chop up the old piece
				// skip the two end-lines
				i += 2;

				// chop
				inString = inString.Substring(i, inString.Length-i);

				// we have some new ad
				haveNew = true;
			}

			// if we have new data, send a notification
			if (haveNew)
			{
				// create it
				OpenNETCF.Notification.Notification notif = new Notification();
				// 30 seconds. should this be longer?
				notif.Duration = 30;
				// notification blurb says:
				notif.HTML = "<b>New ads for you!</b>";
				// unique id number
				notif.ID = this.m_msg_counter;
				this.m_msg_counter++;
				// priority - maybe make this something else?
				notif.Priority = OpenNETCF.Notification.Priority.Inform;
				// title to display
				notif.Title = "New Ads from GIS";

				// now post it
				this.m_engine.Add(notif);			
			}

			// now we're done
			return;
		}

		private void mDetails_Click(object sender, System.EventArgs e)
		{
			// get the selected Ad item from the window and display some
			// details about it
			int loc = this.lAds.SelectedIndices[0];

			// now pull out that one
			System.Windows.Forms.ListViewItem sel = this.lAds.Items[loc];
			
			// get the ad that we want
			AdMessage msg = this.FindAd(sel);

			// if it's null, something is wrong
			if ( msg == null )
			{
				MessageBox.Show("Error: Unable to load message", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
			}
				// otherwise we have a message, so show it
			else
			{
				// post the message to the window
				m_ad_detail.Ad = msg;

				// show it
				m_ad_detail.ShowDialog();
			}			
		}		
	
		/// <summary>
		/// Searches the stored Ads in the Ad Viewer for one that is described by the particular
		/// ListViewItem passed in.  The comparison is based solely on the Ad text, so
		/// if two ads have the same text, one of them is sent back
		/// </summary>
		/// <param name="lvi">The ListViewItem the describes that Ad being searched for</param>
		/// <returns>The AdMessage object that contains the text of the ListViewItem.  If none is
		/// found, null is returned.</returns>
		protected CommonTypes.AdMessage FindAd(System.Windows.Forms.ListViewItem lvi)
		{
			// take the given listview item and search the ads we have for it

			// temp messsage
			CommonTypes.AdMessage msg = null;

			bool found = false;

			foreach ( Object obj in this.m_ads )
			{
				// make sure it's not null
				if ( obj != null && !found)					
				{
					// now cast it to a message
					msg = obj as AdMessage;

					// now see if it matches the listview item
					if ( msg.AdText.CompareTo(lvi.Text) == 0 )
					{
						// save it and quit
						found = true;
						break;
					}
					
					// otherwise keep searching
				}
			}

			// if we found it, return the message
			if ( found )
			{
				return msg;
			}
			else
			{
				return null;
			}
		}

		private void mQuit_Click(object sender, System.EventArgs e)
		{
			// close up shop
			this.Close();
		}

		private void mAbout_Click(object sender, System.EventArgs e)
		{
			// only load up the about window when someone asks about it
			m_about =new About();
			// show the about window
			m_about.ShowDialog();
		}

		private void mFile_Click(object sender, System.EventArgs e)
		{
		
		}

		private void AdWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// leave the GIS
			this.m_gis.RemoveUser(Convert.ToLocal(this.m_user));
		}



			

			

	}
}
