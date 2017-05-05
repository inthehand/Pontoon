//-----------------------------------------------------------------------
// <copyright file="DeviceInformationPairing.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Threading.Tasks;
using System;
#if WINDOWS_UWP
using System.Runtime.InteropServices.WindowsRuntime;
#elif WIN32
using System.Runtime.InteropServices;
using InTheHand.Devices.Bluetooth;
#endif

namespace InTheHand.Devices.Enumeration
{
    /// <summary>
    /// Contains information and enables pairing for a device.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public sealed class DeviceInformationPairing
    {
#if __ANDROID__
        private Android.Bluetooth.BluetoothDevice _device;

        internal DeviceInformationPairing(Android.Bluetooth.BluetoothDevice device)
        {
            _device = device;
        }

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.Devices.Enumeration.DeviceInformationPairing _pairing;

        private DeviceInformationPairing(Windows.Devices.Enumeration.DeviceInformationPairing pairing)
        {
            _pairing = pairing;
        }

        public static implicit operator Windows.Devices.Enumeration.DeviceInformationPairing(DeviceInformationPairing pairing)
        {
            return pairing._pairing;
        }

        public static implicit operator DeviceInformationPairing(Windows.Devices.Enumeration.DeviceInformationPairing pairing)
        {
            return new DeviceInformationPairing(pairing);
        }

#elif WIN32
        private BLUETOOTH_DEVICE_INFO _deviceInfo;

        internal DeviceInformationPairing(BLUETOOTH_DEVICE_INFO info)
        {
            _deviceInfo = info;
        }
#endif

        /// <summary>
        /// Gets a value that indicates whether the device can be paired.
        /// </summary>
        /// <value>True if the device can be paired, otherwise false.</value>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Android</term><description>Android 4.4 and later</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
        /// </remarks>
        public bool CanPair
        {
            get
            {
#if WINDOWS_UWP
                return _pairing.CanPair;
#elif __ANDROID__ || WIN32
                return !IsPaired;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Gets the <see cref="DeviceInformationCustomPairing"/> object necessary for custom pairing.
        /// </summary>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
        /// </remarks>
        public DeviceInformationCustomPairing Custom
        {
            get
            {
#if WINDOWS_UWP
                return _pairing.Custom;
#elif WIN32
                return new DeviceInformationCustomPairing(_deviceInfo);
#else
                return null;
#endif              
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the device is currently paired.
        /// </summary>
        /// <value>True if the device is currently paired, otherwise false.</value>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Android</term><description>Android 4.4 and later</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
        /// </remarks>
        public bool IsPaired
        {
            get
            {
#if __ANDROID__
                return _device.BondState == Android.Bluetooth.Bond.Bonded;

#elif WINDOWS_UWP
                return _pairing.IsPaired;

#elif WIN32
                return _deviceInfo.fAuthenticated;

#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Attempts to pair the device.
        /// </summary>
        /// <returns>The result of the pairing action.</returns>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Android</term><description>Android 4.4 and later</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
        /// </remarks>
        public async Task<DevicePairingResult> PairAsync()
        {
#if __ANDROID__
            return new DevicePairingResult(_device.CreateBond());

#elif WINDOWS_UWP
            return await _pairing.PairAsync();

#elif WIN32
            int result = NativeMethods.BluetoothAuthenticateDevice(IntPtr.Zero, IntPtr.Zero, ref _deviceInfo, null, 0);

            if(result == 0)
            {
                _deviceInfo.fAuthenticated = true;
            }

            return new DevicePairingResult(result);

#else
            return null;
#endif
        }

        /// <summary>
        /// Attempts to unpair the device.
        /// </summary>
        /// <returns>The result of the unpairing action.</returns>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
        /// </remarks>
        public async Task<DeviceUnpairingResult> UnpairAsync()
        {
#if __ANDROID__
            return new DeviceUnpairingResult();

#elif WINDOWS_UWP
            return await _pairing.UnpairAsync();

#elif WIN32
            ulong addr = _deviceInfo.Address;
            int result = NativeMethods.BluetoothRemoveDevice(ref addr);
            return new DeviceUnpairingResult(result);
#else
            return null;
#endif
        }
    }
}