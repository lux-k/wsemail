/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System.Windows.Forms;
using System;
using WSERoutingTable;
using System.Xml;

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

	public class WSRouterConfiguration : WSBaseConfiguration
	{
		public string ServerURL;
		public RoutingTable Table;
		private Routing r;

		public WSRouterConfiguration()
		{
		}

		public override void Configure() 
		{
			current = new PromptingForm();
			current.Title = "Configuring a WSEmail Router...";
			current.DescriptionBox.Text = "This wizard will guide you through configuring a WSEmail router. At the end of this wizard, an XML configuration file will be created which can be used to route messages between WSEmail servers and network boundaries.";
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
				RouterConfigurationReader rcr = new RouterConfigurationReader();
				XmlNode n = doc.SelectSingleNode("/configuration/RoutingTable");
				this.Table = (RoutingTable)rcr.Create(null,null,n);
				
			}
			ShowRoutingTable(null,null);
		}

		public void ShowRoutingTable(object o, System.EventArgs e) 
		{
			current.Hide();
			r = new Routing(this.Table);
			r.NextButton.Click += new EventHandler(RoutesDefined);
			r.Show();
		}
		public void RoutesDefined(object o, System.EventArgs e)  
		{
			this.Table = r.RoutingTable;
			r.Dispose();
			WriteConfiguration(null,null);
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

			XmlNode confs = (XmlElement)doc.SelectSingleNode("/configuration/configSections");
			if (confs == null) 
				confs = conf.AppendChild(doc.ImportNode(doc.CreateElement("configSections"),true));

			XmlNode rt = (XmlElement)doc.SelectSingleNode("/configuration/configSections/section[@name='RoutingTable']");
			if (rt == null) 
			{
				XmlNode s = confs.AppendChild(doc.ImportNode(doc.CreateElement("section"),true));

				XmlNode attr = doc.CreateNode(XmlNodeType.Attribute,"name","");
				attr.InnerText = "RoutingTable";
				s.Attributes.SetNamedItem(attr);

				attr = doc.CreateNode(XmlNodeType.Attribute,"type","");
				attr.InnerText = "WSERoutingTable.RouterConfigurationReader,RoutingTable";
				s.Attributes.SetNamedItem(attr);
			}

			XmlNode routet = (XmlElement)doc.SelectSingleNode("/configuration/RoutingTable");
			if (routet == null) 
				routet = conf.AppendChild(doc.ImportNode(doc.CreateElement("RoutingTable"),true));

			XmlNode routes = (XmlElement)doc.SelectSingleNode("/configuration/RoutingTable/routes");
			if (routes == null) 
				routes = routet.AppendChild(doc.ImportNode(doc.CreateElement("routes"),true));
			
			routes.RemoveAll();

			foreach (Route r in this.Table.Routes)
				routes.AppendChild(r.Serialize(doc));

			current.DescriptionBox.Text = doc.OuterXml;
			base.SaveConfiguration();
			current.NextButton.Text = "Finished";
			current.Show();
			current.NextButton.Click += new EventHandler(Finished);


		}

		public void Finished(object o, System.EventArgs e) 
		{
			current.Visible = false;
		}

		/*
		public void FetchCertificate(object o, System.EventArgs e) 
		{
			string c = base.ShowCertDialog(Microsoft.Web.Services.Security.X509.X509CertificateStore.CurrentUserStore(Microsoft.Web.Services.Security.X509.X509CertificateStore.MyStore));
			current.InputBox.Text = c;
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
				catch (Exception x ) 
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

		*/
	}
}
