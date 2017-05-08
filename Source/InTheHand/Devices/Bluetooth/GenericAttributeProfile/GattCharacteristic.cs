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

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents a Characteristic of a GATT service.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>watchOS</term><description>watchOS 2.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.1 or later</description></item></list>
    /// </remarks>
    public sealed partial class GattCharacteristic
    {
        /// <summary>
        /// Gets the collection of all descriptors belonging to this GattCharacteristic instance.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<GattDescriptor> GetAllDescriptors()
        {
            List<GattDescriptor> descriptors = new List<GattDescriptor>();

            GetAllDescriptors(descriptors);

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

            GetDescriptors(descriptorUuid, descriptors);

            return descriptors.AsReadOnly();
        }

        /// <summary>
        /// Performs a Characteristic Value read from the value cache maintained by Windows.
        /// </summary>
        /// <returns></returns>
        public Task<GattReadResult> ReadValueAsync()
        {
            return DoReadValueAsync();
        }

        /// <summary>
        /// Performs a Characteristic Value write to a Bluetooth LE device.
        /// </summary>
        /// <param name="value">A byte array object which contains the data to be written to the Bluetooth LE device.</param>
        /// <returns>The object that manages the asynchronous operation, which, upon completion, returns the status with which the operation completed.</returns>
        public Task<GattCommunicationStatus> WriteValueAsync(byte[] value)
        {
            return DoWriteValueAsync(value);
        }


        /// <summary>
        /// Gets the GATT characteristic properties, as defined by the GATT profile.
        /// </summary>
        public GattCharacteristicProperties CharacteristicProperties
        {
            get
            {
                return GetCharacteristicProperties();
            }
        }

        /// <summary>
        /// Get the user friendly description for this GattCharacteristic, if the User Description Descriptor is present, otherwise this will be an empty string.
        /// </summary>
        public string UserDescription
        {
            get
            {
                return GetUserDescription();
            }
        }

        /// <summary>
        /// Gets the GATT Characteristic UUID for this GattCharacteristic.
        /// </summary>
        public Guid Uuid
        {
            get
            {
                return GetUuid();
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
        
    }
}