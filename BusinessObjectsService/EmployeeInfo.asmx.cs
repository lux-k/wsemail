/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using PennLibraries;
using DynamicBizObjects;
using Microsoft.Web.Services2;
using Microsoft.Web.Services2.Security.Tokens;

namespace BusinessObjectsService
{
	/// <summary>
	/// Summary description for EmployeeInfo.
	/// </summary>
	/// 
	[WebService(Namespace="http://securitylab.cis.upenn.edu/AuthorizedRouting")]
	public class EmployeeInfo : System.Web.Services.WebService
	{

		Hashtable RoleTable = null;

		private string GetRoleActor(string s) 
		{
			return (string)RoleTable[s];
		}

		public EmployeeInfo()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
			object o = ConfigurationSettings.GetConfig("SecureRoutingMapper");
			if (o is Hashtable) 
			{
				RoleTable = (Hashtable)o;
			} 
			else
				throw new Exception("The secure routing mapper is not configured in the application configuration file.");
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
		public EmployeeInformation GetEmployeeInformation() 
		{
			if (RoleTable == null)
				throw new Exception("Can not verify a role-based business object without the roles being defined.");

			if (RequestSoapContext.Current.Security.Tokens.Count== 0)
				throw new Exception("You did not send a security token!");
			
			X509SecurityToken x = null;

			foreach (SecurityToken s in RequestSoapContext.Current.Security.Tokens) 
			{
				if (s is X509SecurityToken) 
					x = (X509SecurityToken)s;
			}
					
			if (x == null)
				throw new Exception("X509 certificate was not provided.");

			string email = Utilities.GetCertEmail(x.Certificate);
//			switch (email.ToLower()) 
//			{
//				case "kevin@mailservera":
					return new EmployeeInformation("Kevin Lux","abc123","Coder","CIS",(float)30.0, GetRoleActor("Supervisor"),GetRoleActor("BusinessOffice"));
//				default:
//					return null;
//			}
		}	
	}
}
