using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

namespace StoreService
{
	/// <summary>
	/// Takes a latitude and longitude location and returns a list of services in the vicinity
	/// </summary>
	[System.Web.Services.WebService(
	Namespace="http://158.130.67.0/StoreService/",
	Description="A store locator service")]
	public class StoreServer : System.Web.Services.WebService
	{
		public StoreServer()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}

		/// <summary>
		///Takes a numerical representation of a latitude and longitude and returns a set of strings that are
		/// services nearby the location 
		/// </summary>
		/// <param name="loc">The location that we are checking in from</param>
		/// <returns>An array of Service items</returns>
		[WebMethod(Description="This method returns a list of stores in the area near a certain location")]
		public StoreService.CServiceItem[] getServices(StoreService.CLoc loc)
		{
			StoreService.CServiceItem[] stores;
			stores = new StoreService.CServiceItem[2];

			// we have to access some database of existing strings
			// until we have the database and logic implemented, let's just make some dummy code
			// if we are within ten minutes plus or minus of Moore:
			//39:57:9:N
			//-75:11:24:W
			if ( loc.Latitude.Dir == Direction.NORTH
				&& loc.Latitude.Degrees == 39
				&& loc.Latitude.Minutes <= 67 && loc.Latitude.Minutes >= 47
				&& loc.Longitude.Dir == Direction.WEST
				&& loc.Longitude.Degrees == -75
				&& loc.Longitude.Minutes <= 21 && loc.Longitude.Minutes >= 1)
			{
				// return an array of two stores near us
				stores[0] = new CServiceItem();
				stores[0].Name = "Wawa";
				stores[0].Address = "3800 Spruce St";
				stores[0].City = "Philadelphia";
				stores[0].State = "PA";
				stores[0].Phone = "215-123-4567";
				stores[0].Comments = "Convenience store";

				stores[1] = new CServiceItem();
				stores[1].Name = "Chabad";
				stores[1].Address = "4032 Spruce";
				stores[1].City = "Philadelphia";
				stores[1].State = "PA";
				stores[1].Phone = "215-899-8888";
				stores[1].Comments = "Shul";
			}
			else
			{
				stores[0] = new CServiceItem();
				stores[0].Name = "The White House";
				stores[0].Address = "1600 Pennsylvania Ave";
				stores[0].City = "Washington";
				stores[0].State = "DC";
				stores[0].Phone = "800-123-4567";
				stores[0].Comments = "Government center";

				stores[1] = new CServiceItem();
				stores[1].Name = "Hotel California";
				stores[1].Address = "1200 Eagles Way";
				stores[1].City = "Los Angeles";
				stores[1].State = "CA";
				stores[1].Phone = "900-123-4567";
				stores[1].Comments = "Imaginary location";
			}

			return stores;
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
