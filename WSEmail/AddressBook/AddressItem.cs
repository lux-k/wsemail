/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Windows.Forms;

namespace XmlAddressBook
{
	/// <summary>
	/// A custom object to show an address book entry as a list view item.
	/// </summary>
	public class AddressItem : ListViewItem 
	{
		/// <summary>
		/// The reference to the actual address entry
		/// </summary>
		private AddressBookEntry _ent = null;

		/// <summary>
		/// Returns the address entry.
		/// </summary>
		public AddressBookEntry Address 
		{
			get 
			{
				return _ent;
			}
			set 
			{
				_ent = value;
			}
		}

		/// <summary>
		/// Creates a new address item and listens for changes to the address (so it can update the
		/// name, etc)
		/// </summary>
		/// <param name="e"></param>
		public AddressItem(AddressBookEntry e) 
		{
			this.Address = e;
			Address.Changed += new AddressBookChange(AddressChanged);
			this.Text = GetText();
		}

		/// <summary>
		/// Rewrites the caption when the address entry is changed.
		/// </summary>
		private void AddressChanged() 
		{
			this.Text = GetText();
		}

		/// <summary>
		/// Returns the caption of the entry.
		/// </summary>
		/// <returns></returns>
		private string GetText() 
		{
			if (_ent == null) return "<<null>>";
			if (_ent.LastName != null && _ent.LastName.Length > 0)
				return _ent.LastName + ", " + _ent.FirstName;
			return _ent.Email;
		}
	}
}
