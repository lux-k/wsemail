/*===========================================================================

    OpenNETCF.Notification.NotificationEngine
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
using System.Collections;
using System.Runtime.InteropServices;

namespace OpenNETCF.Notification
{
	/// <summary>
	/// Used to create Notification messages
	/// </summary>
	public class NotificationEngine
	{
		#if!NDOC
		//hwnd of notification handling message window
		private NotificationMessageWindow m_msgwnd;
		#endif

		//unique identifier of the notification type
		private Guid m_clsid;
		
		//store collection of active notifications
		private static int s_counter = 0;

		//handle to icon;
		internal IntPtr m_hIcon = IntPtr.Zero;

		/// <summary>
		/// Create a new instance of the NotificationEngine class
		/// </summary>
		/// <param name="uniqueIdentifier">GUID unique to your application.</param>
		public NotificationEngine(Guid uniqueIdentifier)
		{
			//set unique clsid
			m_clsid = uniqueIdentifier;

			#if!NDOC
			//create message window to receive events
			m_msgwnd = new NotificationMessageWindow(this);
			#endif

			//grab the icon from the calling EXE
			ExtractIconEx(System.Reflection.Assembly.GetCallingAssembly().GetModules()[0].FullyQualifiedName, 0, 0, ref m_hIcon, 1);
		}

		#region Add
		/// <summary>
		/// Add the Notification to the tray
		/// </summary>
		public int Add(Notification notification)
		{
			//add app specified clsid
			notification.Clsid = m_clsid;

			#if!NDOC
			//add hwnd of messagewindow
			notification.Hwnd = m_msgwnd.Hwnd;
			#endif

			//add icon handle
			notification.Hicon = m_hIcon;

			//assign a unique id in the collection
			notification.ID = s_counter;
			s_counter ++;

			//add to the collection

			int lResult = SHNotificationAdd(notification);

			return notification.ID;
		}
		#endregion

		#region Remove
		/// <summary>
		/// Remove the Notification from the tray
		/// </summary>
		public void Remove(int ID)
		{
			//remove from system tray
			int lResult = SHNotificationRemove(ref m_clsid, (uint)ID);
		}
		#endregion

		#region GetData
		/// <summary>
		/// Return data for a specific notification
		/// </summary>
		/// <param name="ID">Identifier of the Notification to return</param>
		/// <returns>Notification object describing the notification with the specified ID</returns>
		public Notification GetData(int ID)
		{
			Notification notifydata = new Notification();

			SHNotificationGetData(ref m_clsid, (uint)ID, notifydata);

			
			//Console.WriteLine(notifydata.ID.ToString() + " " + notifydata.Flags.ToString() + " " + notifydata.Title);
			
			return notifydata;
		}
		#endregion

		#region Update
		/// <summary>
		/// Update an existing NotificationBubble
		/// </summary>
		/// <remarks>Use GetData to retrieve the current parameters of a notification.</remarks>
		/// <param name="mask">Combination of UpdateMask members indicating what fields have been updated.</param>
		/// <param name="notification">New Notification data. ID must match an active Notification.</param>
		public void Update(UpdateMask mask, Notification notification)
		{
			//add app specified clsid
			notification.Clsid = m_clsid;

			#if!NDOC
			//add hwnd of messagewindow
			notification.Hwnd = m_msgwnd.Hwnd;
			#endif
			
			int result = SHNotificationUpdate((uint)mask, notification);

		}
		#endregion


		#region Events

		#region Show
		/// <summary>
		/// Occurs when a notification is shown.
		/// </summary>
		public event NotificationEventHandler NotificationShow;

		internal void OnNotificationShow(NotificationEventArgs e)
		{
			if (NotificationShow != null)
				NotificationShow(this, e);
		}
		#endregion

		#region Select
		/// <summary>
		/// Occurs when a link is selected in a notification.
		/// </summary>
		public event NotificationEventHandler NotificationSelect;

		internal void OnNotificationSelect(NotificationEventArgs e)
		{
			if (NotificationSelect != null)
				NotificationSelect(this, e);
		}
		#endregion

		#region Dismiss
		/// <summary>
		/// Occurs when a notification is dismissed.
		/// </summary>
		public event NotificationEventHandler NotificationDismiss;

		internal void OnNotificationDismiss(NotificationEventArgs e)
		{
			if (NotificationDismiss != null)
				NotificationDismiss(this, e);
		}
		#endregion

		#endregion

		#region API Declares

		//Add
		[DllImport("aygshell.dll", EntryPoint="#155")]
		private static extern int SHNotificationAdd(Notification shinfo);

		//Remove
		[DllImport("aygshell.dll",EntryPoint="#157")]
		private static extern int SHNotificationRemove(ref Guid clsid, uint dwID);

		//Update
		[DllImport("aygshell.dll", EntryPoint="#156")]
		private static extern int SHNotificationUpdate(uint grnumUpdateMask, Notification shinfo);

		//Get Data
		[DllImport("aygshell.dll", EntryPoint="#173")]
		private static extern int SHNotificationGetData(ref Guid clsid, uint dwID, Notification shinfo);

		//icon
		[DllImport("coredll.dll")]
		private static extern IntPtr ExtractIconEx(string fileName, int index, int hIconLarge, ref IntPtr hIconSmall, uint nIcons);

		#endregion

	}
}
