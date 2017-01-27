//-----------------------------------------------------------------------
// <copyright file="GattValueChangedEventArgs.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
#if __UNIFIED__
using CoreBluetooth;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using System.Runtime.InteropServices.WindowsRuntime;
#endif

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents the result of an asynchronous read operation of a GATT Characteristic or Descriptor value.
    /// </summary>
    public sealed class GattValueChangedEventArgs
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private Windows.Devices.Bluetooth.GenericAttributeProfile.GattValueChangedEventArgs _args;

        private GattValueChangedEventArgs(Windows.Devices.Bluetooth.GenericAttributeProfile.GattValueChangedEventArgs args)
        {
            _args = args;
        }

        public static implicit operator Windows.Devices.Bluetooth.GenericAttributeProfile.GattValueChangedEventArgs(GattValueChangedEventArgs args)
        {
            return args._args;
        }

        public static implicit operator GattValueChangedEventArgs(Windows.Devices.Bluetooth.GenericAttributeProfile.GattValueChangedEventArgs args)
        {
            return new GattValueChangedEventArgs(args);
        }

#elif __UNIFIED__
        private byte[] _value;
        private DateTimeOffset _timestamp;
      
        internal GattValueChangedEventArgs(byte[] value, DateTimeOffset timestamp)
        {
            _value = value;
            _timestamp = timestamp;
        }
#endif

        /// <summary>
        /// Gets the new Characteristic Value.
        /// </summary>
        public byte[] CharacteristicValue
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _args.CharacteristicValue.ToArray();
#elif __UNIFIED__
                return _value;
#else
                return null;
#endif
            }
        }

        /// <summary>
        /// Gets the time at which the system was notified of the Characteristic Value change.
        /// </summary>
        public DateTimeOffset Timestamp
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _args.Timestamp;
#elif __UNIFIED__
                return _timestamp;
#else
                return DateTimeOffset.MinValue;
#endif
            }
        }
    }
}