/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using DynamicForms;

namespace FormDemo1
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Launcher : BaseObject
	{
		public int[] Moves = null;
		public string[] Players = null;
		public int CurrentPlayer = 0;

		private FrmDemo form = null;

		public Launcher()
		{
			InitializeDynamicConfiguration();
		}

		public override string DebugToScreen() 
		{
			return "I am a formlauncher.";
		}

		public override void Dispose() 
		{
			if (!this.IsDisposed) 
			{
				this.IsDisposed = true;
				form.ThrowOut();
				form = null;
			}
		}

		public override void InitializeDynamicConfiguration() 
		{
			this.LockEmail = false;
			this.Configuration.DLL = "FormDemo1";
			this.Configuration.Name = "FormDemo1.Launcher";
			this.Configuration.Url = "http://tower.cis.upenn.edu/classes/FormDemo1.dll";
			this.Configuration.Version = 1.0F;
			this.Configuration.FriendlyName = "Form Demo #1";
			this.Configuration.Description = "Email Tic-Tac-Toe";
		}

		public override void Run() 
		{
			form = new FrmDemo();
			form.Moves = this.Moves;
			form.Players = this.Players;
			form.CurrentPlayer = this.CurrentPlayer;
			form.Run();
			form.UserDone += new FrmDemo.NullDelegate(FormDone);
		}

		private void FormDone() 
		{
			Moves = form.Moves;
			CurrentPlayer = form.CurrentPlayer;
			Players = form.Players;
			base.Done(this);
		}
	}
}

