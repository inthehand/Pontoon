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
#if __UNIFIED__
using CoreBluetooth;
using Foundation;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using Windows.Devices.Enumeration;
#endif

namespace InTheHand.Devices.Bluetooth
{
    /// <summary>
    /// Represents a Bluetooth LE device.
    /// </summary>
    public sealed class BluetoothLEDevice
    {
#if WINDOWS_UWP || WINDOWS_PHONE_APP
        private Windows.Devices.Bluetooth.BluetoothLEDevice _device;

        private BluetoothLEDevice(Windows.Devices.Bluetooth.BluetoothLEDevice device)
        {
            _device = device;
        }

        public static implicit operator Windows.Devices.Bluetooth.BluetoothLEDevice(BluetoothLEDevice device)
        {
            return device._device;
        }

        public static implicit operator BluetoothLEDevice(Windows.Devices.Bluetooth.BluetoothLEDevice device)
        {
            return new BluetoothLEDevice(device);
        }

#elif __UNIFIED__
        private CBPeripheral _peripheral;

        internal BluetoothLEDevice(CBPeripheral peripheral)
        {
            _peripheral = peripheral;
        }
#endif

        /// <summary>
        /// Returns a BluetoothLEDevice object for the given BluetoothAddress.
        /// </summary>
        /// <param name="bluetoothAddress"></param>
        /// <returns></returns>
        public static async Task<BluetoothLEDevice> FromBluetoothAddressAsync(ulong bluetoothAddress)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return await Windows.Devices.Bluetooth.BluetoothLEDevice.FromBluetoothAddressAsync(bluetoothAddress);
#else
            return null;
#endif
        }

        /// <summary>
        /// Returns a BluetoothLEDevice object for the given Id.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public static async Task<BluetoothLEDevice> FromIdAsync(string deviceId)
        {
#if __UNIFIED__
            foreach(DeviceInformation di in DeviceInformation._devices)
            {
                if(di.Id == deviceId)
                {
                    return new BluetoothLEDevice(di._peripheral);
                }
            }
            return null;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return await Windows.Devices.Bluetooth.BluetoothLEDevice.FromIdAsync(deviceId);
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
#if __UNIFIED__
            return "btle";
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return Windows.Devices.Bluetooth.BluetoothLEDevice.GetDeviceSelector();
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
#if __UNIFIED__
                if (_nameChanged == null)
                {
                    _peripheral.UpdatedName += _peripheral_UpdatedName;
                }
#endif
                _nameChanged += value;

            }

            remove
            {
                _nameChanged -= value;
#if __UNIFIED__
                if (_nameChanged == null)
                {
                    _peripheral.UpdatedName -= _peripheral_UpdatedName;
                }
#endif
            }
        }

        private void _peripheral_UpdatedName(object sender, EventArgs e)
        {
            _nameChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Gets the device address.
        /// </summary>
        public ulong BluetoothAddress
        {
            get
            {
#if __UNIFIED__
                return ulong.MaxValue;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _device.BluetoothAddress;
#else
                return 0;
#endif
            }
        }

        public BluetoothConnectionStatus ConnectionStatus
        {
            get
            {
#if __UNIFIED__
                return _peripheral.State == CBPeripheralState.Connected ? BluetoothConnectionStatus.Connected : BluetoothConnectionStatus.Disconnected;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return (BluetoothConnectionStatus)((int)_device.ConnectionStatus);
#else
                return BluetoothConnectionStatus.Disconnected;
#endif
            }
        }

        /// <summary>
        /// Gets the device Id.
        /// </summary>
        public string DeviceId
        {
            get
            {
#if __UNIFIED__
                return _peripheral.Identifier.ToString();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _device.DeviceId;
#else
                return string.Empty;
#endif
            }
        }
        private EventWaitHandle _servicesHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private List<GattDeviceService> _services = new List<GattDeviceService>();
        public IReadOnlyList<GattDeviceService> GattServices
        {
            get
            {
#if __UNIFIED__
                if (_services.Count == 0)
                {
                    _peripheral.DiscoveredService += _peripheral_DiscoveredService;
                    _peripheral.DiscoverServices();
                    _servicesHandle.WaitOne();
                    foreach (CBService service in _peripheral.Services)
                    {
                        _services.Add(new GattDeviceService(service));
                    }
                }
                
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                _services.Clear();
                foreach(Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService service in _device.GattServices)
                {
                    _services.Add(service);
                }
#endif

                return _services.AsReadOnly();
            }
        }

#if __UNIFIED__
        private void _peripheral_DiscoveredService(object sender, NSErrorEventArgs e)
        {
            _servicesHandle.Set();
        }
#endif
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