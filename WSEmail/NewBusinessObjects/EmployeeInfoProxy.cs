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
using Microsoft.Web.Services2;
using Microsoft.Web.Services2.Security.Tokens;
using Microsoft.Web.Services2.Security;
using DynamicBizObjects;
using PennLibraries;


/// <remarks/>
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Web.Services.WebServiceBindingAttribute(Name="EmployeeInfoSoap", Namespace="http://securitylab.cis.upenn.edu/AuthorizedRouting")]
public class EmployeeInfoProxy : Microsoft.Web.Services2.WebServicesClientProtocol {
    
    /// <remarks/>
	public EmployeeInfoProxy() 
	{
		this.Url = "http://localhost/BusinessObjectsService/EmployeeInfo.asmx";
		Config();
	}
	public EmployeeInfoProxy(string u) {
		this.Url = u;
		Config();
    }

	private void Config() 
	{
		string s = System.Configuration.ConfigurationSettings.AppSettings["SigningCertificate"];
		if (s == "" || s == "CertA") 
		{
			System.Windows.Forms.MessageBox.Show("A configured X509 certificate is required to lookup employee information!","Oops!");
		} 
		else 
		{
			X509SecurityToken sectok = Utilities.GetSecurityToken(s,false);
			if (sectok == null) 
			{
				System.Windows.Forms.MessageBox.Show("Unable to find certificate '" + s + "'. Please verify that the certificate is correctly listed in the app config and that it is available in the local user store.","Oops!");
			} else {
				this.RequestSoapContext.Security.Tokens.Add(sectok);
				MessageSignature sig = new MessageSignature(sectok);
				this.RequestSoapContext.Security.Elements.Add(sig);
			}
		}
	}
	/// <remarks/>
	[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://securitylab.cis.upenn.edu/AuthorizedRouting/GetEmployeeInformation", RequestNamespace="http://securitylab.cis.upenn.edu/AuthorizedRouting", ResponseNamespace="http://securitylab.cis.upenn.edu/AuthorizedRouting", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
	public EmployeeInformation GetEmployeeInformation() 
	{
		try 
		{
			object[] results = this.Invoke("GetEmployeeInformation", new object[0]);
			return ((EmployeeInformation)(results[0]));
		} 
		catch (Exception e) 
		{
			new ExceptionForm(e,"Unable to retrieve employee information!");
		}
		return null;
	}
    
	/// <remarks/>
	public System.IAsyncResult BeginGetEmployeeInformation(System.AsyncCallback callback, object asyncState) 
	{
		return this.BeginInvoke("GetEmployeeInformation", new object[0], callback, asyncState);
	}
    
	/// <remarks/>
	public EmployeeInformation EndGetEmployeeInformation(System.IAsyncResult asyncResult) 
	{
		object[] results = this.EndInvoke(asyncResult);
		return ((EmployeeInformation)(results[0]));
	}

}