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
	/// Summary description for NewAccount.
	/// </summary>
	public class NewAccount : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox txtUser;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.TextBox txtPass1;
		protected System.Web.UI.WebControls.TextBox txtPass2;
		protected System.Web.UI.WebControls.DropDownList DropDownList1;
		protected System.Web.UI.WebControls.Button Button1;
		protected System.Web.UI.WebControls.Label lblResponse;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Image Image1;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.HyperLink HyperLink1;
		protected System.Web.UI.WebControls.Label Label3;
	
		public NewAccount() {}
		private void Page_Load(object sender, System.EventArgs e)
		{
			DropDownList1.Items.Clear();
			DropDownList1.Items.Add(Global.ServerConfiguration.Name);
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

		private void DropDownList1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}

		private void Button1_Click(object sender, System.EventArgs e)
		{
			if (!(txtUser.Text.Length > 0)) 
			{
				lblResponse.Text = "Please include a user name!";
				return;
			}

			if ((txtPass1.Text.Length == 0) || (txtPass2.Text.Length == 0)) 
			{
				lblResponse.Text = "Please include a password!";
				return;
			}

			if (txtPass1.Text != txtPass2.Text) 
			{
				lblResponse.Text = "Please include matching passwords!";
				return;
			}

			if (txtUser.Text.IndexOf("'") >= 0)
				return;

			MailServerInterfaces.DatabaseConnection c = Global.ServerConfiguration.Database.Connection;
			
			SqlCommand command = new SqlCommand("WSEmailNewUser",c.Connection );
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add("@user",	txtUser.Text);
			command.Parameters.Add("@pass", txtPass1.Text);
			try 
			{
				command.ExecuteNonQuery();
				lblResponse.Text = "Created account for user '" + txtUser.Text + "' on WSEmail server " + Global.ServerConfiguration.Name + ".";
			} 
			catch (Exception ex) 
			{
				lblResponse.Text = "An error occurred while trying to create the user account.\r\n\r\nSpecifically: " + ex.Message;
			}

			Global.ServerConfiguration.Log("Created user account for '" + txtUser.Text + "'");
			Global.ServerConfiguration.Database.Free(c);

	}
	}
}
