/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System.IO;
using System;
using System.Security.Cryptography.X509Certificates;
using CAPICOM;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace InstallRootCA
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnInstallCA;
		private System.Windows.Forms.Button btnUNInstallCA;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnInstallCA = new System.Windows.Forms.Button();
			this.btnUNInstallCA = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnInstallCA
			// 
			this.btnInstallCA.Location = new System.Drawing.Point(16, 16);
			this.btnInstallCA.Name = "btnInstallCA";
			this.btnInstallCA.Size = new System.Drawing.Size(80, 32);
			this.btnInstallCA.TabIndex = 0;
			this.btnInstallCA.Text = "Install Root CA";
			this.btnInstallCA.Click += new System.EventHandler(this.btnInstallCA_Click);
			// 
			// btnUNInstallCA
			// 
			this.btnUNInstallCA.Location = new System.Drawing.Point(120, 16);
			this.btnUNInstallCA.Name = "btnUNInstallCA";
			this.btnUNInstallCA.Size = new System.Drawing.Size(80, 32);
			this.btnUNInstallCA.TabIndex = 1;
			this.btnUNInstallCA.Text = "UNInstall Root CA";
			this.btnUNInstallCA.Click += new System.EventHandler(this.btnUNInstallCA_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(216, 109);
			this.Controls.Add(this.btnUNInstallCA);
			this.Controls.Add(this.btnInstallCA);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Certificate Installer...";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private Certificate FindCertificate(StoreClass store, CertificateClass cert) 
		{
			Certificate ret = null;

			foreach( CAPICOM.Certificate currCert in store.Certificates ) 
			{
				Console.WriteLine(currCert.PublicKey().EncodedKey);
				if (currCert.Thumbprint.Equals(cert.Thumbprint)) 
				{
					ret = currCert;
					break;
				}
			}
			return ret;
		}

		private void btnInstallCA_Click(object sender, System.EventArgs e)
		{
			CAPICOM.StoreClass store = new StoreClass();
			store.Open(CAPICOM_STORE_LOCATION.CAPICOM_LOCAL_MACHINE_STORE,"Root",CAPICOM_STORE_OPEN_MODE.CAPICOM_STORE_OPEN_READ_WRITE);
			CertificateClass cert = GetCertificate();
			if (FindCertificate(store,cert) != null)
				MessageBox.Show("You already have this certificate installed!");
			else 
			{
				store.Add(cert);
				MessageBox.Show("Penn Security Lab root certificate has been added to the trusted store.","Success...");
			}

			store.CloseHandle(store.StoreHandle);
		}

		private CAPICOM.CertificateClass GetCertificate() 
		{
			byte[] certbytes = new byte[16384];
			Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("InstallRootCA.psl-ca.cer");
			s.Position = 0;
			s.Read(certbytes,0,(int)s.Length);

			Microsoft.Web.Services2.Security.X509.X509Certificate xc = new Microsoft.Web.Services2.Security.X509.X509Certificate(certbytes);
		
			CAPICOM.CertificateClass ccert = new CertificateClass();
			ccert.Import(xc.ToBase64String());
			return ccert;
		}

		private void btnUNInstallCA_Click(object sender, System.EventArgs e)
		{
			CAPICOM.StoreClass store = new StoreClass();
			store.Open(CAPICOM_STORE_LOCATION.CAPICOM_LOCAL_MACHINE_STORE,"Root",CAPICOM_STORE_OPEN_MODE.CAPICOM_STORE_OPEN_READ_WRITE);
			try 
			{
				Certificate c = FindCertificate(store,GetCertificate());
				if (c != null) 
				{
					store.Remove(c);
					MessageBox.Show("Penn Security Lab root certificate has been removed from the trusted store.","Success...");
				} else
					MessageBox.Show("The Penn Security Lab root certificate does not seem to be installed.","Oops!");
			} 
			catch (Exception ex) 
			{
				MessageBox.Show("Certificate not removed: " + ex.Message,"Oops!");
			}
			store.CloseHandle(store.StoreHandle);
			
		}
	}
}
