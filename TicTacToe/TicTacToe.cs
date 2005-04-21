using System;
using System.Windows.Forms;

namespace TicTacToe
{
	/// <summary>
	/// Contains the game rules for TicTacToe!
	/// </summary>
	public class TicTacToe : MarshalByRefObject 
	{
		bool playing = false;
		private Pieces _cm;
		private string _x = "", _o = "";

		private Pieces[] board;
		/// <summary>
		/// The peer acting as the server will call this event to notify its game controller of a new game.
		/// </summary>
		public event VoidHandler NewGame;
		/// <summary>
		/// Fires when someone makes a move. It passes the square that was changed as a parameter.
		/// </summary>
		public event IntHandler NextMove;
		/// <summary>
		/// Fires when the game has been won. Passes the winning row.
		/// </summary>
		public event IntArrayHandler GameWon;
		/// <summary>
		/// Fires when the client of the server is finished (so the server can disconnect them).
		/// </summary>
		public event VoidHandler ClientDone;

		/// <summary>
		/// Contains the board in an array. (0..2 top row, 6..8 bottom row)
		/// </summary>
		public Pieces[] Board 
		{
			get 
			{
				return board;
			}
			set 
			{
				board = value;
			}
		}

		/// <summary>
		/// Name of the xplayer
		/// </summary>
		public string XPlayer 
		{
			get 
			{
				return _x;
			}
			set 
			{
				_x = value;
			}
		}

		/// <summary>
		/// Name of the oplayer
		/// </summary>
		public string OPlayer 
		{
			get 
			{
				return _o;
			}
			set 
			{
				_o = value;
			}
		}

		/// <summary>
		/// Returns which piece has the current move
		/// </summary>
		public Pieces CurrentMove 
		{
			get 
			{
				return _cm;
			}
			set 
			{
				_cm = value;
			}
		}
		
		/// <summary>
		/// Sends a signal to the server that the client is done.
		/// </summary>
		public void Done() 
		{
			if (this.ClientDone != null)
				ClientDone();
		}

		/// <summary>
		/// Whether the game is being played
		/// </summary>
		public bool Busy 
		{
			get 
			{
				return playing;
			}

			set 
			{
				playing = value;
			}
		}

		/// <summary>
		/// Clears the board, picks a player to start.
		/// </summary>
		public void SetupNewGame() 
		{
			board = new Pieces[9];
			for (int i = 0; i < 9; i++)
				board[i] = Pieces.Neither;
			
			Random r = new Random();
			
			if (r.Next(3) == 2)
				this.CurrentMove = Pieces.X;
			else
				this.CurrentMove = Pieces.O;
		}

		/// <summary>
		/// Allows the peer being asked to play to accept/reject the request to play. If it accepts, it notifies the game controller via the new game event.
		/// </summary>
		/// <param name="u"></param>
		/// <returns></returns>
		public bool AcceptGame(string u) 
		{
			DialogResult r = MessageBox.Show("Do you wish to play against " + u + "?","Question...",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
			if (r == DialogResult.Yes) 
			{
				Busy = true;
				this.SetupNewGame();
				if (NewGame != null)
					NewGame();
				return true;
			}
			else
				return false;
		}

		/// <summary>
		/// Checks if the game has been one. Returns null if not won, or returns an int array of the winning row.
		/// </summary>
		/// <returns></returns>
		private int[] CheckWin() 
		{
			
			object[] wins  = new object[] {new int[] {0,1,2}, new int[] {3,4,5}, new int[] {6,7,8}, new int[] {0,3,6},
											  new int[] {1,4,7}, new int[] {2,5,8}, new int[] {0,4,8}, new int[] {2,4,6}};
			for (int i = 0; i < wins.Length; i++) 
			{
				int[] row = (int[])wins[i];
				if (board[row[0]] != Pieces.Neither && (board[row[0]] == board[row[1]] && board[row[1]] == board[row[2]]))
					return row;
			}

			return null;
		}

		private bool CheckDraw() 
		{
			bool draw = true;
			for (int i = 0; i < 9; i++)
				draw &= (board[i] != Pieces.Neither);
					
			return draw;
		}

		/// <summary>
		/// Records a players move, switches the current player and fires the next move event
		/// </summary>
		/// <param name="i"></param>
		/// <param name="p"></param>
		public void Move(int i, Pieces p) 
		{
			board[i] = p;
			int[] res = CheckWin();
			if (res != null || CheckDraw()) 
			{
				if (NextMove != null)
					NextMove(i);
				this.Busy = false;
				if (GameWon != null)
					GameWon(res);
				return;
			}


				
			if (p == Pieces.O)
				CurrentMove = Pieces.X;
			else
				CurrentMove = Pieces.O;
			if (NextMove != null)
				NextMove(i);
		}

		/// <summary>
		/// Makes sure that this object (when remoted) does not disappear to a missing lease
		/// </summary>
		/// <returns></returns>
		public override object InitializeLifetimeService() 
		{
			return null;
		}
	}
}
