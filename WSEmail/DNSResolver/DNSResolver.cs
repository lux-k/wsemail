/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Collections;
using System.Net;
using System.Runtime.InteropServices;

namespace DNSResolver
{

	/// <summary>
	/// Resolves DNS queries.
	/// </summary>
	public class Resolver
	{

		private static RecordTypes Records = null;

		public enum CharacterSets 
		{
			DnsCharSetUnknown, 
			DnsCharSetUnicode, 
			DnsCharSetUtf8, 
			DnsCharSetAnsi
		};
		public enum ConfigType 
		{
			DnsConfigPrimaryDomainName_W, 
			DnsConfigPrimaryDomainName_A, 
			DnsConfigPrimaryDomainName_UTF8, 
			DnsConfigAdapterDomainName_W, 
			DnsConfigAdapterDomainName_A, 
			DnsConfigAdapterDomainName_UTF8, 
			DnsConfigDnsServerList, 
			DnsConfigSearchList, 
			DnsConfigAdapterInfo, 
			DnsConfigPrimaryHostNameRegistrationEnabled, 
			DnsConfigAdapterHostNameRegistrationEnabled, 
			DnsConfigAddressRegistrationMaxCount, 
			DnsConfigHostName_W, 
			DnsConfigHostName_A, 
			DnsConfigHostName_UTF8, 
			DnsConfigFullHostName_W, 
			DnsConfigFullHostName_A, 
			DnsConfigFullHostName_UTF8
		};
		public enum NameFormat 
		{
			DnsNameDomain, 
			DnsNameDomainLabel, 
			DnsNameHostnameFull, 
			DnsNameHostnameLabel, 
			DnsNameWildcard, 
			DnsNameSrvRecord
		};
		public enum Section 
		{
			DnsSectionQuestion, 
			DnsSectionAnswer, 
			DnsSectionAuthority, 
			DnsSectionAuthorityAdd
		};
		public class RecordFlags 
		{
			public const UInt32 Section =  0x2;
			public const UInt32 Delete = 0x1;
			public const UInt32 CharSet = 0x2;
			public const UInt32 Unused = 0x3;
			public const UInt32 Reserved = 24;
		}

		/// <summary>
		/// The types of records that can be asked for from DNS.
		/// </summary>
		public class RecordTypes 
		{
			public const UInt16 DNS_TYPE_A = 0x0001; // 1
			public const UInt16 DNS_TYPE_NS = 0x0002; // 2
			public const UInt16 DNS_TYPE_MD = 0x0003; // 3
			public const UInt16 DNS_TYPE_MF = 0x0004; // 4
			public const UInt16 DNS_TYPE_CNAME = 0x0005; // 5
			public const UInt16 DNS_TYPE_SOA = 0x0006; // 6
			public const UInt16 DNS_TYPE_MB = 0x0007; // 7
			public const UInt16 DNS_TYPE_MG = 0x0008; // 8
			public const UInt16 DNS_TYPE_MR = 0x0009; // 9
			public const UInt16 DNS_TYPE_NULL = 0x000a; // 10
			public const UInt16 DNS_TYPE_WKS = 0x000b; // 11
			public const UInt16 DNS_TYPE_PTR = 0x000c; // 12
			public const UInt16 DNS_TYPE_HINFO = 0x000d; // 13
			public const UInt16 DNS_TYPE_MINFO = 0x000e; // 14
			public const UInt16 DNS_TYPE_MX = 0x000f; // 15
			public const UInt16 DNS_TYPE_TEXT = 0x0010; // 16
			public const UInt16 DNS_TYPE_SRV = 0x0021; // 33
			public const UInt16 DNS_TYPE_ALL = 0x00ff; // 255
		}

		/// <summary>
		/// Converts a string representation of a record type constant to its value.
		/// </summary>
		/// <param name="s">A string that is like a Record type constant, ie. DNS_TYPE_SRV</param>
		/// <returns>Value of that constant</returns>
		public static ushort QueryTypeToValue(string s) 
		{
			if (Records == null) Records = new Resolver.RecordTypes();
			System.Reflection.FieldInfo inf = Records.GetType().GetField("DNS_TYPE_" + s,
				System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
			object o=inf.GetValue(Records);
			return (ushort)o;
		}

		/// <summary>
		/// Converts a number into a record type string.
		/// </summary>
		/// <param name="s">Value, ie. 33</param>
		/// <returns>String that is that value type, ie. SRV</returns>
		public static string ValueToQueryType(ushort s) 
		{
			if (Records == null) Records = new Resolver.RecordTypes();
			System.Reflection.FieldInfo[] inf = Records.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
			foreach (System.Reflection.FieldInfo f in inf)
			{
				if ( (ushort)f.GetValue(Records) == s)
					return f.Name.Split('_')[2];
			}
			return "UNKNOWN";
		}

		/// <summary>
		/// Parameters that affect how a query is asked.
		/// </summary>
		public class QueryTypes 
		{
			public const UInt32 DNS_QUERY_STANDARD = 0x00000000;
			public const UInt32 DNS_QUERY_ACCEPT_TRUNCATED_RESPONSE = 0x00000001;
			public const UInt32 DNS_QUERY_USE_TCP_ONLY = 0x00000002;
			public const UInt32 DNS_QUERY_NO_RECURSION = 0x00000004;
			public const UInt32 DNS_QUERY_BYPASS_CACHE = 0x00000008;
			public const UInt32 DNS_QUERY_CACHE_ONLY = 0x00000010;
			public const UInt32 DNS_QUERY_SOCKET_KEEPALIVE = 0x00000100;
			public const UInt32 DNS_QUERY_TREAT_AS_FQDN = 0x00001000;
			public const UInt32 DNS_QUERY_ALLOW_EMPTY_AUTH_RESP = 0x00010000;
			public const UInt32 DNS_QUERY_DONT_RESET_TTL_VALUES = 0x00100000;
			public const UInt32 DNS_QUERY_RESERVED = 0xff000000;
		}

		/// <summary>
		/// Internal is used for marshalling from unmanaged code to .net managed code. Don't use these.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public class Internal 
		{
			[StructLayout(LayoutKind.Sequential)]
				public class DNS_RECORD
			{
				public UInt32  Next = 0;
				public string  Name = null;
				public UInt16  Type = 0;
				public short  wDataLength = 0;
				/*
				  union
				  {
				   DWORD               DW;     // flags as DWORD
				   DNS_RECORD_FLAGS    S;      // flags as structure
				  } Flags;
				*/
				public UInt32  Flags = 0;
				//public RecordFlags Flags;
				public UInt32  dwTtl = 0;
				public UInt32  dwReserved = 0;
			}
			[StructLayout(LayoutKind.Sequential)]
				public class DNS_SERVER_LIST
			{
				public UInt32 addrcount = 0;
				public UInt32 ip4addr;
			}

			[StructLayout(LayoutKind.Sequential)]
				public class MX_RECORD
			{
				public UInt32  Next = 0;
				public string  Name = null;
				public ushort  Type = 0;
				public short  wDataLength = 0;
				public UInt32  Flags = 0;
				public UInt32  dwTtl = 0;
				public UInt32  dwReserved = 0;
				public string MailExchanger = null;
				public UInt16 Preference = 0;
				public UInt16 Pad = 0;
			}

			[StructLayout(LayoutKind.Sequential)]
				public class SOA_RECORD
			{
				public UInt32  Next = 0;
				public string  Name = null;
				public ushort  Type = 0;
				public short  wDataLength = 0;
				public UInt32  Flags = 0;
				public UInt32  dwTtl = 0;
				public UInt32  dwReserved = 0;
				public string pNamePrimaryServer = null;
				public string pNameAdministrator = null;
				public UInt32 dwSerialNo = 0;
				public UInt32 dwRefresh = 0;
				public UInt32 dwRetry = 0;
				public UInt32 dwExpire = 0;
				public UInt32 dwDefaultTtl = 0;
			}

			[StructLayout(LayoutKind.Sequential)]
				public class SRV_RECORD
			{
				public UInt32  Next = 0;
				public string  Name = null;
				public ushort  Type = 0;
				public short  wDataLength = 0;

				public UInt32  Flags = 0;
				//public RecordFlags Flags;
				public UInt32  dwTtl = 0;
				public UInt32  dwReserved = 0;
				public string pNameTarget;
				public UInt16 wPriority;
				public UInt16 wWeight;
				public UInt16 wPort;
				public UInt16 wPad;
			}

			[StructLayout(LayoutKind.Sequential)]
			public class TXT_RECORD
			{
				public UInt32  Next = 0;
				public string  Name = null;
				public ushort  Type = 0;
				public short  wDataLength = 0;
				public UInt32  Flags = 0;
				//public RecordFlags Flags;
				public UInt32  dwTtl = 0;
				public UInt32  dwReserved = 0;
				public UInt32 dwStringCount;
				public string[] Text;
			}

			[StructLayout(LayoutKind.Sequential)]
				public class WKS_RECORD
			{
				public UInt32  Next = 0;
				public string  Name = null;
				public ushort  Type = 0;
				public short  wDataLength = 0;
				public UInt32  Flags = 0;
				//public RecordFlags Flags;
				public UInt32  dwTtl = 0;
				public UInt32  dwReserved = 0;
				public UInt32 ip4addr;
				public char chProtocol;
			}

			[StructLayout(LayoutKind.Sequential)]
				public class PTR_RECORD
			{
				public UInt32  Next = 0;
				public string  Name = null;
				public ushort  Type = 0;
				public short  wDataLength = 0;
				public UInt32  Flags = 0;
				//public RecordFlags Flags;
				public UInt32  dwTtl = 0;
				public UInt32  dwReserved = 0;
				public string pNameHost;
			}
			[StructLayout(LayoutKind.Sequential)]
				public class A_RECORD
			{
				public UInt32  Next = 0;
				public string  Name = null;
				public ushort  Type = 0;
				public short  wDataLength = 0;
				public UInt32  Flags = 0;
				//public RecordFlags Flags;
				public UInt32  dwTtl = 0;
				public UInt32  dwReserved = 0;
				public UInt32 ip4addr;
			}

			[StructLayout(LayoutKind.Sequential)]
				public class MINFO_RECORD
			{
				public UInt32  Next = 0;
				public string  Name = null;
				public ushort  Type = 0;
				public short  wDataLength = 0;
				public UInt32  Flags = 0;
				//public RecordFlags Flags;
				public UInt32  dwTtl = 0;
				public UInt32  dwReserved = 0;
				public string pNameMailbox = null;
				public string pNameErrorsMailbox = null;
			}

		}

		/// <summary>
		/// The base DNS record.
		/// </summary>
		public class DNSRecord 
		{
			public string DomainName = null;
			public ushort Type = 0;
			public int Flags = 0;
			public int TimeToLive = 0;
		}

		/// <summary>
		/// A mail exchanger record.
		/// </summary>
		public class MXRecord : DNSRecord
		{
			public string MailExchanger;
			public int Preference;

			public MXRecord(Internal.MX_RECORD s) 
			{
				this.Preference = (int)s.Preference;
				this.MailExchanger = s.MailExchanger;
				this.DomainName = s.Name;
				this.Flags = (int)s.Flags;
				this.TimeToLive = (int)s.dwTtl;
				this.Type = s.Type;
			}

			public override string ToString() 
			{
				return "MX record (" + this.DomainName + ") : " + this.MailExchanger + ", priority: " + this.Preference.ToString();
			}
		}

		/// <summary>
		/// An address record.
		/// </summary>
		public class ARecord : DNSRecord
		{
			System.Net.IPAddress Address;

			public ARecord(Internal.A_RECORD s) 
			{
				Address = new IPAddress((long)s.ip4addr);
				this.DomainName = s.Name;
				this.Flags = (int)s.Flags;
				this.TimeToLive = (int)s.dwTtl;
				this.Type = s.Type;
			}

			public override string ToString() 
			{
				return "A record (" + this.DomainName + ") : " + Address.ToString();
			}
		}

		/// <summary>
		/// A mail info record.
		/// </summary>
		public class MINFORecord : DNSRecord
		{
			string PrimaryMailBox = null;
			string ErrorMailBox = null;

			public MINFORecord(Internal.MINFO_RECORD s) 
			{
				this.PrimaryMailBox = s.pNameMailbox;
				this.ErrorMailBox = s.pNameErrorsMailbox;
				this.DomainName = s.Name;
				this.Flags = (int)s.Flags;
				this.TimeToLive = (int)s.dwTtl;
				this.Type = s.Type;
			}

			public override string ToString() 
			{
				return "MINFO record (" + this.DomainName + ") : mail box = " + this.PrimaryMailBox + ", error mail box = " + this.ErrorMailBox;
			}
		}

		/// <summary>
		/// A text record.
		/// </summary>
		public class TXTRecord : DNSRecord
		{
			public string[] Text;
			public int StringCount;

			public TXTRecord(Internal.TXT_RECORD s) 
			{
				this.StringCount = (int)s.dwStringCount;
				this.Text = s.Text;
				this.DomainName = s.Name;
				this.Flags = (int)s.Flags;
				this.TimeToLive = (int)s.dwTtl;
				this.Type = s.Type;
			}

			public override string ToString() 
			{
				return "TXT record: " + this.Text;
			}
		}

		/// <summary>
		/// A pointer record.
		/// </summary>
		public class PTRRecord : DNSRecord
		{
			public string HostName;

			public PTRRecord(Internal.PTR_RECORD s) 
			{
				this.HostName = s.pNameHost;
				this.DomainName = s.Name;
				this.Flags = (int)s.Flags;
				this.TimeToLive = (int)s.dwTtl;
				this.Type = s.Type;
			}

			public override string ToString() 
			{
				return "PTR record: " + this.HostName + ", for type = " + Resolver.ValueToQueryType(this.Type);
			}
		}

		/// <summary>
		/// A service record.
		/// </summary>
		public class SRVRecord : DNSRecord
		{
			public string Target;
			public int Priority;
			public int Weight;
			public int Port;

			public SRVRecord(Internal.SRV_RECORD s) 
			{
				this.Port = (int)s.wPort;
				this.Priority = (int)s.wPriority;
				this.Target = s.pNameTarget;
				this.Weight = (int)s.wWeight;
				this.DomainName = s.Name;
				this.Flags = (int)s.Flags;
				this.TimeToLive = (int)s.dwTtl;
				this.Type = s.Type;
			}

			public override string ToString() 
			{
				return "SRV record (" + this.DomainName + ") : Port = " + this.Port.ToString() + ", Priority = " + this.Priority.ToString() +
					", Target = " + this.Target + ", Weight = " + this.Weight.ToString();

			}

		}

		/// <summary>
		/// A Start of Authority record. 
		/// </summary>
		public class SOARecord : DNSRecord
		{
			public string PrimaryNameServer;
			public string NameAdministrator;
			public int SerialNumber;
			public int Refresh;
			public int Retry;
			public int Expire;
			public int DefaultTTL;

			public SOARecord(Internal.SOA_RECORD s) 
			{
				this.PrimaryNameServer = s.pNamePrimaryServer;
				this.NameAdministrator = s.pNameAdministrator;
				this.SerialNumber = (int)s.dwSerialNo;
				this.Refresh = (int)s.dwRefresh;
				this.Retry = (int)s.dwRetry;
				this.Expire = (int)s.dwExpire;
				this.DomainName = s.Name;
				this.Flags = (int)s.Flags;
				this.TimeToLive = (int)s.dwTtl;
				this.Type = s.Type;
			}

			public override string ToString() 
			{
				return "SOA record (" + this.DomainName + ") : Primary Name Server = "+ this.PrimaryNameServer + ", Name Administrator = " + this.NameAdministrator +
					", Serial Number = " + this.SerialNumber.ToString() + " Refresh = " + this.Refresh.ToString() + " Retry = " + this.Retry.ToString() + 
					", Expire = " + this.Expire.ToString();
			}

		}


		[DllImport("dnsapi.dll",EntryPoint="DnsQuery_A")]
		private static extern UInt32 Query(
			string lpstrName,
			UInt16 wType,
			UInt32 fOptions,
			IntPtr servers,
			out IntPtr ppQueryResultsSet,
			UInt32 t3 );

		[DllImport("dnsapi.dll",EntryPoint="DnsRecordListFree")]
		private static extern void DnsRecordListFree(
			IntPtr ppQueryResultsSet,
			Int32 FreeType
			);

		/// <summary>
		/// Queries a specific dns server.
		/// </summary>
		/// <param name="query">The question</param>
		/// <param name="querytype">The type of query</param>
		/// <param name="server">The server to ask</param>
		/// <returns>Array of records</returns>
		public static DNSRecord[] Query(string query, UInt16 querytype, string server) 
		{
			ArrayList ret = null;
			UInt32 dns_status = 0;
			IntPtr ppQueryResultsSetOrig = IntPtr.Zero, ppQueryResultsSet = IntPtr.Zero;
			
			IntPtr pSrvList = IntPtr.Zero;
			UInt32 otheropts = 0;

			if (server != null && server.Length > 0) 
			{
				Internal.DNS_SERVER_LIST srvLst = new Internal.DNS_SERVER_LIST();
				srvLst.addrcount = 1;
			
				System.Net.IPAddress ip = System.Net.Dns.GetHostByName(server).AddressList[0];
				srvLst.ip4addr = (UInt32)ip.Address;
				pSrvList = Marshal.AllocHGlobal(0);
				Marshal.StructureToPtr(srvLst,pSrvList,true);
				otheropts |= Resolver.QueryTypes.DNS_QUERY_BYPASS_CACHE;
			}

			dns_status = Query(query, querytype, Resolver.QueryTypes.DNS_QUERY_STANDARD | otheropts ,
				pSrvList, out ppQueryResultsSet, (UInt32)0 );

			ppQueryResultsSetOrig = ppQueryResultsSet;

			if (pSrvList != IntPtr.Zero)
				Marshal.FreeHGlobal(pSrvList);

			while (dns_status == 0 && ppQueryResultsSet.ToInt32() > 0) 
			{
				Internal.DNS_RECORD QueryResultsSet = (Internal.DNS_RECORD)Marshal.PtrToStructure(
					ppQueryResultsSet, typeof(Internal.DNS_RECORD));
				
				DNSRecord record = null;
				switch (QueryResultsSet.Type )
				{
						//HINFO, Hinfo, ISDN, Isdn, TXT, Txt, X25;
					case RecordTypes.DNS_TYPE_A:
						record = new ARecord((Internal.A_RECORD)Marshal.PtrToStructure(ppQueryResultsSet, typeof(Internal.A_RECORD)));
						break;
					case RecordTypes.DNS_TYPE_SOA:
						record = new SOARecord((Internal.SOA_RECORD)Marshal.PtrToStructure(ppQueryResultsSet, typeof(Internal.SOA_RECORD)));
						break;
					case RecordTypes.DNS_TYPE_CNAME:
						goto case RecordTypes.DNS_TYPE_PTR;
					case RecordTypes.DNS_TYPE_MB:
						goto case RecordTypes.DNS_TYPE_PTR;
					case RecordTypes.DNS_TYPE_MD:
						goto case RecordTypes.DNS_TYPE_PTR;
					case RecordTypes.DNS_TYPE_MF:
						goto case RecordTypes.DNS_TYPE_PTR;
					case RecordTypes.DNS_TYPE_MG:
						goto case RecordTypes.DNS_TYPE_PTR;
					case RecordTypes.DNS_TYPE_MR:
						goto case RecordTypes.DNS_TYPE_PTR;
					case RecordTypes.DNS_TYPE_NS:
						goto case RecordTypes.DNS_TYPE_PTR;
					case RecordTypes.DNS_TYPE_PTR:
						record = new PTRRecord((Internal.PTR_RECORD)Marshal.PtrToStructure(ppQueryResultsSet, typeof(Internal.PTR_RECORD)));
						break;
					case RecordTypes.DNS_TYPE_HINFO:
						goto case RecordTypes.DNS_TYPE_TEXT;
					case RecordTypes.DNS_TYPE_MINFO:
						record = new MINFORecord((Internal.MINFO_RECORD)Marshal.PtrToStructure(ppQueryResultsSet, typeof(Internal.MINFO_RECORD)));
						break;
					case RecordTypes.DNS_TYPE_SRV:
						record = new SRVRecord((Internal.SRV_RECORD)Marshal.PtrToStructure(ppQueryResultsSet, typeof(Internal.SRV_RECORD)));
						break;
					case RecordTypes.DNS_TYPE_TEXT:
						record = new TXTRecord((Internal.TXT_RECORD)Marshal.PtrToStructure(ppQueryResultsSet, typeof(Internal.TXT_RECORD)));
						break;
					case RecordTypes.DNS_TYPE_MX:
						record = new MXRecord((Internal.MX_RECORD)Marshal.PtrToStructure(ppQueryResultsSet, typeof(Internal.MX_RECORD)));
						break;
				}
				if (record != null) 
				{
					if (ret == null) ret = new ArrayList();
					ret.Add(record);
				}
				ppQueryResultsSet = (IntPtr)QueryResultsSet.Next;
			}

			if (ppQueryResultsSetOrig != IntPtr.Zero)
				DnsRecordListFree(ppQueryResultsSetOrig, 1);

			if (ret != null && ret.Count > 0)
				return (DNSRecord[])ret.ToArray(typeof(DNSRecord));
			else
				return null;
		}

		/// <summary>
		/// Queries the default DNS servers.
		/// </summary>
		/// <param name="query">The question</param>
		/// <param name="querytype">Type of records to get</param>
		/// <returns>Array of DNS records</returns>
		public static DNSRecord[] Query(string query, UInt16 querytype) 
		{
			return Query(query,querytype,"");
		}

	}
}

