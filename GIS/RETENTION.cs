using System;
using System.Xml;

namespace P3P
{
	/// <summary>
	/// Retention element
	/// </summary>
	public class RETENTION
	{
		public RETENTION()
		{
			m_no_retention = false;
			m_stated_purpose = false;
			m_legal_requirement = false;
			m_business_practices = false;
			m_indefinitely = false;
		}
		#region Variables
		protected bool m_no_retention;
		protected bool m_stated_purpose;
		protected bool m_legal_requirement;
		protected bool m_business_practices;
		protected bool m_indefinitely;
		#endregion

		#region Properties
		public bool no_retention
		{
			get { return m_no_retention; }
			set { m_no_retention = value; }
		}
		public bool stated_purpose
		{
			get { return m_stated_purpose; }
			set { m_stated_purpose = value; }
		}
		public bool legal_requirement
		{
			get { return m_legal_requirement; }
			set { m_legal_requirement = value; }
		}
		public bool business_practices
		{
			get { return m_business_practices; }
			set { m_business_practices = value; }
		}
		public bool indefinitely
		{
			get { return m_indefinitely; }
			set { m_indefinitely = value; }
		}
		#endregion

		#region Comparison
		/// <summary>
		/// Compares another RETENTION to this one
		/// </summary>
		/// <param name="compareTo">RETENTION to compare this one to</param>
		/// <returns>true if RETENTIONs are identical.  false otherwise</returns>
		public bool equals(RETENTION compareTo)
		{
			bool same = true;

			if ( compareTo.m_business_practices != this.m_business_practices)
			{
				same = false;
			}
			if ( compareTo.m_indefinitely != this.m_indefinitely)
			{
				same = false;
			}
			if ( compareTo.m_legal_requirement != this.m_legal_requirement)
			{
				same = false;
			}
			if ( compareTo.m_no_retention != this.m_no_retention)
			{
				same = false;
			}
			if ( compareTo.m_stated_purpose != this.m_stated_purpose)
			{
				same = false;
			}

			return same;
		}
		#endregion

		/// <summary>
		/// Creates an XmlElement object with the RETENTION's properties translated into XML
		/// </summary>
		/// <returns>XmlElement with properties as valid P3P XML</returns>
		public System.Xml.XmlElement toXML()
		{
			// turn this object into an xml element
			System.Xml.XmlElement elem;
			
			System.Xml.XmlDocument doc = new XmlDocument();
			doc.LoadXml("<RETENTION></RETENTION>");
			elem = doc.DocumentElement;

			// put in children based on the existing attributes
			if ( this.m_business_practices )
			{
				doc.LoadXml("<business_practices/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_indefinitely )
			{
				doc.LoadXml("<indefinitely/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_legal_requirement )
			{
				doc.LoadXml("<legal_requirement/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_no_retention )
			{
				doc.LoadXml("<no_retention/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_stated_purpose )
			{
				doc.LoadXml("<stated_purpose/>");
				elem.AppendChild(doc.DocumentElement);
			}

			// now that we have a fully built Xml Element, send it back
			return elem;
		}



	}
}
