using System;

namespace Merchant
{
	/// <summary>
	/// Summary description for Convert.
	/// </summary>
	public class Convert
	{
		public Convert()
		{
		}

		public static CommonTypes.User ToCommon(Merchant.GIS.User rhs)
		{
			CommonTypes.User user = new CommonTypes.User();

			// copy over the fields
			user.Address = rhs.Address;
			user.EmailAddress = rhs.EmailAddress;
			user.EntryTime = rhs.EntryTime;
			user.License = rhs.License as System.Xml.XmlDocument;
			user.Name = rhs.Name;

			// return the new one
			return user;
		}

		public static Merchant.GIS.User ToLocal(CommonTypes.User rhs)
		{
			Merchant.GIS.User user = new Merchant.GIS.User();
			
			// copy over the fields
			user.Address = rhs.Address;
			user.EmailAddress = rhs.EmailAddress;
			user.EntryTime = rhs.EntryTime;
			user.License = rhs.License as System.Xml.XmlDocument;
			user.Name = rhs.Name;

			// return the new one
			return user;
		}

	}
}
