/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;

namespace WSEmailProxy
{
	/// <summary>
	/// Contains the header information for a WSEmailMessage.
	/// </summary>
	public class WSEmailHeader 
	{
		/// <summary>
		/// Default constructor which does nothing.
		/// </summary>
		public WSEmailHeader () { }


		private int _mid, _code;
		/// <summary>
		/// Holds the message ID of the message. This identifies the message on the server.
		/// </summary>
		public int MessageID 
		{
			set 
			{
				_mid = value;
			}
			get 
			{
				return _mid;
			}
		}

		[System.Xml.Serialization.XmlIgnore()]
		public string Contents 
		{
			get 
			{
				return WSEmailFlags.GetContents(Flags);
			}
		}

		public int Flags 
		{
			set 
			{
				_code = value;
			}
			get 
			{
				return _code;
			}
		}

		private string _sender, _subject;
		private DateTime _timestamp;
		/// <summary>
		/// A user@host string representing the sender.
		/// </summary>
		public string Sender 
		{
			get 
			{
				return _sender;
			}
			set 
			{
				_sender = value;
			}
		}
		/// <summary>
		/// A string that holds the subject of the message.
		/// </summary>
		public string Subject 
		{
			get 
			{
				return _subject;
			}
			set 
			{
				_subject = value;
			}
		}

		/// <summary>
		/// A string that holds a timestamp on the message.
		/// </summary>
		public DateTime Timestamp 
		{
			get 
			{
				return _timestamp;
			}
			set 
			{
				_timestamp = value;
			}
		}
		/// <summary>
		/// A method which allows the message to be easily printed.
		/// </summary>
		/// <returns>String representation</returns>
		public override string ToString() 
		{
			string s = "From: " + Sender + "\tSubject: " + Subject + "\tDate: "+Timestamp;
			return s;
		}
	}
}
