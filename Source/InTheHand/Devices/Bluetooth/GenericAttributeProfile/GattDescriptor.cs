//-----------------------------------------------------------------------
// <copyright file="GattDescriptor.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
#if __UNIFIED__
using CoreBluetooth;
using Foundation;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using Windows.Devices.Enumeration;
using Windows.Foundation;
#endif

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents a Descriptor of a GATT Characteristic.
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
    public sealed class GattDescriptor
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
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
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return await _descriptor.ReadValueAsync().AsTask();
#elif __UNIFIED__
            return new GattReadResult(((NSData)_descriptor.Value).ToArray());
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
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _descriptor.Uuid;
#elif __UNIFIED__
                return _descriptor.UUID.ToGuid();
#else
                return Guid.Empty;
#endif
            }
        }
    }
}