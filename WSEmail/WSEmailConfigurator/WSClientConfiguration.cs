/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Xml;
using Microsoft.Web.Services2.Security.X509;

namespace WSEmailConfigurator
{
	/// <summary>
	/// Summary description for WSClientConfiguration.
	/// </summary>
	/// 

	/*
	    <add key="MailServer" value="http://localhost/WSEmailServer/MailServer.asmx" />
		<add key="SigningCertificate" value="KevinA" />
	*/

	public class WSClientConfiguration : WSBaseConfiguration
	{
		public string ServerURL;

		public WSClientConfiguration()
		{
		}

		public override void Configure() 
		{
			current = new PromptingForm();
			current.Title = "Configuring a WSEmail Client...";
			current.DescriptionBox.Text = "This wizard will guide you through configuring a WSEmail client. At the end of this wizard, an XML configuration file will be created which can be used to connect to a WSEmail server.";
			current.NextButton.Enabled = true;
			current.NextButton.Click += new System.EventHandler(this.GetFileName);
			current.Show();
		}

		public override void GetFileName(object o, System.EventArgs e) 
		{
			this.NextSub = new EventDelegate(this.LoadFile);
			base.GetFileName(null,null);
		}

		public void LoadFile(object o, System.EventArgs e) 
		{
			if (this.FileName != null && System.IO.File.Exists(this.FileName)) 
			{
				this.doc = new XmlDocument();
				doc.PreserveWhitespace = true;
				doc.Load(this.FileName);
				XmlNode n = null;
				n = doc.SelectSingleNode("/configuration/appSettings/add[@key='MailServer']");
				if (n != null)
					this.ServerURL = n.Attributes["value"].Value;

				n = doc.SelectSingleNode("/configuration/appSettings/add[@key='SigningCertificate']");
				if (n != null)
					this.SigningCertificate = n.Attributes["value"].Value;
			}
			GetServerURL(null,null);
		}

		public void GetServerURL(object o, System.EventArgs e) 
		{
			NewForm();
			current.Title = "Picking a Server...";
			current.DescriptionBox.Text = "Enter the URL of the WSEmail server below.\r\n\r\n.It should look like a normal URL (ie. http://somewhere/WSEmailServer/MailServer.asmx)";
			current.InputLabel.Text = "URL:";
			current.InputBox.Focus();
			current.InputBox.Visible = true;
			current.InputBox.Text = this.ServerURL;
			current.InputBox.TextChanged += new System.EventHandler(this.UpdateServerURL);
			current.InputLabel.Visible = true;
			current.InputButton.Visible = true;
			current.InputButton.Text = "Browse...";
			current.InputButton.Click += new System.EventHandler(this.BrowseServers);

			current.ExampleLabel.Visible = true;
			//WriteConfiguration
			//current.NextButton.Click += new System.EventHandler(this.WriteConfiguration);
			current.NextButton.Click += new System.EventHandler(this.GetCertificate);
			current.NextButton.Enabled = false;
			UpdateServerURL(null,null);
			current.Show();
		}
	
		public void GetCertificate(object o, System.EventArgs e) 
		{
			NewForm();
			current.Title = "Picking a Signing Certificate...";
			current.DescriptionBox.Text = "Type the common name (CN) of the certificate you will use to authenticate your identity to the server. The server must trust your certificate and it must be bound to the mail server to work. (ie. The email address on the certificate must be username@mailserver name). A certificate is not necessary, but is required by some parts of the system (the workflow and SSL chat).";
			current.InputLabel.Text = "Certificate CN:";
			current.InputBox.Focus();
			current.InputBox.Visible = true;
			current.InputBox.Text = this.SigningCertificate;
			current.InputBox.TextChanged += new EventHandler(this.UpdateCertificate);
			current.InputButton.Visible = true;
			current.InputButton.Text = "Browse...";
			current.InputButton.Click += new System.EventHandler(this.FetchCertificate);
			current.InputLabel.Visible = true;
			current.NextButton.Click += new System.EventHandler(this.WriteConfiguration);
			current.NextButton.Enabled = false;
			UpdateCertificate(null,null);
			current.Show();
		}


		public void UpdateCertificate(object o, System.EventArgs e) 
		{
			this.SigningCertificate = current.InputBox.Text;
			if (this.SigningCertificate.Length > 0)
				current.NextButton.Enabled = true;
		}

		public void WriteConfiguration(object o, System.EventArgs e) 
		{
			NewForm();
			current.Title = "Writing configuration...";
			current.DescriptionBox.Text = "";

			if (this.doc == null) 
			{
				doc = new XmlDocument();
				doc.AppendChild(doc.ImportNode(doc.CreateNode(XmlNodeType.XmlDeclaration,string.Empty,string.Empty),true));
			}

			XmlNodeList xnl = doc.GetElementsByTagName("configuration");
			XmlNode conf = null;
			if (xnl.Count == 0)
				conf = doc.AppendChild(doc.ImportNode(doc.CreateElement("configuration"),true));
			else
				conf = xnl[0];

			xnl = ((XmlElement)conf).GetElementsByTagName("appSettings");
			XmlNode appsettings;
			if (xnl.Count == 0)
				appsettings = conf.AppendChild(doc.ImportNode(doc.CreateElement("appSettings"),true));
			else
				appsettings = xnl[0];
			/*
				<add key="DeliveryQueue" value="WSEmailServer.MessageQueue" />
				<add key="DatabaseManager" value="WSEmailServer.DatabaseManager" />
				<add key="LocalMTA" value="WSEmailServer.LocalMTA" />
				<add key="MessageAccessor" value="WSEmailServer.MessageAccessor" />
	
				<add key="MailServerName" value="MailServerA" />
				<add key="MailRouter" value="http://tower/WSEMailRouter/Router.ashx" />
	
				<add key="SigningCertificate" value="MailServerA" />
	
			*/

			#region appsettings
			foreach (string s in new string[] {"MailServer","SigningCertificate"}) 
			{
				XmlNode attr; XmlNode j =null;
				XmlNode n = ((XmlElement)appsettings).SelectSingleNode("//add[@key='"+s+"']");
				if (n == null) 
				{
					j = appsettings.AppendChild(doc.ImportNode(doc.CreateElement("add"),true));
					attr = doc.CreateNode(XmlNodeType.Attribute,"key","");
					
					attr.InnerText = s;
					j.Attributes.SetNamedItem(attr);
					attr = doc.CreateNode(XmlNodeType.Attribute,"value","");
				} 
				else 
				{
					attr = n.Attributes["value"];
				}

				switch (s) 
				{
					case "MailServer":
						attr.InnerText = this.ServerURL;
						break;
					case "SigningCertificate":
						attr.InnerText = this.SigningCertificate;
						break;
				}
				
				if (n == null)
					j.Attributes.SetNamedItem(attr);
			}
			#endregion
			base.SaveConfiguration();
			current.NextButton.Text = "Finished";
			current.Show();
			current.NextButton.Click += new EventHandler(Finished);


		}

		public void Finished(object o, System.EventArgs e) 
		{
			current.Visible = false;
		}

		public void FetchCertificate(object o, System.EventArgs e) 
		{
			string c = base.ShowCertDialog(X509CertificateStore.CurrentUserStore(X509CertificateStore.MyStore));
			current.InputBox.Text = c;
		}

		public void BrowseServers(object o, System.EventArgs e) 
		{
			BrowseServers t = new BrowseServers();
			t.ShowDialog();
			current.InputBox.Text = t.URL;
			t.Dispose();
		}


		public void UpdateServerURL(object o, System.EventArgs e) 
		{
			
			if (current.InputBox.Text != "") 
			{
				try 
				{
					Uri u = new Uri(current.InputBox.Text);
					current.ExampleLabel.Text = "Valid URL.";
					current.NextButton.Enabled = true;
					this.ServerURL = current.InputBox.Text;
				} 
				catch
				{
					current.ExampleLabel.Text = "This is an invalid URL, please try again.";
				}

			}
			else 
			{
				current.ExampleLabel.Text = "Please input server URL above.";
				current.NextButton.Enabled = false;
			}
		}


	}
}
