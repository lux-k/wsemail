/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Collections;

namespace WSEmailProxy
{
	[Serializable]
	public class RecipientList : IList
	{
		private ArrayList thelist = null;

		/// <summary>
		/// Copys the recipient list to a destination array.
		/// </summary>
		/// <param name="array">Destination array</param>
		/// <param name="index">Start index in destination</param>
		/// 
		public void CopyTo(Array array, int index) 
		{
			for (int i = index; i < thelist.Count; i++) 
			{
				array.SetValue(thelist[i],i-index);
			}
		}

		/// <summary>
		/// Returns an enumerator.
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator() 
		{
			return thelist.GetEnumerator();
		}

		/// <summary>
		/// Returns whether this object is natively threadsafe
		/// </summary>
		public bool IsSynchronized 
		{
			get 
			{
				return false;
			}
		}

		/// <summary>
		/// Returns an object that can be used for synchronizing.
		/// </summary>
		public object SyncRoot 
		{
			get 
			{
				return thelist;
			}
		}

		/// <summary>
		/// Creates an empty list
		/// </summary>
		public RecipientList () 
		{
			thelist = new ArrayList();
		}

		/// <summary>
		/// Returns the number of recipients
		/// </summary>
		public int Count 
		{
			get 
			{
				return thelist.Count;
			}
		}

		/// <summary>
		/// Returns a string array of recipients
		/// </summary>
		[System.Xml.Serialization.XmlArray()]
		public string[] AllRecipients 
		{
			get 
			{
				if (thelist == null)
					thelist = new ArrayList();
				return (string[])thelist.ToArray(typeof(string));
			}
			set 
			{
				if (thelist == null)
					thelist = new ArrayList();
				thelist.Clear();
				thelist.AddRange(value);
			}
		}

		/// <summary>
		/// Returns whether this array is of a fixed size or not.
		/// </summary>
		public bool IsFixedSize 
		{
			get 
			{
				return false;
			}
		}

		/// <summary>
		/// Returns whether this array is read-only or not.
		/// </summary>
		public bool IsReadOnly 
		{
			get 
			{
				return false;
			}
		}

		/// <summary>
		/// Validates that the index is correct for the current list.
		/// </summary>
		/// <param name="i"></param>
		private void ValidateIndex(int i) 
		{
			if (thelist == null || i >= thelist.Count || i < 0)
				throw new Exception("Invalid index " + i.ToString());
		}

		/// <summary>
		/// Makes sure an object is a string.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		private string ValidateObject(object o) 
		{
			if (! (o is string) )
				throw new Exception("You may only add strings to a recipient list!");
			return (string)o;
		}

		/// <summary>
		/// Exposes an indexer method.
		/// </summary>
		public object this[int i] 
		{
			get 
			{
				ValidateIndex(i);
				return thelist[i];
			}
			set 
			{
				ValidateIndex(i);
				thelist[i] = value;
			}
		}

		/// <summary>
		/// Adds an address to the list (if it doesn't exist).
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public int Add(object o) 
		{
			int i = -1;
			if (thelist == null)
				thelist = new ArrayList();
			if (IndexOf(o) == -1)
				i = thelist.Add(o);
			return i;
		}

		/// <summary>
		/// Gets the domain of a string.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string GetDomain(string s) 
		{
			string[] parts = null;
			parts = s.Split(new char[] {'@'});

			if (parts == null || parts.Length != 2) 
			{
				return null;
			}
			return parts[1].ToLower();
		}

		/// <summary>
		/// Returns a list of all the distinct domains the recipients belong to.
		/// </summary>
		/// <returns></returns>
		public string[] GetDistinctDestinations() 
		{
			try 
			{
				Hashtable h = new Hashtable();
				foreach (string s in this.AllRecipients) 
				{
					string d = GetDomain(s);
					if (d != null && h[d] == null)
						h.Add(d,d);
				}
				ArrayList a = new ArrayList(h.Keys);
				return (string[])a.ToArray(typeof(string));
			} 
			catch (Exception e) 
			{
				PennLibraries.Utilities.LogEvent("Error: " + e.Message + "\n" + e.StackTrace);
			}
			return null;
		}

		/// <summary>
		/// Returns only the addresses in a given domain.
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public string[] LocalRecipients(string d) 
		{
			if (thelist == null)
				PennLibraries.Utilities.LogEvent("The list is null!?!");

			if (thelist == null)
				return new string[0];

			ArrayList l = new ArrayList();
			foreach (string s in this.AllRecipients)  
			{
				if (s.ToLower().EndsWith("@" + d.ToLower()))
					l.Add(s);
			}
			return (string[])l.ToArray(typeof(string));
		}

		/// <summary>
		/// Adds a list of recipients.
		/// </summary>
		/// <param name="addys"></param>
		public void AddRange(string[] addys) 
		{
			foreach (string s in addys)
				this.Add(s);
		}

		public void Clear() 
		{
			if (thelist != null)
				thelist.Clear();
		}

		public bool Contains(object o) 
		{
			return IndexOf(o) == -1;
		}

		/// <summary>
		/// Returns the index of an object in the list
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public int IndexOf(object o) 
		{
			string s = ValidateObject(o).ToLower();
			string[] rec = this.AllRecipients;
			for (int i = 0; i < rec.Length; i++) 
			{
				if (rec[i].ToLower().Equals(s))
					return i;
			}
			return -1;
		}

		public void Insert(int i, object o) 
		{
			string s = ValidateObject(o);
			if (thelist == null)
				thelist = new ArrayList();

			thelist.Insert(i,s);
		}

		public void Remove(object o) 
		{
			int i = IndexOf(o);
			if (i != -1)
				thelist.RemoveAt(i);
		}

		public void Remove(string s) 
		{
			int i = IndexOf(s);
			if (i != -1)
				thelist.RemoveAt(i);
		}

		public void RemoveAt(int i) 
		{
			ValidateIndex(i);
			thelist.RemoveAt(i);
		}

		public override string ToString() 
		{
			return FormatRecipients(this.AllRecipients);
		}

		public static string[] RemoveDuplicates(string[] reps) 
		{
			try 
			{
				ArrayList a = new ArrayList();
				foreach (string s in reps) 
				{
					if (s.Length != 0) 
					{
						
						bool found = false;
						foreach (object o in a) 
						{
							if (((string)o).ToLower().Equals(s.ToLower())) 
							{
								found = true;
								break;
							}
						}
						if (!found)
							a.Add(s);
					}
				}

				string[] m  = (string[])a.ToArray(typeof(string));
				return m;
			} 
			catch (Exception e) 
			{
				PennLibraries.Utilities.LogEvent("Error: " + e.Message + "\n" + e.StackTrace) ;
				return null;
			}
		}

		public static string FormatRecipients(string[] reps) 
		{
			string s = "";
			string[] myreps = RemoveDuplicates(reps);
			if (myreps != null && myreps.Length > 0)
				s = String.Join(", ",myreps);
			return s;
		}

		public static string[] ParseRecipients(string addys) 
		{
			addys = addys.Replace("\r\n",",");
			addys = addys.Replace("\r",",");
			addys = addys.Replace("\n",",");
			string[] b = addys.Split(',');
			for (int i = 0; i < b.Length; i++)
				b[i] = b[i].Trim();
			return RemoveDuplicates(b);
		}
	}
}
