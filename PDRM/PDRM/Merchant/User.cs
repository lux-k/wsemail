using System;

namespace Merchant
{
	/// <summary>
	/// Represents a mobile User who can be contacted with ads
	/// </summary>
	public class User
	{
		public User()
		{
			// make some defaults
			m_name = "";
			m_address = "";
			m_entryTime = System.DateTime.Now;
			this.m_email_address = "";
			m_license = new System.Xml.XmlDocument();
			m_gis = new GIS.GIS();
		}

		#region Variables
		/// <summary>
		/// The name of the user of the device
		/// </summary>
		protected string m_name;
		/// <summary>
		/// The uniquely identifying address of the device
		/// </summary>
		protected string m_address;
		/// <summary>
		/// The time that this user device entered the GIS database
		/// </summary>
		protected System.DateTime m_entryTime;

		/// <summary>
		/// The WSEmail address at which the user can be reached to be
		/// asked for approval for contact
		/// </summary>
		protected string m_email_address;

		/// <summary>
		/// The license for contacting this particular device
		/// </summary>
		protected System.Xml.XmlDocument m_license;

		/// <summary>
		/// The GIS that the user is registered in
		/// </summary>
		protected GIS.GIS m_gis;
		#endregion

		#region Properties
		/// <summary>
		/// The name of the user of the device
		/// </summary>
		public string Name
		{
			get { return m_name; }
			set 
			{
				m_name = value;
			}
		}

		/// <summary>
		/// The uniquely identifying address of the device
		/// </summary>
		public string Address
		{
			get { return m_address; }
			set { m_address = value; }
		}

		/// <summary>
		/// The time that this user device entered the GIS database
		/// </summary>
		public System.DateTime EntryTime
		{
			get { return m_entryTime; }
			set { m_entryTime = value; }
		}
		/// <summary>
		/// The license for contacting this particular device
		/// </summary>
		public System.Xml.XmlDocument License
		{
			get { return m_license; }
			set { m_license = value; }
		}

		/// <summary>
		/// The WSEmail address at which the user can be reached to approve
		/// or reject offers.
		/// </summary>
		public string EmailAddress
		{
			get { return m_email_address;}
			set { m_email_address = value; }
		}

		/// <summary>
		/// The GIS that the user is registered in
		/// </summary>
		public GIS.GIS AtGIS
		{
			get { return m_gis; }
			set
			{
				m_gis = value;
			}
		}
		#endregion

		/// <summary>
		/// Parse this User into a string for display in list boxes
		/// </summary>
		/// <returns>String representation of this User</returns>
		override public string ToString()
		{
			string t = "";

			// name
			t += m_name;
			t += ": ";

			// address
			t += m_address;
			t += ": ";

			return t;
		}

		/// <summary>
		/// Compare two Merchant.Users to see if they are the same. Ignores the EntryTime,
		/// License, and GIS fields since they may change for the same user.
		/// </summary>
		/// <param name="obj">Object to compare against</param>
		/// <returns>true if they are identical.  false otherwise</returns>
		public override bool Equals(object obj)
		{
			// make sure we have a real Merchant.User here
			if ( (obj as Merchant.User) == null )
				return base.Equals(obj);
	
			// ok we have a valid one
			Merchant.User rhs = obj as Merchant.User;

			// compare against other users ignoring fields that are irrelevant
			bool same = true;

			if ( this.m_address != rhs.m_address )
			{
				same = false;
			}

			if ( this.m_email_address != rhs.m_email_address )
			{
				same = false;
			}

			if ( this.m_name != rhs.m_name )
			{
				same = false;
			}

			// return the result
			return same;		
		}
	}
}
