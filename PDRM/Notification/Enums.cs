/*===========================================================================

    OpenNETCF.Notification.Enums
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
namespace OpenNETCF.Notification
{
	#region Priority
	/// <summary>
	/// Priority of the Notification Bubble
	/// </summary>
	public enum Priority :uint
	{
		/// <summary>
		/// Bubble shown for duration, then goes away
		/// </summary>
		Inform = 0x1B1,
		/// <summary>
		/// No bubble, icon shown for duration then goes away
		/// </summary>
		Iconic
	}
	#endregion

	#region Update Mask
	/// <summary>
	/// Flags used for updating Notifications in progress
	/// </summary>
	public enum UpdateMask : uint
	{
		/// <summary>
		/// Priority has changed
		/// </summary>
		Priority     = 0x0001,
		/// <summary>
		/// Duration has changed
		/// </summary>
		Duration     = 0x0002,
		/// <summary>
		/// Icon has changed
		/// </summary>
		Icon         = 0x0004,
		/// <summary>
		/// Html has changed
		/// </summary>
		Html         = 0x0008,
		/// <summary>
		/// Title has changed
		/// </summary>
		Title        = 0x0010
	}
	#endregion

	#region Flags
	/// <summary>
	/// Flags that affect the display behaviour of the Notification
	/// </summary>
	public enum NotificationFlags :uint
	{
		/// <summary>
		/// For SHNP_INFORM priority and above, don't display the notification bubble when it's initially added;
		/// the icon will display for the duration then it will go straight into the tray.
		/// The user can view the icon / see the bubble by opening the tray.
		/// </summary>
		StraightToTray  = 0x00000001,
		/// <summary>
		/// Critical information - highlights the border and title of the bubble.
		/// </summary>
		Critical        = 0x00000002,
		/// <summary>
		/// Force the message (bubble) to display even if settings says not to.
		/// </summary>
		ForceMessage    = 0x00000008
	}
	#endregion

	#region Actions
	internal enum Actions : int
	{
		LinkSelect = -1000,
		Dismiss = -1001,
		Show = -1002
	}
	#endregion
}
