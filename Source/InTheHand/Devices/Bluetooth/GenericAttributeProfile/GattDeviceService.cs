//-----------------------------------------------------------------------
// <copyright file="GattDeviceService.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    ///
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
    public sealed partial class GattDeviceService
    {
        public static Task<GattDeviceService> FromIdAsync(string deviceId)
        {
            return FromIdAsyncImpl(deviceId);
        }

        public static string GetDeviceSelectorFromShortId(ushort serviceShortId)
        {
            return GetDeviceSelectorFromShortIdImpl(serviceShortId);
        }

        public static string GetDeviceSelectorFromUuid(Guid serviceUuid)
        {
            return GetDeviceSelectorFromUuidImpl(serviceUuid);
        }


        public IReadOnlyList<GattCharacteristic> GetAllCharacteristics()
        {
            List<GattCharacteristic> characteristics = new List<GattCharacteristic>();

            GetAllCharacteristics(characteristics);

            return characteristics.AsReadOnly();
        }

        public IReadOnlyList<GattCharacteristic> GetCharacteristics(Guid characteristicUuid)
        {
            List<GattCharacteristic> characteristics = new List<GattCharacteristic>();

            GetCharacteristics(characteristicUuid, characteristics);

            return characteristics.AsReadOnly();
        }

        /// <summary>
        /// The GATT Service UUID associated with this GattDeviceService.
        /// </summary>
        public Guid Uuid
        {
            get
            {
                return GetUuid();
            }
        }
    }
}