/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Configuration;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Xml;
using WSEmailProxy;
using SysManCommon;
using MailServerInterfaces;

namespace WSEmailServer
{
	public class SysMan : IExtensionProcessor
	{
		/// <summary>
		/// Reference to the Mail Server interface
		/// </summary>
		protected MailServerConfiguration MailServer = null;

		public SysMan()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		protected bool _init = false, _shut = false;

		public bool IsInitialized 
		{
			get 
			{
				return _init;
			}
		}

		public bool IsShutdown
		{
			get 
			{
				return _shut;
			}
		}

		public string Extension 
		{
			get 
			{
				return "SysMan";
			}
		}

		public XmlElement ProcessRequest(XmlElement args, ProcessingEnvironment env) 
		{
			ExtensionArgument e = new ExtensionArgument(args);
			ExtensionArgument r = new ExtensionArgument("Response");
			switch ((RequestType)Enum.Parse(typeof(RequestType),e.MethodName,true)) 
			{
				case RequestType.ServerRestart:
				{
					FileInfo fi = new FileInfo(ConfigurationSettings.AppSettings["ForceLibraryLoadDirectory"] + @"\..\web.config");
					fi.LastWriteTime = DateTime.Now;
				}
					break;
				case RequestType.UnloadPlugin:
				{
					MethodInfo m = MailServer.GetType().GetMethod("UnloadPlugin",BindingFlags.Public | BindingFlags.Instance);
					bool res = (bool)m.Invoke(MailServer,new object[] {e["Plugin"],TranslateType(e["Queue"])});
					if (res) 
						r["Plugin"] = e["Plugin"] + " was successfully unloaded.";
					else
						r["Plugin"] = e["Plugin"] + " was not successfully unloaded.";

				}
					break;
					//Unload("name",type);
					/* unload() {
					 * if type==x then ...
					 * */
					
				case RequestType.LoadPlugin:
				{
					MethodInfo m = MailServer.GetType().GetMethod("LoadPlugin",BindingFlags.NonPublic | BindingFlags.Instance);
					foreach (string s in e.Arguments) 
					{
						bool res = (bool)m.Invoke(MailServer,new object[] {s});
						if (res)
							r[s] = s + " was loaded successfully.";
						else
							r[s] = s + " was not loaded successfully.";
					}
				}
					break;
				case RequestType.DataRequest:
				{
					r = ProcessDataRequest(e);
				}
					break;
				case RequestType.PluginStatus:
				{
				
				}
					break;
				case RequestType.CoreStatus: 
				{
					foreach (string s in e.Arguments) 
					{
						CorePlugins t = (CorePlugins)Enum.Parse(typeof(CorePlugins),s,true);
						switch (t) 
						{
							case CorePlugins.DatabaseManager:
								r[s] = MailServer.Database.GetStatus();
								break;
							case CorePlugins.LocalMTA:
								r[s] = MailServer.LocalMTA.GetStatus();
								break;
							case CorePlugins.MessageAccessor:
								r[s] = MailServer.MessageAccessor.GetStatus();
								break;
							case CorePlugins.MessageQueue:
								r[s] = MailServer.DeliveryQueue.GetStatus();
								break;
							case CorePlugins.DataAccessor:
								r[s] = MailServer.DataAccessor.GetStatus();
								break;
						}
					}

				}
					break;
			}
			return r.AsXmlElement();
		}

		public object GetField(string field) 
		{
			FieldInfo f = MailServer.GetType().GetField(field,BindingFlags.NonPublic | BindingFlags.Instance);
			object o = f.GetValue(MailServer);
			return o;
		}

		private ExtensionArgument ProcessDataRequest(ExtensionArgument e) 
		{
			string ret = "";
			ExtensionArgument r = new ExtensionArgument("Response");
			foreach (string s in e.Arguments) 
			{
				DataRequested t = (DataRequested)Enum.Parse(typeof(DataRequested),s,true);
				switch (t) 
				{
					case DataRequested.ServerName:
						ret = MailServer.Name;
						break;
					case DataRequested.ServerURL:
						ret = MailServer.Url;
						break;
					case DataRequested.SMTPRelay:
						ret = MailServer.SMTPRelay;
						break;
					case DataRequested.Router:
						ret = MailServer.Router;
						break;
					case DataRequested.DNSServer:
						ret = MailServer.DnsServer;
						break;
					case DataRequested.Certificate:
						ret = MailServer.Certificate.GetName();
						break;
					case DataRequested.Database:
						ret = MailServer.Database.GetType().FullName;
						break;
					case DataRequested.LocalMTA:
						ret = MailServer.LocalMTA.GetType().FullName;
						break;
					case DataRequested.DeliveryQueue:
						ret = MailServer.DeliveryQueue.GetType().FullName;
						break;
					case DataRequested.MessageAccessor:
						ret = MailServer.MessageAccessor.GetType().FullName;
						break;
					case DataRequested.DataAccessor:
						ret = MailServer.DataAccessor.GetType().FullName;
						break;
					case DataRequested.StartTime:
						ret = MailServer.StartTime.ToString();
						break;
					case DataRequested.Uptime:
						ret = MailServer.Uptime.ToString() + " seconds";
						break;
					case DataRequested.PluginList:
					{
//						PluginTypes p = (PluginTypes)Enum.Parse(typeof(SysManCommon.PluginTypes),e[s]);
						object o = this.GetQueue(TranslateType(e[s]));
						if (o is Hashtable) 
						{
							Hashtable h = (Hashtable)o;
							foreach (string key in h.Keys)
								ret += h[key].GetType().FullName +  ",";
						} 
						else if (o is Array) 
						{
							foreach (object k in (Array)o) 
								ret += k.GetType().FullName + ",";
						}
					}
						break;
				}
				if (ret != null && ret.EndsWith(","))
					ret = ret.Substring(0,ret.Length - 1);
				r[s]=ret;
			}
			return r;
		}

		/// <summary>
		/// Translates a string in to the interface name.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		private Type TranslateType(string s) 
		{
			switch (s) 
			{
				case "MappedAddress":
					return typeof(IMappedAddress);
				case "SendingProcessor":
					return typeof(ISendingProcessor);
				case "DeliveryProcessor":
					return typeof(IDeliveryProcessor);
				case "Service":
					return typeof(IService);
				case "ExtensionProcessor":
					return typeof(IExtensionProcessor);
			}
			return null;
		}


		/// <summary>
		/// Initializes the plugin and receives a reference to the mail server.
		/// </summary>
		/// <param name="m">Mail server interface reference</param>
		/// <returns>Bool if successful initialization</returns>
		public bool Initialize(IMailServer m) 
		{
			MailServer = (MailServerConfiguration)m;
			_init = true;
			return true;
		}

		/// <summary>
		/// Gets the current status from the plugin. Doesn't do much of anything.
		/// </summary>
		/// <returns></returns>
		public string GetStatus() 
		{
			return "Hello!";
		}

		/// <summary>
		/// Returns the type of the plugin, in this case PluginType.MappedAddress
		/// </summary>
		/// <returns></returns>
		public MailServerInterfaces.PluginType PluginType  
		{
			get 
			{
				return MailServerInterfaces.PluginType.Service;
			}
		}

		private object GetQueue(Type t) 
		{
			MethodInfo m = MailServer.GetType().GetMethod("GetQueue",BindingFlags.NonPublic | BindingFlags.Instance);
			object o = null;
//			try 
//			{
				o = m.Invoke(MailServer,new object[] {t});
/*			}	
			catch (Exception e) 
			{
				MailServer.Log(e.Message);
				MailServer.Log(e.StackTrace);
				if (e.InnerException != null) 
				{
					MailServer.Log(e.InnerException.Message);
					MailServer.Log(e.InnerException.StackTrace);
				}
			}
			*/
			return o;
		}

		/// <summary>
		/// Shuts down the plugin. Again, it doesn't do much here.
		/// </summary>
		/// <returns></returns>
		public bool Shutdown() 
		{
			_shut = true;
			return true;
		}
	}
}
