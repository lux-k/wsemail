/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.IO;
using System.Configuration;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Cryptography.Xml;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Web.Services2.Security;
using Microsoft.Web.Services2.Security.Cryptography;

using Microsoft.Web.Services2.Security.Tokens;

using WSEmailProxy;

namespace FederatedBinaryToken
{

	/// <summary>
	/// Handles getting new federated tokens and keep dibs on current tokens.
	/// </summary>
	public class FederatedTokenManager 
	{
		/// <summary>
		/// RSA container
		/// </summary>
		private string _containername;
		/// <summary>
		/// Parameters for creating keys
		/// </summary>
		private CspParameters cp = null;
		/// <summary>
		/// The federated token
		/// </summary>
		private FederatedToken _token = null;
		/// <summary>
		/// The security token used to request federated tokens
		/// </summary>
		private SecurityToken _authtok = null;

		/// <summary>
		/// Gets a federated token to use
		/// </summary>
		public FederatedToken Token 
		{
			get 
			{
				if (_token == null || _token.Expired) 
					_token = this.GetToken();
				return _token;
			}
		}

		/// <summary>
		/// Gets or sets the container name the token's keys are in
		/// </summary>
		public string ContainerName 
		{
			get 
			{
				return _containername;
			}
			set 
			{
				_containername = value;
			}
		}


		/// <summary>
		/// Gets or sets the security token used in requesting federated tokens.
		/// </summary>
		public SecurityToken AuthenticationToken 
		{
			get 
			{
				return _authtok;
			}
			set 
			{
				_authtok = value;
			}
		}

		/// <summary>
		/// Creates a new token manager with a certain container name
		/// </summary>
		/// <param name="containername"></param>
		public FederatedTokenManager(string containername) 
		{
			this.ContainerName = containername;
			Init();
		}

		/// <summary>
		/// Default token manager creates a random container name.
		/// </summary>
		public FederatedTokenManager() 
		{
			this.ContainerName = System.Guid.NewGuid().ToString();
			Init();
		}

		/// <summary>
		/// Initializes the crypto properties of the Csp.
		/// </summary>
		private void Init() 
		{
			CspParameters cp = new CspParameters();
			cp.KeyContainerName = this.ContainerName;
			cp.KeyNumber = 1;
			cp.ProviderType = 1;
		}

		/// <summary>
		/// Gets a federated token. This relies upon the AppSettings[MailServer] configuration option
		/// for the web service that will issue the token.
		/// </summary>
		/// <returns>Federated token</returns>
		public FederatedToken GetToken() 
		{
			System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider(cp);
			rsa.KeySize = 1024;
			RSA15 r = new RSA15(rsa);

			MailServerProxy p = new MailServerProxy(ConfigurationSettings.AppSettings["MailServer"]);
			p.RequestSoapContext.Security.Tokens.Add(this.AuthenticationToken);
			MessageSignature sig = new MessageSignature(this.AuthenticationToken);
			p.RequestSoapContext.Security.Elements.Add(sig);

			ExtensionArgument args = new ExtensionArgument("RequestFederatedToken");
			args.AddArgument("KeyContainer",this.ContainerName);
			args.AddArgument("KeyInfo",Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(rsa.ToXmlString(false))));
			
			p.ExecuteExtensionHandler("FederatedTokenBroker",args.AsXmlElement());
			if (p.ResponseSoapContext.Security.Tokens.Count > 0) 
			{
				foreach (SecurityToken t in p.ResponseSoapContext.Security.Tokens) 
				{
					if (t is FederatedToken)
						this._token = (FederatedToken)t;
				}
			}

			if (this._token != null)
				this._token.SetCryptoProvider(rsa);

			return this._token;
		}

	}
}
