/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace WSSecureIMLib
{
	/// <summary>
	/// Holds a channel request. This is used to request a socket to be allocated on the secure IM proxy.
	/// </summary>
	[Serializable]
	public class ChannelRequest 
	{
	
		/// <summary>
		/// Private reference to the list of recipients
		/// </summary>
		private string _recip;
		/// <summary>
		/// Private reference to the sender
		/// </summary>
		private string _send;
		/// <summary>
		/// The location of the proxy
		/// </summary>
		private string _proxy;
		/// <summary>
		/// The destination port
		/// </summary>
		private int _dp;

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
		/// Sets/Returns the one receiving the request in user@host, user@host form.
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
		/// The destination port on the server.
		/// </summary>
		public int DestinationPort 
		{
			get 
			{
				return _dp;
			}
			set 
			{
				_dp = value;
			}
		}

		/// <summary>
		/// Deserializes a channel request from an XmlNode.
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static ChannelRequest Deserialize(XmlNode x) 
		{
			if (x == null) return null;

			XmlSerializer xs = new XmlSerializer(typeof(ChannelRequest));

			MemoryStream ms = new MemoryStream();
			byte[] bytes = System.Text.Encoding.ASCII.GetBytes(x.OuterXml);
			ms.Write(bytes,0,bytes.Length);
			ms.Position = 0;

			return (ChannelRequest)xs.Deserialize(ms);
			
		}

		/// <summary>
		/// Serializes this object to an xmlelement.
		/// </summary>
		/// <returns></returns>
		public XmlElement Serialize() 
		{
			XmlSerializer xs = new XmlSerializer(typeof(ChannelRequest));
		
			MemoryStream ms = new MemoryStream();
			xs.Serialize(ms,this);
			ms.Position = 0;

			XmlDocument d = new XmlDocument();
			d.Load(ms);
			if (d.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
				d.RemoveChild(d.FirstChild);

			return (XmlElement)d.FirstChild;			
		}

	}
}
