using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using BaseObject;

namespace DerivedA
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class A : Base
	{
		public const string Url = "http://tower.cis.upenn.edu/classes/DerivedA.dll";

		public string hehe = "HEHE";
		public string hoho = "hoho";

		public A()
		{
			this.Configuration.Version = 1.0F;
			this.Configuration.Url = Url;
		}


		public override string DoYourThing() 
		{
			return ("Hello, this is a DerivedA class. How are you? " + hehe);
		}

	}
}
