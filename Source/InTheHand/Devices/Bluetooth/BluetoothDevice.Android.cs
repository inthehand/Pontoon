//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.Android.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Android.OS;
using InTheHand.Devices.Bluetooth.Rfcomm;
using InTheHand.Devices.Enumeration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth
{
    public sealed partial class BluetoothDevice
    {
        internal Android.Bluetooth.BluetoothDevice _device;

        private BluetoothDevice(Android.Bluetooth.BluetoothDevice device)
        {
            _device = device;
        }

        public static implicit operator Android.Bluetooth.BluetoothDevice(BluetoothDevice device)
        {
            return device._device;
        }

        public static implicit operator BluetoothDevice(Android.Bluetooth.BluetoothDevice device)
        {
            return new BluetoothDevice(device);
        }


        private static async Task<BluetoothLEDevice> FromIdAsyncImpl(string deviceId)
        {
            return Android.Bluetooth.BluetoothAdapter.DefaultAdapter.GetRemoteDevice(deviceId);
        }

        private ulong GetBluetoothAddress()
        {
            return ulong.Parse(_device.Address.Replace(":", ""), NumberStyles.HexNumber);
        }

        private BluetoothClassOfDevice GetClassOfDevice()
        {
            return new BluetoothClassOfDevice((uint)_device.BluetoothClass.DeviceClass);
        }

        private string GetDeviceId()
        {
            return _device.Address;
        }

        private string GetName()
        {
            return _device.Name;
        }

        private void GetRfcommServices(List<RfcommDeviceService> services)
        {
            ParcelUuid[] uuids = _device.GetUuids();
            if (uuids != null)
            {
                foreach (ParcelUuid g in uuids)
                {
                    services.Add(new RfcommDeviceService(this, RfcommServiceId.FromUuid(new Guid(g.Uuid.ToString()))));
                }
            }
        }
    }
}