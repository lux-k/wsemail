/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.0.3705.288
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by wsdl, Version=1.0.3705.288.
// 
using System.Diagnostics;
using System.Xml.Serialization;
using System;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Services;

namespace PennLibraries
{

	/// <remarks/>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Web.Services.WebServiceBindingAttribute(Name="ReportSoap", Namespace="http://securitylab.cis.upenn.edu/WSEmail")]
	public class WebReporter : System.Web.Services.Protocols.SoapHttpClientProtocol 
	{
    
		/// <remarks/>
		public WebReporter() 
		{
			this.Url = "http://tower.cis.upenn.edu/WSEmailErrorReporting/Report.asmx";
		}
    
		/// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://securitylab.cis.upenn.edu/WSEmail/ReportError", RequestNamespace="http://securitylab.cis.upenn.edu/WSEmail", ResponseNamespace="http://securitylab.cis.upenn.edu/WSEmail", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public bool ReportError(string s) 
		{
			object[] results = this.Invoke("ReportError", new object[] {
																		   s});
			return ((bool)(results[0]));
		}
    
		/// <remarks/>
		public System.IAsyncResult BeginReportError(string s, System.AsyncCallback callback, object asyncState) 
		{
			return this.BeginInvoke("ReportError", new object[] {
																	s}, callback, asyncState);
		}
    
		/// <remarks/>
		public bool EndReportError(System.IAsyncResult asyncResult) 
		{
			object[] results = this.EndInvoke(asyncResult);
			return ((bool)(results[0]));
		}
	}
}