using System.Configuration;
using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using Microsoft.Web.Services;
using Microsoft.Web.Services.Security;
using Microsoft.Web.Services.Configuration;
using Microsoft.Web.Services.Routing;
using Microsoft.Web.Services.Referral;
using Microsoft.Web.Services.Timestamp;
using PennLibraries;

namespace WSEMailRouter 
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global : System.Web.HttpApplication
	{
		public Global()
		{
			InitializeComponent();
		}	
		
		protected void Application_Start(Object sender, EventArgs e)
		{
			// PennRoutingFilters.PennRoutingUtilities.AddPennRoutingFilters(true);
			PennLibraries.Utilities.AddPennLoggingFilters();
		}
 
		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}

