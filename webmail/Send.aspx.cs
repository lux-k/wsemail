/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using WSEmailProxy;
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
	/// Summary description for Send.
	/// </summary>
	public class Send : WSEmailPage
	{
		protected System.Web.UI.WebControls.TextBox To;
		protected System.Web.UI.WebControls.TextBox Subject;
		protected System.Web.UI.WebControls.TextBox Message;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.HtmlControls.HtmlForm Form1;
		protected System.Web.UI.WebControls.Button Button1;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			if (Request.QueryString["to"] != null) 
				this.To.Text = Request.QueryString["to"];
			if (Request.QueryString["replyid"] != null) 
			{
				string repid = Request.QueryString["replyid"];
				int id = -1;
				try 
				{
					id = int.Parse(repid);
				} 
				catch {}
				if (id > 0) 
				{
					if (Session["LastViewed"] != null) 
					{
						WSEmailPackage p = (WSEmailPackage)Session["LastViewed"];
						PopulateForm(p);
					} 
					else 
					{
						WSEmailPackage p = this.GetMailServerProxy().WSEmailRetrieve(id);
						PopulateForm(p);


					}
				}
			}
		}

		private void PopulateForm(WSEmailPackage p) 
		{
			this.To.Text = p.theMessage.Sender;
			if (p.theMessage.Subject.ToLower().StartsWith("re:"))
				this.Subject.Text = p.theMessage.Subject;
			else
				this.Subject.Text = "Re: " + p.theMessage.Subject;
			this.Message.Text = "\n\nIn reply to:\n\n" + p.theMessage.Body;
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

			WSEmailMessage m= new WSEmailMessage();
			m.Recipients.AddRange(RecipientList.ParseRecipients(To.Text));
			m.Subject = Subject.Text;
			m.Body = Message.Text;
			m.Timestamp = DateTime.Now;
			this.GetMailServerProxy().WSEmailSend(m,null);
			Response.Redirect("Sent.aspx?r="+m.Recipients.ToString()+"&s="+m.Subject);
		}
	}
}
