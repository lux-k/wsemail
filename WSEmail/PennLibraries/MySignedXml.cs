/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using System.IO;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Xml;
using System.Collections;

namespace PennLibraries
{
	/// <summary>
	/// A signed xml object that uses public key cryptography to create and verify signatures.
	/// </summary>
	public class MySignedXml : Microsoft.Web.Services.Security.SignedXml 
	{
		private IEnumerator m_keyInfoEnum = null;
		public MySignedXml() : base() {}
		public MySignedXml(XmlDocument document) : base(document) {}

		const uint X509_ASN_ENCODING 		= 0x00000001;
		const uint PKCS_7_ASN_ENCODING 	= 0x00010000;
		const uint CERT_FIND_SUBJECT_STR	= 0x00080007;
		static uint ENCODING_TYPE 		= PKCS_7_ASN_ENCODING | X509_ASN_ENCODING ;
		const uint RSA_CSP_PUBLICKEYBLOB	= 19;

		[DllImport("crypt32.dll")]
		public static extern bool CryptDecodeObject(
			uint CertEncodingType,
			uint lpszStructType,
			byte[] pbEncoded,
			uint cbEncoded,
			uint flags,
			[In, Out] byte[] pvStructInfo,
			ref uint cbStructInfo);
	
		[StructLayout(LayoutKind.Sequential)]
			public struct PUBKEYBLOBHEADERS 
		{
			public byte bType;	//BLOBHEADER
			public byte bVersion;	//BLOBHEADER
			public short reserved;	//BLOBHEADER
			public uint aiKeyAlg;	//BLOBHEADER
			public uint magic;	 //RSAPUBKEY
			public uint bitlen;	 //RSAPUBKEY
			public uint pubexp;	 //RSAPUBKEY
		}

		// Based off....
		// DecodeCertKey
		// X.509 Certificate to Public Key Decoder
		//
		// Copyright (C) 2003.  Michel I. Gallant
		//

		protected override AsymmetricAlgorithm GetPublicKey() 
		{
			KeyInfoX509Data tempCert;

			if (m_keyInfoEnum == null)
				m_keyInfoEnum = KeyInfo.GetEnumerator();

			while (m_keyInfoEnum.MoveNext()) 
			{
				tempCert = m_keyInfoEnum.Current as KeyInfoX509Data;
				if (tempCert != null) 
				{
					// The code here doesn't look in all the certificates and doesn't check if they belong 
					// to the same certificate chain, or whether they are valid, it does that later.
					if (tempCert.Certificates != null) 
					{
						foreach (System.Security.Cryptography.X509Certificates.X509Certificate x509certificate in tempCert.Certificates) 
						{
							byte[] encodedpubkey = null;
							byte[] publickeyblob = null;

							encodedpubkey = x509certificate.GetPublicKey();
       
							uint blobbytes=0;
							// Display the value to the console.
							if(CryptDecodeObject(ENCODING_TYPE, RSA_CSP_PUBLICKEYBLOB, encodedpubkey, (uint)encodedpubkey.Length, 0, null, ref blobbytes))
							{
								// gets the size of the publickeyblob
								publickeyblob = new byte[blobbytes];
								if (!CryptDecodeObject(ENCODING_TYPE, RSA_CSP_PUBLICKEYBLOB, encodedpubkey, (uint)encodedpubkey.Length, 0, publickeyblob, ref blobbytes)) 
								{
									System.Diagnostics.Debug.WriteLine("Couldn't decode publickeyblob from certificate publickey") ;
									return null;
								}
							}
							else
							{
								System.Diagnostics.Debug.WriteLine("Couldn't decode publickeyblob from certificate publickey") ;
								return null;
							}
	 
							PUBKEYBLOBHEADERS pkheaders = new PUBKEYBLOBHEADERS() ;
							int headerslength = Marshal.SizeOf(pkheaders);
							IntPtr buffer = Marshal.AllocHGlobal( headerslength);
							Marshal.Copy( publickeyblob, 0, buffer, headerslength );
							pkheaders = (PUBKEYBLOBHEADERS) Marshal.PtrToStructure( buffer, typeof(PUBKEYBLOBHEADERS) );
							Marshal.FreeHGlobal( buffer );

							byte[] exponent = BitConverter.GetBytes(pkheaders.pubexp); //returns bytes in little-endian order
							Array.Reverse(exponent);    //PUBLICKEYBLOB stores in LITTLE-endian order; convert to BIG-endian order

							//-----  Get modulus in big-endian byte array, suitable for RSAParameters.Modulus -------------
							int modulusbytes = (int)pkheaders.bitlen/8 ;
							byte[] modulus = new byte[modulusbytes];
							try
							{
								Array.Copy(publickeyblob, headerslength, modulus, 0, modulusbytes);
								Array.Reverse(modulus);   //convert from little to big-endian ordering.
							}
							catch(Exception)
							{
								System.Diagnostics.Debug.WriteLine("Problem getting modulus from publickeyblob");
							}

							RSACryptoServiceProvider oRSA = new RSACryptoServiceProvider();

							//Create a new instance of RSAParameters.
							RSAParameters RSAKeyInfo = new RSAParameters();

							//Set RSAKeyInfo to the public key values. 
							RSAKeyInfo.Modulus = modulus;
							RSAKeyInfo.Exponent = exponent;

							//Import key parameters into RSA.
							oRSA.ImportParameters(RSAKeyInfo);
							System.Diagnostics.Debug.WriteLine("Returning oRSA");
							return oRSA;							
							/*								
							IntPtr KeyBlob = IntPtr.Zero;
							uint KeyBlobSize = 0;
							int result = PublicKeyBlobFromCertificateRawData(x509certificate.GetRawCertData(), x509certificate.GetRawCertData().Length, out KeyBlob, out KeyBlobSize);
							byte[] blob = new byte[KeyBlobSize];
							Marshal.Copy(KeyBlob, blob, 0, (int)KeyBlobSize);
							UnmanagedArrayFree(KeyBlob);
							RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
							RSAParameters parameters = new RSAParameters();

							IntPtr ExponentBlob = IntPtr.Zero, ModulusBlob = IntPtr.Zero;
							uint PubExponent = 0, ModulusSize = 0;
							ImportPublicKeyBlob(blob, blob.Length, out PubExponent, out ModulusBlob, out ModulusSize);

							parameters.Exponent = ConvertIntToByteArray(PubExponent);
							parameters.Modulus = new byte[ModulusSize];

							Marshal.Copy(ModulusBlob, parameters.Modulus, 0, (int)ModulusSize);
							UnmanagedArrayFree(ModulusBlob);
							rsa.ImportParameters(parameters);      


							return rsa; 
							*/
						}
					}
				}
			}
			return(null);
		}

	}

	/*
	 * this old code required certgetkey.dll, which was just another dependency on the whole project.
	 * this code now just calls crypt32.dll...
			[DllImport("CertGetKey.dll", ExactSpelling=true)]
			private static extern void UnmanagedArrayFree (IntPtr ppbKeyBlob);

			[DllImport("CertGetKey.dll", ExactSpelling=true)]
			private static extern int PublicKeyBlobFromCertificateRawData (byte[] rgbRawData, int cbRawData, 
				out IntPtr ppbKeyBlob, out uint pKeyBlobSize);

			[DllImport("CertGetKey.dll", ExactSpelling=true)]
			private static extern void ImportPublicKeyBlob (byte[] rgbPubKeyBlob, int cbPubKeyBlob, out uint pExponent, 
				out IntPtr ppbModulus, out uint pModulusSize); 

			private static byte[] ConvertIntToByteArray(uint dwInput) 
			{
				// output of this routine is always big endian
				byte[] rgbTemp = new byte[4]; 
				uint t1, t2;

				if (dwInput == 0) return new byte[1]; 
				t1 = dwInput; 
				int i = 0;
				while (t1 > 0) 
				{
					t2 = t1 % 256;
					rgbTemp[i] = (byte) t2;
					t1 = (t1 - t2)/256;
					i++;
				}
				// Now, copy only the non-zero part of rgbTemp and reverse
				byte[] rgbOutput = new byte[i];
				// copy and reverse in one pass
				for (int j = 0; j < i; j++) 
				{
					rgbOutput[j] = rgbTemp[i-j-1];
				}
				return(rgbOutput);
			}
	*/

}
