/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Resources;
using System.Reflection;
using System.Runtime;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace FormDemo1
{
	/// <summary>
	/// Summary description for FrmDemo.
	/// </summary>
	public class FrmDemo : System.Windows.Forms.Form
	{
		private string[] _p = null;
		private int[] _m = null;
		private Image X,O = null;
		private int _curr = -1;

		private int MoveMade = -1;

		public int CurrentPlayer 
		{
			get 
			{
				return _curr;
			}
			set 
			{
				_curr = value;
			}
		}
		public string[] Players 
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

		public int[] Moves 
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

		public delegate void NullDelegate();
		public event NullDelegate UserDone ;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label player2;
		private System.Windows.Forms.Label player1;
		private System.Windows.Forms.Label spot0;
		private System.Windows.Forms.Label spot1;
		private System.Windows.Forms.Label spot2;
		private System.Windows.Forms.Label spot3;
		private System.Windows.Forms.Label spot4;
		private System.Windows.Forms.Label spot5;
		private System.Windows.Forms.Label spot6;
		private System.Windows.Forms.Label spot7;
		private System.Windows.Forms.Label spot8;
		private System.Windows.Forms.Button btnDone;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrmDemo()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			X = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType().Namespace + ".x.gif"));
			O = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType().Namespace + ".o.gif"));
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		public void ThrowOut() 
		{
			this.Dispose();
		}

		public Label GetLabel(int i) 
		{
			Label l = null;
			System.Reflection.FieldInfo inf =this.GetType().GetField("spot"+i.ToString(),
				System.Reflection.BindingFlags.NonPublic |
				System.Reflection.BindingFlags.Instance);
			object o=inf.GetValue(this);

			if (o != null)
				l = (Label)o;

			return l;
		}

		private void DrawMove(int spot, int i) 
		{
			Label l = GetLabel(spot);
			l.Image = GetImage(i);
		}

		private Image GetImage(int i) 
		{
			if (i == 1) 
				return X;
			else if (i == 2) 
				return O;
			else
				return null;
		}

		private void DoMove(int i) 
		{
			if (i == MoveMade) 
			{
				DrawMove(i,0);
				Moves[MoveMade] = 0;
				MoveMade = -1;
				btnDone.Enabled = false;
			} 
			else if (MoveMade == -1) 
			{
				if (Moves[i] != 0) 
				{
					MessageBox.Show("Someone already has that square!","Oops!");
				} 
				else 
				{
					MoveMade = i;
					DrawMove(i,CurrentPlayer);
					Moves[MoveMade] = CurrentPlayer;
					btnDone.Enabled = true;
					CheckWinner(i);
				}
			} else 
				MessageBox.Show("You already made a move!","Oops!");
		}

		private void CheckWinner(int i) 
		{
			bool win = false;
			switch (i) 
			{
				case 0:
					if ( (Moves[0] == Moves[1] && Moves[0] == Moves[2]) || (Moves[0] == Moves[3] && Moves[0] == Moves[6]) || (Moves[0] == Moves[4] && Moves[0] == Moves[8])) 
						win = true;
					break;
				case 1:
					if ( (Moves[0] == Moves[1] && Moves[1] == Moves[2]) || (Moves[1] == Moves[4] && Moves[1] == Moves[7]))
						win = true;
					break;
				case 2:
					if ( (Moves[0] == Moves[2] && Moves[1] == Moves[2]) || (Moves[2] == Moves[5] && Moves[2] == Moves[8]) || (Moves[2] == Moves[4] && Moves[2] == Moves[6]))
						win = true;
					break;
				case 3:
					if ( (Moves[3] == Moves[4] && Moves[3] == Moves[5]) || (Moves[0] == Moves[3] && Moves[3] == Moves[6])) 
						win = true;
					break;
				case 4:
					if ( (Moves[3] == Moves[4] && Moves[4] == Moves[5]) || (Moves[0] == Moves[4] && Moves[4] == Moves[8]) || (Moves[1] == Moves[4] && Moves[4] == Moves[7]) || (Moves[2] == Moves[4] && Moves[4] == Moves[6])) 
						win = true;
					break;
				case 5:
					if ( (Moves[3] == Moves[5] && Moves[4] == Moves[5]) || (Moves[2] == Moves[5] && Moves[5] == Moves[8]))
						win = true;
					break;
				case 6:
					if (  (Moves[0] == Moves[6] && Moves[3] == Moves[6]) || (Moves[2] == Moves[6] && Moves[4] == Moves[6]) || (Moves[6] == Moves[7] && Moves[6] == Moves[8]))
						win = true;
					break;
				case 7:
					if ( (Moves[6] == Moves[7] && Moves[7] == Moves[8]) || (Moves[1] == Moves[7] && Moves[4] == Moves[7]))
						win = true;
					break;
				case 8:
					if ( (Moves[6] == Moves[8] && Moves[7] == Moves[8]) || (Moves[0] == Moves[8] && Moves[4] == Moves[8]) || (Moves[2] == Moves[8] && Moves[5] == Moves[8]))
						win = true;
					break;
			}
			if (win)
				MessageBox.Show("You win!","Rock on...");
		}

		public void Run() 
		{
			if (Players == null) 
			{
				Players = new string[2];
				Players[0] = InputBox.ShowInputBox("What is your name?");
				Players[1] = InputBox.ShowInputBox("What is your opponents name?");

				Moves = new int[9];
				for (int i = 0; i < 9; i++)
					Moves[i] = 0;

				CurrentPlayer = 1;
			} 
			else 
			{
				for (int i = 0; i < 9; i++)
					DrawMove(i,Moves[i]);
			}
								
			UpdatePlayers();
			this.Show();
		}

		private void UpdatePlayers() 
		{
			player1.Text = Players[0];
			player2.Text = Players[1];
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.player2 = new System.Windows.Forms.Label();
			this.player1 = new System.Windows.Forms.Label();
			this.spot0 = new System.Windows.Forms.Label();
			this.spot1 = new System.Windows.Forms.Label();
			this.spot2 = new System.Windows.Forms.Label();
			this.spot3 = new System.Windows.Forms.Label();
			this.spot4 = new System.Windows.Forms.Label();
			this.spot5 = new System.Windows.Forms.Label();
			this.spot6 = new System.Windows.Forms.Label();
			this.spot7 = new System.Windows.Forms.Label();
			this.spot8 = new System.Windows.Forms.Label();
			this.btnDone = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Black;
			this.label1.Location = new System.Drawing.Point(40, 136);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(304, 8);
			this.label1.TabIndex = 9;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Black;
			this.label2.Location = new System.Drawing.Point(40, 232);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(304, 8);
			this.label2.TabIndex = 10;
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Black;
			this.label3.Location = new System.Drawing.Point(136, 48);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(8, 280);
			this.label3.TabIndex = 11;
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Black;
			this.label4.Location = new System.Drawing.Point(240, 48);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(8, 280);
			this.label4.TabIndex = 12;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(56, 16);
			this.label5.TabIndex = 13;
			this.label5.Text = "Player 1:";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(208, 8);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(56, 16);
			this.label6.TabIndex = 14;
			this.label6.Text = "Player 2:";
			// 
			// player2
			// 
			this.player2.Location = new System.Drawing.Point(264, 8);
			this.player2.Name = "player2";
			this.player2.Size = new System.Drawing.Size(112, 16);
			this.player2.TabIndex = 15;
			// 
			// player1
			// 
			this.player1.Location = new System.Drawing.Point(72, 8);
			this.player1.Name = "player1";
			this.player1.Size = new System.Drawing.Size(112, 16);
			this.player1.TabIndex = 16;
			// 
			// spot0
			// 
			this.spot0.Location = new System.Drawing.Point(40, 48);
			this.spot0.Name = "spot0";
			this.spot0.Size = new System.Drawing.Size(96, 88);
			this.spot0.TabIndex = 18;
			this.spot0.Click += new System.EventHandler(this.spot0_Click);
			// 
			// spot1
			// 
			this.spot1.Location = new System.Drawing.Point(144, 48);
			this.spot1.Name = "spot1";
			this.spot1.Size = new System.Drawing.Size(96, 88);
			this.spot1.TabIndex = 19;
			this.spot1.Click += new System.EventHandler(this.spot1_Click);
			// 
			// spot2
			// 
			this.spot2.Location = new System.Drawing.Point(248, 48);
			this.spot2.Name = "spot2";
			this.spot2.Size = new System.Drawing.Size(96, 88);
			this.spot2.TabIndex = 20;
			this.spot2.Click += new System.EventHandler(this.spot2_Click);
			// 
			// spot3
			// 
			this.spot3.Location = new System.Drawing.Point(40, 144);
			this.spot3.Name = "spot3";
			this.spot3.Size = new System.Drawing.Size(96, 88);
			this.spot3.TabIndex = 21;
			this.spot3.Click += new System.EventHandler(this.spot3_Click);
			// 
			// spot4
			// 
			this.spot4.Location = new System.Drawing.Point(144, 144);
			this.spot4.Name = "spot4";
			this.spot4.Size = new System.Drawing.Size(96, 88);
			this.spot4.TabIndex = 22;
			this.spot4.Click += new System.EventHandler(this.spot4_Click);
			// 
			// spot5
			// 
			this.spot5.Location = new System.Drawing.Point(248, 144);
			this.spot5.Name = "spot5";
			this.spot5.Size = new System.Drawing.Size(96, 88);
			this.spot5.TabIndex = 23;
			this.spot5.Click += new System.EventHandler(this.spot5_Click);
			// 
			// spot6
			// 
			this.spot6.Location = new System.Drawing.Point(40, 240);
			this.spot6.Name = "spot6";
			this.spot6.Size = new System.Drawing.Size(96, 88);
			this.spot6.TabIndex = 24;
			this.spot6.Click += new System.EventHandler(this.spot6_Click);
			// 
			// spot7
			// 
			this.spot7.Location = new System.Drawing.Point(144, 240);
			this.spot7.Name = "spot7";
			this.spot7.Size = new System.Drawing.Size(96, 88);
			this.spot7.TabIndex = 25;
			this.spot7.Click += new System.EventHandler(this.spot7_Click);
			// 
			// spot8
			// 
			this.spot8.Location = new System.Drawing.Point(248, 240);
			this.spot8.Name = "spot8";
			this.spot8.Size = new System.Drawing.Size(96, 88);
			this.spot8.TabIndex = 26;
			this.spot8.Click += new System.EventHandler(this.spot8_Click);
			// 
			// btnDone
			// 
			this.btnDone.Enabled = false;
			this.btnDone.Location = new System.Drawing.Point(288, 352);
			this.btnDone.Name = "btnDone";
			this.btnDone.Size = new System.Drawing.Size(80, 32);
			this.btnDone.TabIndex = 27;
			this.btnDone.Text = "Done...";
			this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
			// 
			// FrmDemo
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(384, 397);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btnDone,
																		  this.spot8,
																		  this.spot7,
																		  this.spot6,
																		  this.spot5,
																		  this.spot4,
																		  this.spot3,
																		  this.spot2,
																		  this.spot1,
																		  this.spot0,
																		  this.player1,
																		  this.player2,
																		  this.label6,
																		  this.label5,
																		  this.label4,
																		  this.label3,
																		  this.label2,
																		  this.label1});
			this.Name = "FrmDemo";
			this.Text = "Tic Tac Toe!";
			this.ResumeLayout(false);

		}
		#endregion

		private void spot0_Click(object sender, System.EventArgs e)
		{
			DoMove(0);
		}

		private void spot1_Click(object sender, System.EventArgs e)
		{
			DoMove(1);
		}

		private void spot2_Click(object sender, System.EventArgs e)
		{
			DoMove(2);
		}

		private void spot3_Click(object sender, System.EventArgs e)
		{
			DoMove(3);
		}

		private void spot4_Click(object sender, System.EventArgs e)
		{
			DoMove(4);
		}

		private void spot5_Click(object sender, System.EventArgs e)
		{
			DoMove(5);
		}

		private void spot6_Click(object sender, System.EventArgs e)
		{
			DoMove(6);
		}

		private void spot7_Click(object sender, System.EventArgs e)
		{
			DoMove(7);
		}

		private void spot8_Click(object sender, System.EventArgs e)
		{
			DoMove(8);
		}

		private void btnDone_Click(object sender, System.EventArgs e)
		{
			if (CurrentPlayer == 1)
				CurrentPlayer = 2;
			else
				CurrentPlayer = 1;

			if (UserDone != null)
				UserDone.DynamicInvoke(null);
		}
	}
}
