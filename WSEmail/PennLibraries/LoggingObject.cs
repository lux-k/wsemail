/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;

namespace PennLibraries
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
			s = Utilities.GetCurrentTime() + ": " + s;
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
