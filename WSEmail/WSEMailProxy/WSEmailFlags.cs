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
	/// WSEmailFlags class. This defines the various ways in which a message can be acted upon and delivered by
	/// the remote mail server and client.
	/// </summary>
	public class WSEmailFlags 
	{
		/// <summary>
		/// The instant messaging flag group
		/// </summary>
		/// 

		public static string GetContents(int i) 
		{
			string s = "";
			if ( (i & Contains.Form) == Contains.Form) 
			{
				s += "Contains a form, ";
			}

			if ( (i & Contains.DynamicForm) == Contains.DynamicForm) 
			{
				s += "Contains a dynamic form, ";
			}

			if ( (i & Precedence.BulkDelivery) == Precedence.BulkDelivery) 
			{
				s += "Bulk message, ";
			}
			if ( (i & InstantMessaging.SendAsInstantMessage) == InstantMessaging.SendAsInstantMessage) 
			{
				s += "Meant an an instant message, ";
			}

			if (s.Length > 0)
				s=s.TrimEnd(new char[] {' ',','});
			return s;
		}
		/// <summary>
		/// Specifies more exactly what the WSEmail contains, such as old style (non-dynamic) forms or the new
		/// dynamic forms.
		/// </summary>
		public class Contains 
		{
			/// <summary>
			/// Contains an old style form.
			/// </summary>
			public const int Form = 16;
			/// <summary>
			/// Contains at least 1 new style dynamic form.
			/// </summary>
			public const int DynamicForm = 32;
		}

		public class InstantMessaging 
		{
			/// <summary>
			/// Specifies to the server that this message should be delivered the the recipient as an instant message.
			/// </summary>
			public const int SendAsInstantMessage = 1;
			/// <summary>
			/// Tells the server to delete this message if it is not deliverable as an instant message.
			/// </summary>
			public const int DeleteIfNotDeliverable = 2;
			/// <summary>
			/// A secure P2P chat conversation.
			/// </summary>
			public const int DirectConnectInvitation = 8;

		}


		/// <summary>
		/// Constants that describe the message's distribution
		/// </summary>
		public class Precedence 
		{
			/// <summary>
			/// Specifies this message targets a wide audience, ie. precedence: bulk
			/// </summary>
			public const int BulkDelivery = 4;
			public const int EncryptedDelivery = 64;
			public const int DirectDelivery = 128;
		}
	}

}
