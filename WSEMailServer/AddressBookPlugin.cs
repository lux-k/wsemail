/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Xml;
using System.Collections;
using System.Configuration;
using MailServerInterfaces;
using WSEmailProxy;
using XmlAddressBook;
using DynamicForms;

namespace WSEmailServer
{
	/// <summary>
	/// Summary description for FederatedAttachmentProcessor.
	/// </summary>
	public class AddressBookPlugin : IExtensionProcessor
	{
		/// <summary>
		/// Reference to the Mail Server interface
		/// </summary>
		protected IMailServer MailServer = null;
		protected AddressBook book = null;
		protected bool bookinit = false;

		public AddressBookPlugin()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		protected bool _init = false, _shut = false;

		private void InitializeBook(ProcessingEnvironment p) 
		{
			if (!bookinit) 
			{
				MailServer.Log("AddressBook filename is : " +p.Server.MapPath("AddressBook.xml"));
				AddressBook.Filename = p.Server.MapPath("AddressBook.xml");
				AddressBook.KeepDeleted = true;
				book = AddressBook.GetInstance();
				bookinit = true;
			}
		}

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
				return "AddressBook";
			}
		}

		public XmlElement ProcessRequest(XmlElement args, ProcessingEnvironment env) 
		{
			InitializeBook(env);
			ExtensionArgument e = new ExtensionArgument(args);
			switch (e.MethodName) 
			{
				case "GetAddresses":
					return GetAddresses(e);
				case "RegisterAddress":
					return RegisterAddress(e,env);
				case "UnregisterAddress":
					return UnregisterAddress(e,env);
			}
			return null;
		}

		private XmlElement RegisterAddress(ExtensionArgument e, ProcessingEnvironment env) 
		{
			AddressBookEntry a = MakeEntry(e,env);
			ExtensionArgument ex = new ExtensionArgument();
			if (!book.DetectDuplicate(a)) 
			{
				book.AddEntry(a);
				e["Result"] = "Thank you for registering in the WSEmail directory.";
			} 
			else 
			{
				AddressBookEntry m = book.GetEntry(a.Email);
				if (m.Deleted == true) 
				{
					m.Deleted = false;
					e["Result"] = "Your entry has been made public.";
				} else
					e["Result"] = "You are already in the WS Email directory.";
			}
			return e.AsXmlElement();
		}

		private AddressBookEntry MakeEntry(ExtensionArgument ex, ProcessingEnvironment e) 
		{
			AddressBookEntry a = new AddressBookEntry();
			a.Email = e.UserID;
			a.ModifiedDate = DateTime.Now;
			a.AddDate = DateTime.Now;
			return a;
		}

		private XmlElement UnregisterAddress(ExtensionArgument e, ProcessingEnvironment env) 
		{
			AddressBookEntry a = book.GetEntry(env.UserID);
			if (a != null)
				book.RemoveEntry(a);

			ExtensionArgument ex = new ExtensionArgument();
			e["Result"] = "Your entry has been removed from the directory.";
			return e.AsXmlElement();
		}

		public XmlElement GetAddresses(ExtensionArgument e) 
		{
			DateTime t = DateTime.MinValue;
			if (e["SinceDate"] != null)
				t = DateTime.Parse(e["SinceDate"]);

			MemoryStream m = ObjectLoader.Serialize(typeof(AddressBookEntry[]),book.GetEntries(t));
			XmlDocument d = new XmlDocument();
			d.Load(m);

			XmlNode n = d.FirstChild;
			if (d.FirstChild.NodeType == XmlNodeType.XmlDeclaration && d.ChildNodes.Count > 1)
				n = d.ChildNodes[1];
			else
				n = null;
			MailServer.Log(n.OuterXml);

			return (XmlElement)n;
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
