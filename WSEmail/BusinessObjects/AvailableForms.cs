using System;

namespace BusinessObjects
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
