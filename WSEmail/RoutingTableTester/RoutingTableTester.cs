using System;
using WSERoutingTable;
using System.Configuration;

namespace RoutingTableTester
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Tester
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			RoutingTable rt = (RoutingTable)ConfigurationSettings.GetConfig("RoutingTable");
			if (rt == null) 
			{
				throw new Exception("Unable to load routing table.");
			}

			string find = "mailservera";
			Console.WriteLine("Finding route for " + find);
			Console.WriteLine(rt.findRoute("buh!",find));

			find = "blarg";
			Console.WriteLine("Finding route for " + find);
			Console.WriteLine(rt.findRoute("buh!",find));

			find = "mailserverb";
			Console.WriteLine("Finding route for " + find);
			Console.WriteLine(rt.findRoute("superbuh",find));
			Console.ReadLine();

		}
	}
}
