/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Data;
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
	/// Summary description for WSBaseConfiguration.
	/// </summary>
	public abstract class WSBaseConfiguration
	{
		public WSBaseConfiguration()
		{
		}

		public abstract void Configure();

		public string FileName;
		public string SigningCertificate;
		protected X509CertificateStore store;
		protected XmlDocument doc;

		protected PromptingForm current;
		protected ArrayList pastForms = new ArrayList();
		public delegate void EventDelegate(object o, System.EventArgs e);
		protected EventDelegate NextSub;
		protected int spot = -1;

		public void GoBack(object o, System.EventArgs e) 
		{
			spot--;
			MessageBox.Show("Spot="+spot.ToString());
			current.Hide();
			current = (PromptingForm)pastForms[spot];
			current.Show();
			
		}

		public void NewForm() 
		{
			
			spot++;
			//MessageBox.Show("Spot="+spot.ToString());
			if (spot + 1 <= pastForms.Count) 
			{
					current.Hide();
					current = (PromptingForm)pastForms[spot];
					current.Show();
			}
			else 
			{
				if (current != null) 
				{			
					pastForms.Add(current);
					current.Hide();
				}
				current = new PromptingForm();
				if (spot >= 1) 
				{
				//	current.BackButton.Enabled = true;
					current.BackButton.Click += new EventHandler(this.GoBack);
				}

			}

			current.InputBox.Focus();
		}

		public virtual void GetFileName(object o, System.EventArgs e) 
		{
			NewForm();
			current.Title = "File to Use...";
			current.DescriptionBox.Text = "In order to update an old configuration, we need to know the location of your existing configuration.\r\n\r\nType in the name of the file below, or click on the 'find' button to search for it.";
			current.InputLabel.Text = "File name:";
			current.InputButton.Click += new System.EventHandler(this.ShowFileDialog);
			current.InputButton.Text = "Browse...";
			current.InputBox.Visible = true;
			current.InputButton.Visible = true;
			current.InputLabel.Visible = true;
			current.NextButton.Click += new System.EventHandler(NextSub);
			current.NextButton.Enabled = false;
			current.InputBox.TextChanged += new EventHandler(UpdateFileName);
			current.Show();
		}

		public void UpdateFileName(object o, System.EventArgs e) 
		{
			if (current.InputBox.Text != "") 
			{
				current.NextButton.Enabled = true;
				this.FileName = current.InputBox.Text;
			}
			else 
			{
				current.NextButton.Enabled = false;
			}
		}

		public void SaveConfiguration() 
		{
			current.DescriptionBox.Text = "Configuration saved!\r\n\r\nThe specified configuration has been created and saved.";
			XmlTextWriter xtw = new XmlTextWriter(this.FileName,System.Text.Encoding.ASCII);
			doc.PreserveWhitespace=true;
			doc.Save(xtw);
			xtw.Close();
		}

		public void ShowFileDialog(object o, System.EventArgs e) 
		{
			OpenFileDialog f = new OpenFileDialog();
			
			f.DefaultExt = "config";
			f.Filter = "ASP.Net Configs (*.config)|*.config|XML Files (*.xml)|*.xml|All files (*.*)|*.*";
			f.FilterIndex = 0;
			if (System.IO.Directory.Exists(@"c:\inetpub\wwwroot"))
				f.InitialDirectory = @"c:\inetpub\wwwroot\";
			else
				f.InitialDirectory = ".";

			f.CheckFileExists = false;
			f.Title = "Select file to open/save to...";
			System.Windows.Forms.DialogResult r = f.ShowDialog();

			if (r == DialogResult.OK) 
			{
				this.FileName = f.FileName;
				current.InputBox.Text = f.FileName;
			}
	
		}

		public string ShowCertDialog(X509CertificateStore c) 
		{
			StoreDialog s = new StoreDialog(c);
			X509Certificate cert =  s.SelectCertificate(IntPtr.Zero,"Select a certificate...","");
			if (cert != null) 
			{
				CAPICOM.Certificate cc = new CAPICOM.CertificateClass();
				cc.Import(cert.ToBase64String());
				return cc.GetInfo(CAPICOM.CAPICOM_CERT_INFO_TYPE.CAPICOM_CERT_INFO_SUBJECT_SIMPLE_NAME);
			}
			return "";
		}
	}
}
