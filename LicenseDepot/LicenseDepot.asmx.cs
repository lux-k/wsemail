using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Xml;

namespace LicenseDepot
{
	/// <summary>
	/// LicenseDepot acts as a storage space for licenses from a User Agent that are waiting
	/// to be picked up by a Merchant.
	/// </summary>
	[System.Web.Services.WebService(Namespace="http://localhost/LicenseDepot/",
		 Description="Web service that acts as a depot for licenses between a User Agent and a Merchant")]	 
	public class LicenseDepot : System.Web.Services.WebService
	{
		public LicenseDepot()
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

		/// <summary>
		/// This Web Method just takes an XrML license from an outside place and puts it
		/// in a secure location for use by someone else 
		/// </summary>
		/// <param name="lic">The XrML license to be taken</param>
		/// <returns>true if license is accepted.  false otherwise.</returns>
		[WebMethod(Description="Accepts licenses and makes them available for later retrival")]
		public bool Deposit( System.Xml.XmlDocument lic )
		{
			if (lic == null )
				return false;

			// put this in a file somewhere based on the time
			XmlTextWriter writer = new XmlTextWriter(System.DateTime.Now.ToString() + "-license.xml", System.Text.Encoding.ASCII);
			writer.Formatting = Formatting.Indented;
			lic.WriteContentTo( writer );
			writer.Flush();
			writer.WriteWhitespace("\n");

			return true;
		}

	}
}
