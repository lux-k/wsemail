/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System.Configuration;
using System;
using WSEmailProxy;
using PennLibraries;
using DynamicBizObjects;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;

namespace BusinessObjectsService
{
	/// <summary>
	/// Summary description for Verifier.
	/// </summary>
	[WebService(Namespace="http://securitylab.cis.upenn.edu/AuthorizedRouting")]
	public class Verifier : System.Web.Services.WebService
	{
		Hashtable RoleTable = null;
		public Verifier()
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

		private string GetRoleActor(string s) 
		{
			return (string)RoleTable[s];
		}

		private static void Log(LogType t, string s) 
		{
			PennLibraries.Utilities.LogEvent(t,s);
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

		[WebMethod]
		[XmlInclude(typeof(Timesheet)), XmlInclude(typeof(PurchaseOrder))]
		public string PostObject(BusinessRequest r) 
		{
			BusinessObject b = r.BusinessObject;

			Log(LogType.Debug, "Verifier beginning verification...");
			if (b == null)
				throw new Exception("Can not verify a null business object.");

			if (RoleTable == null)
				throw new Exception("Can not verify a role-based business object without the roles being defined.");

			if (b is Timesheet) 
			{
				MappedItem sup, biz, emp;
				r.RefreshMappings();
				emp = r.GetMappedItem("Employee");
				sup = r.GetMappedItem("Supervisor");
				biz = r.GetMappedItem("BusinessOffice");

				if (emp == null || sup == null || biz == null)
					throw new Exception("Not all necessary mappings are on the timesheet.");

				Log(LogType.ServerDebug, "Form is a timesheet and has the appropriate mappings.");
				Log(LogType.ServerDebug, "Verifying that mappings are correctly signed.");

				Log(LogType.ServerDebug, "Verifying " + emp.Name + " mapping...\n" + "Verifies: " + r.VerifySignature(r.Approvals.GetSignatureByName(emp.User)[0]).ToString());
				Log(LogType.ServerDebug, "Verifying " + sup.Name + " mapping...\n" + "Verifies: " + r.VerifySignature(r.Approvals.GetSignatureByName(sup.User)[0]).ToString());
				Log(LogType.ServerDebug, "Verifying " + biz.Name + " mapping...\n" + "Verifies: " + r.VerifySignature(r.Approvals.GetSignatureByName(biz.User)[0]).ToString());

				Hashtable supMap = new Hashtable();
				supMap.Add(GetRoleActor("Employee"),GetRoleActor("Supervisor"));

				if ((string)supMap[emp.User] == sup.User)
					Log(LogType.ServerDebug, "According to my data, " + emp.User + " is an employee of " + sup.User);
				else 
				{
					Log(LogType.ServerDebug, "According to my data, " + emp.User + " is NOT an employee of " + sup.User);
					throw new Exception("According to my data, " + emp.User + " is NOT an employee of " + sup.User);
				}

				Hashtable bizMap = new Hashtable();
				bizMap.Add(GetRoleActor("Supervisor"),GetRoleActor("BusinessOffice"));

				if ((string)bizMap[sup.User] == biz.User)
					Log(LogType.ServerDebug, "According to my data, " + sup.User + " uses the business office " + biz.User);
				else 
				{
					Log(LogType.ServerDebug, "According to my data, " + sup.User + " does NOT use the business office " + biz.User);
					throw new Exception("According to my data, " + sup.User + " does NOT use the business office " + biz.User);
				}
				
				bool res = true;
				res &= r.RecursivelyVerifySignature(emp.Signature);
				Log(LogType.ServerDebug, "Verifying the signature of the employee: " + res.ToString());
				res &= r.RecursivelyVerifySignature(sup.Signature);
				Log(LogType.ServerDebug, "Verifying the signature of the supervisor: " + res.ToString());
				res &= r.RecursivelyVerifySignature(biz.Signature);
				Log(LogType.ServerDebug, "Verifying the signature of the business office: " + res.ToString());
				Log(LogType.ServerDebug, "Final results: " + res.ToString());
				
				WSEmailMessage m = new WSEmailMessage();
				m.Subject = "Timesheet Approved";
				m.Timestamp = DateTime.Now;
				m.Body = "Hello " + emp.User+",\r\nThis is a friendly note to let you know that your timesheet has been approved.";
				m.Sender = Utilities.GetCertEmail(Utilities.GetSecurityToken(ConfigurationSettings.AppSettings["SigningCertificate"],true).Certificate);
				m.Recipients.Add(emp.User);
				m.Recipients.Add(sup.User);
				Log(LogType.Informational, "Timesheet approved (initiator: " + emp.User +")");
				MailServerProxy ms = new MailServerProxy();
				ms.SecurityToken = Utilities.GetSecurityToken(System.Configuration.ConfigurationSettings.AppSettings["SigningCertificate"],true);
				ms.WSEmailSend(m,null);

			}
			else if (b is PurchaseOrder) 
			{
				MappedItem sup, biz, emp, pur;
				r.RefreshMappings();
				emp = r.GetMappedItem("Claimant");
				sup = r.GetMappedItem("Supervisor");
				biz = r.GetMappedItem("BusinessOffice");
				pur = r.GetMappedItem("Purchasing");

				if (emp == null || sup == null || biz == null || pur == null)
					throw new Exception("Not all necessary mappings are on the PO.");

				Log(LogType.ServerDebug, "Form is a PO and has the appropriate mappings.");
				Log(LogType.ServerDebug, "Verifying that mappings are correctly signed.");

				string[] me = r.GetMappingEntities();
				Hashtable sigcache = new Hashtable();
				for (int i = 0; i < me.Length; i++) 
					sigcache.Add(me[i],r.VerifySignature(r.Approvals.GetSignatureByName(me[i])[0]));

				Log(LogType.ServerDebug, "Verifying " + emp.Name + " mapping...\n" + "Verifies: " + sigcache[emp.MappedBy].ToString());
				Log(LogType.ServerDebug, "Verifying " + sup.Name + " mapping...\n" + "Verifies: " + sigcache[sup.MappedBy].ToString());
				Log(LogType.ServerDebug, "Verifying " + biz.Name + " mapping...\n" + "Verifies: " + sigcache[biz.MappedBy].ToString());
				Log(LogType.ServerDebug, "Verifying " + pur.Name + " mapping...\n" + "Verifies: " + sigcache[pur.MappedBy].ToString());

				Hashtable supMap = new Hashtable();
				supMap.Add(GetRoleActor("Employee"),GetRoleActor("Supervisor"));

				if ((string)supMap[emp.User] == sup.User)
					Log(LogType.ServerDebug, "According to my data, " + emp.User + " is an employee of " + sup.User);
				else 
				{
					Log(LogType.ServerDebug, "According to my data, " + emp.User + " is NOT an employee of " + sup.User);
					throw new Exception("According to my data, " + emp.User + " is NOT an employee of " + sup.User);
				}

				Hashtable bizMap = new Hashtable();
				bizMap.Add(GetRoleActor("Supervisor"),GetRoleActor("BusinessOffice"));

				if ((string)bizMap[sup.User] == biz.User)
					Log(LogType.ServerDebug, "According to my data, " + sup.User + " uses the business office " + biz.User);
				else 
				{
					Log(LogType.ServerDebug, "According to my data, " + sup.User + " does NOT use the business office " + biz.User);
					throw new Exception("According to my data, " + sup.User + " does NOT use the business office " + biz.User);
				}
				
				bool res = true;
				res &= r.RecursivelyVerifySignature(emp.Signature);
				Log(LogType.ServerDebug, "Verifying the signature of the employee: " + res.ToString());

				res &= r.RecursivelyVerifySignature(r.Approvals.GetSignatureByName(GetRoleActor("Purchasing"))[0]);
				Log(LogType.ServerDebug, "Verifying the signature of the PO service: " + res.ToString());
				Log(LogType.ServerDebug, "Final results: " + res.ToString());
				
				WSEmailMessage m = new WSEmailMessage();
				m.Subject = "PO Approved";
				m.Timestamp = DateTime.Now;
				m.Body = "Hello " + emp.User+",\r\nThis is a friendly note to let you know that your PO has been approved.";
				m.Sender = Utilities.GetCertEmail(Utilities.GetSecurityToken(ConfigurationSettings.AppSettings["SigningCertificate"],true).Certificate);
				m.Recipients.Add(emp.User);
				Log(LogType.Informational, "PO approved (initiator: " + emp.User +")");
				MailServerProxy ms = new MailServerProxy();
				ms.SecurityToken = Utilities.GetSecurityToken(System.Configuration.ConfigurationSettings.AppSettings["SigningCertificate"],true);
				ms.WSEmailSend(m,null);




			}
			else
				throw new Exception("Do not know how to verify the object you sent me.");

			return "Message received by verifier.";

		}
	}
}
