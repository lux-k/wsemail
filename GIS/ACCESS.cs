using System;
using System.Xml;

namespace P3P
{
	/// <summary>
	/// ACCESS element for P3P
	/// </summary>
	public class ACCESS
	{
		public ACCESS()
		{
			m_nonident = false;
			m_all = false;
			m_contact_and_other = false;
			m_ident_contact = false;
			m_other_ident = false;
			m_none = false;
		}

		#region Variables
		protected bool m_nonident;
		protected bool m_all;
		protected bool m_contact_and_other;
		protected bool m_ident_contact;
		protected bool m_other_ident;
		protected bool m_none;
		#endregion

		#region Properties
		public bool nonident
		{
			get { return m_nonident; }
			set	{ m_nonident = value; }
		}

		public bool all
		{
			get { return m_all;	}
			set	{ m_all = value; }
		}

		public bool contact_and_other
		{
			get { return m_contact_and_other; }
			set { m_contact_and_other = value; }
		}

		public bool ident_contact
		{
			get { return m_ident_contact; }
			set { m_ident_contact = value; }
		}

		public bool other_ident
		{
			get { return m_other_ident; }
			set { m_other_ident = value; }
		}

		public bool none
		{
			get { return m_none; }
			set { m_none = value; }
		}

		#endregion

		#region Comparison
		/// <summary>
		/// Compares another ACCESS to this one
		/// </summary>
		/// <param name="compareTo">ACCESS to compare this one to</param>
		/// <returns>true if ACCESSes are identical.  false otherwise</returns>
		public bool equals(ACCESS compareTo)
		{
			bool same = true;

			if ( this.all != compareTo.all )
			{
				same = false;
			}
			
			if ( this.contact_and_other != compareTo.contact_and_other )
			{
				same = false;
			}

			if ( this.ident_contact != compareTo.ident_contact )
			{
				same = false;
			}

			if ( this.none != compareTo.none )
			{
				same = false;
			}

			if ( this.nonident != compareTo.nonident )
			{
				same = false;
			}

			if ( this.other_ident != compareTo.other_ident )
			{
				same = false;
			}

			return same;
		}

		#endregion

		/// <summary>
		/// Creates an XmlElement object with the ACCESS' properties translated into XML
		/// </summary>
		/// <returns>XmlElement with properties as valid P3P XML</returns>
		public System.Xml.XmlElement toXML()
		{
			// turn this object into an xml element
			XmlElement elem;
			
			XmlDocument doc = new XmlDocument();
			doc.LoadXml("<ACCESS></ACCESS>");
			elem = doc.DocumentElement;

			// put in children based on the existing attributes
			if ( this.m_nonident )
			{
				doc.LoadXml("<nonident/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_all )
			{
				doc.LoadXml("<all/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_contact_and_other )
			{
				doc.LoadXml("<contact_and_other/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_ident_contact )
			{
				doc.LoadXml("<ident_contact/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_none )
			{
				doc.LoadXml("<none/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_other_ident )
			{
				doc.LoadXml("<other_ident/>");
				elem.AppendChild(doc.DocumentElement);
			}

			// now that we have a fully built Xml Element, send it back
			return elem;
		}



	}
}
