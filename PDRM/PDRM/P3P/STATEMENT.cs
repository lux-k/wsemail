using System;
using System.Xml;

namespace P3P
{
	/// <summary>
	/// Statement element
	/// </summary>
	public class STATEMENT
	{
		public STATEMENT()
		{
			this.m_consequence = "";
			this.m_nonidentifiable = false;
			this.m_purpose = new PURPOSE();
			this.m_recipient = new RECIPIENT();
			this.m_retention = new RETENTION();
			this.m_categories = new CATEGORIES();			
		}

		#region Variables
		protected string m_consequence;
		protected bool m_nonidentifiable;
		protected PURPOSE m_purpose;
		protected RECIPIENT m_recipient;
		protected RETENTION m_retention;
		/// <summary>
		/// Collected CATEGORIES of data transfer from DATA-GROUP and DATA elements.
		/// This may need some work...
		/// </summary>
		protected CATEGORIES m_categories;
		#endregion
		
		#region Properties
		public string consequence
		{
			get { return m_consequence; }
			set { m_consequence = value; }
		}

		public bool nonidentifiable
		{
			get { return m_nonidentifiable; }
			set { m_nonidentifiable = value; }
		}

		public PURPOSE purpose
		{
			get { return m_purpose; }
			set { m_purpose = value; }
		}

		public RECIPIENT recipient
		{
			get { return m_recipient; }
			set { m_recipient = value; }
		}

		public RETENTION retention
		{
			get { return m_retention; }
			set { m_retention = value; }
		}

		public CATEGORIES categories
		{
			get { return m_categories; }
			set { m_categories = value; }
		}
		#endregion

		#region Comparison
		/// <summary>
		/// Compares another STATEMENT to this one
		/// </summary>
		/// <param name="compareTo">STATEMENT to compare this one to</param>
		/// <returns>true if STATEMENTs are identical.  false otherwise</returns>
		public bool equals(STATEMENT compareTo)
		{
			bool same = true;

			if ( !compareTo.m_categories.equals(this.m_categories) )
			{
				same = false;
			}
	
			// maybe this shouldn't be here...
			//if ( !compareTo.m_consequence.equals(this.m_consequence) )
			//{
			//	same = false;
			//}

			if ( compareTo.m_nonidentifiable != this.m_nonidentifiable )
			{
				same = false;
			}

			if ( !compareTo.m_purpose.equals(this.m_purpose) )
			{
				same = false;
			}

			if ( !compareTo.m_recipient.equals(this.m_recipient) )
			{
				same = false;
			}

			if ( !compareTo.m_retention.equals(this.m_retention) )
			{
				same = false;
			}

			return same;
		}
		#endregion

		/// <summary>
		/// Creates an XmlElement object with the STATEMENT's properties translated into XML
		/// </summary>
		/// <returns>XmlElement with properties as valid P3P XML</returns>
		public System.Xml.XmlElement toXML()
		{
			// turn this object into an xml document
			System.Xml.XmlDocument doc = new XmlDocument();
			// temp
			System.Xml.XmlElement elem;
			
			doc.LoadXml("<STATEMENT></STATEMENT>");

			// we must include the children of this statment as XML

			// nonidentifiable element
			if ( this.m_nonidentifiable )
			{
				elem = doc.CreateElement("nonidentifiable");
				elem.IsEmpty = true;
				doc.DocumentElement.AppendChild(elem);
			}

			// include CONSEQUENCE if it's not empty
			if (this.m_consequence != "" )
			{
				elem = doc.CreateElement("CONSEQUENCE");
				elem.InnerText = this.m_consequence;
				doc.DocumentElement.AppendChild(elem);
			}

			// PURPOSE subelement
			elem = this.m_purpose.toXML();
			elem = doc.ImportNode(elem, true) as XmlElement;
			doc.DocumentElement.AppendChild(elem);

			// RECIPIENT
			elem = this.m_recipient.toXML();
			elem = doc.ImportNode(elem, true) as XmlElement;
			doc.DocumentElement.AppendChild(elem);

			// RETENTION
			elem = this.m_retention.toXML();
			elem = doc.ImportNode(elem, true) as XmlElement;
			doc.DocumentElement.AppendChild(elem);

			// CATEGORIES
			elem = this.m_categories.toXML();
			elem = doc.ImportNode(elem, true) as XmlElement;
			doc.DocumentElement.AppendChild(elem);

			// now we have a full STATEMENT, so send it back
			return doc.DocumentElement;
		}	


	}
}
