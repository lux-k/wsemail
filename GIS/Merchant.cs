using System;

namespace GIS
{
	/// <summary>
	/// Describes a Merchant's information.  Merchant may register with the GIS and then query
	/// it.  This must be removed for secure usage of the GIS.
	/// </summary>
	public class Merchant
	{
		public Merchant()
		{
			m_name = "";
		}

		/// <summary>
		/// Makes a new Merchant object
		/// </summary>
		/// <param name="name">Name of the Merchant</param>
		public Merchant(string name)
		{
			m_name = name;
		}

		/// <summary>
		/// Name of the merchant
		/// </summary>
		protected string m_name;

		/// <summary>
		/// Name of the merchant
		/// </summary>
		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}
	}
}
