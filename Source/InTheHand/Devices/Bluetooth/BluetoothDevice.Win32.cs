//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.Win32.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Devices.Enumeration;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth
{
    public sealed partial class BluetoothDevice
    {
        private static Task<BluetoothDevice> FromBluetoothAddressAsyncImpl(ulong address)
        {
            BLUETOOTH_DEVICE_INFO info = new BLUETOOTH_DEVICE_INFO();
            info.dwSize = global::System.Runtime.InteropServices.Marshal.SizeOf(info);
            info.Address = address;
            int result = NativeMethods.BluetoothGetDeviceInfo(IntPtr.Zero, ref info);
            if (result == 0)
            {
                return Task.FromResult<BluetoothDevice>(new BluetoothDevice(info));
            }

            return Task.FromResult<BluetoothDevice>(null);
        }

        private BLUETOOTH_DEVICE_INFO _info;

        internal BluetoothDevice(BLUETOOTH_DEVICE_INFO info)
        {
            _info = info;
        }

        private ulong GetBluetoothAddress()
        {
            return _info.Address;
        }
        
        private BluetoothClassOfDevice GetClassOfDevice()
        {
            return new BluetoothClassOfDevice(_info.ulClassofDevice);
        }

        private BluetoothConnectionStatus GetConnectionStatus()
        {
            return _info.fConnected ? BluetoothConnectionStatus.Connected : BluetoothConnectionStatus.Disconnected;
        }

        private string GetName()
        {
            return _info.szName;
        }
    }
}