using System;
using System.Collections;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;

namespace TicTacToe
{
	public class NetworkDriver : IDisposable
	{
		HttpChannel channel = null;
		bool disposing = false;
		private TicTacToe t = null;
		/// <summary>
		/// The published name of the game.
		/// </summary>
		public const string PUBLISHEDNAME = "TTT";
		/// <summary>
		/// The port the remoting service waits on.
		/// </summary>
		public const int PORT = 9876;

		/// <summary>
		/// Sets the game that is/can be remoted.
		/// </summary>
		public TicTacToe Game 
		{
			get 
			{
				return t;
			}
			set 
			{
				t = value;
			}
		}

		/// <summary>
		/// Builds the remoting channel.
		/// </summary>
		public NetworkDriver() 
		{
			SoapServerFormatterSinkProvider serverProv = new SoapServerFormatterSinkProvider();
			serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
			SoapClientFormatterSinkProvider clientProv = new SoapClientFormatterSinkProvider();
			IDictionary props = new Hashtable();
			props["port"] = PORT;
			
			channel = new HttpChannel (props,clientProv, serverProv);        
			ChannelServices.RegisterChannel( channel );
		}

		/// <summary>
		/// Remotes the current game out to the world.
		/// </summary>
		public void Remote() 
		{
			try 
			{
				RemotingServices.Marshal(t,PUBLISHEDNAME);
			} 
			catch {}
		}

		/// <summary>
		/// Disconnects the game from the world.
		/// </summary>
		public void UnRemote() 
		{
			try 
			{
				RemotingServices.Disconnect(t);
			} 
			catch {}

		}

		/// <summary>
		/// Properly cleans up resources for this object.
		/// </summary>
		public void Dispose()
		{
			if (!disposing) 
			{
				UnRemote();
				try 
				{
					if (channel != null) 
						ChannelServices.UnregisterChannel( channel );
				} 
				catch {}

			}
		}
	}
}
