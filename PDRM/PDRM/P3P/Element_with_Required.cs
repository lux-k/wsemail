using System;

namespace P3P
{
	/// <summary>
	/// enumeration for the 'required' attribute of PURPOSE elements
	/// </summary>
	public enum REQUIRED {always, opt_in, opt_out};

	/// <summary>
	/// PURPOSE element
	/// </summary>
	public class Element_with_Required
	{
		public Element_with_Required()
		{
			m_present = false;
			m_required = REQUIRED.always;		
		}

		/// <summary>
		/// Is this element here at all?
		/// </summary>
		protected bool m_present;
		public bool present
		{
			get { return m_present; }
			set { m_present = value; }
		}

		/// <summary>
		/// What, if any, 'required' attribute is there for this element.  Default is "always"
		/// </summary>
		protected REQUIRED m_required;
		public REQUIRED required
		{
			get { return m_required; }
			set { m_required = value; }
		}

		/// <summary>
		/// Compares another Element_with_Required to this one
		/// </summary>
		/// <param name="compareTo">Element_with_Required to compare this one to</param>
		/// <returns>true if Element_with_Requireds are identical.  false otherwise</returns>
		public bool equals(Element_with_Required compareTo)
		{
			bool same = true;

			if ( compareTo.m_present != this.m_present )
			{
				same = false;
			}

			if ( compareTo.m_required != this.m_required )
			{
				same = false;
			}

			return same;
		}


	}
}
