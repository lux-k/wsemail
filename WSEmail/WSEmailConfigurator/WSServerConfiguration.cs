/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using ACLs;
using System.IO;
using System.Data.SqlClient;
using System.Collections;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Web.Services2.Security.X509;

namespace WSEmailConfigurator
{
	/// <summary>
	/// Summary description for WSServerConfiguration.
	/// </summary>
	/// 
	public delegate void EmptyDelegate();

	public class WSServerConfiguration : WSBaseConfiguration
	{

		public string ServerName;
		public string Router;
		public string SQLServerName;
		public string SQLInstanceName;
		public string SQLDatabaseName;
		public string SQLUserName;
		public string SQLPassword;
		public string DNSServer;

		public WSServerConfiguration()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override void Configure() 
		{

			NewForm();
			current.Title = "Configuring a WSEmail Server...";
			current.DescriptionBox.Text = "This wizard will guide you through configuring a WSEmail server. At the end of this wizard, an XML configuration file will be created which can be used to start the server.";
			current.NextButton.Enabled = true;
			current.NextButton.Click += new System.EventHandler(this.GetFileName);
			current.Show();
		}

		public void LoadFile(object o, System.EventArgs e) 
		{
			if (this.FileName != null && System.IO.File.Exists(this.FileName)) 
			{
				this.doc = new XmlDocument();
				doc.PreserveWhitespace = true;
				doc.Load(this.FileName);
				XmlNode n = null;

				n = doc.SelectSingleNode("/configuration/appSettings/add[@key='MailServerName']");
				if (n != null)
					this.ServerName = n.Attributes["value"].Value;

				n = doc.SelectSingleNode("/configuration/appSettings/add[@key='MailRouter']");
				if (n != null)
					this.Router = n.Attributes["value"].Value;

				n = doc.SelectSingleNode("/configuration/appSettings/add[@key='SigningCertificate']");
				if (n != null)
					this.SigningCertificate = n.Attributes["value"].Value;

				n = doc.SelectSingleNode("/configuration/appSettings/add[@key='DNSServer']");
				if (n != null)
					this.DNSServer = n.Attributes["value"].Value;

				n = doc.SelectSingleNode("/configuration/Database/username");
				if (n != null)
					this.SQLUserName = n.Attributes["value"].Value;

				n = doc.SelectSingleNode("/configuration/Database/password");
				if (n != null)
					this.SQLPassword = n.Attributes["value"].Value;

				n = doc.SelectSingleNode("/configuration/Database/server");
				if (n != null) 
				{
					string[] b = n.Attributes["value"].Value.Split('\\');
					if (b.Length <= 1) 
						this.SQLServerName = n.Attributes["value"].Value;
					else 
					{
						this.SQLServerName = b[0];
						this.SQLInstanceName = b[1];
					}
				}

				n = doc.SelectSingleNode("/configuration/Database/database");
				if (n != null)
					this.SQLDatabaseName = n.Attributes["value"].Value;
				string file = @"C:\WSEmail-LogFile.txt";
				if (!File.Exists(file)) 
				{
					File.Create(file);
				}
				
				System.OperatingSystem v = Environment.OSVersion;
				if (v.Version.Minor == 2) 
				{
					// 2003 server
					SetACL("Network Service",file);
					SetACL("Network Service",FileName);
				} 
				else 
				{
					SetACL("ASPNET",file);
					SetACL("ASPNET",FileName);
				}
			}
			GetServerName(null,null);
		}

		private bool SetACL(string acl, string file) 
		{
			try 
			{
				SecurityDescriptor secDesc = SecurityDescriptor.GetFileSecurity (file, SECURITY_INFORMATION.DACL_SECURITY_INFORMATION); 

				Dacl dacl = secDesc.Dacl; 
				dacl.AddAce (new AceAccessAllowed (new Sid (acl), AccessType.GENERIC_READ | AccessType.GENERIC_WRITE,AceFlags.NO_PROPAGATE_INHERIT_ACE)); 

				secDesc.SetDacl(dacl); 

				secDesc.SetFileSecurity(file, SECURITY_INFORMATION.DACL_SECURITY_INFORMATION);
				return true;
			} 
			catch (Exception e) 
			{
				MessageBox.Show("Unable to set appropriate ACL on file. Exception: " + e.Message,"Oops!");
				return false;
			}
		}

		public override void GetFileName(object o, System.EventArgs e) 
		{
			this.NextSub = new EventDelegate(this.LoadFile);
			base.GetFileName(null,null);
		}

		public void GetRouterName(object o, System.EventArgs e) 
		{
			NewForm();
			current.Title = "Picking a Router...";
			current.DescriptionBox.Text = "Presently WS-Email relies on a semi-static routing scheme for transferring messages to non-local destinations. This requires the use of a WS-Email routing server.\r\n\r\nEnter the address of the WS-Email server below. It should look like a normal URL (ie. http://somewhere/WSEmailRouter/Router.ashx).";
			current.InputLabel.Text = "Router name:";
			current.InputBox.Focus();
			current.InputBox.Text = this.Router;
			current.InputBox.Visible = true;
			current.InputBox.TextChanged += new System.EventHandler(this.UpdateRouterName);
			current.InputLabel.Visible = true;
			current.ExampleLabel.Visible = true;
			UpdateRouterName(null,null);
			current.NextButton.Click += new System.EventHandler(this.CheckRouterURL);
			current.Show();
		}

		public void GetDNSServer(object o, System.EventArgs e) 
		{
			NewForm();
			current.Title = "Picking a DNS Server...";
			current.DescriptionBox.Text = "WSEmail depends on DNS SRV records to determine how and where to route mail. If you require a specific DNS server to be queried for WSEmail message routing, enter it below.";
			current.InputLabel.Text = "DNS hostname/ip:";
			current.InputBox.Focus();
			current.InputBox.Text = this.DNSServer;
			current.InputBox.Visible = true;
			current.InputBox.TextChanged += new System.EventHandler(this.UpdateDNSServer);
			current.InputLabel.Visible = true;
			current.ExampleLabel.Visible = true;
			UpdateDNSServer(null,null);
			current.NextButton.Click += new System.EventHandler(this.CheckDNSServer);
			current.Show();
		}

		public void GetSQLServerName(object o, System.EventArgs e) 
		{
			NewForm();
			current.Title = "Database setup...";
			current.DescriptionBox.Text = "A WSEmail server needs a database server to store messages in. SQL Server 2000 is the current recommended server (and the only supported).\r\n\r\nWe'll need some information about your server to continue.";
			current.DescriptionBox.Select(0,0);
			current.InputLabel.Text = "Server name:";
			current.InputBox.Focus();
			current.InputBox.Text = this.SQLServerName;
			current.InputBox.Visible = true;
			current.InputBox.TextChanged += new System.EventHandler(this.UpdateSQLServerName);
			current.InputLabel.Visible = true;
			current.ExampleLabel.Visible = true;
			current.NextButton.Click += new System.EventHandler(this.GetSQLInstanceName);
			current.NextButton.Enabled=false;
			UpdateSQLServerName(null,null);
			current.Show();

		}

		public void GetSQLInstanceName(object o, System.EventArgs e) 
		{
			NewForm();
			current.Title = "Database setup (continued)...";
			current.DescriptionBox.Text = "Some DBAs prefer to give seperate instances to different databases. If you are using a specific instance on the database server, enter it below.";
			current.DescriptionBox.Select(0,0);
			current.InputLabel.Text = "Instance name:";
			current.InputBox.Focus();
			current.InputBox.Text = this.SQLInstanceName;
			current.InputBox.Visible = true;
			current.InputBox.TextChanged += new System.EventHandler(this.UpdateSQLInstanceName);
			current.InputLabel.Visible = true;
			current.ExampleLabel.Visible = true;
			current.NextButton.Click += new System.EventHandler(this.GetSQLDatabaseName);
			UpdateSQLInstanceName(null,null);
			current.Show();
		}

		public void GetSQLDatabaseName(object o, System.EventArgs e) 
		{
			pastForms.Add(current);
			current.Hide();
			current = new PromptingForm();

			current.Title = "Database setup (continued)...";
			current.DescriptionBox.Text = "What specific database on the server " + this.SQLServerName + " will be used to store data?";
			current.DescriptionBox.Select(0,0);
			current.InputLabel.Text = "Database name:";
			current.InputBox.Focus();
			current.InputBox.Text = this.SQLDatabaseName;
			current.InputBox.Visible = true;
			current.InputBox.TextChanged += new System.EventHandler(this.UpdateSQLDatabaseName);
			current.InputLabel.Visible = true;
			current.NextButton.Click += new System.EventHandler(this.GetSQLUserName);
			UpdateSQLDatabaseName(null,null);
			current.Show();

		}


		public void GetSQLUserName(object o, System.EventArgs e) 
		{
			NewForm();

			current.Title = "Database setup (continued)...";
			current.DescriptionBox.Text = "What username should the WS-EMail server use to connect to the server?";
			current.DescriptionBox.Select(0,0);
			current.InputLabel.Text = "User name:";
			current.InputBox.Focus();
			current.InputBox.Text = this.SQLUserName;
			current.InputBox.Visible = true;
			current.InputBox.TextChanged += new System.EventHandler(this.UpdateSQLUserName);
			current.InputLabel.Visible = true;
			current.NextButton.Click += new System.EventHandler(this.GetSQLPassword);
			UpdateSQLUserName(null,null);
			current.Show();
		}

		public void GetSQLPassword(object o, System.EventArgs e) 
		{
			NewForm();

			current.Title = "Database setup (continued)...";
			current.DescriptionBox.Text = "What password will authenticate the username '"+this.SQLUserName+"' at the SQL Server?";
			current.DescriptionBox.Select(0,0);
			current.InputLabel.Text = "Password:";
			current.InputBox.Text = this.SQLPassword;
			current.InputBox.PasswordChar = '*';
			current.InputBox.Focus();
			current.InputBox.Visible = true;
			current.InputBox.TextChanged += new System.EventHandler(this.UpdateSQLPassword);
			current.InputLabel.Visible = true;
			current.NextButton.Click += new System.EventHandler(this.TestSQLConnection);
			UpdateSQLPassword(null,null);
			current.Show();
		}

		public void TestSQLConnection(object o, System.EventArgs e) 
		{
			NewForm();

			current.Title = "Database setup (final)...";
			current.DescriptionBox.Text = "We now have all the information needed to test the database connection. Click the button below to test database connectivity or skip the test by clicking next.";
			current.DescriptionBox.Select(0,0);
			
			current.InputLabel.Visible = false;
			current.InputBox.Visible = false;
			current.InputButton.Visible=true;
			current.InputButton.Text = "Check Connectivity...";
			current.InputButton.Click += new System.EventHandler(this.DoTestSQLConnection);
			current.ExampleLabel.Visible = true;
			current.NextButton.Click += new System.EventHandler(this.GetCertificate);
			current.Show();
		}


		public void GetCertificate(object o, System.EventArgs e) 
		{
			NewForm();
			current.Title = "Picking a Signing Certificate...";
			current.DescriptionBox.Text = "Type the common name of the certificate you will use to prove your identity to clients. This certificate should be trusted by clients. There are no required semantics on the certificate at this time, but there may be in the future.\r\n\r\nThe certificate must live in the local machine store.";
			current.InputLabel.Text = "Certificate CN:";
			current.InputBox.Focus();
			current.InputBox.Visible = true;
			current.InputBox.Text = this.SigningCertificate;
			current.InputBox.TextChanged += new System.EventHandler(this.UpdateCertificate);
			current.InputButton.Visible = true;
			current.InputButton.Text = "Browse...";
			current.InputButton.Click += new System.EventHandler(this.FetchCertificate);
			current.InputLabel.Visible = true;
			current.NextButton.Click += new System.EventHandler(this.WriteConfiguration);
			current.Show();
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

			xnl=((XmlElement)conf).GetElementsByTagName("configSections");
			XmlNode configsections;
			if (xnl.Count == 0)
				configsections = conf.AppendChild(doc.ImportNode(doc.CreateElement("configSections"),true));
			else
				configsections = xnl[0];

			xnl = ((XmlElement)configsections).GetElementsByTagName("section");
			bool found = false;
			if (xnl.Count > 0) 
			{
				for (int i = 0; i < xnl.Count; i++) 
				{
					if (xnl[0].Attributes["name"].Value.Equals("Database")) 
					{
						found = true;
						break;
					}
				}
			}

			if (found == false) 
			{
				XmlNode j = doc.ImportNode(doc.CreateElement("section"),true);

				XmlNode attr = 
					doc.CreateNode(XmlNodeType.Attribute,"name","");
				attr.InnerText = "Database";
				j.Attributes.SetNamedItem(attr);

				attr = doc.CreateNode(XmlNodeType.Attribute,"type","");
				attr.InnerText = "WSEmailServer.DatabaseConfigurationReader,WSEmailServer";
				j.Attributes.SetNamedItem(attr);
				configsections.AppendChild(j);
			}

			#region database
			xnl = ((XmlElement)conf).GetElementsByTagName("Database");
			XmlNode database;
			if (xnl.Count == 0)
				database = conf.AppendChild(doc.ImportNode(doc.CreateElement("Database"),true));
			else
				database = xnl[0];

			{
				XmlNodeList xl = ((XmlElement)database).GetElementsByTagName("username");
				if (xl.Count == 0) 
				{
					XmlNode j = doc.ImportNode(doc.CreateElement("username"),true);
					XmlNode attr = 
						doc.CreateNode(XmlNodeType.Attribute,"value","");
					attr.InnerText = this.SQLUserName;
					j.Attributes.SetNamedItem(attr);
					database.AppendChild(j);
				} 
				else 
				{
					xl[0].Attributes["value"].InnerText = this.SQLUserName;
				}

				xl = ((XmlElement)database).GetElementsByTagName("password");
				if (xl.Count == 0) 
				{
					XmlNode j = doc.ImportNode(doc.CreateElement("password"),true);
					XmlNode attr = 
						doc.CreateNode(XmlNodeType.Attribute,"value","");
					attr.InnerText = this.SQLPassword;
					j.Attributes.SetNamedItem(attr);
					database.AppendChild(j);
				} 
				else 
				{
					xl[0].Attributes["value"].InnerText = this.SQLPassword;
				}

				xl = ((XmlElement)database).GetElementsByTagName("server");
				if (xl.Count == 0) 
				{
					XmlNode j = doc.ImportNode(doc.CreateElement("server"),true);
					XmlNode attr = 
						doc.CreateNode(XmlNodeType.Attribute,"value","");
					string k = this.SQLServerName;
					if (this.SQLInstanceName.Length > 0) 
						k += "\\" + this.SQLInstanceName;
					attr.InnerText = k;
					j.Attributes.SetNamedItem(attr);
					database.AppendChild(j);
				} 
				else 
				{
					string k = this.SQLServerName;
					if (this.SQLInstanceName.Length > 0) 
						k += "\\" + this.SQLInstanceName;
					xl[0].Attributes["value"].InnerText = k;
				}

				xl = ((XmlElement)database).GetElementsByTagName("database");
				if (xl.Count == 0) 
				{
					XmlNode j = doc.ImportNode(doc.CreateElement("database"),true);
					XmlNode attr = 
						doc.CreateNode(XmlNodeType.Attribute,"value","");
					attr.InnerText = this.SQLDatabaseName;
					j.Attributes.SetNamedItem(attr);
					database.AppendChild(j);
				} 
				else 
				{
					xl[0].Attributes["value"].InnerText = this.SQLDatabaseName;
				}

			}
			#endregion

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
			foreach (string s in new string[] {"DeliveryQueue","DatabaseManager","LocalMTA","MessageAccessor","MailServerName","MailRouter","SigningCertificate","DNSServer"}) 
			{
				XmlNode attr;XmlNode j =null;
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
					case "DeliveryQueue":
						attr.InnerText = "WSEmailServer.MessageQueue";
						break;
					case "DatabaseManager":
						attr.InnerText = "WSEmailServer.DatabaseManager";
						break;
					case "LocalMTA":
						attr.InnerText = "WSEmailServer.LocalMTA";
						break;
					case "MessageAccessor":
						attr.InnerText = "WSEmailServer.MessageAccessor";
						break;
					case "MailServerName":
						attr.InnerText = this.ServerName;
						break;
					case "MailRouter":
						attr.InnerText = this.Router;
						break;
					case "SigningCertificate":
						attr.InnerText = this.SigningCertificate;
						break;
					case "DNSServer":
						attr.InnerText = this.DNSServer;
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
			string c = base.ShowCertDialog(X509CertificateStore.LocalMachineStore(X509CertificateStore.MyStore));
			current.InputBox.Text = c;
			UpdateCertificate(null,null);
		}

		public void UpdateCertificate(object o, System.EventArgs e) 
		{
			this.SigningCertificate = current.InputBox.Text;
			if (this.SigningCertificate.Length > 0)
				current.NextButton.Enabled = true;
		}
		
		public void DoTestSQLConnection(object o, System.EventArgs e) 
		{
			try 
			{
				string str = "data source="+this.SQLServerName;
				if (this.SQLInstanceName != null)
					str += "\\"+this.SQLInstanceName;
				str += "; user id="+this.SQLUserName;
				str += "; password="+this.SQLPassword;
				str += "; initial catalog="+this.SQLDatabaseName;

				SqlConnection s = new SqlConnection(str);
				s.Open();
				current.ExampleLabel.Text = "Database connection succeeded!";
				s.Close();
			}
			catch (Exception x) 
			{
				current.ExampleLabel.Text = "Database connection failed. " + x.Message;
			}
		}

		
		public void UpdateSQLPassword(object o, System.EventArgs e) 
		{
			if (current.InputBox.Text != "") 
			{
				current.NextButton.Enabled = true;
				this.SQLPassword = current.InputBox.Text;
			}
			else 
			{
				current.ExampleLabel.Text = "";
				current.NextButton.Enabled = false;
			}
		}

		public void UpdateSQLDatabaseName(object o, System.EventArgs e) 
		{
			if (current.InputBox.Text != "") 
			{
				current.NextButton.Enabled = true;
				this.SQLDatabaseName = current.InputBox.Text;
			}
			else 
			{
				current.ExampleLabel.Text = "";
				current.NextButton.Enabled = false;
			}
		}

		public void UpdateSQLUserName(object o, System.EventArgs e) 
		{
			if (current.InputBox.Text != "") 
			{
				current.NextButton.Enabled = true;
				this.SQLUserName = current.InputBox.Text;
			}
			else 
			{
				current.ExampleLabel.Text = "";
				current.NextButton.Enabled = false;
			}
		}


		public void UpdateSQLInstanceName(object o, System.EventArgs e) 
		{
			if (current.InputBox.Text != "") 
			{
				current.NextButton.Enabled = true;
				current.ExampleLabel.Text = "Use the instance '"+current.InputBox.Text + "' on server '" + this.SQLServerName +"'.";
				this.SQLInstanceName = current.InputBox.Text;
			}
			else 
			{
				current.ExampleLabel.Text = "";
				current.NextButton.Enabled = false;
			}
		}


		public void UpdateSQLServerName(object o, System.EventArgs e) 
		{
			if (current.InputBox.Text != "") 
			{
				current.NextButton.Enabled = true;
				this.SQLServerName = current.InputBox.Text;
			}
			else 
			{
				current.NextButton.Enabled = false;
			}
		}

		public void GetServerName(object o, System.EventArgs e) 
		{
			NewForm();

			current.Title = "Picking a Server Name...";
			current.DescriptionBox.Text = "A WSEmail server will only save messages that are addressed to users on the local server. All other messages will be forwarded.\r\n\r\nPick the name of the server. You'll see an example of user addresses below.";
			current.DescriptionBox.Select(0,0);
			current.InputLabel.Text = "Server name:";
			current.InputBox.Text = this.ServerName;
			current.InputBox.Focus();
			current.InputBox.Visible = true;
			current.InputBox.TextChanged += new System.EventHandler(this.UpdateServerName);
			current.InputLabel.Visible = true;
			current.ExampleLabel.Visible = true;
			UpdateServerName(null,null);
			current.NextButton.Click += new System.EventHandler(this.GetRouterName);
			current.Show();
		}

		public void CheckRouterURL(object o, System.EventArgs e) 
		{
			try 
			{
				System.Net.WebClient w = new System.Net.WebClient();
				System.Text.Encoding.ASCII.GetString(w.UploadData(this.Router,"POST", System.Text.Encoding.ASCII.GetBytes("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Header>    <wsrp:path soap:actor=\"http://schemas.xmlsoap.org/soap/actor/next\" soap:mustUnderstand=\"1\" xmlns:wsrp=\"http://schemas.xmlsoap.org/rp\">      <wsrp:action>http://securitylab.cis.upenn.edu/WSEmail/WSEmailSend</wsrp:action>      <wsrp:to>http://localhost/WSEmailServer/MailServer.asmx</wsrp:to>      <wsrp:id>uuid:94737e96-dd96-4210-89e7-4c80ce37b549</wsrp:id>    </wsrp:path>    <wsu:Timestamp xmlns:wsu=\"http://schemas.xmlsoap.org/ws/2002/07/utility\">      <wsu:Created>2003-10-13T15:11:51Z</wsu:Created>      <wsu:Expires>2003-10-13T15:12:51Z</wsu:Expires>    </wsu:Timestamp>  </soap:Header>  <soap:Body>    <WSEmailSend xmlns=\"http://securitylab.cis.upenn.edu/WSEmail\">      <theMessage>        <Body>This is a test message!</Body>        <Subject>Testing...</Subject>        <Recipient>Kevin@MailServerA</Recipient>        <Sender>Kevin@MailServerA</Sender>        <Timestamp>Monday, October 13, 2003 11:11:48 AM</Timestamp>        <MessageFlags>0</MessageFlags>      </theMessage>    </WSEmailSend>  </soap:Body></soap:Envelope>")));
			} 
			catch (Exception x) 
			{
				
				if (x.Message.IndexOf("500") > 0) 
				{
					current.ExampleLabel.Text = "Web site found!";
					current.NextButton.Enabled = true;
					this.GetDNSServer(null,null);
				}
				else
					current.ExampleLabel.Text = "Error checking webserver: " + x.Message;

			}
		}

		public void CheckDNSServer(object o, System.EventArgs l) 
		{
			try 
			{
				System.Net.IPHostEntry e = System.Net.Dns.GetHostByName(this.DNSServer);
				if (e.AddressList.Length != 0) 
				{
					current.ExampleLabel.Text = "DNS Server seems OK";
					current.NextButton.Enabled = true;
					this.GetSQLServerName(null,null);
				}

			} 
			catch (Exception x) 
			{
					current.ExampleLabel.Text = "Error Resolving DNS Server: " + x.Message;
			}
		}


		public void UpdateRouterName(object o, System.EventArgs e) 
		{
			
			if (current.InputBox.Text != "") 
			{
				try 
				{
					Uri u = new Uri(current.InputBox.Text);
					current.ExampleLabel.Text = "Valid URL.";
					current.NextButton.Enabled = true;
					this.Router = current.InputBox.Text;
				} 
				catch 				{
					current.ExampleLabel.Text = "This is an invalid URL, please try again.";
				}

			}
			else 
			{
				current.ExampleLabel.Text = "Please input router URL above.";
				current.NextButton.Enabled = false;
			}
		}

		public void UpdateServerName(object o, System.EventArgs e) 
		{
			
			if (current.InputBox.Text != "") 
			{
				current.ExampleLabel.Text = "For example, postmaster@"+current.InputBox.Text;
				current.NextButton.Enabled = true;
			}
			else 
			{
				current.ExampleLabel.Text = "Please input server name above.";
				current.NextButton.Enabled = false;
			}

			this.ServerName = current.InputBox.Text;
		}

		public void UpdateDNSServer(object o, System.EventArgs e) 
		{
			
			if (current.InputBox.Text != "") 
			{
				current.NextButton.Enabled = true;
			}
			else 
			{
				// current.ExampleLabel.Text = "Please input server name above.";
				current.NextButton.Enabled = false;
			}

			this.DNSServer = current.InputBox.Text;
		}

	}
}
