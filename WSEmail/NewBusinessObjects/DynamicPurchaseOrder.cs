/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using DynamicForms;

namespace DynamicBizObjects
{
	/// <summary>
	/// Summary description for DynamicTimesheet.
	/// </summary>
	public class DynamicPurchaseOrder : BaseObject, BizObjectsInterface
	{
		public BusinessRequest bizReq = null;
		private FrmPurchaseOrder ft = null;

		public DynamicPurchaseOrder()
		{
			InitializeDynamicConfiguration();
		}

		public BusinessRequest GetBusinessRequest() 
		{
			return bizReq;
		}

		public override string DebugToScreen() 
		{
			return "I am a PO.";
		}

		public override void Dispose() 
		{
			if (!this.IsDisposed) 
			{
				this.IsDisposed = true;
				ft.ThrowOut();
				ft = null;
			}
		}

		public override void InitializeDynamicConfiguration() 
		{
			this.LockEmail = true;
			this.Configuration.DLL = "DynamicBizObjects";
			this.Configuration.Name = "DynamicBizObjects.DynamicPurchaseOrder";
			this.Configuration.Url = "http://tower.cis.upenn.edu/classes/DynamicBizObjects.dll";
			this.Configuration.Version = 1.1F;
			this.Configuration.FriendlyName = "Penn Purchase Order";
			this.Configuration.Description = "A purchase order so you can buy things.";
		}

		public override void Run() 
		{
			ft = new FrmPurchaseOrder();
			ft.UserDone += new BusinessObjectsDelegates.NullDelegate(FormDone);
			ft.BusinessRequest = bizReq;
			ft.LoadBusinessRequestAndShow(null);
		}

		private void FormDone() 
		{
			bizReq = ft.BusinessRequest;
			base.HopChanged(bizReq.GetNextHop);
			base.Done(this);
		}
	}
}
