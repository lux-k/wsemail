/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System.Configuration;
using WSEmailProxy;
using System;
using Microsoft.Web.Services2.Security.Tokens;
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
	/// Summary description for WebForm1.
	/// </summary>
	public class WSEmailPage : System.Web.UI.Page
	{
	
		public SecurityToken GetUserToken() 
		{
			SecurityToken s = (SecurityToken)Session["Token"];
			if (s == null)
				throw new Exception("Null token!!");
			return s;
		}

		public MailServerProxy GetMailServerProxy() 
		{
			if (Session["Proxy"] != null) 
			{
				return (MailServerProxy)Session["Proxy"];
			}
		
			MailServerProxy p = new MailServerProxy(ConfigurationSettings.AppSettings["ServerUrl"]);
			if (p.SecurityToken == null)
				p.SecurityToken = GetUserToken();
			Session["Proxy"] = p;
			return p;
		}
	}
}
