//-----------------------------------------------------------------------
// <copyright file="GattDescriptor.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
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
    /// Represents a Descriptor of a GATT Characteristic.
    /// </summary>
    public sealed class GattDescriptor
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        private Windows.Devices.Bluetooth.GenericAttributeProfile.GattDescriptor _descriptor;

        private GattDescriptor(Windows.Devices.Bluetooth.GenericAttributeProfile.GattDescriptor descriptor)
        {
            _descriptor = descriptor;
        }

        public static implicit operator Windows.Devices.Bluetooth.GenericAttributeProfile.GattDescriptor(GattDescriptor descriptor)
        {
            return descriptor._descriptor;
        }

        public static implicit operator GattDescriptor(Windows.Devices.Bluetooth.GenericAttributeProfile.GattDescriptor descriptor)
        {
            return new GattDescriptor(descriptor);
        }

#elif __UNIFIED__
        internal CBDescriptor _descriptor;
      
        private GattDescriptor(CBDescriptor descriptor)
        {
            _descriptor = descriptor;
        }

        public static implicit operator CBDescriptor(GattDescriptor descriptor)
        {
            return descriptor._descriptor;
        }

        public static implicit operator GattDescriptor(CBDescriptor descriptor)
        {
            return new GattDescriptor(descriptor);
        }
#endif

        public async Task<GattReadResult> ReadValueAsync()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return await _descriptor.ReadValueAsync().AsTask();
#elif __UNIFIED__
            return new GattReadResult(_descriptor.Value);
#else
                return null;
#endif
        }

        /// <summary>
        /// Gets the GATT Descriptor UUID for this GattDescriptor.
        /// </summary>
        public Guid Uuid
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _descriptor.Uuid;
#else
                return Guid.Empty;
#endif
            }
        }
    }
}