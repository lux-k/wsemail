using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Text;

namespace AdapterInfo
{
	/// <summary>
	/// Test Form class
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		#region Private Data
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader hdrName;
		private System.Windows.Forms.ColumnHeader hdrIP;
		private System.Windows.Forms.ColumnHeader hdrNetMask;
		private System.Windows.Forms.ColumnHeader hdrGW;
		private System.Windows.Forms.MainMenu mainMenu1;
		#endregion

		#region Constructors/Destructors
		public Form1()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.listView1 = new System.Windows.Forms.ListView();
			this.hdrName = new System.Windows.Forms.ColumnHeader();
			this.hdrIP = new System.Windows.Forms.ColumnHeader();
			this.hdrNetMask = new System.Windows.Forms.ColumnHeader();
			this.hdrGW = new System.Windows.Forms.ColumnHeader();
			// 
			// listView1
			// 
			this.listView1.Columns.Add(this.hdrName);
			this.listView1.Columns.Add(this.hdrIP);
			this.listView1.Columns.Add(this.hdrNetMask);
			this.listView1.Columns.Add(this.hdrGW);
			this.listView1.Size = new System.Drawing.Size(240, 208);
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// hdrName
			// 
			this.hdrName.Text = "Name";
			this.hdrName.Width = 60;
			// 
			// hdrIP
			// 
			this.hdrIP.Text = "IP";
			this.hdrIP.Width = 60;
			// 
			// hdrNetMask
			// 
			this.hdrNetMask.Text = "Mask";
			this.hdrNetMask.Width = 60;
			// 
			// hdrGW
			// 
			this.hdrGW.Text = "Def GW";
			this.hdrGW.Width = 60;
			// 
			// Form1
			// 
			this.Controls.Add(this.listView1);
			this.Menu = this.mainMenu1;
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>

		static void Main() 
		{
			Application.Run(new Form1());
		}


		public static AdapterInfo.IP_ADAPTER_INFO getInfo()
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


		private void Form1_Load(object sender, System.EventArgs e)
		{
			int cb = 0;
			int ret = GetAdaptersInfo(IntPtr.Zero, ref cb);
			IntPtr pInfo = LocalAlloc(0x40, cb); //LPTR
			ret = GetAdaptersInfo(pInfo, ref cb);
			if ( ret == 0 )
			{
				IP_ADAPTER_INFO info = new IP_ADAPTER_INFO(pInfo, 0);
				while ( info != null )
				{
					listView1.Items.Add(new ListViewItem(new string[] { info.AdapterName, info.CurrentIpAddress.IpAddress.String, info.CurrentIpAddress.IpMask.String, info.GatewayList.IpAddress.String } ) );
					info = info.Next;
				}
			}
			LocalFree(pInfo);
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
		public IP_ADDRESS_STRING IpAddress
		{
			get { return new IP_ADDRESS_STRING(data, 4); }

		}
		public IP_ADDRESS_STRING IpMask
		{
			get { return new IP_ADDRESS_STRING(data, 20); }
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
