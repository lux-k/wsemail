using System;

namespace TicTacToe
{
	public delegate void VoidHandler();
	public delegate void IntHandler(int i);
	public delegate void MessageHandler(Message m);
	public delegate void IntArrayHandler(int[] i);
	public delegate void StringHandler(string s);

	public enum Pieces {X,O,Neither};
	public enum Actions {Join, Part, Chat, Win, Draw};
	public enum Mode {Client, Server};
}
