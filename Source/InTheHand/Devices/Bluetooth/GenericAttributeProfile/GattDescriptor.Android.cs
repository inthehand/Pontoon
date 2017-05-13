//-----------------------------------------------------------------------
// <copyright file="GattDescriptor.Android.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Android.Bluetooth;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    partial class GattDescriptor
    {
        private BluetoothLEDevice _device;
        private BluetoothGattDescriptor _descriptor;

        internal GattDescriptor(BluetoothLEDevice device, BluetoothGattDescriptor descriptor)
        {
            _device = device;
            _descriptor = descriptor;
        }

        private GattDescriptor(BluetoothGattDescriptor descriptor)
        {
            _descriptor = descriptor;
        }

        public static implicit operator BluetoothGattDescriptor(GattDescriptor descriptor)
        {
            return descriptor._descriptor;
        }

        public static implicit operator GattDescriptor(BluetoothGattDescriptor descriptor)
        {
            return new GattDescriptor(descriptor);
        }

        private async Task<GattReadResult> DoReadValueAsync()
        {
            if (_device._bluetoothGatt.ReadDescriptor(_descriptor))
            {
                return new GattReadResult(GattCommunicationStatus.Success, _descriptor.GetValue());
            }

            return new GattReadResult(GattCommunicationStatus.Unreachable, null);
        }
        
        private Guid GetUuid()
        {
            return _descriptor.Uuid.ToGuid();
        }


    }
}