//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InTheHand.Foundation;
#if __UNIFIED__
using CoreBluetooth;
using Foundation;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using Windows.Devices.Enumeration;
using System.Runtime.InteropServices.WindowsRuntime;
#endif

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents a Characteristic of a GATT service.
    /// </summary>
    public sealed class GattCharacteristic
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic _characteristic;

        private GattCharacteristic(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic characteristic)
        {
            _characteristic = characteristic;
        }

        public static implicit operator Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic(GattCharacteristic characteristic)
        {
            return characteristic._characteristic;
        }

        public static implicit operator GattCharacteristic(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic characteristic)
        {
            return new GattCharacteristic(characteristic);
        }

#elif __UNIFIED__
        private CBCharacteristic _characteristic;
        private CBPeripheral _peripheral;
        
        internal GattCharacteristic(CBCharacteristic characteristic, CBPeripheral peripheral)
        {
            _characteristic = characteristic;
            _peripheral = peripheral;
        }

        public static implicit operator CBCharacteristic(GattCharacteristic characteristic)
        {
            return characteristic._characteristic;
        }
#endif
        public IReadOnlyList<GattDescriptor> GetAllDescriptors()
        {
            List<GattDescriptor> descriptors = new List<GattDescriptor>();

#if __UNIFIED__
            foreach (CBDescriptor d in _characteristic.Descriptors)
            {
                descriptors.Add(d);
            }

#elif WINDOWS_UWP || WINDOWS_PHONE_APP
            foreach(GattDescriptor d in _characteristic.GetAllDescriptors())
            {
                descriptors.Add(d);
            }
#elif WINDOWS_APP || WINDOWS_PHONE_81
            // TODO: GetAll is missing from SL8.1 so test this workaround
            foreach (GattDescriptor d in _characteristic.GetDescriptors(Guid.Empty))
            {
                descriptors.Add(d);
            }
#endif
            return descriptors.AsReadOnly();
        }

        /// <summary>
        /// Returns a vector of descriptors, that are identified by the specified UUID, and belong to this GattCharacteristic instance.
        /// </summary>
        /// <param name="descriptorUuid">The UUID for the descriptors to be retrieved.</param>
        /// <returns>A vector of descriptors whose UUIDs match descriptorUuid.</returns>
        public IReadOnlyList<GattDescriptor> GetDescriptors(Guid descriptorUuid)
        {
            List<GattDescriptor> descriptors = new List<GattDescriptor>();

#if __UNIFIED__
            foreach (CBDescriptor d in _characteristic.Descriptors)
            {
                if (d.UUID.ToGuid() == descriptorUuid)
                {
                    descriptors.Add(d);
                }
            }

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            foreach(GattDescriptor d in _characteristic.GetDescriptors(descriptorUuid))
            {
                descriptors.Add(d);
            }

#endif
            return descriptors.AsReadOnly();
        }

        /// <summary>
        /// Performs a Characteristic Value read from the value cache maintained by Windows.
        /// </summary>
        /// <returns></returns>
        public async Task<GattReadResult> ReadValueAsync()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return await _characteristic.ReadValueAsync().AsTask();
#elif __UNIFIED__
            return new GattReadResult(_characteristic.Value.ToArray());
#else
                return null;
#endif
        }

        /// <summary>
        /// Performs a Characteristic Value write to a Bluetooth LE device.
        /// </summary>
        /// <param name="value">A byte array object which contains the data to be written to the Bluetooth LE device.</param>
        /// <returns>The object that manages the asynchronous operation, which, upon completion, returns the status with which the operation completed.</returns>
        public async Task<GattCommunicationStatus> WriteValueAsync(byte[] value)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return await _characteristic.WriteValueAsync(value.AsBuffer()).AsTask() == Windows.Devices.Bluetooth.GenericAttributeProfile.GattCommunicationStatus.Success ? GattCommunicationStatus.Success : GattCommunicationStatus.Unreachable;
#elif __UNIFIED__
            try
            {
                _characteristic.Value = NSData.FromArray(value);
                return GattCommunicationStatus.Success;
            }
            catch
            {
                return GattCommunicationStatus.Unreachable;
            }
#else
                throw new PlatformNotSupportedException();
#endif
        }


        /// <summary>
        /// Gets the GATT characteristic properties, as defined by the GATT profile.
        /// </summary>
        public GattCharacteristicProperties CharacteristicProperties
        {
            get
            {
#if __UNIFIED__
                return _characteristic.Properties.ToGattCharacteristicProperties();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return (GattCharacteristicProperties)((uint)_characteristic.CharacteristicProperties);
#else
                return GattCharacteristicProperties.None;
#endif
            }
        }

        /// <summary>
        /// Get the user friendly description for this GattCharacteristic, if the User Description Descriptor is present, otherwise this will be an empty string.
        /// </summary>
        public string UserDescription
        {
            get
            {
#if __UNIFIED__
                foreach(CBDescriptor desc in _characteristic.Descriptors)
                {
                    if(desc.UUID.ToGuid() == GattDescriptorUuids.CharacteristicUserDescription)
                    {
                        return desc.Value.ToString();
                    }
                }

                return string.Empty;

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _characteristic.UserDescription;
#else
                return string.Empty;
#endif
            }
        }

        /// <summary>
        /// Gets the GATT Characteristic UUID for this GattCharacteristic.
        /// </summary>
        public Guid Uuid
        {
            get
            {
#if __UNIFIED__
                return _characteristic.UUID.ToGuid();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _characteristic.Uuid;
#else
                return Guid.Empty;
#endif
            }
        }

        private event TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs> valueChanged;
        /// <summary>
        /// An App can register an event handler in order to receive events when notification or indications are received from a device, after setting the Client Characteristic Configuration Descriptor.
        /// </summary>
        public event TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs> ValueChanged
        {
            add
            {
                if(valueChanged == null)
                {
#if __UNIFIED__
                    _peripheral.SetNotifyValue(true, _characteristic);
                    _peripheral.UpdatedCharacterteristicValue += _peripheral_UpdatedCharacterteristicValue;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                    _characteristic.ValueChanged += _characteristic_ValueChanged;
#endif

                }

                valueChanged += value;
            }

            remove
            {
                valueChanged -= value;

                if (valueChanged == null)
                {
#if __UNIFIED__
                    _peripheral.UpdatedCharacterteristicValue -= _peripheral_UpdatedCharacterteristicValue;
                    _peripheral.SetNotifyValue(false, _characteristic);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                    _characteristic.ValueChanged -= _characteristic_ValueChanged;
#endif
                }
            }
        }
        

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private void _characteristic_ValueChanged(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic sender, Windows.Devices.Bluetooth.GenericAttributeProfile.GattValueChangedEventArgs args)
        {
            valueChanged?.Invoke(this, args);
        }
#elif __UNIFIED__
        private void _peripheral_UpdatedCharacterteristicValue(object sender, CBCharacteristicEventArgs e)
        {
            valueChanged?.Invoke(this, new GattValueChangedEventArgs(e.Characteristic.Value.ToArray(), DateTimeOffset.Now));
        }
#endif
    }
}