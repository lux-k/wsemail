using System;
using System.Xml.Serialization;
using DerivedA;
using DerivedB;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Web.Services;


namespace TestObjects
{
	/// <summary>
	/// Summary description for Service1.
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

		[WebMethod]
		[XmlInclude(typeof(A))]
		public XmlDocument GiveObject()
		{
			B b = new B();
			
			A a = new A();
			a.hehe = "blarg";
			return a.AsXmlDocument();
			return b.AsXmlDocument();
			return a.AsXmlDocument();

		}
	}
}
