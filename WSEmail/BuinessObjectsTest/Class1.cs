using System;
using BusinessObjects;

namespace BuinessObjectsTest
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			BusinessObject b = new Timesheet();
			Timesheet t = new Timesheet();
//			((Timesheet)t).Hours = 10;

//			PurchaseOrder p = new PurchaseOrder();
//			((PurchaseOrder)p).PurchaseAmount = 5;

			Service1 s = new Service1();
//			string a = s.PostObject(t);
//			string b = s.PostObject(p);
			BusinessObject c = s.GetTimesheet(t);
			Timesheet n  = (Timesheet)c;
//			Console.WriteLine(n.Hours);
//			Console.WriteLine("Timesheet post: " + a);
//			Console.WriteLine("PurchaseOrder post: " + b);

		}
	}
}
