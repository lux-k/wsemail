/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Collections;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.ComponentModel;

namespace DynamicBizObjects
{
	[Serializable]
	[XmlInclude(typeof(POItem))]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securitylab.cis.upenn.edu/AuthorizedRouting")]
	public class PurchaseOrder : BusinessObject
	{
		
		POItems _Items;
		private float _total=0;
		public event BusinessObjectsDelegates.NullDelegate TotalChanged;

		private string _acct,_vend;

		public string Account 
		{
			get 
			{
				return _acct;
			}
			set 
			{
				_acct = value;
			}
		}

		public string Vendor 
		{
			get 
			{
				return _vend;
			}
			set 
			{
				_vend = value;
			}
		}

		public float Total 
		{
			get 
			{
				return _total;
			}
		}

		public PurchaseOrder() {
			_Items = new POItems();
			_Items.TotalChanged += new BusinessObjectsDelegates.NullDelegate(UpdateTotal);
		}

		public POItem GetItem(int i) 
		{
			return (POItem)Items[i];
		}

		public void AddItem(POItem i) 
		{
			Items.Add(i);
		}

		private void UpdateTotal() 
		{
			_total = 0;
			for (int i = 0; i < Items.Count; i++)
				_total += Items[i].Total;
			if (TotalChanged != null)
				TotalChanged.DynamicInvoke(null);
		}
		public POItems Items 
		{
			get 
			{
				return _Items;
			}
			set 
			{
				_Items = value;
			}
		}

		public DataGridTableStyle GetDataGridTableStyle() {
			DataGridTableStyle gs = new DataGridTableStyle();
			gs.MappingName = this.Items.GetType().Name;

			DataGridTextBoxColumn cs = new DataGridTextBoxColumn();
			cs.MappingName = "Quantity";
			cs.HeaderText = "Quantity";
			cs.Width = 80;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "Item";
			cs.HeaderText = "Item Description";	
			cs.Alignment = HorizontalAlignment.Right;
			cs.Width = 220;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "UnitPrice";
			cs.HeaderText = "Unit Price";
			cs.Width = 85;
			cs.Format="c";
			cs.Alignment = HorizontalAlignment.Right;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "Total";
			cs.HeaderText = "Amount";
			cs.Alignment = HorizontalAlignment.Right;
			cs.ReadOnly = true;
			cs.Width = 110;
			cs.Format="c";
			gs.GridColumnStyles.Add(cs);

			return gs;
		}
	}

	[Serializable]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securitylab.cis.upenn.edu/AuthorizedRouting")]
	public class POItem 
	{
		private string _item;
		private float _unit;
		private int _q;
		public event BusinessObjectsDelegates.NullDelegate PriceChanged;

		public POItem() {}

		public POItem (string ItemName, int Quantity, float UnitPrice) 
		{
			this.Item = ItemName;
			this.Quantity = Quantity;
			this.UnitPrice = UnitPrice;
		}

		#region Properties
		private void FirePriceChanged() {
			if (PriceChanged != null) 
				this.PriceChanged.DynamicInvoke(null);
		}

		[System.Xml.Serialization.XmlAttribute()]
		public int Quantity 
		{
			get 
			{
				return _q;
			}
			set 
			{
				_q = value;
				FirePriceChanged();
			}
		}

		[System.Xml.Serialization.XmlAttribute()]
		public float UnitPrice 
		{
			get 
			{
				return _unit;
			}
			set 
			{
				_unit = value;
				FirePriceChanged();
			}
		}
		public float Total 
		{
			get 
			{
				return UnitPrice * Quantity;
			}
		}
		[System.Xml.Serialization.XmlAttribute()]
		public string Item 
		{
			get 
			{
				return _item;
			}
			set 
			{
				_item = value;
			}
		}
		#endregion
	}

	[Serializable]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securitylab.cis.upenn.edu/AuthorizedRouting")]
	public class POItems : ArrayList 
	{
		public delegate void POChangeEvent(POItem p);
		public event POChangeEvent POAdded;
		public event POChangeEvent PODeleted;
		public event BusinessObjectsDelegates.NullDelegate TotalChanged;
		public const int ADDED = 1, DELETED = 2;

		public new POItem this[int i] 
		{
			get 
			{
				return (POItem)base[i];
			}
			
			set 
			{
				base[i] = value;
			}
		}

		private void FirePriceChanged() 
		{
			if (TotalChanged != null) 
				TotalChanged.DynamicInvoke(null);
		}

		public override void RemoveAt(int i) 
		{
			POItem p = this[i];
			base.RemoveAt(i);
			if (PODeleted != null)
				POAdded.DynamicInvoke(new object[] {p,DELETED});
			FirePriceChanged();

		}

		public override int Add(object o) 
		{
			POItem p = (POItem)o;
			int i = base.Add(p);
			p.PriceChanged += new BusinessObjectsDelegates.NullDelegate(FirePriceChanged);
			if (POAdded != null)
				POAdded.DynamicInvoke(new object[] {p,ADDED});
			FirePriceChanged();
			return i;
		}
	}
	#region Obsoleted code
/*
 * 	

	public class POOItems : ArrayList, IBindingList 
	{
		public bool AllowEdit 
		{
			get 
			{
				return true;
			}
		}
		public bool AllowNew  
		{
			get 
			{
				return true;
			}
		}

		public bool AllowRemove 
		{
			get 
			{
				return true;
			}
		}
		public bool IsSorted 
		{
			get 
			{
				return false;
			}
		}

		public ListSortDirection SortDirection 
		{
			get 
			{
				throw new NotSupportedException();
			}
		}
		public PropertyDescriptor SortProperty 
		{
			get 
			{
				throw new NotSupportedException();
			}
		}

		public bool SupportsChangeNotification  
		{
			get 
			{
				return true;
			}
		}
		public bool SupportsSearching 
		{
			get 
			{
				return false;
			}
		}

		public bool SupportsSorting 
		{
			get 
			{
				return false;
			}
		}

		public event ListChangedEventHandler ListChanged;
		/*
		public override int Add(object o) 
		{
			int i = base.Add(o);

			if (ListChanged != null) 
				this.ListChanged.DynamicInvoke(new object[] { (new ListChangedEventArgs(System.ComponentModel.ListChangedType.ItemAdded,i,-1)) });
			return i;
		}

		public object AddNew() 
		{
			POItem p = new POItem();
			int i = this.Add(p);
			MessageBox.Show("New Index = " + i.ToString());
			this.ListChanged.DynamicInvoke(new object[] { (new ListChangedEventArgs(System.ComponentModel.ListChangedType.ItemAdded,i,-1)) });
			return p;
		}
		public void AddIndex(PropertyDescriptor property) {}
		public void ApplySort(PropertyDescriptor property,	ListSortDirection direction) 
		{
			throw new NotSupportedException();
		}

		public void RemoveSort() 
		{
			throw new NotSupportedException();
		}
		
		public void RemoveIndex(PropertyDescriptor property) 
		{
			throw new NotSupportedException();
		}
		
		public int Find(PropertyDescriptor property,object key) {
			throw new NotSupportedException();
		}


	}
*/
#endregion
}

