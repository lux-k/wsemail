using System;
using System.Text;
using System.Diagnostics;
using System.Configuration;
using System.Net;
using System.Net.Sockets;


namespace TicTacToe
{
	public class UDPMulticastListener 
	{
		private IPAddress GroupAddress  = null;
		private const int GroupPort = 11000;
		private UdpClient listener = null;
		private bool done = false;
		public event StringHandler MessageReceived;

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
					listener.DropMulticastGroup(GroupAddress);
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
					catch (Exception e) 
					{
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
