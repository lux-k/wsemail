/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System.Collections;
using System.Xml.Serialization;
using System;
using System.IO;
using System.Xml;
using Microsoft.Web.Services2.Dime;
using Microsoft.Web.Services2.Security;
using Microsoft.Web.Services2.Security.Tokens;

namespace DynamicForms
{
	/// <summary>
	/// A class that should be inherited by all objects that wish to be executable as forms on the WSEmail platform.
	/// </summary>
	/// 
	[Serializable]
	public abstract class BaseObject : IDisposable
	{
		/// <summary>
		/// Default constructor. Will automatically create an objectconfiguration for this object and
		/// fill in the name of this type.
		/// </summary>
		public BaseObject()
		{
			this.Configuration = new ObjectConfiguration();
			this.Configuration.Name = this.GetType().FullName;
		}

		/// <summary>
		/// An array of dime attachments that want to be sent as dime attachments with this form.
		/// </summary>
		[System.Xml.Serialization.XmlIgnore()]
		public DimeAttachment[] DimeAttachments = null;
		/// <summary>
		/// An enum of token types this object can request from its environment. Asking does not guarantee receiving.
		/// </summary>
		public enum TokenType {FederatedToken, LocalAuthenticationToken};

		/// <summary>
		/// If this object is disposed or not.
		/// </summary>
		protected bool IsDisposed = false;
		/// <summary>
		/// Object configuration declaration. Used to serialize/deserialize the object later and to identify its type.
		/// </summary>
		public ObjectConfiguration Configuration;
		
		/// <summary>
		/// Action types for how you want the form to respond.
		/// </summary>
		public enum UpdateActions {REPLACE, ADD, NOACTION};

		/// <summary>
		/// Default is no action, that is to say nothing has been specified
		/// </summary>
		[System.Xml.Serialization.XmlIgnore()]
		public UpdateActions UpdateAction = UpdateActions.NOACTION;
		[System.Xml.Serialization.XmlIgnore()]
		public int Identifier = -1;
		/// <summary>
		/// Must be implemented by inheriting classes. This function, generally speaking, will show a form of
		/// some kind to graphically present the data of the object to a user. See some of the skeleton formlets
		/// for more information.
		/// </summary>
		public abstract void Run();
		/// <summary>
		/// Must be implemented by inheriting classes. This function should clean-up and release all resources created.
		/// </summary>
		public abstract void Dispose();
		/// <summary>
		/// Must be implemented by inheriting classes. This function should fill in the objectconfiguration of the object.
		/// It is included to be a reminder to the implementor to actually include object configuration information.
		/// </summary>
		public abstract void InitializeDynamicConfiguration();
		/// <summary>
		/// Must be implemented by inheriting classes. This should give some type of feedback about the object loaded. It's
		/// probably enough to return ObjectConfiguration.Name, but actual implementation is reserved for the programmer.
		/// </summary>
		/// <returns>An identifying string about the object loaded.</returns>
		public abstract string DebugToScreen();
		/// <summary>
		/// Whether the object should only be allowed to be viewed (true) or edited (false).
		/// </summary>
		[System.Xml.Serialization.XmlIgnore()]
		public bool LockEmail = false;
		
		/// <summary>
		/// Delegate to be used by the EmailClient for receiving back an object.
		/// </summary>
		public delegate void BaseObjectDelegate(BaseObject b);
		/// <summary>
		/// Email clients will bind to this event, however it will be fired in the forms by calling Base.DoneEditing()
		/// </summary>
		public event BaseObjectDelegate DoneEditing;
		/// <summary>
		/// A generic string delegate that can be used by events returning a string.
		/// </summary>
		public delegate void StringDelegate(string s);
		/// <summary>
		/// Fired when the XmlAttachment needs to change who receives the next message.
		/// </summary>
		public event StringDelegate NextHopChanged;

		/// <summary>
		/// A delegate for requesting tokens from the environment.
		/// </summary>
		public delegate SecurityToken RequestTokenDelegate(TokenType t);
		/// <summary>
		/// Raise this event to ask for a token. You might not receive it, though.
		/// </summary>
		public event RequestTokenDelegate TokenRequest;

		/// <summary>
		/// Called from the derived class to fire the method to the WS Email Client.
		/// </summary>
		/// <param name="s">New destination</param>
		protected void HopChanged(string s) 
		{
			if (NextHopChanged != null)
				NextHopChanged.DynamicInvoke(new object[] {s});
		}

		/// <summary>
		/// Attempts to get a security token from the operating environment.
		/// </summary>
		/// <param name="t">The token type</param>
		/// <returns>A token or null</returns>
		protected SecurityToken GetSecurityToken(TokenType t) 
		{
			if (TokenRequest != null) 
			{
				object o = TokenRequest.DynamicInvoke(new object[] {t});
				if (o != null)
					return (SecurityToken)o;
			}
			return null;
		}

		/// <summary>
		/// Takes the current instance of this object and returns it as an XmlDocument.
		/// </summary>
		/// <returns>XmlDocument capable of being read back into being by this object loader.</returns>
		public XmlDocument AsXmlDocument() 
		{
			return ObjectLoader.MemoryStreamToXmlDocument(ObjectLoader.Serialize(this.GetType(),this));
		}

		/// <summary>
		/// Fired when the derived object is finished. This might be triggered by automated processes, a user hitting a
		/// close button on a loaded form, etc.
		/// </summary>
		/// <param name="b">Reference to baseobject type. This is returned to the client program so that it can be reserialized if necessary.</param>
		protected void Done(BaseObject b) 
		{
//			if (ViewOnly) 
//			{
//				if (DoneViewing != null)
//					DoneViewing.DynamicInvoke(new object[] {});
//			} else {
				if (DoneEditing != null) 
					DoneEditing.DynamicInvoke(new object[] {b});
//			}
		}

		/// <summary>
		/// Creates a new instance of a BaseObject-derived object given the Xml string data.
		/// </summary>
		/// <param name="s">String containing Xml data describing the object</param>
		/// <returns>BaseObject type</returns>
		public BaseObject LoadFrom(string s) 
		{
			return (BaseObject)ObjectLoader.Deserialize(this.GetType(),s);
		}

		/// <summary>
		/// Creates a new instance of a BaseObject-derived object given the XmlNode/XmlDocument
		/// </summary>
		/// <param name="d">XmlNode/XmlDocument</param>
		/// <returns>BaseObject type of null</returns>
		public BaseObject LoadFrom(XmlNode d) 
		{
			return LoadFrom(d.OuterXml);
		}
			
		/// <summary>
		/// Converts an Xml-esque string to an XmlDocument
		/// </summary>
		/// <param name="s">Xml string</param>
		/// <returns>XmlDocument</returns>
		public XmlDocument StringToXmlDocument(string s) 
		{
			XmlDocument xd = new XmlDocument();
			xd.LoadXml(s);
			// remove the xml declaration node because it causes problems being embedded in soap messages
			// that also contain it (ie it violates spec)
			if (xd.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
				xd.RemoveChild(xd.FirstChild);

			return xd;
		}

		/// <summary>
		/// Returns the serialized form of this object as an Xml string
		/// </summary>
		/// <returns>Xml string</returns>
		public string AsXmlString() 
		{
			return this.AsXmlDocument().OuterXml;
		}

		/// <summary>
		/// Returns a string describing this object for list boxes and such.
		/// </summary>
		/// <returns>Configuration.FriendlyName description</returns>
		public override string ToString() 
		{
			return this.Configuration.FriendlyName;
		}
	}



}
