/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using WSEmailProxy;

namespace WSInstantMessagingLibraries
{
	/// <summary>
	/// This is the interface that will be implemented by any object that can
	/// be used by the mail server to send messages to an instant messaging client.
	/// </summary>
	public interface IMPosting 
	{
		/// <summary>
		/// Takes a WSEmailMessage to post.
		/// </summary>
		/// <param name="m">Message to process.</param>
		void postMessage(WSEmailMessage m);
		/// <summary>
		/// Takes a serialized WSEmailMessage to post. (For remoting or other interfaces where relying on built-in serialization is problematic.)
		/// </summary>
		/// <param name="m">Message to process.</param>
		void postMessage(string s);
	}
}
