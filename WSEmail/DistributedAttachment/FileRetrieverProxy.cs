/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/


using System.Diagnostics;
using System.Xml.Serialization;
using System.Xml;
using System;
using System.IO;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Services;
using System.Collections;
using Microsoft.Web.Services2.Security.Tokens;
using Microsoft.Web.Services2.Security;

namespace DistributedAttachment
{

	/// <summary>
	/// The file retriever proxy. This is basically a small subset of the WSEmail proxy which can
	/// only talk to the ExecuteExtentsion handler.
	/// </summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Web.Services.WebServiceBindingAttribute(Name="MailServerSoap", Namespace="http://securitylab.cis.upenn.edu/WSEmail")]
	public class FileRetrieverProxy : Microsoft.Web.Services2.WebServicesClientProtocol 
	{
    
		private SecurityToken _sectok = null;

		public void Init() 
		{
			this.Url = "http://localhost/WSEMailServer/MailServer.asmx";
			this.RequestSoapContext.Security.Timestamp.TtlInSeconds = 600;
		}

		/// <remarks/>
		public FileRetrieverProxy() 
		{
			Init();
		}

		public FileRetrieverProxy(string u) 
		{
			Init();
			this.Url = u;
		}

		public SecurityToken SecurityToken 
		{
			get 
			{
				return this._sectok;
			}
			set 
			{
				if (value != null) 
				{
					this._sectok = value;
					this.RequestSoapContext.Security.Tokens.Add(_sectok);
					MessageSignature sig = new MessageSignature(_sectok);
					this.RequestSoapContext.Security.Elements.Add(sig);
				}
			}
		}


		#region WSEmailFetchHeaders
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://securitylab.cis.upenn.edu/WSEmail/ExecuteExtensionHandler", RequestNamespace="http://securitylab.cis.upenn.edu/WSEmail", ResponseNamespace="http://securitylab.cis.upenn.edu/WSEmail", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public XmlElement ExecuteExtensionHandler(string ext, XmlElement args) 
		{
			object[] results = this.Invoke("ExecuteExtensionHandler", new object[] {ext,args});
			return ((XmlElement)(results[0]));
		}
    
		/// <remarks/>
		public System.IAsyncResult BeginExecuteExtensionHandler(string ext, XmlElement args,System.AsyncCallback callback, object asyncState) 
		{
			return this.BeginInvoke("ExecuteExtensionHandler", new object[] {ext,args}, callback, asyncState);
		}
    
		/// <remarks/>
		public XmlElement EndExecuteExtensionHandler(System.IAsyncResult asyncResult) 
		{
			object[] results = this.EndInvoke(asyncResult);
			return ((XmlElement)(results[0]));
		}

		#endregion

	}
}
