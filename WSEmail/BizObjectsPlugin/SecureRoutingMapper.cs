/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Collections;
using System.Configuration;
using MailServerInterfaces;
using WSEmailProxy;
using DynamicForms;
using System.Xml.Serialization;
using System.Xml;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Services;
using DynamicBizObjects;


namespace BizObjectsPlugin
{

	/// <summary>
	/// Maps email addresses to form routing entities. Use to verify forms or do automated things to forms. (This plugs into
	/// the WSEmail server.)
	/// </summary>
	public class SecureRoutingMapper : IMappedAddress
	{
		/// <summary>
		/// Handle to the mail server.
		/// </summary>
		private IMailServer MailServer = null;
		/// <summary>
		/// A hashtable that holds role names as keys and email addresses as values.
		/// </summary>
		private Hashtable RoleTable = null;

		/// <summary>
		/// Creates a new secure routing mapper and tries to read the role configuration info out of the configuration file (ie. app.config or web.config).
		/// </summary>
		public SecureRoutingMapper()
		{
			object o = ConfigurationSettings.GetConfig("SecureRoutingMapper");
			if (o is Hashtable) 
			{
				RoleTable = (Hashtable)o;
			} 
			else
				throw new Exception("The secure routing mapper is not configured in the application configuration file.");
		}

		protected bool _init = false, _shut = false;

		/// <summary>
		/// Returns true if initialized, false otherwise.
		/// </summary>
		public bool IsInitialized 
		{
			get 
			{
				return _init;
			}
		}

		/// <summary>
		/// Returns true if shutdown, false otherwise.
		/// </summary>
		public bool IsShutdown
		{
			get 
			{
				return _shut;
			}
		}

		/// <summary>
		/// Returns the plugin type, in this case "extension".
		/// </summary>
		public MailServerInterfaces.PluginType PluginType  
		{
			get 
			{
				return MailServerInterfaces.PluginType.Extension;
			}
		}

		private void Log(MailServerLogType t, string s) 
		{
			MailServer.Log(t,s);
		}


		/// <summary>
		/// Returns the actor (email address) of a particular role.
		/// </summary>
		/// <param name="s">Role name</param>
		/// <returns>Email address</returns>
		private string GetRoleActor(string s) 
		{
			Log(MailServerLogType.ServerDebug, "Returning value '" + (string)RoleTable[s] + "' for role '"+s+"'.");
			return (string)RoleTable[s];
		}

		/// <summary>
		/// Implements the saving interface as defined in the WSEmail server. This will process messages
		/// sent to it and perform whatever actions are coded in this bit of code.
		/// </summary>
		/// <param name="theMessage"></param>
		/// <param name="theSig"></param>
		/// <param name="recipient"></param>
		/// <returns></returns>
		public WSEmailStatus ProcessSave(WSEmailMessage theMessage,XmlElement theSig, string recipient) 
		{
			if (RoleTable == null)
				throw new Exception("The roles table is null; can't process role-based messages.");

			string recip = recipient.Split(new char[] {'@'})[0].ToLower();
			Log(MailServerLogType.ServerDebug, this + "Recipient = " + recip);

			BaseObject o = ObjectLoader.LoadObject(theMessage.XmlAttachments[0].OuterXml);
			BizObjectsInterface bz = (BizObjectsInterface)o;
			BusinessRequest br = bz.GetBusinessRequest();

			switch (recip) 
			{
				case "pos":
				{
					Log(MailServerLogType.ServerDebug, "Automated server starting processing on PO message.");

					br.Delegates = new string[] {GetRoleActor("Supervisor"),GetRoleActor("BusinessOffice")};
					br.InsertDelegatesAtFront = true;
					br.Mappings.Add(new MappedItem("Supervisor",GetRoleActor("Supervisor"),null));
					br.Mappings.Add(new MappedItem("BusinessOffice",GetRoleActor("BusinessOffice"),null));
					br.GenerateApproval(MailServer.Name+"POs",true);
					theMessage.Sender = recipient;
					theMessage.Recipients.Clear();
					theMessage.Recipients.Add(br.GetNextHop);
					theMessage.XmlAttachments[0] = o.AsXmlDocument();

					theMessage.Body = "This message was automatically processed by an automation server.\r\n" + theMessage.Body;
					MailServer.DeliveryQueue.Enqueue(theMessage,theMessage.Sign(PennLibraries.Utilities.GetSecurityToken("CapricornPOs",true)),AuthenticatingTokenEnum.Internal);

					return new WSEmailStatus(200,"Message received and processed.");
				}
				case "verifier":
				{
					VerifierProxy vp = new VerifierProxy();
					return new WSEmailStatus(200,vp.PostObject(br));
				}
			}
			return new WSEmailStatus(500,"Recipient not found.");
		}

		/// <summary>
		/// Enumerates the mappings (email IDs) this plugin will handle.
		/// </summary>
		/// <returns></returns>
		public string[] EnumerateMappings() 
		{
			return new string[] {"Verifier","POs"};

		}

		/// <summary>
		/// Initializes this plugin with a reference to the mailserver.
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		public bool Initialize(IMailServer m) 
		{
			MailServer = m;
			_init = true;
			return true;
		}

		/// <summary>
		/// Returns a status string about the plugin.
		/// </summary>
		/// <returns></returns>
		public string GetStatus() 
		{
			return "Hello!";
		}

		/// <summary>
		/// Shuts down the plugin.
		/// </summary>
		/// <returns></returns>
		public bool Shutdown() 
		{
			_shut = true;
			return true;
		}
	}
}


