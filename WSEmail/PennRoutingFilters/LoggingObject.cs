using System;

namespace PennRoutingFilters
{
	/// <summary>
	/// Summary description for LoggingObject.
	/// </summary>
	public abstract class LoggingObject
	{
		protected static object LogTo = null;

		public LoggingObject()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void LogEvent(string s) 
		{
			s = PennRoutingFilters.PennRoutingUtilities.GetCurrentTime() + ": " + s;
			if (LogTo is System.Windows.Forms.TextBox) 
			{
				System.Windows.Forms.TextBox t = (System.Windows.Forms.TextBox)LogTo;
				t.Text += s + "\r\n";
				t.SelectionStart=t.Text.Length;
				t.ScrollToCaret();
			}
			else
				Console.WriteLine(s);
		}

	}
}
