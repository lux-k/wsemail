/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Data.SqlClient;
using MailServerInterfaces;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


namespace WSEmailServer
{
	/// <summary>
	/// Provides a web page that will list the users who have a password on the server.
	/// </summary>
	public class ListUsers : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.HyperLink HyperLink1;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Image Image1;
	
		private void Page_Load(object sender, System.EventArgs e)
		{

		}

		public string GetUsers() 
		{
			string output = "<br><br>The following users have registered a password with this server:<br><br>";
			DatabaseConnection c = Global.ServerConfiguration.Database.Connection;
			SqlCommand command = new SqlCommand("WSEmailRetrieveUserList",c.Connection );
			command.CommandType = CommandType.StoredProcedure;
			SqlDataReader r = command.ExecuteReader();
			
			while (r.Read())
				output += r.GetString(0) + "<br>";
			r.Close();

			Global.ServerConfiguration.Database.Free(c);
			return output;
		}
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
