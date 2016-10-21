//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
#if __IOS__
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

        internal GattCharacteristic(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic characteristic)
        {
            _characteristic = characteristic;
        }
#elif __IOS__
        internal CBCharacteristic _characteristic;
      
        internal GattCharacteristic(CBCharacteristic characteristic)
        {
            _characteristic = characteristic;
        }
#endif

        /// <summary>
        /// Gets the GATT characteristic properties, as defined by the GATT profile.
        /// </summary>
        public GattCharacteristicProperties CharacteristicProperties
        {
            get
            {
#if __IOS__
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
#if __IOS__
                return new Guid(_characteristic.UUID.Uuid);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _characteristic.Uuid;
#else
                return Guid.Empty;
#endif
            }
        }
    }
}