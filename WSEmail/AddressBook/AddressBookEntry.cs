/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;

namespace XmlAddressBook
{
	/// <summary>
	/// Address book entry; contains the data about a particular user such as first name, last name, 
	/// email address and notes
	/// </summary>
	public class AddressBookEntry : IComparable
	{
		/// <summary>
		/// When changes are made to any of the properties, this is fired.
		/// </summary>
		public event AddressBookChange Changed;

		private string _first = "", _last = "", _email = "", _notes = "";

		/// <summary>
		/// The first name
		/// </summary>
		public string FirstName 
		{
			get 
			{
				return _first;
			}
			set 
			{
				_first = value;
				FireChange();
			}
		}

		/// <summary>
		/// The last name
		/// </summary>
		public string LastName 
		{
			get 
			{
				return _last;
			}
			set 
			{
				_last = value;
				FireChange();
			}
		}

		/// <summary>
		/// The email address
		/// </summary>
		public string Email 
		{
			get 
			{
				return  _email;
			}
			set 
			{
				_email = value;
				FireChange();
			}
		}

		/// <summary>
		/// The notes
		/// </summary>
		public string Notes 
		{
			get 
			{
				return _notes;
			}
			set 
			{
				_notes = value;
				FireChange();
			}
		}

		/// <summary>
		/// Date the entry was created
		/// </summary>
		public DateTime AddDate;
		public DateTime ModifiedDate;

		public bool Deleted 
		{
			get 
			{
				return _del;
			}
			set 
			{
				_del = value;
				FireChange();
			}
		}
		public bool _del = false;

		/// <summary>
		/// Fires a change and sets the addressbook to dirty so it'll be
		/// written to disk.
		/// </summary>
		private void FireChange() 
		{
			if (Changed != null)
				Changed();

			if (AddressBook.Loaded)
				AddressBook.GetInstance().SetDirty();
			
		}

		/// <summary>
		/// Default empty constructor needed for serialization to work
		/// </summary>
		public AddressBookEntry()
		{
		}
		
		/// <summary>
		/// Implements IComparable so that the entries can be sorted
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public int CompareTo(object x) 
		{
			if (x is AddressBookEntry) 
			{
				
				AddressBookEntry a = null;
				a = (AddressBookEntry)x;

				try 
				{
					if (this.LastName != null) 
					{
						if (this.LastName.Equals(a.LastName)) 
						{
							if (this.FirstName != null)
								return this.FirstName.CompareTo(a.FirstName);
							else
								return 1;
						} 
						else 
							return this.LastName.CompareTo(a.LastName);
					}
					else 
					{
						if (this.Email != null) 
							return this.Email.CompareTo(a.Email);
						else
							return 0;
					}

				}
				catch {	}
			} 
			
			throw new ArgumentException();
		}
		
	}


}
