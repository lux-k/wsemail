using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace GetGateway
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class GatewayGetter
	{
		public GatewayGetter()
		{
		}

		[DllImport("iphlpapi.dll", ExactSpelling=true)]
		public static extern int GetAdaptersInfo( IntPtr buffer, ref int SrcIP );

		[StructLayoutAttribute(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
		public struct IPAdapterInfo
		{
			public IntPtr  Next;
			public Int32  ComboIndex;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=260)]
			public String  AdapterName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=132)]
			public String  Description;

			public Int32  AddressLength;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst=8)]
			public Byte[]  Address;

			public Int32  Index;
			public Int32  Type;
			public Int32  DhcpEnabled;

			public IntPtr  CurrentIPAddress;
			public IPAddrString IPAddressList;
			public IPAddrString GatewayList;
			public IPAddrString DhcpServer;
			public Boolean  HaveWins;
			public IPAddrString PrimaryWinsServer;
			public IPAddrString SecondaryWinsServer;

			public Int32  LeaseObtained;
			public Int32  LeaseExpires;
		}

		[StructLayoutAttribute(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
		public struct IPAddrString
		{
			public IntPtr  Next;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=16)]
			public String  IpAddress;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=16)]
			public String  IpMask;
			public Int32  Context;
		}


		public static string[] Get() 
		{
			ArrayList gateways = new ArrayList();
			Type infoTyp = typeof( IPAdapterInfo );
			int infoSize = Marshal.SizeOf( infoTyp );

			int size = 0;
			int r = GetAdaptersInfo( IntPtr.Zero, ref size );
			if( (r != 111) || (size < infoSize) )  // 111 : ERROR_BUFFER_OVERFLOW
				return new string[] {};
			size += 8000;
			IntPtr buf = Marshal.AllocHGlobal( size );
			r = GetAdaptersInfo( buf, ref size );
			if( r != 0 )   // ERROR_SUCCESS
				return new string[] {};       // add error handling, and free 'buf'!

			IntPtr run = buf;
			IPAdapterInfo iai;
			do
			{
				iai = (IPAdapterInfo) Marshal.PtrToStructure( run, infoTyp );
				// string mac = "?";
//				if( (iai.AddressLength > 0) && (iai.AddressLength <= 8) )
//					mac = BitConverter.ToString( iai.Address, 0, iai.AddressLength );
				// Console.WriteLine( "Adapter: {0}\n\t   MAC:{1}", iai.Description, mac );
				gateways.Add(iai.GatewayList.IpAddress);
//				ret = iai.GatewayList.IpAddress;
				run = iai.Next;
			}
			while( run != IntPtr.Zero );

			Marshal.FreeHGlobal( buf ); buf = IntPtr.Zero;
			return (string[])gateways.ToArray(typeof(string));
		}
	}
}
