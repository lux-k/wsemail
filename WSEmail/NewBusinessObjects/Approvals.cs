/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Collections;

namespace DynamicBizObjects
{
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securitylab.cis.upenn.edu/AuthorizedRouting")]
	public class Approvals 
	{
		SignatureList _req, _rec;
		
		public Approvals() 
		{
			_req = new SignatureList();
			_rec = new SignatureList();
		}
		
		public SignatureList Required 
		{
			get 
			{
				return _req;
			}

			set 
			{
				_req = value;
			}
		}

		public SignatureList Received 
		{
			get 
			{
				return _rec;
			}
			set 
			{
				_rec = value;
			}
		}

		public Signature[] GetSignatureByName(string name) 
		{
			ArrayList a = new ArrayList();
			for (int j = 0; j < Received.Count; j++) 
			{
				Signature s = (Signature)((Signature)Received[j]).Clone();
				if (s.User != null && s.User.Equals(name))
					a.Add(s);
			}
			for (int j = 0; j < Required.Count; j++) 
			{
				Signature s = (Signature)((Signature)Required[j]).Clone();
				if (s.User != null && s.User.Equals(name))
					a.Add(s);
			}

			return (Signature[])a.ToArray(typeof(Signature));
		}

		public Signature[] GetDelegates(string addingID)
		{
			ArrayList a = new ArrayList();
			for (int j = 0; j < Received.Count; j++) 
			{
				Signature s = (Signature)((Signature)Received[j]).Clone();
				if (s.AddedBy != null && s.AddedBy.Equals(addingID))
					a.Add(s);
			}
			for (int j = 0; j < Required.Count; j++) 
			{
				Signature s = (Signature)((Signature)Required[j]).Clone();
				if (s.AddedBy != null && s.AddedBy.Equals(addingID))
					a.Add(s);
			}

			return (Signature[])a.ToArray(typeof(Signature));
		}

		public Signature[] GetDelegates(string[] delegates, string addingID) 
		{
			if (delegates == null || delegates.Length == 0)
				return new Signature[0];
			Signature[] sigs = new Signature[delegates.Length];
			for (int i = 0; i < delegates.Length; i++) 
			{
				for (int j = 0; j < Received.Count; j++) 
				{
					Signature s = (Signature)((Signature)Received[j]).Clone();
					if (s.AddedBy != null && s.AddedBy.Equals(addingID) && s.User.Equals(delegates[i]))
						sigs[i] = s;
				}

				for (int j = 0; j < Required.Count; j++) 
				{
					Signature s = (Signature)((Signature)Required[j]).Clone();
					if (s.AddedBy != null && s.AddedBy.Equals(addingID) && s.User.Equals(delegates[i]))
						sigs[i] = s;
				}

			}
			return sigs;
		}

	}
}
