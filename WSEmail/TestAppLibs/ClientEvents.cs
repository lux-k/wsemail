/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Runtime.Remoting.Messaging;

namespace TestAppLibs
{
	/// <summary>
	/// Summary description for ClientEvents.
	/// </summary>
	public class ClientEventsProxy : MarshalByRefObject
	{
		public event EventHandler StartTest;

		public ClientEventsProxy()
		{
			
		}

		[OneWay]
		public void LocallyHandleMessage(object o, EventArgs e) 
		{
			if (StartTest != null) 
				StartTest(o,EventArgs.Empty);
		}

		
	}
}
