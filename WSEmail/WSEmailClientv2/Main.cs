/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Windows.Forms;

namespace WSEmailClientv2
{
	public class TheApp 
	{
		[STAThread]
		static void Main() 
		{
			// catch unhandled exceptions in this application
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledException);
			// catch unhanled child thread exceptions
			Application.ThreadException +=new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
			// clean up anything before the app closes
			AppDomain.CurrentDomain.DomainUnload +=new EventHandler(CurrentDomain_DomainUnload);

			PennLibraries.Utilities.AddPennLoggingFilters();
			Application.Run(new FrmMain());
		}

		private static void UnhandledException(object sender, UnhandledExceptionEventArgs e) 
		{
			HandleException((Exception)e.ExceptionObject);
		}

		private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			HandleException(e.Exception);
		}

		private static void CurrentDomain_DomainUnload(object sender, EventArgs e)
		{
			if (Global.AlertIcon != null)
				Global.AlertIcon.Dispose();
		}

		private static void HandleException(Exception e) 
		{
			if (e != null) 
			{
				if (!(e is System.Threading.ThreadAbortException))
					new PennLibraries.ExceptionForm(e,"An unexpected and unhandled exception has occurred.");		
				CurrentDomain_DomainUnload(null,null);
			}
		}
	}
}
