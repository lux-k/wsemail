using System;

namespace CommonTypes
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

		/// <summary>
		/// Compares two CommonTypes.Merchant object together.
		/// </summary>
		/// <param name="obj">The CommonTypes.Merchant object to compare against</param>
		/// <returns>true if the two have the same name. false othewise</returns>
		public override bool Equals(object obj)
		{
			// make sure this is the same type
			CommonTypes.Merchant rhs = obj as CommonTypes.Merchant;

			if (rhs == null ) return false;

			// it's the same, so compare by name
			return rhs.m_name.Equals(this.m_name);		
		}

	}
}
