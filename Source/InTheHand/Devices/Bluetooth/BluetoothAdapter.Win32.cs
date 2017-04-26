//-----------------------------------------------------------------------
// <copyright file="BluetoothAdapter.Win32.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace InTheHand.Devices.Bluetooth
{
    public sealed partial class BluetoothAdapter
    {
        private static Task<BluetoothAdapter> GetDefaultAsyncImpl()
        {
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
        }
        
        private NativeMethods.BLUETOOTH_RADIO_INFO _radioInfo;

        internal BluetoothAdapter(NativeMethods.BLUETOOTH_RADIO_INFO radioInfo)
        {
            _radioInfo = radioInfo;
        }
        
        private ulong GetBluetoothAddress()
        {
            return _radioInfo.address;
        }
        
        private BluetoothClassOfDevice GetClassOfDevice()
        {
            return new BluetoothClassOfDevice(_radioInfo.ulClassofDevice);
        }
        
        private string GetName()
        {
            return _radioInfo.szName;
        }
    }
}