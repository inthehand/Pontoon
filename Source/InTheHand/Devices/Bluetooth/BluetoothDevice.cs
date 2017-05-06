//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

#if __ANDROID__
using Android.OS;
#endif
using InTheHand.Devices.Bluetooth.Rfcomm;
using InTheHand.Devices.Enumeration;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Android</term><description>Android 4.4 and later</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
        /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.1 or later</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
        /// </remarks>
        public static async Task<BluetoothDevice> FromBluetoothAddressAsync(ulong address)
        {
#if __ANDROID__
            byte[] buffer = new byte[6];
            var addressBytes = BitConverter.GetBytes(address);
            for(int i=0; i< 6; i++)
            {
                buffer[i] = addressBytes[i];
            }
            return new BluetoothDevice(DeviceInformation.Manager.Adapter.GetRemoteDevice(buffer));

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return await Windows.Devices.Bluetooth.BluetoothDevice.FromBluetoothAddressAsync(address);

#elif WIN32
            return await FromBluetoothAddressAsyncImpl(address);

#else
            return null;
#endif
        }

        /// <summary>
        /// Returns a <see cref="BluetoothDevice"/> object for the given Id.
        /// </summary>
        /// <param name="deviceId">The DeviceId value that identifies the <see cref="BluetoothDevice"/> instance.</param>
        /// <returns>After the asynchronous operation completes, returns the <see cref="BluetoothDevice"/> object identified by the given DeviceId.</returns>
        public static async Task<BluetoothDevice> FromIdAsync(string deviceId)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return await Windows.Devices.Bluetooth.BluetoothDevice.FromIdAsync(deviceId);

#elif WIN32
            if (deviceId.StartsWith("BLUETOOTH#"))
            {
                var parts = deviceId.Split('#');

                string addrString = parts[1];
                ulong addr = 0;
                if (ulong.TryParse(addrString, global::System.Globalization.NumberStyles.HexNumber, null, out addr))
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
        /// Returns a <see cref="BluetoothDevice"/> object for the given DeviceInformation.
        /// </summary>
        /// <param name="deviceInformation">The DeviceInformation value that identifies the BluetoothDevice instance.</param>
        /// <returns>After the asynchronous operation completes, returns the BluetoothDevice object identified by the given DeviceInformation.</returns>
        public static async Task<BluetoothDevice> FromDeviceInformationAsync(DeviceInformation deviceInformation)
        {
#if __ANDROID__
            return deviceInformation._device;

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return await Windows.Devices.Bluetooth.BluetoothDevice.FromIdAsync(deviceInformation.Id);

#elif WIN32
            return new BluetoothDevice(deviceInformation._deviceInfo);

#else
            return null;
#endif
        }

        /// <summary>
        /// Gets an Advanced Query Syntax (AQS) string for identifying all Bluetooth devices.
        /// This string is passed to the <see cref="DeviceInformation.FindAllAsync"/> or CreateWatcher method in order to get a list of Bluetooth devices.
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

        /// <summary>
        /// Creates an Advanced Query Syntax (AQS) filter string that contains a query for Bluetooth devices that are either paired or unpaired.
        /// The AQS string is passed into the CreateWatcher method to return a collection of <see cref="DeviceInformation"/> objects.
        /// </summary>
        /// <param name="pairingState">The current pairing state for Bluetooth devices used for constructing the AQS string.
        /// Bluetooth devices can be either paired (true) or unpaired (false).
        /// The AQS Filter string will request scanning to be performed when the pairingState is false.</param>
        /// <returns>An AQS string that can be passed as a parameter to the CreateWatcher method.</returns>
        public static string GetDeviceSelectorFromPairingState(bool pairingState)
        {
#if WINDOWS_UWP
            return Windows.Devices.Bluetooth.BluetoothDevice.GetDeviceSelectorFromPairingState(pairingState);
#elif WIN32
            return "bluetoothPairingState:" + pairingState.ToString();
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
#elif __ANDROID__ || WIN32
                return GetBluetoothAddress();
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

#elif __ANDROID__ || WIN32
                return GetClassOfDevice();
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
                return GetConnectionStatus();
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

#elif __ANDROID__ || WIN32
                return GetName();
#else
                return string.Empty;
#endif
            }
        }

        /// <summary>
        /// Gets the read-only list of RFCOMM services supported by the device.
        /// </summary>
        public IReadOnlyList<RfcommDeviceService> RfcommServices
        {
            get
            {
                var list = new List<RfcommDeviceService>();

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                foreach(Windows.Devices.Bluetooth.Rfcomm.RfcommDeviceService service in _device.RfcommServices)
                {
                    list.Add(service);
                }

#elif WIN32
                GetRfcommServices(list);

#elif __ANDROID__
                bool success = _device.FetchUuidsWithSdp();
                if (success)
                {
                    ParcelUuid[] uuids = _device.GetUuids();
                    if (uuids != null)
                    {
                        foreach (ParcelUuid g in uuids)
                        {
                            list.Add(new RfcommDeviceService(this, RfcommServiceId.FromUuid(new Guid(g.Uuid.ToString()))));
                        }
                    }
                }
#endif
                return list.AsReadOnly();
            }
        }

        public override string ToString()
        {
            return "BLUETOOTH#" + BluetoothAddress.ToString("X12");
        }
    }
}