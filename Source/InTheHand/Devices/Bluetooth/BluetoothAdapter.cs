//-----------------------------------------------------------------------
// <copyright file="BluetoothAdapter.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace InTheHand.Devices.Bluetooth
{
    /// <summary>
    /// Represents a local Bluetooth adapter.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public sealed partial class BluetoothAdapter
    {
        /// <summary>
        /// Gets the default BluetoothAdapter.
        /// </summary>
        /// <returns>An asynchronous operation that completes with a BluetoothAdapter.</returns>
        public static Task<BluetoothAdapter> GetDefaultAsync()
        {
#if WIN32
            return Task.Run<BluetoothAdapter>(() =>
            {
                BluetoothAdapter adapter = null;
                NativeMethods.BLUETOOTH_FIND_RADIO_PARAMS p = new NativeMethods.BLUETOOTH_FIND_RADIO_PARAMS();
                p.dwSize = Marshal.SizeOf(p);
                IntPtr radioHandle;
                IntPtr findHandle = NativeMethods.BluetoothFindFirstRadio(ref p, out radioHandle);
                if(findHandle != IntPtr.Zero)
                {
                    NativeMethods.BLUETOOTH_RADIO_INFO radioInfo = new NativeMethods.BLUETOOTH_RADIO_INFO();
                    radioInfo.dwSize = Marshal.SizeOf(radioInfo);
                    if(NativeMethods.BluetoothGetRadioInfo(radioHandle, ref radioInfo) == 0)
                    {
                        adapter = new BluetoothAdapter(radioInfo);
                    }

                    NativeMethods.BluetoothFindRadioClose(findHandle);
                }

                return adapter;
            });

#else
            return Task.ForResult<BluetoothAdapter>(null);
#endif
        }

        private NativeMethods.BLUETOOTH_RADIO_INFO _radioInfo;

        internal BluetoothAdapter(NativeMethods.BLUETOOTH_RADIO_INFO radioInfo)
        {
            _radioInfo = radioInfo;
        }


        /// <summary>
        /// Gets the device address.
        /// </summary>
        public ulong BluetoothAddress
        {
            get
            {
                return _radioInfo.address;
            }
        }

        /*
        /// <summary>
        /// Gets a boolean indicating if the adapter supports the Bluetooth Classic transport type.
        /// </summary>
        public bool IsClassicSupported
        {
            get
            {
                return true;
            }
        }*/

        /// <summary>
        /// Gets the Name of the adapter.
        /// </summary>
        /// <value>The name of the adapter.</value>
        public string Name
        {
            get
            {
#if WIN32
                return _radioInfo.szName;
#else
                return string.Empty;
#endif
            }
        }

    }
}