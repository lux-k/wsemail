/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/


namespace WSEmailConfigurator
{
	/// <summary>
	/// Summary description for GetServersProxy.
	/// </summary>
	using System.Diagnostics;
	using System.Xml.Serialization;
	using System;
	using System.Web.Services.Protocols;
	using System.ComponentModel;
	using System.Web.Services;


	/// <remarks/>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Web.Services.WebServiceBindingAttribute(Name="AvailableServersSoap", Namespace="http://securitylab.cis.upenn.edu/WSEmail")]
	public class AvailableServers : System.Web.Services.Protocols.SoapHttpClientProtocol 
	{
    
		/// <remarks/>
		public AvailableServers() 
		{
			this.Url = "http://tower.cis.upenn.edu/WSEMailServer/AvailableServers.asmx";
		}
    
		/// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://securitylab.cis.upenn.edu/WSEmail/GetServers", RequestNamespace="http://securitylab.cis.upenn.edu/WSEmail", ResponseNamespace="http://securitylab.cis.upenn.edu/WSEmail", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public Server[] GetServers() 
		{
			object[] results = this.Invoke("GetServers", new object[0]);
			return ((Server[])(results[0]));
		}
    
		/// <remarks/>
		public System.IAsyncResult BeginGetServers(System.AsyncCallback callback, object asyncState) 
		{
			return this.BeginInvoke("GetServers", new object[0], callback, asyncState);
		}
    
		/// <remarks/>
		public Server[] EndGetServers(System.IAsyncResult asyncResult) 
		{
			object[] results = this.EndInvoke(asyncResult);
			return ((Server[])(results[0]));
		}
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securitylab.cis.upenn.edu/WSEmail")]
	public class Server 
	{
    
		/// <remarks/>
		public string Url;
    
		/// <remarks/>
		public string ServerName;
    
		/// <remarks/>
		public string Owner;
		public override string ToString() 
		{
			return this.ServerName + " operated by " + this.Owner;
		}
	}




}
