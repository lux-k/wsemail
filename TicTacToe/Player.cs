using System;

namespace TicTacToe
{
	/// <summary>
	/// Contains information about other players. Primarily used in the player list.
	/// </summary>
	public class Player 
	{
		private string _p = "", _i = "";

		/// <summary>
		/// The player's name
		/// </summary>
		public String Name 
		{
			get 
			{
				return _p;
			}
			set 
			{
				_p = value;
			}
		}

		/// <summary>
		/// The player's IP address
		/// </summary>
		public String IP 
		{
			get 
			{
				return _i;
			}
			set 
			{
				_i = value;
			}

		}

		/// <summary>
		/// Creates a new player with a given name and IP
		/// </summary>
		/// <param name="p"></param>
		/// <param name="i"></param>
		public Player(string p, string i) 
		{
			this.Name = p;
			this.IP = i;
		}
	}
}
