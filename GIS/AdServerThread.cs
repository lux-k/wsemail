using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GIS
{
	/// <summary>
	/// A thread with state that serves ads to a given client.  The ads are sent as plain
	/// text, just a string, although more complex and richer things could be sent with just
	/// some minor improvements.  The class uses an instance Mutex to ensure that the ad
	/// string is read only after it is done being written.  It is the user's responsibility
	/// to ensure that the mutex is acquired and released appropriately.
	/// </summary>
	public class AdServerThread
	{
		/// <summary>
		/// Constructor for an Ad Server Thread
		/// </summary>
		/// <param name="client">The TCP Client connection that we are talking to</param>
		public AdServerThread(System.Net.Sockets.TcpClient client)
		{
			// set the things up right
			m_client = client;
			m_mutex = new System.Threading.Mutex(false, "NextAd Mutex");
		}

		/// <summary>
		/// Default constructor for the Ad Server Thread.  Must not be used
		/// </summary>
		protected AdServerThread()
		{
			// set things up blank
			m_userId = "";
			m_uri = new Uri("");
			m_client = new System.Net.Sockets.TcpClient();
		}


		#region Variables
		/// <summary>
		/// The unique user id for the user that this thread is watching
		/// </summary>
		protected string m_userId;
		/// <summary>
		/// The unique uri/email address for this user that this thread is
		/// watching
		/// </summary>
		protected System.Uri m_uri;
		/// <summary>
		/// The TCPClient that we are sending to
		/// </summary>
		protected System.Net.Sockets.TcpClient m_client;
		/// <summary>
		/// Mutex to prevent data mishandling in sending and receiving data
		/// </summary>
		protected System.Threading.Mutex m_mutex;
		/// <summary>
		/// The next Ad to be sent out.  Must not be written unless the 
		/// Mutex is taken!
		/// </summary>
		protected string m_nextAd;
		#endregion

		#region Properties
		/// <summary>
		/// The unique user id for the user that this thread is watching
		/// </summary>
		public string ID
		{
			get { return m_userId;}
			set { m_userId = value; }
		}

		/// <summary>
		/// The unique uri/email address for this user that this thread is
		/// watching
		/// </summary>
		public System.Uri Uri
		{
			get { return m_uri; }
			set { m_uri = value; }
		}
		/// <summary>
		/// The TCP Client that we are sending to
		/// </summary>
		public System.Net.Sockets.TcpClient Client
		{
			get { return m_client; }
			set { m_client = value; }
		}

		/// <summary>
		/// Mutex used to prevent the thread from reading data until the ad
		/// data is finished being written
		/// </summary>
		public System.Threading.Mutex Mutex
		{
			get { return m_mutex; }
			set { m_mutex = value; }
		}

		/// <summary>
		/// The next ad to be sent to the device.  Must not be set unless
		/// the Mutex is taken first!
		/// </summary>
		public string NextAd
		{
			set { this.m_nextAd  = value; }
		}

		#endregion

		/// <summary>
		/// Run the connection to the client sending ads whenever there is a new ad to pass
		/// along.  The user must be careful to set and maintain the Mutex for the NextAd
		/// property.  If the user fails to do that, the results are unpredictable.
		/// </summary>
		public void RunAdServerThread()
		{
			System.Net.Sockets.NetworkStream outStream;
			System.Net.Sockets.NetworkStream inStream;
			Byte[] sendBytes;
			Byte[] receiveBytes = new Byte[256];
			string data;
			int firstSpace, secondSpace;

			try
			{
				// first fetch the information about the user that we are talking to
				inStream = m_client.GetStream();

				// if there is some data to read
				if ( inStream.DataAvailable )
				{
					inStream.Read( receiveBytes, 0, receiveBytes.Length );
					data = System.Text.Encoding.ASCII.GetString(receiveBytes);

					// parse until the first space - that's the id
					firstSpace = data.IndexOf(" ", 0);
					this.m_userId = data.Substring(0, firstSpace);

					// now the rest until the next string is the uri
					secondSpace = data.IndexOf(" ", firstSpace);
					this.m_uri = new Uri(data.Substring(firstSpace, secondSpace-firstSpace));
				}
				else
				{
					// no data - leave
					this.m_client.Close();
					Console.WriteLine("Error: No data on client socket");
					return;
				}
			}
			catch ( System.Exception e )
			{
				// something is wrong
				this.m_client.Close();
				Console.WriteLine("Error: Can't read initialization data from TCP Client.");
				return;
			}

			// we must run the connection, sending data when there is new stuff

			// run this loop forever until we are told to quit
			while ( true )
			{
				// wait until the nextAd string is done
				m_mutex.WaitOne();

				// now we have the nextAd string to use

				// catch any errors in sending
				try
				{
					// if it's not blank, send the ad along the TCP Client
					if ( m_nextAd != "" )
					{
						outStream = m_client.GetStream();

						sendBytes = System.Text.Encoding.ASCII.GetBytes(m_nextAd);
						outStream.Write(sendBytes, 0, sendBytes.Length);

						// now we can blank the nextAd
						m_nextAd = "";
					}
				}
					// something went wrong here - die
				catch ( System.Exception e )
				{
					Console.WriteLine("Error in writing to Client" + m_client.ToString());
					// release the mutex and exit the thread
					m_mutex.ReleaseMutex();
					// close the client connection to be safe
					m_client.Close();
					// bye bye world
					return; 
				}
				finally
				{
					// now we can release the mutex being done with the read
					m_mutex.ReleaseMutex();
				}
			}

			// now close the client connection since we're done
			m_client.Close();

			// done serving ads
			return;
		}
	}
}
