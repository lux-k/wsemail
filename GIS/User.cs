using System;
using System.Xml;
using System.Xml.Serialization;

namespace GIS
{
	/// <summary>
	/// Represents a User that is listed in this GIS
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
		public DateTime EntryTime
		{
			get { return m_entryTime; }
			set { m_entryTime = value; }
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
		#endregion

		/// <summary>
		/// Parse this User into a string for display in list boxes
		/// </summary>
		/// <returns>String representation of this User</returns>
		public override string ToString()
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
		/// Compares an object to the User object.  Only the Name, Address, and EmailAddress
		/// fields are compared as the other ones are not User properties that would affect
		/// the equality of two Users.
		/// </summary>
		/// <param name="obj">Object to compare against</param>
		/// <returns>true if obj is a User and is the same, false otherwise.</returns>
		public override bool Equals(object obj)
		{
			bool same = true;

			// check if this is a User object
			if ( obj.GetType() != this.GetType() )
			{
				// just let the base method run on this one, it's not even the same type
				return base.Equals (obj);
			}
			else
			{
				// we have another User object, let's compare the items that we care about
				User u = obj as User;

				if ( u.Name != this.Name )
				{
					same = false;
				}

				if (u.Address != this.Address )
				{
					same = false;
				}

				if ( u.EmailAddress != this.EmailAddress )
				{
					same = false;
				}
			}

			// now return the result of our comparison
			return same;
		}

		/// <summary>
		/// Returns a Hash Code for the User object.  Just calls the base GetHashCode for
		/// Object.
		/// </summary>
		/// <returns>An integer Hash Code</returns>
		public override int GetHashCode()
		{
			// just pass the buck back to the base class
			return base.GetHashCode ();
		}
 

	}
}
