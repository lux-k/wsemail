/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;

namespace WSEmailProxy
{
	/// <summary>
	/// A container to hold delivery information about a particular piece of mail.
	/// </summary>
	public class WSEmailStatus 
	{
		public int ResponseCode;
		public string Message;
		public WSEmailStatus() {}
		public WSEmailStatus(int code) 
		{
			ResponseCode = code;
		}
		public WSEmailStatus(int code, string message) 
		{
			ResponseCode = code;
			Message = message;
		}
		public override string ToString() 
		{
			return "Status: " + this.ResponseCode.ToString() + " (" + this.Message + ")" ;
		}
	}
}
