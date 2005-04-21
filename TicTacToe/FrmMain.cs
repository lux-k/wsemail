using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;

namespace TicTacToe
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class FrmMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox chatInput;
		private System.Windows.Forms.Button chatSend;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox chatOutput;
		private System.Windows.Forms.ListView playerList;
		private System.Windows.Forms.GroupBox gameBox;
		private static GameController gc = new GameController();
		private System.Windows.Forms.Label playerLabel;
		private System.Windows.Forms.Label chatLabel;
		private Label[] theBoard = null;
		private Color boardBackColor = Color.LemonChiffon;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem mnuExit;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem mnuNetworkInfo;
		private System.Windows.Forms.MenuItem mnuConnectTo;
		private System.Windows.Forms.MenuItem mnuResetGame;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem mnuAbout;
		private System.Windows.Forms.MenuItem mnuUsername;
		private Color boardWinColor = Color.LawnGreen;
		public FrmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			BindEvents();
			BuildBoard();
			Game_GameReset();
		}

		private void BindEvents() 
		{
			gc.NewMessage +=new MessageHandler(gc_NewMessage);
			gc.PlayerListChanged += new MessageHandler(gc_PlayerListChanged);
			gc.NewGame +=new VoidHandler(Game_NewGame);
			gc.GameMove += new IntHandler(Game_NextMove);
			gc.GameWon += new IntArrayHandler(Game_GameWon);
			gc.GameReset += new VoidHandler(Game_GameReset);
		}

		private void BuildBoard() 
		{
			theBoard = new Label[9];
			int w = 70;
			Random r = new Random();
			for (int i = 0; i < 9; i++) 
			{
				Label l = new Label();
				//				l.Text = i.ToString();
				l.Height = w;
				l.Width = w;
				l.Left = (i % 3) * (w+10) + 20;
				l.Top = (i / 3) * (w+10) + 20;
				l.Name = i.ToString();
				l.BorderStyle = BorderStyle.Fixed3D;
				theBoard[i] = l;
				l.Click +=new EventHandler(spotClick);
				l.Font = new Font("Arial",20,FontStyle.Bold);
				l.TextAlign = ContentAlignment.MiddleCenter;
				l.BackColor = this.boardBackColor;
				if (r.Next(3) == 2)
					l.Text = Pieces.O.ToString();
				else
					l.Text = Pieces.X.ToString();
				this.gameBox.Controls.Add(l);
			}
			this.gameBox.Width = 3 * (w + 20);
			this.playerList.Left = this.gameBox.Left + this.gameBox.Width + 20;
			this.playerLabel.Left = this.gameBox.Left + this.gameBox.Width + 20;
			this.playerList.Width = this.Width - this.playerList.Left - 20;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			gc.Dispose();
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FrmMain));
			this.chatInput = new System.Windows.Forms.TextBox();
			this.chatSend = new System.Windows.Forms.Button();
			this.chatOutput = new System.Windows.Forms.TextBox();
			this.playerList = new System.Windows.Forms.ListView();
			this.gameBox = new System.Windows.Forms.GroupBox();
			this.playerLabel = new System.Windows.Forms.Label();
			this.chatLabel = new System.Windows.Forms.Label();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mnuUsername = new System.Windows.Forms.MenuItem();
			this.mnuConnectTo = new System.Windows.Forms.MenuItem();
			this.mnuResetGame = new System.Windows.Forms.MenuItem();
			this.mnuNetworkInfo = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.mnuExit = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.mnuAbout = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// chatInput
			// 
			this.chatInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.chatInput.Location = new System.Drawing.Point(8, 360);
			this.chatInput.Name = "chatInput";
			this.chatInput.Size = new System.Drawing.Size(368, 20);
			this.chatInput.TabIndex = 0;
			this.chatInput.Text = "";
			this.chatInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.chatInput_KeyPress);
			this.chatInput.TextChanged += new System.EventHandler(this.chatInput_TextChanged);
			// 
			// chatSend
			// 
			this.chatSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.chatSend.Location = new System.Drawing.Point(400, 360);
			this.chatSend.Name = "chatSend";
			this.chatSend.TabIndex = 1;
			this.chatSend.Text = "Send...";
			this.chatSend.Click += new System.EventHandler(this.chatSend_Click);
			// 
			// chatOutput
			// 
			this.chatOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.chatOutput.Location = new System.Drawing.Point(8, 296);
			this.chatOutput.Multiline = true;
			this.chatOutput.Name = "chatOutput";
			this.chatOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.chatOutput.Size = new System.Drawing.Size(480, 56);
			this.chatOutput.TabIndex = 2;
			this.chatOutput.Text = "";
			// 
			// playerList
			// 
			this.playerList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.playerList.Location = new System.Drawing.Point(384, 24);
			this.playerList.Name = "playerList";
			this.playerList.Size = new System.Drawing.Size(104, 248);
			this.playerList.TabIndex = 4;
			this.playerList.DoubleClick += new System.EventHandler(this.playerList_DoubleClick);
			// 
			// gameBox
			// 
			this.gameBox.Location = new System.Drawing.Point(8, 8);
			this.gameBox.Name = "gameBox";
			this.gameBox.Size = new System.Drawing.Size(368, 264);
			this.gameBox.TabIndex = 5;
			this.gameBox.TabStop = false;
			this.gameBox.Text = "Current Game";
			// 
			// playerLabel
			// 
			this.playerLabel.Location = new System.Drawing.Point(384, 8);
			this.playerLabel.Name = "playerLabel";
			this.playerLabel.TabIndex = 6;
			this.playerLabel.Text = "Player list:";
			// 
			// chatLabel
			// 
			this.chatLabel.Location = new System.Drawing.Point(8, 280);
			this.chatLabel.Name = "chatLabel";
			this.chatLabel.TabIndex = 7;
			this.chatLabel.Text = "Chat:";
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem3});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuUsername,
																					  this.mnuConnectTo,
																					  this.mnuResetGame,
																					  this.mnuNetworkInfo,
																					  this.menuItem4,
																					  this.mnuExit});
			this.menuItem1.Text = "&File";
			// 
			// mnuUsername
			// 
			this.mnuUsername.Index = 0;
			this.mnuUsername.Text = "&Username";
			this.mnuUsername.Click += new System.EventHandler(this.mnuUsername_Click);
			// 
			// mnuConnectTo
			// 
			this.mnuConnectTo.Index = 1;
			this.mnuConnectTo.Text = "&Connect to";
			this.mnuConnectTo.Click += new System.EventHandler(this.mnuConnectTo_Click);
			// 
			// mnuResetGame
			// 
			this.mnuResetGame.Index = 2;
			this.mnuResetGame.Text = "&Reset Game";
			this.mnuResetGame.Click += new System.EventHandler(this.mnuResetGame_Click);
			// 
			// mnuNetworkInfo
			// 
			this.mnuNetworkInfo.Index = 3;
			this.mnuNetworkInfo.Text = "&Network Info";
			this.mnuNetworkInfo.Click += new System.EventHandler(this.mnuNetworkInfo_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 4;
			this.menuItem4.Text = "-";
			// 
			// mnuExit
			// 
			this.mnuExit.Index = 5;
			this.mnuExit.Text = "E&xit";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuAbout});
			this.menuItem3.Text = "Help";
			// 
			// mnuAbout
			// 
			this.mnuAbout.Index = 0;
			this.mnuAbout.Text = "About";
			this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
			// 
			// FrmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(496, 393);
			this.Controls.Add(this.gameBox);
			this.Controls.Add(this.playerList);
			this.Controls.Add(this.chatOutput);
			this.Controls.Add(this.chatSend);
			this.Controls.Add(this.chatInput);
			this.Controls.Add(this.playerLabel);
			this.Controls.Add(this.chatLabel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.Name = "FrmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Network Tic-Tac-Toe";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.Closed += new System.EventHandler(this.Form1_Closed);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledException);
			// catch unhanled child thread exceptions
			Application.ThreadException +=new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
			// clean up anything before the app closes
			Application.Run(new FrmMain());
		}

		private static void UnhandledException(object sender, UnhandledExceptionEventArgs e) 
		{
			HandleException((Exception)e.ExceptionObject);
		}

		private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			HandleException(e.Exception);
		}

		private static void HandleException(Exception e) 
		{
			if (!(e is ThreadAbortException)) 
			{
				MessageBox.Show("The following error was detected: " + e.Message, "Oops!",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			}
			gc.ResetGame();
		}

		private void chatInput_TextChanged(object sender, System.EventArgs e)
		{
		
		}

		private void chatSend_Click(object sender, System.EventArgs e)
		{
			if (!chatInput.Text.Equals("")) 
			{
				Message m = new Message(Actions.Chat,chatInput.Text);
				chatInput.Text = "";
				gc.SendBroadcast(m);
			}
		}


		private void PostMessage(string s) 
		{
			chatOutput.Text += s + "\r\n";
			chatOutput.SelectionStart = chatOutput.Text.Length -1;
			chatOutput.ScrollToCaret();
		}

		private void gc_NewMessage(Message m)
		{
			chatOutput.Invoke(new StringHandler(PostMessage),new object[] {m.ToString()});
		}

		private void chatInput_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == 13) 
			{
				e.Handled = true;
				chatSend_Click(this,null);
			}
		}

		private void Form1_Closed(object sender, System.EventArgs e)
		{
			gc.Dispose();
		}

		private void gc_PlayerListChanged(Message m)
		{
			if (m.Action == Actions.Join)
				playerList.Items.Add(m.User);
			if (m.Action == Actions.Part) 
			{
				for (int i = 0; i < playerList.Items.Count; i++) 
				{
					if (playerList.Items[i].Text.Equals(m.User))
						playerList.Items.RemoveAt(i);
				}
			}
		}

		private void playerList_DoubleClick(object sender, System.EventArgs e)
		{
			if (playerList.SelectedIndices.Count == 1) 
				gc.PlayUser(playerList.Items[playerList.SelectedIndices[0]].Text);
		}

		private void Game_NewGame()
		{
			gc.BindEvents();
			string s= "";
			for (int i = 0; i < 9; i++) 
			{
				Pieces p = gc.Game.Board[i];
				if (p == Pieces.Neither)
					s = "";
				else
					s = p.ToString();
			
				theBoard[i].Text = s;
				theBoard[i].BackColor = this.boardBackColor;

			}
			UpdateWhoseMove();

		}

		private void spotClick(object sender, EventArgs e)
		{
			if (gc.Playing) 
			{
				int i = int.Parse(((Label)sender).Name);
				if (gc.Game.CurrentMove == gc.User.MyPiece) 
				{
					if (gc.Game.Board[i] == Pieces.Neither) 
					{
						theBoard[i].Text = gc.User.MyPiece.ToString();
						gc.Game.Move(i,gc.User.MyPiece);
					} 
					else 
						MessageBox.Show("Someone already has that spot!","Oops!",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				} 
				else
					MessageBox.Show("It's not your turn!","Oops!",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			} 
			else 
				MessageBox.Show("You're not currently playing a game!","Oops!",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
		}

		private void UpdateWhoseMove() 
		{
			String s = "Current move: " + gc.Game.CurrentMove.ToString();
			if (gc.Game.CurrentMove == gc.User.MyPiece)
				s += ", your move";
			else
				s += ", opponents move";
			gameBox.Text = s;
		}

		private void Game_NextMove(int j)
		{
			theBoard[j].Text = gc.Game.Board[j].ToString();
			UpdateWhoseMove();
		}

		private void Game_GameWon(int[] j) 
		{
			string opp = "";
			if (gc.User.MyPiece == Pieces.X)
				opp = gc.Game.OPlayer;
			else
				opp = gc.Game.XPlayer;

			if (j == null) 
			{
				if (gc.CurrentMode == Mode.Server)
					gc.SendBroadcast(new Message(Actions.Draw,opp));
			} 
			else 
			{
				theBoard[j[0]].BackColor = this.boardWinColor;
				theBoard[j[1]].BackColor = this.boardWinColor;
				theBoard[j[2]].BackColor = this.boardWinColor;
				if (gc.Game.Board[j[0]] == gc.User.MyPiece) 
				{
					gc.SendBroadcast(new Message(Actions.Win,opp));
					new Thread(new ThreadStart(WonGame)).Start();
				}
				else
					new Thread(new ThreadStart(LostGame)).Start();
			}

			if (gc.CurrentMode == Mode.Client && !gc.Game.Busy) 
			{
				gc.Game.Done();	
				gc.ResetGame();
			}
			
		}

		private void WonGame() 
		{
			gameBox.Text = "You won the last game!";
			MessageBox.Show("You won the game!","Congratulations...",MessageBoxButtons.OK,MessageBoxIcon.Information);
		}

		private void LostGame() 
		{
			gameBox.Text = "You lost the last game.";
			MessageBox.Show("You lost the game!","Oops!",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			gc.Start();

		}

		private void mnuExit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void mnuNetworkInfo_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show("Current network information:\n\nYour IP: " + gc.IP,"Network Information...",MessageBoxButtons.OK,MessageBoxIcon.Information);
		}

		private void mnuConnectTo_Click(object sender, System.EventArgs e)
		{
			string s = InputBox.ShowInputBox("Please enter the IP of the user you want to play...");
			if (!s.Equals(""))
				gc.Play(s);
		}

		private void mnuResetGame_Click(object sender, System.EventArgs e)
		{
			gc.ResetGame();
		}

		private void Game_GameReset()
		{
			this.gameBox.Text = "Not currently playing...";
		}

		private void mnuAbout_Click(object sender, System.EventArgs e)
		{
			FrmAbout f = new FrmAbout();
			f.ShowDialog();
		}

		private void mnuUsername_Click(object sender, System.EventArgs e)
		{
			string u = InputBox.ShowInputBox("Please enter your username:",gc.User.Name);
			if (!u.Equals("")) 
			{
				gc.SendBroadcast(new Message(Actions.Part));
				gc.User.Name = u;
				gc.SendBroadcast(new Message(Actions.Join));
			}
		}
	}
}
