using System;
using DynamicForms;

namespace BabyObjects
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class BabyObject : BaseObject
	{
		public const string Url = "http://tower/classes/BabyObjects.dll";
		// just a stupid message.
		public string theMessage = "Hello, I am a baby object!";
		public bool approve = false;
		BabyForm bf;

		public BabyObject()
		{
			InitializeDynamicConfiguration();
		}

		public override void InitializeDynamicConfiguration() 
		{
			this.Configuration.Version = 1.0F;
			this.Configuration.Url = Url;	
			this.Configuration.DLL = "BabyObjects";
			this.Configuration.Description = "A test form.";
			this.Configuration.Name = "BabyObjects.BabyObject";
		}

		public override void Destroy() 
		{
			bf.Dispose();
		}

		public override string DebugToScreen() 
		{
			return theMessage;
		}

		public override void Run() 
		{
			bf = new BabyForm();
			bf.Done += new BabyForm.NullDelegate(FormDone);
			bf.Baby = this;
			bf.Show();
		}
			
		public void FormDone() 
		{
			// the form should've worked on a reference to this object
			base.Done(this);
			bf.Hide();

		}
	}
}
