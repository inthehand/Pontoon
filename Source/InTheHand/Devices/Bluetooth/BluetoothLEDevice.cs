//-----------------------------------------------------------------------
// <copyright file="BluetoothLEDevice.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InTheHand.Devices.Enumeration;
using InTheHand.Devices.Bluetooth.GenericAttributeProfile;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InTheHand.Foundation;
using System.Threading;

namespace InTheHand.Devices.Bluetooth
{
    /// <summary>
    /// Represents a Bluetooth LE device.
    /// </summary>
    public sealed partial class BluetoothLEDevice
    {
        /// <summary>
        /// Returns a BluetoothLEDevice object for the given BluetoothAddress.
        /// </summary>
        /// <param name="bluetoothAddress"></param>
        /// <returns></returns>
        public static Task<BluetoothLEDevice> FromBluetoothAddressAsync(ulong bluetoothAddress)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return FromBluetoothAddressAsyncImpl(bluetoothAddress);
#else
            return Task.FromResult<BluetoothLEDevice>(null);
#endif
        }

        /// <summary>
        /// Returns a BluetoothLEDevice object for the given Id.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public static Task<BluetoothLEDevice> FromIdAsync(string deviceId)
        {
#if __UNIFIED__ || WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return FromIdAsyncImpl(deviceId);
#else
            return null;
#endif
        }

        /// <summary>
        /// Gets an Advanced Query Syntax (AQS) string for identifying all Bluetooth Low Energy (LE) devices.
        /// This string is passed to the FindAllAsync or CreateWatcher method in order to get a list of Bluetooth LE devices.
        /// </summary>
        /// <returns></returns>
        public static string GetDeviceSelector()
        {
#if __UNIFIED__ || WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return GetDeviceSelectorImpl();
#else
            return string.Empty;
#endif
        }

        private event TypedEventHandler<BluetoothLEDevice, object> _nameChanged;
        /// <summary>
        /// Occurs when the name of the device has changed.
        /// </summary>
        public event TypedEventHandler<BluetoothLEDevice, Object> NameChanged
        {
            add
            {
                if (_nameChanged == null)
                {
#if __UNIFIED__ || WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                    NameChangedAdd();
#endif
                }

                _nameChanged += value;

            }

            remove
            {
                _nameChanged -= value;

                if (_nameChanged == null)
                {
#if __UNIFIED__ || WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                    NameChangedRemove();
#endif
                }
            }
        }

        /// <summary>
        /// Gets the device address.
        /// </summary>
        public ulong BluetoothAddress
        {
            get
            {
#if __UNIFIED__ || WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return GetBluetoothAddress();
#else
                return 0;
#endif
            }
        }

        public BluetoothConnectionStatus ConnectionStatus
        {
            get
            {
#if __UNIFIED__ || WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return GetConnectionStatus();
#else
                return BluetoothConnectionStatus.Disconnected;
#endif
            }
        }

        private event TypedEventHandler<BluetoothLEDevice, object> _connectionStatusChanged;

        public event TypedEventHandler<BluetoothLEDevice, object> ConnectionStatusChanged
        {
            add
            {
                if(_connectionStatusChanged == null)
                {
#if __UNIFIED__ || WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                    ConnectionStatusChangedAdd();
#endif
                }

                _connectionStatusChanged += value;
            }
            remove
            {
                _connectionStatusChanged -= value;

                if (_connectionStatusChanged == null)
                {
#if __UNIFIED__ || WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                    ConnectionStatusChangedRemove();
#endif
                }
            }
        }


        /// <summary>
        /// Gets the device Id.
        /// </summary>
        public string DeviceId
        {
            get
            {
#if __UNIFIED__ || WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return GetDeviceId();
#else
                return string.Empty;
#endif
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyList<GattDeviceService> GattServices
        {
            get
            {
#if __UNIFIED__ || WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return GetGattServices();
#else
                return null;
#endif
            }
        }
    }

    /// <summary>
    /// Indicates the connection status of the device.
    /// </summary>
    public enum BluetoothConnectionStatus
    {
        /// <summary>
        /// The device is disconnected.
        /// </summary>
        Disconnected = 0,

        /// <summary>
        /// The device is connected.
        /// </summary>
        Connected = 1,
    }
}