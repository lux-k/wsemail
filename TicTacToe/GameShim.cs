using System;
using System.Runtime.Remoting.Messaging;

namespace TicTacToe
{
	/// <summary>
	/// Shims requests that would talk to the GUI to the game controller (for security and GUI management ease).
	/// </summary>
	public class GameShim : MarshalByRefObject
	{
		public event IntHandler NextMove;
		public event IntArrayHandler GameWon;

		public GameShim()
		{
			
		}

		[OneWay]
		public void LocalNextMove(int i) 
		{
			if (NextMove != null) 
				NextMove(i);
		}

		[OneWay]
		public void LocalGameWon(int[] i) 
		{
			if (GameWon != null)
				GameWon(i);
		}
		
	}
}
