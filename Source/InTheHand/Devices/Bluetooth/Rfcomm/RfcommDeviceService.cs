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
    /// Represents an instance of a service on a Bluetooth device.
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
    }
}