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


namespace WSEmailClientv2
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global : WSEmailClientConfig.ClientConfiguration
	{
		/// <summary>
		/// How many secs to wait before polling mail again.
		/// </summary>
		private static int _ct = 60;
		private static Image _good, _bad = null;
		private static NotifyIconEx _not = null;

		/// <summary>
		/// Loads the unused stamp image out of the assembly
		/// </summary>
		public static Image UnusedStamp 
		{
			get 
			{
				if (_good == null) 
					_good =  Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WSEmailClientv2.stamp-orig.gif"));
				return _good;
			}
		}

		public static NotifyIconEx AlertIcon
		{
			get 
			{
				if (_not == null) 
				{
					_not = new NotifyIconEx();
					_not.Text = "WSEmail Client";
					_not.Icon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("WSEmailClientv2.Mailbox.ico"));
				} 
				return _not;
			}
		}

		/// <summary>
		/// Loads the cancelled stamp image out of the assembly
		/// </summary>
		public static Image CancelledStamp 
		{
			get 
			{
				if (_bad == null)
					_bad = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WSEmailClientv2.stamp-cancelled.gif"));
				return _bad;
			}
		}

		/// <summary>
		/// How many seconds to wait before polling the server for new messages.
		/// </summary>
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
