using System;
using DynamicForms;
using System.Reflection;

namespace TestSelfReg
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
			string f = "DynamicBizObjects.dll";

			Assembly a = Assembly.LoadFrom(f);
			Type[] types = a.GetTypes();
			foreach (System.Type t in types) 
			{
				Console.WriteLine("Type: " + t.FullName);
				Console.WriteLine("Is a class: " + t.IsClass.ToString());
				Console.WriteLine("Is an interface: " + t.IsInterface.ToString());
				if (t.BaseType != null) 
				{
					Console.WriteLine("Directly inherits from: " + t.BaseType.FullName);
					if (t.BaseType.FullName.Equals("DynamicForms.BaseObject")) 
					{
						Console.WriteLine("Implements the BaseObject architecture: true");
						Console.WriteLine();
						Console.WriteLine("Found new useable object: " + t.FullName);
                        BaseObject o = (BaseObject)a.CreateInstance(t.FullName);
						Console.WriteLine("Configuration: " + o.Configuration);
						Console.WriteLine();
					}
				}
			}

			Console.WriteLine();
			Console.WriteLine("Press <enter> to continue...");
			string asldjalsdk = Console.ReadLine();
		}
	}
}
