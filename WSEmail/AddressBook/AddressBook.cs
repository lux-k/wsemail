/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using DynamicForms;
using System.IO;

namespace XmlAddressBook
{
	/// <summary>
	/// The address book is really a collection of addressbookentries and some maintenance things. This
	/// is implemented as a singleton class.
	/// </summary>
	public class AddressBook : IDisposable
	{
		/// <summary>
		/// A reference to the address book
		/// </summary>
		static AddressBook _me = null;
		/// <summary>
		/// Behind the address book is an array list holding the items.
		/// </summary>
		ArrayList thebook = new ArrayList();
		/// <summary>
		/// Whether or not this is disposing
		/// </summary>
		bool Disposing = false;
		/// <summary>
		/// The filename of the address book.
		/// </summary>
		public static string Filename = Application.StartupPath + @"\addressbook.xml";
		/// <summary>
		/// Whether or not the addressbook needs to write out a new version when closing
		/// </summary>
		bool Dirty = false;
		/// <summary>
		/// Whether the address book has been loaded (so that infinite loops can be avoided).
		/// </summary>
		public static bool Loaded = false;
		public static bool KeepDeleted = false;
		/// <summary>
		/// Fired when addresses are deleted/added.
		/// </summary>
		public event AddressBookChange Updated;

		/// <summary>
		/// Creates an instance of the address book or returns the one already created.
		/// </summary>
		/// <returns></returns>
		public static AddressBook GetInstance() 
		{
			if (_me == null) 
			{
				_me = new AddressBook();
				Loaded = true;
			}
	
			return _me;
		}

		/// <summary>
		/// Sets the dirty flag to true
		/// </summary>
		public void SetDirty() 
		{
			Dirty = true;
		}

		/// <summary>
		/// We want to make sure dispose gets called, so put it in the destructor.
		/// </summary>
		~AddressBook() 
		{
			this.Dispose();
		}

		/// <summary>
		/// IDisposable implementation
		/// </summary>
		public void Dispose() 
		{
			if (!Disposing) 
			{
				Disposing = true;
				if (Dirty) 
				{
					FileStream f = File.Create(Filename);
					byte[] bytes = ObjectLoader.Serialize(typeof(AddressBookEntry[]),this.GetAllEntries()).ToArray();
					f.Write(bytes,0,bytes.Length);
					f.Flush();
					f.Close();
				}
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>
		/// Creates an address book by loading it as XML.
		/// </summary>
		public AddressBook() 
		{
			if (File.Exists(Filename)) 
			{
				XmlDocument d = new XmlDocument();
				
				try 
				{
					d.Load(Filename);
					thebook.AddRange((AddressBookEntry[])ObjectLoader.Deserialize(typeof(AddressBookEntry[]),d.OuterXml));
				} 
				catch 
				{
					//silenty ignore errors here.
				}
			}
		}

		/// <summary>
		/// Returns all the address book entries, sorted.
		/// </summary>
		/// <returns></returns>
		public AddressBookEntry[] GetAllEntries() 
		{
			Array a = (AddressBookEntry[])thebook.ToArray(typeof(AddressBookEntry));
			Array.Sort(a);
				
			return (AddressBookEntry[])a;
		}

		public bool AddEntry(XmlElement e) 
		{
			AddressBookEntry[] es = (AddressBookEntry[])ObjectLoader.Deserialize(typeof(AddressBookEntry[]),e.OuterXml);
			
			foreach (AddressBookEntry a in es) 
			{
				if (a.Deleted) 
				{
					AddressBookEntry temp = GetEntry(a.Email);
					if (temp != null)
						RemoveEntry(temp);
				}
				else
					AddEntry(a);
			}
			return true;

		}
		/// <summary>
		/// Adds a new entry to the address book, checking for duplicates
		/// </summary>
		/// <param name="a"></param>
		public bool AddEntry(AddressBookEntry a) 
		{
			bool isin = DetectDuplicate(a);
			if (!isin) 
			{
				thebook.Add(a);
			//	
				
				this.SetDirty();
				if (this.Updated != null)
					Updated();
				return true;
			}
			else
				return false;

			//	
		}

		/// <summary>
		/// Removes an address book entry.
		/// </summary>
		/// <param name="e"></param>
		public void RemoveEntry(AddressBookEntry e) 
		{
			e.Deleted = true;
			e.ModifiedDate = DateTime.Now;

			if (!KeepDeleted)
				thebook.Remove(e);

			if (this.Updated != null)
				Updated();
		}

		/// <summary>
		/// Returns true if the entry is already in the book.
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public bool DetectDuplicate(AddressBookEntry a) 
		{
			foreach (object o in thebook) 
			{
				AddressBookEntry e = (AddressBookEntry)o;
				if (e.Email.Equals(a.Email))
					return true;
			}
			return false;
		}

		public AddressBookEntry GetEntry(string email) 
		{
			email = email.ToLower();
			foreach (object o in thebook) 
			{
				AddressBookEntry e = (AddressBookEntry)o;
				if (e.Email.ToLower().Equals(email))
					return e;
			}
			return null;
		}

		public AddressBookEntry[] GetEntries(string t) 
		{
			ArrayList a = new ArrayList();

			foreach (object o in thebook) 
			{
				AddressBookEntry e = (AddressBookEntry)o;
				if (e.Email.ToLower().StartsWith(t))
					a.Add(e);
			}

			return (AddressBookEntry[])a.ToArray(typeof(AddressBookEntry));
		}

		public AddressBookEntry[] GetEntries(DateTime t) 
		{
			ArrayList a = new ArrayList();

			foreach (object o in thebook) 
			{
				AddressBookEntry e = (AddressBookEntry)o;
				if (e.ModifiedDate >= t)
					a.Add(e);
			}

			return (AddressBookEntry[])a.ToArray(typeof(AddressBookEntry));
		}
	}
}
