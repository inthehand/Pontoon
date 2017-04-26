//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth
{
    /// <summary>
    /// Represents a Bluetooth device.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public sealed partial class BluetoothDevice
    {
        /// <summary>
        /// Returns a <see cref="BluetoothDevice"/> object for the given BluetoothAddress.
        /// </summary>
        /// <param name="address">The address of the Bluetooth device.</param>
        /// <returns>After the asynchronous operation completes, returns the BluetoothDevice object with the given BluetoothAddress or null if the address does not resolve to a valid device.</returns>
        public static async Task<BluetoothDevice> FromBluetoothAddressAsync(ulong address)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return await Windows.Devices.Bluetooth.BluetoothDevice.FromBluetoothAddressAsync(address);
#elif WIN32
            BLUETOOTH_DEVICE_INFO info = new BLUETOOTH_DEVICE_INFO();
            info.dwSize = global::System.Runtime.InteropServices.Marshal.SizeOf(info);
            info.Address = address;
            int result = NativeMethods.BluetoothGetDeviceInfo(IntPtr.Zero, ref info);
            if(result == 0)
            {
                return new BluetoothDevice(info);
            }

            return null;
#else
            return null;
#endif
        }

        /// <summary>
        /// Returns a <see cref="BluetoothDevice"/> object for the given Id.
        /// </summary>
        /// <param name="deviceId">The DeviceId value that identifies the BluetoothDevice instance.</param>
        /// <returns>After the asynchronous operation completes, returns the BluetoothDevice object identified by the given DeviceId.</returns>
        public static async Task<BluetoothDevice> FromIdAsync(string deviceId)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return await Windows.Devices.Bluetooth.BluetoothDevice.FromIdAsync(deviceId);
#elif WIN32
            if(deviceId.StartsWith("bluetooth:"))
            {
                string addrString = deviceId.Substring(10);
                ulong addr = 0;
                if(ulong.TryParse(addrString, global::System.Globalization.NumberStyles.HexNumber, null, out addr))
                {
                    return await FromBluetoothAddressAsync(addr);
                }
            }

            return null;
#else
            return null;
#endif
        }

        /// <summary>
        /// Gets an Advanced Query Syntax (AQS) string for identifying all Bluetooth devices.
        /// This string is passed to the FindAllAsync or CreateWatcher method in order to get a list of Bluetooth devices.
        /// </summary>
        /// <returns></returns>
        public static string GetDeviceSelector()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return Windows.Devices.Bluetooth.BluetoothDevice.GetDeviceSelector();
#else
            return string.Empty;
#endif
        }

        /// <summary>
        /// Creates an Advanced Query Syntax (AQS) filter string from a BluetoothClassOfDevice object.
        /// The AQS string is passed into the CreateWatcher method to return a collection of DeviceInformation objects.
        /// </summary>
        /// <param name="classOfDevice">The class of device used for constructing the AQS string.</param>
        /// <returns>An AQS string that can be passed as a parameter to the CreateWatcher method.</returns>
        public static string GetDeviceSelectorFromClassOfDevice(BluetoothClassOfDevice classOfDevice)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return Windows.Devices.Bluetooth.BluetoothDevice.GetDeviceSelectorFromClassOfDevice(classOfDevice);
#elif WIN32
            return "bluetoothClassOfDevice:" + classOfDevice.RawValue.ToString("X12");
#else
            return string.Empty;
#endif
        }

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private Windows.Devices.Bluetooth.BluetoothDevice _device;

        private BluetoothDevice(Windows.Devices.Bluetooth.BluetoothDevice device)
        {
            _device = device;
        }

        public static implicit operator Windows.Devices.Bluetooth.BluetoothDevice(BluetoothDevice device)
        {
            return device._device;
        }

        public static implicit operator BluetoothDevice(Windows.Devices.Bluetooth.BluetoothDevice device)
        {
            return new BluetoothDevice(device);
        }

#elif WIN32
        private BLUETOOTH_DEVICE_INFO _info;

        internal BluetoothDevice(BLUETOOTH_DEVICE_INFO info)
        {
            _info = info;
        }
#else
#endif

        /// <summary>
        /// Gets the device address.
        /// </summary>
        public ulong BluetoothAddress
        {
            get
            {
#if _WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _device.BluetoothAddress;
#else
                return 0;
#endif
            }
        }

        /// <summary>
        /// Gets the Bluetooth Class Of Device information of the device.
        /// </summary>
        public BluetoothClassOfDevice ClassOfDevice
        {
            get
            {
#if _WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _device.ClassOfDevice;
#elif WIN32
                return new BluetoothClassOfDevice(_info.ulClassofDevice);
#else
                return null;
#endif
            }
        }

        /// <summary>
        /// Gets the connection status of the device.
        /// </summary>
        public BluetoothConnectionStatus ConnectionStatus
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return (BluetoothConnectionStatus)((int)_device.ConnectionStatus);
#elif WIN32
                return _info.fConnected ? BluetoothConnectionStatus.Connected : BluetoothConnectionStatus.Disconnected;
#else
                return BluetoothConnectionStatus.Disconnected;
#endif
            }
        }


        /// <summary>
        /// Gets the device Id.
        /// </summary>
        /// <value>The ID of the device.</value>
        public string DeviceId
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _device.DeviceId;
#else
                return string.Empty;
#endif
            }
        }

        /// <summary>
        /// Gets the Name of the device.
        /// </summary>
        /// <value>The name of the device.</value>
        public string Name
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _device.Name;
#elif WIN32
                return _info.szName;
#else
                return string.Empty;
#endif
            }
        }
    }
}