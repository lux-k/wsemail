using System;
using System.Xml;
using BaseObject;


namespace ConsoleApplication1
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			//
			// TODO: Add code to start application here
			//

			System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(".");
			
			foreach (System.IO.FileInfo file in dir.GetFiles("Derived*"))
				file.Delete();
		
			Base baseref = null;
			Service1 s = new Service1();
			XmlNode xn = s.GiveObject();
			
			XmlDocument xd = new XmlDocument();
			xd.AppendChild(xd.ImportNode(xn,true));

            Console.WriteLine(xd.OuterXml);
			baseref = BaseObject.ObjectLoader.LoadObject(xd);
			Console.WriteLine(baseref.GetType().ToString());
			Console.WriteLine(baseref.DoYourThing());
			Console.ReadLine();
			
		}
	}
}
