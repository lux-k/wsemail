using System;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using DynamicForms;
using System.IO;

namespace WSEmailClientv2
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
		string filename = Application.StartupPath + @"\addressbook.xml";
		/// <summary>
		/// Whether or not the addressbook needs to write out a new version when closing
		/// </summary>
		bool Dirty = false;
		/// <summary>
		/// Whether the address book has been loaded (so that infinite loops can be avoided).
		/// </summary>
		public static bool Loaded = false;
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
					FileStream f = File.Create(filename);
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
			if (File.Exists(filename)) 
			{
				XmlDocument d = new XmlDocument();
				
				try 
				{
					d.Load(filename);
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

		/// <summary>
		/// Adds a new entry to the address book, checking for duplicates
		/// </summary>
		/// <param name="a"></param>
		public void AddEntry(AddressBookEntry a) 
		{
			bool isin = DetectDuplicate(a);
			if (!isin) 
			{
				thebook.Add(a);
				MessageBox.Show("Added " + a.Email + " to your address book.");
				this.SetDirty();
				if (this.Updated != null)
					Updated();
			}
			else
				MessageBox.Show("The email address " + a.Email + " is already in your address book.");
		}

		/// <summary>
		/// Removes an address book entry.
		/// </summary>
		/// <param name="e"></param>
		public void RemoveEntry(AddressBookEntry e) 
		{
			thebook.Remove(e);
			if (this.Updated != null)
				Updated();
		}

		/// <summary>
		/// Returns true if the entry is already in the book.
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		private bool DetectDuplicate(AddressBookEntry a) 
		{
			foreach (object o in thebook) 
			{
				AddressBookEntry e = (AddressBookEntry)o;
				if (e.Email.Equals(a.Email))
					return true;
			}
			return false;
		}
	}
}
