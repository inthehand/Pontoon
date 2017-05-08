//-----------------------------------------------------------------------
// <copyright file="GattReadResult.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using Windows.Devices.Enumeration;
using Windows.Foundation;
#endif

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents the result of an asynchronous read operation of a GATT Characteristic or Descriptor value.
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
    public sealed class GattReadResult
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private Windows.Devices.Bluetooth.GenericAttributeProfile.GattReadResult _result;

        internal GattReadResult(Windows.Devices.Bluetooth.GenericAttributeProfile.GattReadResult result)
        {
            _result = result;
        }

        public static implicit operator Windows.Devices.Bluetooth.GenericAttributeProfile.GattReadResult(GattReadResult result)
        {
            return result._result;
        }

        public static implicit operator GattReadResult(Windows.Devices.Bluetooth.GenericAttributeProfile.GattReadResult result)
        {
            return new GattReadResult(result);
        }

#elif __UNIFIED__
        private byte[] _value;
      
        internal GattReadResult(byte[] value)
        {
            _value = value;
        }
#endif

        /// <summary>
        /// Gets the GATT Descriptor UUID for this GattDescriptor.
        /// </summary>
        public byte[] Value
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                byte[] buffer = new byte[_result.Value.Length];
                Windows.Storage.Streams.DataReader.FromBuffer(_result.Value).ReadBytes(buffer);
                return buffer;
#elif __UNIFIED__
                return _value;
#else
                return null;
#endif
            }
        }
    }
}