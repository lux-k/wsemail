using System;

namespace StoreService
{
	/// <summary>
	/// Geographic latitude and longitude coordinates
	/// </summary>
	public class CLoc
	{
		public CLoc()
		{
			m_lat = new CCoord();
			m_long = new CCoord();
		}

		protected CCoord m_lat;
		protected CCoord m_long;

		/// <summary>
		/// Latitude which must be either North or South
		/// </summary>
		public CCoord Latitude
		{
			get
			{
				return m_lat;
			}
			set
			{
				if ( value.Dir == Direction.NORTH || value.Dir == Direction.SOUTH )
				{
					m_lat = value;
				}
				else
				{
					throw new ArgumentException("Latitude must be either North or South");
					//m_lat = value;
					//m_lat.Dir = Direction.EAST;
				}
			}
		}

		/// <summary>
		/// Longitude which must be either East of West
		/// </summary>
		public CCoord Longitude
		{
			get
			{
				return m_long;
			}
			set
			{
				if ( value.Dir == Direction.EAST || value.Dir == Direction.WEST )
				{
					m_long = value;
				}
				else
				{
					throw new ArgumentException("Longitude must be either East or West");
					//m_long = value;
					//m_long.Dir = Direction.EAST;
				}
			}
		}		

	}
}
