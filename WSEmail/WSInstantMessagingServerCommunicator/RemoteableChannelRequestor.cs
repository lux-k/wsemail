/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Threading;
using System.Collections;

namespace WSSecureIMLib
{
	/// <summary>
	/// Holds all the incoming channel request creations requests. Not really used anymore.
	/// </summary>
	public class ChannelRequestQueue
	{
		/// <summary>
		/// The queue.
		/// </summary>
		ArrayList queue = new ArrayList();

		/// <summary>
		/// Default empty constructor.
		/// </summary>
		public ChannelRequestQueue()
		{
			
		}

		/// <summary>
		/// Read-only property which returns the number of requests in the queue.
		/// </summary>
		public int PendingRequests 
		{
			get 
			{
				return queue.Count;
			}
		}

		/// <summary>
		/// Adds a new request to the queue.
		/// </summary>
		/// <param name="r">Request to add</param>
		public void AddRequest(ChannelRequest r) 
		{
			queue.Add(r);
		}

		/// <summary>
		/// Gets the next request to be processed from the queue or returns null if there are no more requests.
		/// </summary>
		/// <returns></returns>
		public ChannelRequest GetRequest() 
		{
			if (PendingRequests > 0) 
			{
				ChannelRequest r = (ChannelRequest)queue[0];
				queue.RemoveAt(0);
				return r;
			}
			return null;
		}		
	}

	/// <summary>
	/// This is the actual object that gets remoted. It contains the channel request queue.  It works, however, there may be concurrency problems.
	/// Since it uses monitors, it might be the case that the monitor unlocks the wrong request.
	/// </summary>
	[Serializable]
	public class RemoteableChannelRequest : MarshalByRefObject
	{
		/// <summary>
		/// The delegate used in the request added event. For simplicity, it might be smart to make
		/// the event return a ChannelRequest in the future.
		/// </summary>
		public delegate void VoidDelegate();
		/// <summary>
		/// The event that is fired when a request is added
		/// </summary>
		public event VoidDelegate RequestAdded;
		/// <summary>
		/// Holds the return data from processing a request. This is the main concurrency issue.
		/// </summary>
		private ChannelRequest _cr;

		/// <summary>
		/// Begins a new request for a channel. It will submit the request to the IMBroker who will allocate ports, open them, etc.
		/// While this is happening, this routine waits on a monitor on this actual object instance. The IMBroker
		/// will pulse this object when it has placed the return value in the Request property of this object.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="recipient"></param>
		/// <returns></returns>
		public ChannelRequest RequestChannel (string sender, string recipient) 
		{
			IMBroker.LogEvent("RequestChannel started.");
			lock(this) 
			{
				ChannelRequest r = new ChannelRequest(sender,recipient);
				Request = r;
				if (RequestAdded != null)
					RequestAdded.DynamicInvoke(null);
				IMBroker.LogEvent("RequestAdded event fired.");
				Monitor.Wait(this);
			}
			return Request;
		}

		/// <summary>
		/// Holds the results of the channel request.
		/// </summary>
		public ChannelRequest Request 
		{
			get 
			{
				return _cr;
			}
			set 
			{
				_cr = value;
			}
		}

		/// <summary>
		/// Used so this object doesn't time out in remoting.
		/// </summary>
		/// <returns>(nothing)</returns>
		public override object InitializeLifetimeService() 
		{
			return null;
		}

	}
}
