/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Configuration;
using PennLibraries;
using WSEmailClientConfig;

namespace WSInstantMessagingLibraries
{
	/// <summary>
	/// Holds various configuration items needed to send and receive instant messages
	/// @Author Kevin Lux
	/// </summary>
	public class WSInstantMessagingConfig 
	{

		private string _remotingUrl;
		private int _remotingPort;

		/// <summary>
		/// The remoting URL to be published to the webserver. Usually this will
		/// be an instance to the MessageBuffer object.
		/// </summary>
		public string RemotingUrl
		{
			get 
			{
				return _remotingUrl;
			}
			set 
			{
				_remotingUrl = value;
			}
		}

		/// <summary>
		/// The remoting port number we are listening on. This will be an instantance of web remoting.
		/// </summary>
		public int RemotingPort 
		{
			get 
			{
				return _remotingPort;
			}

			set 
			{
				_remotingPort = value;
			}
		}

		/// <summary>
		/// Default constructor that doesn't do anything.
		/// </summary>
		public WSInstantMessagingConfig()
		{
			loadConfig();

		}

		/// <summary>
		/// Loads the remoting port, certificateID, mailServerURL and UserID from the app's configuration file
		/// and certificate store.
		/// </summary>
		public void loadConfig () 
		{
			// load the remoting port, the certificate common name, the mailserver url and
			// finally, the "email" address of the cert.
			try 
			{
				RemotingPort = int.Parse(ConfigurationSettings.AppSettings["InstantMessagingPort"]);
			} 
			catch
			{
				RemotingPort = 8888;
			}
		}
	}
}
