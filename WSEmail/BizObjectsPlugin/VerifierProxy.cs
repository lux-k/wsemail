/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Collections;
using System.Configuration;
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
	/// Used to talk to the verifier web service.
	/// </summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Web.Services.WebServiceBindingAttribute(Name="VerifierSoap", Namespace="http://securitylab.cis.upenn.edu/AuthorizedRouting")]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(MappedItem))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(Signature))]
	public class VerifierProxy : Microsoft.Web.Services2.WebServicesClientProtocol 
	{
    
		/// <remarks/>
		public VerifierProxy() 
		{
			this.Url = "http://localhost/BusinessObjectsService/Verifier.asmx";
		}
    
		/// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://securitylab.cis.upenn.edu/AuthorizedRouting/PostObject", RequestNamespace="http://securitylab.cis.upenn.edu/AuthorizedRouting", ResponseNamespace="http://securitylab.cis.upenn.edu/AuthorizedRouting", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public string PostObject(BusinessRequest r) 
		{
			object[] results = this.Invoke("PostObject", new object[] {
																		  r});
			return ((string)(results[0]));
		}
    
		/// <remarks/>
		public System.IAsyncResult BeginPostObject(BusinessRequest r, System.AsyncCallback callback, object asyncState) 
		{
			return this.BeginInvoke("PostObject", new object[] {
																   r}, callback, asyncState);
		}
    
		/// <remarks/>
		public string EndPostObject(System.IAsyncResult asyncResult) 
		{
			object[] results = this.EndInvoke(asyncResult);
			return ((string)(results[0]));
		}
	}
}
