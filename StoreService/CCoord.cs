using System;

namespace StoreService
{
	public enum Direction {NORTH=0, SOUTH, EAST, WEST};

	/// <summary>
	/// Summary description for CCoord.
	/// </summary>
	public class CCoord
	{
		public CCoord()
		{
			m_dir = Direction.NORTH;
			m_deg = 0;
			m_min = 0;
			m_sec = 0;			
		}

		protected Direction m_dir;
		protected int m_deg;
		protected int m_min;
		protected int m_sec;

		/// <summary>
		/// Degrees
		/// </summary>
		public int Degrees
		{
			get
			{
				return m_deg;
			}
			set
			{
				if (value <= 180 && value >= -180)
					m_deg = value;
				else
					throw new System.ArgumentException("Cannot have degree outside of [-180, 180]");
			}
		}

		/// <summary>
		/// Minutes
		/// </summary>
		public int Minutes
		{
			get
			{
				return m_min;
			}
			set
			{
				if (value <= 59 && value >=0)
					m_min = value;
				else
					throw new System.ArgumentException("Cannot have minutes outside of [0, 60}");
			}
		}

		/// <summary>
		/// Seconds
		/// </summary>
		public int Seconds
		{
			get
			{
				return m_sec;
			}
			set
			{
				if (value <= 59 && value >= 0)
					m_sec = value;
				else
					throw new System.ArgumentException("Cannot have seconds outside of [0, 60}");
			}
		}

		/// <summary>
		/// Direction
		/// </summary>
		public Direction Dir
		{
			get
			{
				return m_dir;
			}
			set
			{
				if (value == Direction.NORTH || value == Direction.SOUTH ||
					value == Direction.EAST || value == Direction.WEST)
					m_dir = value;
				else
					throw new ArgumentException("Direction must be one of the cardinals: North, South, East, or West");
			}
		}
	}
}
