/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using Microsoft.Web.Services2.Dime;
using System.IO;
using System.Xml;
using System.Collections;
using System.Configuration;
using MailServerInterfaces;
using WSEmailProxy;
using DynamicForms;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using DistributedAttachment;



namespace WSEmailServer
{/// <summary>
	/// Summary description for FederatedAttachmentProcessor.
	/// </summary>
	public class FederatedAttachmentProcessor : ISendingProcessor, IExtensionProcessor
	{
		/// <summary>
		/// Reference to the Mail Server interface
		/// </summary>
		protected IMailServer MailServer = null;

		public FederatedAttachmentProcessor()
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
				return "FederatedAttachment";
			}
		}

		public XmlElement ProcessRequest(XmlElement args, ProcessingEnvironment env) 
		{
			XmlNodeList l = args.GetElementsByTagName("FileKey");
			if (l.Count == 0)
				return null;

			DatabaseConnection c = MailServer.Database.Connection;
			SqlCommand com = c.Connection.CreateCommand();
			com.CommandText = "WSEmailRetrieveFederatedAttachment";
			com.CommandType = CommandType.StoredProcedure;
			com.Parameters.Add("@user",env.UserID);
			com.Parameters.Add("@filekey",l[0].InnerText);

			SqlDataReader r = com.ExecuteReader();
			if (r.Read()) 
			{
				byte[] file = (byte[])r.GetValue(0);
				string filetype = r.GetString(1);
				

				MemoryStream s = new MemoryStream(file);
				s.Position = 0;
				DimeAttachment d = new DimeAttachment(filetype,TypeFormat.MediaType,s);
				env.ResponseContext.Attachments.Add(d);
				MailServer.Log(MailServerLogType.ServerInfo, "User " + env.UserID + " requested federated attachment " + l[0].InnerText);
			} else
				MailServer.Log(MailServerLogType.ServerError,"User " + env.UserID + " requested federated attachment " + l[0].InnerText + " and was denied.");
			r.Close();
			MailServer.Database.Free(c);
			return null;
		}
		/// <summary>
		/// Implementation of ProcessMessage (required via IMappedAddress interface) that will see if
		/// it can map the user name to a message. This should always work, because for the mail server to
		/// know that it should hand off processing of a message to a plugin, the plugin has to register
		/// which addresses it can service.
		/// </summary>
		/// <param name="theMessage">The WSEmail Message to respond to</param>
		/// <param name="theSig">The Signature on the WSEmail Message</param>
		/// <returns>WSEmailStatus code</returns>
		public void ProcessSend(WSEmailMessage theMessage, System.Xml.XmlElement theSig, ProcessingEnvironment p) 
		{
			if (p.IsLocalUser)
			{
				if (p.RequestContext.Attachments.Count > 0) 
				{
					ObjectConfiguration[] confs = ObjectLoader.GetFormInventory(theMessage.XmlAttachments);
					if (confs != null) 
					{
						foreach (ObjectConfiguration o in confs) 
						{
							if (o.Name.Equals("DistributedAttachment.DistributedAttachment")) 
							{
								MailServer.Log(MailServerLogType.ServerDebug, this + " : Found distributed attachment.");
								DistributedAttachment.DistributedAttachment d = (DistributedAttachment.DistributedAttachment)(ObjectLoader.LoadObject(theMessage.XmlAttachments[0].OuterXml));
								d.Attachments[0].FileKey = System.Guid.NewGuid().ToString();
								d.Attachments[0].ServerUrl = MailServer.Url;
								theMessage.XmlAttachments[0] = d.AsXmlDocument();

								DatabaseConnection c = MailServer.Database.Connection;
								SqlCommand com = c.Connection.CreateCommand();
								com.CommandText = "WSEmailSaveFederatedAttachment";
								com.CommandType = CommandType.StoredProcedure;
								com.Parameters.Add("@filekey",d.Attachments[0].FileKey);
								MemoryStream s = (MemoryStream)p.RequestContext.Attachments[0].Stream;
								SqlParameter temp = com.Parameters.Add("@attachment",SqlDbType.Image);
								temp.Value = s.ToArray();
								com.Parameters.Add("@filetype",p.RequestContext.Attachments[0].ContentType);
								com.Parameters.Add("@owner",p.UserID);

								try 
								{
									com.ExecuteNonQuery();
									com.Parameters.Clear();
								} 
								catch (Exception e) 
								{
									MailServer.Log(MailServerLogType.ServerError, e.Message);
								}
								
								com.CommandText = "WSEmailSaveFederatedAttachmentAuth";
								com.Parameters.Add("@filekey",d.Attachments[0].FileKey);
								foreach (string recip in theMessage.Recipients.AllRecipients) 
								{
									SqlParameter user = com.Parameters.Add("@user", SqlDbType.Text);
									user.Value = recip;
									com.ExecuteNonQuery();
									com.Parameters.Remove(user);
								}
								MailServer.Database.Free(c);

								MailServer.Log(MailServerLogType.ServerDebug, this + " : Done. New attachment[0]: " + theMessage.XmlAttachments[0].OuterXml);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Initializes the plugin and receives a reference to the mail server.
		/// </summary>
		/// <param name="m">Mail server interface reference</param>
		/// <returns>Bool if successful initialization</returns>
		public bool Initialize(IMailServer m) 
		{
			MailServer = m;
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
