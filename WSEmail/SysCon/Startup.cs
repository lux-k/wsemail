/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Windows.Forms;

namespace SysCon
{
	/// <summary>
	/// Summary description for Startup.
	/// </summary>
	public class Startup
	{
		[STAThread]
		static void Main() 
		{
			try 
			{
				Application.Run(new FrmConsole());
			} 
			catch (Exception e) 
			{
				PennLibraries.ExceptionForm f = new PennLibraries.ExceptionForm(e,"An unexpected exception has occurred.");
			}
		}

	}
}
