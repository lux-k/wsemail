/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Xml;

namespace WSEmailProxy
{
	/// <summary>
	/// Contains the messageID, message and signature of the message.
	/// </summary>
	/// 
	public class WSEmailPackage 
	{
		/// <summary>
		/// ID number of the message on the server.
		/// </summary>
		public int MessageID;
		/// <summary>
		/// The message
		/// </summary>
		public WSEmailMessage theMessage;
		/// <summary>
		/// Signature of the message.
		/// </summary>
		public XmlElement sig;
	}
}
