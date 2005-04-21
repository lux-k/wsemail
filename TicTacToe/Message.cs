using System;

namespace TicTacToe
{
	/// <summary>
	/// Defines a message. Primarily used for the UDP multicast (that is, chat and user list maintainance)
	/// </summary>
	public class Message 
	{
		private string _u;
		private Actions _a;
		private string _t = "";
		
		/// <summary>
		/// The action this message specifies
		/// </summary>
		public Actions Action 
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

		/// <summary>
		/// The user generating the message
		/// </summary>
		public string User 
		{
			get 
			{
				return _u;
			}
			set 
			{
				_u = value;
			}
		}

		/// <summary>
		/// The text of the message
		/// </summary>
		public string Text 
		{
			get 
			{
				return _t;
			}
			set 
			{
				_t = value;
			}
		}

		/// <summary>
		/// Formats a message into a string for transmitting on the wire.
		/// </summary>
		/// <returns>"Packed" message</returns>
		public string ToWire() 
		{
			return User + "\t" + Action.ToString() + "\t" + Text;
		}

		/// <summary>
		/// Puts a message into pretty, human readable format.
		/// </summary>
		/// <returns></returns>
		public override string ToString() 
		{
			if (Action == Actions.Win)
				return User + " has won against " + Text;
			else if (Action == Actions.Draw)
				return User + " and " + Text + " have had a draw.";
			else if (Action != Actions.Chat)
				return User + " has " + Action.ToString().ToLower() + "ed.";
			else
				return "<" + User + "> " + Text;
		}

		/// <summary>
		/// New message with only an action
		/// </summary>
		/// <param name="a"></param>
		public Message(Actions a) 
		{
			this.Action = a;
		}

		/// <summary>
		/// New message with no information
		/// </summary>
		public Message() {}

		/// <summary>
		/// New message with an action and text
		/// </summary>
		/// <param name="a"></param>
		/// <param name="t"></param>
		public Message(Actions a, string t) 
		{
			this.Action = a;
			this.Text = t;
		}

		/// <summary>
		/// Parses a "wire format" string into a message object
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static Message Parse(string s) 
		{
			string[] o = s.Split(new char[] {'\t'},3);
			Message m = new Message();
			m.User = o[0];
			m.Action = (Actions)Enum.Parse(typeof(Actions),o[1]);
			if (o.Length == 3)
				m.Text = o[2];
			return m;
		}
	}
}
