/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Drawing;
using System.Resources;
using System.Reflection;
using System.Configuration;
using WSEmailProxy;
using FederatedBinaryToken;
using DNSResolver;
using Microsoft.Web.Services2.Security.X509;

namespace WSEmailClientConfig
{
	public abstract class ClientConfiguration
	{
		/// <summary>
		/// The proxy that will be used.
		/// </summary>
		private static MailServerProxy _msp = null;
		/// <summary>
		/// The token manager
		/// </summary>
		private static FederatedTokenManager fedtokman = null;
		private static string _userid = "";
		//private static string _msu = null;
		private static X509Certificate _cert = null;

		/// <summary>
		/// The userid of the user.
		/// </summary>
		public static string UserID 
		{
			get 
			{
				return _userid;
			}
			set 
			{
				_userid = value;
			}
		}

		/// <summary>
		/// Federated token manager
		/// </summary>
		public static FederatedTokenManager FederatedTokenManager 
		{
			get 
			{
				if (fedtokman == null) 
				{
					fedtokman = new FederatedTokenManager();
					fedtokman.AuthenticationToken = Proxy.SecurityToken;
				}
				return fedtokman;
			}
		}

		/// <summary>
		/// Gets the SigningCertificate section from the configuration file
		/// </summary>
		public static string CertCN 
		{
			get 
			{
				return ConfigurationSettings.AppSettings["SigningCertificate"];
			}
		}

		public static X509Certificate Certificate
		{
			get 
			{
				if (_cert != null)
					return _cert;

				try 
				{
					_cert = PennLibraries.Utilities.GetSecurityToken(CertCN,false).Certificate;
				} 
				catch {}

				return _cert;
			}
		}
		/// <summary>
		/// Gets the DNS server from the configuration file
		/// </summary>
		public static string DNSServer 
		{
			get 
			{
				return ConfigurationSettings.AppSettings["DnsServer"];
			}
		}


	
		/// <summary>
		/// Mail server proxy, configured to the url of "MailServer" configuration entry
		/// </summary>
		public static MailServerProxy Proxy 
		{
			get 
			{
				if (_msp == null)
					_msp = new MailServerProxy(MailServerUrl);

				return _msp;
			}
		}

		public static string MailServerUrl 
		{
			get 
			{
				/*
				if (_msu == null) 
				{
					
					string dom = ConfigurationSettings.AppSettings["MailServerDomain"];

					if (dom == null || dom.Equals(""))
						throw new Exception("MailServerDomain is not specified in the configuration.");

					string url = GetServerUrl(dom);

					if (url == null)
						throw new Exception("No server URL found in DNS for domain " + dom);

					_msu = url;
				}

				return _msu;
				*/
				string s = ConfigurationSettings.AppSettings["MailServer"];
				if (s == null || s.Equals(""))
					throw new Exception("The MailServer does not have a URL in configuration file. Please run the configurator.");
				return s;
			}
		}

		public static string GetServerUrl(string dom) 
		{

			DNSResolver.Resolver.DNSRecord[] Records = DNSResolver.Resolver.Query("_wsemail._tcp."+dom,DNSResolver.Resolver.RecordTypes.DNS_TYPE_SRV,DNSServer);
			
			if (Records == null) 
			{
				throw new Exception("Unable to get DNS SRV Records for " + dom);
			}

			string target = "";
			for (int i = 0; i < Records.Length; i++) 
			{
				if (Records[i] is Resolver.SRVRecord) 
				{
					Resolver.SRVRecord rec = (Resolver.SRVRecord)Records[i];
					target = rec.Target;
				}
			}
			
			if (target.Equals(""))
				return null;

			return target;
		}


		/// <summary>
		/// Logs a message
		/// </summary>
		/// <param name="s"></param>
		public static void Log(string s) 
		{
			PennLibraries.Utilities.LogEvent(s);
		}

		/// <summary>
		/// Logs to the log status window
		/// </summary>
		/// <param name="s"></param>
		public static void LogToStatusWindow(string s)
		{
			PennLibraries.Utilities.LogToStatusWindow(s);
		}
	}
}
