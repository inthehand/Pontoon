//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.Unified.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InTheHand.Foundation;
using System.Diagnostics;
using CoreBluetooth;
using Foundation;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    partial class GattCharacteristic
    {
        private CBCharacteristic _characteristic;
        private CBPeripheral _peripheral;
        
        internal GattCharacteristic(CBCharacteristic characteristic, CBPeripheral peripheral)
        {
            _characteristic = characteristic;
            _peripheral = peripheral;
            _peripheral.DiscoveredDescriptor += _peripheral_DiscoveredDescriptor;
        }

        ~GattCharacteristic()
        {
            _peripheral.DiscoveredDescriptor -= _peripheral_DiscoveredDescriptor;
        }

        private void _peripheral_DiscoveredDescriptor(object sender, CBCharacteristicEventArgs e)
        {
            if(e.Characteristic == _characteristic)
            {
                Debug.WriteLine(DateTimeOffset.Now.ToString() + " DiscoveredDescriptor");
            }
        }

        public static implicit operator CBCharacteristic(GattCharacteristic characteristic)
        {
            return characteristic._characteristic;
        }


        private void GetAllDescriptors(List<GattDescriptor> descriptors)
        {
            _peripheral.DiscoverDescriptors(_characteristic);

            foreach (CBDescriptor d in _characteristic.Descriptors)
            {
                descriptors.Add(d);
            }
        }

        private void GetDescriptors(Guid descriptorUuid, List<GattDescriptor> descriptors)
        {
            _peripheral.DiscoverDescriptors(_characteristic);

            foreach (CBDescriptor descriptor in _characteristic.Descriptors)
            {
                if (descriptor.UUID.ToGuid() == descriptorUuid)
                {
                    descriptors.Add(descriptor);
                }
            }
        }
        
        private async Task<GattReadResult> DoReadValueAsync()
        {
            try
            {
                return new GattReadResult(GattCommunicationStatus.Success, _characteristic.Value.ToArray());
            }
            catch
            {
                return new GattReadResult(GattCommunicationStatus.Unreachable, null);
            }
        }

        private async Task<GattCommunicationStatus> DoWriteValueAsync(byte[] value)
        {
            try
            {
                _characteristic.Value = NSData.FromArray(value);
                return GattCommunicationStatus.Success;
            }
            catch
            {
                return GattCommunicationStatus.Unreachable;
            }
        }

        private GattCharacteristicProperties GetCharacteristicProperties()
        {
            return _characteristic.Properties.ToGattCharacteristicProperties();
        }

        private string GetUserDescription()
        {
            foreach (CBDescriptor desc in _characteristic.Descriptors)
            {
                if (desc.UUID.ToGuid() == GattDescriptorUuids.CharacteristicUserDescription)
                {
                    return desc.Value.ToString();
                }
            }

            return string.Empty;
        }

        private Guid GetUuid()
        {
            return _characteristic.UUID.ToGuid();
        }

        private void ValueChangedAdd()
        {
            _peripheral.SetNotifyValue(true, _characteristic);
            _peripheral.UpdatedCharacterteristicValue += _peripheral_UpdatedCharacterteristicValue;
        }

        private void ValueChangedRemove()
        {
            _peripheral.UpdatedCharacterteristicValue -= _peripheral_UpdatedCharacterteristicValue;
            _peripheral.SetNotifyValue(false, _characteristic);
        }
       

        private void _peripheral_UpdatedCharacterteristicValue(object sender, CBCharacteristicEventArgs e)
        {
            valueChanged?.Invoke(this, new GattValueChangedEventArgs(e.Characteristic.Value.ToArray(), DateTimeOffset.Now));
        }
    }
}