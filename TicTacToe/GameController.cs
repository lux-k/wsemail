using System;
using System.Windows.Forms;
using System.Collections;
using System.Threading;

namespace TicTacToe
{
	/// <summary>
	/// Controls the users, the network communications, the events between the game and the GUI. That is to say,
	/// this thing controls everything.
	/// </summary>
	public class GameController : IDisposable
	{
		private UserInfo _ui;
		private UDPMulticastListener _uml;
		private UDPMulticastSender _ums;
		public event MessageHandler NewMessage;
		public event VoidHandler GameReset;
		private Thread ListenerThread;
		private PlayerList pl;
		private NetworkDriver nd = null;
		public event MessageHandler PlayerListChanged;
		private TicTacToe game = null;
		public event VoidHandler NewGame;
		public event IntHandler GameMove;
		public event IntArrayHandler GameWon;
		public bool _p = false;
		private bool disposing = false;
		private GameShim gs;
		private Mode cm = Mode.Client;
		private Thread RebroadcastThread;
		private string _ip;

		/// <summary>
		/// Returns the IP of the this controller is running on.
		/// </summary>
		public string IP 
		{
			get 
			{
				return _ip;
			}
			set 
			{
				_ip = value;
			}
		}

		/// <summary>
		/// Returns the mode the game is operating in for this user (server or client)
		/// </summary>
		public Mode CurrentMode 
		{
			get 
			{
				return cm;
			}
			set 
			{
				cm = value;
			}
		}

		/// <summary>
		/// Returns whether or not this user is playing.
		/// </summary>
		public bool Playing 
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
		/// The actual game
		/// </summary>
		public TicTacToe Game 
		{
			get 
			{
				return game;
			}
			set 
			{
				game = value;
			}

		}
		
		/// <summary>
		/// Information about the user
		/// </summary>
		public UserInfo User 
		{
			get 
			{
				return _ui;
			}
			set 
			{
				_ui = value;
			}
		}

		/// <summary>
		/// Begins listeners, broadcasters, etc.
		/// </summary>
		public GameController()
		{
			_uml = new UDPMulticastListener();
			_ums = new UDPMulticastSender();
			_ui = new UserInfo();
			pl=new PlayerList();
			nd = new NetworkDriver();
			CreateGame();
			_uml.MessageReceived += new StringHandler(_uml_MessageReceived);

			this.IP = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString();
			ListenerThread = new Thread(new ThreadStart(_uml.StartListener));
			ListenerThread.Start();
			RebroadcastThread = new Thread(new ThreadStart(RebroadcastThreadCode));
			RebroadcastThread.Start();
		}

		/// <summary>
		/// The entry point for rebroadcasting a node's presence
		/// </summary>
		private void RebroadcastThreadCode() 
		{
			while (true) 
			{
				Thread.Sleep(15 * 1000);
				SendBroadcast(new Message(Actions.Join,this.IP));
			}
		}

		/// <summary>
		/// Creates a new game and publishes it to the world.
		/// </summary>
		private void CreateGame() 
		{
			game = new TicTacToe();
			nd.Game = game;
			nd.Remote();
			game.NewGame +=new VoidHandler(game_NewGame);
			game.NextMove += new IntHandler(game_NextMove);
			this.Playing = false;
		}

		/// <summary>
		/// Sends the message that this user is joining the network.
		/// </summary>
		public void Start() 
		{
			this.SendBroadcast(new Message(Actions.Join,this.IP));
		}

		/// <summary>
		/// Binds the controller's events to the current game.
		/// </summary>
		public void BindEvents() 
		{
			gs = new GameShim();
			gs.NextMove += new IntHandler(game_NextMove);
			game.NextMove += new IntHandler(gs.LocalNextMove);
			gs.GameWon += new IntArrayHandler(game_GameWon);
			game.GameWon += new IntArrayHandler(gs.LocalGameWon);
		}

		/// <summary>
		/// Plays a particular username.
		/// </summary>
		/// <param name="user"></param>
		public void PlayUser(string user) 
		{
			Play(pl.GetPlayer(user).IP);
		}

		/// <summary>
		/// Plays a particular IP address
		/// </summary>
		/// <param name="ip"></param>
		public void Play(string ip) 
		{
			this.game.Busy = true;
			TicTacToe t = null;
			bool err = false;
			try 
			{
				t = (TicTacToe)Activator.GetObject(typeof(TicTacToe),"http://" + ip +":"+NetworkDriver.PORT+"/"+ NetworkDriver.PUBLISHEDNAME);
			} 
			catch {
				err = true;
			}

			try 
			{
				bool i = t.Busy;
			}
			catch 
			{
				err = true;
			}

			if (err) 
			{
				MessageBox.Show("Unable to connect to that player.","Oops!",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				this.game.Busy = false;
				return;
			}
			
			if (t.Busy) 
			{
				MessageBox.Show("That user is already playing :(");
				this.game.Busy = false;
			}
			else 
			{
				if (t.AcceptGame(this.User.Name)) 
				{
					this.Playing = true;
					MessageBox.Show("Your game was accepted!","Let the game begin!",MessageBoxButtons.OK,MessageBoxIcon.Information);
					nd.UnRemote();
					this.game = t;
					this.User.MyPiece = Pieces.O;
					this.game.OPlayer = User.Name;
					this.CurrentMode = Mode.Client;
					if (NewGame != null)
						NewGame();
				}
				else 
				{
					MessageBox.Show("Your game was not accepted.");
					this.game.Busy = false;
				}
			}
		}

		private void _uml_MessageReceived(string s)
		{
			Message m = Message.Parse(s);
			if (m.Action == Actions.Join || m.Action == Actions.Part) 
			{
				bool res = false;
				if (m.Action == Actions.Join)
					res = pl.AddPlayer(m.User,m.Text);
				else
					res = pl.RemovePlayer(m.User);
				if (res) 
				{
					if (PlayerListChanged != null)
						PlayerListChanged(m);
				} else
					return;
			} 

			if (NewMessage != null)
				NewMessage(m);
		}

		/// <summary>
		/// Sends a message to the multicast group
		/// </summary>
		/// <param name="m"></param>
		public void SendBroadcast(Message m) 
		{
			m.User = _ui.Name;
			_ums.Send(m.ToWire());
			//this._uml_MessageReceived(m.ToWire());
		}

		public void Dispose()
		{
			if (!disposing) 
			{
				disposing = true;
				SendBroadcast(new Message(Actions.Part));
				_uml.StopListener();
				nd.Dispose();
				if (this.ListenerThread != null) 
					try 
					{
						this.ListenerThread.Abort();
					} 
					catch {}
				if (this.RebroadcastThread != null) 
					try 
					{
						this.RebroadcastThread.Abort();
					} 
					catch {}
			}
		}

		/// <summary>
		/// Resets the game and creates a new game.
		/// </summary>
		public void ResetGame() 
		{
			if (this.CurrentMode == Mode.Server)
				this.nd.UnRemote();
			this.game = null;
			CreateGame();
			if (this.GameReset != null)
				GameReset();
		}

		private void game_NewGame()
		{
			this.Playing = true;
			this.User.MyPiece = Pieces.X;
			this.CurrentMode = Mode.Server;
			this.game.XPlayer = User.Name;
			this.game.ClientDone += new VoidHandler(game_ClientDone);
			if (this.NewGame != null)
				NewGame();
		}

		private void game_NextMove(int i)
		{
			if (this.GameMove != null)
				this.GameMove(i);
		}

		private void game_GameWon(int[] i)
		{
			if (this.GameWon != null)
				this.GameWon(i);
		}

		private void game_ClientDone()
		{
			this.ResetGame();
		}
	}
}

