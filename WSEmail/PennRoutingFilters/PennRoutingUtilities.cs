using System.Threading;
using System.Reflection;
using System.Configuration;
using Microsoft.Web.Services.Configuration;
using System;
using System.Collections;
using Microsoft.Web.Services;
using Microsoft.Web.Services.Security;
using Microsoft.Web.Services.Security.X509;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels;
using System.Xml;
using System.Net;
using EventQueue;
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

	public class PennRoutingUtilities
	{
		/// <summary>
		/// Default constructor. Doesn't do anything. All functions are static, so don't create
		/// this as an object.
		/// </summary>
		public PennRoutingUtilities(){ }

		private static bool GUILogChecked = false;
		private static bool UseGUILog = false;

		private static void CheckGUILogWanted() 
		{
			UseGUILog = File.Exists(@"C:\GUILog");
			GUILogChecked = true;
		}

		public static void GUILog(EventItem p) 
		{
			if (!GUILogChecked) 
			{
				CheckGUILogWanted();
			}

			if (GUILogChecked && !UseGUILog)
				return;

			p.Date = GetCurrentTime();
			if (theBuffer == null || REMOTINGCHANNEL == null) {
				REMOTINGCHANNEL = new HttpChannel();
				try 
				{
					ChannelServices.RegisterChannel(REMOTINGCHANNEL);
				} 
				catch {}
				theBuffer = (MessageBuffer)Activator.GetObject(typeof(MessageBuffer),"http://localhost:8787/LogSink");
			}
			theBuffer.postMessage(p);
			
		}


		private static EventItem.RecordingEntities _mytype = EventItem.RecordingEntities.Unknown;
		public static EventItem.RecordingEntities MyType 
		{
			get 
			{
				if (_mytype == EventItem.RecordingEntities.Unknown) 
				{
					switch(ConfigurationSettings.AppSettings["SigningCertificate"].ToLower()) 
					{
						case "mailservera":
							_mytype = EventItem.RecordingEntities.MailA;
							break;
						case "mailserverb":
							_mytype = EventItem.RecordingEntities.MailB;
							break;
						case "router1":
							_mytype = EventItem.RecordingEntities.Router;
							break;
						case "kevina":
							_mytype = EventItem.RecordingEntities.ClientA;
							break;
						case "kevinb":
							_mytype = EventItem.RecordingEntities.ClientB;
							break;
						case "mailqueue":
							_mytype = EventItem.RecordingEntities.Queue;
							break;
					}
				}
				return _mytype;
			}
		}
		private static HttpChannel REMOTINGCHANNEL = null;
		private static MessageBuffer theBuffer = null;
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

		
		/// <summary>
		/// Removes the message signature from the request context.
		/// </summary>
		public static void RemoveWSEmailSignature() 
		{
			HttpSoapContext.RequestContext.Remove("MessageSignature");
		}

		public static bool VerifyCertificate(X509Certificate c) 
		{
			CAPICOM.Certificate cert = new CAPICOM.CertificateClass();
			cert.Import(c.ToBase64String());
			CAPICOM.ICertificateStatus status = cert.IsValid();
			LogEvent("Certificate is valid and trusted: " + status.Result.ToString());
			return status.Result;
				
		}

		public static void LogToStatusWindow(string s) 
		{
			if (MessageForm != null && ((System.Windows.Forms.Form)MessageForm).IsDisposed == false && ((System.Windows.Forms.Form)MessageForm).Disposing == false)
				MessageForm.PostMessage(s);
		}

		public static EventItem.RecordingEntities GetDestinationID(string s) 
		{
			if (s.StartsWith("http")) 
			{
				if (s.IndexOf("MailServer2") > 0) 
					return EventItem.RecordingEntities.MailB;
				else if (s.IndexOf("MailServer") > 0)
					return EventItem.RecordingEntities.MailA;
				else
					return EventItem.RecordingEntities.Router;
			}
			if (s.StartsWith("client")) 
			{
				if (s.IndexOf("a") > 0)
					return EventItem.RecordingEntities.ClientA;
				else
					return EventItem.RecordingEntities.ClientB;
			}
			return EventItem.RecordingEntities.Queue;
		}

		public static void LogTransit(string dest, string action) 
		{
			LogTransit(dest,(TransitItem.Actions)Enum.Parse(typeof(TransitItem.Actions),action));
		}
		public static void LogTransit(string dest, TransitItem.Actions action) 
		{
			LogTransit(dest,action,"");
		}
		public static void LogTransit(string dest, TransitItem.Actions action,string reason) 
		{
			// GUILog(new EventItem(GetCurrentTime(),MyType,"I was called.",outbound.ToString()));
			return;
			EventItem.RecordingEntities v;
			
					
			string build = "";
			string temp = Enum.GetName(typeof(EventItem.RecordingEntities),MyType);
			if (temp.IndexOf("Mail") >= 0 || temp.IndexOf("Queue") >= 0) 
			{
				build = temp;
				temp = "";
				//output = "1";
				if (action != TransitItem.Actions.Erase)
					action = TransitItem.Actions.From;
			}

			if (dest == "" ) 
			{
				if (build.ToLower().Equals("maila"))
					temp = "ClientA";
				else if (build.ToLower().Equals("mailb"))
					temp = "ClientB";
				else
					temp = "Queue";
			} 
			else 
			{
				if (dest.ToLower().IndexOf("mailserver/mailserver.asmx") >= 0)
					build = "MailA";
				else if (dest.ToLower().IndexOf("mailserver2") >= 0)
					build = "MailB";
				else if (dest.ToLower().IndexOf("router") >= 0)
					temp = "Router";
				else if (dest.ToLower().IndexOf("queue") >= 0)
					temp = "Queue";
			}
			
			build = build + temp;
			//GUILog(new EventItem(GetCurrentTime(),MyType,"build = " + build+", dest=" + dest,"moo"));
			v = (EventItem.RecordingEntities)Enum.Parse(typeof(EventItem.RecordingEntities),build);
			GUILog(new TransitItem(GetCurrentTime(),v,action,reason));
			//GUILog(new EventItem(GetCurrentTime(),v,"mememe",outbound.ToString()));
		}

		#region SignatureVerificationFunctions
		/// <summary>
		/// Verifies the signature of a WSEmailMessage. It assumes it can find the message in HttpSoapContext.RequestContext["MessageSignature"].
		/// </summary>
		/// <returns></returns>
		public static bool VerifyWSEmailMessageSignature(XmlElement xe) 
		{
			// XmlElement xe = (XmlElement)HttpSoapContext.RequestContext["MessageSignature"];
			SoapEnvelope env = HttpSoapContext.RequestContext.Envelope;
			
			if (xe == null) 
			{
				PennRoutingUtilities.LogEvent("VerifyWSEmailMessageSignature() : Unable to verify WSEmail Message signature... no signature found!\nMy identity is CN = " + ConfigurationSettings.AppSettings["SigningCertificate"]);
				return false;
			}
			
			PennRoutingUtilities.LogEvent("VerifyWSEmailMessageSignature() : Verifying WSEmail Message signature...\nMy identity is CN = " + ConfigurationSettings.AppSettings["SigningCertificate"] + "\nInput: "+xe.OuterXml+"\n"+"Onward to verifying...");
			// get a handle to the penn signature.
			// and the signature element within that.
			XmlElement sig = (XmlElement)xe.GetElementsByTagName("Signature")[0];

			// clone the entire  structure
			// this will be sent to the verification server.
			XmlElement toSend = (XmlElement)xe.Clone();

			// recreate the original signme node (this will be used in the signature verification.
			XmlElement toCheck = (XmlElement)env.CreateNode(XmlNodeType.Element,"","SignMe","http://securitylab.cis.upenn.edu/RoutingSignature/");
			// the node's value is the message, which was signed by the previous
			// hop.
			toCheck.InnerText = ((XmlElement)env.Body.GetElementsByTagName("theMessage")[0]).InnerXml;

			// recreate the data object.
			DataObject dO = new DataObject("MessageSignature","","",(XmlElement)toCheck);
			// since tosend is a clone of the penn signature,
			// find the signature element within that,
			// append the data object
			(toSend.GetElementsByTagName("Signature")[0]).AppendChild(env.ImportNode(dO.GetXml(),true));

			// at this point, the toSend xml has been recreated as it was on the routing output filter.



			// verify the signature and return the signing cert
			Microsoft.Web.Services.Security.X509.X509Certificate signingCert = PennRoutingUtilities.VerifySignature(toSend);
			if (signingCert == null) 
			{
				PennRoutingUtilities.LogEvent("VerifyWSEmailMessageSignature() : is done, but unable to verify signature!");
				return false;
			}
			else 
			{

				PennRoutingUtilities.LogEvent("VerifyWSEmailMessageSignature() : is done!");
				return true;
			}

		}


		/// <summary>
		/// <param name="env"></param>
		/// <param name="s"></param>
		/// <returns></returns>
		public static X509Certificate VerifyLocalWSEmailMessageSignature(SoapEnvelope env, XmlElement xe) 
		{
			xe = (XmlElement)env.ImportNode(xe,true);
			if (xe == null) 
			{
				PennRoutingUtilities.LogEvent("VerifyWSEmailMessageSignature() : Unable to verify WSEmail Message signature... no signature found!\nMy identity is CN = " + ConfigurationSettings.AppSettings["SigningCertificate"]);
				return null;
			}
			
			PennRoutingUtilities.LogEvent("VerifyWSEmailMessageSignature() : Verifying WSEmail Message signature...\nMy identity is CN = " + ConfigurationSettings.AppSettings["SigningCertificate"] + "\nInput: "+xe.OuterXml+"\n"+"Onward to verifying...");
			// get a handle to the penn signature.
			// and the signature element within that.
			XmlElement sig = (XmlElement)xe.GetElementsByTagName("Signature")[0];

			// clone the entire  structure
			// this will be sent to the verification server.
			XmlElement toSend = (XmlElement)xe.Clone();

			// recreate the original signme node (this will be used in the signature verification.
			XmlElement toCheck = (XmlElement)env.CreateNode(XmlNodeType.Element,"","SignMe","http://securitylab.cis.upenn.edu/RoutingSignature/");
			// the node's value is the message, which was signed by the previous
			// hop.
			toCheck.InnerText = ((XmlElement)env.Body.GetElementsByTagName("theMessage")[0]).InnerXml;

			// recreate the data object.
			DataObject dO = new DataObject("MessageSignature","","",(XmlElement)toCheck);
			// since tosend is a clone of the penn signature,
			// find the signature element within that,
			// append the data object
			(toSend.GetElementsByTagName("Signature")[0]).AppendChild(env.ImportNode(dO.GetXml(),true));
			Microsoft.Web.Services.Security.X509.X509Certificate cert = VerifySignature(toSend);
			return cert;
		}

		/// <summary>
		/// Verifies the signature on a MySignedXML object and returns the signing certificate.
		/// </summary>
		/// <param name="toVerify">XML object that can be loaded as a MySignedXML object</param>
		/// <returns>X509Certificate if valid signature, exception otherwise</returns>
		public static Microsoft.Web.Services.Security.X509.X509Certificate VerifySignature(XmlElement toVerify)
		{
			/* our customized signed xml object which verifies
			 * the signatures on the xml message using
			 * the public key of the certificate that's included.
			 */

			// load it..
			PennRoutingFilters.MySignedXml sx = new PennRoutingFilters.MySignedXml();
			//TODO takeout...
			sx.LoadXml((XmlElement)toVerify.GetElementsByTagName("Signature")[0]);
			bool good = false;

			try 
			{ 
				good = sx.CheckSignature();

			} 
			catch (Exception e) 
			{
				LogEvent(e.Message);
			}
			// check the signature
			if(!good) 
			{
				LogEvent("VerifySignature() xml = " + sx.GetXml().OuterXml);
				LogEvent("PennRoutingUtilities.VerifySignature(): Verifying signature.\r\n\r\n\tVerification failure.");
				throw new Exception ("Verification failed.");
			}
			else 
			{
				// if it's good, return the certificate.
				byte[] buffer = Convert.FromBase64String( ((XmlElement)toVerify.GetElementsByTagName("X509Certificate")[0]).InnerText );
				Microsoft.Web.Services.Security.X509.X509Certificate cert = new Microsoft.Web.Services.Security.X509.X509Certificate(buffer);
				LogEvent("PennRoutingUtilities.VerifySignature(): Verifying signature.\r\n\r\n\tVerification succeeded.\r\n\r\nSigner is " + cert.GetName());
				if (VerifyCertificate(cert))
					return cert;
				else 
					return null;
			}
		}

		#endregion

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
/*
 * 			string s = c.GetName();
			int i = s.IndexOf("E=");
			int j = s.IndexOf(" ",i+1);
			if (j <= 0)
				j = s.Length;
			return s.Substring(i+2,j-i-3);
			*/
		}

		public static string GetCertCN(X509Certificate c) 
		{
			if (c != null) 
			{
				CAPICOM.Certificate cert = new CAPICOM.CertificateClass();
				cert.Import(c.ToBase64String());
				return cert.GetInfo(CAPICOM.CAPICOM_CERT_INFO_TYPE.CAPICOM_CERT_INFO_SUBJECT_SIMPLE_NAME);
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
				return (Microsoft.Web.Services.Security.X509SecurityToken)CertificateCache["cert-"+certKeyID+MachineStore.ToString()];
			}
			X509SecurityToken securityToken;  
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
				Microsoft.Web.Services.Security.X509.X509Certificate cert = null;
				X509CertificateCollection matchingCerts = store.FindCertificateBySubjectString(certKeyID);

				if (matchingCerts.Count == 0)
				{
					throw new ApplicationException("No matching certificates were found for the key ID provided in " + store.Location.ToString()+" store. Please run the configuration utility to guide you through client and server setup.");
				}
				else 
				{
					if (matchingCerts.Count > 1)
						LogEvent("Warning: Multiple certificates found for CN = " + certKeyID + ". ("+matchingCerts.Count.ToString() + ")");
					// pick the first one arbitrarily
					foreach (Microsoft.Web.Services.Security.X509.X509Certificate c in matchingCerts) 
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
					throw new ApplicationException("The certificate must support digital signatures and have a private key available.");
				}
				else 
				{
					byte[] keyId = cert.GetKeyIdentifier();
					Console.WriteLine("Key Name                       : {0}", cert.GetName());
					Console.WriteLine("Key ID of Certificate selected : {0}", Convert.ToBase64String(keyId));
					securityToken = new X509SecurityToken(cert);
				}
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

		/// <summary>
		/// Logs a message to the logfile ("C:\Temp\Logfile") along with the current time and a seperator.
		/// </summary>
		/// <param name="s">Message to log</param>
		public static void LogEvent (string s) 
		{
			
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
				s = s.Substring(0,i) + "<< formatted xml follows >>" + s.Substring(j+1,s.Length - j - 1);
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(theStuff);
				MemoryStream m = new MemoryStream(theStuff.Length);
				XmlTextWriter x = new XmlTextWriter(m,System.Text.Encoding.ASCII);
				x.Formatting = System.Xml.Formatting.Indented;

				/*
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
				*/

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
			GUILog(new EventItem(GetCurrentTime(),MyType,s,output));
			
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
				return;
			StreamWriter sw = new StreamWriter(fs);
			sw.WriteLine("---------------------------------------------------------------");
			sw.WriteLine("My identity is " + ConfigurationSettings.AppSettings["SigningCertificate"] + ".");
			sw.WriteLine(GetCurrentTime());
			sw.WriteLine(s);
			if (output.Length > 0)
				sw.WriteLine("Formatted XML Message:\n" + output);
			sw.Close();
			fs.Close();
		}

		#endregion
	
		#region FilterManipulations

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

		#endregion
	}

	

	/// <summary>
	/// A signed xml object that uses public key cryptography to create and verify signatures.
	/// </summary>
	public class MySignedXml : Microsoft.Web.Services.Security.SignedXml 
	{
		private IEnumerator m_keyInfoEnum = null;
		public MySignedXml() : base() {}
		public MySignedXml(XmlDocument document) : base(document) {}

		[DllImport("CertGetKey.dll", ExactSpelling=true)]
		private static extern void UnmanagedArrayFree (IntPtr ppbKeyBlob);

		[DllImport("CertGetKey.dll", ExactSpelling=true)]
		private static extern int PublicKeyBlobFromCertificateRawData (byte[] rgbRawData, int cbRawData, 
			out IntPtr /* ref byte[] */ ppbKeyBlob, out uint pKeyBlobSize);

		[DllImport("CertGetKey.dll", ExactSpelling=true)]
		private static extern void ImportPublicKeyBlob (byte[] rgbPubKeyBlob, int cbPubKeyBlob, out uint pExponent, 
			out IntPtr /* ref byte[] */ ppbModulus, out uint pModulusSize); 

		private static byte[] ConvertIntToByteArray(uint dwInput) 
		{
			// output of this routine is always big endian
			byte[] rgbTemp = new byte[4]; 
			uint t1, t2;

			if (dwInput == 0) return new byte[1]; 
			t1 = dwInput; 
			int i = 0;
			while (t1 > 0) 
			{
				t2 = t1 % 256;
				rgbTemp[i] = (byte) t2;
				t1 = (t1 - t2)/256;
				i++;
			}
			// Now, copy only the non-zero part of rgbTemp and reverse
			byte[] rgbOutput = new byte[i];
			// copy and reverse in one pass
			for (int j = 0; j < i; j++) 
			{
				rgbOutput[j] = rgbTemp[i-j-1];
			}
			return(rgbOutput);
		}

		protected override AsymmetricAlgorithm GetPublicKey() 
		{
			RSAKeyValue tempRSA;
			DSAKeyValue tempDSA;
			KeyInfoX509Data tempCert;

			if (m_keyInfoEnum == null)
				m_keyInfoEnum = KeyInfo.GetEnumerator();

			while (m_keyInfoEnum.MoveNext()) 
			{
				tempRSA = m_keyInfoEnum.Current as RSAKeyValue;
				if (tempRSA != null) return(tempRSA.Key);

				tempDSA = m_keyInfoEnum.Current as DSAKeyValue;
				if (tempDSA != null) return(tempDSA.Key);

				tempCert = m_keyInfoEnum.Current as KeyInfoX509Data;
				if (tempCert != null) 
				{
					if (tempCert.Certificates != null) 
					{
						// The code here doesn't look in all the certificates and doesn't check if they belong 
						// to the same certificate chain, or whether they are valid
						if (tempCert.Certificates != null) 
						{
							foreach (System.Security.Cryptography.X509Certificates.X509Certificate x509certificate in tempCert.Certificates) 
							{
								IntPtr KeyBlob = IntPtr.Zero;
								uint KeyBlobSize = 0;
								int result = PublicKeyBlobFromCertificateRawData(x509certificate.GetRawCertData(), x509certificate.GetRawCertData().Length, out KeyBlob, out KeyBlobSize);
								byte[] blob = new byte[KeyBlobSize];
								Marshal.Copy(KeyBlob, blob, 0, (int)KeyBlobSize);
								UnmanagedArrayFree(KeyBlob);
								RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
								RSAParameters parameters = new RSAParameters();
								IntPtr ExponentBlob = IntPtr.Zero, ModulusBlob = IntPtr.Zero;
								uint PubExponent = 0, ModulusSize = 0;
								ImportPublicKeyBlob(blob, blob.Length, out PubExponent, out ModulusBlob, out ModulusSize);
								parameters.Exponent = ConvertIntToByteArray(PubExponent);
								parameters.Modulus = new byte[ModulusSize];
								Marshal.Copy(ModulusBlob, parameters.Modulus, 0, (int)ModulusSize);
								UnmanagedArrayFree(ModulusBlob);
								rsa.ImportParameters(parameters);                                
								return rsa; 
							}
						}
					}
				}
			}
			return(null);
		}

	}

}
