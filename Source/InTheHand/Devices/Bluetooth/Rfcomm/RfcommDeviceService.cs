//-----------------------------------------------------------------------
// <copyright file="RfcommDeviceService.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Bluetooth.Rfcomm
{
    /// <summary>
    /// Represents an instance of a service on a remote Bluetooth device.
    /// </summary>
    public sealed class RfcommDeviceService
    {
        /// <summary>
        /// Gets an Advanced Query Syntax (AQS) string for identifying instances of an RFCOMM service.
        /// </summary>
        /// <param name="serviceId">The service id for which to query.</param>
        /// <returns>An AQS string for identifying RFCOMM service instances.</returns>
        public static string GetDeviceSelector(RfcommServiceId serviceId)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return Windows.Devices.Bluetooth.Rfcomm.RfcommDeviceService.GetDeviceSelector(serviceId);

#elif WIN32
            return serviceId.Uuid.ToString();

#else
            return string.Empty;
#endif
        }

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private Windows.Devices.Bluetooth.Rfcomm.RfcommDeviceService _service;

        private RfcommDeviceService(Windows.Devices.Bluetooth.Rfcomm.RfcommDeviceService service)
        {
            _service = service;
        }

        public static implicit operator Windows.Devices.Bluetooth.Rfcomm.RfcommDeviceService(RfcommDeviceService service)
        {
            return service._service;
        }

        public static implicit operator RfcommDeviceService(Windows.Devices.Bluetooth.Rfcomm.RfcommDeviceService service)
        {
            return new RfcommDeviceService(service);
        }

#else
        private RfcommServiceId _id;
#endif

        /// <summary>
        /// Gets the RfcommServiceId of this RFCOMM service instance.
        /// </summary>
        /// <value>The RfcommServiceId of the RFCOMM service instance.</value>
        public RfcommServiceId ServiceId
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _service.ServiceId;

#else
                return _id;
#endif
            }
        }
    }
}