using System;
using Microsoft.Web;
using Microsoft.Web.Services;
using Microsoft.Web.Services.Security;
using Microsoft.Web.Services.Security.X509;

namespace GIS
{
	/// <summary>
	/// Utility class that contains methods to extract an X.509 Certificate
	/// </summary>
	public class GetCert
	{
		public GetCert()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Returns the X.509 Certificate that is extracted from a SoapContext object.  This
		/// can be used to take a signed Soap request and extract the X.509 Certificate used
		/// to sign it for later use.  See WSE QuickStart Example ReponseEncryption.
		/// </summary>
		/// <param name="context">The SoapContext to extract the X.509 Cert from</param>
		/// <returns>The X.509 Cert used to sign the SoapContext.  null is invalid or not present.</returns>
		static public X509Certificate GetCertFromContext( Microsoft.Web.Services.SoapContext context)
		{
			// The certificate to encrypt the response with
			X509Certificate signingCertificate = null;

			// No tokens means that the message can be rejected quickly
			if ( context.Security.Tokens.Count == 0 )
				return null;

			// Check for a Signature that signed the soap Body and uses a token that
			// we accept.
			for ( int i = 0; signingCertificate == null && i < context.Security.Elements.Count; i++ )
			{
				Signature signature = context.Security.Elements[i] as Signature;

				// We only care about signatures that signed the soap Body
				if ( signature != null && (signature.SignatureOptions & SignatureOptions.IncludeSoapBody) != 0 )
				{
					X509SecurityToken x509token = signature.SecurityToken as X509SecurityToken;

					if (x509token != null)
					{
						// Save the certificate
						X509Certificate cert = x509token.Certificate;
						if ( cert.SupportsDataEncryption )
						{
							signingCertificate = cert;
						}
					}
				}
			}

			// now that we have the cetificate, return it
			return signingCertificate;
		}
	}
}
