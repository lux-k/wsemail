using System;
using System.IO;

namespace AttachService
{
	/// <summary>
	/// Summary description for AttachUtiliy.
	/// </summary>
	public class AttachUtility
	{
		public static System.IO.StreamWriter sw = null;
		public AttachUtility()
		{
			//
			// TODO: Add constructor logic here
			//
			
		}

		public static void Log(String msg)
		{
			if (sw==null)
				sw = new StreamWriter("C:\\testlog.txt",true);
			sw.WriteLine("");
			sw.WriteLine(msg);
		}
	}
}
