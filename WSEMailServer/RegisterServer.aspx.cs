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
	/// Summary description for RegisterServer.
	/// </summary>
	public class RegisterServer : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Image Image1;
		protected System.Web.UI.WebControls.Label lblResponse;
		protected System.Web.UI.WebControls.Button Button1;
		protected System.Web.UI.WebControls.TextBox txtServer;
		protected System.Web.UI.WebControls.TextBox txtUrl;
		protected System.Web.UI.WebControls.TextBox txtAdmin;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
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
			this.Button1.Click += new System.EventHandler(this.Button1_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void Button1_Click(object sender, System.EventArgs e)
		{
			if (txtServer.Text.Length == 0 || txtAdmin.Text.Length == 0 || txtUrl.Text.Length == 0) 
			{
				lblResponse.Text = "Please be sure to fill out the server name, url and administrator fields!";
				return;
			}

			MailServerInterfaces.DatabaseConnection c = Global.ServerConfiguration.Database.Connection;
			
			SqlCommand command = new SqlCommand("WSEmailAddServer",c.Connection );
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add("@server",txtServer.Text);
			command.Parameters.Add("@owner",txtAdmin.Text);
			command.Parameters.Add("@url",txtUrl.Text);

			try 
			{
				command.ExecuteNonQuery();
				lblResponse.Text = "Added the server named " + txtServer.Text + ".";
			} 
			catch (Exception ex) 
			{
				lblResponse.Text = "An error occurred while trying to create the machine account.\r\n\r\nSpecifically: " + ex.Message;
			}

			Global.ServerConfiguration.Database.Free(c);

		}
	}
}
