//-----------------------------------------------------------------------
// <copyright file="Radio.Win32.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace InTheHand.Devices.Radios
{
    public sealed partial class Radio
    {
        private static Type s_type10 = Type.GetType("Windows.Devices.Radios.Radio, Windows, ContentType=WindowsRuntime");
        private object _object10 = null;



        private static string GetDeviceSelectorImpl()
        {
            if (s_type10 != null)
            {
                return s_type10.GetMethod(nameof(GetDeviceSelector), BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[0]).ToString();
            }

            return string.Empty;
        }

        private static void GetRadiosAsyncImpl(List<Radio> radios)
        {
            radios.Add(new Radio());
        }

        private Task<RadioAccessStatus> SetStateAsyncImpl(RadioState state)
        {
            bool enable = state == RadioState.On;
            if(!enable)
            {
                bool discoverySuccess = NativeMethods.BluetoothEnableDiscovery(IntPtr.Zero, false);
            }

            bool success = NativeMethods.BluetoothEnableIncomingConnections(IntPtr.Zero, state == RadioState.On);

            return Task.FromResult<RadioAccessStatus>(success ? RadioAccessStatus.Allowed : RadioAccessStatus.Unspecified);
        }

        // only supporting Bluetooth radio
        private RadioKind GetKindImpl()
        {
            return RadioKind.Bluetooth;
        }

        // matching the UWP behaviour (although we could have used the radio name)
        private string GetNameImpl()
        {
            return "Bluetooth";
        }

        private RadioState GetStateImpl()
        {
            try
            {
                bool state = NativeMethods.BluetoothIsConnectable(IntPtr.Zero);
                return state ? RadioState.On : RadioState.Off;
            }
            catch
            {
                return RadioState.Unknown;
            }
        }

        private static class NativeMethods
        {
            [DllImport("irprops.cpl", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BluetoothEnableDiscovery(IntPtr hRadio, [MarshalAs(UnmanagedType.Bool)] bool fEnabled);

            [DllImport("irprops.cpl", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BluetoothEnableIncomingConnections(IntPtr hRadio, [MarshalAs(UnmanagedType.Bool)] bool fEnabled);

            [DllImport("irprops.cpl", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BluetoothIsConnectable(IntPtr hRadio);
        }
    }
}