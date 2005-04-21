using System;
using System.Collections;

namespace TicTacToe
{
	/// <summary>
	/// Maintains a list of players
	/// </summary>
	public class PlayerList 
	{
		private Hashtable players;

		/// <summary>
		/// Default constructor
		/// </summary>
		public PlayerList() 
		{
			players = new Hashtable();
		}

		/// <summary>
		/// Adds a player to the list. If the player is a new addition (that is, not an update), it returns true.
		/// </summary>
		/// <param name="p">Player name</param>
		/// <param name="i">Player IP address</param>
		/// <returns></returns>
		public bool AddPlayer(string p, string i) 
		{
			bool ret = true;
			if (players.ContainsKey(p))
				ret = false;

			if (!ret) 
				players[p] = i;
			else
				players.Add(p,i);

			return ret;
		}

		/// <summary>
		/// Gets a player record for a particular player.
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public Player GetPlayer(string p) 
		{
			return new Player(p,(string)players[p]);
		}

		/// <summary>
		/// Removes a user from the playerlist. Returns true if it actually removed the user.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public bool RemovePlayer(string s) 
		{
			if (players.ContainsKey(s)) 
			{
				players.Remove(s);
				return true;
			}
			return false;
		}

	}
}
