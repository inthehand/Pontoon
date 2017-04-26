//-----------------------------------------------------------------------
// <copyright file="DeviceInformation.Win32.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using InTheHand.Devices.Bluetooth;

namespace InTheHand.Devices.Enumeration
{
    public sealed partial class DeviceInformation
    {

        private BLUETOOTH_DEVICE_INFO _deviceInfo;

        internal DeviceInformation(BLUETOOTH_DEVICE_INFO info)
        {
            _deviceInfo = info;
        }


        private static void FindAllAsyncImpl(string aqsFilter, List<DeviceInformation> list)
        {
            NativeMethods.BLUETOOTH_DEVICE_SEARCH_PARAMS searchParams = new NativeMethods.BLUETOOTH_DEVICE_SEARCH_PARAMS();
            searchParams.dwSize = Marshal.SizeOf(searchParams);
            searchParams.cTimeoutMultiplier = 4;
            searchParams.fIssueInquiry = true;
            searchParams.fReturnAuthenticated = true;
            BLUETOOTH_DEVICE_INFO info = new BLUETOOTH_DEVICE_INFO();
            info.dwSize = Marshal.SizeOf(info);
            IntPtr searchHandle = NativeMethods.BluetoothFindFirstDevice(ref searchParams, ref info);

            if (searchHandle != IntPtr.Zero)
            {
                do
                {
                    list.Add(new DeviceInformation(info));
                }
                while (NativeMethods.BluetoothFindNextDevice(searchHandle, ref info));


                NativeMethods.BluetoothFindDeviceClose(searchHandle);
            }
        }

        private string GetId()
        {
            return "bluetooth:" + _deviceInfo.Address.ToString("X12");
        }

        private string GetName()
        {
            return _deviceInfo.szName;
        }

        private DeviceInformationPairing GetPairing()
        {
            return new DeviceInformationPairing(_deviceInfo);
        }

        private static class NativeMethods
        {
            [DllImport("bthprops.cpl", SetLastError = true)]
            internal static extern IntPtr BluetoothFindFirstDevice(
                ref BLUETOOTH_DEVICE_SEARCH_PARAMS pbtsp,
                ref BLUETOOTH_DEVICE_INFO pbtdi);

            [DllImport("bthprops.cpl", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BluetoothFindNextDevice(
                IntPtr hFind,
                ref BLUETOOTH_DEVICE_INFO pbtdi);

            [DllImport("bthprops.cpl", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BluetoothFindDeviceClose(IntPtr hFind);

            [StructLayout(LayoutKind.Sequential)]
            internal struct BLUETOOTH_DEVICE_SEARCH_PARAMS
            {
                internal int dwSize;
                internal bool fReturnAuthenticated;
                internal bool fReturnRemembered;
                internal bool fReturnUnknown;
                internal bool fReturnConnected;
                internal bool fIssueInquiry;
                internal ushort cTimeoutMultiplier;
                internal IntPtr hRadio;
            }

        }
    }
}