using System;
using System.Threading;
using System.Collections;
using WSEmailProxy;

namespace WSEmailQueueLib
{
	/// <summary>
	/// This is a thread safe message buffer object that is made for holding
	/// messages to be delivered.
	/// @Author Kevin Lux
	/// </summary>
	public class MessageBuffer : MarshalByRefObject
	{
		/// <summary>
		/// A generic mutex which controls mutual exclusion in several parts
		/// of the class.
		/// </summary>
		private volatile Mutex mutex;
		/// <summary>
		/// Holds the messages in a FIFO order.
		/// </summary>
		private Queue messages;

		private object theMonitor = new object();

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
				mutex.WaitOne();
				int i = messages.Count;
				mutex.ReleaseMutex();
				return i;
			}
		}

		/// <summary>
		/// Posts a new message to the message buffer. This is a thread-safe operation.
		/// </summary>
		/// <param name="m">Message to post.</param>
		public void postMessage(WSEmailPackage p) 
		{
			mutex.WaitOne();
			WSEmailQueue.LogEvent("A new message has been deposited in the buffer. (Total = " + (this.MessageCount + 1).ToString() + ")");
			Console.WriteLine("Message buffer received postMessage call.");
			// get the mutex lock
			Console.WriteLine("Got mutex lock.");
			// and enqueue the message.
			messages.Enqueue(p);
			Console.WriteLine("Enqueued the message!");
			// the getMessage() method waits on the monitor of <this> object. so, we
			// need to lock this object
			// and pulse it.
			Console.WriteLine("Pulsing monitor on message queue object.");
			lock(theMonitor) 
			{
				Monitor.Pulse(theMonitor);
			}
			// finally, release the mutex.
			Console.WriteLine("Released the mutex.");
			mutex.ReleaseMutex();

		}
		
		/// <summary>
		/// Gets the next available message in the buffer. It waits on a monitor, so there is
		/// no busy waiting.
		/// </summary>
		/// <returns>A WSEmailMessage representing the next message</returns>
		public WSEmailPackage getMessage() 
		{
			WSEmailPackage m = null;
			Monitor.Enter(theMonitor);
			if (MessageCount == 0) 
			{
				// if there are no messages, wait for the monitor to wake us
				// up
				Console.WriteLine("No messages available for processing, waiting for monitor update.");
				WSEmailQueue.LogEvent("No more messages available in the queue... waiting...");
				lock(this) 
				{
					Monitor.Wait(theMonitor,Timeout.Infinite);
				}
			}
			// otherwise, remove an object and return it. since dequeuing is done
			// on one thread and is mutex protected this should be ok.
			Console.WriteLine("Message available and monitor pulsed; processing.");
			
			// wait for a mutex lock
			//mutex.WaitOne();
			Console.WriteLine("Dequeued one message.");
			WSEmailQueue.LogEvent("Dequeued one message. (" + (this.MessageCount - 1).ToString() + " remaining in queue.)");
			m = (WSEmailPackage)messages.Dequeue();
			// then dequeue and return the message.
			//mutex.ReleaseMutex();
			Monitor.Exit(theMonitor);
			return m;
		}
		
		// used so that this object (when it's remoted) doesn't time out.
		public override object InitializeLifetimeService() 
		{
			return null; 
		}
	}
}
