using System;

namespace Merchant
{
	/// <summary>
	/// Represents a GIS that can be queried and kept in a list
	/// </summary>
	public class GISServer
	{
		public GISServer()
		{
			// initialize them
			m_name = "";
			m_url = new Uri("http://localhost/");
			m_location = "";
			// get a GIS web service proxy with WS extensions
			m_gis_proxy = new Merchant.GIS.GISWse();
			m_registered = false;
		}

		#region Variables
		/// <summary>
		/// Name of the GIS
		/// </summary>
		protected string m_name;
		/// <summary>
		/// The URI for the GIS
		/// </summary>
		protected System.Uri m_url;
		/// <summary>
		/// Geographic location for the GIS
		/// </summary>
		protected string m_location;
		/// <summary>
		/// Web Service proxy object to talk to this GIS via webservice call
		/// </summary>
		protected Merchant.GIS.GISWse m_gis_proxy;
		/// <summary>
		/// Whether or not we have been registered with this GIS or not
		/// </summary>
		protected bool m_registered;
		#endregion

		#region Properties
		/// <summary>
		/// The name of the GIS Server
		/// </summary>
		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}

		/// <summary>
		/// The URI for the GIS Server as a string
		/// </summary>
		public string Url_String
		{
			get { return m_url.AbsoluteUri; }
			set 
			{
				// if there is no http in the beginning - add it
				if ( !value.StartsWith("http://") )
				{
					value = "http://" + value;
				}

				// if it doesn't end in a /, add it
				if ( !value.EndsWith("/") )
				{
					value = value + "/";
				}

				m_url = new Uri(value);
			}
		}

		/// <summary>
		/// The URI for the GIS Server
		/// </summary>
		public System.Uri Url
		{
			get { return m_url; }
			set { m_url = value; }
		}

		/// <summary>
		/// Geographic location as a string
		/// </summary>
		public string Location
		{
			get { return m_location; }
			set { m_location = value; }
		}

		/// <summary>
		/// The Web Service Proxy object to contact this GIS
		/// </summary>
		public Merchant.GIS.GISWse Proxy
		{
			get { return m_gis_proxy; }
			set { m_gis_proxy = value; }
		}

		/// <summary>
		/// Whether or not we have registered with this GIS to retrieve user lists
		/// </summary>
		public bool Registered
		{
			get { return m_registered; }
			set { m_registered = value; }
		}
		#endregion

		/// <summary>
		/// Converts the GIS to some meaningful string representation
		/// </summary>
		/// <returns>GIS represented as a string</returns>
		public override string ToString()
		{
			// turn this into a string representation
			string t = "";

			// name
			t += m_name;
			t += ": ";

			// location
			t += this.m_location;
			t += ": ";

			// address
			t += this.m_url;
			
			return t;
		}

	}
}
