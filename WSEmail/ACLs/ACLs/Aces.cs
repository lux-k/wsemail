using System;
using System.Collections;
using System.Diagnostics;

namespace ACLs
{
	/// <summary>
	/// Summary description for Aces.
	/// </summary>
	internal class Aces : CollectionBase
	{
		public Aces()
		{
		}
		public Ace this[int index]
		{
			get
			{
				return (Ace)base.InnerList[index];
			}
		}
		public void SetAce(int i, Ace ace)
		{
			base.InnerList[i] = ace;
		}
		public void Add(Ace ace)
		{
			base.InnerList.Add(ace);
		}
	}
}
