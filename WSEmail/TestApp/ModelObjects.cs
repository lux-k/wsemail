using System;
using System.Threading;

namespace TestApp
{
	public class ModelObjects
	{
		public Actions actions;
		public SecurityTokens tokens;
		public Message[] messages;

		public int threads = 0;
		public int total = 0;

		public DateTime starttime;
		private int succeeded = 0;
		private int failed = 0;

		public void AddSuccess() 
		{
			lock(this) 
			{
				succeeded += 1;
			}
			CheckDone();
		}

		public void AddFailure() 
		{
			lock(this) 
			{
				failed += 1;
			}
			CheckDone();
		}

		private void CheckDone() 
		{
			if (failed + succeeded == total) 
			{
				Console.WriteLine("Test run done.");
				float sec = (float)((DateTime.Now.ToFileTime() - starttime.ToFileTime()) / 10000000);
				Log(((float)total/sec).ToString() + " attempts/sec");
			}
		}
		public void Run() 
		{
			Thread[] t = new Thread[this.threads];
			for (int i = 0; i < this.threads; i++) 
			{
				t[i] = new Thread(new ThreadStart( (new TheThread(this,i)).ThreadCode ) );
			}
			starttime = DateTime.Now;
			for (int i = 0; i < this.threads; i++) 
			{
				t[i].Start();
			}
		}

		public void Log(string s) 
		{
			System.Windows.Forms.MessageBox.Show(s);
		}
	}
}
