/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Xml;
using WSEmailProxy;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using XmlAddressBook;
using System.Configuration;

namespace WSEmailClientv2
{
	/// <summary>
	/// Summary description for FrmAddressBook.   
	/// </summary>
	public class FrmAddressBook : System.Windows.Forms.Form
	{
		private enum UpdateType {Register, Unregister, Synchronize};

		private System.Windows.Forms.ListView listAddresses;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem3;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label lblHelp;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.MenuItem mnuNew;
		/// <summary>
		/// Address book instance
		/// </summary>
 		private AddressBook book = null;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		/// <summary>
		/// Used for inquiring forms to get email addresses selected in this form
		/// </summary>
		public string SelectedEmails = "";

		public FrmAddressBook()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			book = AddressBook.GetInstance();
			book.Updated += new AddressBookChange(UpdateList);
			UpdateList();
		}

		/// <summary>
		/// Gets the current list of addresses
		/// </summary>
		private void UpdateList() 
		{
			listAddresses.Items.Clear();
			foreach (AddressBookEntry b in book.GetAllEntries()) 
				listAddresses.Items.Add(new AddressItem(b));
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FrmAddressBook));
			this.listAddresses = new System.Windows.Forms.ListView();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mnuNew = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.lblHelp = new System.Windows.Forms.Label();
			this.btnSelect = new System.Windows.Forms.Button();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// listAddresses
			// 
			this.listAddresses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.listAddresses.Location = new System.Drawing.Point(0, 40);
			this.listAddresses.Name = "listAddresses";
			this.listAddresses.Size = new System.Drawing.Size(600, 393);
			this.listAddresses.TabIndex = 0;
			this.listAddresses.View = System.Windows.Forms.View.List;
			this.listAddresses.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listAddresses_KeyDown);
			this.listAddresses.DoubleClick += new System.EventHandler(this.listAddresses_DoubleClick);
			this.listAddresses.SelectedIndexChanged += new System.EventHandler(this.listAddresses_SelectedIndexChanged);
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuNew,
																					  this.menuItem2,
																					  this.menuItem5,
																					  this.menuItem4,
																					  this.menuItem3});
			this.menuItem1.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
			this.menuItem1.Text = "File";
			// 
			// mnuNew
			// 
			this.mnuNew.Index = 0;
			this.mnuNew.Text = "Add New";
			this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 4;
			this.menuItem3.Text = "Close Address Book";
			this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
			// 
			// lblHelp
			// 
			this.lblHelp.Location = new System.Drawing.Point(0, 0);
			this.lblHelp.Name = "lblHelp";
			this.lblHelp.Size = new System.Drawing.Size(448, 32);
			this.lblHelp.TabIndex = 1;
			this.lblHelp.Text = "Double click on an address to view it in more detail or to make changes to it. Se" +
				"lect one or more items and press \'Delete\' to remove the item.";
			// 
			// btnSelect
			// 
			this.btnSelect.Enabled = false;
			this.btnSelect.Location = new System.Drawing.Point(488, 8);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.Size = new System.Drawing.Size(104, 24);
			this.btnSelect.TabIndex = 2;
			this.btnSelect.Text = "Select Addresses";
			this.btnSelect.Visible = false;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "Register";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 3;
			this.menuItem4.Text = "Synchronize";
			this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 2;
			this.menuItem5.Text = "Unregister";
			this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
			// 
			// FrmAddressBook
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(600, 393);
			this.Controls.Add(this.btnSelect);
			this.Controls.Add(this.lblHelp);
			this.Controls.Add(this.listAddresses);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.Name = "FrmAddressBook";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Address Book";
			this.Load += new System.EventHandler(this.FrmAddressBook_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// In a modal (address selection) context, this returns an email address. In a normal context,
		/// it opens the address for editing.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listAddresses_DoubleClick(object sender, System.EventArgs e)
		{
			
			if (listAddresses.SelectedItems.Count > 0) 
			{
				if (this.Modal) 
				{
					foreach (ListViewItem i in listAddresses.SelectedItems)
						this.SelectedEmails = ((AddressItem)i).Address.Email;
					
					this.Hide();
				} 
				else 
				{
					FrmAddressView f = new FrmAddressView(((AddressItem)listAddresses.SelectedItems[0]));
					f.ShowDialog();
					if (f.IsOk) 
					{
						// it was OK.. so update
						AddressItem a = ((AddressItem)listAddresses.SelectedItems[0]);
						a.Address.Email = f.Email;
						a.Address.FirstName = f.First;
						a.Address.LastName = f.Last;
						a.Address.Notes = f.Notes;
					}
					f.Dispose();
				}
			}
		}

		/// <summary>
		/// Allows multiple recipients to be selected and returned.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSelect_Click(object sender, System.EventArgs e)
		{
			foreach (ListViewItem i in listAddresses.SelectedItems)
				this.SelectedEmails += ((AddressItem)i).Address.Email +", ";
					
			this.SelectedEmails = this.SelectedEmails.Substring(0,this.SelectedEmails.Length -2);
			this.Hide();
		}

		/// <summary>
		/// Turns the select button on/off if there are selections or not.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listAddresses_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (listAddresses.SelectedIndices.Count > 0)
				this.btnSelect.Enabled = true;
			else
				this.btnSelect.Enabled = false;
		}

		/// <summary>
		/// Sets some helpful info on the screen for users if modal.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FrmAddressBook_Load(object sender, System.EventArgs e)
		{
			if (Modal) 
			{
				this.Text = "Please select recipients";
				this.lblHelp.Text = "Double-click on an entry to select 1 recipient, or select a few recipients and press the the 'Select Addresses' button";
				this.btnSelect.Visible = true;
			}
		}

		/// <summary>
		/// Allows the addition of new entries.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuNew_Click(object sender, System.EventArgs e)
		{
			FrmAddressView f = new FrmAddressView();
			f.ShowDialog();
			if (f.IsOk) 
			{
				AddressBookEntry a = new AddressBookEntry();
				a.AddDate = DateTime.Now;
				a.Email = f.Email;
				a.FirstName = f.First;
				a.LastName = f.Last;
				a.Notes = f.Notes;
				if (book.AddEntry(a)) 
					MessageBox.Show("Added " + a.Email + " to your address book.");
				else
					MessageBox.Show("The email address " + a.Email + " is already in your address book.");
			}
		}

		/// <summary>
		/// Handles the delete keypress to remove entries.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listAddresses_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete) 
			{
				DialogResult r= MessageBox.Show("Really delete these entries?","Question",MessageBoxButtons.YesNo,
					MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
				if (r == DialogResult.Yes) 
				{
					foreach (ListViewItem i in listAddresses.SelectedItems)
						book.RemoveEntry(((AddressItem)i).Address);
				}
			}
		}

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			this.Dispose();
		}

		private MailServerProxy MakeProxy() 
		{
			string str = ConfigurationSettings.AppSettings["AddressBookServer"];
			if (str == null || str.Length == 0) 
			{
				return null;
			}

			MailServerProxy s = new MailServerProxy(str);
			s.SecurityToken = Global.FederatedTokenManager.Token;
			return s;
		}

		private void TalkToServer(UpdateType t) 
		{
			MailServerProxy s = MakeProxy();
			if (s == null) 
			{
				MessageBox.Show("No address book server is configured. Nayan probably messed up.","Oops! Nayan sucks");
				return;
			}

			switch (t) 
			{
				case UpdateType.Register:
				{
					ExtensionArgument ex = new ExtensionArgument("RegisterAddress");
					ExtensionArgument res = new ExtensionArgument(s.ExecuteExtensionHandler("AddressBook",ex.AsXmlElement()));
					MessageBox.Show(res["Result"],"Results...");
				}
					break;

				case UpdateType.Synchronize: 
				{

					ExtensionArgument ex = new ExtensionArgument("GetAddresses");
					XmlElement res = s.ExecuteExtensionHandler("AddressBook",ex.AsXmlElement());
					AddressBook.GetInstance().AddEntry(res);
				}
					break;
				case UpdateType.Unregister:
				{
					ExtensionArgument ex = new ExtensionArgument("UnregisterAddress");
					ExtensionArgument h = new ExtensionArgument(s.ExecuteExtensionHandler("AddressBook",ex.AsXmlElement()));
					MessageBox.Show(h["Result"],"Results...");
				}
					break;

			}			
		}


		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			TalkToServer(UpdateType.Register);
/*
//			MailServerProxy s = MakeProxy();
			if (s != null) 
			{
				ExtensionArgument ex = new ExtensionArgument("RegisterAddress");
				ExtensionArgument h = new ExtensionArgument(s.ExecuteExtensionHandler("AddressBook",ex.AsXmlElement()));
				MessageBox.Show(h["Result"],"Results...");
			}
*/
		}

		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			TalkToServer(UpdateType.Synchronize);
/*			MailServerProxy s = MakeProxy();
			if (s != null) 
			{
				ExtensionArgument ex = new ExtensionArgument("GetAddresses");
				XmlElement res = s.ExecuteExtensionHandler("AddressBook",ex.AsXmlElement());
				AddressBook.GetInstance().AddEntry(res);
			}
*/
		}

		private void menuItem5_Click(object sender, System.EventArgs e)
		{
			TalkToServer(UpdateType.Unregister);

			/*			MailServerProxy s = MakeProxy();
			if (s != null) 
			{
				ExtensionArgument ex = new ExtensionArgument("UnregisterAddress");
				ExtensionArgument h = new ExtensionArgument(s.ExecuteExtensionHandler("AddressBook",ex.AsXmlElement()));
				MessageBox.Show(h["Result"],"Results...");
			}
*/
		}
	}
}
