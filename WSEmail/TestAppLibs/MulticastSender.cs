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

	public class UDPMulticastSender 
	{

		private static IPAddress GroupAddress = null;
		private static int GroupPort = 11000;
    
		public UDPMulticastSender() 
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
			} else
				err = true;

			if (err)
				GroupAddress = IPAddress.Parse("224.168.100.2");

			Debug.WriteLine(GroupAddress.ToString());
			
		}

		public void Send(string message) 
		{
			UdpClient sender = new UdpClient();
			IPEndPoint groupEP = new IPEndPoint(GroupAddress,GroupPort);

			try 
			{
				Console.WriteLine("Sending datagram : {0}", message);
				byte[] bytes = Encoding.ASCII.GetBytes(message);

				sender.Send(bytes, bytes.Length, groupEP);
            
				sender.Close();
 			} 
			catch (Exception e) 
			{
				Console.WriteLine(e.ToString());
			}
        
		}

	}
}
