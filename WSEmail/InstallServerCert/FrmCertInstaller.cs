/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using CAPICOM;
using System;
using ACLs;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace InstallServerCert
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class FrmCertInstaller : System.Windows.Forms.Form
	{
		private System.Windows.Forms.OpenFileDialog fileDialog;
		private System.Windows.Forms.Button btnServerCert;
		private System.Windows.Forms.Button btnUNInstallCA;
		private System.Windows.Forms.Button btnInstallCA;
		private System.Windows.Forms.RichTextBox txtDescr;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrmCertInstaller()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			txtDescr.Text = "These buttons will help get your server communicating with the Penn Security Lab servers and in configuring the certificate for your server.";
			txtDescr.Text += "\r\n\r\nIf you plan on communicating with a Penn Security Lab server, please make sure our CA cert is trusted. Simply click on the \"Insert PSL Root CA\" button to install, or its uninstall equivalent to uninstall.";
			txtDescr.Text += "\r\n\r\nTo install a certificate for a WSEmail Server on the current machine, click the \"Server Certificate Installer\" button. This wizard will help import the certificate and set the correct file permissions for ASP.net to use it.";
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
			this.btnServerCert = new System.Windows.Forms.Button();
			this.fileDialog = new System.Windows.Forms.OpenFileDialog();
			this.btnUNInstallCA = new System.Windows.Forms.Button();
			this.btnInstallCA = new System.Windows.Forms.Button();
			this.txtDescr = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// btnServerCert
			// 
			this.btnServerCert.Location = new System.Drawing.Point(312, 208);
			this.btnServerCert.Name = "btnServerCert";
			this.btnServerCert.Size = new System.Drawing.Size(104, 48);
			this.btnServerCert.TabIndex = 0;
			this.btnServerCert.Text = "Server Certificate Installer";
			this.btnServerCert.Click += new System.EventHandler(this.button1_Click);
			// 
			// fileDialog
			// 
			this.fileDialog.Filter = "PFX file|*.pfx|CRT file|*.crt|All files|*.*";
			// 
			// btnUNInstallCA
			// 
			this.btnUNInstallCA.Location = new System.Drawing.Point(176, 208);
			this.btnUNInstallCA.Name = "btnUNInstallCA";
			this.btnUNInstallCA.Size = new System.Drawing.Size(104, 48);
			this.btnUNInstallCA.TabIndex = 3;
			this.btnUNInstallCA.Text = "UNInstall PSL Root CA Cert";
			this.btnUNInstallCA.Click += new System.EventHandler(this.btnUNInstallCA_Click);
			// 
			// btnInstallCA
			// 
			this.btnInstallCA.Location = new System.Drawing.Point(40, 208);
			this.btnInstallCA.Name = "btnInstallCA";
			this.btnInstallCA.Size = new System.Drawing.Size(104, 48);
			this.btnInstallCA.TabIndex = 2;
			this.btnInstallCA.Text = "Install PSL Root CA Cert";
			this.btnInstallCA.Click += new System.EventHandler(this.btnInstallCA_Click);
			// 
			// txtDescr
			// 
			this.txtDescr.Location = new System.Drawing.Point(0, 0);
			this.txtDescr.Name = "txtDescr";
			this.txtDescr.ReadOnly = true;
			this.txtDescr.Size = new System.Drawing.Size(456, 168);
			this.txtDescr.TabIndex = 4;
			this.txtDescr.Text = "richTextBox1";
			// 
			// FrmCertInstaller
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(450, 285);
			this.Controls.Add(this.txtDescr);
			this.Controls.Add(this.btnUNInstallCA);
			this.Controls.Add(this.btnInstallCA);
			this.Controls.Add(this.btnServerCert);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "FrmCertInstaller";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Certificate Installer";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new FrmCertInstaller());
		}


		private CAPICOM.CertificateClass LoadCertificate(string  f) 
		{
			try 
			{
				CAPICOM.CertificateClass c = new CAPICOM.CertificateClass();
				try 
				{
					c.Load(f,"",CAPICOM.CAPICOM_KEY_STORAGE_FLAG.CAPICOM_KEY_STORAGE_DEFAULT,CAPICOM.CAPICOM_KEY_LOCATION.CAPICOM_LOCAL_MACHINE_KEY);
				} 
				catch  
				{
					string pass = PennLibraries.InputBox.ShowInputBox("What is the password on this certificate?",true);
					try 
					{
						c.Load(f,pass,CAPICOM.CAPICOM_KEY_STORAGE_FLAG.CAPICOM_KEY_STORAGE_DEFAULT,CAPICOM.CAPICOM_KEY_LOCATION.CAPICOM_LOCAL_MACHINE_KEY);
					} 
					catch 
					{
						MessageBox.Show("Unable to load the certificate with the password you specified.","Oops!");
						return null;
					}
				}
				return c;
			} 
			catch (Exception e) 
			{
				MessageBox.Show("Unable to load certificate. Exception: " + e.Message,"Oops");
			}
			return null;
			
		}

		private bool CheckCertificateStatus(CAPICOM.CertificateClass c) 
		{
			try 
			{
				CAPICOM.ICertificateStatus status = c.IsValid();
				if (!status.Result) 
				{
					status.CheckFlag = CAPICOM.CAPICOM_CHECK_FLAG.CAPICOM_CHECK_TRUSTED_ROOT;
					if (!status.Result)
						MessageBox.Show("This certificate is not issued from a trusted root!","Oops!");

					status.CheckFlag = CAPICOM.CAPICOM_CHECK_FLAG.CAPICOM_CHECK_TIME_VALIDITY;
					if (!status.Result)
						MessageBox.Show("This certificate has expired or is not valid yet!","Oops!");

					return false;
				} 
				else
					return true;
			} 
			catch (Exception e) 
			{
				MessageBox.Show("Unable to check certificate status. Exception: " + e.Message, "Oops!");
				return false;
			}
		}

		private bool AddToStore(CAPICOM.CertificateClass c) 
		{
			MessageBox.Show("Adding this certificate (or updating if it already exists):\n\n"+c.SubjectName +"\n\nto Local Computer Store\\Personal certificate store.","Notification");
			try 
			{
				CAPICOM.StoreClass s = new CAPICOM.StoreClass();
				s.Open(CAPICOM.CAPICOM_STORE_LOCATION.CAPICOM_LOCAL_MACHINE_STORE,"My",CAPICOM.CAPICOM_STORE_OPEN_MODE.CAPICOM_STORE_OPEN_READ_WRITE);
				foreach (CAPICOM.ICertificate cert in s.Certificates) 
				{
					if (c.Thumbprint.Equals(cert.Thumbprint)) 
						s.Remove(c);
				}
				s.Add(c);
				return true;
			} 
			catch (Exception e) 
			{
				MessageBox.Show("Failed to add certificate to the store. Exception: " + e.Message , "OOps!");
				return false;
			}
		}

		private string GetFileKey() 
		{
			try 
			{
				string key = "";
				DateTime t = DateTime.MinValue;
				string[] files = Directory.GetFiles(@"C:\Documents and Settings\All Users\Application Data\Microsoft\Crypto\RSA\MachineKeys\","*.*");
				foreach (string f in files) 
				{
					DateTime temp = Directory.GetLastWriteTime(f);
					if (temp.CompareTo(t) > 0) 
					{
						t = temp;
						key = f;
					}
				}
				return key;
			} 
			catch (Exception e) 
			{
				MessageBox.Show("Unable to get file key name. Exception: " + e.Message, "Oops!");
				return "";
			}
		}

		private bool SetACL(string user, string file) 
		{
			try 
			{
				SecurityDescriptor secDesc = SecurityDescriptor.GetFileSecurity (file, SECURITY_INFORMATION.DACL_SECURITY_INFORMATION); 

				Dacl dacl = secDesc.Dacl; 
				dacl.AddAce (new AceAccessAllowed (new Sid (user), AccessType.GENERIC_READ,AceFlags.NO_PROPAGATE_INHERIT_ACE)); 

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

		private bool ProcessCertificate() 
		{
			DialogResult r = fileDialog.ShowDialog();
			if (r==DialogResult.OK) 
			{
				CAPICOM.CertificateClass c = LoadCertificate(fileDialog.FileName);
				if (c == null) return false;

				if (!CheckCertificateStatus(c)) return false;

				if (!AddToStore(c)) return false;

				string file = GetFileKey();
				if (file.Length == 0) return false;
				System.OperatingSystem v = Environment.OSVersion;
				if (v.Version.Minor == 2) 
					// 2003 server
					return SetACL("Network Service",file);
				else 
					return SetACL("ASPNET",file);
				
			}
			return false;
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			if (ProcessCertificate())
				MessageBox.Show("The certificate has been imported to your certificate store and file permissions have been set on it to allow its use from ASP.net","YEAH!");
			else
				MessageBox.Show("The certificate was not correctly imported due to the errors shown. You should try again.","Oops!");
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
			Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("InstallServerCert.psl-ca.cer");
			s.Position = 0;
			s.Read(certbytes,0,(int)s.Length);

			CAPICOM.CertificateClass ccert = new CertificateClass();
			ccert.Import(Convert.ToBase64String(certbytes));
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
				} 
				else
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

