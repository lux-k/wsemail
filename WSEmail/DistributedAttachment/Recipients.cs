/*
using System;

namespace DistributedAttachment
{
	/// <summary>
	/// Summary description for Recipients.
	/// </summary>
	public class Recipients
	{
		public Recipient[] recs;
		public int numRec;
		
		public Recipients()
		{
			//
			// TODO: Add constructor logic here
			//
			recs = null;
			numRec = 0;
		}
		/// <summary>
		/// Add a new recipient.
		/// </summary>
		/// <param name="rec">The added recipient</param>
		public void addRecipient(Recipient rec)
		{
			numRec++;
			Recipient[] recTemp = recs;
			recs = new Recipient[numRec];
			if(recTemp != null)
				Array.Copy(recTemp,0,recs,0,recTemp.Length);
			recs[numRec-1]=rec;
		}
		/// <summary>
		/// Get the index of the specified Recipient in ACL
		/// </summary>
		/// <param name="vrec">Specified Recipient</param>
		/// <returns>The index of the searched index</returns>
		public int getIndexRec(Recipient vrec)
		{
			int rindex = 0;
			for(int i=0;i<recs.Length;i++)
				if(recs[i].EmailAddress.ToLower().Equals(vrec.EmailAddress.ToLower()))
				{
					rindex = i;
					break;
				}
			return rindex;
		}
		/// <summary>
		/// Get the Recipient ojbect by its emailaddress
		/// </summary>
		/// <param name="emailaddress">Searched Recipient's emailaddress</param>
		/// <returns>The searched recipient</returns>
		public Recipient getRec(string emailaddress)
		{
			Recipient recTemp = null;
			for(int i=0;i<recs.Length ;i++)
				if(recs[i].EmailAddress.ToLower().Equals(emailaddress.ToLower()))
				{
					recTemp = recs[i];
					break;
				}
			return recTemp;

		}
		/// <summary>
		/// Remove a Recipient by index
		/// </summary>
		/// <param name="indexR">The removec Recipient's index</param>
		public void removeRecipient(int indexR)
		{
			Recipient[] recsTemp = recs;
			numRec--;
			recs = new Recipient[numRec];
			for(int i=indexR;i<recsTemp.Length - 1;i++)
				recsTemp[i] = recsTemp[i+1];
			for(int i=0;i<numRec;i++)
				recs[i] = recsTemp[i];
		}
	}
}
*/