using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Merchant
{
	/// <summary>
	/// Summary description for P3PCreator.
	/// </summary>
	public class P3PCreator : System.Windows.Forms.Form
	{
		/// <summary>
		/// Details box for Disputes editing
		/// </summary>
		private Merchant.DisputesDetails m_details;

		/// <summary>
		/// The Policy object that we are referring to
		/// </summary>
		private P3P.POLICY m_policy;
		public P3P.POLICY Policy
		{
			get { return m_policy; }
			set
			{
				m_policy = value;

				// reload the windows to match the policy
				this.LoadPolicy();
			}
		}


		#region Property Pages Components
		private System.Windows.Forms.TabPage AccessTab;
		private System.Windows.Forms.TabPage CategoriesTab;
		private System.Windows.Forms.TabPage DisputesTab;
		private System.Windows.Forms.TabPage PurposeTab;
		private System.Windows.Forms.TabPage RecipientTab;
		private System.Windows.Forms.TabPage RetentionTab;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox cbAll;
		private System.Windows.Forms.CheckBox cbContact_and_Other;
		private System.Windows.Forms.CheckBox cbIdent_Contact;
		private System.Windows.Forms.CheckBox cbNone;
		private System.Windows.Forms.CheckBox cbNonident;
		private System.Windows.Forms.CheckBox cbOther_Ident;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox cbComputer;
		private System.Windows.Forms.CheckBox cbContent;
		private System.Windows.Forms.CheckBox cbDemographic;
		private System.Windows.Forms.CheckBox cbFinancial;
		private System.Windows.Forms.CheckBox cbGovernment;
		private System.Windows.Forms.CheckBox cbHealth;
		private System.Windows.Forms.CheckBox cbInteractive;
		private System.Windows.Forms.CheckBox cbLocation;
		private System.Windows.Forms.CheckBox cbNavigation;
		private System.Windows.Forms.CheckBox cbOnline;
		private System.Windows.Forms.CheckBox cbOther_Category;
		private System.Windows.Forms.CheckBox cbState;
		private System.Windows.Forms.CheckBox cbPhysical;
		private System.Windows.Forms.CheckBox cbPolitical;
		private System.Windows.Forms.CheckBox cbPreference;
		private System.Windows.Forms.CheckBox cbPurchase;
		private System.Windows.Forms.TextBox tbOther_Category;
		private System.Windows.Forms.CheckBox cbUniqueID;
		private System.Windows.Forms.ListBox lbDisputes;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button bAdd;
		private System.Windows.Forms.Button bEdit;
		private System.Windows.Forms.Button bRemove;
		private System.Windows.Forms.TabControl P3PTabs;
		private System.Windows.Forms.CheckBox cbAdmin;
		private System.Windows.Forms.CheckBox cbContact;
		private System.Windows.Forms.CheckBox cbCurrent;
		private System.Windows.Forms.CheckBox cbDevelop;
		private System.Windows.Forms.CheckBox cbHistorical;
		private System.Windows.Forms.CheckBox cbIndividual_Analysis;
		private System.Windows.Forms.CheckBox cbIndividual_Decision;
		private System.Windows.Forms.CheckBox cbOther_Purpose;
		private System.Windows.Forms.CheckBox cbPseudo_Analysis;
		private System.Windows.Forms.CheckBox cbPseudo_Decision;
		private System.Windows.Forms.CheckBox cbTailoring;
		private System.Windows.Forms.CheckBox cbTelemarketing;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tbOther_Purpose;
		private System.Windows.Forms.ComboBox cmbAdmin;
		private System.Windows.Forms.ComboBox cmbContact;
		private System.Windows.Forms.ComboBox cmbDevelop;
		private System.Windows.Forms.ComboBox cmbHistorical;
		private System.Windows.Forms.ComboBox cmbIndividual_Analysis;
		private System.Windows.Forms.ComboBox cmbTelemarketing;
		private System.Windows.Forms.ComboBox cmbTailoring;
		private System.Windows.Forms.ComboBox cmbPseudo_Decision;
		private System.Windows.Forms.ComboBox cmbOther_Purpose;
		private System.Windows.Forms.ComboBox cmbIndividual_Decision;
		private System.Windows.Forms.ComboBox cmbUnrelated;
		private System.Windows.Forms.ComboBox cmbSame;
		private System.Windows.Forms.ComboBox cmbPublic;
		private System.Windows.Forms.ComboBox cmbOther_Recipient;
		private System.Windows.Forms.ComboBox cmbDelivery;
		private System.Windows.Forms.CheckBox cbUnrelated;
		private System.Windows.Forms.CheckBox cbSame;
		private System.Windows.Forms.CheckBox cbPublic;
		private System.Windows.Forms.CheckBox cbOurs;
		private System.Windows.Forms.CheckBox cbOther_Recipient;
		private System.Windows.Forms.CheckBox cbDelivery;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox cbStated_Purpose;
		private System.Windows.Forms.CheckBox cbNo_Retention;
		private System.Windows.Forms.CheckBox cbLegal_Requirement;
		private System.Windows.Forms.CheckBox cbIndefinitely;
		private System.Windows.Forms.CheckBox cbBusiness_Practices;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button bReset;
		private System.Windows.Forms.Button bCancel;
		private System.Windows.Forms.Button bSave;
		private System.Windows.Forms.TabPage MiscTab;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox tbConsequence;
		private System.Windows.Forms.CheckBox cbNonidentifiable;
		private System.Windows.Forms.ComboBox cmbPseudo_Analysis;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		public P3PCreator()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// create the details box that we will use for editing Disputes
			// items
			m_details = new DisputesDetails();
			this.m_policy = new P3P.POLICY();
			this.m_policy.m_statements[0] = new P3P.STATEMENT();
			this.LoadPolicy();

			this.lbDisputes.DrawMode = DrawMode.OwnerDrawFixed;
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
			this.P3PTabs = new System.Windows.Forms.TabControl();
			this.AccessTab = new System.Windows.Forms.TabPage();
			this.cbOther_Ident = new System.Windows.Forms.CheckBox();
			this.cbNonident = new System.Windows.Forms.CheckBox();
			this.cbNone = new System.Windows.Forms.CheckBox();
			this.cbIdent_Contact = new System.Windows.Forms.CheckBox();
			this.cbContact_and_Other = new System.Windows.Forms.CheckBox();
			this.cbAll = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.CategoriesTab = new System.Windows.Forms.TabPage();
			this.cbUniqueID = new System.Windows.Forms.CheckBox();
			this.tbOther_Category = new System.Windows.Forms.TextBox();
			this.cbPurchase = new System.Windows.Forms.CheckBox();
			this.cbPreference = new System.Windows.Forms.CheckBox();
			this.cbPolitical = new System.Windows.Forms.CheckBox();
			this.cbPhysical = new System.Windows.Forms.CheckBox();
			this.cbState = new System.Windows.Forms.CheckBox();
			this.cbOther_Category = new System.Windows.Forms.CheckBox();
			this.cbOnline = new System.Windows.Forms.CheckBox();
			this.cbNavigation = new System.Windows.Forms.CheckBox();
			this.cbLocation = new System.Windows.Forms.CheckBox();
			this.cbInteractive = new System.Windows.Forms.CheckBox();
			this.cbHealth = new System.Windows.Forms.CheckBox();
			this.cbGovernment = new System.Windows.Forms.CheckBox();
			this.cbFinancial = new System.Windows.Forms.CheckBox();
			this.cbDemographic = new System.Windows.Forms.CheckBox();
			this.cbContent = new System.Windows.Forms.CheckBox();
			this.cbComputer = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.DisputesTab = new System.Windows.Forms.TabPage();
			this.bRemove = new System.Windows.Forms.Button();
			this.bEdit = new System.Windows.Forms.Button();
			this.bAdd = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.lbDisputes = new System.Windows.Forms.ListBox();
			this.PurposeTab = new System.Windows.Forms.TabPage();
			this.cmbTelemarketing = new System.Windows.Forms.ComboBox();
			this.cmbTailoring = new System.Windows.Forms.ComboBox();
			this.cmbPseudo_Decision = new System.Windows.Forms.ComboBox();
			this.cmbPseudo_Analysis = new System.Windows.Forms.ComboBox();
			this.cmbOther_Purpose = new System.Windows.Forms.ComboBox();
			this.cmbIndividual_Decision = new System.Windows.Forms.ComboBox();
			this.cmbIndividual_Analysis = new System.Windows.Forms.ComboBox();
			this.cmbHistorical = new System.Windows.Forms.ComboBox();
			this.cmbDevelop = new System.Windows.Forms.ComboBox();
			this.cmbContact = new System.Windows.Forms.ComboBox();
			this.cmbAdmin = new System.Windows.Forms.ComboBox();
			this.tbOther_Purpose = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.cbTelemarketing = new System.Windows.Forms.CheckBox();
			this.cbTailoring = new System.Windows.Forms.CheckBox();
			this.cbPseudo_Decision = new System.Windows.Forms.CheckBox();
			this.cbPseudo_Analysis = new System.Windows.Forms.CheckBox();
			this.cbOther_Purpose = new System.Windows.Forms.CheckBox();
			this.cbIndividual_Decision = new System.Windows.Forms.CheckBox();
			this.cbIndividual_Analysis = new System.Windows.Forms.CheckBox();
			this.cbHistorical = new System.Windows.Forms.CheckBox();
			this.cbDevelop = new System.Windows.Forms.CheckBox();
			this.cbCurrent = new System.Windows.Forms.CheckBox();
			this.cbContact = new System.Windows.Forms.CheckBox();
			this.cbAdmin = new System.Windows.Forms.CheckBox();
			this.RecipientTab = new System.Windows.Forms.TabPage();
			this.label5 = new System.Windows.Forms.Label();
			this.cmbUnrelated = new System.Windows.Forms.ComboBox();
			this.cmbSame = new System.Windows.Forms.ComboBox();
			this.cmbPublic = new System.Windows.Forms.ComboBox();
			this.cmbOther_Recipient = new System.Windows.Forms.ComboBox();
			this.cmbDelivery = new System.Windows.Forms.ComboBox();
			this.cbUnrelated = new System.Windows.Forms.CheckBox();
			this.cbSame = new System.Windows.Forms.CheckBox();
			this.cbPublic = new System.Windows.Forms.CheckBox();
			this.cbOurs = new System.Windows.Forms.CheckBox();
			this.cbOther_Recipient = new System.Windows.Forms.CheckBox();
			this.cbDelivery = new System.Windows.Forms.CheckBox();
			this.RetentionTab = new System.Windows.Forms.TabPage();
			this.label6 = new System.Windows.Forms.Label();
			this.cbStated_Purpose = new System.Windows.Forms.CheckBox();
			this.cbNo_Retention = new System.Windows.Forms.CheckBox();
			this.cbLegal_Requirement = new System.Windows.Forms.CheckBox();
			this.cbIndefinitely = new System.Windows.Forms.CheckBox();
			this.cbBusiness_Practices = new System.Windows.Forms.CheckBox();
			this.MiscTab = new System.Windows.Forms.TabPage();
			this.cbNonidentifiable = new System.Windows.Forms.CheckBox();
			this.tbConsequence = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.bReset = new System.Windows.Forms.Button();
			this.bCancel = new System.Windows.Forms.Button();
			this.bSave = new System.Windows.Forms.Button();
			this.P3PTabs.SuspendLayout();
			this.AccessTab.SuspendLayout();
			this.CategoriesTab.SuspendLayout();
			this.DisputesTab.SuspendLayout();
			this.PurposeTab.SuspendLayout();
			this.RecipientTab.SuspendLayout();
			this.RetentionTab.SuspendLayout();
			this.MiscTab.SuspendLayout();
			this.SuspendLayout();
			// 
			// P3PTabs
			// 
			this.P3PTabs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.P3PTabs.Controls.Add(this.AccessTab);
			this.P3PTabs.Controls.Add(this.CategoriesTab);
			this.P3PTabs.Controls.Add(this.DisputesTab);
			this.P3PTabs.Controls.Add(this.PurposeTab);
			this.P3PTabs.Controls.Add(this.RecipientTab);
			this.P3PTabs.Controls.Add(this.RetentionTab);
			this.P3PTabs.Controls.Add(this.MiscTab);
			this.P3PTabs.Location = new System.Drawing.Point(8, 8);
			this.P3PTabs.Name = "P3PTabs";
			this.P3PTabs.SelectedIndex = 0;
			this.P3PTabs.Size = new System.Drawing.Size(496, 408);
			this.P3PTabs.TabIndex = 0;
			// 
			// AccessTab
			// 
			this.AccessTab.Controls.Add(this.cbOther_Ident);
			this.AccessTab.Controls.Add(this.cbNonident);
			this.AccessTab.Controls.Add(this.cbNone);
			this.AccessTab.Controls.Add(this.cbIdent_Contact);
			this.AccessTab.Controls.Add(this.cbContact_and_Other);
			this.AccessTab.Controls.Add(this.cbAll);
			this.AccessTab.Controls.Add(this.label1);
			this.AccessTab.Location = new System.Drawing.Point(4, 22);
			this.AccessTab.Name = "AccessTab";
			this.AccessTab.Size = new System.Drawing.Size(488, 382);
			this.AccessTab.TabIndex = 0;
			this.AccessTab.Text = "Access";
			// 
			// cbOther_Ident
			// 
			this.cbOther_Ident.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbOther_Ident.Location = new System.Drawing.Point(24, 327);
			this.cbOther_Ident.Name = "cbOther_Ident";
			this.cbOther_Ident.Size = new System.Drawing.Size(128, 24);
			this.cbOther_Ident.TabIndex = 6;
			this.cbOther_Ident.Text = "Other Ident";
			// 
			// cbNonident
			// 
			this.cbNonident.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbNonident.Location = new System.Drawing.Point(24, 276);
			this.cbNonident.Name = "cbNonident";
			this.cbNonident.Size = new System.Drawing.Size(128, 24);
			this.cbNonident.TabIndex = 5;
			this.cbNonident.Text = "Non Ident";
			// 
			// cbNone
			// 
			this.cbNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbNone.Location = new System.Drawing.Point(24, 72);
			this.cbNone.Name = "cbNone";
			this.cbNone.Size = new System.Drawing.Size(128, 24);
			this.cbNone.TabIndex = 4;
			this.cbNone.Text = "None";
			this.cbNone.CheckedChanged += new System.EventHandler(this.cbNone_CheckedChanged);
			// 
			// cbIdent_Contact
			// 
			this.cbIdent_Contact.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbIdent_Contact.Location = new System.Drawing.Point(24, 174);
			this.cbIdent_Contact.Name = "cbIdent_Contact";
			this.cbIdent_Contact.Size = new System.Drawing.Size(128, 24);
			this.cbIdent_Contact.TabIndex = 3;
			this.cbIdent_Contact.Text = "Ident Contact";
			// 
			// cbContact_and_Other
			// 
			this.cbContact_and_Other.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbContact_and_Other.Location = new System.Drawing.Point(24, 123);
			this.cbContact_and_Other.Name = "cbContact_and_Other";
			this.cbContact_and_Other.Size = new System.Drawing.Size(128, 24);
			this.cbContact_and_Other.TabIndex = 2;
			this.cbContact_and_Other.Text = "Contact and Other";
			// 
			// cbAll
			// 
			this.cbAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbAll.Location = new System.Drawing.Point(24, 225);
			this.cbAll.Name = "cbAll";
			this.cbAll.Size = new System.Drawing.Size(128, 24);
			this.cbAll.TabIndex = 1;
			this.cbAll.Text = "All";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(456, 40);
			this.label1.TabIndex = 0;
			this.label1.Text = "Select the Access category check boxes that are appropriate.  Be careful not to c" +
				"reate a self-contradictory policy!";
			// 
			// CategoriesTab
			// 
			this.CategoriesTab.Controls.Add(this.cbUniqueID);
			this.CategoriesTab.Controls.Add(this.tbOther_Category);
			this.CategoriesTab.Controls.Add(this.cbPurchase);
			this.CategoriesTab.Controls.Add(this.cbPreference);
			this.CategoriesTab.Controls.Add(this.cbPolitical);
			this.CategoriesTab.Controls.Add(this.cbPhysical);
			this.CategoriesTab.Controls.Add(this.cbState);
			this.CategoriesTab.Controls.Add(this.cbOther_Category);
			this.CategoriesTab.Controls.Add(this.cbOnline);
			this.CategoriesTab.Controls.Add(this.cbNavigation);
			this.CategoriesTab.Controls.Add(this.cbLocation);
			this.CategoriesTab.Controls.Add(this.cbInteractive);
			this.CategoriesTab.Controls.Add(this.cbHealth);
			this.CategoriesTab.Controls.Add(this.cbGovernment);
			this.CategoriesTab.Controls.Add(this.cbFinancial);
			this.CategoriesTab.Controls.Add(this.cbDemographic);
			this.CategoriesTab.Controls.Add(this.cbContent);
			this.CategoriesTab.Controls.Add(this.cbComputer);
			this.CategoriesTab.Controls.Add(this.label2);
			this.CategoriesTab.Location = new System.Drawing.Point(4, 22);
			this.CategoriesTab.Name = "CategoriesTab";
			this.CategoriesTab.Size = new System.Drawing.Size(488, 382);
			this.CategoriesTab.TabIndex = 1;
			this.CategoriesTab.Text = "Categories";
			// 
			// cbUniqueID
			// 
			this.cbUniqueID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbUniqueID.Location = new System.Drawing.Point(360, 344);
			this.cbUniqueID.Name = "cbUniqueID";
			this.cbUniqueID.Size = new System.Drawing.Size(112, 24);
			this.cbUniqueID.TabIndex = 19;
			this.cbUniqueID.Text = "Unique ID";
			// 
			// tbOther_Category
			// 
			this.tbOther_Category.Enabled = false;
			this.tbOther_Category.Location = new System.Drawing.Point(192, 320);
			this.tbOther_Category.Name = "tbOther_Category";
			this.tbOther_Category.TabIndex = 18;
			this.tbOther_Category.Text = "";
			// 
			// cbPurchase
			// 
			this.cbPurchase.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbPurchase.Location = new System.Drawing.Point(360, 232);
			this.cbPurchase.Name = "cbPurchase";
			this.cbPurchase.Size = new System.Drawing.Size(112, 24);
			this.cbPurchase.TabIndex = 17;
			this.cbPurchase.Text = "Purchase";
			// 
			// cbPreference
			// 
			this.cbPreference.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbPreference.Location = new System.Drawing.Point(360, 176);
			this.cbPreference.Name = "cbPreference";
			this.cbPreference.Size = new System.Drawing.Size(112, 24);
			this.cbPreference.TabIndex = 16;
			this.cbPreference.Text = "Preference";
			// 
			// cbPolitical
			// 
			this.cbPolitical.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbPolitical.Location = new System.Drawing.Point(360, 120);
			this.cbPolitical.Name = "cbPolitical";
			this.cbPolitical.Size = new System.Drawing.Size(112, 24);
			this.cbPolitical.TabIndex = 15;
			this.cbPolitical.Text = "Political";
			// 
			// cbPhysical
			// 
			this.cbPhysical.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbPhysical.Location = new System.Drawing.Point(360, 64);
			this.cbPhysical.Name = "cbPhysical";
			this.cbPhysical.Size = new System.Drawing.Size(112, 24);
			this.cbPhysical.TabIndex = 14;
			this.cbPhysical.Text = "Physical";
			// 
			// cbState
			// 
			this.cbState.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbState.Location = new System.Drawing.Point(360, 288);
			this.cbState.Name = "cbState";
			this.cbState.Size = new System.Drawing.Size(112, 24);
			this.cbState.TabIndex = 13;
			this.cbState.Text = "State";
			// 
			// cbOther_Category
			// 
			this.cbOther_Category.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbOther_Category.Location = new System.Drawing.Point(192, 288);
			this.cbOther_Category.Name = "cbOther_Category";
			this.cbOther_Category.Size = new System.Drawing.Size(112, 24);
			this.cbOther_Category.TabIndex = 12;
			this.cbOther_Category.Text = "Other Category:";
			this.cbOther_Category.CheckedChanged += new System.EventHandler(this.cbOther_Category_CheckedChanged);
			// 
			// cbOnline
			// 
			this.cbOnline.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbOnline.Location = new System.Drawing.Point(192, 232);
			this.cbOnline.Name = "cbOnline";
			this.cbOnline.Size = new System.Drawing.Size(112, 24);
			this.cbOnline.TabIndex = 11;
			this.cbOnline.Text = "Online";
			// 
			// cbNavigation
			// 
			this.cbNavigation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbNavigation.Location = new System.Drawing.Point(192, 176);
			this.cbNavigation.Name = "cbNavigation";
			this.cbNavigation.Size = new System.Drawing.Size(112, 24);
			this.cbNavigation.TabIndex = 10;
			this.cbNavigation.Text = "Navigation";
			// 
			// cbLocation
			// 
			this.cbLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbLocation.Location = new System.Drawing.Point(192, 120);
			this.cbLocation.Name = "cbLocation";
			this.cbLocation.Size = new System.Drawing.Size(112, 24);
			this.cbLocation.TabIndex = 9;
			this.cbLocation.Text = "Location";
			// 
			// cbInteractive
			// 
			this.cbInteractive.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbInteractive.Location = new System.Drawing.Point(192, 64);
			this.cbInteractive.Name = "cbInteractive";
			this.cbInteractive.Size = new System.Drawing.Size(112, 24);
			this.cbInteractive.TabIndex = 8;
			this.cbInteractive.Text = "Interactive";
			// 
			// cbHealth
			// 
			this.cbHealth.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbHealth.Location = new System.Drawing.Point(24, 344);
			this.cbHealth.Name = "cbHealth";
			this.cbHealth.Size = new System.Drawing.Size(112, 24);
			this.cbHealth.TabIndex = 7;
			this.cbHealth.Text = "Health";
			// 
			// cbGovernment
			// 
			this.cbGovernment.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbGovernment.Location = new System.Drawing.Point(24, 288);
			this.cbGovernment.Name = "cbGovernment";
			this.cbGovernment.Size = new System.Drawing.Size(112, 24);
			this.cbGovernment.TabIndex = 6;
			this.cbGovernment.Text = "Government";
			// 
			// cbFinancial
			// 
			this.cbFinancial.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbFinancial.Location = new System.Drawing.Point(24, 232);
			this.cbFinancial.Name = "cbFinancial";
			this.cbFinancial.Size = new System.Drawing.Size(112, 24);
			this.cbFinancial.TabIndex = 5;
			this.cbFinancial.Text = "Financial";
			// 
			// cbDemographic
			// 
			this.cbDemographic.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbDemographic.Location = new System.Drawing.Point(24, 176);
			this.cbDemographic.Name = "cbDemographic";
			this.cbDemographic.Size = new System.Drawing.Size(112, 24);
			this.cbDemographic.TabIndex = 4;
			this.cbDemographic.Text = "Demographic";
			// 
			// cbContent
			// 
			this.cbContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbContent.Location = new System.Drawing.Point(24, 120);
			this.cbContent.Name = "cbContent";
			this.cbContent.Size = new System.Drawing.Size(112, 24);
			this.cbContent.TabIndex = 3;
			this.cbContent.Text = "Content";
			// 
			// cbComputer
			// 
			this.cbComputer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbComputer.Location = new System.Drawing.Point(24, 64);
			this.cbComputer.Name = "cbComputer";
			this.cbComputer.Size = new System.Drawing.Size(112, 24);
			this.cbComputer.TabIndex = 2;
			this.cbComputer.Text = "Computer";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(16, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(456, 40);
			this.label2.TabIndex = 1;
			this.label2.Text = "Select the data categories check boxes that are appropriate.  Be careful not to c" +
				"reate a self-contradictory policy!";
			// 
			// DisputesTab
			// 
			this.DisputesTab.Controls.Add(this.bRemove);
			this.DisputesTab.Controls.Add(this.bEdit);
			this.DisputesTab.Controls.Add(this.bAdd);
			this.DisputesTab.Controls.Add(this.label3);
			this.DisputesTab.Controls.Add(this.lbDisputes);
			this.DisputesTab.Location = new System.Drawing.Point(4, 22);
			this.DisputesTab.Name = "DisputesTab";
			this.DisputesTab.Size = new System.Drawing.Size(488, 382);
			this.DisputesTab.TabIndex = 2;
			this.DisputesTab.Text = "Disputes and Remedies";
			this.DisputesTab.ToolTipText = "Disputes and Resolution information";
			// 
			// bRemove
			// 
			this.bRemove.Location = new System.Drawing.Point(392, 264);
			this.bRemove.Name = "bRemove";
			this.bRemove.TabIndex = 4;
			this.bRemove.Text = "Remove";
			this.bRemove.Click += new System.EventHandler(this.bRemove_Click);
			// 
			// bEdit
			// 
			this.bEdit.Location = new System.Drawing.Point(292, 264);
			this.bEdit.Name = "bEdit";
			this.bEdit.TabIndex = 3;
			this.bEdit.Text = "Edit";
			this.bEdit.Click += new System.EventHandler(this.bEdit_Click);
			// 
			// bAdd
			// 
			this.bAdd.Location = new System.Drawing.Point(192, 264);
			this.bAdd.Name = "bAdd";
			this.bAdd.TabIndex = 2;
			this.bAdd.Text = "Add";
			this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(8, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(128, 23);
			this.label3.TabIndex = 1;
			this.label3.Text = "Disputes Items:";
			// 
			// lbDisputes
			// 
			this.lbDisputes.Location = new System.Drawing.Point(8, 48);
			this.lbDisputes.Name = "lbDisputes";
			this.lbDisputes.Size = new System.Drawing.Size(464, 199);
			this.lbDisputes.TabIndex = 0;
			this.lbDisputes.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbDisputes_DrawItem);
			// 
			// PurposeTab
			// 
			this.PurposeTab.Controls.Add(this.cmbTelemarketing);
			this.PurposeTab.Controls.Add(this.cmbTailoring);
			this.PurposeTab.Controls.Add(this.cmbPseudo_Decision);
			this.PurposeTab.Controls.Add(this.cmbPseudo_Analysis);
			this.PurposeTab.Controls.Add(this.cmbOther_Purpose);
			this.PurposeTab.Controls.Add(this.cmbIndividual_Decision);
			this.PurposeTab.Controls.Add(this.cmbIndividual_Analysis);
			this.PurposeTab.Controls.Add(this.cmbHistorical);
			this.PurposeTab.Controls.Add(this.cmbDevelop);
			this.PurposeTab.Controls.Add(this.cmbContact);
			this.PurposeTab.Controls.Add(this.cmbAdmin);
			this.PurposeTab.Controls.Add(this.tbOther_Purpose);
			this.PurposeTab.Controls.Add(this.label4);
			this.PurposeTab.Controls.Add(this.cbTelemarketing);
			this.PurposeTab.Controls.Add(this.cbTailoring);
			this.PurposeTab.Controls.Add(this.cbPseudo_Decision);
			this.PurposeTab.Controls.Add(this.cbPseudo_Analysis);
			this.PurposeTab.Controls.Add(this.cbOther_Purpose);
			this.PurposeTab.Controls.Add(this.cbIndividual_Decision);
			this.PurposeTab.Controls.Add(this.cbIndividual_Analysis);
			this.PurposeTab.Controls.Add(this.cbHistorical);
			this.PurposeTab.Controls.Add(this.cbDevelop);
			this.PurposeTab.Controls.Add(this.cbCurrent);
			this.PurposeTab.Controls.Add(this.cbContact);
			this.PurposeTab.Controls.Add(this.cbAdmin);
			this.PurposeTab.Location = new System.Drawing.Point(4, 22);
			this.PurposeTab.Name = "PurposeTab";
			this.PurposeTab.Size = new System.Drawing.Size(488, 382);
			this.PurposeTab.TabIndex = 3;
			this.PurposeTab.Text = "Purpose";
			// 
			// cmbTelemarketing
			// 
			this.cmbTelemarketing.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbTelemarketing.Items.AddRange(new object[] {
																  "Always",
																  "Opt-In",
																  "Opt-Out"});
			this.cmbTelemarketing.Location = new System.Drawing.Point(368, 344);
			this.cmbTelemarketing.Name = "cmbTelemarketing";
			this.cmbTelemarketing.Size = new System.Drawing.Size(104, 23);
			this.cmbTelemarketing.TabIndex = 25;
			// 
			// cmbTailoring
			// 
			this.cmbTailoring.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbTailoring.Items.AddRange(new object[] {
															  "Always",
															  "Opt-In",
															  "Opt-Out"});
			this.cmbTailoring.Location = new System.Drawing.Point(368, 288);
			this.cmbTailoring.Name = "cmbTailoring";
			this.cmbTailoring.Size = new System.Drawing.Size(104, 23);
			this.cmbTailoring.TabIndex = 24;
			// 
			// cmbPseudo_Decision
			// 
			this.cmbPseudo_Decision.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbPseudo_Decision.Items.AddRange(new object[] {
																	"Always",
																	"Opt-In",
																	"Opt-Out"});
			this.cmbPseudo_Decision.Location = new System.Drawing.Point(368, 232);
			this.cmbPseudo_Decision.Name = "cmbPseudo_Decision";
			this.cmbPseudo_Decision.Size = new System.Drawing.Size(104, 23);
			this.cmbPseudo_Decision.TabIndex = 23;
			// 
			// cmbPseudo_Analysis
			// 
			this.cmbPseudo_Analysis.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbPseudo_Analysis.Items.AddRange(new object[] {
																	"Always",
																	"Opt-In",
																	"Opt-Out"});
			this.cmbPseudo_Analysis.Location = new System.Drawing.Point(368, 176);
			this.cmbPseudo_Analysis.Name = "cmbPseudo_Analysis";
			this.cmbPseudo_Analysis.Size = new System.Drawing.Size(104, 23);
			this.cmbPseudo_Analysis.TabIndex = 22;
			// 
			// cmbOther_Purpose
			// 
			this.cmbOther_Purpose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbOther_Purpose.Items.AddRange(new object[] {
																  "Always",
																  "Opt-In",
																  "Opt-Out"});
			this.cmbOther_Purpose.Location = new System.Drawing.Point(368, 120);
			this.cmbOther_Purpose.Name = "cmbOther_Purpose";
			this.cmbOther_Purpose.Size = new System.Drawing.Size(104, 23);
			this.cmbOther_Purpose.TabIndex = 21;
			// 
			// cmbIndividual_Decision
			// 
			this.cmbIndividual_Decision.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbIndividual_Decision.Items.AddRange(new object[] {
																		"Always",
																		"Opt-In",
																		"Opt-Out"});
			this.cmbIndividual_Decision.Location = new System.Drawing.Point(368, 64);
			this.cmbIndividual_Decision.Name = "cmbIndividual_Decision";
			this.cmbIndividual_Decision.Size = new System.Drawing.Size(104, 23);
			this.cmbIndividual_Decision.TabIndex = 20;
			// 
			// cmbIndividual_Analysis
			// 
			this.cmbIndividual_Analysis.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbIndividual_Analysis.Items.AddRange(new object[] {
																		"Always",
																		"Opt-In",
																		"Opt-Out"});
			this.cmbIndividual_Analysis.Location = new System.Drawing.Point(104, 344);
			this.cmbIndividual_Analysis.Name = "cmbIndividual_Analysis";
			this.cmbIndividual_Analysis.Size = new System.Drawing.Size(104, 23);
			this.cmbIndividual_Analysis.TabIndex = 19;
			// 
			// cmbHistorical
			// 
			this.cmbHistorical.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbHistorical.Items.AddRange(new object[] {
															   "Always",
															   "Opt-In",
															   "Opt-Out"});
			this.cmbHistorical.Location = new System.Drawing.Point(104, 288);
			this.cmbHistorical.Name = "cmbHistorical";
			this.cmbHistorical.Size = new System.Drawing.Size(104, 23);
			this.cmbHistorical.TabIndex = 18;
			// 
			// cmbDevelop
			// 
			this.cmbDevelop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbDevelop.Items.AddRange(new object[] {
															"Always",
															"Opt-In",
															"Opt-Out"});
			this.cmbDevelop.Location = new System.Drawing.Point(104, 232);
			this.cmbDevelop.Name = "cmbDevelop";
			this.cmbDevelop.Size = new System.Drawing.Size(104, 23);
			this.cmbDevelop.TabIndex = 17;
			// 
			// cmbContact
			// 
			this.cmbContact.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbContact.Items.AddRange(new object[] {
															"Always",
															"Opt-In",
															"Opt-Out"});
			this.cmbContact.Location = new System.Drawing.Point(104, 120);
			this.cmbContact.Name = "cmbContact";
			this.cmbContact.Size = new System.Drawing.Size(104, 23);
			this.cmbContact.TabIndex = 15;
			// 
			// cmbAdmin
			// 
			this.cmbAdmin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbAdmin.Items.AddRange(new object[] {
														  "Always",
														  "Opt-In",
														  "Opt-Out"});
			this.cmbAdmin.Location = new System.Drawing.Point(104, 64);
			this.cmbAdmin.Name = "cmbAdmin";
			this.cmbAdmin.Size = new System.Drawing.Size(104, 23);
			this.cmbAdmin.TabIndex = 14;
			// 
			// tbOther_Purpose
			// 
			this.tbOther_Purpose.Enabled = false;
			this.tbOther_Purpose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tbOther_Purpose.Location = new System.Drawing.Point(248, 144);
			this.tbOther_Purpose.Name = "tbOther_Purpose";
			this.tbOther_Purpose.Size = new System.Drawing.Size(104, 21);
			this.tbOther_Purpose.TabIndex = 13;
			this.tbOther_Purpose.Text = "";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(16, 8);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(464, 40);
			this.label4.TabIndex = 12;
			this.label4.Text = "Select the Purpose items appropriately using the check boxes.  Use the drop down " +
				"boxes to set the \"required\" attribute.";
			// 
			// cbTelemarketing
			// 
			this.cbTelemarketing.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbTelemarketing.Location = new System.Drawing.Point(232, 344);
			this.cbTelemarketing.Name = "cbTelemarketing";
			this.cbTelemarketing.Size = new System.Drawing.Size(128, 24);
			this.cbTelemarketing.TabIndex = 11;
			this.cbTelemarketing.Text = "Telemarketing";
			this.cbTelemarketing.ThreeState = true;
			this.cbTelemarketing.CheckedChanged += new System.EventHandler(this.cbTelemarketing_CheckedChanged);
			// 
			// cbTailoring
			// 
			this.cbTailoring.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbTailoring.Location = new System.Drawing.Point(232, 288);
			this.cbTailoring.Name = "cbTailoring";
			this.cbTailoring.Size = new System.Drawing.Size(128, 24);
			this.cbTailoring.TabIndex = 10;
			this.cbTailoring.Text = "Tailoring";
			this.cbTailoring.ThreeState = true;
			this.cbTailoring.CheckedChanged += new System.EventHandler(this.cbTailoring_CheckedChanged);
			// 
			// cbPseudo_Decision
			// 
			this.cbPseudo_Decision.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbPseudo_Decision.Location = new System.Drawing.Point(232, 232);
			this.cbPseudo_Decision.Name = "cbPseudo_Decision";
			this.cbPseudo_Decision.Size = new System.Drawing.Size(128, 16);
			this.cbPseudo_Decision.TabIndex = 9;
			this.cbPseudo_Decision.Text = "Pseudo Decision";
			this.cbPseudo_Decision.ThreeState = true;
			this.cbPseudo_Decision.CheckedChanged += new System.EventHandler(this.cbPseudo_Decision_CheckedChanged);
			// 
			// cbPseudo_Analysis
			// 
			this.cbPseudo_Analysis.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbPseudo_Analysis.Location = new System.Drawing.Point(232, 176);
			this.cbPseudo_Analysis.Name = "cbPseudo_Analysis";
			this.cbPseudo_Analysis.Size = new System.Drawing.Size(128, 16);
			this.cbPseudo_Analysis.TabIndex = 8;
			this.cbPseudo_Analysis.Text = "Pseudo Analysis";
			this.cbPseudo_Analysis.ThreeState = true;
			this.cbPseudo_Analysis.CheckedChanged += new System.EventHandler(this.cbPseudo_Analysis_CheckedChanged);
			// 
			// cbOther_Purpose
			// 
			this.cbOther_Purpose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbOther_Purpose.Location = new System.Drawing.Point(232, 120);
			this.cbOther_Purpose.Name = "cbOther_Purpose";
			this.cbOther_Purpose.Size = new System.Drawing.Size(128, 16);
			this.cbOther_Purpose.TabIndex = 7;
			this.cbOther_Purpose.Text = "Other Purpose:";
			this.cbOther_Purpose.ThreeState = true;
			this.cbOther_Purpose.CheckedChanged += new System.EventHandler(this.cbOther_Purpose_CheckedChanged);
			// 
			// cbIndividual_Decision
			// 
			this.cbIndividual_Decision.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbIndividual_Decision.Location = new System.Drawing.Point(232, 64);
			this.cbIndividual_Decision.Name = "cbIndividual_Decision";
			this.cbIndividual_Decision.Size = new System.Drawing.Size(128, 16);
			this.cbIndividual_Decision.TabIndex = 6;
			this.cbIndividual_Decision.Text = "Individual Decision";
			this.cbIndividual_Decision.ThreeState = true;
			this.cbIndividual_Decision.CheckedChanged += new System.EventHandler(this.cbIndividual_Decision_CheckedChanged);
			// 
			// cbIndividual_Analysis
			// 
			this.cbIndividual_Analysis.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbIndividual_Analysis.Location = new System.Drawing.Point(16, 336);
			this.cbIndividual_Analysis.Name = "cbIndividual_Analysis";
			this.cbIndividual_Analysis.Size = new System.Drawing.Size(80, 32);
			this.cbIndividual_Analysis.TabIndex = 5;
			this.cbIndividual_Analysis.Text = "Individual Analysis";
			this.cbIndividual_Analysis.ThreeState = true;
			this.cbIndividual_Analysis.CheckedChanged += new System.EventHandler(this.cbIndividual_Analysis_CheckedChanged);
			// 
			// cbHistorical
			// 
			this.cbHistorical.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbHistorical.Location = new System.Drawing.Point(16, 288);
			this.cbHistorical.Name = "cbHistorical";
			this.cbHistorical.Size = new System.Drawing.Size(80, 16);
			this.cbHistorical.TabIndex = 4;
			this.cbHistorical.Text = "Historical";
			this.cbHistorical.ThreeState = true;
			this.cbHistorical.CheckedChanged += new System.EventHandler(this.cbHistorical_CheckedChanged);
			// 
			// cbDevelop
			// 
			this.cbDevelop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbDevelop.Location = new System.Drawing.Point(16, 232);
			this.cbDevelop.Name = "cbDevelop";
			this.cbDevelop.Size = new System.Drawing.Size(80, 16);
			this.cbDevelop.TabIndex = 3;
			this.cbDevelop.Text = "Develop";
			this.cbDevelop.ThreeState = true;
			// 
			// cbCurrent
			// 
			this.cbCurrent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbCurrent.Location = new System.Drawing.Point(16, 176);
			this.cbCurrent.Name = "cbCurrent";
			this.cbCurrent.Size = new System.Drawing.Size(80, 16);
			this.cbCurrent.TabIndex = 2;
			this.cbCurrent.Text = "Current";
			this.cbCurrent.ThreeState = true;
			// 
			// cbContact
			// 
			this.cbContact.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbContact.Location = new System.Drawing.Point(16, 120);
			this.cbContact.Name = "cbContact";
			this.cbContact.Size = new System.Drawing.Size(80, 16);
			this.cbContact.TabIndex = 1;
			this.cbContact.Text = "Contact";
			this.cbContact.ThreeState = true;
			this.cbContact.CheckedChanged += new System.EventHandler(this.cbContact_CheckedChanged);
			// 
			// cbAdmin
			// 
			this.cbAdmin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbAdmin.Location = new System.Drawing.Point(16, 64);
			this.cbAdmin.Name = "cbAdmin";
			this.cbAdmin.Size = new System.Drawing.Size(80, 16);
			this.cbAdmin.TabIndex = 0;
			this.cbAdmin.Text = "Admin";
			this.cbAdmin.ThreeState = true;
			this.cbAdmin.CheckedChanged += new System.EventHandler(this.cbAdmin_CheckedChanged);
			// 
			// RecipientTab
			// 
			this.RecipientTab.Controls.Add(this.label5);
			this.RecipientTab.Controls.Add(this.cmbUnrelated);
			this.RecipientTab.Controls.Add(this.cmbSame);
			this.RecipientTab.Controls.Add(this.cmbPublic);
			this.RecipientTab.Controls.Add(this.cmbOther_Recipient);
			this.RecipientTab.Controls.Add(this.cmbDelivery);
			this.RecipientTab.Controls.Add(this.cbUnrelated);
			this.RecipientTab.Controls.Add(this.cbSame);
			this.RecipientTab.Controls.Add(this.cbPublic);
			this.RecipientTab.Controls.Add(this.cbOurs);
			this.RecipientTab.Controls.Add(this.cbOther_Recipient);
			this.RecipientTab.Controls.Add(this.cbDelivery);
			this.RecipientTab.Location = new System.Drawing.Point(4, 22);
			this.RecipientTab.Name = "RecipientTab";
			this.RecipientTab.Size = new System.Drawing.Size(488, 382);
			this.RecipientTab.TabIndex = 4;
			this.RecipientTab.Text = "Recipient";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(12, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(464, 40);
			this.label5.TabIndex = 31;
			this.label5.Text = "Select the Recipient items appropriately using the check boxes.  Use the drop dow" +
				"n boxes to set the \"required\" attribute.";
			// 
			// cmbUnrelated
			// 
			this.cmbUnrelated.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbUnrelated.Items.AddRange(new object[] {
															  "Always",
															  "Opt-In",
															  "Opt-Out"});
			this.cmbUnrelated.Location = new System.Drawing.Point(144, 342);
			this.cmbUnrelated.Name = "cmbUnrelated";
			this.cmbUnrelated.Size = new System.Drawing.Size(104, 23);
			this.cmbUnrelated.TabIndex = 30;
			// 
			// cmbSame
			// 
			this.cmbSame.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbSame.Items.AddRange(new object[] {
														 "Always",
														 "Opt-In",
														 "Opt-Out"});
			this.cmbSame.Location = new System.Drawing.Point(144, 288);
			this.cmbSame.Name = "cmbSame";
			this.cmbSame.Size = new System.Drawing.Size(104, 23);
			this.cmbSame.TabIndex = 29;
			// 
			// cmbPublic
			// 
			this.cmbPublic.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbPublic.Items.AddRange(new object[] {
														   "Always",
														   "Opt-In",
														   "Opt-Out"});
			this.cmbPublic.Location = new System.Drawing.Point(144, 234);
			this.cmbPublic.Name = "cmbPublic";
			this.cmbPublic.Size = new System.Drawing.Size(104, 23);
			this.cmbPublic.TabIndex = 28;
			// 
			// cmbOther_Recipient
			// 
			this.cmbOther_Recipient.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbOther_Recipient.Items.AddRange(new object[] {
																	"Always",
																	"Opt-In",
																	"Opt-Out"});
			this.cmbOther_Recipient.Location = new System.Drawing.Point(144, 126);
			this.cmbOther_Recipient.Name = "cmbOther_Recipient";
			this.cmbOther_Recipient.Size = new System.Drawing.Size(104, 23);
			this.cmbOther_Recipient.TabIndex = 27;
			// 
			// cmbDelivery
			// 
			this.cmbDelivery.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbDelivery.Items.AddRange(new object[] {
															 "Always",
															 "Opt-In",
															 "Opt-Out"});
			this.cmbDelivery.Location = new System.Drawing.Point(144, 72);
			this.cmbDelivery.Name = "cmbDelivery";
			this.cmbDelivery.Size = new System.Drawing.Size(104, 23);
			this.cmbDelivery.TabIndex = 26;
			// 
			// cbUnrelated
			// 
			this.cbUnrelated.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbUnrelated.Location = new System.Drawing.Point(24, 342);
			this.cbUnrelated.Name = "cbUnrelated";
			this.cbUnrelated.Size = new System.Drawing.Size(112, 16);
			this.cbUnrelated.TabIndex = 25;
			this.cbUnrelated.Text = "Unrelated";
			this.cbUnrelated.ThreeState = true;
			this.cbUnrelated.CheckedChanged += new System.EventHandler(this.cbUnrelated_CheckedChanged);
			// 
			// cbSame
			// 
			this.cbSame.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbSame.Location = new System.Drawing.Point(24, 288);
			this.cbSame.Name = "cbSame";
			this.cbSame.Size = new System.Drawing.Size(112, 16);
			this.cbSame.TabIndex = 24;
			this.cbSame.Text = "Same";
			this.cbSame.ThreeState = true;
			this.cbSame.CheckedChanged += new System.EventHandler(this.cbSame_CheckedChanged);
			// 
			// cbPublic
			// 
			this.cbPublic.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbPublic.Location = new System.Drawing.Point(24, 234);
			this.cbPublic.Name = "cbPublic";
			this.cbPublic.Size = new System.Drawing.Size(112, 16);
			this.cbPublic.TabIndex = 23;
			this.cbPublic.Text = "Public";
			this.cbPublic.ThreeState = true;
			this.cbPublic.CheckedChanged += new System.EventHandler(this.cbPublic_CheckedChanged);
			// 
			// cbOurs
			// 
			this.cbOurs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbOurs.Location = new System.Drawing.Point(24, 180);
			this.cbOurs.Name = "cbOurs";
			this.cbOurs.Size = new System.Drawing.Size(112, 16);
			this.cbOurs.TabIndex = 22;
			this.cbOurs.Text = "Ours";
			this.cbOurs.ThreeState = true;
			// 
			// cbOther_Recipient
			// 
			this.cbOther_Recipient.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbOther_Recipient.Location = new System.Drawing.Point(24, 126);
			this.cbOther_Recipient.Name = "cbOther_Recipient";
			this.cbOther_Recipient.Size = new System.Drawing.Size(112, 16);
			this.cbOther_Recipient.TabIndex = 21;
			this.cbOther_Recipient.Text = "Other Recipient";
			this.cbOther_Recipient.ThreeState = true;
			this.cbOther_Recipient.CheckedChanged += new System.EventHandler(this.cbOther_Recipient_CheckedChanged);
			// 
			// cbDelivery
			// 
			this.cbDelivery.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbDelivery.Location = new System.Drawing.Point(24, 72);
			this.cbDelivery.Name = "cbDelivery";
			this.cbDelivery.Size = new System.Drawing.Size(112, 16);
			this.cbDelivery.TabIndex = 20;
			this.cbDelivery.Text = "Delivery";
			this.cbDelivery.ThreeState = true;
			this.cbDelivery.CheckedChanged += new System.EventHandler(this.cbDelivery_CheckedChanged);
			// 
			// RetentionTab
			// 
			this.RetentionTab.Controls.Add(this.label6);
			this.RetentionTab.Controls.Add(this.cbStated_Purpose);
			this.RetentionTab.Controls.Add(this.cbNo_Retention);
			this.RetentionTab.Controls.Add(this.cbLegal_Requirement);
			this.RetentionTab.Controls.Add(this.cbIndefinitely);
			this.RetentionTab.Controls.Add(this.cbBusiness_Practices);
			this.RetentionTab.Location = new System.Drawing.Point(4, 22);
			this.RetentionTab.Name = "RetentionTab";
			this.RetentionTab.Size = new System.Drawing.Size(488, 382);
			this.RetentionTab.TabIndex = 5;
			this.RetentionTab.Text = "Retention";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label6.Location = new System.Drawing.Point(16, 16);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(456, 64);
			this.label6.TabIndex = 13;
			this.label6.Text = "Select the Retention category check boxes that are appropriate.  Be careful not t" +
				"o create a self-contradictory policy!";
			// 
			// cbStated_Purpose
			// 
			this.cbStated_Purpose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbStated_Purpose.Location = new System.Drawing.Point(16, 336);
			this.cbStated_Purpose.Name = "cbStated_Purpose";
			this.cbStated_Purpose.Size = new System.Drawing.Size(128, 24);
			this.cbStated_Purpose.TabIndex = 11;
			this.cbStated_Purpose.Text = "Stated Purpose";
			// 
			// cbNo_Retention
			// 
			this.cbNo_Retention.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbNo_Retention.Location = new System.Drawing.Point(16, 96);
			this.cbNo_Retention.Name = "cbNo_Retention";
			this.cbNo_Retention.Size = new System.Drawing.Size(128, 24);
			this.cbNo_Retention.TabIndex = 10;
			this.cbNo_Retention.Text = "No Retention";
			this.cbNo_Retention.CheckedChanged += new System.EventHandler(this.cbNo_Retention_CheckedChanged);
			// 
			// cbLegal_Requirement
			// 
			this.cbLegal_Requirement.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbLegal_Requirement.Location = new System.Drawing.Point(16, 216);
			this.cbLegal_Requirement.Name = "cbLegal_Requirement";
			this.cbLegal_Requirement.Size = new System.Drawing.Size(128, 24);
			this.cbLegal_Requirement.TabIndex = 9;
			this.cbLegal_Requirement.Text = "Legal Requirement";
			// 
			// cbIndefinitely
			// 
			this.cbIndefinitely.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbIndefinitely.Location = new System.Drawing.Point(16, 156);
			this.cbIndefinitely.Name = "cbIndefinitely";
			this.cbIndefinitely.Size = new System.Drawing.Size(128, 24);
			this.cbIndefinitely.TabIndex = 8;
			this.cbIndefinitely.Text = "Indefinitely";
			// 
			// cbBusiness_Practices
			// 
			this.cbBusiness_Practices.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cbBusiness_Practices.Location = new System.Drawing.Point(16, 276);
			this.cbBusiness_Practices.Name = "cbBusiness_Practices";
			this.cbBusiness_Practices.Size = new System.Drawing.Size(128, 24);
			this.cbBusiness_Practices.TabIndex = 7;
			this.cbBusiness_Practices.Text = "Business Practices";
			// 
			// MiscTab
			// 
			this.MiscTab.Controls.Add(this.cbNonidentifiable);
			this.MiscTab.Controls.Add(this.tbConsequence);
			this.MiscTab.Controls.Add(this.label8);
			this.MiscTab.Controls.Add(this.label7);
			this.MiscTab.Location = new System.Drawing.Point(4, 22);
			this.MiscTab.Name = "MiscTab";
			this.MiscTab.Size = new System.Drawing.Size(488, 382);
			this.MiscTab.TabIndex = 6;
			this.MiscTab.Text = "Misc";
			this.MiscTab.ToolTipText = "Other P3P data";
			// 
			// cbNonidentifiable
			// 
			this.cbNonidentifiable.Location = new System.Drawing.Point(16, 120);
			this.cbNonidentifiable.Name = "cbNonidentifiable";
			this.cbNonidentifiable.TabIndex = 3;
			this.cbNonidentifiable.Text = "Non Identifiable";
			// 
			// tbConsequence
			// 
			this.tbConsequence.Location = new System.Drawing.Point(120, 72);
			this.tbConsequence.Name = "tbConsequence";
			this.tbConsequence.Size = new System.Drawing.Size(352, 20);
			this.tbConsequence.TabIndex = 2;
			this.tbConsequence.Text = "";
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label8.Location = new System.Drawing.Point(16, 72);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(88, 16);
			this.label8.TabIndex = 1;
			this.label8.Text = "Consequence";
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label7.Location = new System.Drawing.Point(16, 16);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(456, 40);
			this.label7.TabIndex = 0;
			this.label7.Text = "This page has some Statement-specific data, things that didn\'t fit into the other" +
				" categories.";
			// 
			// bReset
			// 
			this.bReset.Location = new System.Drawing.Point(8, 424);
			this.bReset.Name = "bReset";
			this.bReset.TabIndex = 1;
			this.bReset.Text = "Reset";
			this.bReset.Click += new System.EventHandler(this.bReset_Click);
			// 
			// bCancel
			// 
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(424, 424);
			this.bCancel.Name = "bCancel";
			this.bCancel.TabIndex = 2;
			this.bCancel.Text = "Cancel";
			this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
			// 
			// bSave
			// 
			this.bSave.Location = new System.Drawing.Point(328, 424);
			this.bSave.Name = "bSave";
			this.bSave.TabIndex = 3;
			this.bSave.Text = "Save";
			this.bSave.Click += new System.EventHandler(this.bSave_Click);
			// 
			// P3PCreator
			// 
			this.AcceptButton = this.bSave;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.AutoScroll = true;
			this.CancelButton = this.bCancel;
			this.ClientSize = new System.Drawing.Size(512, 461);
			this.Controls.Add(this.bSave);
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.bReset);
			this.Controls.Add(this.P3PTabs);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(520, 488);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(520, 488);
			this.Name = "P3PCreator";
			this.Text = "P3P Policy Editor";
			this.Load += new System.EventHandler(this.P3PCreator_Load);
			this.P3PTabs.ResumeLayout(false);
			this.AccessTab.ResumeLayout(false);
			this.CategoriesTab.ResumeLayout(false);
			this.DisputesTab.ResumeLayout(false);
			this.PurposeTab.ResumeLayout(false);
			this.RecipientTab.ResumeLayout(false);
			this.RetentionTab.ResumeLayout(false);
			this.MiscTab.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Disputes Details
		private void bAdd_Click(object sender, System.EventArgs e)
		{
			// create a new Disputes Item and add it to the list
			P3P.DISPUTES newDisp = new P3P.DISPUTES();

			// add it into the list box
			this.lbDisputes.Items.Add(newDisp);

			// show a dialog box for details editing
			newDisp = DoDetails(newDisp);

			// update the view
			lbDisputes.Update();

			return;
		}

		/// <summary>
		/// Show the Disputes Details window to allow the user to modify
		/// the properties of a particular Disputes item.  If the dialog
		/// result is OK then the changes made in the details box are
		/// returned.
		/// </summary>
		/// <param name="disp">The Disputes item to display for editing</param>
		/// <returns>The edited Disputes item if the user clicked OK.  Otherwise
		/// the original Disputes item is returned.</returns>		
		private P3P.DISPUTES DoDetails(P3P.DISPUTES disp)
		{
			// the dialog result from the details window
			DialogResult res;

			// show a details box to let the user set information
			m_details.DisputesItem = disp;			
			res = m_details.ShowDialog(this);

			// check the dialog box result
			switch(res)
			{
					// if the dialog result is Ok, save the new data
				case DialogResult.OK:
					return m_details.DisputesItem;
					break;

					// if it's cancel, just ignore
				default:
					return disp;
			}
		}
			
		/// <summary>
		/// Custom drawing of items in the list box.  Draws Disputes items nicely.
		/// </summary>
		/// <param name="sender">Object that invoked the DrawItem method</param>
		/// <param name="e">Event Description</param>
		private void lbDisputes_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			// Set the DrawMode property to draw fixed sized items.
			lbDisputes.DrawMode = DrawMode.OwnerDrawFixed;
			// Draw the background of the ListBox control for each item.
			e.DrawBackground();

			// the disputes item about to be drawn
			P3P.DISPUTES disp = lbDisputes.Items[e.Index] as P3P.DISPUTES;

			// create a string based on the remedy type and service field
			string text = "";
			switch(disp.resolution_type)
			{
				case P3P.RESOLUTION_TYPE.COURT:
					text = "Court: ";
					break;

				case P3P.RESOLUTION_TYPE.INDEPENDENT:
					text = "Independent: ";
					break;

				case P3P.RESOLUTION_TYPE.LAW:
					text = "Law: ";
					break;

				case P3P.RESOLUTION_TYPE.SERVICE:
					text = "Service: ";
					break;
			}

			text += disp.service;		
			
			// Draw the current item text based on the above made string
			e.Graphics.DrawString(text, e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);

			// If the ListBox has focus, draw a focus rectangle around the selected item.
			e.DrawFocusRectangle();		
		}


		/// <summary>
		/// Edit the currently selected item in the list box
		/// </summary>
		/// <param name="sender">Object that invoked the button click event</param>
		/// <param name="e">Event Description</param>
		private void bEdit_Click(object sender, System.EventArgs e)
		{
			// if no item selected, return
			if ( lbDisputes.SelectedIndex == -1 ) return;

			// get the currently highlighted disputes item
			P3P.DISPUTES disp = lbDisputes.SelectedItem as P3P.DISPUTES;

			// we have to show a dialog box with the selected item
			disp = this.DoDetails(disp);		
		}

		private void bRemove_Click(object sender, System.EventArgs e)
		{
			// if no item selected, return
			if ( lbDisputes.SelectedIndex == -1 ) return;

			// remove the currently highlighted item
			lbDisputes.Items.Remove(lbDisputes.SelectedItem);
		
		}
		#endregion

		#region Access None CheckBox Manipulations
		private void cbNone_CheckedChanged(object sender, System.EventArgs e)
		{
			// if this check box is checked then we disable the rest of the
			// check boxes
			if ( cbNone.Checked == true )
			{
				this.cbAll.Enabled = false;
				this.cbContact_and_Other.Enabled = false;
				this.cbIdent_Contact.Enabled = false;
				this.cbNonident.Enabled = false;
				this.cbOther_Ident.Enabled = false;
			}
			// if it's now unchecked then we can enable the rest of them
			else
			{
				this.cbAll.Enabled = true;
				this.cbContact_and_Other.Enabled = true;
				this.cbIdent_Contact.Enabled = true;
				this.cbNonident.Enabled = true;
				this.cbOther_Ident.Enabled = true;
			}
		}
		#endregion

		#region Access Tab TextBox Manipulations
		private void cbOther_Category_CheckedChanged(object sender, System.EventArgs e)
		{
			// if this check box is now checked then we can fill in a value
			// for the text box under Other Category.  If it's unchecked then
			// we must dis-enable it
			this.tbOther_Category.Enabled = cbOther_Category.Checked;
		}
		#endregion

		#region Purpose Tab ComboBox Manipulations
		private void cbOther_Purpose_CheckedChanged(object sender, System.EventArgs e)
		{
			// if this check box is now checked then we can fill in a value
			// for the text box under Other Purpose.  If it's unchecked then
			// we must dis-enable it
			this.tbOther_Purpose.Enabled = cbOther_Purpose.Checked;
			
			// when this check box is checked we activate the associated "required"
			// field combobox.  When it's unchecked we disable it.
			this.cmbOther_Purpose.Enabled = cbOther_Purpose.Checked;
		}

		private void cbAdmin_CheckedChanged(object sender, System.EventArgs e)
		{
			// when this check box is checked we activate the associated "required"
			// field combobox.  When it's unchecked we disable it.
			this.cmbAdmin.Enabled = cbAdmin.Checked;
		}

		private void cbContact_CheckedChanged(object sender, System.EventArgs e)
		{
			// when this check box is checked we activate the associated "required"
			// field combobox.  When it's unchecked we disable it.
			this.cmbContact.Enabled = cbContact.Checked;		
		}

		private void cbDevelop_CheckedChanged_1(object sender, System.EventArgs e)
		{
			// when this check box is checked we activate the associated "required"
			// field combobox.  When it's unchecked we disable it.
			this.cmbDevelop.Enabled = cbDevelop.Checked;		
		}

		private void cbHistorical_CheckedChanged(object sender, System.EventArgs e)
		{
			// when this check box is checked we activate the associated "required"
			// field combobox.  When it's unchecked we disable it.
			this.cmbHistorical.Enabled = cbHistorical.Checked;			
		}

		private void cbIndividual_Analysis_CheckedChanged(object sender, System.EventArgs e)
		{
			// when this check box is checked we activate the associated "required"
			// field combobox.  When it's unchecked we disable it.
			this.cmbIndividual_Analysis.Enabled = cbIndividual_Analysis.Checked;						
		}

		private void cbIndividual_Decision_CheckedChanged(object sender, System.EventArgs e)
		{
			// when this check box is checked we activate the associated "required"
			// field combobox.  When it's unchecked we disable it.
			this.cmbIndividual_Decision.Enabled = cbIndividual_Decision.Checked;									
		}

		private void cbPseudo_Analysis_CheckedChanged(object sender, System.EventArgs e)
		{
			// when this check box is checked we activate the associated "required"
			// field combobox.  When it's unchecked we disable it.
			this.cmbPseudo_Analysis.Enabled = cbPseudo_Analysis.Checked;	
		}

		private void cbPseudo_Decision_CheckedChanged(object sender, System.EventArgs e)
		{
			// when this check box is checked we activate the associated "required"
			// field combobox.  When it's unchecked we disable it.
			this.cmbPseudo_Decision.Enabled = cbPseudo_Decision.Checked;			
		}

		private void cbTailoring_CheckedChanged(object sender, System.EventArgs e)
		{
			// when this check box is checked we activate the associated "required"
			// field combobox.  When it's unchecked we disable it.
			this.cmbTailoring.Enabled = cbTailoring.Checked;	
		}

		private void cbTelemarketing_CheckedChanged(object sender, System.EventArgs e)
		{
			// when this check box is checked we activate the associated "required"
			// field combobox.  When it's unchecked we disable it.
			this.cmbTelemarketing.Enabled = cbTelemarketing.Checked;
		}
		#endregion

		#region Recipient ComboBox Manipulations
		private void cbDelivery_CheckedChanged(object sender, System.EventArgs e)
		{
			// when this check box is checked we activate the associated "required"
			// field combobox.  When it's unchecked we disable it.
			this.cmbDelivery.Enabled = cbDelivery.Checked;				
		}

		private void cbOther_Recipient_CheckedChanged(object sender, System.EventArgs e)
		{
			// when this check box is checked we activate the associated "required"
			// field combobox.  When it's unchecked we disable it.
			this.cmbOther_Recipient.Enabled = cbOther_Recipient.Checked;						
		}

		private void cbPublic_CheckedChanged(object sender, System.EventArgs e)
		{
			// when this check box is checked we activate the associated "required"
			// field combobox.  When it's unchecked we disable it.
			this.cmbPublic.Enabled = cbPublic.Checked;		
		}

		private void cbSame_CheckedChanged(object sender, System.EventArgs e)
		{
			// when this check box is checked we activate the associated "required"
			// field combobox.  When it's unchecked we disable it.
			this.cmbSame.Enabled = cbSame.Checked;
		}

		private void cbUnrelated_CheckedChanged(object sender, System.EventArgs e)
		{
			// when this check box is checked we activate the associated "required"
			// field combobox.  When it's unchecked we disable it.
			this.cmbUnrelated.Enabled = cbUnrelated.Checked;	
		}
		#endregion

		#region Retention None CheckBox Manipulations
		private void cbNo_Retention_CheckedChanged(object sender, System.EventArgs e)
		{
			// if this check box is checked then we disable the rest of the
			// check boxes
			if ( this.cbNo_Retention.Checked )
			{
				this.cbBusiness_Practices.Enabled = false;
				this.cbIndefinitely.Enabled = false;
				this.cbLegal_Requirement.Enabled = false;
				this.cbStated_Purpose.Enabled = false;
			}
				// if it's unchecked then we can enable the rest of them
			else
			{
				this.cbBusiness_Practices.Enabled = true;
				this.cbIndefinitely.Enabled = true;
				this.cbLegal_Requirement.Enabled = true;
				this.cbStated_Purpose.Enabled = true;
			}
		}
		#endregion

		#region Load and Store Member Policy to Property Pages
		/// <summary>
		/// Sets the P3P creator property pages to match the stored P3P policy
		/// </summary>
		private void LoadPolicy()
		{
			// load up the P3P policy into the window appropriately
			
			// Access Tab
			// enable everything
			this.cbAll.Enabled = true;
			this.cbContact_and_Other.Enabled = true;
			this.cbIdent_Contact.Enabled = true;
			this.cbNone.Enabled = true;
			this.cbNonident.Enabled = true;
			this.cbOther_Ident.Enabled = true;

			// now check things appropriately
			// do None last as it overrides everything else
			this.cbAll.Checked = m_policy.access.all;
			this.cbContact_and_Other.Checked = m_policy.access.contact_and_other;
			this.cbIdent_Contact.Checked = m_policy.access.ident_contact;
			this.cbNonident.Checked = m_policy.access.nonident;
			this.cbOther_Ident.Checked = m_policy.access.other_ident;
			this.cbNone.Checked = m_policy.access.none;

			// Categories Tab
			// enable everything
			this.cbComputer.Enabled = true;
			this.cbContent.Enabled = true;
			this.cbDemographic.Enabled = true;
			this.cbFinancial.Enabled = true;
			this.cbGovernment.Enabled = true;
			this.cbHealth.Enabled = true;
			this.cbInteractive.Enabled = true;
			this.cbLocation.Enabled = true;
			this.cbNavigation.Enabled = true;
			this.cbOnline.Enabled = true;
			this.cbOther_Category.Enabled = true;
			this.cbPhysical.Enabled = true;
			this.cbPolitical.Enabled = true;
			this.cbPreference.Enabled = true;
			this.cbPurchase.Enabled = true;
			this.cbState.Enabled = true;
			this.cbUniqueID.Enabled = true;

			// now check things appropriately
			// follow just the first statement in the list
			this.cbComputer.Checked = m_policy.m_statements[0].categories.computer;
			this.cbContent.Checked = m_policy.m_statements[0].categories.content;
			this.cbDemographic.Checked = m_policy.m_statements[0].categories.demographic;
			this.cbFinancial.Checked = m_policy.m_statements[0].categories.financial;
			this.cbGovernment.Checked = m_policy.m_statements[0].categories.government;
			this.cbHealth.Checked = m_policy.m_statements[0].categories.health;
			this.cbInteractive.Checked = m_policy.m_statements[0].categories.interactive;
			this.cbLocation.Checked = m_policy.m_statements[0].categories.location;
			this.cbNavigation.Checked = m_policy.m_statements[0].categories.navigation;
			this.cbOnline.Checked = m_policy.m_statements[0].categories.online;

			// see if there is something in the 'other category' listing
			if ( m_policy.m_statements[0].categories.other_category != null
				&& m_policy.m_statements[0].categories.other_category != "")
			{
				this.cbOther_Category.Checked = true;
				this.tbOther_Category.Text = m_policy.m_statements[0].categories.other_category;
			}
			else
			{
				this.cbOther_Category.Checked = false;
			}

			this.cbPhysical.Checked = m_policy.m_statements[0].categories.physical;
			this.cbPolitical.Checked = m_policy.m_statements[0].categories.political;
			this.cbPreference.Checked = m_policy.m_statements[0].categories.preference;
			this.cbPurchase.Checked = m_policy.m_statements[0].categories.purchase;
			this.cbState.Checked = m_policy.m_statements[0].categories.state;
			this.cbUniqueID.Checked = m_policy.m_statements[0].categories.uniqueid;

			// Disputes Listing
			// empty it all out first
			this.lbDisputes.Items.Clear();

			// for each disputes element we have to create a new entry in the listbox
			foreach (P3P.DISPUTES d in m_policy.m_disputes)
			{
				if ( d != null )
					lbDisputes.Items.Add(d);
			}

			// Purpose Tab
			// everything should be enabled by default

			// set the checked things appropriately
			this.cbAdmin.Checked = m_policy.m_statements[0].purpose.admin.present;
			this.cbContact.Checked = m_policy.m_statements[0].purpose.contact.present;
			this.cbCurrent.Checked = m_policy.m_statements[0].purpose.current;
			this.cbDevelop.Checked = m_policy.m_statements[0].purpose.develop.present;
			this.cbHistorical.Checked = m_policy.m_statements[0].purpose.historical.present;
			this.cbIndividual_Analysis.Checked = m_policy.m_statements[0].purpose.individual_analysis.present;
			this.cbIndividual_Decision.Checked = m_policy.m_statements[0].purpose.individual_decision.present;

			// set the other purpose checkbox and required appropriately
			if ( m_policy.m_statements[0].purpose.other_purpose != "" )
			{
				this.cbOther_Purpose.Checked = true;
				this.tbOther_Purpose.Text = m_policy.m_statements[0].purpose.other_purpose;
				this.cmbOther_Purpose.Enabled = true;

				switch ( m_policy.m_statements[0].purpose.other_purpose_required )
				{
					case P3P.REQUIRED.always:
						this.cmbOther_Purpose.SelectedIndex = 0;//"Always"
						break;

					case P3P.REQUIRED.opt_in:
						this.cmbOther_Purpose.SelectedIndex = 1;//"Opt-In"
						break;

					case P3P.REQUIRED.opt_out:
						this.cmbOther_Purpose.SelectedIndex = 2;//"Opt-Out"
						break;
				}
			}
			else
			{
				this.cbOther_Purpose.Checked = false;
			}

			this.cbPseudo_Analysis.Checked = m_policy.m_statements[0].purpose.pseudo_analysis.present;
			this.cbPseudo_Decision.Checked = m_policy.m_statements[0].purpose.pseudo_decision.present;
			this.cbTailoring.Checked = m_policy.m_statements[0].purpose.tailoring.present;
			this.cbTelemarketing.Checked = m_policy.m_statements[0].purpose.telemarketing.present;

			// now set the 'required' comboboxes appropriately
			if ( this.cbAdmin.Checked )
			{
				switch ( m_policy.m_statements[0].purpose.admin.required )
				{
					case P3P.REQUIRED.always:
						this.cmbAdmin.SelectedIndex = 0;//"Always"
						break;

					case P3P.REQUIRED.opt_in:
						this.cmbAdmin.SelectedIndex = 1;//"Opt-In"
						break;

					case P3P.REQUIRED.opt_out:
						this.cmbAdmin.SelectedIndex = 2;//"Opt-Out"
						break;
				}
			}

			if ( this.cbContact.Checked )
			{
				switch ( m_policy.m_statements[0].purpose.contact.required )
				{
					case P3P.REQUIRED.always:
						this.cmbContact.SelectedIndex = 0;//"Always"
						break;

					case P3P.REQUIRED.opt_in:
						this.cmbContact.SelectedIndex = 1;//"Opt-In"
						break;

					case P3P.REQUIRED.opt_out:
						this.cmbContact.SelectedIndex = 2;//"Opt-Out"
						break;
				}
			}

			if ( this.cbDevelop.Checked )
			{
				switch ( m_policy.m_statements[0].purpose.develop.required)
				{
					case P3P.REQUIRED.always:
						this.cmbDevelop.SelectedIndex = 0;//"Always"
						break;

					case P3P.REQUIRED.opt_in:
						this.cmbDevelop.SelectedIndex = 1;//"Opt-In"
						break;

					case P3P.REQUIRED.opt_out:
						this.cmbDevelop.SelectedIndex = 2;//"Opt-Out"
						break;
				}
			}

			if ( this.cbHistorical.Checked )
			{
				switch ( m_policy.m_statements[0].purpose.historical.required )
				{
					case P3P.REQUIRED.always:
						this.cmbHistorical.SelectedIndex = 0;//"Always"
						break;

					case P3P.REQUIRED.opt_in:
						this.cmbHistorical.SelectedIndex = 1;//"Opt-In"
						break;

					case P3P.REQUIRED.opt_out:
						this.cmbHistorical.SelectedIndex = 2;//"Opt-Out"
						break;
				}
			}

			if ( this.cbIndividual_Analysis.Checked )
			{
				switch ( m_policy.m_statements[0].purpose.individual_analysis.required )
				{
					case P3P.REQUIRED.always:
						this.cmbIndividual_Analysis.SelectedIndex = 0;//"Always"
						break;

					case P3P.REQUIRED.opt_in:
						this.cmbIndividual_Analysis.SelectedIndex = 1;//"Opt-In"
						break;

					case P3P.REQUIRED.opt_out:
						this.cmbIndividual_Analysis.SelectedIndex = 2;//"Opt-Out"
						break;
				}
			}

			if ( this.cbIndividual_Decision.Checked )
			{
				switch ( m_policy.m_statements[0].purpose.individual_decision.required )
				{
					case P3P.REQUIRED.always:
						this.cmbIndividual_Decision.SelectedIndex = 0;//"Always"
						break;

					case P3P.REQUIRED.opt_in:
						this.cmbIndividual_Decision.SelectedIndex = 1;//"Opt-In"
						break;

					case P3P.REQUIRED.opt_out:
						this.cmbIndividual_Decision.SelectedIndex = 2;//"Opt-Out"
						break;
				}
			}

			if (this.cbPseudo_Analysis.Checked )
			{
				switch ( m_policy.m_statements[0].purpose.pseudo_analysis.required )
				{
					case P3P.REQUIRED.always:
						this.cmbPseudo_Analysis.SelectedIndex = 0;//"Always"
						break;

					case P3P.REQUIRED.opt_in:
						this.cmbPseudo_Analysis.SelectedIndex = 1;//"Opt-In"
						break;

					case P3P.REQUIRED.opt_out:
						this.cmbPseudo_Analysis.SelectedIndex = 2;//"Opt-Out"
						break;
				}
			}

			if ( this.cbPseudo_Decision.Checked )
			{
				switch ( m_policy.m_statements[0].purpose.pseudo_decision.required )
				{
					case P3P.REQUIRED.always:
						this.cmbPseudo_Decision.SelectedIndex = 0;//"Always"
						break;

					case P3P.REQUIRED.opt_in:
						this.cmbPseudo_Decision.SelectedIndex = 1;//"Opt-In"
						break;

					case P3P.REQUIRED.opt_out:
						this.cmbPseudo_Decision.SelectedIndex = 2;//"Opt-Out"
						break;
				}
			}

			if ( this.cbTailoring.Checked )
			{
				switch ( m_policy.m_statements[0].purpose.tailoring.required )
				{
					case P3P.REQUIRED.always:
						this.cmbTailoring.SelectedIndex = 0;//"Always"
						break;

					case P3P.REQUIRED.opt_in:
						this.cmbTailoring.SelectedIndex = 1;//"Opt-In"
						break;

					case P3P.REQUIRED.opt_out:
						this.cmbTailoring.SelectedIndex = 2;//"Opt-Out"
						break;
				}
			}

			if ( this.cbTelemarketing.Checked )
			{
				switch ( m_policy.m_statements[0].purpose.telemarketing.required )
				{
					case P3P.REQUIRED.always:
						this.cmbTelemarketing.SelectedIndex = 0;//"Always"
						break;

					case P3P.REQUIRED.opt_in:
						this.cmbTelemarketing.SelectedIndex = 1;//"Opt-In"
						break;

					case P3P.REQUIRED.opt_out:
						this.cmbTelemarketing.SelectedIndex = 2;//"Opt-Out"
						break;
				}
			}

			// Recipient Tab
			// everything should be enabled by default

			// make things checked appropriately
			this.cbDelivery.Checked = m_policy.m_statements[0].recipient.delivery.present;
			this.cbOther_Recipient.Checked = m_policy.m_statements[0].recipient.other_recipient.present;
			this.cbPublic.Checked = m_policy.m_statements[0].recipient._public.present;
			this.cbSame.Checked = m_policy.m_statements[0].recipient.same.present;
			this.cbUnrelated.Checked = m_policy.m_statements[0].recipient.unrelated.present;
			this.cbOurs.Checked = m_policy.m_statements[0].recipient.ours;

			// now do the 'required' combobox appropriately
			if ( this.cbDelivery.Checked )
			{
				switch (m_policy.m_statements[0].recipient.delivery.required)
				{
					case P3P.REQUIRED.always:
						this.cmbDelivery.SelectedIndex = 0;//"Always"
						break;

					case P3P.REQUIRED.opt_in:
						this.cmbDelivery.SelectedIndex = 1;//"Opt-In"
						break;

					case P3P.REQUIRED.opt_out:
						this.cmbDelivery.SelectedIndex = 2;//"Opt-Out"
						break;
				}
			}

			if ( this.cbOther_Recipient.Checked )
			{
				switch (m_policy.m_statements[0].recipient.other_recipient.required)
				{
					case P3P.REQUIRED.always:
						this.cmbOther_Recipient.SelectedIndex = 0;//"Always"
						break;

					case P3P.REQUIRED.opt_in:
						this.cmbOther_Recipient.SelectedIndex = 1;//"Opt-In"
						break;

					case P3P.REQUIRED.opt_out:
						this.cmbOther_Recipient.SelectedIndex = 2;//"Opt-Out"
						break;
				}
			}

			if ( this.cbPublic.Checked )
			{
				switch (m_policy.m_statements[0].recipient._public.required)
				{
					case P3P.REQUIRED.always:
						this.cmbPublic.SelectedIndex = 0;//"Always"
						break;

					case P3P.REQUIRED.opt_in:
						this.cmbPublic.SelectedIndex = 1;//"Opt-In"
						break;

					case P3P.REQUIRED.opt_out:
						this.cmbPublic.SelectedIndex = 2;//"Opt-Out"
						break;
				}
			}

			if ( this.cbSame.Checked )
			{
				switch (m_policy.m_statements[0].recipient.same.required)
				{
					case P3P.REQUIRED.always:
						this.cmbSame.SelectedIndex = 0;//"Always"
						break;

					case P3P.REQUIRED.opt_in:
						this.cmbSame.SelectedIndex = 1;//"Opt-In"
						break;

					case P3P.REQUIRED.opt_out:
						this.cmbSame.SelectedIndex = 2;//"Opt-Out"
						break;
				}
			}

			if (this.cbUnrelated.Checked )
			{
				switch (m_policy.m_statements[0].recipient.unrelated.required)
				{
					case P3P.REQUIRED.always:
						this.cmbUnrelated.SelectedIndex = 0;//"Always"
						break;

					case P3P.REQUIRED.opt_in:
						this.cmbUnrelated.SelectedIndex = 1;//"Opt-In"
						break;

					case P3P.REQUIRED.opt_out:
						this.cmbUnrelated.SelectedIndex = 2;//"Opt-Out"
						break;
				}
			}

			// Retention Tab
			// enable everything
			this.cbBusiness_Practices.Enabled = true;
			this.cbIndefinitely.Enabled = true;
			this.cbLegal_Requirement.Enabled = true;
			this.cbNo_Retention.Enabled = true;
			this.cbStated_Purpose.Enabled = true;

			// now put in the check boxes accordingly
			// always put the none in last as it overrides the others
			this.cbBusiness_Practices.Checked = m_policy.m_statements[0].retention.business_practices;
			this.cbIndefinitely.Checked = m_policy.m_statements[0].retention.indefinitely;
			this.cbLegal_Requirement.Checked = m_policy.m_statements[0].retention.legal_requirement;
			this.cbStated_Purpose.Checked = m_policy.m_statements[0].retention.stated_purpose;
			this.cbNo_Retention.Checked = m_policy.m_statements[0].retention.no_retention;

			// Misc Tab
			// everything should be enabled by default

			// fill in the checkbox and textbox accordingly
			this.cbNonidentifiable.Checked = m_policy.m_statements[0].nonidentifiable;
			this.tbConsequence.Text = m_policy.m_statements[0].consequence;

			// now everything is happy
			return;
		}

		/// <summary>
		/// Takes the policy as described in the property pages and saves it to our member
		/// variable policy.
		/// </summary>
		private void SavePolicy()
		{
			// save the policy setting in the window to the window's memeber P3P Policy object

			// clear out the current policy
			m_policy = new P3P.POLICY();
			m_policy.m_statements[0] = new P3P.STATEMENT();

			// Access Tab
			// check if None as has been selected
			if ( this.cbNone.Checked )
			{
				// put that one in the policy and ignore the rest
				m_policy.access.none = true;
			}
				// otherwise check the others for their status
			else
			{
				m_policy.access.all = this.cbAll.Checked;
				m_policy.access.contact_and_other = this.cbContact_and_Other.Checked;
				m_policy.access.ident_contact = this.cbIdent_Contact.Checked;
				m_policy.access.nonident = this.cbNonident.Checked;
				m_policy.access.other_ident = this.cbOther_Ident.Checked;

			}

			// Categories Tab
			// check which ones have been checked and put them in the policy accordingly
			m_policy.m_statements[0].categories.computer = this.cbComputer.Checked;
			m_policy.m_statements[0].categories.content = this.cbContent.Checked;
			m_policy.m_statements[0].categories.demographic = this.cbDemographic.Checked;
			m_policy.m_statements[0].categories.financial = this.cbFinancial.Checked;
			m_policy.m_statements[0].categories.government = this.cbGovernment.Checked;
			m_policy.m_statements[0].categories.health = this.cbHealth.Checked;
			m_policy.m_statements[0].categories.interactive = this.cbInteractive.Checked;
			m_policy.m_statements[0].categories.location = this.cbLocation.Checked;
			m_policy.m_statements[0].categories.navigation = this.cbNavigation.Checked;
			m_policy.m_statements[0].categories.online = this.cbOnline.Checked;
			m_policy.m_statements[0].categories.other_category = this.tbOther_Category.Text;
			m_policy.m_statements[0].categories.physical = this.cbPhysical.Checked;
			m_policy.m_statements[0].categories.political = this.cbPolitical.Checked;
			m_policy.m_statements[0].categories.preference = this.cbPreference.Checked;
			m_policy.m_statements[0].categories.purchase = this.cbPurchase.Checked;
			m_policy.m_statements[0].categories.state = this.cbState.Checked;
			m_policy.m_statements[0].categories.uniqueid = this.cbUniqueID.Checked;

			// Disputes and Remedies Tab
			// for each disputes item in the list, we must add it to the policy
			P3P.DISPUTES disp;
			int i = 0;
			foreach ( Object o in this.lbDisputes.Items )
			{
				// cast it to a disputes
				disp = o as P3P.DISPUTES;

				// now save it in the policy
				m_policy.m_disputes[i] = disp;
			}

			// Purpose Tab
			// check each checkbox for status and store it along with the required field drop
			// down box
			m_policy.m_statements[0].purpose.admin.present = this.cbAdmin.Checked;
			switch ( this.cmbAdmin.SelectedIndex )
			{
				case 0:
					m_policy.m_statements[0].purpose.admin.required = P3P.REQUIRED.always;
					break;

				case 1:
					m_policy.m_statements[0].purpose.admin.required = P3P.REQUIRED.opt_in;
					break;

				case 2:
					m_policy.m_statements[0].purpose.admin.required = P3P.REQUIRED.opt_out;
					break;
			}

			m_policy.m_statements[0].purpose.contact.present = this.cbContact.Checked;
			switch ( this.cmbContact.SelectedIndex )
			{
				case 0:
					m_policy.m_statements[0].purpose.contact.required = P3P.REQUIRED.always;
					break;

				case 1:
					m_policy.m_statements[0].purpose.contact.required = P3P.REQUIRED.opt_in;
					break;

				case 2:
					m_policy.m_statements[0].purpose.contact.required = P3P.REQUIRED.opt_out;
					break;
			}

			m_policy.m_statements[0].purpose.current = this.cbCurrent.Checked;

			m_policy.m_statements[0].purpose.develop.present = this.cbDevelop.Checked;
			switch ( this.cmbDevelop.SelectedIndex )
			{
				case 0:
					m_policy.m_statements[0].purpose.develop.required = P3P.REQUIRED.always;
					break;

				case 1:
					m_policy.m_statements[0].purpose.develop.required = P3P.REQUIRED.opt_in;
					break;

				case 2:
					m_policy.m_statements[0].purpose.develop.required = P3P.REQUIRED.opt_out;
					break;
			}

			m_policy.m_statements[0].purpose.historical.present = this.cbHistorical.Checked;
			switch ( this.cmbHistorical.SelectedIndex )
			{
				case 0:
					m_policy.m_statements[0].purpose.historical.required = P3P.REQUIRED.always;
					break;

				case 1:
					m_policy.m_statements[0].purpose.historical.required = P3P.REQUIRED.opt_in;
					break;

				case 2:
					m_policy.m_statements[0].purpose.historical.required = P3P.REQUIRED.opt_out;
					break;
			}

			m_policy.m_statements[0].purpose.individual_analysis.present = this.cbIndividual_Analysis.Checked;
			switch ( this.cmbIndividual_Analysis.SelectedIndex )
			{
				case 0:
					m_policy.m_statements[0].purpose.individual_analysis.required = P3P.REQUIRED.always;
					break;

				case 1:
					m_policy.m_statements[0].purpose.individual_analysis.required = P3P.REQUIRED.opt_in;
					break;

				case 2:
					m_policy.m_statements[0].purpose.individual_analysis.required = P3P.REQUIRED.opt_out;
					break;
			}

			m_policy.m_statements[0].purpose.individual_decision.present = this.cbIndividual_Decision.Checked;
			switch (this.cmbIndividual_Decision.SelectedIndex )
			{
				case 0:
					m_policy.m_statements[0].purpose.individual_decision.required = P3P.REQUIRED.always;
					break;

				case 1:
					m_policy.m_statements[0].purpose.individual_decision.required = P3P.REQUIRED.opt_in;
					break;

				case 2:
					m_policy.m_statements[0].purpose.individual_decision.required = P3P.REQUIRED.opt_out;
					break;
			}

			// for other purpose, fill it in
			if ( this.cbOther_Purpose.Checked )
			{
				m_policy.m_statements[0].purpose.other_purpose = this.tbOther_Purpose.Text;
				switch (this.cmbOther_Purpose.SelectedIndex )
				{
					case 0:
						m_policy.m_statements[0].purpose.other_purpose_required = P3P.REQUIRED.always;
						break;

					case 1:
						m_policy.m_statements[0].purpose.other_purpose_required = P3P.REQUIRED.opt_in;
						break;

					case 2:
						m_policy.m_statements[0].purpose.other_purpose_required = P3P.REQUIRED.opt_out;
						break;
				}
			}
			else
			{
				m_policy.m_statements[0].purpose.other_purpose = "";
			}

			m_policy.m_statements[0].purpose.pseudo_analysis.present = this.cbPseudo_Analysis.Checked;
			switch (this.cmbPseudo_Analysis.SelectedIndex )
			{
				case 0:
					m_policy.m_statements[0].purpose.pseudo_analysis.required = P3P.REQUIRED.always;
					break;

				case 1:
					m_policy.m_statements[0].purpose.pseudo_analysis.required = P3P.REQUIRED.opt_in;
					break;

				case 2:
					m_policy.m_statements[0].purpose.pseudo_analysis.required = P3P.REQUIRED.opt_out;
					break;
			}

			m_policy.m_statements[0].purpose.pseudo_decision.present = this.cbPseudo_Decision.Checked;
			switch (this.cmbPseudo_Decision.SelectedIndex )
			{
				case 0:
					m_policy.m_statements[0].purpose.pseudo_decision.required = P3P.REQUIRED.always;
					break;

				case 1:
					m_policy.m_statements[0].purpose.pseudo_decision.required = P3P.REQUIRED.opt_in;
					break;

				case 2:
					m_policy.m_statements[0].purpose.pseudo_decision.required = P3P.REQUIRED.opt_out;
					break;
			}

			m_policy.m_statements[0].purpose.tailoring.present = this.cbTailoring.Checked;
			switch (this.cmbTailoring.SelectedIndex )
			{
				case 0:
					m_policy.m_statements[0].purpose.tailoring.required = P3P.REQUIRED.always;
					break;

				case 1:
					m_policy.m_statements[0].purpose.tailoring.required = P3P.REQUIRED.opt_in;
					break;

				case 2:
					m_policy.m_statements[0].purpose.tailoring.required = P3P.REQUIRED.opt_out;
					break;
			}

			m_policy.m_statements[0].purpose.telemarketing.present = this.cbTelemarketing.Checked;
			switch (this.cmbTelemarketing.SelectedIndex )
			{
				case 0:
					m_policy.m_statements[0].purpose.telemarketing.required = P3P.REQUIRED.always;
					break;

				case 1:
					m_policy.m_statements[0].purpose.telemarketing.required = P3P.REQUIRED.opt_in;
					break;

				case 2:
					m_policy.m_statements[0].purpose.telemarketing.required = P3P.REQUIRED.opt_out;
					break;
			}

			// Recipient Tab
			// exaine the checkboxes and required comboboxes and set the policy accordingly
			m_policy.m_statements[0].recipient.delivery.present = this.cbDelivery.Checked;
			switch (this.cmbDelivery.SelectedIndex )
			{
				case 0:
					m_policy.m_statements[0].recipient.delivery.required = P3P.REQUIRED.always;
					break;

				case 1:
					m_policy.m_statements[0].recipient.delivery.required = P3P.REQUIRED.opt_in;
					break;

				case 2:
					m_policy.m_statements[0].recipient.delivery.required = P3P.REQUIRED.opt_out;
					break;
			}

			m_policy.m_statements[0].recipient.other_recipient.present = this.cbOther_Recipient.Checked;
			switch (this.cmbOther_Recipient.SelectedIndex )
			{
				case 0:
					m_policy.m_statements[0].recipient.other_recipient.required = P3P.REQUIRED.always;
					break;

				case 1:
					m_policy.m_statements[0].recipient.other_recipient.required = P3P.REQUIRED.opt_in;
					break;

				case 2:
					m_policy.m_statements[0].recipient.other_recipient.required = P3P.REQUIRED.opt_out;
					break;
			}

			m_policy.m_statements[0].recipient.ours = this.cbOurs.Checked;

			m_policy.m_statements[0].recipient.same.present = this.cbSame.Checked;
			switch (this.cmbSame.SelectedIndex )
			{
				case 0:
					m_policy.m_statements[0].recipient.same.required = P3P.REQUIRED.always;
					break;

				case 1:
					m_policy.m_statements[0].recipient.same.required = P3P.REQUIRED.opt_in;
					break;

				case 2:
					m_policy.m_statements[0].recipient.same.required = P3P.REQUIRED.opt_out;
					break;
			}

			m_policy.m_statements[0].recipient.unrelated.present = this.cbUnrelated.Checked;
			switch (this.cmbUnrelated.SelectedIndex )
			{
				case 0:
					m_policy.m_statements[0].recipient.unrelated.required = P3P.REQUIRED.always;
					break;

				case 1:
					m_policy.m_statements[0].recipient.unrelated.required = P3P.REQUIRED.opt_in;
					break;

				case 2:
					m_policy.m_statements[0].recipient.unrelated.required = P3P.REQUIRED.opt_out;
					break;
			}

			// Retention Tab
			// see if the None box is checked
			if ( this.cbNo_Retention.Checked )
			{
				// then set it and nothing else
				m_policy.m_statements[0].retention.no_retention = true;
			}
				// otherwise we check the rest of them
			else
			{
				m_policy.m_statements[0].retention.no_retention = false;
				m_policy.m_statements[0].retention.business_practices = this.cbBusiness_Practices.Checked;
				m_policy.m_statements[0].retention.indefinitely = this.cbIndefinitely.Checked;
				m_policy.m_statements[0].retention.legal_requirement = this.cbLegal_Requirement.Checked;
				m_policy.m_statements[0].retention.stated_purpose = this.cbStated_Purpose.Checked;
			}

			// Misc Tab
			// save the consequence and unidentifiable boxes
			m_policy.m_statements[0].nonidentifiable = this.cbNonidentifiable.Checked;
			m_policy.m_statements[0].consequence = this.tbConsequence.Text;
		}
		#endregion

		#region Button Handlers
		private void bReset_Click(object sender, System.EventArgs e)
		{
			DialogResult result;

			// make sure that the user wants to clear out all changes
			result = MessageBox.Show(this, "Clear all changes to P3P policy?\nWarning: All changes will be lost permanently!", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

			// if it's a yes, clear things out
			if ( result == DialogResult.Yes )
			{
				// reload from the stored policy
				this.LoadPolicy();
			}
		}

		private void bCancel_Click(object sender, System.EventArgs e)
		{
			DialogResult result;

			// make sure that the user wants to do this
			result = MessageBox.Show(this, "Leave without saving changes?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

			// if yes, then close out of the window
			if ( result == DialogResult.Yes )
			{
				this.Close();
			}
		}

		private void bSave_Click(object sender, System.EventArgs e)
		{
			// save the changes and then close the window
			SavePolicy();

			this.Close();
		}
		#endregion

		private void P3PCreator_Load(object sender, System.EventArgs e)
		{
			// load up the property pages from the stored policy
			this.LoadPolicy();
		}
	}
}
