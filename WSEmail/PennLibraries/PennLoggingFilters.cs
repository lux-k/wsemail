/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System.Configuration;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using Microsoft.Web.Services2;
using System.Xml;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;


namespace PennLibraries
{
	/// <summary>
	/// Provides the logging support for WSEmail.
	/// </summary>
	public class InputLoggingFilter :  SoapInputFilter
	{
		/// <summary>
		/// Default constructor. Doesn't do a whole lot.
		/// </summary>
		public InputLoggingFilter()
		{
		}
		
		/// <summary>
		/// Logs the message.
		/// </summary>
		/// <param name="env">Reference to the SoapEnvelope of the request.</param>
		public override void ProcessMessage ( SoapEnvelope env ) 
		{
			Utilities.LogEvent(LogType.MessageReceived, this + " : received message\n"+env.OuterXml);

		}
	}
	/// <summary>
	/// Provides the logging support for WSEmail.
	/// </summary>
	public class OutputLoggingFilter :  SoapOutputFilter
	{
		/// <summary>
		/// Default constructor. Doesn't do a whole lot.
		/// </summary>
		public OutputLoggingFilter()
		{
		}
		
		/// <summary>
		/// Logs the message.
		/// </summary>
		/// <param name="env">Reference to the SoapEnvelope of the request.</param>
		public override void ProcessMessage ( SoapEnvelope env ) 
		{
			Utilities.LogEvent(LogType.MessageSent, this + " : sent message\n"+env.OuterXml);

		}
	}
}

