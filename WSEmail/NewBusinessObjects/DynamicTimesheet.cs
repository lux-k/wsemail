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
	/// Holds the data for a dynamic time sheet. This combined with the actual form
	/// produces the entire effect.
	/// </summary>
	public class DynamicTimesheet : BaseObject, BizObjectsInterface
	{
		/// <summary>
		/// A handle to the business request. This holds the signatures and such.
		/// </summary>
		public BusinessRequest bizReq = null;

		/// <summary>
		/// A reference to the actual form.
		/// </summary>
		private FrmTimesheet ft = null;


		/// <summary>
		/// Default constructor.
		/// </summary>
		public DynamicTimesheet()
		{
			InitializeDynamicConfiguration();
		}

		/// <summary>
		/// Returns the business request. That's the stuff like the signatures, etc.
		/// </summary>
		/// <returns></returns>
		public BusinessRequest GetBusinessRequest() 
		{
			return bizReq;
		}

		/// <summary>
		/// Returns a rather stupid and vague message about the object.
		/// </summary>
		/// <returns></returns>
		public override string DebugToScreen() 
		{
			return "I am a timesheet.";
		}

		/// <summary>
		/// Disposes this object, custom like.
		/// </summary>
		public override void Dispose() 
		{
			if (!this.IsDisposed) 
			{
				this.IsDisposed = true;
				ft.ThrowOut();
				ft = null;
			}
		}

		/// <summary>
		/// Specifies the dynamic configuration.. like the name and url to download it.
		/// </summary>
		public override void InitializeDynamicConfiguration() 
		{
			this.LockEmail = true;
			this.Configuration.DLL = "DynamicBizObjects";
			this.Configuration.Name = "DynamicBizObjects.DynamicTimesheet";
			this.Configuration.Url = "http://tower.cis.upenn.edu/classes/DynamicBizObjects.dll";
			this.Configuration.Version = 1.1F;
			this.Configuration.FriendlyName = "Penn Timesheet";
			this.Configuration.Description = "A timesheet so you can get paid.";
		}

		/// <summary>
		/// Runs the form. Shows it, etc.
		/// </summary>
		public override void Run() 
		{
			ft = new FrmTimesheet();
			ft.UserDone += new BusinessObjectsDelegates.NullDelegate(FormDone);
			ft.BusinessRequest = bizReq;
			ft.LoadBusinessRequestAndShow(null);
		}

		/// <summary>
		/// All done! Raise the done event.
		/// </summary>
		private void FormDone() 
		{
			bizReq = ft.BusinessRequest;
			base.HopChanged(bizReq.GetNextHop);
			base.Done(this);
		}
	}
}
