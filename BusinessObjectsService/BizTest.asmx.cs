/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System.Xml.Serialization;
using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using DynamicBizObjects;


namespace BusinessObjectsService
{
	/// <summary>
	/// Basically used as a testbed, but not really used at all.
	/// </summary>
	public class Service1 : System.Web.Services.WebService
	{
		public Service1()
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

		// WEB SERVICE EXAMPLE
		// The HelloWorld() example service returns the string Hello World
		// To build, uncomment the following lines then save and build the project
		// To test this web service, press F5

//		[WebMethod]
//		public string HelloWorld()
//		{
//			return "Hello World";
//		}

		[WebMethod]
		[XmlInclude(typeof(Timesheet))]
		public string PostObject(BusinessRequest r, BusinessObject b) 
		{
			string s = ""; //b.FormName;
			if (b is Timesheet)
				s += " object is Timesheet. Hours = ";// + ((Timesheet)b).Hours;

/*			else if (b is PurchaseOrder)
				s += " object is PurchaseOrder. Ammount = " + ((PurchaseOrder)b).PurchaseAmount;
*/
			return s;
		}

		[WebMethod]
		public BusinessRequest MakeRequest(BusinessRequest b) 
		{
			b.GenerateApproval(ConfigurationSettings.AppSettings["SigningCertificate"],true);
			return b;
		}

		[WebMethod]
		[XmlInclude(typeof(Timesheet))]
		public BusinessObject GetTimesheet(BusinessObject b) 
		{
			Timesheet t = new Timesheet();
//			t.Hours = 99;
			t.FormName = "Timesheet";
			return t;
		}

	}
}
