using System;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Web.Services;
using Microsoft.Web.Services.Security;
using Microsoft.Web.Services.Security.X509;

namespace P3PtoXMLTest
{
	/// <summary>
	/// Summary description for P3Ptester.
	/// </summary>
	class P3Ptester
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			// make the P3P
			P3P.POLICY pol = makeP3P();

			// now load up the certificate from my store
			Microsoft.Web.Services.Security.X509.X509CertificateStore local =
				Microsoft.Web.Services.Security.X509.X509CertificateStore.CurrentUserStore(
					Microsoft.Web.Services.Security.X509.X509CertificateStore.MyStore);

			local.OpenRead();

			// get the first certificate out of the collection
			Microsoft.Web.Services.Security.X509.X509Certificate cert = local.Certificates[0];

			// now take the cert and the P3P policy and make an offer out of it
			UserAgent.Offer off = new UserAgent.Offer();
			off.Cert = cert;
			off.Policy = pol;

			// create the license
			System.Xml.XmlDocument lic = MakeLicense(off);

			// now print it out
			PrintXmlDocument(lic);

			return;
		}

		static P3P.POLICY makeP3P()
		{
			// make a new P3P sample object
			P3P.POLICY p = new P3P.POLICY();

			// statment 1
			P3P.STATEMENT s1 = new P3P.STATEMENT();
			s1.categories.computer = true;
			s1.categories.financial = true;
		
			s1.consequence = "College kids love pizza";

			s1.nonidentifiable = true;

			s1.purpose.admin.present = true;
			s1.purpose.develop.present = true;
			s1.purpose.pseudo_analysis.present = true;

			s1.recipient._public.present = true;
			s1.recipient.other_recipient.present = true;

			s1.retention.business_practices = true;
			s1.retention.stated_purpose = true;

			// put it all together
			p.m_statements[0] = s1;

			return p;
		}


		static void PrintXmlDocument(System.Xml.XmlDocument doc)
		{
			// now print it out
			XmlTextWriter writer = new XmlTextWriter(Console.Out);
			writer.Formatting = Formatting.Indented;
			doc.WriteContentTo( writer );
			writer.Flush();
			Console.WriteLine();

			Console.ReadLine();
		}


		static void PrintP3P(P3P.POLICY p)
		{
			// now print it out
			XmlDocument doc = new XmlDocument();
			XmlElement elem = p.toXML();
			elem = doc.ImportNode(elem, true) as XmlElement;
			doc.AppendChild(elem);
			XmlTextWriter writer = new XmlTextWriter(Console.Out);
			writer.Formatting = Formatting.Indented;
			doc.WriteContentTo( writer );
			writer.Flush();
			Console.WriteLine();

			Console.ReadLine();
		}


		static XmlDocument MakeLicense(UserAgent.Offer off)
		{
			// temporary holder for lists
			System.Xml.XmlNodeList list;

			// prepare a license for sending out with the appropriate information
			System.Xml.XmlDocument lic = new XmlDocument();
			
			// open up the license template file
			System.Xml.XmlTextReader reader = new XmlTextReader("C:\\Documents and Settings\\michael.MIR\\My Documents\\licenseTemplate.xml");
			// ignore all whitespace
			reader.WhitespaceHandling = System.Xml.WhitespaceHandling.None;
			// load up the license template
			lic.Load(reader);

			// decide which rights to add to the license			
			// add them
			// right now just add in a right to send an ad
			// the new right element
			System.Xml.XmlElement right = lic.CreateElement(null, "SendAnyAd", "priv");

			// find the node that is to be replaced
			list = lic.GetElementsByTagName("rights_placeholder", "*");
			// replace the first one with the new right
			list[0].ParentNode.AppendChild(right);
			list[0].ParentNode.RemoveChild(list[0]);
			//lic.ReplaceChild(right, list[0]);			
			
			// plug in the p3p object as XML
			// first translate the policy and then import it to use in our document
			System.Xml.XmlNode n = lic.ImportNode(off.Policy.toXML(), true);
			System.Xml.XmlElement policy = n as XmlElement;
			
			// find the spot in the document where the policy goes
			list = lic.GetElementsByTagName("PrivacyPolicy", "*");
			// and plug it in as a new child
			list[0].AppendChild(policy);

			// include the X.509 Certificate for the merchant that made the offer
			list = lic.GetElementsByTagName("X509Certificate", "*");
			list[0].InnerXml = off.Cert.ToBase64String();

			// include the common name for the company that is making the offer
			// note that this moving up and across the XML hierarchy is completely document
			// specific.  If the license template changes at all, this line must be changed!
			list[0].ParentNode.ParentNode.ParentNode.NextSibling.InnerText = off.Cert.GetName();

			// now send back then new license
			return lic;

		}
		
	}
}
