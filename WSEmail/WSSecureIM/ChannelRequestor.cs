using System;

namespace WSSecureIMLib
{
	/// <summary>
	/// Accepts requests for ports and allocates ports to users. This class simply controls port numbers. It is
	/// imagined that some time later it may be smart enough to reuse connections (ie. multiplex on a port) but
	/// currently it dumbly hands out 2 unused ports. Also, this class doesn't see if the OS has control of a port, 
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
			for (int i = 0; i < AllocatedAmount; i++) 
			{
				if (PortStatus[i] == false) 
				{
					PortStatus[i] = true;
					ret = BasePort + i;
					break;
				}
			}
			IMBroker.LogEvent(ret.ToString() + " allocated for use.");
			return ret;
		}

		/// <summary>
		/// Gets 2 available ports and updates the channel request data structure, returning it. After this, the structure
		/// will have ports assigned to users.
		/// </summary>
		/// <param name="r">Request structure. Should have the names of the users filled in, but not required.</param>
		/// <returns>Updated datastructure with the assigned ports</returns>
		public ChannelRequest RequestChannel(ChannelRequest r) 
		{
			r.RecipientPort = GetAvailablePort();
			r.SenderPort = GetAvailablePort();
			return r;
		}
	}

	/// <summary>
	/// Holds a channel request. This is used to request a pair of sockets be allocated on the secure IM proxy.
	/// </summary>
	[Serializable]
	public class ChannelRequest 
	{
	
		private string _recip, _send, _proxy;
		private int _pr, _ps;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ChannelRequest() {}
		/// <summary>
		/// Constructor specifying the sender and recipient. The sender is the one initiating the request, the recipient receives it.
		/// </summary>
		/// <param name="sender">A user@host type address</param>
		/// <param name="recipient">A user@host type address</param>
		public ChannelRequest(string sender, string recipient) 
		{
			Sender = sender;
			Recipient = recipient;
		}

		/// <summary>
		/// Sets/Returns the hostname of the proxy.
		/// </summary>
		public string Proxy
		{
			get 
			{
				return _proxy;
			}
			set 
			{
				_proxy = value;
			}
		}

		/// <summary>
		/// Sets/Returns the one receiving the request in user@host form.
		/// </summary>
		public string Recipient 
		{
			get 
			{
				return _recip;
			}
			set 
			{
				_recip = value;
			}
		}

		/// <summary>
		/// Sets/Returns the one sending the request in user@host form.
		/// </summary>
		public string Sender 
		{
			get 
			{
				return _send;
			}
			set 
			{
				_send = value;
			}
		}

		/// <summary>
		/// Sets/Returns the port the recpieint should connect to on the proxy.
		/// </summary>
		public int RecipientPort 
		{
			get 
			{
				return _pr;
			}
			set 
			{
				_pr = value;
			}
		}

		/// <summary>
		/// Sets/Returns the port the sender should connect to on the proxy.
		/// </summary>
		public int SenderPort 
		{
			get 
			{
				return _ps;
			}
			set 
			{
				_ps = value;
			}
		}
	}
}
