using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

namespace GLS
{
	/// <summary>
	/// A simple webservice setup to run on a wireless gateway machine.  The web services
	/// included return hard-wired results - the GIS server nearby and the name of the
	/// physical location of the device.
	/// </summary>
	[System.Web.Services.WebService(
	Namespace="http://158.130.67.0/GLS/",
	Description="A Locator Service.  Returns GIS and location information to queries.")]
	public class GLS : System.Web.Services.WebService
	{
		public GLS()
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
		/// A web method invokable from a mobile device to discover the GIS server(s) in
		/// the local area.  Each gateway should run a customized copy of this webservice.
		/// </summary>
		/// <returns>A string with the URL of the local GIS server</returns>
		[WebMethod]
		public string GetGIS()
		{
			// just return the URL for the GIS
			return "158.130.67.0";
		}

		/// <summary>
		/// A web method invokable from a mobile device to discover the name of the physical
		/// location that the web service server is locateed.  Each gateway should run a 
		/// customized copy of this webservice.
		/// </summary>
		/// <returns>A string with the common name of the physical location of the device.</returns>
		[WebMethod]
		public string GetLoc()
		{
			// return a string with the physical location of the user
			// assume a granularity of "city" at this point
			// but we may get finer grained in the future
			return "Philadelphia, PA";
		}
	}
}
