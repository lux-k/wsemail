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
using System.Web.Mail;

namespace WSEmailErrorReporting
{
	[WebService(Namespace="http://securitylab.cis.upenn.edu/WSEmail")]
	public class Report : System.Web.Services.WebService
	{
		public Report()
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

		[WebMethod]
		public bool ReportError(string s)
		{
			bool ret = false;

			try 
			{
				SmtpMail.SmtpServer = ConfigurationSettings.AppSettings["SMTPServer"];
				MailMessage m = new MailMessage();
				m.From = ConfigurationSettings.AppSettings["Recipient"];
				m.To = ConfigurationSettings.AppSettings["Recipient"];
				m.Subject = "WSEmail fault report";
				m.Body = "Received at: " + DateTime.Now.ToLongDateString().ToString() + "\r\n" +s;
				m.BodyFormat = MailFormat.Text;

				SmtpMail.Send(m);
				ret = true;
			} 
			catch 
			{
				ret = false;
			}

			return ret;
		}
	}
}
