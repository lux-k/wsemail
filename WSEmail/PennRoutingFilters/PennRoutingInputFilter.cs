using System.Configuration;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using Microsoft.Web.Services;
using Microsoft.Web.Services.Security;
using Microsoft.Web.Services.Security.X509;
using System.Xml;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
//using System.Net;
//using System.Net.Sockets;

namespace PennLibraries 
{

	/// <summary>
	/// Provides the input functionality of the PennRouting scheme coming in from the wire.
	/// </summary>
	public class PennRoutingInputFilter :  SoapInputFilter
	{
		/// <summary>
		/// Default constructor. Doesn't do a whole lot.
		/// </summary>
		public PennRoutingInputFilter()
		{
		}
		
		/// <summary>
		/// Overrides the ProcessMessage method of a SoapInputFilter. This will take the 
		/// routing signature out of the message, rebuild the signature object and then verify
		/// the signature of the previous hop.
		/// </summary>
		/// <param name="env">Reference to the SoapEnvelope of the request.</param>
		public override void ProcessMessage ( Microsoft.Web.Services.SoapEnvelope env ) 
		{
			/*		XmlNodeList xnl = env.Body.GetElementsByTagName("Sender");
					if (xnl.Count > 0) 
						xnl[0].InnerText = "BLARG!";
			*/
			PennRoutingUtilities.LogEvent(this + " : received message\n"+env.OuterXml);
			return;

			PennRoutingUtilities.LogEvent(this + " : received message, going to verify\n"+env.OuterXml);

			// get a handle to the penn signature.
			XmlNodeList xnl = env.Body.GetElementsByTagName("PennSignature");
			if (xnl.Count == 0)
				return;
			XmlElement xe = (XmlElement)xnl[0];
			// and the signature element within that.
			XmlElement sig = (XmlElement)xe.GetElementsByTagName("Signature")[0];

			// clone the entire pennsignature structure
			// this will be sent to the verification server.
			XmlElement toSend = (XmlElement)xe.Clone();
			// and remove it from the original soap message.
			xe.ParentNode.RemoveChild(xe);

			// recreate the original signme node (this will be used in the signature verification.
			XmlElement toCheck = (XmlElement)env.CreateNode(XmlNodeType.Element,"","SignMe","http://securitylab.cis.upenn.edu/RoutingSignature/");
			// the node's value is the message, which was signed by the previous
			// hop.
			toCheck.InnerText = env.InnerXml;

			// recreate the data object.
			DataObject dO = new DataObject("SoapBody","","",(XmlElement)toCheck);
			// since tosend is a clone of the penn signature,
			// find the signature element within that,
			// append the data object
			(toSend.GetElementsByTagName("Signature")[0]).AppendChild(env.ImportNode(dO.GetXml(),true));

			// at this point, the toSend xml has been recreated as it was on the routing output filter.
			Microsoft.Web.Services.Security.X509.X509Certificate signingCert = PennRoutingUtilities.VerifySignature(toSend);

			if (signingCert == null)
				throw new Exception ("Verification of SOAP message failed!" );
			else 
			{
				env.Context.Add("AuthenticatingCertificate", signingCert);
			}

			PennRoutingUtilities.LogEvent(this + " : verification is good, signature stripped\n" + env.OuterXml);

		}
	}



}
