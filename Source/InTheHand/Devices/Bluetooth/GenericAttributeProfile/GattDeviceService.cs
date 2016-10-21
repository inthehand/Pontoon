//-----------------------------------------------------------------------
// <copyright file="GattDeviceService.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
#if __IOS__
using CoreBluetooth;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
using Windows.Devices.Enumeration;
using Windows.Foundation;
#endif

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    ///
    /// </summary>
    public sealed class GattDeviceService
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        private Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService  _service;

        internal GattDeviceService(Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService service)
        {
            _service = service;
        }
#elif __IOS__
        internal CBService _service;
      
        internal GattDeviceService(CBService service)
        {
            _service = service;
        }
#endif

        public static string GetDeviceSelectorFromShortId(ushort serviceShortId)
        {
#if __IOS__
            return CBUUID.FromPartial(serviceShortId).ToString();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService.GetDeviceSelectorFromShortId(serviceShortId);
#else
            return string.Empty;
#endif
        }

        public static string GetDeviceSelectorFromUuid(Guid serviceUuid)
        {
#if __IOS__
            return CBUUID.FromBytes(serviceUuid.ToByteArray()).ToString();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService.GetDeviceSelectorFromUuid(serviceUuid);
#else
            return string.Empty;
#endif
        }


        public IReadOnlyList<GattCharacteristic> GetAllCharacteristics()
        {
            List<GattCharacteristic> characteristics = new List<GattCharacteristic>();
#if __IOS__
            foreach(CBCharacteristic characteristic in _service.Characteristics)
            {
                characteristics.Add(new GattCharacteristic(characteristic));
            }

            return characteristics.AsReadOnly();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            foreach (Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic characteristic in _service.GetAllCharacteristics())
            {
                characteristics.Add(new GattCharacteristic(characteristic));
            }

            return new ReadOnlyCollection<GattCharacteristic>(characteristics);
#else
            return new ReadOnlyCollection<GattCharacteristic>(new List<GattCharacteristic>());
#endif
        }
    }
}