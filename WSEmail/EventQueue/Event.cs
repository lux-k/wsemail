using System;

namespace EventQueue
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	/// 
	[Serializable]
	public class EventItem
	{
		public string Date;
		public string ShortDescription;
		public string Data;
		public RecordingEntities RecordingEntity;
		public enum RecordingEntities {
			MailA, MailB, ClientA, ClientB, Router, Queue, Unknown,
			MailAClientA, MailAQueue, MailBQueue,
			MailBClientB, MailARouter,
			MailBRouter, QueueRouter}

		public EventItem()
		{
		}

		public EventItem(string da, EventItem.RecordingEntities RecordingEntity, string sd, string data) 
		{
			this.Date = da;
			this.RecordingEntity = RecordingEntity;
			this.ShortDescription = sd;
			this.Data = data;
		}

	}

	[Serializable]
	public class TransitItem : EventItem 
	{
		public Actions Action;
		public enum Actions {
			To, From, Erase }

		public TransitItem(Actions a) 
		{
			this.Action = a;
		}

		public TransitItem(string da, EventItem.RecordingEntities re, Actions a) 
		{
			this.Date = da;
			this.RecordingEntity = re;
			this.Action = a;
		}
		public TransitItem(string da, EventItem.RecordingEntities re, Actions a, string desc) 
		{
			this.Date = da;
			this.RecordingEntity = re;
			this.Action = a;
			this.ShortDescription = desc;
		}

	}
}
