using System;
using System.Xml;

namespace P3P
{
	/// <summary>
	/// PURPOSE element within STATEMENT
	/// </summary>
	public class PURPOSE
	{
		public PURPOSE()
		{
			m_current = false;
			m_admin = new Element_with_Required();
			m_develop = new Element_with_Required();
			m_tailoring = new Element_with_Required();
			m_pseudo_analysis = new Element_with_Required();
			m_pseudo_decision = new Element_with_Required();
			m_individual_analysis = new Element_with_Required();
			m_individual_decision = new Element_with_Required();
			m_contact = new Element_with_Required();
			m_telemarketing = new Element_with_Required();
			m_historical = new Element_with_Required();
			m_other_purpose = "";
			m_other_purpose_required = REQUIRED.always;
		}

		#region Variables
		protected bool m_current;
		protected Element_with_Required m_admin;
		protected Element_with_Required m_develop;
		protected Element_with_Required m_tailoring;
		protected Element_with_Required m_pseudo_analysis;
		protected Element_with_Required m_pseudo_decision;
		protected Element_with_Required m_individual_analysis;
		protected Element_with_Required m_individual_decision;
		protected Element_with_Required m_contact;
		protected Element_with_Required m_telemarketing;
		protected Element_with_Required m_historical;
		protected string m_other_purpose;
		protected REQUIRED m_other_purpose_required;
		#endregion

		#region Properties
		public bool current
		{
			get { return m_current; }
			set { m_current = value; }
		}
		public Element_with_Required admin
		{
			get { return m_admin; }
			set { m_admin = value; }
		}
		public Element_with_Required develop
		{
			get { return m_develop; }
			set { m_develop = value; }
		}
		public Element_with_Required tailoring
		{
			get { return m_tailoring; }
			set { m_tailoring = value; }
		}
		public Element_with_Required pseudo_analysis
		{
			get { return m_pseudo_analysis; }
			set { m_pseudo_analysis = value; }
		}
		public Element_with_Required pseudo_decision
		{
			get { return m_pseudo_decision; }
			set { m_pseudo_decision = value; }
		}
		public Element_with_Required individual_analysis
		{
			get { return m_individual_analysis; }
			set { m_individual_analysis = value; }
		}
		public Element_with_Required individual_decision
		{
			get { return m_individual_decision; }
			set { m_individual_decision = value; }
		}
		public Element_with_Required contact
		{
			get { return m_contact; }
			set { m_contact = value; }
		}
		public Element_with_Required telemarketing
		{
			get { return m_telemarketing; }
			set { m_telemarketing = value; }
		}
		public Element_with_Required historical
		{
			get { return m_historical; }
			set { m_historical = value; }
		}

		/// <summary>
		/// other-purpose element for Purpose.  NOTE: Setting this from a null string
		/// to a non-null string will cause the other_purpose_required property to change
		/// to the default value of "always". Changing from non-null to null, from non-null
		/// to non-null, or from null to null will not affect the value of 
		/// other_purpose_required.
		/// </summary>
		public string other_purpose
		 {
			 get { return m_other_purpose; }
			 set
			 {
				 if ( m_other_purpose == null && value != "")
				 {
					 // we are clearing setting a new value for the other-purpose field so
					 // change the other-purpose required field to the default value of
					 // always
					 m_other_purpose_required = REQUIRED.always;
				 }

				 m_other_purpose = value;
			 }
		 }

		/// <summary>
		/// required attribute for other-purpose element.
		/// </summary>
		public REQUIRED other_purpose_required
		{
			get { return m_other_purpose_required; }
			set
			{
				m_other_purpose_required = value;

			}
		}
		#endregion

		#region Comparison
		/// <summary>
		/// Compares another PURPOSE to this one
		/// </summary>
		/// <param name="compareTo">PURPOSE to compare this one to</param>
		/// <returns>true if PURPOSEs are identical.  false otherwise</returns>
		public bool equals(PURPOSE compareTo)
		{
			bool same = true;

			if ( !compareTo.m_admin.equals(this.m_admin ) )
			{
				same = false;
			}

			if ( !compareTo.m_contact.equals(this.m_contact))
			{
				same = false;
			}

			if ( compareTo.m_current != this.m_current )
			{
				same = false;
			}

			if ( !compareTo.m_develop.equals(this.m_develop))
			{
				same = false;
			}

			if ( !compareTo.m_historical.equals(this.m_historical))
			{
				same = false;
			}

			if ( !compareTo.m_individual_analysis.equals(this.m_individual_analysis))
			{
				same = false;
			}

			if ( !compareTo.m_individual_decision.equals(this.m_individual_decision))
			{
				same = false;
			}

			if ( !compareTo.m_other_purpose.Equals(this.m_other_purpose))
			{
				same = false;
			}

			if ( compareTo.m_other_purpose_required != this.m_other_purpose_required )
			{
				same = false;
			}

			if ( !compareTo.m_pseudo_analysis.equals(this.m_pseudo_analysis))
			{
				same = false;
			}

			if ( !compareTo.m_pseudo_decision.equals(this.m_pseudo_decision))
			{
				same = false;
			}

			if ( !compareTo.m_tailoring.equals(this.m_tailoring))
			{
				same = false;
			}

			if ( !compareTo.m_telemarketing.equals(this.m_telemarketing))
			{
				same = false;
			}

			return same;
		}
		#endregion

		/// <summary>
		/// Creates an XmlElement object with the PURPOSE's properties translated into XML
		/// </summary>
		/// <returns>XmlElement with properties as valid P3P XML</returns>
		public System.Xml.XmlElement toXML()
		{
			// turn this object into an xml element
			XmlElement elem;
			
			XmlDocument doc = new XmlDocument();
			// the PURPOSE element
			doc.LoadXml("<PURPOSE></PURPOSE>");
			elem = doc.DocumentElement;

			// for each possible child, define an element along with the required field
			// if appropriate
			#region m_current
			if ( this.m_current )
			{
				doc.LoadXml("<current/>");
				elem.AppendChild(doc.DocumentElement);
			}
			#endregion

			#region m_admin
			if ( this.m_admin.present )
			{
				doc.LoadXml("<admin/>");

				switch ( this.m_admin.required )
				{
					case REQUIRED.always:
						doc.DocumentElement.SetAttribute("required", "always");
						break;

					case REQUIRED.opt_in:
						doc.DocumentElement.SetAttribute("required", "opt-in");
						break;

					case REQUIRED.opt_out:
						doc.DocumentElement.SetAttribute("required", "opt-out");
						break;
				}

				elem.AppendChild(doc.DocumentElement);
			}
			#endregion

			#region m_contact
			if ( this.m_contact.present )
			{
				doc.LoadXml("<contact/>");

				switch ( this.m_contact.required )
				{
					case REQUIRED.always:
						doc.DocumentElement.SetAttribute("required", "always");
						break;

					case REQUIRED.opt_in:
						doc.DocumentElement.SetAttribute("required", "opt-in");
						break;

					case REQUIRED.opt_out:
						doc.DocumentElement.SetAttribute("required", "opt-out");
						break;
				}

				elem.AppendChild(doc.DocumentElement);
			}
			#endregion

			#region m_develop
			if ( this.m_develop.present )
			{
				doc.LoadXml("<develop/>");

				switch ( this.m_develop.required )
				{
					case REQUIRED.always:
						doc.DocumentElement.SetAttribute("required", "always");
						break;

					case REQUIRED.opt_in:
						doc.DocumentElement.SetAttribute("required", "opt-in");
						break;

					case REQUIRED.opt_out:
						doc.DocumentElement.SetAttribute("required", "opt-out");
						break;
				}

				elem.AppendChild(doc.DocumentElement);
			}
			#endregion

			#region m_historical
			if ( this.m_historical.present )
			{
				doc.LoadXml("<historical/>");

				switch ( this.m_historical.required )
				{
					case REQUIRED.always:
						doc.DocumentElement.SetAttribute("required", "always");
						break;

					case REQUIRED.opt_in:
						doc.DocumentElement.SetAttribute("required", "opt-in");
						break;

					case REQUIRED.opt_out:
						doc.DocumentElement.SetAttribute("required", "opt-out");
						break;
				}

				elem.AppendChild(doc.DocumentElement);
			}
			#endregion

			#region m_individual_analysis
			if ( this.m_individual_analysis.present )
			{
				doc.LoadXml("<individual_analysis/>");

				switch ( this.m_individual_analysis.required )
				{
					case REQUIRED.always:
						doc.DocumentElement.SetAttribute("required", "always");
						break;

					case REQUIRED.opt_in:
						doc.DocumentElement.SetAttribute("required", "opt-in");
						break;

					case REQUIRED.opt_out:
						doc.DocumentElement.SetAttribute("required", "opt-out");
						break;
				}

				elem.AppendChild(doc.DocumentElement);
			}
			#endregion

			#region m_individual_decision
			if ( this.m_individual_decision.present )
			{
				doc.LoadXml("<individual_decision/>");

				switch ( this.m_individual_decision.required )
				{
					case REQUIRED.always:
						doc.DocumentElement.SetAttribute("required", "always");
						break;

					case REQUIRED.opt_in:
						doc.DocumentElement.SetAttribute("required", "opt-in");
						break;

					case REQUIRED.opt_out:
						doc.DocumentElement.SetAttribute("required", "opt-out");
						break;
				}

				elem.AppendChild(doc.DocumentElement);
			}
			#endregion

			#region m_psuedo_analysis
			if ( this.m_pseudo_analysis.present )
			{
				doc.LoadXml("<pseudo_analysis/>");

				switch ( this.m_pseudo_analysis.required )
				{
					case REQUIRED.always:
						doc.DocumentElement.SetAttribute("required", "always");
						break;

					case REQUIRED.opt_in:
						doc.DocumentElement.SetAttribute("required", "opt-in");
						break;

					case REQUIRED.opt_out:
						doc.DocumentElement.SetAttribute("required", "opt-out");
						break;
				}

				elem.AppendChild(doc.DocumentElement);
			}
			#endregion

			#region m_pseudo_deicision
			if ( this.m_pseudo_decision.present )
			{
				doc.LoadXml("<pseudo_decision/>");

				switch ( this.m_pseudo_decision.required )
				{
					case REQUIRED.always:
						doc.DocumentElement.SetAttribute("required", "always");
						break;

					case REQUIRED.opt_in:
						doc.DocumentElement.SetAttribute("required", "opt-in");
						break;

					case REQUIRED.opt_out:
						doc.DocumentElement.SetAttribute("required", "opt-out");
						break;
				}

				elem.AppendChild(doc.DocumentElement);
			}
			#endregion

			#region m_tailoring
			if ( this.m_tailoring.present )
			{
				doc.LoadXml("<tailoring/>");

				switch ( this.m_tailoring.required )
				{
					case REQUIRED.always:
						doc.DocumentElement.SetAttribute("required", "always");
						break;

					case REQUIRED.opt_in:
						doc.DocumentElement.SetAttribute("required", "opt-in");
						break;

					case REQUIRED.opt_out:
						doc.DocumentElement.SetAttribute("required", "opt-out");
						break;
				}

				elem.AppendChild(doc.DocumentElement);
			}
			#endregion

			#region m_telemarketing
			if ( this.m_telemarketing.present )
			{
				doc.LoadXml("<telemarketing/>");

				switch ( this.m_telemarketing.required )
				{
					case REQUIRED.always:
						doc.DocumentElement.SetAttribute("required", "always");
						break;

					case REQUIRED.opt_in:
						doc.DocumentElement.SetAttribute("required", "opt-in");
						break;

					case REQUIRED.opt_out:
						doc.DocumentElement.SetAttribute("required", "opt-out");
						break;
				}

				elem.AppendChild(doc.DocumentElement);
			}
			#endregion

			#region m_other_purpose
			if ( this.m_other_purpose != "" )
			{
				doc.LoadXml("<other_purpose>" + this.m_other_purpose + "</other_purpose>");

				switch ( this.m_other_purpose_required )
				{
					case REQUIRED.always:
						doc.DocumentElement.SetAttribute("required", "always");
						break;

					case REQUIRED.opt_in:
						doc.DocumentElement.SetAttribute("required", "opt-in");
						break;

					case REQUIRED.opt_out:
						doc.DocumentElement.SetAttribute("required", "opt-out");
						break;
				}

				elem.AppendChild(doc.DocumentElement);
			}			
			#endregion

			// now that we have a fully built Xml Element, send it back
			return elem;
		}			
	}
}
