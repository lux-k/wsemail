using System;
using System.Xml;

namespace P3P
{
	/// <summary>
	/// RECIPIENT element
	/// </summary>
	public class RECIPIENT
	{
		public RECIPIENT()
		{
			// initialize the subelements
			this.m_delivery = new Element_with_Required();
			this.m_same = new Element_with_Required();
			this.m_other_recipient = new Element_with_Required();
			this.m_unrelated = new Element_with_Required();
			this.m_public = new Element_with_Required();

			this.m_ours = false;
			this.m_delivery.present = false;
			this.m_same.present = false;
			this.m_other_recipient.present = false;
			this.m_unrelated.present = false;
			this.m_public.present = false;
		}
		#region Variables
		protected bool m_ours;
		protected Element_with_Required m_delivery;
		protected Element_with_Required m_same;
		protected Element_with_Required m_other_recipient;
		protected Element_with_Required m_unrelated;
		protected Element_with_Required m_public;
		#endregion

		#region Properties
		public bool ours
		{
			get { return m_ours; }
			set { m_ours = value; }
		}
		public Element_with_Required delivery
		{
			get { return m_delivery; }
			set { m_delivery = value; }
		}
		public Element_with_Required same		
		{
			get { return m_same; }
			set { m_same = value; }
		}
		public Element_with_Required other_recipient
		{
			get { return m_other_recipient; }
			set { m_other_recipient = value; }
		}
		public Element_with_Required unrelated		
		{
			get { return m_unrelated; }
			set { m_unrelated = value; }
		}
		public Element_with_Required _public
		{
			get { return m_public; }
			set { m_public = value; }
		}
		#endregion

		#region Comparison
		/// <summary>
		/// Compares another RECIPIENT to this one
		/// </summary>
		/// <param name="compareTo">RECIPIENT to compare this one to</param>
		/// <returns>true if RECIPIENTs are identical.  false otherwise</returns>
		public bool equals( RECIPIENT compareTo)
		{
			bool same = true;

			if ( !compareTo.m_delivery.equals(this.m_delivery) )
			{
				same = false;
			}
			if ( !compareTo.m_other_recipient.equals(this.m_other_recipient) )
			{
				same = false;
			}
			if ( compareTo.m_ours != this.m_ours )
			{
				same = false;
			}
			if ( !compareTo.m_public.equals(this.m_public) )
			{
				same = false;
			}			
			if ( !compareTo.m_same.equals(this.m_same) )
			{
				same = false;
			}
			if ( !compareTo.m_unrelated.equals(this.m_unrelated) )
			{
				same = false;
			}

			return same;
		}
		#endregion

		public System.Xml.XmlElement toXML()
		{
			// turn this object into an xml element
			System.Xml.XmlElement elem;
			
			System.Xml.XmlDocument doc = new XmlDocument();
			// the PURPOSE element
			doc.LoadXml("<RECIPIENT></RECIPIENT>");
			elem = doc.DocumentElement;

			// for each possible child, define an element along with the required field
			// if appropriate
			#region m_ours
			if ( this.m_ours )
			{
				doc.LoadXml("<ours/>");
				elem.AppendChild(doc.DocumentElement);
			}
			#endregion

			#region m_delivery
			if ( this.m_delivery.present )
			{
				doc.LoadXml("<delivery/>");

				switch ( this.m_delivery.required )
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

			#region m_same
			if ( this.m_same.present )
			{
				doc.LoadXml("<same/>");

				switch ( this.m_same.required )
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

			#region m_other_recipient
			if ( this.m_other_recipient.present )
			{
				doc.LoadXml("<other_recipient/>");

				switch ( this.m_other_recipient.required )
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

			#region m_unrelated
			if ( this.m_unrelated.present )
			{
				doc.LoadXml("<unrelated/>");

				switch ( this.m_unrelated.required )
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

			#region m_public
			if ( this.m_public.present )
			{
				doc.LoadXml("<public/>");

				switch ( this.m_public.required )
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
