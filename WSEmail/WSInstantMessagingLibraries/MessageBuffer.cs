/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Threading;
using System.Collections;
using WSEmailProxy;

namespace WSInstantMessagingLibraries
{
	/// <summary>
	/// This is a thread safe message buffer object that is made for holding
	/// instant messages.
	/// @Author Kevin Lux
	/// </summary>
	public class MessageBuffer : MarshalByRefObject, IMPosting
	{
		/// <summary>
		/// A generic mutex which controls mutual exclusion in several parts
		/// of the class.
		/// </summary>
		private Mutex mutex;
		/// <summary>
		/// Holds the messages in a FIFO order.
		/// </summary>
		private Queue messages;

		/// <summary>
		/// Default constructor which initializes the mutex to available and the message buffer to empty.
		/// </summary>
		public MessageBuffer()
		{
			mutex = new Mutex(false);
			messages = new Queue();
		}

		/// <summary>
		/// Gets the current count of available messages in the buffer. This is thread-safe.
		/// </summary>
		public int MessageCount
		{
			get 
			{
				// get the mutex, get the count, then release the mutex.
				Monitor.Enter(messages);
				int i = messages.Count;
				Monitor.Exit(messages);
				//mutex.ReleaseMutex();
				return i;
			}
		}

		public void postMessage(string s) 
		{
			postMessage(WSEmailMessage.Deserialize(s));
		}

		/// <summary>
		/// Posts a new message to the message buffer. This is a thread-safe operation.
		/// </summary>
		/// <param name="m">Message to post.</param>
		public void postMessage(WSEmailMessage m) 
		{
			Monitor.Enter(messages);
			Console.WriteLine("Message buffer received postMessage call.");
			// get the mutex lock
			Console.WriteLine("Got mutex lock.");
			// and enqueue the message.
			messages.Enqueue(m);
			Console.WriteLine("Enqueued the message!");
			// the getMessage() method waits on the monitor of <this> object. so, we
			// need to lock this object
			// and pulse it.
			lock(this) 
			{
				Console.WriteLine("Pulsing monitor on message queue object.");
				Monitor.Pulse(this);
			}
			// finally, release the mutex.
			Monitor.Exit(messages);
		}
		
		/// <summary>
		/// Gets the next available message in the buffer. It waits on a monitor, so there is
		/// no busy waiting.
		/// </summary>
		/// <returns>A WSEmailMessage representing the next message</returns>
		public WSEmailMessage getMessage() 
		{
			bool err = false;
			if (MessageCount == 0) {
				// if there are no messages, wait for the monitor to wake us
				// up
				Console.WriteLine("No messages available for processing, waiting for monitor update.");
				lock(this) 
				{
					try 
					{
						Monitor.Wait(this,Timeout.Infinite);
					} 
					catch {
						err = true;
					}
				}
			}
			if (!err) 
			{
				// otherwise, remove an object and return it. since dequeuing is done
				// on one thread and is mutex protected this should be ok.
				Console.WriteLine("Message available and monitor pulsed; processing.");
				WSEmailMessage m = null;
				// wait for a mutex lock
				Monitor.Enter(messages);
				m = (WSEmailMessage)messages.Dequeue();
				// then dequeue and return the message.
				Monitor.Exit(messages);
				return m;
			} else 
				return null;
		}
		
		// used so that this object (when it's remoted) doesn't time out.
		public override object InitializeLifetimeService() 
		{
			return null;
		}
	}
}
