/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;

namespace DynamicBizObjects
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
