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
        private BluetoothGattDescriptor _descriptor;

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
            return new GattReadResult(_descriptor.GetValue());
        }
        
        private Guid GetUuid()
        {
            return _descriptor.Uuid.ToGuid();
        }
    }
}