using System;
using System.Xml;

namespace P3P
{
	/// <summary>
	/// Categories elements
	/// </summary>
	public class CATEGORIES
	{
		public CATEGORIES()
		{
			m_physical = false;
			m_online = false;
			m_uniqueid = false;
			m_purchase = false;
			m_financial = false;
			m_computer = false;
			m_navigation = false;
			m_interactive = false;
			m_demographic = false;
			m_content = false;
			m_state = false;
			m_political = false;
			m_health = false;
			m_preference = false;
			m_location = false;
			m_government = false;
			m_other_category = null;
		}
		#region Variables
		protected bool m_physical;
		protected bool m_online;
		protected bool m_uniqueid;
		protected bool m_purchase;
		protected bool m_financial;
		protected bool m_computer;
		protected bool m_navigation;
		protected bool m_interactive;
		protected bool m_demographic;
		protected bool m_content;
		protected bool m_state;
		protected bool m_political;
		protected bool m_health;
		protected bool m_preference;
		protected bool m_location;
		protected bool m_government;
		protected string m_other_category;
		#endregion

		#region Properties
		public bool physical
		{
			get { return m_physical; }
			set { m_physical = value; }
		}
		public bool online
		{
			get { return m_online; }
			set { m_online = value; }
		}
		public bool uniqueid
		{
			get { return m_uniqueid; }
			set { m_uniqueid = value; }
		}
		public bool purchase
		{
			get { return m_purchase; }
			set { m_purchase = value; }
		}
		public bool financial
		{
			get { return m_financial; }
			set { m_financial = value; }
		}
		public bool computer
		{
			get { return m_computer; }
			set { m_computer = value; }
		}
		public bool navigation
		{
			get { return m_navigation; }
			set { m_navigation = value; }
		}
		public bool interactive
		{
			get { return m_interactive; }
			set { m_interactive = value; }
		}
		public bool demographic
		{
			get { return m_demographic; }
			set { m_demographic = value; }
		}
		public bool content
		{
			get { return m_content; }
			set { m_content = value; }
		}
		public bool state
		{
			get { return m_state; }
			set { m_state = value; }
		}
		public bool political
		{
			get { return m_political; }
			set { m_political = value; }
		}
		public bool health
		{
			get { return m_health; }
			set { m_health = value; }
		}
		public bool preference
		{
			get { return m_preference; }
			set { m_preference = value; }
		}
		public bool location
		{
			get { return m_location; }
			set { m_location = value; }
		}
		public bool government
		{
			get { return m_government; }
			set { m_government = value; }
		}
		public string other_category
		{
			get { return m_other_category; }
			set { m_other_category = value; }
		}

		#endregion
		
		#region Comparison
		/// <summary>
		/// Compares another CATEGORIES to this one
		/// </summary>
		/// <param name="compareTo">CATEGORIES to compare this one to</param>
		/// <returns>true if CATEGORIESes are identical.  false otherwise</returns>		
		public bool equals(CATEGORIES compareTo)
		{
			bool same = true;

			if ( this.computer != compareTo.computer )
			{
				same = false;
			}
			
			if ( this.content != compareTo.content )
			{
				same = false;
			}

			if ( this.demographic != compareTo.demographic )
			{
				same = false;
			}

			if ( this.financial != compareTo.financial )
			{
				same = false;
			}

			if ( this.government != compareTo.government )
			{
				same = false;
			}

			if ( this.health != compareTo.health )
			{
				same = false;
			}

			if ( this.interactive != compareTo.interactive )
			{
				same = false;
			}

			if ( this.location != compareTo.location )
			{
				same = false;
			}

			if ( this.navigation != compareTo.navigation )
			{
				same = false;
			}

			if ( this.navigation != compareTo.navigation )
			{
				same = false;
			}
			if ( this.online != compareTo.online )
			{
				same = false;
			}

			if ( this.other_category != compareTo.other_category )
			{
				same = false;
			}

			if ( this.physical != compareTo.physical )
			{
				same = false;
			}

			if ( this.political != compareTo.political )
			{
				same = false;
			}
			if ( this.preference != compareTo.preference )
			{
				same = false;
			}

			if ( this.purchase != compareTo.purchase )
			{
				same = false;
			}

			if ( this.state != compareTo.state )
			{
				same = false;
			}

			if ( this.uniqueid != compareTo.uniqueid )
			{
				same = false;
			}

			return same;
		}

		#endregion

		/// <summary>
		/// Creates an XmlElement object with the CATEGORIES' properties translated into XML
		/// </summary>
		/// <returns>XmlElement with properties as valid P3P XML</returns>
		public System.Xml.XmlElement toXML()
		{
			// turn this object into an xml element
			XmlElement elem;
			
			XmlDocument doc = new XmlDocument();
			doc.LoadXml("<CATEGORIES></CATEGORIES>");
			elem = doc.DocumentElement;
			
			// put in children based on the existing attributes
			if ( this.m_computer )
			{
				doc.LoadXml("<computer/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_content )
			{
				doc.LoadXml("<content/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_demographic )
			{
				doc.LoadXml("<demographic/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_financial )
			{
				doc.LoadXml("<financial/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_government )
			{
				doc.LoadXml("<government/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_health )
			{
				doc.LoadXml("<health/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_health )
			{
				doc.LoadXml("<health/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_interactive )
			{
				 doc.LoadXml("<interactive/>");
				 elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_location )
			{
				  doc.LoadXml("<location/>");
				  elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_navigation )
			{
				doc.LoadXml("<navigation/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_online )
			{
				doc.LoadXml("<online/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_other_category != "")
			{
				doc.LoadXml("<other_category>" + this.m_other_category + "</other_category>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_physical )
			{
				doc.LoadXml("<physical/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_political )
			{
				doc.LoadXml("<political/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_preference )
			{
				doc.LoadXml("<preference/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_purchase )
			{
				doc.LoadXml("<purchase/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_state )
			{
				doc.LoadXml("<state/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_uniqueid )
			{
				doc.LoadXml("<uniqueid/>");
				elem.AppendChild(doc.DocumentElement);
			}

			// now that we have a fully built Xml Element, send it back
			return elem;
		}


	}
}
