/*===========================================================================

    OpenNETCF.Notification.NotificationMessageWindow
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
	#if !NDOC
	using Microsoft.WindowsCE.Forms;

	/// <summary>
	/// Handles messages received from the Notification system and throws events in the parent NotificationEngine object
	/// </summary>
	internal class NotificationMessageWindow : MessageWindow
	{
		//parent NotificationEngine object
		NotificationEngine m_parent;

		public NotificationMessageWindow(NotificationEngine parent)
		{
			m_parent = parent;
		}

		protected override void WndProc(ref Message m)
		{
			NotificationEventArgs nm;

			switch(m.Msg)
			{
					//WM_NOTIFY
				case 78:
					nm = new NotificationEventArgs(m.LParam);
				switch(nm.code)
				{
					case Actions.Dismiss:
						m_parent.OnNotificationDismiss(nm);
						break;
					case Actions.Show:
						m_parent.OnNotificationShow(nm);
						break;
				}
					break;
					//WM_COMMAND
				case 0x0111:
					nm = new NotificationEventArgs((int)m.LParam, (int)m.WParam);
					m_parent.OnNotificationSelect(nm);
					//Console.WriteLine(m.LParam.ToString() + " " + m.WParam.ToString());
					break;
			}
			
			//do base wndproc
			base.WndProc (ref m);
		}

	}
	#endif

	#region NotificationEventHandler
	/// <summary>
	/// Represents the method that will handle the event raised by a Notification action.
	/// </summary>
	public delegate void NotificationEventHandler(object sender, NotificationEventArgs e);

	#endregion
}


