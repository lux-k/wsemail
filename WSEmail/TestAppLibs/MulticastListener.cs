/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Configuration;

namespace TestAppLibs
{
	public delegate void MessageReceivedHandler(string s);

	public class UDPMulticastListener 
	{
		private IPAddress GroupAddress  = null;
		private const int GroupPort = 11000;
		private UdpClient listener = null;
		private bool done = false;
		public event MessageReceivedHandler MessageReceived;

		public UDPMulticastListener() 
		{
			string s = ConfigurationSettings.AppSettings["MulticastAddress"];
			bool err = false;
			if (s != null && s.Length > 0) 
			{
				try 
				{
					GroupAddress = IPAddress.Parse(s);
				} 
				catch
				{
					err = true;
				}
			} 
			else
				err = true;

			if (err)
				GroupAddress = IPAddress.Parse("224.168.100.2");

		}

		public override string ToString()
		{
			return GroupAddress.ToString();
		}

		public void StopListener() 
		{
			if (listener != null) 
			{
				done = true;
				try 
				{
					listener.Close();
				} 
				catch {}
			}
		}
			

		public void StartListener() 
		{
			listener = new UdpClient(GroupPort);
			IPEndPoint groupEP = new IPEndPoint(GroupAddress, GroupPort);//new IPEndPoint(GroupAddress,GroupPort);

			try 
			{
				listener.JoinMulticastGroup(GroupAddress);

				while (!done) 
				{
					try 
					{
						Debug.WriteLine("Waiting for broadcast");
						byte[] bytes = listener.Receive(ref groupEP);


						if (MessageReceived != null)
							MessageReceived(Encoding.ASCII.GetString(bytes,0,bytes.Length));

					} 
					catch (Exception e) {
						Debug.WriteLine(e.Message);
						Debug.WriteLine(e.StackTrace);
					}
				}

				listener.Close();
            
			} 
			catch (Exception e) 
			{
				Console.WriteLine(e.ToString());
			}
        
		}
	}
}