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
	/// Summary description for Message.
	/// </summary>
	/// 
	[Serializable]
	public class MailMessage
	{
		public string _email;
		public int _weight;
		public int _number;
		public string _subject;
		public string _body;

		public MailMessage(string email,int number,string subject,string body) 
		{
			this.Email = email;
			this.Body = body;
			this.Number = number;
			this.Subject = subject;
		}

		public string Email 
		{
			get 
			{
				return _email;
			}
			set 
			{
				_email = value;
			}
		}

		public string Subject 
		{
			get 
			{
				return _subject;
			}
			set 
			{
				_subject = value;
			}
		}

		public string Body 
		{
			get 
			{
				return _body;
			}
			set 
			{
				_body = value;
			}
		}

		public int Weight 
		{
			get 
			{
				return _weight;
			}
			set 
			{
				_weight = value;
			}
		}

		public int Number 
		{
			get 
			{
				return _number;
			}
			set 
			{
				_number = value;
			}
		}

	}
}
