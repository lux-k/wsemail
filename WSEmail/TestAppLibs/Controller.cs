/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Collections;
using System.Xml.XPath;
using System.Xml;
using System.Configuration;

namespace TestAppLibs
{
	/// <summary>
	/// Summary description for Controller.
	/// </summary>
	public class Controller : MarshalByRefObject
	{

		public event EventHandler BeginTest;
		public event EventHandler SlaveCountChange;
		public event EventHandler ResultsReceived;

		public TestResult[] Results = new TestResult[0];
		private int _c = 0;
		private int _t = 0;
		private string _s;
		private Hashtable slaves = new Hashtable();

		public void StartTest() 
		{
			DateTime t = DateTime.Now;
			Delegate[] dels = BeginTest.GetInvocationList();
			foreach (Delegate del in dels) 
			{
				try 
				{
					((EventHandler)del).DynamicInvoke(new object[] {t,EventArgs.Empty});
				} 
				catch (Exception e) 
				{
					System.Windows.Forms.MessageBox.Show(e.Message);
					EventHandler.Remove(BeginTest,del);
				}
			}
		}

		public void PushResults(TestResult[] t) 
		{
			lock (this) 
			{
				ArrayList a = new ArrayList();
				if (Results != null)
					a.AddRange(Results);
				a.AddRange(t);
				Results = (TestResult[])a.ToArray(typeof(TestResult));
				if (ResultsReceived != null)
					ResultsReceived(this,EventArgs.Empty);
			}
		}

		public int SlaveCount 
		{
			get 
			{
				return slaves.Keys.Count;
			}
		}

		public string Slaves 
		{
			get 
			{
				ArrayList a = new ArrayList(slaves.Keys);
				return string.Join(", ",(string[])a.ToArray(typeof(string)));
			}
		}

		public void RegisterSlave(string name) 
		{
			lock(this) {
				if (slaves[name] == null)
					slaves[name] = 1;
				FireSlaveEvent();
			}
		}

		public void UnregisterSlave(string name) 
		{
			lock(this) 
			{
				if (slaves[name] != null)
					slaves.Remove(name);
				FireSlaveEvent();
			}
		}
		
		private void FireSlaveEvent() 
		{
			if (this.SlaveCountChange != null)
				this.SlaveCountChange(this,EventArgs.Empty);
		}

		public string RetrieveConfigurationSection(SECTIONTYPES t) 
		{
			XmlDocument d = new XmlDocument();
			d.Load("TestApp.exe.config");
			XmlNodeList l = d.SelectNodes("/configuration/" + t.ToString());
			if (l.Count > 0)
				return l[0].OuterXml;
			else
				return null;
		}

		public int Threads 
		{
			get 
			{
				///TODO: read thread count from config file or somewhere else
				return _t;
			}
			set 
			{
				_t = value;
			}
		}

		public string Server 
		{
			get 
			{
				///TODO: read server url from config file
				// return "http://tower.cis.upenn.edu/WSEmailServer/MailServer.asmx";
				return ConfigurationSettings.AppSettings["TargetServer"];
			}
			set 
			{
				_s = value;
			}
		}

		public int Cycles 
		{
			get 
			{
				///TODO: right value
				return _c;
			}
			set 
			{
				_c = value;
			}

		}


		public override object InitializeLifetimeService() 
		{
			return null;
		}		

	}
}
