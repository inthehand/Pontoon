//-----------------------------------------------------------------------
// <copyright file="DevicePairingRequestedEventArgs.cs" company="In The Hand Ltd">
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
    public sealed class DevicePairingRequestedEventArgs
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.Devices.Enumeration.DevicePairingRequestedEventArgs _args;

        private DevicePairingRequestedEventArgs(Windows.Devices.Enumeration.DevicePairingRequestedEventArgs args)
        {
            _args = args;
        }

        public static implicit operator Windows.Devices.Enumeration.DevicePairingRequestedEventArgs(DevicePairingRequestedEventArgs args)
        {
            return args._args;
        }

        public static implicit operator DevicePairingRequestedEventArgs(Windows.Devices.Enumeration.DevicePairingRequestedEventArgs args)
        {
            return new DevicePairingRequestedEventArgs(args);
        }

#elif WIN32
        internal DevicePairingRequestedEventArgs(DeviceInformation deviceInformation, DevicePairingKinds pairingKind, string pin)
        {
            DeviceInformation = deviceInformation;
            PairingKind = pairingKind;
            Pin = pin;
        }
#endif

        public DeviceInformation DeviceInformation { get; private set; }

        public DevicePairingKinds PairingKind { get; private set; }

        public string Pin { get; private set; }

        public void Accept()
        {
            
        }

        public void Accept(string pin)
        {

        }
    }
}