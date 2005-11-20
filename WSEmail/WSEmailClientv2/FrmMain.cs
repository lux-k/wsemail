/*



Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Configuration;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.Web.Services2.Security;
using Microsoft.Web.Services2.Security.Tokens;

using WSEmailProxy;
using XmlAddressBook;

using FederatedBinaryToken;
using System.Security.Cryptography;

namespace WSEmailClientv2
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class FrmMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ImageList listViewImage;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.ToolBarButton toolBarButton2;
		private System.Windows.Forms.ToolBarButton toolBarButton3;
		private System.Windows.Forms.ToolBarButton toolBarButton4;
		private System.Windows.Forms.ToolBarButton toolBarButton5;
		private System.Windows.Forms.ToolBarButton toolBarButton6;
		private System.Windows.Forms.ToolBarButton toolBarButton7;
		private System.Windows.Forms.ToolBarButton toolBarButton8;
		private System.Windows.Forms.ToolBarButton toolBarButton9;
		private FrmMessages frmMessages = null;
		private System.Windows.Forms.ToolBarButton toolBarButton10;
		private System.Windows.Forms.ToolBarButton toolBarButton11;
		private FrmStatus frmStatus = null;
		private FrmIMMain frmIMMain = null;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem2;
		private FrmAddressBook frmAddrBook = null;

		public FrmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.Text = "WS Email Client v" + Application.ProductVersion;
			//this.BackColor = Color.FromArgb(160,183,224);
/*			foreach (Control c in this.Controls) 
			{

				if (c is MdiClient) 
				{

					((MdiClient)c).BackColor = Color.FromArgb(1,31,91);
					((MdiClient)c).Text = "Penn Security Lab";

					Image i = Image.FromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("WSEmailClientv2.pennlogo.gif"));
					Label l = new Label();
					l.Width = i.Width;
					l.Height = i.Height;
					l.Image = i;
					l.Top = 0;
					l.Left = 0;
					((MdiClient)c).Controls.Add(l);

				}
			}
			NotifyIcon n = new NotifyIcon();
			n.Icon = new Icon("Mailbox.ico");
			n.Text = "Nayan sucks";
			n.Visible = true;
*/

/*
			 NotifyIconEx n = new NotifyIconEx();
			//	n.Text = TipText;
			n.Icon = new Icon("Mailbox.ico");
			n.Visible = true;
			//n.ContextMenu = NotifyContextMenu;	

			//n.Click += new EventHandler(OnClickIcon);
			//n.DoubleClick += new EventHandler(OnDoubleClickIcon);
			//n.BalloonClick += new EventHandler(OnClickBalloon);
	//		n.ShowBalloon("YOSEF!!!","Smells like cheese... eww",NotifyIconEx.NotifyInfoFlags.Warning,0);
*/
			NotifyIconEx n = Global.AlertIcon;

			Global.Proxy.SecurityTokenRetriever = new WSEmailProxy.TokenGrabber(ShowAuthenticationScreen);
			frmStatus = new FrmStatus();
			frmStatus.MdiParent = this;
			PennLibraries.Utilities.MessageForm = frmStatus;
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
			Global.AlertIcon.Visible = false;
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FrmMain));
			this.listViewImage = new System.Windows.Forms.ImageList(this.components);
			this.toolBar1 = new System.Windows.Forms.ToolBar();
			this.toolBarButton8 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton9 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton5 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton4 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton6 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton7 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton10 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton11 = new System.Windows.Forms.ToolBarButton();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// listViewImage
			// 
			this.listViewImage.ImageSize = new System.Drawing.Size(32, 32);
			this.listViewImage.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("listViewImage.ImageStream")));
			this.listViewImage.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// toolBar1
			// 
			this.toolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBar1.AutoSize = false;
			this.toolBar1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						this.toolBarButton8,
																						this.toolBarButton9,
																						this.toolBarButton1,
																						this.toolBarButton5,
																						this.toolBarButton4,
																						this.toolBarButton6,
																						this.toolBarButton2,
																						this.toolBarButton7,
																						this.toolBarButton3,
																						this.toolBarButton10,
																						this.toolBarButton11});
			this.toolBar1.ButtonSize = new System.Drawing.Size(90, 80);
			this.toolBar1.Dock = System.Windows.Forms.DockStyle.Left;
			this.toolBar1.DropDownArrows = true;
			this.toolBar1.ImageList = this.listViewImage;
			this.toolBar1.Location = new System.Drawing.Point(0, 0);
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.ShowToolTips = true;
			this.toolBar1.Size = new System.Drawing.Size(93, 553);
			this.toolBar1.TabIndex = 4;
			this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
			// 
			// toolBarButton8
			// 
			this.toolBarButton8.ImageIndex = 5;
			this.toolBarButton8.Text = "New Message";
			// 
			// toolBarButton9
			// 
			this.toolBarButton9.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButton1
			// 
			this.toolBarButton1.ImageIndex = 3;
			this.toolBarButton1.Text = "Inbox";
			// 
			// toolBarButton5
			// 
			this.toolBarButton5.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButton4
			// 
			this.toolBarButton4.ImageIndex = 4;
			this.toolBarButton4.Text = "Log";
			// 
			// toolBarButton6
			// 
			this.toolBarButton6.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButton2
			// 
			this.toolBarButton2.ImageIndex = 2;
			this.toolBarButton2.Text = "Sent Messages";
			// 
			// toolBarButton7
			// 
			this.toolBarButton7.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButton3
			// 
			this.toolBarButton3.ImageIndex = 0;
			this.toolBarButton3.Text = "Contact List";
			// 
			// toolBarButton10
			// 
			this.toolBarButton10.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButton11
			// 
			this.toolBarButton11.ImageIndex = 1;
			this.toolBarButton11.Text = "Instant Messages";
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem5,
																					  this.menuItem3});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MdiList = true;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem6,
																					  this.menuItem2});
			this.menuItem1.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
			this.menuItem1.Text = "File";
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 0;
			this.menuItem6.MergeOrder = 1;
			this.menuItem6.Text = "-";
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 1;
			this.menuItem5.MdiList = true;
			this.menuItem5.MergeOrder = 1;
			this.menuItem5.Text = "Windows";
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 2;
			this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem4});
			this.menuItem3.MergeOrder = 2;
			this.menuItem3.Text = "Help";
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 0;
			this.menuItem4.Text = "About";
			this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.MergeOrder = 2;
			this.menuItem2.Text = "Exit WSEmail Client";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// FrmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.DarkSeaGreen;
			this.ClientSize = new System.Drawing.Size(792, 553);
			this.Controls.Add(this.toolBar1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.IsMdiContainer = true;
			this.Menu = this.mainMenu1;
			this.Name = "FrmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "WSEmail Client v2.0";
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.ResumeLayout(false);

		}
		#endregion




		public SecurityToken ShowAuthenticationScreen() 
		{
			PennLibraries.AuthenticationForm f = new PennLibraries.AuthenticationForm();
			f.ShowDialog();
			SecurityToken s = f.SecurityToken;
			if (s != null) 
			{
				if (s is UsernameToken) 
				{
					Global.UserID = ((UsernameToken)s).Username;
				}
				else if (s is X509SecurityToken) 
				{
					Global.UserID = PennLibraries.Utilities.GetCertEmail(((X509SecurityToken)s).Certificate);
				}
				else if (s is FederatedToken) 
				{
					Global.UserID = ((FederatedToken)s).IdentityToken.UserID;
				}
			} else 
				MessageBox.Show("No security token was given. Things probably won't work correctly.");
			f.Dispose();
			return s;
		}

		//http://www.iconarchive.com/icon/system/scetchcons_by_iconfactory/index.html
		private void listView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
/*			if (listView.SelectedItems.Count <= 0) 
				return;
			switch (listView.SelectedItems[0].Text) 
			{
			}
*/			
		}

		private void panelDisplay_Resize(object sender, System.EventArgs e)
		{

		}

		private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			switch (e.Button.Text) 
			{
				case "Inbox":
					frmMessages = (FrmMessages)LoadOrShowForm(frmMessages, typeof(FrmMessages));
					break;
				case "New Message":
					FrmSend f = new FrmSend();
					f.MdiParent = this;
					f.Show();
					f.WindowState = FormWindowState.Maximized;
					break;
				case "Log":
					frmStatus.Show();
					frmStatus.Focus();
					//frmStatus.BringToFront();
					break;
				case "Instant Messages":
					frmIMMain = (FrmIMMain)LoadOrShowForm(frmIMMain, typeof(FrmIMMain));
					break;
				case "Contact List":
					//frmAddrBook
					frmAddrBook = (FrmAddressBook)LoadOrShowForm(frmAddrBook, typeof(FrmAddressBook));
					break;

					
			}
		}

		private Form LoadOrShowForm(Form f, Type t) 
		{
			if (f == null || f.IsDisposed) 
			{
				f = (Form)t.Assembly.CreateInstance(t.FullName);
				f.MdiParent = this;
				f.Show();
				f.WindowState = FormWindowState.Maximized;
			}
			f.Focus();
			f.Refresh();
			f.Activate();
			return f;
		}

		private void frmMain_Load(object sender, System.EventArgs e)
		{
		
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			this.Dispose();
		}

		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			FrmAbout f = new FrmAbout();
			f.ShowDialog();
			f.Dispose();
		}

		private void menuItem7_Click(object sender, System.EventArgs e)
		{
			 

			

			
			

			

		}

	}
}
