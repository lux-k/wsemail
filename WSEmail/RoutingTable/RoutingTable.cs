/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System.Configuration;
using System.Xml;
using System.Collections;
using System.Net;
using System;


namespace WSERoutingTable
{

	/// <summary>
	/// A configuration object capable of reading the XML representation of a routing object.
	/// </summary>
	public class RouterConfigurationReader : IConfigurationSectionHandler 
	{

		/// <summary>
		/// Creates a new reader in a specified node and context.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="configContext"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		public object Create(object parent, object configContext, XmlNode section) 
		{
			if (section == null)
				return null;
			RoutingTable rt = new RoutingTable();
			foreach (XmlNode c in section.ChildNodes) 
			{

				switch (c.Name.ToLower())
				{
					case "options":
						//
						// Console.WriteLine("Found options node!");
						break;
					case "routes":
						foreach (XmlNode sc in c.ChildNodes) 
						{
							if (sc.Attributes != null)
								rt.addRoute((new Route(sc.Attributes)));
						}
						break;
				}
			}
			Console.Write(rt);
			return rt;
		}
	}

	/// <summary>
	/// Routing table object that defines routes for various web services and domains.
	/// </summary>
	public class RoutingTable
	{
		/// <summary>
		/// Holds the list of routes 
		/// </summary>
		private ArrayList routes = new ArrayList();

		public ArrayList Routes 
		{
			get 
			{
				return this.routes;
				//return (Route[])routes.ToArray(typeof(Route));
			}
			set 
			{
				routes = value;

//				routes.Clear();
//				routes.Add(value);
				this.ToString();
			}
		}
		/// <summary>
		/// Default constructor
		/// </summary>
		public RoutingTable()
		{
			populateLists();
		}

		/// <summary>
		/// Adds a new route to the routing table.
		/// </summary>
		/// <param name="r">Route to add</param>
		public void addRoute(Route r) 
		{
			routes.Add(r);
		}

		/// <summary>
		/// Finds the route to take, given the source and the destination. For our purposes, dest should
		/// be a URL and src should be a domain.
		/// </summary>
		/// <param name="src">Domain (ie. MailServerA)</param>
		/// <param name="dest">URL (ie. http://www/foo/)</param>
		/// <returns></returns>
		public Route findRoute(String src, String dest) 
		{
			// go throught all the routes, see what matches.
			ArrayList matches = new ArrayList();
			// collect multiple matches
			for (int i = 0; i < routes.Count; i++) 
			{
				if ( ((Route)routes[i]).matches(src, dest) )
					matches.Add((Route)routes[i]);
			}

			// no matches? there's no route.
			if (matches.Count == 0)
				return null;

			// 1 match? return it.
			if (matches.Count == 1)
				return (Route)matches[0];

			Route ret = null;
			foreach (Route r in (Route[])matches.ToArray(typeof(Route))) 
			{
				if (ret == null)
					ret = r;
				// if ret is a wildcard, but r is more specific.. use r instead
				else if (ret.Destination.Equals("*") && !r.Destination.Equals("*"))
					ret = r;
			}
			return ret;

		}

		/// <summary>
		/// Reserved for future use.
		/// </summary>
		private void populateLists() 
		{
		}

		/// <summary>
		/// Returns a string representing the Routing table.
		/// </summary>
		/// <returns>String</returns>
		public override string ToString() 
		{
			string output = null;
			output += "Dump of routing table\n";
			output += String.Format("{0,30}\t{1,25}\t{2,50}\t{3,5}\n","Source","Destination","Router","Accept");
			for (int i = 0; i < routes.Count; i++)
				output += routes[i].ToString();
			return output;
		}
	}
	
	/// <summary>
	/// Holds information for one route.
	/// </summary>
	public class Route
	{
		/// <summary>
		/// The source, destination, router to use and whether this route is enabled.
		/// </summary>
		private String _src, _dest, _router = "*";
		private bool _accept;
		
		public XmlNode Serialize(XmlDocument doc) 
		{
			XmlNode n = doc.ImportNode(doc.CreateElement("route"),true);
			XmlNode attr;
			
			attr = doc.CreateNode(XmlNodeType.Attribute,"src","");
			attr.InnerText = this.Source;
			n.Attributes.SetNamedItem(attr);

			attr = doc.CreateNode(XmlNodeType.Attribute,"dest","");
			attr.InnerText = this.Destination;
			n.Attributes.SetNamedItem(attr);

			attr = doc.CreateNode(XmlNodeType.Attribute,"router","");
			attr.InnerText = this.Router;
			n.Attributes.SetNamedItem(attr);
			
			attr = doc.CreateNode(XmlNodeType.Attribute,"accept","");
			if (this.Accept == true)
				attr.InnerText = "1";
			else
				attr.InnerText = "0";

			n.Attributes.SetNamedItem(attr);
			return n;

		}
		/// <summary>
		/// Creates a new route which will be read from XML.
		/// </summary>
		/// <param name="xac">XML collection containing route information.</param>
		public Route(XmlAttributeCollection xac) 
		{
			if (xac == null)
				Console.WriteLine("xac is null?");
			// find the source, destination and router
			Source = xac.GetNamedItem("src").Value;
			if (Source.Equals(""))
				Source = "*";
			Destination = xac.GetNamedItem("dest").Value;
			Router = xac.GetNamedItem("router").Value;
			// and then parse the accept flag.
			try 
			{
				Accept =  xac.GetNamedItem("accept").Value.Equals("1");
			} 
			catch 
			{
				Accept = false;
			}
		}

		public Route() 
		{
		}

		/// <summary>
		/// The source of the route. This will be a domain.
		/// </summary>
		public string Source 
		{
			get 
			{
				return _src;
			}
			set 
			{
				_src = value;
			}
		}

		/// <summary>
		/// The destination of the route. This will be a URL.
		/// </summary>
		public string Destination 
		{
			get 
			{
				return _dest;
			}
			set 
			{
				Console.WriteLine("Overwrote destination!");
				_dest = value;
			}
		}

		/// <summary>
		/// Router to use for the given source and destination. This will be a URL.
		/// </summary>
		public string Router 
		{
			get 
			{
				return _router;
			}
			set 
			{
				_router = value;
			}
		}

		/// <summary>
		/// Whether or not to allow this route. Unallowed routes are not routed.
		/// </summary>
		public bool Accept 
		{
			get 
			{
				return _accept;
			}
			set 
			{
				_accept = value;
			}
		}

		/// <summary>
		/// Prints this route nicely to a string.
		/// </summary>
		/// <returns>String</returns>
		public override string ToString() 
		{
			return String.Format("{0,30}\t{1,25}\t{2,50}\t{3,5}\n",Source,Destination,Router,Accept);
		}

		/// <summary>
		/// Returns true if the source and destination given match this route. This is a caseless comparision.
		/// </summary>
		/// <param name="src">A domain</param>
		/// <param name="dest">A URL</param>
		/// <returns>True: if they match, false otherwise.</returns>
		public bool matches (string src, string dest) 
		{
			if (Source.ToLower().Equals("*") || Source.ToLower().Equals(src.ToLower())) 
			{
				if ( (Destination.ToLower().Equals(dest.ToLower()) || Destination.Equals("*")) && this.Accept == true)
					return true;
				else
					return false;
			}
			return false;
		}

		/// <summary>
		/// Disabled at the moment.
		/// </summary>
		/// <param name="src_ip"></param>
		/// <param name="dest_ip"></param>
		/// <returns></returns>
		public bool Matches(string src_ip, string dest_ip) 
		{
			//return (MatchesSource(src_ip) & MatchesDestination(dest_ip));
			return false;
		}

		/// <summary>
		/// Computes the network a host is on, given a net mask.
		/// </summary>
		/// <param name="src">The source address</param>
		/// <param name="mask">Net mask</param>
		/// <returns>The network</returns>
		private string ComputeNetwork(string src, string mask) 
		{
			long ip, nm;
			ip = PackNetworkNumber(src);
			nm = PackNetworkNumber(mask);
			ip &= nm;
			return UnPackNetworkNumber(ip);
		}

		/// <summary>
		/// Returns an IP address from a packed network number.
		/// </summary>
		/// <param name="net">Packed IP address</param>
		/// <returns>String form of IP address</returns>
		private string UnPackNetworkNumber(long net) 
		{
			IPAddress nn = new IPAddress(IPAddress.NetworkToHostOrder(net));
			return nn.ToString();
		}

		/// <summary>
		/// Packs an IP into a long number
		/// </summary>
		/// <param name="net">String containing the IP</param>
		/// <returns>Packed IP</returns>
		private long PackNetworkNumber(string net) 
		{
			IPAddress nn = IPAddress.Parse(net);
			return IPAddress.HostToNetworkOrder(nn.Address);
		}

	}
}
