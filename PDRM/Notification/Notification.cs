/*===========================================================================

    OpenNETCF.Notification.Notification
    Copyright (C)  2003, OpenNETCF.org

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 
    If you wish you contact the OpenNETCF Advisory Board to discuss licensing,
    please email licensing@opennetcf.org. 

    For general enquiries, email enquiries@opennetcf.org or visit our website
    at http://www.opennetcf.org
  ===========================================================================*/
using System;
using System.Runtime.InteropServices;

namespace OpenNETCF.Notification
{
	/// <summary>
	/// An individual Notification Bubble instance
	/// </summary>
	public class Notification : IDisposable
	{
		// for verification and versioning
		private uint        cbStruct;
		// identifier for this particular notification
		private uint        dwID;
		// priority
		private Priority    npPriority;
		// duration of the notification (usage depends on priority)
		private uint        csDuration;
		// the icon for the notification
		private IntPtr			hicon;
		// flags
		private NotificationFlags       grfFlags;
		// unique identifier for the notification class
		private Guid				clsid;
		// window to receive command choices, dismiss, etc.
		private IntPtr			hwndSink;
		// HTML content for the bubble
		private IntPtr			pszHTML;
		// Optional title for bubble
		private IntPtr			pszTitle;
		// User-defined parameter
		private uint				lParam;

		public Notification()
		{
			cbStruct = (uint)Marshal.SizeOf(this);
			dwID = 0;
			npPriority = Priority.Inform;
			csDuration = 20;
		}
		~Notification()
		{
			this.Dispose();
		}

		#region Internal
		internal Guid Clsid
		{
			set
			{
				clsid = value;
			}
		}
		internal IntPtr Hwnd
		{
			set
			{
				hwndSink = value;
			}
		}
		internal IntPtr Hicon
		{
			set
			{
				hicon = value;
			}
		}
		#endregion

		#region Properties

		/// <summary>
		/// Unique ID of the Notification instance
		/// </summary>
		public int ID
		{
			get
			{
				return (int)dwID;
			}
			set
			{
				dwID = (uint)value;
			}
		}

		public Priority Priority
		{
			get
			{
				return npPriority;
			}
			set
			{
				npPriority = value;
			}
		}
		/// <summary>
		/// Title to display in Notification bubble
		/// </summary>
		public string Title
		{
			get
			{
				return Marshal.PtrToStringUni(pszTitle);
			}
			set
			{
				//if existing string data
				FreePtr(pszTitle);

				//marshal title
				pszTitle = StringToPtr(value);
			}
		}

		/// <summary>
		/// HTML Text to display within the bubble
		/// </summary>
		public string HTML
		{
			get
			{
				return Marshal.PtrToStringUni(pszHTML);
			}
			set
			{
				//if existing string data free memory
				FreePtr(pszHTML);

				//marshal new value
				pszHTML = StringToPtr(value);
			}
		}

		/// <summary>
		/// Flags that affect the behaviour of the Notification bubble
		/// </summary>
		public NotificationFlags Flags
		{
			get
			{
				return Flags;
			}
			set
			{
				grfFlags = value;
			}
		}

		/// <summary>
		/// Duration to display the bubble (in seconds).
		/// </summary>
		public uint Duration
		{
			get
			{
				return csDuration;
			}
			set
			{
				csDuration = value;
			}
		}

		/// <summary>
		/// User defined parameter
		/// </summary>
		public uint LParam
		{
			get
			{
				return lParam;
			}
			set
			{
				lParam = value;
			}
		}
		#endregion

		#region String Ptr Functions
		private IntPtr StringToPtr(string text)
		{
			//determine length
			int length = text.Length * Marshal.SystemDefaultCharSize;

			//allocate sufficient unmanaged memory
			IntPtr ptr = LocalAlloc(0x40, (uint)(length + Marshal.SystemDefaultCharSize));
				
			//marshal data
			for(int iCount = 0; iCount < length; iCount = iCount + Marshal.SystemDefaultCharSize)
			{
				Marshal.WriteInt16(ptr, iCount, (short)text[iCount/Marshal.SystemDefaultCharSize]);
			}

			return ptr;
		}
		private void FreePtr(IntPtr stringPtr)
		{
			if(stringPtr!=IntPtr.Zero)
			{
				LocalFree(stringPtr);
				stringPtr = IntPtr.Zero;
			}
		}
		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			//free pointer to body text
			FreePtr(pszHTML);
			//free pointer to title text
			FreePtr(pszTitle);
		}

		#endregion

		#region API P/Invokes

		//Allocate Unmanaged Memory
		[DllImport("coredll.dll")]
		private static extern IntPtr LocalAlloc(uint uFlags, uint uBytes ); 

		//Free Unmanaged Memory
		[DllImport("coredll.dll")]
		private static extern IntPtr LocalFree(IntPtr hMem);

		#endregion
	}
}
 

 