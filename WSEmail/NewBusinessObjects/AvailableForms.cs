/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;

namespace DynamicBizObjects
{
	/// <summary>
	/// Summary description for AvailableForms.
	/// </summary>
	public class AvailableForms
	{
		public static string[] GetForms() {
			return (new string[] {"Timesheet","Purchase Order"});
		}

		public static BusinessObjectsFormInterface LoadForm(string s) 
		{
			switch (s) 
			{
				case "Timesheet":
					return new FrmTimesheet();
				case "Purchase Order":
					return new FrmPurchaseOrder();
			}
			throw new ArgumentException("Unknown form: " + s);
		}
	}
}
