/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System.Collections;
using Microsoft.Web.Services2.Security.X509;
using Microsoft.Web.Services2.Security.Tokens;
using System.Xml;
using System;
using System.Configuration;
using MailServerInterfaces;
using System.Threading;
using WSEmailProxy;

namespace WSEmailServer
{
	/// <summary>
	/// Summary description for MailServerConfiguration.
	/// </summary>
	public class MailServerConfiguration : IMailServer
	{
		protected Statistics _stats = null;
		public Statistics Stats 
		{
			get 
			{
				return _stats;
			}
			set 
			{
				_stats = value;
			}
		}

		/// <summary>
		/// Private access to the isinit data.
		/// </summary>
		protected bool _isinit = false, _disc = false;

		protected string _name, _router, _url, _relay;
		/// <summary>
		/// Private access to the delivery queue
		/// </summary>
		protected IMailQueue _mailq = null;
		/// <summary>
		/// Private access to the mta
		/// </summary>
		protected ILocalMTA _lmta = null;
		/// <summary>
		/// Private access to the message accessor
		/// </summary>
		protected IMessageAccess _ma = null;
		/// <summary>
		/// Private access to the database manager
		/// </summary>
		protected IDatabaseManager _db = null;
		/// <summary>
		/// Private access to the data accessor
		/// </summary>
		protected IDataAccessor _da = null;
		/// <summary>
		/// Private access to the mapped address queue
		/// </summary>
		protected IMappedAddress[] _addys = null;
		/// <summary>
		/// Private access to the sending queue
		/// </summary>
		protected ISendingProcessor[] _spgs = null;
		/// <summary>
		/// Private access to the delivery queue
		/// </summary>
		protected IDeliveryProcessor[] _idps = null;
		/// <summary>
		/// Private access to service queue
		/// </summary>
		protected IService[] _is = null;
		/// <summary>
		/// Private access to startup time
		/// </summary>
		protected DateTime _st;

		public string LastError 
		{
			get 
			{
				return _last;
			}
		}

		private string _last = "";

		public event EventHandler CleanUpTick;

		/// <summary>
		/// Returns the time this server was started.
		/// </summary>
		public DateTime StartTime 
		{
			get 
			{
				return _st;
			}
		}

		/// <summary>
		/// Returns the number of seconds the server has been online.
		/// </summary>
		public long Uptime 
		{
			get 
			{
				return (long)((DateTime.Now.ToFileTime() - StartTime.ToFileTime()) / 10000000);
			}
		}
		/// <summary>
		/// Contains a list of users (keys) and the programs (values) they are mapped to.
		/// </summary>
		protected Hashtable MappedUsers = null;
		/// <summary>
		/// Contains a mapping of Extensions (keys) and their code (values)
		/// </summary>
		protected Hashtable ExtensionHandlers = null;
		/// <summary>
		/// Contains a list of request IDs (keys) and the date which they expire (values).
		/// </summary>
		protected Hashtable Requests = null;
		/// <summary>
		/// Contains a reference to the thread that periodically empties the requests hashtable.
		/// </summary>
		protected Thread RequestEmptier = null;
		/// <summary>
		/// Contains a reference to the certificate used in communicating.
		/// </summary>
		protected X509Certificate _cert = null;
		/// <summary>
		/// Contains a reference to the DNS servers used in determining message destinations.
		/// </summary>
		protected string _dns;
		/// <summary>
		/// X509 Security Token of the server.
		/// </summary>
		protected X509SecurityToken _certtok = null;
		protected Microsoft.Web.Services.Security.X509SecurityToken _wse1x509tok = null;

		/// <summary>
		/// Returns true if the server has been properly initialized.
		/// </summary>
		public bool IsInitialized 
		{
			get 
			{
				return _isinit;
			}
		}

		/// <summary>
		/// If true, messages will not be saved to the database. If false (default), messages will be saved.
		/// </summary>
		public bool DiscardMessages 
		{
			get 
			{
				return _disc;
			}
			set 
			{
				_disc = value;
			}
		}
		/// <summary>
		/// Maps a recipient's address to a loaded plugin.
		/// </summary>
		/// <param name="addr">Address to load (should be the complete address)</param>
		public void AddMappedAddressPlugin(IMappedAddress addr) 
		{
			bool found=false;
			for (int i = 0; i < _addys.Length; i++) 
			{
				if (_addys[i].Equals(addr))
					found = true;
			}
			if (!found) 
			{
				ArrayList a = new ArrayList(_addys);
				a.Add(addr);
				_addys=(IMappedAddress[])a.ToArray(typeof(IMappedAddress));
			}
		}

		/// <summary>
		/// Runs the set of saving plugins (after delivery)
		/// </summary>
		/// <param name="m"></param>
		/// <param name="theSig"></param>
		/// <param name="recip"></param>
		/// <returns></returns>
		public bool RunSavingPlugins(WSEmailMessage m, XmlElement theSig, string recip) {
			try 
			{
				if (IsAddressMapped(recip) != null) 
				{
					int i = IsAddressMapped(recip).ProcessSave(m, theSig,recip).ResponseCode;
					if (i == 200)
						return false;
					else
						return true;
				}
			} 
			catch (Exception e) 
			{
				Log(MailServerLogType.ServerError,this + " : Error running saving plugin for " + recip + ", " + e.Message + e.StackTrace);
			}
			return true;
		}

		/// <summary>
		/// Runs the set of delivery plugins on a message (before saving)
		/// </summary>
		/// <param name="m"></param>
		/// <param name="theSig"></param>
		/// <param name="recip"></param>
		/// <returns></returns>
		public bool RunDeliveryPlugins(WSEmailMessage m, XmlElement theSig, string recip) 
		{

			try 
			{
				bool res = true;

				foreach (IDeliveryProcessor i in _idps) 
				{
						res = i.ProcessDeliver(m,theSig,recip);
						if (!res)
							break;
				}
				return res;
			} 
			catch (Exception e) 
			{
				Log(MailServerLogType.ServerError,this + " : Error running delivery plugin for " + recip + ", " + e.Message);
			}
			return true;

		}

		/// <summary>
		/// Adds another extension handler to the list.
		/// </summary>
		/// <param name="addr"></param>
		public void AddExtensionProcessorPlugin(IExtensionProcessor addr) 
		{
			ExtensionHandlers[addr.Extension] = addr;
			Log(MailServerLogType.ServerInfo,this + " : Mapped extension '" + addr.Extension + "' to program (" + addr.GetType().FullName + ")");
		}

		/// <summary>
		/// Maps a recipient's address to a loaded plugin.
		/// </summary>
		/// <param name="addr">Address to load (should be the complete address)</param>
		public void AddSendingProcessorPlugin(ISendingProcessor proc) 
		{
			if (proc == null)
				return;

			bool found=false;
			for (int i = 0; i < _spgs.Length; i++) 
			{
				if (_spgs[i].Equals(proc))
					found = true;
			}
			if (!found) 
			{
				ArrayList a = new ArrayList(_spgs);
				a.Add(proc);
				_spgs=(ISendingProcessor[])a.ToArray(typeof(ISendingProcessor));
			}
		}

		/// <summary>
		/// Adds a service to set.
		/// </summary>
		/// <param name="proc"></param>
		public void AddService(IService proc) 
		{
			if (proc == null)
				return;

			bool found=false;
			for (int i = 0; i < _is.Length; i++) 
			{
				if (_is[i].Equals(proc))
					found = true;
			}
			if (!found) 
			{
				ArrayList a = new ArrayList(_is);
				a.Add(proc);
				_is=(IService[])a.ToArray(typeof(IService));
			}
		}

		/// <summary>
		/// Adds a delivery processor to the set.
		/// </summary>
		/// <param name="proc"></param>
		public void AddDeliveryProcessorPlugin(IDeliveryProcessor proc) 
		{
			if (proc == null)
				return;

			bool found=false;
			for (int i = 0; i < _idps.Length; i++) 
			{
				if (_idps[i].Equals(proc))
					found = true;
			}
			if (!found) 
			{
				ArrayList a = new ArrayList(_idps);
				a.Add(proc);
				_idps=(IDeliveryProcessor[])a.ToArray(typeof(IDeliveryProcessor));
			}
		}

		/// <summary>
		/// Cleans up the resources used by this configuration, mostly the request ID (replay attack protection) thread.
		/// </summary>
		~MailServerConfiguration() 
		{
			if (RequestEmptier != null && RequestEmptier.ThreadState == ThreadState.Running)
				RequestEmptier.Abort();
		}

		/// <summary>
		/// Attempts to delete expired requests every so often. This is part of the replay protection code.
		/// </summary>
		private void RequestEmptierThreadCode() 
		{
			int i = 0;
			while (true) 
			{
				i = 0;
				Thread.Sleep(30000);
				
				if (CleanUpTick != null)
					CleanUpTick(this,EventArgs.Empty);

				if (Requests.Keys.Count > 0) 
				{
					ArrayList a= new ArrayList();
					foreach (object o in Requests.Keys) 
					{
						if ( DateTime.Now.ToUniversalTime().Ticks > ((DateTime)Requests[o]).Ticks ) 
							a.Add(o);
					}

					foreach (object o in a) 
					{
						Requests.Remove(o);
						i++;
					}

					a.Clear();
				}
			}
		}

		/// <summary>
		/// The URL of the WS-Email server.
		/// </summary>
		public string Url 
		{
			get 
			{
				return _url;
			}
			set 
			{
				_url = value;
			}
		}

		/// <summary>
		/// The DNS server WSEmail uses to determine routes.
		/// </summary>
		public string DnsServer 
		{
			get 
			{
				return _dns;
			}
			set 
			{
				_dns = value;
			}
		}

		/// <summary>
		/// The name of the WS-Email server. ie. in user@maila, the server name is maila.
		/// </summary>
		public string Name 
		{
			get 
			{
				return _name;
			}
			set 
			{
				_name = value;
			}
		}

		/// <summary>
		/// The WS-Email's default mail router. This is hardwired at the moment, but might be based on something
		/// else at some point.
		/// </summary>
		public string Router 
		{
			get 
			{
				return _router;
			}
			set 
			{
				_router = value;
			}
		}

		/// <summary>
		/// Holds a reference to an item of type IMailQueue. This is used to deliver messages to any destination, including
		/// the local server.
		/// </summary>
		public IMailQueue DeliveryQueue 
		{
			get 
			{
				return _mailq;
			}
			set 
			{
				_mailq = value;
			}
		}

		/// <summary>
		/// Holds a reference to an object implementing ILocalMTA. This object is used to access the functionality of
		/// serializing a message directly to a database.
		/// </summary>
		public ILocalMTA LocalMTA
		{
			get 
			{
				return _lmta;
			}
			set 
			{
				_lmta = value;
			}
		}

		/// <summary>
		/// Returns the certificate that should be used to sign responses.
		/// </summary>
		public X509Certificate Certificate 
		{
			get 
			{
				return _cert;
			}
			set 
			{
				_cert = value;
			}
		}

		
		/// <summary>
		/// Returns the security token that should be used to sign responses.
		/// </summary>
		public Microsoft.Web.Services.Security.X509SecurityToken WSE1SecurityToken 
		{
			get 
			{
				return _wse1x509tok;
			}
			set 
			{
				_wse1x509tok = value;
			}
		}

		/// <summary>
		/// Holds a reference to an object implementing IMessageAccess. This object is essentially a POP/IMAP interface
		/// to the mail server for retrieving and deleting messages.
		/// </summary>
		public IMessageAccess MessageAccessor
		{
			get 
			{
				return _ma;
			}
			set 
			{
				_ma = value;
			}
		}

		/// <summary>
		/// Returns the server to be used for SMTP relaying.
		/// </summary>
		public string SMTPRelay 
		{
			get 
			{
				return _relay;
			}
			set 
			{
				_relay = value;
			}
		}

		/// <summary>
		/// Holds a reference to an object implementing IDatabaseManager. This object hands out connections to a database
		/// and presumably has other features such as connection pooling.
		/// </summary>
		public IDatabaseManager Database 
		{
			get 
			{
				return _db;
			}
			set 
			{
				_db = value;
			}
		}

		/// <summary>
		/// Holds a reference to an object implementing IDataAccessor. This is the data abstraction layer between the database and WSEmail objects.
		/// </summary>
		public IDataAccessor DataAccessor 
		{
			get 
			{
				return _da;
			}
			set 
			{
				_da = value;
			}
		}

		public X509SecurityToken X509Token
		{
			get 
			{
				return this._certtok;
			}
			set 
			{
				this._certtok = value;
			}
		}

		/// <summary>
		/// Runs the named extension, giving it the args and environment.
		/// </summary>
		/// <param name="ext"></param>
		/// <param name="args"></param>
		/// <param name="env"></param>
		/// <returns></returns>
		public XmlElement RunExtensionHandler(string ext, XmlElement args, ProcessingEnvironment env) 
		{
			try 
			{
				if (ExtensionHandlers[ext] != null) 
				{
					Log(MailServerLogType.ServerInfo,this + " : Running handler for extension " + ext);
					return ((IExtensionProcessor)ExtensionHandlers[ext]).ProcessRequest(args,env);
				} 
				else 
				{
					Log(MailServerLogType.ServerError,"Extension " + ext + " is not configured, but was requested.");
					ExtensionArgument a = new ExtensionArgument("fault");
					a.AddArgument("fault","This server is not configured to execute the requested extension.");
					return a.AsXmlElement();
				}
			} 
			catch (Exception e) 
			{
				Log(MailServerLogType.ServerError,"Error running extension... " + e.Message + e.StackTrace);
				ExtensionArgument a = new ExtensionArgument("fault");
				a.AddArgument("fault","An error occurred while running the requested extension.");
				return a.AsXmlElement();
			}
		}

		/// <summary>
		/// Default constructor. It will basically start a thread to empty the request buffer every so often.
		/// </summary>
		public MailServerConfiguration()
		{
			_st = DateTime.Now;
			_addys = new IMappedAddress[0];
			_spgs = new ISendingProcessor[0];
			_idps = new IDeliveryProcessor[0];
			_is = new IService[0];
			_stats = new Statistics();

			RequestEmptier = new Thread(new ThreadStart(RequestEmptierThreadCode));
			RequestEmptier.Start();
		}

		/// <summary>
		/// Initializes the configuration. This will read some configuration information (router, server, etc) out
		/// of the configuration XML file and create some more objects.
		/// </summary>
		/// <returns></returns>
		public bool Initialize() 
		{
			Log(MailServerLogType.ServerDebug,this + " : Initialize() starting...");
			this.Router = ConfigurationSettings.AppSettings["MailRouter"];
			this.Name = ConfigurationSettings.AppSettings["MailServerName"];
			this.DnsServer = ConfigurationSettings.AppSettings["DnsServer"];
			this.SMTPRelay = ConfigurationSettings.AppSettings["SMTPRelay"];
			string temp = ConfigurationSettings.AppSettings["DiscardMessages"];
			if (temp != null)
				this.DiscardMessages = bool.Parse(temp);
			X509SecurityToken s = PennLibraries.Utilities.GetSecurityToken(ConfigurationSettings.AppSettings["SigningCertificate"],true);
			if (s == null) 
			{
				Log(MailServerLogType.ServerError,this + " : Unable to load server certificate... make sure there is a certificate with CN = '" + 
					ConfigurationSettings.AppSettings["SigningCertificate"] + "' in the local computer/personal store " +
					"and that the userid this webserver is running as has permission to access it.");
				return false;
			}
			this.X509Token = s;
			
			if (!X509Token.SupportsDigitalSignature) 
			{
				Log(MailServerLogType.ServerError,this + " : Loaded certificate does not support digital signatures. Either the certificate was made that way or the private key isn't accessible.");
				return false;
			}

			this.WSE1SecurityToken = new Microsoft.Web.Services.Security.X509SecurityToken(new Microsoft.Web.Services.Security.X509.X509Certificate(s.Certificate.GetRawCertData()));

			//Log(MailServerLogType.ServerStatus,this + " : Server starting... running as UserID " + Environment.UserName);

			string tot = "";
			Hashtable h = new Hashtable(Environment.GetEnvironmentVariables());
			foreach (string key in h.Keys)
				tot += "Variable: " + key + "\r\nValue: " + h[key] + "\r\n\r\n";
				
			Log(MailServerLogType.ServerStatus,this + " : server environment\r\n\r\n" + tot);

			this.Certificate = s.Certificate;
			Log(MailServerLogType.ServerStatus,this + " : Loaded signing certificate, Config CN: " + ConfigurationSettings.AppSettings["SigningCertificate"] + ", CN: " + this.Certificate.GetName());
			MappedUsers = new Hashtable();
			Requests = new Hashtable();
			ExtensionHandlers = new Hashtable();
			
			if (InitializeHandlers()) 
			{
				LoadPlugins();
				_isinit = true;
			} else 
				return false;
                
			Log(MailServerLogType.ServerDebug,this + " : Initialize() finished...");
			return true;
		}

		/// <summary>
		/// Creates an object from an assembly. (For loading plugins).
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		protected object CreateObject(string s) 
		{
			System.Reflection.Assembly ass = System.Reflection.Assembly.Load( s.Split('.')[0]);
			return ass.CreateInstance(s);
		}

		/// <summary>
		/// Reads the configuration file and loads the plugins listed.
		/// </summary>
		private void LoadPlugins() 
		{
			Log(MailServerLogType.ServerDebug,this + " : LoadPlugins() starting...");
			object o = ConfigurationSettings.GetConfig("Plugins");
			if (o != null) 
			{
				string[] plugins = (string[])o;
				foreach (string s in plugins) 
				{
					LoadPlugin(s);
				}
			}
			Log(MailServerLogType.ServerDebug,this + " : LoadPlugins() finished...");
		}
	
		/// <summary>
		/// Removes a certain plugin from the given queue.
		/// </summary>
		/// <param name="arr"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		private bool RemovePluginFromQueue(ref Array arr, string name) 
		{
			int pre =0, post = 0;
			
			pre = arr.Length;
			ArrayList a = new ArrayList(arr);
			Type t = arr.GetType().GetElementType();
			Log(MailServerLogType.Debug,t.FullName);
			for (int i = 0; i < a.Count; i++) 
			{
				if ( a[i].GetType().FullName.Equals(name) ) 
				{
					IServerPlugin p = (IServerPlugin)a[i];
					a.RemoveAt(i);
					i--;
					ShutdownPlugin(p);
				}
			}
			arr = a.ToArray(t);
			post = arr.Length;
			return !(pre==post);
		}
		
		/// <summary>
		/// Unloads a plugin
		/// </summary>
		/// <param name="name">Full namespace.name of the plugin</param>
		/// <param name="t">Type of the plugin</param>
		/// <returns></returns>
		public bool UnloadPlugin(string name, Type t) 
		{
			Log(MailServerLogType.Debug,"Unload hit, name is: " + t.Name);
			
			switch (t.Name) 
			{
				case "IMappedAddress":
				{
					Array a = _addys;
					bool b = RemovePluginFromQueue(ref a,name);
					_addys = (IMappedAddress[])a;
					if (b) 
					{
						ArrayList l = new ArrayList();
						foreach (string s in MappedUsers.Keys) 
						{
							if (MappedUsers[s].GetType().FullName.Equals(name))
								l.Add(s);
						}

						foreach (string s in l)
							MappedUsers.Remove(s);
					}
					return b;
				}
				case "ISendingProcessor":
				{
					Array a = _spgs;
					bool b = RemovePluginFromQueue(ref a,name);
					_spgs = (ISendingProcessor[])a;
					return b;
				}
				case "IDeliveryProcessor":
				{
					Array a = _idps;
					bool b = RemovePluginFromQueue(ref a,name);
					_idps = (IDeliveryProcessor[])a;
					return b;
				}
				case "IService":
				{
					Array a = _is;
					bool b = RemovePluginFromQueue(ref a,name);
					_is = (IService[])a;
					return b;
				}
				case "IExtensionProcessor":
				{
					bool res = false;
					ArrayList l = new ArrayList();
					foreach (string s in ExtensionHandlers.Keys) 
					{
						
						if (ExtensionHandlers[s].GetType().FullName.Equals(name)) 
						{
							IServerPlugin p = (IServerPlugin)ExtensionHandlers[s];
							ShutdownPlugin(p);
							l.Add(s);
							res = true;
						}
					}

					if (res) 
					{
						foreach (string s in l)
							ExtensionHandlers.Remove(s);
					}
					return res;
				}
			}
			return false;
		}

		/// <summary>
		/// Gets the appropriate queue depending on what type of plugin you ask for. (Could return an array
		/// or hashtable).
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		private object GetQueue(Type t) 
		{
			if (t != null) 
			{
				switch (t.Name) 
				{
					case "IMappedAddress":
						return _addys;
					case "ISendingProcessor":
						return _spgs;
					case "IDeliveryProcessor":
						return _idps;
					case "IService":
						return _is;
					case "IExtensionProcessor":
						return ExtensionHandlers;
				}
			}
			return null;
		}

		/// <summary>
		/// Loads a namespace.name plugin and adds it to the appropriate queue.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		private bool LoadPlugin(string s) 
		{
			IServerPlugin p = null;
			try 
			{
				p = (IServerPlugin)CreateObject(s);
			} 
			catch (Exception e) 
			{
				Log(MailServerLogType.ServerWarning,this + " : Unable to load plugin " + s + ", error: " + e.Message);
			}

			if (p != null) 
			{
				if (p is IMappedAddress)
				{
					IMappedAddress a = (IMappedAddress)p;
					a.Initialize(this);
					this.AddMappedAddressPlugin(a);
					foreach (string addy in a.EnumerateMappings())
						this.MapAddress(addy,a);
					Log(MailServerLogType.ServerInfo,this + " : Finished loading mapped address plugin : " + s);

				} 
					
				if (p is ISendingProcessor) 
					//else if (p.GetPluginType() == PluginType.MessageProcessor) 
				{
					ISendingProcessor a = (ISendingProcessor)p;
					//ServerConfiguration.addy = a;
					if (!a.IsInitialized)
						a.Initialize(this);
					this.AddSendingProcessorPlugin(a);
					Log(MailServerLogType.ServerInfo,this + " : Finished loading sending processor plugin : " + s);
				}

				if (p is IExtensionProcessor) 
					//else if (p.GetPluginType() == PluginType.MessageProcessor) 
				{
					IExtensionProcessor a = (IExtensionProcessor)p;
					//ServerConfiguration.addy = a;
					if (!a.IsInitialized)
						a.Initialize(this);
					this.AddExtensionProcessorPlugin(a);
					Log(MailServerLogType.ServerInfo,this + " : Finished loading extension plugin : " + s);
				}

				if (p is IDeliveryProcessor) 
					//else if (p.GetPluginType() == PluginType.MessageProcessor) 
				{
					IDeliveryProcessor a = (IDeliveryProcessor)p;
					//ServerConfiguration.addy = a;
					if (!a.IsInitialized)
						a.Initialize(this);
					this.AddDeliveryProcessorPlugin(a);
					Log(MailServerLogType.ServerInfo,this + " : Finished loading delivery plugin : " + s);
				}

				if (p is IService) 
				{
					IService a = (IService)p;

					if (!((IServerPlugin)a).IsInitialized)
						((IServerPlugin)a).Initialize(this);
					this.AddService(a);
					Log(MailServerLogType.ServerInfo,this + " : Finished loading service : " + s);
				}
				return true;
			}
					
			else 
			{
				Log(MailServerLogType.ServerWarning,this + " : Unable to load the class " + s + ".");
				return false;
			}
		}

		/// <summary>
		/// Loads the core handlers.
		/// </summary>
		/// <returns></returns>
		private bool InitializeHandlers() 
		{
			try 
			{
				Log(MailServerLogType.ServerDebug,this + " : InitializeHandlers() starting...");
				Log(MailServerLogType.ServerInfo,this + " : Loading database manager: " + ConfigurationSettings.AppSettings["DatabaseManager"]);
				this.Database = (IDatabaseManager)CreateObject(ConfigurationSettings.AppSettings["DatabaseManager"]);
				this.Database.Initialize(this);

				Log(MailServerLogType.ServerInfo,this + " : Loading data accessor: " + ConfigurationSettings.AppSettings["DataAccessor"]);
				this.DataAccessor = (IDataAccessor)CreateObject(ConfigurationSettings.AppSettings["DataAccessor"]);
				this.DataAccessor.Initialize(this);

				Log(MailServerLogType.ServerInfo,this + " : Loading queue: " + ConfigurationSettings.AppSettings["DeliveryQueue"]);
				this.DeliveryQueue = (IMailQueue)CreateObject(ConfigurationSettings.AppSettings["DeliveryQueue"]);
				this.DeliveryQueue.Initialize(this);

				Log(MailServerLogType.ServerInfo,this + " : Loading local mta: " + ConfigurationSettings.AppSettings["LocalMTA"]);
				this.LocalMTA = (ILocalMTA)CreateObject(ConfigurationSettings.AppSettings["LocalMTA"]);
				this.LocalMTA.Initialize(this);

			
				Log(MailServerLogType.ServerInfo,this + " : Loading message accessor: " + ConfigurationSettings.AppSettings["MessageAccessor"]);
				this.MessageAccessor = (IMessageAccess)CreateObject(ConfigurationSettings.AppSettings["MessageAccessor"]);
				this.MessageAccessor.Initialize(this);
				Log(MailServerLogType.ServerDebug,this + " : InitializeHandlers() finished...");
				return true;
			} 
			catch (Exception e) 
			{
				if (e.InnerException != null)
					Log(MailServerLogType.ServerError,this + " : Error loading handlers: " + e.Message + e.InnerException.Message + e.InnerException.StackTrace);
				else
					Log(MailServerLogType.ServerError,this + " : Error loading handlers: " + e.Message + e.StackTrace);
				return false;
			}
		}
		/// <summary>
		/// Registers a connection in the replay protection buffer. See the seperate paper for how this actually works.
		/// </summary>
		/// <param name="s">RequestID</param>
		/// <param name="t">Expiration</param>
		public void RegisterRequest(string s, DateTime t) 
		{
			if (Requests[s] != null)
				throw new Exception ("Request was already registered!");

			if (DateTime.Now.ToUniversalTime().AddMinutes(10).Ticks < t.Ticks)
				throw new Exception ("Message expiration time is too large!");

			Requests[s] = t;
		}

		/// <summary>
		/// Maps an address to a program.
		/// </summary>
		/// <param name="s">Full (user@MailServer) or partial (user, but will be appended with @ServerName) address</param>
		/// <param name="m">IMappedAddress program to execute</param>
		public void MapAddress(string s, IMappedAddress m) 
		{
			if (s.IndexOf("@") < 0)
				s = s + "@" + this.Name;
			MappedUsers[s.ToLower()] = m;
			Log(MailServerLogType.ServerInfo,this + " : Mapped address '" + s + "' to program (" + m.GetType().FullName + ")");
		}

		/// <summary>
		/// Checks to see whether an address is mapped to a program or not. If it is, the program that should run is
		/// returned. The search is case-insensitive.
		/// </summary>
		/// <param name="s">Full email address (ie. user@mailserver)</param>
		/// <returns></returns>
		public IMappedAddress IsAddressMapped(string s) 
		{
			return (IMappedAddress)MappedUsers[s.ToLower()];
		}

		/// <summary>
		/// Runs sending plugins for a message. Sending plugins are invoked when a message is received but before it is 
		/// delivered.
		/// </summary>
		/// <param name="m"></param>
		/// <param name="theSig"></param>
		/// <param name="p"></param>
		public void RunSendingPlugins(WSEmailProxy.WSEmailMessage m, System.Xml.XmlElement theSig, ProcessingEnvironment p) 
		{
			if (this._spgs != null && this._spgs.Length > 0) 
			{
				Log(MailServerLogType.ServerDebug, this + " : Beginning to execute message processing plugins.");
				foreach (ISendingProcessor proc in _spgs) 
				{
					try 
					{
						proc.ProcessSend(m,theSig,p);
					} 
					catch (Exception e) 
					{
						Log(MailServerLogType.ServerError,"Error running sending plugin " + proc.GetType().FullName + " " + e.Message + e.StackTrace);
					}
				}
				Log(MailServerLogType.ServerDebug, this + " : Done executing message processing plugins.");
			}
		}


		/// <summary>
		/// Logs a message to the server's log file.
		/// </summary>
		/// <param name="s"></param>
		public void Log(string s) 
		{
			PennLibraries.Utilities.LogEvent(s);
		}

		/// <summary>
		/// Logs a messages of a certain logtype
		/// </summary>
		/// <param name="t"></param>
		/// <param name="s"></param>
		public void Log(MailServerLogType t, string s) 
		{
			if (t == MailServerLogType.Error || t == MailServerLogType.ServerError)
				_last = s;

			PennLibraries.LogType l = (PennLibraries.LogType)t;
			PennLibraries.Utilities.LogEvent(l,s);
		}

		/// <summary>
		/// Shuts down a plugin, if it hasn't been shut down already.
		/// </summary>
		/// <param name="p"></param>
		private void ShutdownPlugin(IServerPlugin p) 
		{
			if (!p.IsShutdown) 
			{
				Log(MailServerLogType.ServerInfo,this + " : Shutting down plugin " + p.GetType().Name);
				p.Shutdown();
			}
		}

		/// <summary>
		/// Shuts down this object. Basically it only stops the request buffer emptier thread.
		/// </summary>
		/// <returns></returns>
		public bool Shutdown() 
		{
			if (this.DeliveryQueue != null)
				this.DeliveryQueue.Shutdown();
			if (this.LocalMTA != null)
				this.LocalMTA.Shutdown();
			if (this.DataAccessor != null)
				this.DataAccessor.Shutdown();
			if (this.Database != null)
				this.Database.Shutdown();

			RequestEmptier.Abort();

			ArrayList Plugins = new ArrayList();

			Plugins.AddRange(_addys);
			Plugins.AddRange(_spgs);
			Plugins.AddRange(_idps);
			if (ExtensionHandlers != null)
				Plugins.AddRange(ExtensionHandlers.Values);
			Plugins.AddRange(_is);

			foreach (object o in Plugins) 
			{
				if (o != null)
					ShutdownPlugin((IServerPlugin)o);
			}
			return true;
		}
	}
}
