using System;
using Microsoft.Web.Services;
using Microsoft.Web.Services.Security;
using Microsoft.Web.Services.Security.X509;



namespace UserAgent
{
	/// <summary>
	/// A pairing of an X.509 Certificate (for ID) and a P3P policy that represents an offer
	/// </summary>
	public class Offer
	{
		public Offer()
		{
			m_policy = new P3P.POLICY();
		}

		/// <summary>
		/// X.509 Certificate of the Merchant making the offer
		/// </summary>
		protected Microsoft.Web.Services.Security.X509.X509Certificate m_cert;
		public Microsoft.Web.Services.Security.X509.X509Certificate Cert
		{
			get
			{
				return m_cert;
			}
			set
			{
				m_cert = value;
			}
		}

		/// <summary>
		/// P3P Policy of the Merchant making the offer
		/// </summary>
		protected P3P.POLICY m_policy;
		public P3P.POLICY Policy
		{
			get
			{
				return m_policy;
			}
			set
			{
				m_policy = value;
			}
		}

		/// <summary>
		/// Time that the offer was made
		/// </summary>
		protected System.DateTime m_time;
		public System.DateTime Time
		{
			get { return m_time; }
			set { m_time = value; }
		}

		/// <summary>
		/// URL for the LicenseDepot to deposit the license by.
		/// </summary>
		protected string m_url;
		public string Url
		{
			get { return m_url; }
			set { m_url = value; }
		}
		
	}
}
