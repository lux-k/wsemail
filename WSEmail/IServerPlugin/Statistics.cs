/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;

namespace MailServerInterfaces
{
	public class StatisticItem 
	{
		public int MessageCount = 0;
		public double ByteCount = 0;
		public StatisticItem() 
		{
		}
		public void Clear() 
		{
			this.MessageCount = 0;
			this.ByteCount = 0;
		}
	}

	public class SentRecvItem 
	{
		public StatisticItem Internal = new StatisticItem();
		public StatisticItem External = new StatisticItem();

		public SentRecvItem() 
		{
		}

		public void Clear() 
		{
			Internal.Clear();
			External.Clear();
		}
	}

	public class Statistics 
	{
		public SentRecvItem Received = new SentRecvItem();
		public SentRecvItem Sent = new SentRecvItem();
		public void Clear() 
		{
			Received.Clear();
			Sent.Clear();
		}
	}
}	
