/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System.Threading;
using System.Reflection;
using System.Configuration;
using Microsoft.Web.Services2.Configuration;
using System;
using System.Collections;
using System.Data.SqlClient;
using Microsoft.Web.Services2;
using Microsoft.Web.Services2.Security;
using Microsoft.Web.Services2.Security.Tokens;
using Microsoft.Web.Services2.Security.X509;
//using System.Runtime.Remoting.Channels.Http;
//using System.Runtime.Remoting.Channels;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
//using EventQueue;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using System.IO;
using System.Globalization;
//using CAPICOM;
using System.Runtime.InteropServices;

namespace PennLibraries
{
	/// <summary>
	/// Provides a variety of functions common to all the webapps, client apps and filters.
	/// </summary>
	/// 
	public enum LogType {Unknown, Error, MessageSent, MessageReceived, UserAuthentication, UserAuthenticationError,
		Debug, MessageAuthentication, MessageAuthenticationError, ServerInfo, ServerDebug, Informational,
		ServerStatus, ServerDeliveryStatus, ServerError, ServerWarning, RequestStart, RequestEnd};

	public class Utilities
	{
		/// <summary>
		/// Default constructor. Doesn't do anything. All functions are static, so don't create
		/// this as an object.
		/// </summary>
		private static bool DBLogChecked = true;
		private static bool UseLogDB = false;
		private static bool UseLogFile = true;
		private const string DBCONF = @"C:\DBLogging.xml";
		private static SqlConnection sqlconn = null;

		private static void CheckDBLogWanted() 
		{
			DBLogChecked = true;
			try 
			{
				object o = ConfigurationSettings.GetConfig("Database");
				if (o != null && o is DatabaseConfiguration) 
				{
					sqlconn = new SqlConnection(((DatabaseConfiguration)o).connstr);
					UseLogDB = true;
					return;
				}
			} 
			catch 
			{
				//TODO I want to know if i can get the config...
				try 
				{
					if (File.Exists(DBCONF)) 
										{
											XmlDocument d = new XmlDocument();
											d.Load(DBCONF);
											DatabaseConfigurationReader conf = new DatabaseConfigurationReader();
											XmlNodeList l = d.GetElementsByTagName("Database");
											if (l.Count > 0) 
											{
												object o = conf.Create(null,null,l[0]);
												if (o is DatabaseConfiguration) 
												{
													sqlconn = new SqlConnection(((DatabaseConfiguration)o).connstr);
													UseLogDB = true;
													return;
												}
											}
										}
			
				} 
				catch 
				{
					UseLogDB = false;
				}
			
			}	
		}

		public static Hashtable CertificateCache = new Hashtable();
		private static string _myid = null;

		public static string MyIdentifer 
		{
			get 
			{
				if (_myid == null)
					_myid = ConfigurationSettings.AppSettings["SigningCertificate"];

				return _myid;
			}
			set 
			{
				_myid = value;
			}
		}
		public static IPostMessages MessageForm = null;

		public static bool VerifyCertificate(Microsoft.Web.Services2.Security.X509.X509Certificate c) 
		{
			return VerifyCertificate(c.ToBase64String());
		}

		public static bool VerifyCertificate(Microsoft.Web.Services.Security.X509.X509Certificate c) 
		{
			return VerifyCertificate(c.ToBase64String());
		}

		public static bool VerifyCertificate(string s) 
		{
			CAPICOM.Certificate cert = new CAPICOM.CertificateClass();
			cert.Import(s);
			CAPICOM.ICertificateStatus status = cert.IsValid();
			LogEvent(LogType.UserAuthentication,"Certificate is valid and trusted: " + status.Result.ToString());
			return status.Result;
		}

		public static void LogToStatusWindow(string s) 
		{
			if (MessageForm != null && ((System.Windows.Forms.Form)MessageForm).IsDisposed == false && ((System.Windows.Forms.Form)MessageForm).Disposing == false)
				MessageForm.PostMessage(s);
		}

		public static void printXmlObject(Object o)
		{
			XmlSerializer xs = new XmlSerializer(o.GetType());
			MemoryStream ms = new MemoryStream();

			xs.Serialize(ms,o);
			ms.Position = 0;

			XmlDocument d = new XmlDocument();
			d.PreserveWhitespace = false;
			d.Load(ms);

			String filename = o.GetType().ToString() + ".xml";

			FileStream fs = new FileStream("C:\\" + filename,FileMode.Create);
			
            fs.Position = 0;
			
			StreamWriter sw = new StreamWriter(fs);
			sw.WriteLine(d.OuterXml);
			
			sw.Close();
			fs.Close();
			
		}

		#region CertificateManipulations

		/// <summary>
		/// Given a certificate, this function returns the email address bound in it.
		/// </summary>
		/// <param name="c">Certificate</param>
		/// <returns>Email address</returns>
		public static string GetCertEmail(X509Certificate c) 
		{
			CAPICOM.Certificate cert = new CAPICOM.CertificateClass();
			cert.Import(c.ToBase64String());
			return cert.GetInfo(CAPICOM.CAPICOM_CERT_INFO_TYPE.CAPICOM_CERT_INFO_SUBJECT_EMAIL_NAME);
		}

		public static string GetCertCN(X509Certificate c) 
		{
			if (c != null) 
			{
				try 
				{
					CAPICOM.Certificate cert = new CAPICOM.CertificateClass();
					cert.Import(c.ToBase64String());
					return cert.GetInfo(CAPICOM.CAPICOM_CERT_INFO_TYPE.CAPICOM_CERT_INFO_SUBJECT_SIMPLE_NAME);
				} 
				catch (Exception e) 
				{
					LogEvent(LogType.Error,"GetCertCN failed on a non-null certificate. This is most likely due to CAPICOM v2 not being installed. " + e.Message);
					throw new Exception("GetCertCN failed on a non-null certificate. This is most likely due to CAPICOM v2 not being installed.");
				}
			}
			return null;
		}

		/// <summary>
		/// Gets the certificate specified by the given CN. If Machine store is true, it assumes it should
		/// look in the LocalMachineStore, otherwise the CurrentUserStore.
		/// </summary>
		/// <param name="certKeyID">CN of the certificate</param>
		/// <param name="MachineStore">True: use LocalMachineStore, false: use CurrentUserStore</param>
		/// <returns>Certificate or throws an exception</returns>
		public static X509SecurityToken GetSecurityToken(string certKeyID, bool MachineStore)
		{            
			if (CertificateCache["cert-"+certKeyID+MachineStore.ToString()] != null) 
			{
				Console.WriteLine("Cache hit for cert CN " + certKeyID);
				return (X509SecurityToken)CertificateCache["cert-"+certKeyID+MachineStore.ToString()];
			}
			X509SecurityToken securityToken = null;  
			//
			// open the current user's certificate store
			//
			X509CertificateStore store;
			//LogEvent("Fetching cert with CN = " + certKeyID + ", Machinestore: " + MachineStore.ToString());
			if (!MachineStore) 
				store = X509CertificateStore.CurrentUserStore(X509CertificateStore.MyStore);
			else
				store = X509CertificateStore.LocalMachineStore(X509CertificateStore.MyStore);

			bool open = store.OpenRead();

			try 
			{
				// try to find the certificate.
				X509Certificate cert = null;
				X509CertificateCollection matchingCerts = store.FindCertificateBySubjectString(certKeyID);

				if (matchingCerts.Count == 0)
				{
					LogEvent(LogType.Error,"No matching certificates were found for the key ID provided in " + store.Location.ToString()+" store. Please run the configuration utility to guide you through client and server setup.");
					throw new ApplicationException("No matching certificates were found for the key ID provided in " + store.Location.ToString()+" store. Please run the configuration utility to guide you through client and server setup.");
				}
				else 
				{
					if (matchingCerts.Count > 1)
						LogEvent(LogType.Informational, "Warning: Multiple certificates found for CN = " + certKeyID + ". ("+matchingCerts.Count.ToString() + ")");
					// pick the first one arbitrarily
					foreach (X509Certificate c in matchingCerts) 
					{
						if (GetCertCN(c).Equals(certKeyID))
							cert = c;
					}
				}
				
				if (cert == null) 
				{
					//	throw new ApplicationException("You chose not to select an X509 certificate for signing your messages.");
					//}
					Console.WriteLine("No cert selected.");
					return null;
				}
					// we need the digital sign and a private key.
				else if (!cert.SupportsDigitalSignature || cert.Key == null ) 
				{
					LogEvent(LogType.Error,"The certificate must support digital signatures and have a private key available.");
					throw new ApplicationException("The certificate must support digital signatures and have a private key available.");
				}
				else 
				{
//					byte[] keyId = cert.GetKeyIdentifier();
//					Console.WriteLine("Key Name                       : {0}", cert.GetName());
//					Console.WriteLine("Key ID of Certificate selected : {0}", Convert.ToBase64String(keyId));
					securityToken = new X509SecurityToken(cert);
				}
			} 
			catch (Exception e) 
			{
				LogEvent(LogType.Error, "Unable to load certificate... error: " + e.Message);
			}
			finally 
			{
				if (store != null) { store.Close(); }
			}            
			CertificateCache["cert-"+certKeyID+MachineStore.ToString()] = securityToken;
//			ConfigurationSettings.AppSettings["cert-"+certKeyID+MachineStore.ToString()] = securityToken;

			return securityToken;            
		}
		#endregion
		
		#region LogFileManipulations
		/// <summary>
		/// Returns the current date/time.
		/// </summary>
		/// <returns></returns>
		public static string GetCurrentTime() 
		{
			return DateTime.Now.ToString("F",DateTimeFormatInfo.CurrentInfo);

		}

		public static void LogEvent(string s) 
		{
			LogEvent(LogType.Unknown,s);
		}

		/// <summary>
		/// Logs a message to the logfile ("C:\Temp\Logfile") along with the current time and a seperator.
		/// </summary>
		/// <param name="s">Message to log</param>
		public static void LogEvent (LogType logtype, string s) 
		{
			if (!DBLogChecked)
				CheckDBLogWanted();

			if (!UseLogFile && !UseLogDB)
				return;
			//return;
			string output = "";
			if (s.IndexOf("</") > 0) 
			{
				// we have XML.
				string theStuff = "";
				int i,j;
				i = s.IndexOf("<");
				j = s.LastIndexOf(">");
				theStuff = s.Substring(i,j-i+1);
				if (!UseLogDB)
					s = s.Substring(0,i) + " << formatted xml follows >> " + s.Substring(j+1,s.Length - j - 1);
				else
					s = s.Substring(0,i) + "\r\n"+ s.Substring(j+1,s.Length - j - 1);
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(theStuff);
				MemoryStream m = new MemoryStream(theStuff.Length);
				XmlTextWriter x = new XmlTextWriter(m,System.Text.Encoding.ASCII);
				x.Formatting = System.Xml.Formatting.Indented;

				
				foreach (string remme in new string[] {"X509Certificate","SignatureValue","DigestValue","wsse:BinarySecurityToken"})
				{
					XmlNodeList xnl = doc.GetElementsByTagName(remme);
					for (int freakinvar = 0; freakinvar < xnl.Count; freakinvar++) 
					{
						XmlNode newone = xnl[freakinvar].Clone();
						string tt = xnl[freakinvar].InnerText;
						if (tt.Length > 0) 
						{
							tt = tt.Remove(8,tt.Length - 16);
							tt = tt.Insert(8," ... ");
							newone.InnerText = tt;
							xnl[freakinvar].ParentNode.ReplaceChild(newone,xnl[freakinvar]);
						}
					}
				}


			{
				XmlNodeList xnl = doc.GetElementsByTagName("WSEmailFetchHeadersResult");
				if (xnl.Count > 0 && xnl[0].ChildNodes.Count > 3) 
				{
					XmlNode parent = xnl[0];
				
					xnl = parent.ChildNodes;
					int y = xnl.Count;
					for (i = 1; i < y - 1; i++) 
					{
						xnl[1].ParentNode.RemoveChild(xnl[1]);
					}
					XmlNode newnode = doc.ImportNode(doc.CreateNode(XmlNodeType.Element,"Filler","http://filler/"),true);
					newnode.InnerText = "\n\r\r\n\t Trimmed because this can scroll for pages (" + (y - 2).ToString()+" nodes removed)\n\n\t";
					xnl[0].ParentNode.InsertAfter(newnode,xnl[0]);
				}

			}

				doc.WriteContentTo(x);
				x.Flush();
			
				while (output.Length < m.Length)
					output += System.Text.Encoding.ASCII.GetString(m.GetBuffer());

				output = output.Trim('\0');
			}

			//GUILog(new EventItem(GetCurrentTime(),MyType,s,output));


			if (UseLogDB) 
			{
				lock(sqlconn) 
				{
					try 
					{
						sqlconn.Open();
						SqlCommand comm = sqlconn.CreateCommand();
						comm.CommandText = "WSEmailLog";
						comm.CommandType = System.Data.CommandType.StoredProcedure;
						comm.Parameters.Add("@date",DateTime.Now);
					
						string ent = ConfigurationSettings.AppSettings["SigningCertificate"];
					
						if (ent == null || ent.Equals(""))
							ent = "Unknown";

						comm.Parameters.Add("@entity", ent);
						comm.Parameters.Add("@logtype", logtype);
						comm.Parameters.Add("@message", s);
						comm.Parameters.Add("@xmlmessage",output);
						comm.ExecuteNonQuery();
					} 
					catch {
						UseLogDB = false;
					}
					finally 
					{
						sqlconn.Close();
					}
				}
			} 

			if (!UseLogDB && UseLogFile)
			{

				FileStream fs = null;
				int k = 0;
				while (fs == null && k < 10) 
				{
					try 
					{
						fs = new FileStream("C:\\WSEmail-LogFile.txt",FileMode.Append);
					} 
					catch 
					{
						k++;
						Thread.Sleep(500);
					}
				}
				if (fs == null) 
				{
					UseLogFile = false;
					return;
				}
				StreamWriter sw = new StreamWriter(fs);
				sw.WriteLine("---------------------------------------------------------------");
				sw.WriteLine("Log type: " + logtype);
				if (logtype == LogType.Unknown) 
				{
					System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
					sw.WriteLine("Stack: " + st.ToString());
				}
				sw.WriteLine("Identity: " + ConfigurationSettings.AppSettings["SigningCertificate"]);
				sw.WriteLine("Date: " + GetCurrentTime());
				sw.WriteLine(s);
				if (output.Length > 0)
					sw.WriteLine("Formatted XML Message:\n" + output);
				sw.Close();
				fs.Close();
			}
		}

		#endregion
	
		#region FilterManipulations

/*
		/// <summary>
		/// Adds the PennRoutingFilters to the current configuration. Set UseMachineStore = true
		/// when calling this method from a webservice or other entity that isn't currently logged in.
		/// </summary>
		/// <param name="UseMachineStore">True: use machine store, false: use currently logged in store</param>
		public static void AddPennRoutingFilters(bool UseMachineStore) 
		{
			SoapInputFilterCollection defaultInputFilters = WebServicesConfiguration.FilterConfiguration.InputFilters;
			defaultInputFilters.Insert(0,new PennRoutingInputFilter());

			SoapOutputFilterCollection defaultOutputFilters = WebServicesConfiguration.FilterConfiguration.OutputFilters;
			defaultOutputFilters.Insert(0,new PennRoutingOutputFilter(ConfigurationSettings.AppSettings["SigningCertificate"],UseMachineStore));

		}

		public static void AddWSEMailSignatureFilters(bool SignMessages) 
		{
			SoapOutputFilterCollection defaultOutputFilters = WebServicesConfiguration.FilterConfiguration.OutputFilters;
			defaultOutputFilters.Insert(1,new PennWSEmailSignatureOutputFilter(ConfigurationSettings.AppSettings["SigningCertificate"],SignMessages));
		}


		public static void AddWSEMailSignatureFilters(bool SignMessages, bool UseMachineStore) 
		{
			SoapOutputFilterCollection defaultOutputFilters = WebServicesConfiguration.FilterConfiguration.OutputFilters;
			defaultOutputFilters.Insert(1,new PennWSEmailSignatureOutputFilter(ConfigurationSettings.AppSettings["SigningCertificate"],SignMessages,UseMachineStore));
		}
		
		/// <summary>
		/// Removes the WSEMailSignature filter from the output filters collection.
		/// </summary>
		public static void RemoveWSEMailSignatureFilters() 
		{
			SoapOutputFilterCollection defaultOutputFilters = WebServicesConfiguration.FilterConfiguration.OutputFilters;
			defaultOutputFilters.Remove(typeof(PennWSEmailSignatureOutputFilter));
		}
*/

		/// <summary>
		/// Adds the PennRoutingFilters to the current configuration. Set UseMachineStore = true
		/// when calling this method from a webservice or other entity that isn't currently logged in.
		/// </summary>
		/// <param name="UseMachineStore">True: use machine store, false: use currently logged in store</param>
		public static void AddPennLoggingFilters() 
		{
			SoapInputFilterCollection defaultInputFilters = WebServicesConfiguration.FilterConfiguration.InputFilters;
			defaultInputFilters.Insert(0,new InputLoggingFilter());

			SoapOutputFilterCollection defaultOutputFilters = WebServicesConfiguration.FilterConfiguration.OutputFilters;
			defaultOutputFilters.Insert(0,new OutputLoggingFilter());
		}

		/// <summary>
		/// Adds the WSEmailSignatureFilter to the output collection. Typically clients will be called with SignedMessages = true to
		/// enable them to sign messages.
		/// </summary>
		/// <param name="SignMessages">Whether or not this filter should sign messages</param>


		#endregion

	}

}
