using System;
using System.Configuration;
using WSEmailProxy;

namespace WSInstantMessagingLibraries
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global
	{
		private static MailServerProxy _msp = null;
		private static int _ct = 60;
		
		public static MailServerProxy Proxy 
		{
			get 
			{
				if (_msp == null)
					_msp = new MailServerProxy(ConfigurationSettings.AppSettings["MailServer"]);

				return _msp;
			}
			set 
			{
				_msp = value;
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
	}
}
