using System;
using System.Drawing;
using System.Resources;
using System.Reflection;
using System.Configuration;
using WSEmailProxy;
using FederatedBinaryToken;

namespace WSEMailClient
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global
	{
		private static MailServerProxy _msp = null;
		private static int _ct = 60;
		private static Image _good, _bad = null;
		private static FederatedTokenManager fedtokman = null;

		public static FederatedTokenManager FederatedTokenManager 
		{
			get 
			{
				if (fedtokman == null) 
				{
					fedtokman = new FederatedTokenManager();
					fedtokman.AuthenticationToken = Global.Proxy.SecurityToken;
				}
				return fedtokman;
			}
		}

		public static Image UnusedStamp 
		{
			get 
			{
				if (_good == null) 
					_good =  Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WSEMailClient.stamp-orig.gif"));
				return _good;
			}
		}
		public static Image CancelledStamp 
		{
			get 
			{
				if (_bad == null)
					_bad = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WSEMailClient.stamp-cancelled.gif"));
				return _bad;
			}
		}
	
		public static MailServerProxy Proxy 
		{
			get 
			{
				if (_msp == null)
					_msp = new MailServerProxy(ConfigurationSettings.AppSettings["MailServer"]);

				return _msp;
			}
		}

		public static int CheckDelay 
		{
			get 
			{
				return _ct;
			}
			set 
			{
				_ct = value;
			}
		}
		
		public static void Log(string s) 
		{
			PennLibraries.Utilities.LogEvent(s);
		}

		public static void LogToStatusWindow(string s)
		{
			PennLibraries.Utilities.LogToStatusWindow(s);
		}
	}
}
