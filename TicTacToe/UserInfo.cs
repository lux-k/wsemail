using System;

namespace TicTacToe
{
	/// <summary>
	/// Defines information about a user such as their name and "piece" in the game.
	/// </summary>
	public class UserInfo 
	{
		private string _n = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).HostName;
		private Pieces _m = Pieces.X;	

		/// <summary>
		/// The current piece this player is using
		/// </summary>
		public Pieces MyPiece 
		{
			get 
			{
				return _m;
			}
			set 
			{
				_m = value;
			}
		}

		/// <summary>
		/// The current name of the player.
		/// </summary>
		public string Name 
		{
			get 
			{
				return _n;
			}
			set 
			{
				_n = value;
			}
		}
	}
}
