using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Xml;
using System.Web;
using System.Web.Services;
using Microsoft.Web.Services;
using Microsoft.Web.Services.Security;
using Microsoft.Web.Services.Security.X509;

namespace UserAgent
{
	/// <summary>
	/// Possible Authorization policies for the Merchants contacting the User Agent:
	/// Full_Auto: The Agent accepts or rejects automatically based on the P3P policy
	/// Full_Manual: The Agent never accepts or rejects automatically.  It always stores the 
	/// offer and feeds it to the User when next able to.
	/// Auto_If_OK: Automatically approves if the policy is acceptable.  Stores the offer for
	/// human review if it's not acceptable.
	/// </summary>
	public enum Authorization {Full_Auto, Full_Manual, Auto_If_OK};

	/// <summary>
	/// Status of an offer that has been made.  Pending offers are ones that are awaiting
	/// a decision from the user.  We keep the old status for logging purposes.
	/// </summary>
	public enum OfferStatus {Approve, Reject, Pending};

	/// <summary>
	/// Summary description for Service1.
	/// </summary>
	public class UserAgent : System.Web.Services.WebService
	{
		public UserAgent()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		#region Variables
		/// <summary>
		/// The P3P policy for this user
		/// </summary>
		protected P3P.POLICY m_policy;

		/// <summary>
		/// The Merchant authorization mode as set by the user
		/// </summary>
		protected Authorization m_auth;
		/// <summary>
		/// Accept and deny lists
		/// </summary>
		protected System.Collections.SortedList m_accept;
		protected System.Collections.SortedList m_deny;

		/// <summary>
		/// List of pending offers for manual approval or denial
		/// </summary>
		protected System.Collections.SortedList m_offers;

		#endregion

		#region Policy File Manipulation
		/// <summary>
		/// Sets the currently stored P3P user policy
		/// </summary>
		/// <param name="p">P3P Policy object</param>
		/// <returns>true if successful. false otherwise</returns>
		[WebMethod]
		public bool UploadPolicy(P3P.POLICY p)
		{
			// we add this policy as the new user P3P policy
			this.m_policy = p;
			return true;
		}


		/// <summary>
		/// Fetches the currently stored user P3P policy
		/// </summary>
		/// <returns>The P3P Policy object</returns>
		[WebMethod]
		public P3P.POLICY FetchPolicy()
		{
			return this.m_policy;
		}


		#endregion

		#region Authorization Mode
		/// <summary>
		/// Set the Authorization Mode for Merchants
		/// </summary>
		/// <param name="a">Authorization mode enum</param>
		/// <returns>true if successful.  false otherwise</returns>
		[WebMethod]
		public bool SetAuthorizationMode(Authorization a)
		{
			this.m_auth = a;
			return true;
		}

		/// <summary>
		/// Get the Authorization Mode for Merchants
		/// </summary>
		/// <returns>Authorization enum</returns>
		[WebMethod]
		public Authorization GetAuthorizationMode()
		{
			return this.m_auth;
		}

		#endregion

		#region Accept/Deny List Manipulation
		/// <summary>
		/// Add this company name to the always accept list
		/// </summary>
		/// <param name="name">X.509 fully qualified name string</param>
		/// <returns>true if successful.  false otherwise</returns>
		[WebMethod]
		public bool AcceptAdd(string name)
		{
			bool success = true;

			// name of company to always accept from
			this.m_accept.Add(name, null);
			
			// make sure it's really inside
			if (this.m_accept.ContainsKey(name))
			{
				success = true;
				this.m_deny.Remove(name);
			}
			else
			{
				success = false;
			}

			return success;
		}

		/// <summary>
		/// Remove this company name from the always accept list
		/// </summary>
		/// <param name="name">X.509 fully qualified name string</param>
		/// <returns>true if successful.  false otherwise</returns>
		[WebMethod]
		public bool AcceptRemove(string name)
		{
			bool success = true;

			// name of company to always accept from
			this.m_accept.Remove(name);
			
			// make sure it's really gone
			if (this.m_accept.ContainsKey(name))
			{
				success = false;
			}
			else
			{
				success = true;;
			}

			return success;
		}

		/// <summary>
		/// Add this company name to the always deny list
		/// </summary>
		/// <param name="name">X.509 fully qualified name string</param>
		/// <returns>true if successful.  false otherwise</returns>
		[WebMethod]
		public bool DenyAdd(string name)
		{
			bool success = true;

			// name of company to always accept from
			this.m_deny.Add(name, null);
			
			// make sure it's really inside
			if (this.m_deny.ContainsKey(name))
			{
				success = true;
				this.m_accept.Remove(name);
			}
			else
			{
				success = false;
			}

			return success;
		}
		/// <summary>
		/// Remove this company name from the always deny list
		/// </summary>
		/// <param name="name">X.509 fully qualified name string</param>
		/// <returns>true if successful.  false otherwise</returns>
		[WebMethod]
		public bool DenyRemove(string name)
		{
			bool success = true;

			// name of company to always accept from
			this.m_deny.Remove(name);
			
			// make sure it's really gone
			if (this.m_deny.ContainsKey(name))
			{
				success = false;
			}
			else
			{
				success = true;;
			}

			return success;
		}

		#endregion

		#region Offer List Manipulation
		/// <summary>
		/// Fetches the pending offers for the user.
		/// </summary>
		/// <returns>Key pair list of pending offers</returns>
		[WebMethod]
		public System.Collections.SortedList GetPendingOffers()
		{
			System.Collections.SortedList m_pending = new SortedList();;
			Offer off;

			foreach (System.Object obj in this.m_offers.Keys)
			{
				// cast it to an offer
				off = obj as Offer;

				m_pending.Add(obj, OfferStatus.Pending);
			}

			return m_pending;
					
			// put in something here that will send back pending offer information
			// make sure that the user can subsequently respond with an approve/deny, perhaps
			// by some sort of indexing that won't be harmed by subsequent offers
		}

		/// <summary>
		/// Takes back a list of Offer/OfferStatus pairs that will modify the existing offer list.
		/// All offers in the returned list must correspond to existing ones in the list.  If not
		/// an error is returned and the status is undefined.  (Change this?)
		/// </summary>
		/// <param name="sl">The returned offers</param>
		/// <returns>true if fully successful.  false otherwise.</returns>
		[WebMethod]
		public bool DecideOffers(System.Collections.SortedList sl)
		{
			Offer off;
			OfferStatus stat;
			//Object v;

			foreach ( Object k in sl.Keys )
			{
				// cast the objects as offers
				off = k as Offer;
				stat = (OfferStatus) sl[off];
				//stat = v as OfferStatus;

				// if it's actually an offer
				if (off != null && sl[off] != null && m_offers.ContainsKey(off))
				{
					// based on the key/value pair, change its status in the list
					m_offers[off] = stat;

					// now send out an offer approval if it's ok'd
					if ( stat == OfferStatus.Approve )
					{
						this.SendLicense(off);
					}
				}
				else
				{
					// something here is wrong or misformed
					return false;
				}
			}

			// we've done the job, now return true
			return true;
		}

		/// <summary>
		/// Gets all of the offers, approved, denied, and pending.
		/// </summary>
		/// <returns>The list of the offers</returns>
		[WebMethod]
		public System.Collections.SortedList GetOffers()
		{
			// get all of the offers
			return this.m_offers;
		}
		#endregion

		#region License Manipulations
		protected bool SendLicense(Offer off)
		{
			// temporary holder for lists
			System.Xml.XmlNodeList list;

			// prepare a license for sending out with the appropriate information
			System.Xml.XmlDocument lic = new XmlDocument();
			
			// open up the license template file
			System.Xml.XmlTextReader reader = new XmlTextReader("C:\\Documents and Settings\\michael.MIR\\My Documents\\licenseTemplate.xml");
			// ignore all whitespace
			reader.WhitespaceHandling = System.Xml.WhitespaceHandling.None;
			// load up the license template
			lic.Load(reader);

			// decide which rights to add to the license			
			// add them
			// right now just add in a right to send an ad
			// the new right element
			System.Xml.XmlElement right = lic.CreateElement(null, "SendAnyAd", "priv");

			// find the node that is to be replaced
			list = lic.GetElementsByTagName("rights_placeholder", "*");
			// replace the first one with the new right
			list[0].ParentNode.AppendChild(right);
			list[0].ParentNode.RemoveChild(list[0]);
			//lic.ReplaceChild(right, list[0]);			
			
			// plug in the p3p object as XML
			// first translate the policy and then import it to use in our document
			System.Xml.XmlNode n = lic.ImportNode(off.Policy.toXML(), true);
			System.Xml.XmlElement policy = n as XmlElement;
			
			// find the spot in the document where the policy goes
			list = lic.GetElementsByTagName("PrivacyPolicy", "*");
			// and plug it in as a new child
			list[0].AppendChild(policy);

			// include the X.509 Certificate for the merchant that made the offer
			list = lic.GetElementsByTagName("X509Certificate", "*");
			list[0].InnerXml = off.Cert.ToBase64String();

			// include the common name for the company that is making the offer
			// note that this moving up and across the XML hierarchy is completely document
			// specific.  If the license template changes at all, this line must be changed!
			list[0].ParentNode.ParentNode.ParentNode.NextSibling.InnerText = off.Cert.GetName();

			// send out the license object as a document
			LicenseDepot.LicenseDepot depot = new LicenseDepot.LicenseDepot();
			depot.Url = off.Url;
			// now send it
			depot.Deposit(lic.DocumentElement);

			return true;

		}

		protected System.Xml.XmlElement P3P2XML(P3P.POLICY pol)
		{
			// Take the policy object and turn it into an xml element
	 		return pol.toXML();
		}
		#endregion

		// WEB SERVICE EXAMPLE
		// The HelloWorld() example service returns the string Hello World
		// To build, uncomment the following lines then save and build the project
		// To test this web service, press F5

//		[WebMethod]
//		public string HelloWorld()
//		{
//			return "Hello World";
//		}
	}
}
