using System;

namespace StoreService
{
	/// <summary>
	/// Description of a service that is available
	/// </summary>
	public class CServiceItem
	{
		public CServiceItem()
		{
			m_name = "";
			m_address = "";
			m_city = "";
			m_state = "";
			m_comments = "";
			m_phone = "";
		}

		protected string m_name;
		protected string m_address;
		protected string m_city;
		protected string m_state;
		protected string m_comments;
		protected string m_phone;

		/// <summary>
		/// Name of the store
		/// </summary>
		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				m_name = value;
			}
		}

		/// <summary>
		/// Address of the store
		/// </summary>
		public string Address
		{
			get
			{
				return m_address;
			}
			set
			{
				m_address = value;
			}
		}

		/// <summary>
		/// City of the store
		/// </summary>
		public string City
		{
			get
			{
				return m_city;
			}
			set
			{
				m_city = value;
			}
		}

		/// <summary>
		/// State
		/// </summary>
		public string State
		{
			get
			{
				return m_state;
			}
			set
			{
				m_state = value;
			}
		}

		/// <summary>
		/// Comments about the store
		/// </summary>
		public string Comments
		{
			get
			{
				return m_comments;
			}
			set
			{
				m_comments = value;
			}
		}

		/// <summary>
		/// Phone number for the store
		/// </summary>
		public string Phone
		{
			get
			{
				return m_phone;
			}
			set
			{
				m_phone = value;
			}
		}
		/// <summary>
		/// Converts the Service info into a short text summary
		/// </summary>
		/// <returns>Text summary of the Service information</returns>
		public override string ToString()
		{
			// make a meaningful string summary out of this service item
			string s = "";
			s += this.Name;
			s += " ";
			s += this.Address;
			s += " ";
			s += this.City;
			s += ", ";
			s += this.State;

			return s;
		}
			

	}
}
