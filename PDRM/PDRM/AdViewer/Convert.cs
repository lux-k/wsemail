using System;

namespace AdViewer
{
	/// <summary>
	/// Summary description for Convert.
	/// </summary>
	public class Convert
	{
		public Convert()
		{
			
		}

		/// <summary>
		/// Convert from a User object in the GIS' Web Service interface format
		/// to one in the CommonTypes format.
		/// </summary>
		/// <param name="rhs">The User object in GIS' User format</param>
		/// <returns>User object in the CommonTypes format</returns>
		public static CommonTypes.User ToCommon(AdViewer.GIS.User rhs)
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
		/// <summary>
		/// Convert from a User object in the CommonTypes format
		/// to one in the GIS' Web Service format.
		/// </summary>
		/// <param name="rhs">The User object in CommonTypes User format</param>
		/// <returns>User object in the GIS' format</returns>
		public static AdViewer.GIS.User ToLocal(CommonTypes.User rhs)
		{
			AdViewer.GIS.User user = new AdViewer.GIS.User();

			// copy over the fields
			user.Address = rhs.Address;
			user.EmailAddress = rhs.EmailAddress;
			user.EntryTime = rhs.EntryTime;
			user.License = rhs.License as System.Xml.XmlDocument;
			user.Name = rhs.Name;

			// return the new one
			return user;
		}

		/// <summary>
		/// Convert from a AdMessage object in the CommonTypes format
		/// to one in the GIS' Web Service format
		/// </summary>
		/// <param name="rhs">The AdMessage object in the CommonTypes AdMessage format</param>
		/// <returns>AdMessage object in the GIS' format</returns>
		public static AdViewer.GIS.AdMessage ToLocal(CommonTypes.AdMessage rhs)
		{
			AdViewer.GIS.AdMessage ad = new AdViewer.GIS.AdMessage();

			// copy over the fields
			ad.AdText = rhs.AdText;
			ad.Source = rhs.Source;
			ad.Time = rhs.Time;
			switch (rhs.Type)
			{
				case CommonTypes.AdType.Discount:
					ad.Type = AdViewer.GIS.AdType.Discount;
					break;

				case CommonTypes.AdType.NormalAd:
					ad.Type = AdViewer.GIS.AdType.NormalAd;
					break;

				case CommonTypes.AdType.OffensiveAd:
					ad.Type = AdViewer.GIS.AdType.OffensiveAd;
					break;
			}

			//return the new one
			return ad;
		}

		/// <summary>
		/// Convert from a AdMessage object in the GIS' Web Service format
		/// to one in the CommonTypes format
		/// </summary>
		/// <param name="rhs">The AdMessage object in GIS' AdMessage format</param>
		/// <returns>AdMessage object in the CommonTypes format</returns>
		public static CommonTypes.AdMessage ToCommon(AdViewer.GIS.AdMessage rhs)
		{
			CommonTypes.AdMessage ad = new CommonTypes.AdMessage();

			// copy over the fields
			ad.AdText = rhs.AdText;
			ad.Source = rhs.Source;
			ad.Time = rhs.Time;
			switch (rhs.Type)
			{
				case AdViewer.GIS.AdType.Discount:
					ad.Type = CommonTypes.AdType.Discount;
					break;

				case AdViewer.GIS.AdType.NormalAd:
					ad.Type = CommonTypes.AdType.NormalAd;
					break;

				case AdViewer.GIS.AdType.OffensiveAd:
					ad.Type = CommonTypes.AdType.OffensiveAd;
					break;
			}

			//return the new one
			return ad;
		}
	}
}
