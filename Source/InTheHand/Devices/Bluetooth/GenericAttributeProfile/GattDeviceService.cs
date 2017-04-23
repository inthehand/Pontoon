//-----------------------------------------------------------------------
// <copyright file="GattDeviceService.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
#if __UNIFIED__
using CoreBluetooth;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using Windows.Devices.Enumeration;
using Windows.Foundation;
#endif

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
    public sealed class GattDeviceService
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService  _service;

        private GattDeviceService(Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService service)
        {
            _service = service;
        }

        public static implicit operator Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService(GattDeviceService service)
        {
            return service._service;
        }

        public static implicit operator GattDeviceService(Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService service)
        {
            return new GattDeviceService(service);
        }

#elif __UNIFIED__
        internal CBService _service;
        private CBPeripheral _peripheral;
      
        internal GattDeviceService(CBService service, CBPeripheral peripheral)
        {
            _service = service;
            _peripheral = peripheral;
            _peripheral.DiscoveredCharacteristic += _peripheral_DiscoveredCharacteristic;
        }

        ~GattDeviceService()
        {
            _peripheral.DiscoveredCharacteristic -= _peripheral_DiscoveredCharacteristic;
        }

        private void _peripheral_DiscoveredCharacteristic(object sender, CBServiceEventArgs e)
        {
            if (e.Service == _service)
            {
                Debug.WriteLine(DateTimeOffset.Now.ToString() + " DiscoveredCharacteristic");
            }
        }
#endif

        public static async Task<GattDeviceService> FromIdAsync(string deviceId)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return await Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService.FromIdAsync(deviceId).AsTask();
#else
            return null;
#endif
        }

        public static string GetDeviceSelectorFromShortId(ushort serviceShortId)
        {
#if __UNIFIED__
            return CBUUID.FromPartial(serviceShortId).ToString();

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService.GetDeviceSelectorFromShortId(serviceShortId);

#else
            return string.Empty;
#endif
        }

        public static string GetDeviceSelectorFromUuid(Guid serviceUuid)
        {
#if __UNIFIED__
            return CBUUID.FromBytes(serviceUuid.ToByteArray()).ToString();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService.GetDeviceSelectorFromUuid(serviceUuid);
#else
            return string.Empty;
#endif
        }


        public IReadOnlyList<GattCharacteristic> GetAllCharacteristics()
        {
            List<GattCharacteristic> characteristics = new List<GattCharacteristic>();
#if __UNIFIED__
            foreach (CBCharacteristic characteristic in _service.Characteristics)
            {
                characteristics.Add(new GattCharacteristic(characteristic, _service.Peripheral));
            }
            
#elif WINDOWS_UWP || WINDOWS_PHONE_APP
            foreach (Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic characteristic in _service.GetAllCharacteristics())
            {
                characteristics.Add(characteristic);
            }

#elif WINDOWS_PHONE || WINDOWS_APP
            foreach (Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic characteristic in _service.GetCharacteristics(Guid.Empty))
            {
                characteristics.Add(characteristic);
            }

#endif
            return characteristics.AsReadOnly();
        }

        public IReadOnlyList<GattCharacteristic> GetCharacteristics(Guid characteristicUuid)
        {
            List<GattCharacteristic> chars = new List<GenericAttributeProfile.GattCharacteristic>();
#if __UNIFIED__
            foreach (CBCharacteristic characteristic in _service.Characteristics)
            {
                if (characteristic.UUID.ToGuid() == characteristicUuid)
                {
                    chars.Add(new GattCharacteristic(characteristic, _service.Peripheral));
                }
            }

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            foreach (Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic characteristic in _service.GetCharacteristics(characteristicUuid))
            {
                chars.Add(characteristic);
            }
#endif
            return chars.AsReadOnly();
        }

        /// <summary>
        /// The GATT Service UUID associated with this GattDeviceService.
        /// </summary>
        public Guid Uuid
        {
            get
            {
#if __UNIFIED__
                return _service.UUID.ToGuid();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _service.Uuid;
#else
                return Guid.Empty;
#endif
            }
        }
    }
}