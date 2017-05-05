// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PowerManager.Win32.cs" company="In The Hand Ltd">
//   Copyright (c) 2016-17 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace InTheHand.System.Power
{
    public static partial class PowerManager
    {
        private static NativeMethods.SYSTEM_POWER_STATUS _status;

        private static void GetStatus()
        {
            NativeMethods.GetSystemPowerStatus(out _status);
        }

        private static class NativeMethods
        {
            [DllImport("Kernel32")]
            internal static extern bool GetSystemPowerStatus(out SYSTEM_POWER_STATUS systemPowerStatus);

            internal struct SYSTEM_POWER_STATUS
            {
                [MarshalAs(UnmanagedType.U1)]
                public AC_LINE ACLineStatus;
                [MarshalAs(UnmanagedType.U1)]
                public BATTERY_FLAG BatteryFlag;
                public byte BatteryLifePercent;
                public byte SystemStatusFlag;            // set to 0 prior to Win10
                public int BatteryLifeTime;
                public int BatteryFullLifeTime;
            }

            internal enum AC_LINE : byte
            {
                OFFLINE = 0x00,
                ONLINE = 0x01,
                BACKUP_POWER = 0x02,
                UNKNOWN = 0xFF,
            }

            [Flags]
            internal enum BATTERY_FLAG : byte
            {
                /// <summary>
                /// High—the battery capacity is at more than 66 percent
                /// </summary>
                HIGH = 0x01,

                /// <summary>
                /// Low—the battery capacity is at less than 33 percent
                /// </summary>
                LOW = 0x02,

                /// <summary>
                /// Critical—the battery capacity is at less than five percent
                /// </summary>
                CRITICAL = 0x04,

                /// <summary>
                /// Charging
                /// </summary>
                CHARGING = 0x08,

                /// <summary>
                /// No system battery
                /// </summary>
                NO_BATTERY = 0x80,

                /// <summary>
                /// Unknown status—unable to read the battery flag information
                /// </summary>
                UNKNOWN = 0xFF,
            }
        }
    }
}