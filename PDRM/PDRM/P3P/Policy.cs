using System;
using System.Xml;

namespace P3P
{
	/// <summary>
	/// Represents a stored P3P policy.  Maximum of 32 Statement and 32 Disputes elements.
	/// </summary>
	public class POLICY
	{
		public POLICY()
		{
			m_access = new ACCESS();

			m_disputes = new DISPUTES[32];
			this.m_disputes.Initialize();

			this.m_statements = new STATEMENT[32];
			this.m_statements.Initialize();
		}

		#region Variables
		protected ACCESS m_access;
		public DISPUTES[] m_disputes;
		public STATEMENT[] m_statements;
		#endregion

		#region Properties
		public ACCESS access
		{
			get { return m_access; }
			set { m_access = value; }
		}
		#endregion

		#region Comparison
		/// <summary>
		/// Compares another POLICY to this one
		/// </summary>
		/// <param name="compareTo">POLICY to compare this one to</param>
		/// <returns>true if POLICYs are identical.  false otherwise</returns>
		public bool equals(POLICY compareTo)
		{
			bool same = true;

			if ( !compareTo.m_access.equals(this.m_access))
			{
				same = false;
			}

			// for each disputes element in the compareTo one,
			// if there is an equal one in the local disputes collection
			// then we are equal. otherwise not
			foreach ( DISPUTES cmpD in compareTo.m_disputes )
			{
				// check for null
				if ( cmpD == null )
					break;

				bool found = false;

				foreach( DISPUTES m_d in this.m_disputes)
				{
					// check for null
					if ( m_d == null )
						break;

					if (cmpD.equals(m_d))
					{
						found = true;
						break;
					}
				}

				if ( !found )
				{
					same = false;
					break;
				}
			}

			// for each statement in the compareTo one,
			// if there is an equal one in the local statements collection
			// then we are equal. otherwise not
			foreach ( P3P.STATEMENT cmpS in compareTo.m_statements )
			{
				// check for null
				if ( cmpS == null )
					break;

				bool found = false;

				foreach( P3P.STATEMENT m_s in this.m_statements)
				{
					// check for null
					if ( m_s == null )
						break;

					if (cmpS.equals(m_s))
					{
						found = true;
						break;
					}
				}

				if ( !found )
				{
					same = false;
					break;
				}
			}

			return same;
		}
		#endregion

		/// <summary>
		/// Creates an XmlElement object with the POLICY's properties translated into XML
		/// </summary>
		/// <returns>XmlElement with properties as valid P3P XML</returns>
		public System.Xml.XmlElement toXML()
		{
			// turn this object into an xml document
			XmlDocument doc = new XmlDocument();
			// temp
			XmlElement elem;
			
			doc.LoadXml("<POLICY></POLICY>");
			
			// we must include the children of this statment as XML

			// ACCESS subelement
			elem = this.m_access.toXML();
			elem  = doc.ImportNode(elem, true) as XmlElement;
			doc.DocumentElement.AppendChild(elem);

			// DISPUTES
			foreach ( DISPUTES d in this.m_disputes)
			{
				if (d != null)
				{
					elem = d.toXML();
					elem = doc.ImportNode(elem, true) as XmlElement;
					doc.DocumentElement.AppendChild(elem);
				}
			}

			// STATMENTS
			foreach (STATEMENT s in this.m_statements)
			{
				if ( s != null)
				{
					elem = s.toXML();
					elem = doc.ImportNode(elem, true) as XmlElement;
					doc.DocumentElement.AppendChild(elem);
				}
			}

			// now we have a full POLICY, so send it back
			return doc.DocumentElement;
		}	


	}


}
