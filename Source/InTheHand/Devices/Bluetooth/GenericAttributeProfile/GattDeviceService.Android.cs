//-----------------------------------------------------------------------
// <copyright file="GattDeviceService.Android.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Android.Bluetooth;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    partial class GattDeviceService
    {
        private BluetoothGattService _service;
        
        private GattDeviceService(BluetoothGattService service)
        {
            _service = service;
        }

        public static implicit operator BluetoothGattService(GattDeviceService service)
        {
            return service._service;
        }

        public static implicit operator GattDeviceService(BluetoothGattService service)
        {
            return new GattDeviceService(service);
        }

        private static async Task<GattDeviceService> FromIdAsyncImpl(string deviceId)
        {
            return null;
        }

        private static string GetDeviceSelectorFromShortIdImpl(ushort serviceShortId)
        {
            return string.Empty;
        }

        private static string GetDeviceSelectorFromUuidImpl(Guid serviceUuid)
        {
            return string.Empty;
        }


        private void GetAllCharacteristics(List<GattCharacteristic> characteristics)
        {
            foreach(BluetoothGattCharacteristic characteristic in _service.Characteristics)
            {
                characteristics.Add(characteristic);
            }
        }

        private void GetCharacteristics(Guid characteristicUuid, List<GattCharacteristic> characteristics)
        {
            foreach (BluetoothGattCharacteristic characteristic in _service.Characteristics)
            {
                if (characteristic.Uuid.ToGuid() == characteristicUuid)
                {
                    characteristics.Add(characteristic);
                }
            }
        }

        private Guid GetUuid()
        {
            return _service.Uuid.ToGuid();
        }
    }
}