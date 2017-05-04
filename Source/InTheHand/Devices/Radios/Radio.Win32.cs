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
using System.Collections;

namespace InTheHand.Devices.Radios
{
    public sealed partial class Radio
    {
        private static Type s_type10 = Type.GetType("Windows.Devices.Radios.Radio, Windows, ContentType=WindowsRuntime");
        private object _object10 = null;

        internal Radio(object o10)
        {
            _object10 = o10;
        }

        private static string GetDeviceSelectorImpl()
        {
            if (s_type10 != null)
            {
                return s_type10.GetMethod(nameof(GetDeviceSelector), BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[0]).ToString();
            }

            return string.Empty;
        }

        private static async Task<RadioAccessStatus> RequestAccessAsyncImpl()
        {
            if (s_type10 != null)
            {
                object rta = s_type10.GetMethod("RequestAccessAsync").Invoke(null, new object[] { });
                Type tras = Type.GetType("Windows.Devices.Radios.RadioAccessStatus, Windows, ContentType=WindowsRuntime");
                Type tr = typeof(Windows.Foundation.IAsyncOperation<>).MakeGenericType(tras);
                object result = tr.GetMethod("GetResults").Invoke(rta, new object[] { });
                return (RadioAccessStatus)((int)result);
            }
            else
            {
                // Only add a Radio if support is present.
                return RadioAccessStatus.Allowed;
            }
        }

        private static void GetRadiosAsyncImpl(List<Radio> radios)
        {
            if (s_type10 != null)
            {
                object rta = s_type10.GetMethod("GetRadiosAsync").Invoke(null, new object[] { });
                Type ivv = Type.GetType("Windows.Foundation.Collections.IVectorView`1, Windows, ContentType=WindowsRuntime");
                Type t = ivv.MakeGenericType(s_type10);
                Type tr = typeof(Windows.Foundation.IAsyncOperation<>).MakeGenericType(t);
                object results = tr.GetMethod("GetResults").Invoke(rta, new object[] { });
                uint count = (uint)t.GetProperty("Size").GetValue(results);
                IEnumerable il = results as IEnumerable;
                foreach(object o in il)
                {
                    radios.Add(new Radios.Radio(o));
                }
            }
            else
            {
                // Only add a Radio if support is present.
                if (NativeMethods.BluetoothIsVersionAvailable(2, 0))
                {
                    radios.Add(new Radio());
                }
            }
        }

        private Task<RadioAccessStatus> SetStateAsyncImpl(RadioState state)
        {
            bool enable = state == RadioState.On;
            /*int result = NativeMethods.BthpEnableRadioSoftware(enable);
            bool success = result == 0;*/

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
            /*bool state;
            int result = NativeMethods.BthpIsRadioSoftwareEnabled(out state);

            if (result != 0)
                return RadioState.Unknown;

            return state ? RadioState.On : RadioState.Off;*/

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
            [DllImport("BluetoothApis", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BluetoothEnableDiscovery(IntPtr hRadio, [MarshalAs(UnmanagedType.Bool)] bool fEnabled);

            [DllImport("BluetoothApis", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BluetoothEnableIncomingConnections(IntPtr hRadio, [MarshalAs(UnmanagedType.Bool)] bool fEnabled);

            [DllImport("BluetoothApis", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BluetoothIsConnectable(IntPtr hRadio);

            [DllImport("BluetoothApis")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BluetoothIsVersionAvailable(byte MajorVersion, byte MinorVersion);

            /*[DllImport("BluetoothApis", SetLastError = false)]
            internal static extern int BthpIsRadioSoftwareEnabled(out bool value);

            [DllImport("BluetoothApis", SetLastError = false)]
            internal static extern int BthpEnableRadioSoftware(bool fEnable);*/
        }
    }
}