using System;

namespace BusinessObjects
{
	/// <summary>
	/// Summary description for BusinessObjectsFormInterface.
	/// </summary>
	///

	public class BusinessObjectsDelegates 
	{
		public delegate void NullDelegate();
	}

	public interface BusinessObjectsFormInterface
	{
		BusinessRequest GetBusinessRequest();
		event BusinessObjectsDelegates.NullDelegate UserDone;
		void LoadBusinessRequestAndShow(BusinessRequest b);
		void ThrowOut();
	}
}
