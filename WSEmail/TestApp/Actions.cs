using System;
using System.Xml;
using System.Configuration;

namespace TestApp
{
	public enum ACTIONS {SendMessage, SendIM, ListHeaders, RetrieveMessage, DeleteMessage};

	public class Actions : ProbabilisticObject 
	{

		public ACTIONS Get() 
		{
			return (ACTIONS)this.GetObject();
		}
	}

	public class ActionsReader : IConfigurationSectionHandler 
	{
		public object Create(object parent, object configContext, System.Xml.XmlNode section) 
		{
			Actions actions = new Actions();
			Console.WriteLine("ActionsReader reading config...");
			try 
			{
				foreach (XmlNode child in section.ChildNodes) 
				{
					if (child.Attributes != null) 
					{
						int weight = 1;
						//						Console.WriteLine(child.Attributes.Count.ToString());
						//						foreach (XmlNode x in child.ChildNodes)
						//							Console.WriteLine(x.Name + " " + x.Value);
						Console.WriteLine("Loading..." + child.Attributes["name"].Value + " action...");

						if (child.Attributes["weight"] != null)
							weight = int.Parse(child.Attributes["weight"].Value);

						Console.WriteLine("\tWeight: " + child.Attributes["weight"].Value);
						if (child.Attributes["type"] != null) 
						{
							actions.AddObject(Enum.Parse(typeof(ACTIONS),child.Attributes["type"].Value,true),weight);
							Console.WriteLine("\tAction added to collection.");

						}
					}
				}
				return actions;
			} 
			catch (Exception e) 
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
			}
			return actions;
		}

	}
}
