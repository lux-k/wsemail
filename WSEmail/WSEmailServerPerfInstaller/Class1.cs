/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Diagnostics;

namespace WSEmailServerPerfInstaller
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class PerfInstaller
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			if (!PerformanceCounterCategory.Exists("WSEmail Server"))
			{
				System.Diagnostics.CounterCreationDataCollection CounterDatas = 
					new System.Diagnostics.CounterCreationDataCollection();
				// Create the counters and set their properties.
				System.Diagnostics.CounterCreationData cdCounter1 = 
					new System.Diagnostics.CounterCreationData();
				cdCounter1.CounterName = "DeliveryQueueLength";
				cdCounter1.CounterHelp = "Number of messages in the queue";
				cdCounter1.CounterType = System.Diagnostics.PerformanceCounterType.NumberOfItems32;
				// Add both counters to the collection.
				CounterDatas.Add(cdCounter1);
				// Create the category and pass the collection to it.
				System.Diagnostics.PerformanceCounterCategory.Create(
					"WSEmail Server", "WSEmail Server Statistics", CounterDatas);
			}   
		}


	}
}
