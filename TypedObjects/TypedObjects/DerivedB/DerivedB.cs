using System;
using BaseObject;

namespace DerivedB
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class B : Base
	{
		public const string Url = "http://tower.cis.upenn.edu/classes/DerivedB.dll";
		public string myStr = "HELLO!";

		public B()
		{
			this.Configuration.Version = 1.0F;
			this.Configuration.Url = Url;
		}

		public override string DoYourThing() 
		{
			return "This is a DerivedB object. "+ this.GetType().FullName;
		}
	}
}
