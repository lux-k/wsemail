using System;

namespace AdViewer
{
	/// <summary>
	/// Types of advertising messages that can be sent and displayed
	/// </summary>
	public enum AdType { NormalAd, OffensiveAd, Discount };
	
	/// <summary>
	/// Summary description for AdMessage.
	/// </summary>
	public class AdMessage
	{
		/// <summary>
		/// Default public constructor.  All properties must be set manually
		/// </summary>
		public AdMessage()
		{
			// set everything to empty
			this.m_adText = "";
			this.m_source = "";
			this.m_time = System.DateTime.Now;
			this.m_type = AdType.NormalAd;
		}

		/// <summary>
		/// Public constructor for an Advertisement
		/// </summary>
		/// <param name="text">The text of the advertisement</param>
		/// <param name="source">The Name or IP address of the company that sent the ad</param>
		/// <param name="type">Ad Type enum</param>
		/// <param name="time">Time the ad was received</param>
		public AdMessage(string text, string source, AdType type, DateTime time)
		{
			// set things up according to the parameters given
			this.m_adText = text;
			this.m_source = source;
			this.m_time = time;
			this.m_type = type;
		}

		/// <summary>
		/// Public constructor for an Advertisement.  Time of creation is set to Now.
		/// </summary>
		/// <param name="text">The text of the advertisement</param>
		/// <param name="source">The Name or IP address of the company that sent the ad</param>
		/// <param name="type">Ad Type enum</param>
		public AdMessage(string text, string source, AdType type)
		{
			// set things up according to the parameters given
			this.m_adText = text;
			this.m_source = source;
			this.m_type = type;

			// time is right now
			this.m_time = DateTime.Now;
		}

		/// <summary>
		/// Public constructor for an Advertisement.  Time of creation is set to be Now.
		/// </summary>
		/// <param name="adAsString">A string that contains a full AdMessage structure 
		/// turned into a string in the same way as by the AdMessage.ToString() method.</param>
		public AdMessage(string adAsString)
		{
			// parse this appropriately
			int firstLine, secondLine, thirdLine;
			firstLine = adAsString.IndexOf("\n");
			string t;

			// first line is the text
			this.m_adText = adAsString.Substring(0, firstLine);

			// next line
			secondLine = adAsString.IndexOf("\n", firstLine);

			// second line is the source
			this.m_source = adAsString.Substring(firstLine, secondLine-firstLine);

			// next line
			thirdLine = adAsString.IndexOf("\n", secondLine);

			// last line is the type
			t = adAsString.Substring(secondLine, thirdLine-secondLine);
			switch ( t )
			{
				case "NormalAd":
					this.m_type = AdType.NormalAd;
					break;

				case "OffensiveAd":
					this.m_type = AdType.OffensiveAd;
					break;

				case "Discount":
					this.m_type = AdType.Discount;
					break;
			}

			// set the time to be now
			this.m_time = DateTime.Now;
		}

		#region Variables
		/// <summary>
		/// The text of the ad
		/// </summary>
		protected string m_adText;

		/// <summary>
		/// The source - company/ip address of the ad
		/// </summary>
		protected string m_source;

		/// <summary>
		/// The time that the ad arrived
		/// </summary>
		protected System.DateTime m_time;

		/// <summary>
		/// The type of the ad
		/// </summary>
		protected AdType m_type;
		#endregion

		#region Properties
		/// <summary>
		/// The text of the Ad
		/// </summary>
		public string AdText
		{
			get { return m_adText; }
			set { m_adText = value; }
		}
		/// <summary>
		/// The IP address/Company name that sent the ad
		/// </summary>
		public string Source
		{
			get { return m_source; }
			set { m_source = value; }
		}
		/// <summary>
		/// When the ad arrived
		/// </summary>
		public System.DateTime Time
		{
			get { return m_time; }
			set { m_time = value; }
		}
		/// <summary>
		/// The type of the Ad
		/// </summary>
		public AdType Type
		{
			get { return  m_type; }
			set { m_type = value; }
		}
		/// <summary>
		/// Get the type of the advertisement in string form
		/// </summary>
		public string TypeString
		{
			get
			{
				switch (this.m_type)
				{
					case AdType.NormalAd:
						return "NormalAd";
						break;

					case AdType.OffensiveAd:
						return "OffensiveAd";
						break;

					case AdType.Discount:
						return "Discount";
						break;
				}

				return "Other";
			}
		}
		#endregion

		public override string ToString()
		{
			// convert this to a linear string
			string s = "";

			// text
			s += this.m_adText;
			s += "\n";

			// source
			s += this.m_source;
			s += "\n";

			// type
			switch ( this.m_type )
			{
				case AdType.Discount:
					s += "Discount";
					break;

				case AdType.NormalAd:
					s += "NormalAd";
					break;

				case AdType.OffensiveAd:
					s += "OffensiveAd";
					break;
			}

			// return the new string
			return s;
		}

	}
}
