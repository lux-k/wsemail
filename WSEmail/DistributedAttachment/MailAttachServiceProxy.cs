/*


namespace DistributedAttachment 
{
	using System.Diagnostics;
	using System.Xml.Serialization;
	using System.Xml;
	using System;
	using System.IO;
	using System.Web.Services.Protocols;
	using Microsoft.Web.Services2.Security;
	using System.ComponentModel;
	using System.Web.Services;
	using System.Collections;

	using WSEmailProxy;
    
    
	/// <remarks/>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Web.Services.WebServiceBindingAttribute(Name="MailAttachServiceSoap", Namespace="http://securitylab.cis.upenn.edu/WSEmail")]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(WSEmailMessage))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(object[]))]
	public class MailAttachService : System.Web.Services.Protocols.SoapHttpClientProtocol
	//public class MailAttachService : Microsoft.Web.Services.WebServicesClientProtocol
	{
        
		/// <remarks/>
		public MailAttachService() 
		{
			this.Url = "http://localhost/AttachService/MailAttachService.asmx";
		}
        
		/// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://securitylab.cis.upenn.edu/WSEmail/HelloAttach", RequestNamespace="http://securitylab.cis.upenn.edu/WSEmail", ResponseNamespace="http://securitylab.cis.upenn.edu/WSEmail", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public string HelloAttach() 
		{
			object[] results = this.Invoke("HelloAttach", new object[0]);
			return ((string)(results[0]));
		}
        
    
		/// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://securitylab.cis.upenn.edu/WSEmail/WSEmailSend", RequestNamespace="http://securitylab.cis.upenn.edu/WSEmail", ResponseNamespace="http://securitylab.cis.upenn.edu/WSEmail", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public WSEmailStatus WSEmailSend(WSMessageAttach theMessage, System.Xml.XmlElement theSig) 
		{
			object[] results = this.Invoke("WSEmailSend", new object[] {
																		   theMessage,
																		   theSig});
			return ((WSEmailStatus)(results[0]));
		}
        
       
		/// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://securitylab.cis.upenn.edu/WSEmail/WSEmailFetchHeaders", RequestNamespace="http://securitylab.cis.upenn.edu/WSEmail", ResponseNamespace="http://securitylab.cis.upenn.edu/WSEmail", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public WSEmailHeader[] WSEmailFetchHeaders(string recipient) 
		{
			object[] results = this.Invoke("WSEmailFetchHeaders", new object[]{
																			  recipient});
			return ((WSEmailHeader[])(results[0]));
		}
        
        
		/// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://securitylab.cis.upenn.edu/WSEmail/WSEmailRetrieve", RequestNamespace="http://securitylab.cis.upenn.edu/WSEmail", ResponseNamespace="http://securitylab.cis.upenn.edu/WSEmail", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public WSMessageAttach WSEmailRetrieve(int msgID,string vrecipient) 
		{
			object[] results = this.Invoke("WSEmailRetrieve", new object[]{
																			  msgID,
																				vrecipient});
			return ((WSMessageAttach)(results[0]));
		}
        
       
		/// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://securitylab.cis.upenn.edu/WSEmail/WSEmailDelete", RequestNamespace="http://securitylab.cis.upenn.edu/WSEmail", ResponseNamespace="http://securitylab.cis.upenn.edu/WSEmail", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public string WSEmailDelete() 
		{
			object[] results = this.Invoke("WSEmailDelete", new object[0]);
			return ((string)(results[0]));
		}
        
        
		/// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://securitylab.cis.upenn.edu/WSEmail/RequestFederatedToken", RequestNamespace="http://securitylab.cis.upenn.edu/WSEmail", ResponseNamespace="http://securitylab.cis.upenn.edu/WSEmail", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public string RequestFederatedToken() 
		{
			object[] results = this.Invoke("RequestFederatedToken", new object[0]);
			return ((string)(results[0]));
		}
        
        
		/// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://securitylab.cis.upenn.edu/WSEmail/WSEmailRetrieveAttach", RequestNamespace="http://securitylab.cis.upenn.edu/WSEmail", ResponseNamespace="http://securitylab.cis.upenn.edu/WSEmail", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public FileAttachment WSEmailRetrieveAttach(string token,string filename,int msgid) 
		{
			object[] results = this.Invoke("WSEmailRetrieveAttach", new object[]{
																					token,
																					filename,
																					msgid});
			return ((FileAttachment)(results[0]));
		}

		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://securitylab.cis.upenn.edu/WSEmail/setValue", RequestNamespace="http://securitylab.cis.upenn.edu/WSEmail", ResponseNamespace="http://securitylab.cis.upenn.edu/WSEmail", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public int setValue(string s) 
		{
			object[] results = this.Invoke("setValue", new object[] {
													 s});
			return ((int)(results[0]));
		}

		/// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://securitylab.cis.upenn.edu/WSEmail/getValue", RequestNamespace="http://securitylab.cis.upenn.edu/WSEmail", ResponseNamespace="http://securitylab.cis.upenn.edu/WSEmail", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public string getValue() 
		{
			object[] results = this.Invoke("getValue", new object[0]);
			return ((string)(results[0]));
		}


	}
        
}
*/