/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;

namespace TestAppLibs
{
	/// <summary>
	/// Summary description for TestResult.
	/// </summary>
	/// 
	[Serializable]
	public class TestResult
	{
		private double _l=0;
		private ACTIONS _a;
		private string _t;
		private bool _s = false;

		public TestResult()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public double Latency 
		{
			get 
			{
				return _l;
			}
			set 
			{
				_l = value;
			}
		}

		public ACTIONS Action 
		{
			get 
			{
				return _a;
			}
			set 
			{
				_a = value;
			}
		}

		public string Token 
		{
			get 
			{
				return _t;
			}
			set 
			{
				_t=value;
			}
		}

		public bool Successful 
		{
			get 
			{
				return _s;
			}
			set 
			{
				_s = value;
			}
		}
		
	}
}
