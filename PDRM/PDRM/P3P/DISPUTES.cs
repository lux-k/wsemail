using System;
using System.Xml;

namespace P3P
{
	/// <summary>
	/// Types of resolution that can be done
	/// </summary>
	public enum RESOLUTION_TYPE {SERVICE = 0, INDEPENDENT, COURT, LAW};

	/// <summary>
	/// DISPUTES element from DISPUTES-GROUP
	/// </summary>
	public class DISPUTES
	{
		public DISPUTES()
		{
			m_remedies = new REMEDIES();
		}

		#region Variables
		protected REMEDIES m_remedies;
		protected RESOLUTION_TYPE m_resolution_type;
		protected string m_service;		
		#endregion

		#region Properties
		public REMEDIES remedies
		{
			get { return m_remedies; }
			set { m_remedies = value; }
		}
	
		public RESOLUTION_TYPE resolution_type
		{
			get { return m_resolution_type; }
			set { m_resolution_type = value; }
		}

		public string service
		{
			get { return m_service; }
			set { m_service = value; }
		}
		#endregion

		#region Comparison
		/// <summary>
		/// Compares another DISPUTES to this one
		/// </summary>
		/// <param name="compareTo">DISPUTES to compare this one to</param>
		/// <returns>true if DISPUTESes are identical.  false otherwise</returns>
		public bool equals(DISPUTES compareTo)
		{
			bool same = true;

			if ( this.remedies.equals(compareTo.remedies) == false )
			{
				same = false;
			}
			
			if ( this.resolution_type != compareTo.resolution_type )
			{
				same = false;
			}

			if ( this.service != compareTo.service )
			{
				same = false;
			}
			
			return same;
		}
		#endregion

		/// <summary>
		/// Creates an XmlElement object with the DISPUTE's properties translated into XML
		/// </summary>
		/// <returns>XmlElement with properties as valid P3P XML</returns>
		public System.Xml.XmlElement toXML()
		{
			// turn this object into an xml document
			XmlDocument doc = new XmlDocument();
			// temp
			XmlElement elem;
			
			// the DISPUTES element
			doc.LoadXml("<DISPUTES></DISPUTES>");

			//resolution-type attribute			
			switch(this.m_resolution_type)
			{
				case RESOLUTION_TYPE.COURT:
					doc.DocumentElement.SetAttribute("resolution-type", "court");
					break;

				case RESOLUTION_TYPE.INDEPENDENT:
					doc.DocumentElement.SetAttribute("resolution-type", "independent");
					break;
				case RESOLUTION_TYPE.LAW:
					doc.DocumentElement.SetAttribute("resolution-type", "law");
					break;
				case RESOLUTION_TYPE.SERVICE:
					doc.DocumentElement.SetAttribute("resolution-type", "service");
					break;
			}

			// service attribute
			doc.DocumentElement.SetAttribute("service", this.m_service);
			
			// append the remedies child
			elem = this.m_remedies.toXML();
			elem = doc.ImportNode(elem, true) as XmlElement;
			doc.DocumentElement.AppendChild(elem);

			// now that we have a fully built Xml document, send it back
			return doc.DocumentElement;
		}
	}
}
