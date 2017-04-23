//-----------------------------------------------------------------------
// <copyright file="DevicePicker.Win32.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace InTheHand.Devices.Enumeration
{
    public sealed partial class DevicePicker
    {

        private async Task<DeviceInformation> PickSingleDeviceAsyncImpl()
        {
                NativeMethods.BLUETOOTH_SELECT_DEVICE_PARAMS sdp = new NativeMethods.BLUETOOTH_SELECT_DEVICE_PARAMS();
                sdp.dwSize = Marshal.SizeOf(sdp);
                sdp.fShowAuthenticated = true;
                sdp.cNumDevices = 1;
                sdp.hwndParent = NativeMethods.GetForegroundWindow();

                bool success = NativeMethods.BluetoothSelectDevices(ref sdp);
                if(success)
                {
                    BLUETOOTH_DEVICE_INFO info = Marshal.PtrToStructure<BLUETOOTH_DEVICE_INFO>(sdp.pDevices);
                    NativeMethods.BluetoothSelectDevicesFree(ref sdp);

                    return new DeviceInformation(info);
                }

                return null;
        }

        private static class NativeMethods
        {
            [DllImport("User32", SetLastError = true)]
            internal static extern IntPtr GetForegroundWindow();

            [DllImport("IrProps.cpl", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BluetoothSelectDevices(ref BLUETOOTH_SELECT_DEVICE_PARAMS pbtsdp);

            [DllImport("IrProps.cpl", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BluetoothSelectDevicesFree(ref BLUETOOTH_SELECT_DEVICE_PARAMS pbtsdp);

            [StructLayout(LayoutKind.Sequential)]
            internal struct BLUETOOTH_SELECT_DEVICE_PARAMS
            {
                internal int dwSize;
                internal uint cNumOfClasses;
                internal IntPtr /*BLUETOOTH_COD_PAIRS**/ prgClassOfDevices;
                [MarshalAs(UnmanagedType.LPWStr)]
                internal string pszInfo;
                internal IntPtr hwndParent;
                [MarshalAs(UnmanagedType.Bool)]
                internal bool fForceAuthentication;
                [MarshalAs(UnmanagedType.Bool)]
                internal bool fShowAuthenticated;
                [MarshalAs(UnmanagedType.Bool)]
                internal bool fShowRemembered;
                [MarshalAs(UnmanagedType.Bool)]
                internal bool fShowUnknown;
                [MarshalAs(UnmanagedType.Bool)]
                internal bool fAddNewDeviceWizard;
                [MarshalAs(UnmanagedType.Bool)]
                internal bool fSkipServicesPage;
                internal IntPtr /*PFN_DEVICE_CALLBACK*/ pfnDeviceCallback;
                internal IntPtr pvParam;
                internal uint cNumDevices;
                internal IntPtr /*PBLUETOOTH_DEVICE_INFO*/ pDevices;
            }
        }
    }

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
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst =248)]
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
}