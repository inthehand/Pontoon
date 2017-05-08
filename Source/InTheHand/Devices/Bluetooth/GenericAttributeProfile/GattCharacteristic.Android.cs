//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.Android.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InTheHand.Foundation;
using System.Diagnostics;
using Android.Bluetooth;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{

    partial class GattCharacteristic
    {
        private BluetoothGattCharacteristic _characteristic;

        private GattCharacteristic(BluetoothGattCharacteristic characteristic)
        {
            _characteristic = characteristic;
        }

        public static implicit operator BluetoothGattCharacteristic(GattCharacteristic characteristic)
        {
            return characteristic._characteristic;
        }

        public static implicit operator GattCharacteristic(BluetoothGattCharacteristic characteristic)
        {
            return new GattCharacteristic(characteristic);
        }

        private void GetAllDescriptors(List<GattDescriptor> descriptors)
        {
            foreach(BluetoothGattDescriptor descriptor in _characteristic.Descriptors)
            {
                descriptors.Add(descriptor);
            }
        }
        
        private void GetDescriptors(Guid descriptorUuid, List<GattDescriptor> descriptors)
        {
            foreach (BluetoothGattDescriptor descriptor in _characteristic.Descriptors)
            {
                if (descriptor.Uuid.ToGuid() == descriptorUuid)
                {
                    descriptors.Add(descriptor);
                }
            }
        }
        
        private async Task<GattReadResult> DoReadValueAsync()
        {
            return new GattReadResult(GattCommunicationStatus.Success, _characteristic.GetValue());
        }

        /// <summary>
        /// Performs a Characteristic Value write to a Bluetooth LE device.
        /// </summary>
        /// <param name="value">A byte array object which contains the data to be written to the Bluetooth LE device.</param>
        /// <returns>The object that manages the asynchronous operation, which, upon completion, returns the status with which the operation completed.</returns>
        private async Task<GattCommunicationStatus> DoWriteValueAsync(byte[] value)
        {
            bool success = _characteristic.SetValue(value);
            return success ? GattCommunicationStatus.Success : GattCommunicationStatus.Unreachable;
        }
        
        private GattCharacteristicProperties GetCharacteristicProperties()
        {
            return (GattCharacteristicProperties)((int)_characteristic.Properties);
        }

        private string GetUserDescription()
        {
            foreach (BluetoothGattDescriptor descriptor in _characteristic.Descriptors)
            {
                if (descriptor.Uuid.ToGuid() == GattDescriptorUuids.CharacteristicUserDescription)
                {
                    return global::System.Text.Encoding.Unicode.GetString(descriptor.GetValue());
                }
            }

            return string.Empty;
        }

        private Guid GetUuid()
        {
            return _characteristic.Uuid.ToGuid();
        }
    }
}