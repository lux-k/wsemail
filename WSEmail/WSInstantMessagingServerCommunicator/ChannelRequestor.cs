/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;

namespace WSSecureIMLib
{
	/// <summary>
	/// Accepts requests for ports and allocates ports to users. This class simply controls port numbers. 
	/// Also, this class doesn't see if the OS has control of a port, 
	/// it is only worried about handing out ports it thinks aren't used. This should be addressed in a later release.
	/// </summary>
	public class ChannelRequestor
	{
		/// <summary>
		/// The base port (inclusive) that can be allocated.
		/// </summary>
		const int BasePort = 30000;
		/// <summary>
		/// The number of available ports in this range. This will give a range of BasePort .. BasePort + AllocatedAmount port numbers.
		/// </summary>
		const int AllocatedAmount = 2000;
		/// <summary>
		/// A boolean array that maps 0 -> BasePort, 1 -> BasePort + 1 and so on. If it's true, it's considered in use.
		/// </summary>
		private bool[] PortStatus = new bool[AllocatedAmount];


		/// <summary>
		/// Default constructor. Initializes all the PortStatuses to false.
		/// </summary>
		public ChannelRequestor()
		{
			for (int i = 0; i < AllocatedAmount; i++)
				PortStatus[i] = false;
		}

		/// <summary>
		/// Gets one available port from the pool
		/// </summary>
		/// <returns>An integer > 0 if a port is available, 0 otherwise.</returns>
		public int GetAvailablePort() 
		{
			int ret = 0;
			lock (PortStatus) 
			{
				for (int i = 0; i < AllocatedAmount; i++) 
				{
					if (PortStatus[i] == false) 
					{
						PortStatus[i] = true;
						ret = BasePort + i;
						break;
					}
				}
			}
			IMBroker.LogEvent(this + " : " + ret.ToString() + " allocated for use.");
			return ret;
		}

		/// <summary>
		/// Gets an available port and updates the channel request data structure, returning it. After this, the structure
		/// will have ports assigned to users.
		/// </summary>
		/// <param name="r">Request structure. Should have the names of the users filled in, but not required.</param>
		/// <returns>Updated datastructure with the assigned ports</returns>
		public ChannelRequest RequestChannel(ChannelRequest r) 
		{
			r.DestinationPort = GetAvailablePort();
			r.Proxy = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString();
			return r;
		}

		/// <summary>
		/// Marks a channel as free (after everyone has disconnected).
		/// </summary>
		/// <param name="port"></param>
		public void FreeChannel(int port) 
		{
			lock (PortStatus) 
			{
				PortStatus[port - BasePort] = false;
			}
		}
	}

}
