//-----------------------------------------------------------------------
// <copyright file="Radio.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
#if __UNIFIED__
using CoreBluetooth;
using Foundation;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using Windows.Devices.Radios;
#endif

namespace InTheHand.Devices.Radios
{
    /// <summary>
    /// Represents a radio device on the system.
    /// </summary>
    public sealed class Radio
    {
#if WINDOWS_UWP || WINDOWS_PHONE_APP
        private Windows.Devices.Radios.Radio _radio;

        private Radio(Windows.Devices.Radios.Radio radio)
        {
            _radio = radio;
        }

        public static implicit operator Windows.Devices.Radios.Radio(Radio radio)
        {
            return radio._radio;
        }

        public static implicit operator Radio(Windows.Devices.Radios.Radio radio)
        {
            return new Radio(radio);
        }
#elif WIN32
        private static Type s_type10 = Type.GetType("Windows.Devices.Radios.Radio, Windows, ContentType=WindowsRuntime");
        private object _object10 = null;
#elif __UNIFIED__
        private CBCentralManager _manager;

        internal Radio(CBCentralManager manager)
        {
            _manager = manager;
        }
#endif


        /// <summary>
        /// A static method that retrieves a Radio object corresponding to a device Id obtained through FindAllAsync(System.String) and related APIs.
        /// </summary>
        /// <param name="deviceId">A string that identifies a particular radio device.</param>
        /// <returns>An asynchronous retrieval operation.
        /// On successful completion, it contains a <see cref="Radio"/> object that represents the specified radio device.</returns>
        public static async Task<Radio> FromIdAsync(string deviceId)
        {
#if __UNIFIED__
            return null;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return await Windows.Devices.Radios.Radio.FromIdAsync(deviceId);
#else
            return null;
#endif
        }

        /// <summary>
        /// A static method that returns an Advanced Query Syntax (AQS) string to be used to enumerate or monitor Radio devices with FindAllAsync(System.String) and related methods.
        /// </summary>
        /// <returns>An identifier to be used to enumerate radio devices.</returns>
        public static string GetDeviceSelector()
        {
#if __UNIFIED__
            return "radio";
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return Windows.Devices.Radios.Radio.GetDeviceSelector();
#elif WIN32
            if(s_type10 != null)
            {
                return s_type10.GetMethod(nameof(GetDeviceSelector), BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[0]).ToString();
            }

            return string.Empty;
#else
            return string.Empty;
#endif
        }

        /// <summary>
        /// A static, asynchronous method that retrieves a collection of <see cref="Radio"/> objects representing radio devices existing on the system.
        /// </summary>
        /// <returns>An asynchronous retrieval operation. When the operation is complete, contains a list of Radio objects describing available radios.</returns>
        public static async Task<IReadOnlyList<Radio>> GetRadiosAsync()
        {
            List<Radio> radios = new List<Radio>();
#if __UNIFIED__
            radios.Add(new Radio(new CBCentralManager()));
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            foreach (Windows.Devices.Radios.Radio r in await Windows.Devices.Radios.Radio.GetRadiosAsync())
            {
                radios.Add(r);
            }
#endif
            return radios.AsReadOnly();
        }

        /// <summary>
        /// An asynchronous method that retrieves a value indicating what access the current user has to the radio represented by this object.
        /// In circumstances where user permission is required to access the radio, this method prompts the user for permission.
        /// Consequently, always call this method on the UI thread.
        /// </summary>
        /// <returns></returns>
        public static async Task<RadioAccessStatus> RequestAccessAsync()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return (RadioAccessStatus)((int)await Windows.Devices.Radios.Radio.RequestAccessAsync());
#else
            return RadioAccessStatus.Unspecified;
#endif
        }

        public async Task<RadioAccessStatus> SetStateAsync(RadioState value)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return (RadioAccessStatus)((int)await _radio.SetStateAsync((Windows.Devices.Radios.RadioState)((int)value)));
#else
            return RadioAccessStatus.Unspecified;
#endif
        }

        /// <summary>
        /// Gets an enumeration value that describes what kind of radio this object represents.
        /// </summary>
        /// <value>The kind of this radio.</value>
        public RadioKind Kind
        {
            get
            {
#if __UNIFIED__
                return RadioKind.Bluetooth;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return (RadioKind)((int)_radio.Kind);
#else
                return RadioKind.Other;
#endif
            }
        }

        /// <summary>
        /// Gets the name of the radio represented by this object.
        /// </summary>
        /// <value>The radio name.</value>
        public string Name
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _radio.Name;
#else
                return string.Empty;
#endif
            }
        }

        /// <summary>
        /// Gets the current state of the radio represented by this object.
        /// </summary>
        /// <value>The current radio state.</value>
        public RadioState State
        {
            get
            {
#if __UNIFIED__
                return CBCentalManagerStateToRadioState(_manager.State);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return (RadioState)((int)_radio.State);
#else
                return RadioState.Unknown;
#endif
            }
        }

#if __UNIFIED__
        private static RadioState CBCentalManagerStateToRadioState(CBCentralManagerState state)
        {
            switch(state)
            {
                case CBCentralManagerState.PoweredOn:
                    return RadioState.On;

                case CBCentralManagerState.PoweredOff:
                    return RadioState.Off;

                case CBCentralManagerState.Unauthorized:
                    return RadioState.Disabled;

                default:
                    return RadioState.Unknown;
            }
        }
#endif
    }
}