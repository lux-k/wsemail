/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using WSEmailProxy;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace webmail
{
	/// <summary>
	/// Summary description for Read.
	/// </summary>
	public class Read : WSEmailPage
	{
		protected System.Web.UI.WebControls.TextBox To;
		protected System.Web.UI.WebControls.TextBox From;
		protected System.Web.UI.WebControls.TextBox Subject;
		protected System.Web.UI.WebControls.TextBox Message;
		protected System.Web.UI.WebControls.TextBox Date;
		protected System.Web.UI.HtmlControls.HtmlForm Form1;
		protected System.Web.UI.WebControls.LinkButton button;
		protected System.Web.UI.WebControls.HyperLink linkReply;
		protected System.Web.UI.WebControls.Button Button1;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			if (Request.QueryString == null || Request.QueryString.Equals(""))
				throw new Exception("No message number given");
			int i = 0;
			try 
			{
				i=int.Parse(Request.QueryString.Get("id"));
			}
			catch  
			{
				throw new Exception("Query string is not a number.");
			}
			if (i==0)
				throw new Exception("Invalid message number");

			WSEmailPackage wp=this.GetMailServerProxy().WSEmailRetrieve(i);
			WSEmailMessage m=wp.theMessage;
			To.Text = m.Recipients.ToString();
			From.Text = m.Sender;
			Subject.Text = m.Subject;
			Message.Text = m.Body;
			Date.Text = m.Timestamp.ToLongTimeString();
			Session.Add("LastViewed",wp);
			this.linkReply.NavigateUrl = "Send.aspx?replyid="+wp.MessageID.ToString();
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
