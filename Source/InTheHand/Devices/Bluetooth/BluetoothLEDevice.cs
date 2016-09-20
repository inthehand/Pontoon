//-----------------------------------------------------------------------
// <copyright file="BluetoothLEDevice.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InTheHand.Devices.Bluetooth.GenericAttributeProfile;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Foundation;
using System.Threading;
#if __IOS__
using CoreBluetooth;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
using Windows.Devices.Enumeration;
using Windows.Foundation;
#endif

namespace InTheHand.Devices.Enumeration
{
    /// <summary>
    /// Represents a Bluetooth LE device.
    /// </summary>
    public sealed class BluetoothLEDevice
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        private Windows.Devices.Bluetooth.BluetoothLEDevice _device;

        internal BluetoothLEDevice(Windows.Devices.Bluetooth.BluetoothLEDevice device)
        {
            _device = device;
        }
#elif __IOS__
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
#if __IOS__
            return null;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return new BluetoothLEDevice(await Windows.Devices.Bluetooth.BluetoothLEDevice.FromBluetoothAddressAsync(bluetoothAddress));
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
#if __IOS__
            foreach(DeviceInformation di in DeviceInformation._devices)
            {
                if(di.Id == deviceId)
                {
                    return new BluetoothLEDevice(di._peripheral);
                }
            }
            return null;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return new BluetoothLEDevice(await Windows.Devices.Bluetooth.BluetoothLEDevice.FromIdAsync(deviceId));
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
#if __IOS__
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
#if __IOS__
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
#if __IOS__
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
#if __IOS__
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
#if __IOS__
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
#if __IOS__
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
#if __IOS__
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

                return _services.AsReadOnly();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                foreach(Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService service in _device.GattServices)
                {
                    _services.Add(new GattDeviceService(service));
                }

                return new ReadOnlyCollection<GattDeviceService>(_services);
#else
                return new ReadOnlyCollection<GattDeviceService>(new List<GattDeviceService>());
#endif
            }
        }

#if __IOS__
        private void _peripheral_DiscoveredService(object sender, Foundation.NSErrorEventArgs e)
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