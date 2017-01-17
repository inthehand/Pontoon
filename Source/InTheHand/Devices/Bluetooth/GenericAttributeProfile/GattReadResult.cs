//-----------------------------------------------------------------------
// <copyright file="GattReadResult.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
#if __UNIFIED__
using CoreBluetooth;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using Windows.Devices.Enumeration;
using Windows.Foundation;
#endif

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents the result of an asynchronous read operation of a GATT Characteristic or Descriptor value.
    /// </summary>
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
        private object _value;
      
        internal GattReadResult(object value)
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
                return (byte[])_value;
#else
                return null;
#endif
            }
        }
    }
}