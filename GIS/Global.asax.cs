using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;

namespace GIS 
{
	/// <summary>
	/// Global objects for the running GIS web service
	/// </summary>
	public class Global : System.Web.HttpApplication
	{
		#region Variables
		/// <summary>
		/// The TCP listening Server thread
		/// </summary>
		public static System.Threading.Thread m_tServer;
		/// <summary>
		/// Default policy for merchants allowed to fetch user lists
		/// </summary>
		public static P3P.POLICY m_policy;
		/// <summary>
		/// A list of the of the merchants allowed to fetch the full list of users.
		/// </summary>
		public static System.Collections.ArrayList m_merchants;
		/// <summary>
		/// A list of available users
		/// </summary>
		public static System.Collections.ArrayList m_users;
		/// <summary>
		/// TCP Server that will listen for client connections and manage them
		/// </summary>
		public static System.Net.Sockets.TcpListener m_tcp_server;
		/// <summary>
		/// The AdServerThread connections that are sending ads to associated clients
		/// </summary>
		public static System.Collections.ArrayList m_connections;
		/// <summary>
		/// Each user that is added to this list has an array list of new ads for it
		/// </summary>
		public static System.Collections.SortedList m_ads_by_user;
		/// <summary>
		/// List of Merchants that are approved for retrieving users.  Must be removed for
		/// secure operation of the GIS.
		/// </summary>
		public static ArrayList m_insecure;
		#endregion

		public string m_test;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public Global()
		{
			InitializeComponent();
		}	
		
		/// <summary>
		/// Run when the GIS Web Service application is loaded up by the Web Server
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_Start(Object sender, EventArgs e)
		{
			m_users = new ArrayList(256);
			m_ads_by_user = new SortedList();
			m_merchants = new ArrayList(256);
			m_insecure = new ArrayList(256);
			m_policy = new P3P.POLICY();
			// give some default P3P policy
			SetPolicy();
		}

		/// <summary>
		/// Loads some generic P3P policy into the GIS for checking against other policies
		/// </summary>
		private void SetPolicy()
		{
			// make some reasonable policy requirements for this GIS
			// statment 1
			P3P.STATEMENT s1 = new P3P.STATEMENT();
			s1.categories.computer = true;
			s1.categories.demographic = true;
			s1.categories.financial = true;
		
			s1.consequence = "College kids love pizza";

			s1.nonidentifiable = true;

			s1.purpose.admin.present = true;
			s1.purpose.admin.required = P3P.REQUIRED.always;
			s1.purpose.develop.present = true;
			s1.purpose.pseudo_analysis.present = true;

			s1.recipient._public.present = true;
			s1.recipient.other_recipient.present = true;

			s1.retention.business_practices = true;
			s1.retention.stated_purpose = true;

			// put it all together
			m_policy.m_statements[0] = s1;

			// add one access thing
			m_policy.access.none = true;
		}
 
		protected void Session_Start(Object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_Error(Object sender, EventArgs e)
		{

		}

		protected void Session_End(Object sender, EventArgs e)
		{

		}

		protected void Application_End(Object sender, EventArgs e)
		{

		}
			
		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.components = new System.ComponentModel.Container();
		}
		#endregion
	}
}

