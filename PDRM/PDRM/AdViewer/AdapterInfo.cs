using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Text;

namespace AdViewer
{
	/// <summary>
	/// Class that will fetch the adapter information about a Pocket PC
	/// device.
	/// Check out http://groups.google.com/groups?hl=en&lr=&ie=UTF-8&oe=UTF-8&threadm=eoe4jj4CDHA.1548%40TK2MSFTNGP12.phx.gbl&rnum=1&prev=/groups%3Fq%3DAdapterInfo%2Bgroup:microsoft.public.*%2Bauthor:Feinman%26hl%3Den%26lr%3D%26ie%3DUTF-8%26oe%3DUTF-8%26selm%3Deoe4jj4CDHA.1548%2540TK2MSFTNGP12.phx.gbl%26rnum%3D1
	/// for more information.
	/// </summary>
	public class AdapterInfo
	{
		public AdapterInfo()
		{
			// empty class
		}

		/// <summary>
		/// This method returns an IP_ADAPTER_INFO object for analysis.  The object
		/// contains information about the particular network adapters available from
		/// the device.
		/// </summary>
		/// <returns>An IP_ADAPTER_INFO object that describes the available network
		/// adapters.  null if there is a failure.</returns>
		public static IP_ADAPTER_INFO GetInfo()
		{
			// size of the buffer returned
			int cb = 0;
			// first try it with a null pointer
			int ret = GetAdaptersInfo(IntPtr.Zero, ref cb);
			IntPtr pInfo = LocalAlloc(0x40, cb); //LPTR
			// now for real
			ret = GetAdaptersInfo(pInfo, ref cb);
			if ( ret == 0 )
			{
				// if success
				IP_ADAPTER_INFO info = new IP_ADAPTER_INFO(pInfo, 0);
				return info;
			}
			else
			{
				// failure
				return null;
			}
		}

		#region P/Invoke definitions
		[DllImport("iphlpapi")]
		extern public static int GetAdaptersInfo(IntPtr p, ref int cb);
		[DllImport("coredll")]
		extern public static IntPtr LocalAlloc(int flags, int cb);
		[DllImport("coredll")]
		extern public static IntPtr LocalFree(IntPtr p);
		#endregion
	}

	/// <summary>
	/// Class IP_ADAPTER_INFO 
	/// Description:
	/// Implementation of custom marshaller for IPHLPAPI IP_ADAPTER_INFO
	/// </summary>
	public class IP_ADAPTER_INFO: SelfMarshalledStruct
	{
		#region Constructors
		public IP_ADAPTER_INFO():base(640)
		{
		}
		public IP_ADAPTER_INFO(byte[] data, int offset):base(data, offset)
		{
		}


		public IP_ADAPTER_INFO(IntPtr pData, int offset):base(640)
		{
			Marshal.Copy(new IntPtr( pData.ToInt32() + offset ), data, 0, 640);
		}
		#endregion

		#region Properties
		public IP_ADAPTER_INFO Next
		{
			get 
			{ 
				if ( this.GetInt32(0) == 0 ) 
					return null;
				return new IP_ADAPTER_INFO(new IntPtr(this.GetInt32(0)), 0);
			}
		}
		public uint ComboIndex
		{
			get { return GetUInt32( 4 ); }
			set { Set( typeof(uint), 4, value ); }
		}
		public string AdapterName
		{
			get { return GetStringAscii( 8, 268-8); }
			set { Set( typeof(string), 8, value ); }
		}
		public string Description
		{
			get { return GetStringAscii( 268, 400-268 ); }
			set { Set( typeof(string), 268, value ); }
		}
		public uint AddressLength
		{
			get { return GetUInt32( 400 ); }
			set { Set( typeof(uint), 400, value ); }
		}
		public byte[] Address
		{
			get { return GetSlice( 404, (int)AddressLength ); }
		}
		public uint Index
		{
			get { return GetUInt32( 412 ); }
			set { Set( typeof(uint), 412, value ); }
		}
		public uint Type
		{
			get { return GetUInt32( 416 ); }
			set { Set( typeof(uint), 416, value ); }
		}
		public uint DhcpEnabled
		{
			get { return GetUInt32( 420 ); }
			set { Set( typeof(uint), 420, value ); }
		}
		public IP_ADDR_STRING CurrentIpAddress
		{
			get 
			{ 
				return new IP_ADDR_STRING(new IntPtr( GetInt32(424)), 0 );
			}
		}
		public IP_ADDR_STRING IpAddressList
		{
			get 
			{ 
				return new IP_ADDR_STRING(data, 428);
			}
		}
		public IP_ADDR_STRING GatewayList
		{
			get 
			{ 
				return new IP_ADDR_STRING(data, 468);
			}
		}
		public IP_ADDR_STRING DhcpServer
		{
			get 
			{ 
				return new IP_ADDR_STRING(data, 508);
			}
		}
		public int HaveWins
		{
			get { return GetInt32( 548 ); }
			set { Set( typeof(int), 548, value ); }
		}
		public IP_ADDR_STRING PrimaryWinsServer
		{
			get 
			{ 
				return new IP_ADDR_STRING(data, 552);
			}
		}
		public IP_ADDR_STRING SecondaryWinsServer
		{
			get 
			{ 
				return new IP_ADDR_STRING(data, 592);
			}
		}
		public uint LeaseObtained
		{
			get { return GetUInt32( 632 ); }
			set { Set( typeof(uint), 632, value ); }
		}
		public uint LeaseExpires
		{
			get { return GetUInt32( 636 ); }
			set { Set( typeof(uint), 636, value ); }
		}
		#endregion
	}

	/// <summary>
	/// Class IP_ADDR_STRING 
	/// Description:
	/// Implementation of custom marshaller for IPHLPAPI IP_ADDR_STRING
	/// </summary>
	public class IP_ADDR_STRING: SelfMarshalledStruct
	{
		#region Contructors
		public IP_ADDR_STRING():base(40)
		{
		}
		public IP_ADDR_STRING(byte[] data, int offset):base(data, offset)
		{
		}
		public IP_ADDR_STRING(IntPtr pData, int offset):base(40)
		{
			Marshal.Copy(new IntPtr( pData.ToInt32() + offset ), data, 0, 40);
		}
		#endregion

		#region Properties
		public IP_ADDR_STRING Next
		{
			get 
			{ 
				if ( this.GetInt32(0) == 0 ) 
					return null;
				return new IP_ADDR_STRING(new IntPtr( GetInt32(0) ), 0); 
			}
		}
		// changed as per http://groups.google.com/groups?hl=en&lr=&ie=UTF-8&oe=UTF-8&threadm=eoe4jj4CDHA.1548%40TK2MSFTNGP12.phx.gbl&rnum=1&prev=/groups%3Fq%3DAdapterInfo%2Bgroup:microsoft.public.*%2Bauthor:Feinman%26hl%3Den%26lr%3D%26ie%3DUTF-8%26oe%3DUTF-8%26selm%3Deoe4jj4CDHA.1548%2540TK2MSFTNGP12.phx.gbl%26rnum%3D1
		public IP_ADDRESS_STRING IpAddress
		{
			get { return new IP_ADDRESS_STRING(data, baseOffset+4); }

		}
		// changed as per http://groups.google.com/groups?hl=en&lr=&ie=UTF-8&oe=UTF-8&threadm=eoe4jj4CDHA.1548%40TK2MSFTNGP12.phx.gbl&rnum=1&prev=/groups%3Fq%3DAdapterInfo%2Bgroup:microsoft.public.*%2Bauthor:Feinman%26hl%3Den%26lr%3D%26ie%3DUTF-8%26oe%3DUTF-8%26selm%3Deoe4jj4CDHA.1548%2540TK2MSFTNGP12.phx.gbl%26rnum%3D1
		public IP_ADDRESS_STRING IpMask
		{
			get { return new IP_ADDRESS_STRING(data, baseOffset+20); }
		}
		public uint Context
		{
			get { return GetUInt32( 36 ); }
			set { Set( typeof(uint), 36, value ); }
		}
		#endregion
	}

	/// <summary>
	/// Class IP_ADDRESS_STRING 
	/// Description:
	/// Implementation of custom marshaller for IPHLPAPI IP_ADDRESS_STRING
	/// </summary>
	public class IP_ADDRESS_STRING: SelfMarshalledStruct
	{
		#region Contructors
		public IP_ADDRESS_STRING():base(16)
		{
		}
		public IP_ADDRESS_STRING(byte[] data, int offset):base(data, offset)
		{
		}
		public IP_ADDRESS_STRING(IntPtr pData, int offset):base(16)
		{
			Marshal.Copy(new IntPtr( pData.ToInt32() + offset ), data, 0, 16);
		}
		#endregion

		#region Properties
		public string String
		{
			get { return GetStringAscii( 0, 15 ); }
			set { Set( typeof(string), 0, value ); }
		}
		#endregion
	}


}
