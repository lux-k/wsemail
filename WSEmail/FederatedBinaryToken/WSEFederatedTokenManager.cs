/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Security.Permissions;
using System.Text;
using System.Globalization;
using System.Xml;
using Microsoft.Web.Services2.Security;
using Microsoft.Web.Services2.Security.Tokens;


namespace FederatedBinaryToken
{
	/// <summary>
	/// Summary description for FederatedTokenManager.
	/// </summary>
	public class WSEFederatedTokenManager : SecurityTokenManager
	{
		public WSEFederatedTokenManager()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override string TokenType
		{ 
			get 
			{
				return FederatedToken.TokenValueType.Name;
			}
		}

		public override SecurityToken LoadTokenFromXml(XmlElement element)
		{
			return new FederatedToken(element);
		}

		public override SecurityToken LoadTokenFromKeyInfo(KeyInfo keyInfo) 
		{
			// Verify that the keyInfo parameter contains a value.
			if (null == keyInfo)
				throw new ArgumentNullException("keyInfo");
			SecurityTokenReference reference;
			foreach ( KeyInfoClause clause in keyInfo )
			{
				reference = (SecurityTokenReference)clause;
				// Check if the custom XML security token is the
				// security token handled by this security token manager.
				if ( clause is SecurityTokenReference && 
					(reference.KeyIdentifier.ValueType ==
					FederatedToken.TokenValueType.Name))
				{
					// If the custom XML security token is the security
					// token managed by this security token manager,
					// get the keys for the security token from the cryptographic
					// key container specified in the KeyIdentifier property.
					byte[] bytes = reference.KeyIdentifier.Value;
					string keyContainer = 
						System.Text.Encoding.Default.GetString(bytes);

					// Let the custom XML security token do the work of
					// retrieving the key for the security token. 
					FederatedToken token = new FederatedToken(keyContainer);

					return token;
				}
				else
				{
					// If this custom XML security token is not the
					// one managed by this security token manager, let the
					// default security token provider try to handle it.
					return null;
				}
			}
			// If execution reaches here, the custom XML security token
			// could not be loaded.
			return null;
		}

		public override void VerifyToken(SecurityToken token) 
		{
			FederatedToken xmlToken = token as FederatedToken;
			if (xmlToken == null)
			{
				throw new ArgumentException(
					"The security token provided is not an FederatedToken instance.", "token");
			}
			else if (xmlToken.Verify() == false)
			{
				throw new ApplicationException("The provided FederatedToken security token is not valid.");
			}
		}

	}
}
