using System;
using System.Collections;

namespace TestApp
{
	public abstract class ProbabilisticObject 
	{
		private Random _ran = null;

		public ProbabilisticObject() 
		{
			_ran = new Random();
		}

		private Object[] _queue = null;
		private int _total 
		{
			get 
			{
				if (_queue != null)
					return _queue.Length;
				else
					return 0;
			}
		}

		public void AddObject(Object o, int Weight) 
		{
			ArrayList a = new ArrayList();
			if (_queue != null) 
			{
				a.AddRange(_queue);
			}
			for (int i = 0; i < Weight; i++)
				a.Add(o);
			_queue = (object[])a.ToArray(typeof(object));
		}

		protected Object GetObject() 
		{
			int i = _ran.Next(0,_total);
			//Console.WriteLine("Get " + i.ToString() + " out of " + _queue.Length.ToString());
			return _queue[i];
		}
	}
}
