using System;
using System.Xml;

namespace P3P
{
	/// <summary>
	/// REMEDIES element from DISPUTES-GROUP
	/// </summary>
	public class REMEDIES
	{
		public REMEDIES()
		{
			m_correct = false;
			m_money = false;
			m_law = false;
		}

		#region Variables
		protected bool m_correct;
		protected bool m_money;
		protected bool m_law;
		#endregion

		#region Properties
		public bool correct
		{
			get { return m_correct; }
			set { m_correct = value; }
		}

		public bool money
		{
			get { return m_money; }
			set { m_money = value; }
		}

		public bool law
		{
			get { return m_law; }
			set { m_law = value; }
		}
		#endregion

		#region Comparison
		/// <summary>
		/// Compares another REMEDIES to this one
		/// </summary>
		/// <param name="compareTo">REMEDIES to compare this one to</param>
		/// <returns>true if REMEDIESes are identical.  false otherwise</returns>
		public bool equals(REMEDIES compareTo)
		{
			bool same = true;

			if ( this.correct != compareTo.correct )
			{
				same = false;
			}
			
			if ( this.law != compareTo.law )
			{
				same = false;
			}

			if ( this.money != compareTo.money )
			{
				same = false;
			}

			return same;
		}
		#endregion

		/// <summary>
		/// Creates an XmlElement object with the REMEDIES' properties translated into XML
		/// </summary>
		/// <returns>XmlElement with properties as valid P3P XML</returns>
		public System.Xml.XmlElement toXML()
		{
			// turn this object into an xml element
			System.Xml.XmlElement elem;
			
			System.Xml.XmlDocument doc = new XmlDocument();
			doc.LoadXml("<REMEDIES></REMEDIES>");
			elem = doc.DocumentElement;

			// put in children based on the existing attributes
			if ( this.m_correct )
			{
				doc.LoadXml("<correct/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_law )
			{
				doc.LoadXml("<law/>");
				elem.AppendChild(doc.DocumentElement);
			}

			if ( this.m_money )
			{
				doc.LoadXml("<money/>");
				elem.AppendChild(doc.DocumentElement);
			}

			// now that we have a fully built Xml Element, send it back
			return elem;
		}
	
	}
}
