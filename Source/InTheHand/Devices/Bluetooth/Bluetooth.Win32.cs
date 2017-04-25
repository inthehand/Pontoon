//-----------------------------------------------------------------------
// <copyright file="Bluetooth.Win32.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct BLUETOOTH_DEVICE_INFO
    {
        internal int dwSize;
        internal ulong Address;
        internal uint ulClassofDevice;
        [MarshalAs(UnmanagedType.Bool)]
        internal bool fConnected;
        [MarshalAs(UnmanagedType.Bool)]
        internal bool fRemembered;
        [MarshalAs(UnmanagedType.Bool)]
        internal bool fAuthenticated;
        internal SYSTEMTIME stLastSeen;
        internal SYSTEMTIME stLastUsed;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 248)]
        internal string szName;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SYSTEMTIME
    {
        private ushort year;
        private short month;
        private short dayOfWeek;
        private short day;
        private short hour;
        private short minute;
        private short second;
        private short millisecond;
    }

    internal static class NativeMethods
    {
        [DllImport("bthprops.cpl", SetLastError = true)]
        internal static extern int BluetoothGetDeviceInfo(IntPtr hRadio,  ref BLUETOOTH_DEVICE_INFO pbtdi);
    }
}