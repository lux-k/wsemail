using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Microsoft.Web.Services.Security.X509;
using Microsoft.Web.Services;
using Microsoft.Web.Services.Security;
using CommonTypes;


namespace GIS
{
	/// <summary>
	/// Summary description for GIS.
	/// </summary>
	[System.Web.Services.WebService(
	 Namespace="http://158.130.67.0/GIS/",
	 Description="A Geographic Information Service.  Allows Merchants and Users to interact by letting authotized Merchants attain location information about registered users.")]	 
	public class GIS : System.Web.Services.WebService
	{
		/// <summary>
		/// The port that we will talk to the Ad Client on
		/// </summary>
		public const int AD_PORT  = 8085;

		public GIS()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();	
		}
	
		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();

				// close down the thread for servers
				//this.m_tServer.Abort();
				// close the server
				//this.m_tcp_server.Stop();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		#region User Interactions
		/// <summary>
		/// Invoked by the Mobile Device to indicate entry to the specified area. 
		/// If the given name is already listed then the old location is overwritten.
		/// </summary>
		/// <param name="u">User information object</param>
		/// <returns>true indicates success.  false otherwise</returns>
		[WebMethod]
		public bool RegisterUser( User newUser )
		{
			// index of the user object if any
			int index;
			index = Global.m_users.IndexOf(newUser);

			// let's add this user name to our directory for the site
			// if it's already in there just update the time
			if ( index != -1 )
			{
				(Global.m_users[index] as User).EntryTime = System.DateTime.Now;

			}
			else
				// make a new one and put it in the array
			{
				// set the fields right
				newUser.EntryTime = System.DateTime.Now;
				
				// add it
				Global.m_users.Add(newUser);

				// also add it to the ads by user list
				Global.m_ads_by_user.Add(newUser, new ArrayList());
			}

			// we send back a response to indicate success or failure
			// if the key is now in the list, we have success
			if ( Global.m_users.IndexOf(newUser) != -1 )
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Invoked by the Mobile Device to indicate leaving a certain area.  If the
		/// address key doesn't match the one listed, the name is not removed.  It is assumed
		/// that it is an error.
		/// </summary>
		/// <param name="u">User information object</param></param>
		/// <returns>true if the name/address pair was removed.  false otherwise.</returns>
		[WebMethod]
		public bool RemoveUser( User u )
		{
			// remove the specified user at the specified address from the database
			if ( Global.m_users.IndexOf(u) != -1 )
			{
				Global.m_users.Remove(u);

				// also remove from the ads by user list
				Global.m_ads_by_user.Remove(u);
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Retrives new ads for a given user to download.  The ads are only retrieved once,
		/// after they have been sent once they are removed from the user's database.
		/// </summary>
		/// <param name="u">The User object for whom we are sending back ads</param>
		/// <returns>ArrayList of AdMessages for the device to display</returns>
		[WebMethod]
		public CommonTypes.AdMessage[] GetNewAds( User u )
		{
			ArrayList arr = null;
			CommonTypes.AdMessage[] Adarr;

			// the user wants to fetch the available ads for it

			// index the ads by user list
			arr = Global.m_ads_by_user[u] as ArrayList;

			// if we can't find it, then we'll return an empty array
			if ( arr == null )
				return new CommonTypes.AdMessage[0];

			// now clear out the old list
			Global.m_ads_by_user[u] = new ArrayList();

			// convert them to AdMessage array
			Adarr = new CommonTypes.AdMessage[arr.Count];
			int i = 0;
			foreach ( CommonTypes.AdMessage ad in arr )
			{
				// if it's not null
				if ( ad == null )
					break;

				// put it in the array
				Adarr[i] = ad;

				i++;
			}

			// send back the new ads
			return Adarr;
		}
		#endregion

		#region Merchant Interactions
		/// <summary>
		/// Web method that's invoked by the merchants who wish to acquire a copy of the
		/// available users.  Checks for authentication before sending the list back.
		/// </summary>
		/// <returns>Sorted list with the full user list in the area if authenticated.
		/// null otherwise.</returns>
		[WebMethod]
		public ArrayList getUsers()
		{
			Microsoft.Web.Services.Security.X509.X509Certificate cert;

			// we must receive a X.509 cert to validate this request
			// we can't just take an unsigned request
			SoapContext requestContext  = HttpSoapContext.RequestContext;

			// If we don't have a request SOAP context, we need to throw
			// because we were not given a SOAP request
			if (requestContext == null)
				throw new ApplicationException("Only SOAP requests are permitted.");

			// extract the cert from the request context
			cert = GetCert.GetCertFromContext(requestContext);

			// Make sure the security information is acceptable
			if ( cert == null || cert.SupportsDataEncryption == false )
			{
				throw new SoapException("The security information supplied was not valid.",
					new System.Xml.XmlQualifiedName("Bad.Security", "http://microsoft.com/wse/samples/SumService"));
			}

			// check if the name is on our list and the cert is current
			if ( Global.m_merchants.Contains( cert.GetName() ) && cert.IsCurrent)
			{
				// we need to set the request context to encrypt our SOAP response
				// we don't want to send out the data in clear text!
				// make sure we can use this certificate for encryption
				
				// the response context
				//SoapContext responseContext = HttpSoapContext.ResponseContext;
				// turn cert provided into token
				//X509SecurityToken myCertToken = new X509SecurityToken(cert);
				// add in security now with this cert
				//responseContext.Security.Tokens.Add(myCertToken);
				//responseContext.Security.Elements.Add(new EncryptedData(myCertToken));
			
				return Global.m_users;
			}
			else
			{
				return null;
			}

		}

		[WebMethod]
		public bool RegisterMerchant(P3P.POLICY policy)
		{
			// fetch the X.509 Cert out of the request context
			Microsoft.Web.Services.Security.X509.X509Certificate cert;

			// we must receive a X.509 cert to validate this request
			// we can't just take an unsigned request
			SoapContext requestContext  = HttpSoapContext.RequestContext;

			// If we don't have a request SOAP context, we need to throw
			// because we were not given a SOAP request
			if (requestContext == null)
				throw new ApplicationException("Only SOAP requests are permitted.");

			// extract the cert from the request context
			cert = GetCert.GetCertFromContext(requestContext);

			// Make sure the security information is acceptable
			if ( cert == null || cert.SupportsDataEncryption == false )
			{
				throw new SoapException("The security information supplied was not valid.",
					new System.Xml.XmlQualifiedName("Bad.Security", "http://158.130.67.0/GIS/GIS.asmx"));
			}

			// take the policy and compare it to our own
			// if it's better or equal then we accept it
			if ( policy.equals(Global.m_policy) )
			{
				// add to accept list
				Global.m_merchants.Add(cert.GetName());
					return true;
			}
			else
			{
				// unacceptable policy
				return false;
			}
		}

		/// <summary>
		/// Puts an Ad in the send queue for the specified user
		/// </summary>
		/// <param name="u">The User to send the ad to</param>
		/// <param name="ad">The Ad to send along</param>
		/// <returns>true if the user was found and the ad could be added to
		/// the send queue.  false otherwise.</returns>
		[WebMethod]
		public bool SendAd(User u, AdMessage ad)
		{
			ArrayList arr;

			// if the user is in the list, add this ad to the user's list
			if ( Global.m_users.Contains(u) && Global.m_ads_by_user.Contains(u))
			{
				// index the ads by user list
				arr = Global.m_ads_by_user[u] as ArrayList;

				// add it to the list
				arr.Add(ad);

				return true;
			}
			else
			{
				return false;
			}
		}
		#endregion

		#region Insecure operations - for testing purposes only
		/// <summary>
		/// Insecure way to fetch the user list from the GIS
		/// </summary>
		/// <param name="merch">The Merchant who is requesting it</param>
		/// <returns>List of users if the merchant is already registered.  Null otherwise.</returns>
		[WebMethod]
		public ArrayList InsecureGetUsers(Merchant merch)
		{
			// check if the name is on our list and the cert is current
			if ( Global.m_insecure.Contains( merch))
			{			
				return Global.m_users;
			}
			else
			{
				return null;
			}
		}
		/// <summary>
		/// Insecure method for registering Merchants for future retrieval of user data
		/// </summary>
		/// <param name="merch">Merchant information</param>
		/// <param name="policy">P3P Policy for the Merchant</param>
		/// <returns>True if the P3P policy is acceptable and the merchant has been registered.
		/// False otherwise.</returns>
		[WebMethod]
		public bool InsecureRegisterMerchant(Merchant merch, P3P.POLICY policy)
		{
			// take the policy and compare it to our own
			// if it's better or equal then we accept it
			if ( policy.equals(Global.m_policy) )
			{
				// add to accept list
				Global.m_insecure.Add(merch);
				return true;
			}
			else
			{
				// unacceptable policy
				return false;
			}
		}
		#endregion

		#region deprecated
		/// <summary>
		/// Runs a thread that creates new threads to listen for and handle socket connections.
		/// </summary>
/*		protected void RunListeners()
		{
			// start the TCP listener working on port 8080 on the local host
			System.Net.IPHostEntry info = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName());
			m_tcp_server = new System.Net.Sockets.TcpListener(info.AddressList[0],  AD_PORT);

			System.Net.Sockets.TcpClient client;
			AdServerThread newThread;

			// we must keep the TCP listener running, accepting connections
			// and farming out the connections as they come
			this.m_tcp_server.Start();

			try
			{
				// run the listen and load loop forever
				while (true)
				{
					// wait for the next connection
					client = this.m_tcp_server.AcceptTcpClient();

					// now that we have the connection, make a thread
					// that will handle the connections
					newThread = new AdServerThread(client);

					// add the thread object to the array
					this.m_connections.Add(newThread);

					// now run the thread
					(new System.Threading.Thread(new System.Threading.ThreadStart(newThread.RunAdServerThread))).Start();
				}
			}
			// catch any error and make sure that we close down safely
			finally
			{
				// we're done listening, so kill all

				// stop listening
				this.m_tcp_server.Stop();

				// close all of the connections
				AdServerThread ast;
				foreach ( Object obj in this.m_connections )
				{
					ast = obj as AdServerThread;

					ast.Client.Close();
				}
			}
		}*/
		#endregion
			

	}
}
