using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Merchant
{
	/// <summary>
	/// A form for displaying an offer for approval or disapproval.
	/// </summary>
	public class OfferDisplay : System.Windows.Forms.Form
	{
		/// <summary>
		/// The offer that we are displaying
		/// </summary>
		protected System.Xml.XmlDocument m_offer;

		/// <summary>
		/// The P3P policy that is includedthe offer
		/// </summary>
		protected P3P.POLICY m_policy;

		/// <summary>
		/// The offer that we are displaying
		/// </summary>
		public System.Xml.XmlDocument Offer
		{
			get { return m_offer; }
			set
			{
				m_offer = value;

				// reload
				ReloadData();
			}
		}

		/// <summary>
		/// The privacy policy that we are displaying
		/// </summary>
		public P3P.POLICY Policy
		{
			get { return m_policy; }
			set
			{
				m_policy = value;

				// reload
				ReloadData();
			}
		}

		/// <summary>
		/// XML TreeView window that will display the source in a tree view
		/// </summary>
		protected Merchant.XMLTreeView m_xtv;

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbSource;
		private System.Windows.Forms.GroupBox gRights;
		private System.Windows.Forms.CheckBox cbOffensiveAd;
		private System.Windows.Forms.CheckBox cbDiscount;
		private System.Windows.Forms.CheckBox cbOther;
		private System.Windows.Forms.TextBox tbOther;
		private System.Windows.Forms.GroupBox gPrivacy;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbSummary;
		private System.Windows.Forms.CheckBox cbIdent;
		private System.Windows.Forms.CheckBox cbRetain;
		private System.Windows.Forms.CheckBox cbTransfer;
		private System.Windows.Forms.TextBox tbTransfer;
		private System.Windows.Forms.Button bViewSource;
		private System.Windows.Forms.Button bApprove;
		private System.Windows.Forms.Button bReject;
		private System.Windows.Forms.Button bDiscard;
		private System.Windows.Forms.CheckBox cbNormalAd;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public OfferDisplay()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// initialization
			m_policy = new P3P.POLICY();
			m_offer = new XmlDocument();
			m_xtv = new XMLTreeView();
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
			this.tbSource = new System.Windows.Forms.TextBox();
			this.gRights = new System.Windows.Forms.GroupBox();
			this.tbOther = new System.Windows.Forms.TextBox();
			this.cbOther = new System.Windows.Forms.CheckBox();
			this.cbDiscount = new System.Windows.Forms.CheckBox();
			this.cbOffensiveAd = new System.Windows.Forms.CheckBox();
			this.cbNormalAd = new System.Windows.Forms.CheckBox();
			this.gPrivacy = new System.Windows.Forms.GroupBox();
			this.tbTransfer = new System.Windows.Forms.TextBox();
			this.cbTransfer = new System.Windows.Forms.CheckBox();
			this.cbRetain = new System.Windows.Forms.CheckBox();
			this.cbIdent = new System.Windows.Forms.CheckBox();
			this.tbSummary = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.bViewSource = new System.Windows.Forms.Button();
			this.bApprove = new System.Windows.Forms.Button();
			this.bReject = new System.Windows.Forms.Button();
			this.bDiscard = new System.Windows.Forms.Button();
			this.gRights.SuspendLayout();
			this.gPrivacy.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(192, 16);
			this.label1.TabIndex = 0;
			this.label1.Tag = "";
			this.label1.Text = "An offer for sending mobile ads from:";
			// 
			// tbSource
			// 
			this.tbSource.Location = new System.Drawing.Point(216, 16);
			this.tbSource.Name = "tbSource";
			this.tbSource.ReadOnly = true;
			this.tbSource.Size = new System.Drawing.Size(208, 20);
			this.tbSource.TabIndex = 1;
			this.tbSource.Text = "";
			// 
			// gRights
			// 
			this.gRights.Controls.Add(this.tbOther);
			this.gRights.Controls.Add(this.cbOther);
			this.gRights.Controls.Add(this.cbDiscount);
			this.gRights.Controls.Add(this.cbOffensiveAd);
			this.gRights.Controls.Add(this.cbNormalAd);
			this.gRights.Location = new System.Drawing.Point(8, 72);
			this.gRights.Name = "gRights";
			this.gRights.Size = new System.Drawing.Size(416, 88);
			this.gRights.TabIndex = 2;
			this.gRights.TabStop = false;
			this.gRights.Text = "Requested rights";
			// 
			// tbOther
			// 
			this.tbOther.Location = new System.Drawing.Point(216, 56);
			this.tbOther.Name = "tbOther";
			this.tbOther.ReadOnly = true;
			this.tbOther.TabIndex = 4;
			this.tbOther.Text = "";
			// 
			// cbOther
			// 
			this.cbOther.Enabled = false;
			this.cbOther.Location = new System.Drawing.Point(152, 56);
			this.cbOther.Name = "cbOther";
			this.cbOther.Size = new System.Drawing.Size(56, 16);
			this.cbOther.TabIndex = 3;
			this.cbOther.Text = "Other:";
			// 
			// cbDiscount
			// 
			this.cbDiscount.Enabled = false;
			this.cbDiscount.Location = new System.Drawing.Point(152, 24);
			this.cbDiscount.Name = "cbDiscount";
			this.cbDiscount.Size = new System.Drawing.Size(104, 16);
			this.cbDiscount.TabIndex = 2;
			this.cbDiscount.Text = "Send discounts";
			// 
			// cbOffensiveAd
			// 
			this.cbOffensiveAd.Enabled = false;
			this.cbOffensiveAd.Location = new System.Drawing.Point(16, 56);
			this.cbOffensiveAd.Name = "cbOffensiveAd";
			this.cbOffensiveAd.Size = new System.Drawing.Size(120, 16);
			this.cbOffensiveAd.TabIndex = 1;
			this.cbOffensiveAd.Text = "Send offensive ads";
			// 
			// cbNormalAd
			// 
			this.cbNormalAd.Enabled = false;
			this.cbNormalAd.Location = new System.Drawing.Point(16, 24);
			this.cbNormalAd.Name = "cbNormalAd";
			this.cbNormalAd.Size = new System.Drawing.Size(112, 24);
			this.cbNormalAd.TabIndex = 0;
			this.cbNormalAd.Text = "Send normal ads";
			// 
			// gPrivacy
			// 
			this.gPrivacy.Controls.Add(this.tbTransfer);
			this.gPrivacy.Controls.Add(this.cbTransfer);
			this.gPrivacy.Controls.Add(this.cbRetain);
			this.gPrivacy.Controls.Add(this.cbIdent);
			this.gPrivacy.Controls.Add(this.tbSummary);
			this.gPrivacy.Controls.Add(this.label2);
			this.gPrivacy.Location = new System.Drawing.Point(8, 176);
			this.gPrivacy.Name = "gPrivacy";
			this.gPrivacy.Size = new System.Drawing.Size(416, 152);
			this.gPrivacy.TabIndex = 3;
			this.gPrivacy.TabStop = false;
			this.gPrivacy.Text = "Privacy policy summary";
			// 
			// tbTransfer
			// 
			this.tbTransfer.Location = new System.Drawing.Point(152, 120);
			this.tbTransfer.Name = "tbTransfer";
			this.tbTransfer.ReadOnly = true;
			this.tbTransfer.Size = new System.Drawing.Size(248, 20);
			this.tbTransfer.TabIndex = 5;
			this.tbTransfer.Text = "";
			// 
			// cbTransfer
			// 
			this.cbTransfer.Enabled = false;
			this.cbTransfer.Location = new System.Drawing.Point(16, 120);
			this.cbTransfer.Name = "cbTransfer";
			this.cbTransfer.Size = new System.Drawing.Size(128, 16);
			this.cbTransfer.TabIndex = 4;
			this.cbTransfer.Text = "May transfer data to:";
			// 
			// cbRetain
			// 
			this.cbRetain.Enabled = false;
			this.cbRetain.Location = new System.Drawing.Point(16, 92);
			this.cbRetain.Name = "cbRetain";
			this.cbRetain.Size = new System.Drawing.Size(144, 16);
			this.cbRetain.TabIndex = 3;
			this.cbRetain.Text = "Keeps data indefinitely";
			// 
			// cbIdent
			// 
			this.cbIdent.Enabled = false;
			this.cbIdent.Location = new System.Drawing.Point(16, 64);
			this.cbIdent.Name = "cbIdent";
			this.cbIdent.Size = new System.Drawing.Size(184, 16);
			this.cbIdent.TabIndex = 2;
			this.cbIdent.Text = "Collects identifying information";
			// 
			// tbSummary
			// 
			this.tbSummary.Location = new System.Drawing.Point(72, 24);
			this.tbSummary.Name = "tbSummary";
			this.tbSummary.ReadOnly = true;
			this.tbSummary.Size = new System.Drawing.Size(328, 20);
			this.tbSummary.TabIndex = 1;
			this.tbSummary.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 16);
			this.label2.TabIndex = 0;
			this.label2.Text = "Summary:";
			// 
			// bViewSource
			// 
			this.bViewSource.Location = new System.Drawing.Point(16, 40);
			this.bViewSource.Name = "bViewSource";
			this.bViewSource.Size = new System.Drawing.Size(176, 23);
			this.bViewSource.TabIndex = 4;
			this.bViewSource.Text = "View XML source of the offer";
			this.bViewSource.Click += new System.EventHandler(this.bViewSource_Click);
			// 
			// bApprove
			// 
			this.bApprove.Location = new System.Drawing.Point(120, 344);
			this.bApprove.Name = "bApprove";
			this.bApprove.Size = new System.Drawing.Size(88, 23);
			this.bApprove.TabIndex = 5;
			this.bApprove.Text = "Approve Offer";
			// 
			// bReject
			// 
			this.bReject.Location = new System.Drawing.Point(228, 344);
			this.bReject.Name = "bReject";
			this.bReject.Size = new System.Drawing.Size(88, 23);
			this.bReject.TabIndex = 6;
			this.bReject.Text = "Reject Offer";
			// 
			// bDiscard
			// 
			this.bDiscard.Location = new System.Drawing.Point(336, 344);
			this.bDiscard.Name = "bDiscard";
			this.bDiscard.Size = new System.Drawing.Size(88, 23);
			this.bDiscard.TabIndex = 7;
			this.bDiscard.Text = "Discard";
			// 
			// OfferDisplay
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(440, 381);
			this.Controls.Add(this.bDiscard);
			this.Controls.Add(this.bReject);
			this.Controls.Add(this.bApprove);
			this.Controls.Add(this.bViewSource);
			this.Controls.Add(this.gPrivacy);
			this.Controls.Add(this.gRights);
			this.Controls.Add(this.tbSource);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "OfferDisplay";
			this.Text = "Offer";
			this.Load += new System.EventHandler(this.OfferDisplay_Load);
			this.gRights.ResumeLayout(false);
			this.gPrivacy.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Reload the stored data to be shown in the window
		/// </summary>
		private void ReloadData()
		{
			// we must take the new license document and redisplay it

			// check it for the name of the offerer
			System.Xml.XmlNodeList list = m_offer.GetElementsByTagName("x509SubjectName", "*");
			this.tbSource.Text = list[0].InnerXml;

			// now check for requested rights
			// normal ad
			list = m_offer.GetElementsByTagName("sendnormalad", "*");
			if ( list.Count > 0)
			{
				// check the box for normal ads
				this.cbNormalAd.Checked = true;
			}
			else
			{
				this.cbNormalAd.Checked = false;
			}

			// offensive ad
			list = m_offer.GetElementsByTagName("sendoffensivead", "*");
			if ( list.Count > 0 )
			{
				// check the box for offensive ads
				this.cbOffensiveAd.Checked = true;
			}
			else
			{
				this.cbNormalAd.Checked = false;
			}

			// any discount
			list = m_offer.GetElementsByTagName("sendanydiscount", "*");
			if ( list.Count > 0 )
			{
				// check the box for discounts
				this.cbOffensiveAd.Checked = true;
			}
			else
			{
				this.cbOffensiveAd.Checked = false;
			}

			// something else?

			// first clear
			this.tbOther.Text = "";
			this.cbOther.Checked = false;

			// now search
			XmlElement c;
			bool found = false;

			list = m_offer.GetElementsByTagName("grant", "*");
			System.Xml.XmlElement g = list[0] as XmlElement;
			// for every subelement of "grant", the one that houses the
			// rights stuff
			foreach (System.Xml.XmlNode n in g.ChildNodes )
			{
				// if it's valid and it's the first one
				if ( n != null && found == false)
				{
					// cast it to an element
					c = n as XmlElement;

					// see if it's not something that we recognize
					if ( c.Name != "PrivacyPolicy" &&
						c.Name != "mobile" &&
						c.Name != "sendanydiscount" &&
						c.Name != "sendoffensivead" &&
						c.Name != "sendnormalad" )
					{
						// put it in the other checkbox
						this.cbOther.Checked = true;
						this.tbOther.Text = c.Name;
					}
				}
			}

			// now load up the rights stuff
			// summary == consequence
			this.tbSummary.Text = m_policy.m_statements[0].consequence;

			// identifiable information?
			if ( m_policy.m_statements[0].nonidentifiable &&
				m_policy.access.nonident )
			{
				// set the non-identifiable flag
				this.cbIdent.Checked = false;
			}
			else
			{
				this.cbIdent.Checked = true;
			}

			// retention forever?
			this.cbRetain.Checked = m_policy.m_statements[0].retention.indefinitely;

			// gives it away to?
			this.cbOther.Checked = false;
			this.tbOther.Text = "";

			// other people randomly?
			if ( m_policy.m_statements[0].recipient.other_recipient.present )
			{
				this.cbOther.Checked = true;

				this.tbOther.Text += "Other recipients -- ";
			}

			// unrelated recipients
			if ( m_policy.m_statements[0].recipient.unrelated.present)
			{
				this.cbOther.Checked = true;
				this.tbOther.Text += "Others with unrelated privacy policies -- ";
			}

			// public domain
			if ( m_policy.m_statements[0].recipient._public.present )
			{
				this.cbOther.Checked = true;
				this.tbOther.Text += "Released to the public domain -- ";
			}

			// delivery services
			if ( m_policy.m_statements[0].recipient.delivery.present )
			{
				this.cbOther.Checked = true;
				this.tbOther.Text += "Delivery services -- ";
			}

			// others with the same privacy policies
			if ( m_policy.m_statements[0].recipient.same.present )
			{
				this.cbOther.Checked = true;
				this.tbOther.Text += "Others with comparable privacy policies -- ";
			}

		}

		private void OfferDisplay_Load(object sender, System.EventArgs e)
		{
			// reload the data
			this.ReloadData();
		}

		private void bViewSource_Click(object sender, System.EventArgs e)
		{
			// show a window with just the XML source in it
			m_xtv.Doc = this.m_offer;
			m_xtv.DocName = "New Offer";

			// show it
			m_xtv.ShowDialog(this);
		}

			
	}
}
