/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Reflection;
using SysManCommon;
using WSEmailProxy;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.Web.Services2.Security.Tokens;

namespace SysCon
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class FrmConsole : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox grpboxVars;
		private System.Windows.Forms.GroupBox grpboxPlugins;
		private System.Windows.Forms.GroupBox grpboxCoreServices;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem mnuRefreshCore;
		private System.Windows.Forms.MenuItem mnuRefreshVars;
		MailServerProxy Proxy = null;
		Hashtable core = null, vars = null, plugins = null;
		private System.Windows.Forms.MenuItem mnuRefreshAll;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem mnuFileRestart;
		private System.Windows.Forms.MenuItem mnuRefreshPlugins;
		private bool goUp = true;
		private Control _c = null;
		string CONFFILE = Application.UserAppDataPath + @"\..\config.xml";
		private System.Windows.Forms.MenuItem mnuConfiguration;
		private System.Windows.Forms.MenuItem mnuExit;
		private System.Windows.Forms.MenuItem mnuPluginLoad;
		private Config _conf = null;

		public FrmConsole()
		{
			InitializeComponent();
			_conf = Config.GetConfig(CONFFILE);
			if (_conf == null) 
			{
				this.mnuConfiguration_Click(null,null);
			} 
			else 
			{
				LoadProxy();
			}
			//
			// Required for Windows Form Designer support
			//
		}

		private void LoadProxy() 
		{
			MailServerProxy p = new MailServerProxy();
			string pass = PennLibraries.InputBox.ShowInputBox("What is the password for " + _conf.Username + "@" + _conf.URL +"?",true);
			p.SecurityToken = new UsernameToken(_conf.Username,pass,PasswordOption.SendNone);
			p.Url = _conf.URL;
			Proxy = p;
			refreshAll();
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
			this.label3 = new System.Windows.Forms.Label();
			this.grpboxPlugins = new System.Windows.Forms.GroupBox();
			this.grpboxVars = new System.Windows.Forms.GroupBox();
			this.grpboxCoreServices = new System.Windows.Forms.GroupBox();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mnuConfiguration = new System.Windows.Forms.MenuItem();
			this.mnuFileRestart = new System.Windows.Forms.MenuItem();
			this.mnuExit = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.mnuRefreshVars = new System.Windows.Forms.MenuItem();
			this.mnuRefreshCore = new System.Windows.Forms.MenuItem();
			this.mnuRefreshPlugins = new System.Windows.Forms.MenuItem();
			this.mnuRefreshAll = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.mnuPluginLoad = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(328, 416);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(120, 16);
			this.label3.TabIndex = 5;
			this.label3.Text = "Services";
			this.label3.Visible = false;
			// 
			// grpboxPlugins
			// 
			this.grpboxPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grpboxPlugins.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.grpboxPlugins.Location = new System.Drawing.Point(8, 304);
			this.grpboxPlugins.Name = "grpboxPlugins";
			this.grpboxPlugins.Size = new System.Drawing.Size(432, 104);
			this.grpboxPlugins.TabIndex = 15;
			this.grpboxPlugins.TabStop = false;
			this.grpboxPlugins.Text = "Plugins";
			// 
			// grpboxVars
			// 
			this.grpboxVars.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grpboxVars.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.grpboxVars.Location = new System.Drawing.Point(8, 8);
			this.grpboxVars.Name = "grpboxVars";
			this.grpboxVars.Size = new System.Drawing.Size(432, 136);
			this.grpboxVars.TabIndex = 18;
			this.grpboxVars.TabStop = false;
			this.grpboxVars.Text = "Server Variables";
			// 
			// grpboxCoreServices
			// 
			this.grpboxCoreServices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grpboxCoreServices.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.grpboxCoreServices.Location = new System.Drawing.Point(8, 152);
			this.grpboxCoreServices.Name = "grpboxCoreServices";
			this.grpboxCoreServices.Size = new System.Drawing.Size(432, 136);
			this.grpboxCoreServices.TabIndex = 19;
			this.grpboxCoreServices.TabStop = false;
			this.grpboxCoreServices.Text = "Status of Core Services ";
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem3,
																					  this.menuItem4});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuConfiguration,
																					  this.mnuFileRestart,
																					  this.mnuExit});
			this.menuItem1.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
			this.menuItem1.Text = "File";
			// 
			// mnuConfiguration
			// 
			this.mnuConfiguration.Index = 0;
			this.mnuConfiguration.Text = "Configuration";
			this.mnuConfiguration.Click += new System.EventHandler(this.mnuConfiguration_Click);
			// 
			// mnuFileRestart
			// 
			this.mnuFileRestart.Index = 1;
			this.mnuFileRestart.Text = "Restart Server";
			this.mnuFileRestart.Click += new System.EventHandler(this.mnuFileRestart_Click);
			// 
			// mnuExit
			// 
			this.mnuExit.Index = 2;
			this.mnuExit.Text = "Exit";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuRefreshVars,
																					  this.mnuRefreshCore,
																					  this.mnuRefreshPlugins,
																					  this.mnuRefreshAll});
			this.menuItem3.Text = "Refresh";
			// 
			// mnuRefreshVars
			// 
			this.mnuRefreshVars.Index = 0;
			this.mnuRefreshVars.Text = "System Variables";
			this.mnuRefreshVars.Click += new System.EventHandler(this.mnuRefreshVars_Click);
			// 
			// mnuRefreshCore
			// 
			this.mnuRefreshCore.Index = 1;
			this.mnuRefreshCore.Text = "Core Service";
			this.mnuRefreshCore.Click += new System.EventHandler(this.mnuRefreshCore_Click);
			// 
			// mnuRefreshPlugins
			// 
			this.mnuRefreshPlugins.Index = 2;
			this.mnuRefreshPlugins.Text = "Plugins";
			this.mnuRefreshPlugins.Click += new System.EventHandler(this.mnuRefreshPlugins_Click);
			// 
			// mnuRefreshAll
			// 
			this.mnuRefreshAll.Index = 3;
			this.mnuRefreshAll.Text = "All";
			this.mnuRefreshAll.Click += new System.EventHandler(this.mnuRefreshAll_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 2;
			this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuPluginLoad});
			this.menuItem4.Text = "Plugins";
			// 
			// mnuPluginLoad
			// 
			this.mnuPluginLoad.Index = 0;
			this.mnuPluginLoad.Text = "Load";
			this.mnuPluginLoad.Click += new System.EventHandler(this.mnuPluginLoad_Click);
			// 
			// FrmConsole
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(448, 425);
			this.Controls.Add(this.grpboxVars);
			this.Controls.Add(this.grpboxPlugins);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.grpboxCoreServices);
			this.Menu = this.mainMenu1;
			this.Name = "FrmConsole";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "System Monitor";
			this.Activated += new System.EventHandler(this.FrmConsole_Activated);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FrmConsole_MouseMove);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		private void textBox1_TextChanged(object sender, System.EventArgs e)
		{
		
		}

		private void refreshAll() 
		{
			
			populateDataLists();
			grpboxCoreServices.Top = this.grpboxVars.Top + this.grpboxVars.Height;
			populateCoreServiceLists();
			this.grpboxPlugins.Top = this.grpboxCoreServices.Top + this.grpboxCoreServices.Height;
			populatePluginLists();
			this.Height = this.grpboxCoreServices.Top + this.grpboxCoreServices.Height;

			
		}

		private void populateCoreServiceLists() 
		{
			Hashtable h = new Hashtable();
			ExtensionArgument ext = new ExtensionArgument(RequestType.CoreStatus.ToString());

			FieldInfo[] fs = (typeof(CorePlugins)).GetFields(BindingFlags.Public | BindingFlags.Static);
			int offset = 20;
			int height = 50;
			foreach (FieldInfo f in fs) 
			{
				if (core == null) 
				{
					Label l = newLabel(5,120,height,offset,f.Name + ":");
					TextBox t = newTextBox(125,grpboxCoreServices.Width - 125 - 10,height,offset,"");
					t.Multiline = true;
					System.Diagnostics.Debug.WriteLine("Created " + f.Name);
				
					offset += height;
					grpboxCoreServices.Controls.AddRange(new Control[] {l,t});
					h[f.Name] = t;
				} else 
					h[f.Name] = core[f.Name];
				
				ext.AddArgument(f.Name,"");
			}
			
			if (core == null)
				grpboxCoreServices.Height = offset + 10;

			ExtensionArgument res = new ExtensionArgument(Proxy.ExecuteExtensionHandler("SysMan",ext.AsXmlElement()));
			foreach (string s in res.Arguments) 
			{
				if (res[s] != null) 
				{
					if (h[s] != null) 
						((TextBox)h[s]).Text = res[s];
				}
			}
			core = h;

		}

		private Label newLabel(int left, int width, int height, int top, string text) 
		{
			Label l = new Label();
			l.Left = left;
			l.Width = width;
			l.Height = height;
			l.Top = top;
			l.Font = label3.Font;
			l.Text = text;
			l.TextAlign = ContentAlignment.MiddleLeft;
			return l;
		}

		private TextBox newTextBox(int left, int width, int height, int top, string text) 
		{
			TextBox l = new TextBox();
			if (_c == null)
				_c = l;
			l.Left = left;
			l.Width = width;
			l.Height = height;
			l.Top = top;
			l.Font = label3.Font;
			l.Text = text;
			l.ReadOnly = true;
			l.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
			return l;
		}


		private void populateDataLists() 
		{
			Hashtable h = new Hashtable();
			ExtensionArgument ext = new ExtensionArgument(RequestType.DataRequest.ToString());

			FieldInfo[] fs = (typeof(DataRequested)).GetFields(BindingFlags.Public | BindingFlags.Static);
			int offset = 20;
			int height = 20;
			foreach (FieldInfo f in fs) 
			{

				if (!f.Name.StartsWith("Plugin")) 
				{
					if (vars == null) 
					{
						Label l = newLabel(5,120,height,offset,f.Name + ":");
						TextBox t = newTextBox(125,grpboxVars.Width - 125 - 10,height,offset,"");
						offset += height;
						grpboxVars.Controls.AddRange(new Control[] {l,t});
						h[f.Name] = t;
					} else 
						h[f.Name] = vars[f.Name];
					
					ext.AddArgument(f.Name,"");
				}
			}

			if (vars == null)
				grpboxVars.Height = offset + 10;

			ExtensionArgument res = new ExtensionArgument(Proxy.ExecuteExtensionHandler("SysMan",ext.AsXmlElement()));
			foreach (string s in res.Arguments)
				((TextBox)h[s]).Text = res[s];

			vars = h;
		}

		private void populatePluginLists() 
		{
			Hashtable h = new Hashtable();
			FieldInfo[] fs = (typeof(PluginTypes)).GetFields(BindingFlags.Public | BindingFlags.Static);
			int offset = 20;
			int height = 80;
			int width = (grpboxPlugins.Width - 20) /2;
			int count = 0;

			//MessageBox.Show(grpboxPlugins.Width.ToString());
			foreach (FieldInfo f in fs) 
			{
				if (!f.Name.StartsWith("All")) 
				{
					if (plugins == null) 
					{
						count++;
						Label l = null;
//						if (count % 2 == 1)
							l =	newLabel(5,this.grpboxPlugins.Width-10,20,offset,f.Name);
							offset += height + l.Height;
//						else 
//						{
//							l =	newLabel(width+10,width,20,offset,f.Name);
//							offset += height + l.Height;
//						}
						

						ListBox b = new ListBox();
						b.KeyDown += new KeyEventHandler(pluginKeyPress);
						b.DoubleClick += new EventHandler(pluginDoubleClick);
						b.Font = l.Font;
						b.Name = f.Name;
						b.Top = l.Top + l.Height + 2;
						b.Height = height;
						b.Width = l.Width;
						b.Left = l.Left;

						grpboxPlugins.Controls.AddRange(new Control[] {l,b});
						h[f.Name] = b;
					} else
						h[f.Name] = plugins[f.Name];

					ExtensionArgument ext = new ExtensionArgument(RequestType.DataRequest.ToString());
					ext = new ExtensionArgument(RequestType.DataRequest.ToString());
					ext[DataRequested.PluginList.ToString()] = f.Name;
					ExtensionArgument res = new ExtensionArgument(Proxy.ExecuteExtensionHandler("SysMan",ext.AsXmlElement()));
					((ListBox)h[f.Name]).Items.Clear();
					((ListBox)h[f.Name]).Items.AddRange(res[DataRequested.PluginList.ToString()].Split(','));


				}

			}
			if (plugins == null) 
				grpboxPlugins.Height = offset + 20;// + height + 20;

			plugins = h;


		}

		private void pluginDoubleClick(object sender,EventArgs e) 
		{
			if (((ListBox)sender).SelectedItem != null) 
			{
				MessageBox.Show("Status of plugin " + (string)((ListBox)sender).SelectedItem + " from the " + ((ListBox)sender).Name + " queue");
				ExtensionArgument ext = new ExtensionArgument(RequestType.PluginStatus.ToString());
				ext["Queue"] = ((ListBox)sender).Name;
				ext["Plugin"] = (string)((ListBox)sender).SelectedItem;
				ExtensionArgument res = new ExtensionArgument(Proxy.ExecuteExtensionHandler("SysMan",ext.AsXmlElement()));
				foreach (string s in res.Arguments)
					MessageBox.Show(res[s]);
			}

		}

		private void pluginKeyPress(object sender,KeyEventArgs e) 
		{
			
			if (e.KeyCode == Keys.Delete && ((ListBox)sender).SelectedItem != null) {
				MessageBox.Show("Delete plugin " + (string)((ListBox)sender).SelectedItem + " from the " + ((ListBox)sender).Name + " queue");
				ExtensionArgument ext = new ExtensionArgument(RequestType.UnloadPlugin.ToString());
				ext["Queue"] = ((ListBox)sender).Name;
				ext["Plugin"] = (string)((ListBox)sender).SelectedItem;
				ExtensionArgument res = new ExtensionArgument(Proxy.ExecuteExtensionHandler("SysMan",ext.AsXmlElement()));
				foreach (string s in res.Arguments)
					MessageBox.Show(res[s]);

			}
		}
		private void mnuRefreshCore_Click(object sender, System.EventArgs e)
		{
			populateCoreServiceLists();
		}

		private void mnuRefreshVars_Click(object sender, System.EventArgs e)
		{
			populateDataLists();
		}

		private void mnuRefreshPlugins_Click(object sender, System.EventArgs e)
		{
			populatePluginLists();
		}

		private void mnuPluginLoad_Click(object sender, System.EventArgs e)
		{
			string s = PennLibraries.InputBox.ShowInputBox("Type in the name of the plugin to load (or nothing to abort)");
			if (s.Length > 0) 
			{
				ExtensionArgument ext = new ExtensionArgument(RequestType.LoadPlugin.ToString());
				ext[s] = "";
				ExtensionArgument res = new ExtensionArgument(Proxy.ExecuteExtensionHandler("SysMan",ext.AsXmlElement()));
				foreach (string s2 in res.Arguments)
					MessageBox.Show(res[s2]);
			}

		}

		private void mnuFileRestart_Click(object sender, System.EventArgs e)
		{
			DialogResult r = MessageBox.Show("Are you SURE you want to restart the server?","Question...",
				MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
			if (r == DialogResult.Yes) 
			{
				ExtensionArgument ext = new ExtensionArgument(RequestType.ServerRestart.ToString());
				Proxy.ExecuteExtensionHandler("SysMan",ext.AsXmlElement());
				MessageBox.Show("Server has been sent the restart command.");
			}

		}

		private void mnuRefreshAll_Click(object sender, System.EventArgs e)
		{
			refreshAll();
		}

		private void FrmConsole_Activated(object sender, System.EventArgs e)
		{
			if (goUp) 
			{
				this.AutoScrollPosition = new Point(0,0);
				goUp = !goUp;
				_c.Focus();
			}
		}

		private void FrmConsole_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
		}

		private void mnuExit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void mnuConfiguration_Click(object sender, System.EventArgs e)
		{
			FrmConfig f = null;
			if (_conf != null)
				f = new FrmConfig(_conf);
			else
				f = new FrmConfig();
			f.ShowDialog();
			_conf = f.Configuration;
			f.Dispose();
			Config.SetConfig(_conf,CONFFILE);

			this.LoadProxy();
		}
	}
}
