using System;
using System.Configuration;
using System.Xml;
using Microsoft.Web.Services;
using Microsoft.Web.Services.Routing;
using Microsoft.Web.Services.Security;
using Microsoft.Web.Services.Security.X509;
using System.Collections;
using PennLibraries;
using WSERoutingTable;
using WSEmailProxy;
using EventQueue;


namespace WSEMailRouter
{
	/// <summary>
	/// Routes WSEmailMessages based on a routing table in the router's configuration and the
	/// destination domain.
	/// </summary>
	/// 
	public class MailRouter : Microsoft.Web.Services.Routing.RoutingHandler
	{
	
		/// <summary>
		/// Gets the URL of a service, given a service name. Not used since the routing table was added.
		/// </summary>
		/// <param name="servName">ServiceName</param>
		/// <returns>Uri of service</returns>
		private Uri GetServiceURL(string servName)
		{
			string newURL = ConfigurationSettings.AppSettings[servName];
			if (newURL == null || newURL.Length == 0)
			{
				throw new ConfigurationException("There was no "+servName+" entry in the <appSettings> section of the router's configuration file.");
			}
			return new Uri(newURL);
		}
		
		/// <summary>
		/// Default constructor which doesn't do much.
		/// </summary>
		public MailRouter()
		{
		}

		/// <summary>
		/// Processes the message by loading a routing table, looking at the message's destination and
		/// trying to find a match.
		/// </summary>
		/// <param name="message">Message to process</param>
		/// <param name="outgoingPath">Message's outgoing path</param>
		protected override void ProcessRequestMessage(SoapEnvelope message, Path outgoingPath)
		{
			// load the routing table
			RoutingTable rt = (RoutingTable)ConfigurationSettings.GetConfig("RoutingTable");
			if (rt == null) 
			{
				throw new Exception("Unable to load routing table.");
			}

			// get the destination of the message
			string dest = ((XmlElement)message.GetElementsByTagName("Recipient")[0]).InnerText.ToLower();
			dest = dest.Substring(dest.IndexOf("@") + 1);

			// and get the message flags
			int messageFlags = System.Int32.Parse(((XmlElement)message.GetElementsByTagName("MessageFlags")[0]).InnerText);

			// this message is set to be a bulk delivered message do further processing
			if ( (messageFlags & WSEmailFlags.Precedence.BulkDelivery) == WSEmailFlags.Precedence.BulkDelivery) 
			{
				// now that we know it's bulk, we have to decide what to do with it
				// it may not, necessarily, be directly received from the client directly
				// that being the case, we need to get the signing certificate of the message
				XmlElement messageSig = (XmlElement)message.Body.GetElementsByTagName("MessageSignature")[0];
				byte[] buffer = Convert.FromBase64String( ((XmlElement)messageSig.GetElementsByTagName("X509Certificate")[0]).InnerText );
				Microsoft.Web.Services.Security.X509.X509Certificate cert = new Microsoft.Web.Services.Security.X509.X509Certificate(buffer);
				string emailaddr = Utilities.GetCertEmail(cert).ToLower();
				if (!emailaddr.EndsWith("upenn.edu"))
					dest = "HoldingTank";
				// if the cert isn't a upenn.edu one, then forward the message to the holding tank
				// otherwise, we'll let it go through the routing table.
				// with the original destination.
			}

			// find a route to the destination
			Route r = rt.findRoute(" ",dest);
			Utilities.LogEvent("Finding route to " + dest);

			// if theres a route, put it in the fwd path
			if (r != null) 
			{
				Utilities.LogEvent("Route through: " + r.Router);
				outgoingPath.Fwd.Insert(0, new Via(new Uri(r.Router)));
			}
			else 
			{
				// or whine that you don't know how to get there.
				Utilities.LogEvent("No route.");
				throw new Exception ("I don't know how to route to " + dest + "!");
			}

			/*
			if (((XmlElement)message.GetElementsByTagName("Recipient")[0]).InnerText.ToLower().EndsWith("mailservera")) 
			{
				outgoingPath.Fwd.Insert(0, new Via(GetServiceURL("MailEndPointA")));
				PennRoutingFilters.PennRoutingUtilities.LogEvent(this + " sending request to: " + GetServiceURL("MailEndPointA"));
			}
			else 
			{
				outgoingPath.Fwd.Insert(0, new Via(GetServiceURL("MailEndPointB")));
				PennRoutingFilters.PennRoutingUtilities.LogEvent(this + " sending request to: " + GetServiceURL("MailEndPointB"));
			}
			*/
		}
	}
}