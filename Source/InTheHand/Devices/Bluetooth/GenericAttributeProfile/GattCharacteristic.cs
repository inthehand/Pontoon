//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#if __UNIFIED__
using CoreBluetooth;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
using Windows.Devices.Enumeration;
using Windows.Foundation;
#endif

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents a Characteristic of a GATT service.
    /// </summary>
    public sealed class GattCharacteristic
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
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
        internal CBCharacteristic _characteristic;
      
        private GattCharacteristic(CBCharacteristic characteristic)
        {
            _characteristic = characteristic;
        }

        public static implicit operator CBCharacteristic(GattCharacteristic characteristic)
        {
            return characteristic._characteristic;
        }

        public static implicit operator GattCharacteristic(CBCharacteristic characteristic)
        {
            return new GattCharacteristic(characteristic);
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

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP          
            foreach(GattDescriptor d in _characteristic.GetAllDescriptors())
            {
                descriptors.Add(d);
            }
               
#endif
            return descriptors.AsReadOnly();
        }

        public async Task<GattReadResult> ReadValueAsync()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return await _characteristic.ReadValueAsync().AsTask();
#elif __UNIFIED__
            return new GattReadResult(_characteristic.Value);
#else
                return null;
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
                return (GattCharacteristicProperties)((uint)_characteristic.Properties);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return (GattCharacteristicProperties)((uint)_characteristic.CharacteristicProperties);
#else
                return GattCharacteristicProperties.None;
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
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _characteristic.Uuid;
#else
                return Guid.Empty;
#endif
            }
        }
    }
}