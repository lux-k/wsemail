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
using FederatedBinaryToken;

namespace WSEmailServer
{
	/// <summary>
	/// Through an extension to WSEmail servers, this plugin can process requests to create federated tokens.
	/// </summary>
	public class FederatedTokenBroker: IExtensionProcessor
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public FederatedTokenBroker()
		{
		}
	

		/// <summary>
		/// Reference to the Mail Server interface
		/// </summary>
		protected IMailServer MailServer = null;


		protected bool _init = false, _shut = false;

		/// <summary>
		/// Returns whether or not this plugin is initialized
		/// </summary>
		public bool IsInitialized 
		{
			get 
			{
				return _init;
			}
		}

		/// <summary>
		/// Returns whether or not this plugin is shutdown.
		/// </summary>
		public bool IsShutdown
		{
			get 
			{
				return _shut;
			}
		}

		/// <summary>
		/// Returns the extension name this plugin responds to
		/// </summary>
		public string Extension 
		{
			get 
			{
				return "FederatedTokenBroker";
			}
		}

		/// <summary>
		/// Requests a token.
		/// </summary>
		/// <param name="args">Expects to load the KeyContainer and KeyInfo elements from here</param>
		/// <param name="env">Needed to bind the UserID</param>
		/// <returns>Summary string</returns>
		private XmlElement RequestToken(ExtensionArgument args, ProcessingEnvironment env) 
		{
			if (env.AuthenticatingTokenType == AuthenticatingTokenEnum.FederatedIdentity) 
			{
				MailServer.Log(MailServerLogType.UserAuthenticationError, "Refusing to issue a federated token when only identity information is a federated token.");
				return null;
			}

			MailServer.Log(MailServerLogType.ServerDebug, "Received in RequestToken() : " + args.AsXmlElement().OuterXml);
			if (args == null) 
			{
				MailServer.Log(MailServerLogType.UserAuthenticationError, "Can't process federated identity request with no arguments.");
				return null;
			}

			if (args["KeyContainer"] == null)
				throw new Exception("Key container can not be null.");

			if (args["KeyInfo"] == null)
				throw new Exception("RSA Params can not be null.");

			FederatedToken f = new FederatedToken(args["KeyContainer"],System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(args["KeyInfo"])));
			f.IdentityToken.UserID = env.UserID;
			f.IdentityToken.Identifer = Guid.NewGuid().ToString();
			f.IdentityToken.CheckURL = MailServer.Url;
			f.IdentityToken.ValidFrom = System.DateTime.UtcNow.ToString("r");
			f.IdentityToken.ValidTill = System.DateTime.UtcNow.AddDays(1).ToString("r");
			f.IdentityTokenSignature = f.IdentityToken.Sign(MailServer.WSE1SecurityToken.Certificate);
			env.ResponseContext.Security.Tokens.Add(f);
			
			XmlDocument d = new XmlDocument();
			XmlElement e = d.CreateElement("RequestFederatedTokenResult");
			e.InnerText = "Token granted to " + env.UserID;
			MailServer.Log(MailServerLogType.UserAuthentication,"Federated token issued to " + env.UserID);
			return e;
		}

		/// <summary>
		/// Processes requests from the mailserver.
		/// </summary>
		/// <param name="args">Arguments in XML given by client</param>
		/// <param name="env">Calling environment</param>
		/// <returns>Results in XML format</returns>
		public XmlElement ProcessRequest(XmlElement args, ProcessingEnvironment env) 
		{
			MailServer.Log(MailServerLogType.ServerDebug, "Received this as args: " + args.OuterXml);
			ExtensionArgument e = new ExtensionArgument(args);
			
			if (e.MethodName.Equals("RequestFederatedToken"))
				return RequestToken(e,env);
			
			return null;
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
				return MailServerInterfaces.PluginType.Extension;
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


