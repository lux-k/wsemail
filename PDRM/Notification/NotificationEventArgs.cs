/*===========================================================================

    OpenNETCF.Notification.NotificationEventArgs
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
	/// Paramters passed when a Notification Bubble event occurs
	/// </summary>
	public class NotificationEventArgs
	{
		//window handle of source
		private IntPtr   hWndFrom;
		//id of notification
		private int		idFrom;
		//message type
		internal Actions  code;

		private int lParam;

		//link id e.g. cmd:10 -> 10
		private int m_link;

		//determines if dismiss was due to timeout
		private bool fTimeout = false;

		//co-ordinates of bubble point (only sent on Show)
		private System.Drawing.Point pt = new System.Drawing.Point(0, 0);

		#region Constructor
		/// <summary>
		/// Constructs a new NotificationEventArgs instance from unmanaged memory
		/// </summary>
		/// <param name="address">LParam pointer</param>
		internal NotificationEventArgs(IntPtr address)
		{
			hWndFrom = (IntPtr)Marshal.ReadInt32(address);
			idFrom = Marshal.ReadInt32(address, 4);
			code = (Actions)Marshal.ReadInt32(address, 8);

			lParam = Marshal.ReadInt32(address, 12);
			

			switch(code)
			{
				case Actions.LinkSelect:
					IntPtr stringptr = (IntPtr)Marshal.ReadInt32(address, 16);
					//pszLink = Marshal.PtrToStringUni(stringptr);
					break;
				case Actions.Dismiss:
					fTimeout = Marshal.ReadByte(address, 16) == 0 ? false : true;		
					break;
				case Actions.Show:
					pt = new System.Drawing.Point(Marshal.ReadInt16(address, 16), Marshal.ReadInt16(address, 18));
					break;
			}
		}
		internal NotificationEventArgs(int id, int link)
		{
			idFrom = id;
			m_link = link;
		}
		#endregion

		#region ID
		/// <summary>
		/// ID of Notification which caused event
		/// </summary>
		public int ID
		{
			get
			{
				return idFrom;
			}
		}
		#endregion

		#region Link
		/// <summary>
		/// Identifier of Link selected
		/// </summary>
		/// <remarks>You create links of the form "CMD:10" where the number is passed back when the link is clicked.</remarks>
		public int Link
		{
			get
			{
				return m_link;
			}
		}
		#endregion

		#region Timeout
		/// <summary>
		/// Indicates if Notification was dismissed due to a timeout
		/// </summary>
		public bool Timeout
		{
			get
			{
				return fTimeout;
			}
		}
		#endregion
	}
}
