/*===========================================================================

    OpenNETCF.Notification.LED
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
	/// Represents the collection of Notification LEDs on the device
	/// </summary>
	/// <remarks>Support varies depending on the device but all devices should include at least 1 notification LED</remarks>
	public class LED 
	{
		private const int NLED_COUNT_INFO_ID = 0;
		private const int NLED_SUPPORTS_INFO_ID	= 1;

		private int m_count;

		/// <summary>
		/// Initialise the LED collection
		/// </summary>
		public LED()
		{
			NLED_COUNT_INFO CountStruct = new NLED_COUNT_INFO(); 
			if(!NLedGetDeviceCount(NLED_COUNT_INFO_ID,ref CountStruct))
			{
				throw new Exception("Error Initialising LED's");
			}

			m_count = (int)CountStruct.cLeds;
		}

		/// <summary>
		/// Set the state of the specified LED
		/// </summary>
		/// <param name="led">0 based index of the LED</param>
		/// <param name="newState">New state of the LED - see LedState enumeration</param>
		public void SetLedStatus(uint led, LedState newState)
		{
			NLED_SETTINGS_INFO nsi = new NLED_SETTINGS_INFO();

			nsi.LedNum = led;
			nsi.OffOnBlink = (int)newState;
			bool bSuccess = NLedSetDevice(2, ref nsi);
		}


		[ DllImport("coredll.dll", EntryPoint="NLedGetDeviceInfo") ]
		private extern static bool NLedGetDeviceCount(short nID, ref NLED_COUNT_INFO pOutput);

		[ DllImport("coredll.dll", EntryPoint="NLedGetDeviceInfo") ]
		private extern static bool NLedGetDeviceSupports(short nID, ref NLED_SUPPORTS_INFO pOutput);


		[ DllImport("coredll.dll", EntryPoint="NLedSetDevice") ]
		private extern static bool NLedSetDevice(short nID, ref NLED_SETTINGS_INFO pOutput);
	
		private struct NLED_COUNT_INFO
		{
			public uint cLeds;
		}


		private struct NLED_SETTINGS_INFO
		{
			public uint LedNum; // LED number, 0 is first LED
			public int OffOnBlink; // 0 == off, 1 == on, 2 == blink
			public int TotalCycleTime; // total cycle time of a blink in microseconds
			public int OnTime; // on time of a cycle in microseconds
			public int OffTime; // off time of a cycle in microseconds
			public int MetaCycleOn; // number of on blink cycles
			public int MetaCycleOff; // number of off blink cycles
		}

		private struct NLED_SUPPORTS_INFO
		{
			public uint	LedNum;			// LED number, 0 is first LED
			public int	lCycleAdjust;	// Granularity of cycle time adjustments (microseconds)
			public bool	fAdjustTotalCycleTime;// LED has an adjustable total cycle time
			public bool	fAdjustOnTime;	// LED has separate on time
			public bool	fAdjustOffTime; // LED has separate off time
			public bool	fMetaCycleOn;	// LED can do blink n, pause, blink n, ...
			public bool	fMetaCycleOff;	// LED can do blink n, pause n, blink n, ...
		}

		/// <summary>
		/// Defines the possible states for an LED
		/// </summary>
		public enum LedState : int
		{
			/// <summary>
			/// LED is off
			/// </summary>
			Off = 0,
			/// <summary>
			/// LED is on
			/// </summary>
			On = 1,
			/// <summary>
			/// LED cycles between On and Off
			/// </summary>
			Blink = 2
		}
	}
}