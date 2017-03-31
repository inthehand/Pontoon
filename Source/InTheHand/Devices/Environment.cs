// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Environment.cs" company="In The Hand Ltd">
//   Copyright (c) 2016-17 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace InTheHand.Devices
{
    /// <summary>
    /// Gives applications access to information about the environment in which they are running.
    /// The only supported property of this class is <see cref="DeviceType"/>, which is used to determine if an application is running on an actual Windows Phone device or on the device emulator on a PC.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item></list>
    /// </remarks>
    internal static class Environment
    {
        private static DeviceType? _deviceType = null;

        /// <summary>
        /// Gets the type of device on which the application is running.
        /// Use this property to determine if your application is running on an actual device or on the device emulator.
        /// </summary>
        public static DeviceType DeviceType
        {
            get
            {
                if (!_deviceType.HasValue)
                {
#if __IOS__ || __TVOS__
                    _deviceType = UIKit.UIDevice.CurrentDevice.Name.IndexOf("Simulator") > -1 ? DeviceType.Emulator : DeviceType.Device;

#elif WINDOWS_UWP || WINDOWS_PHONE_APP
                    Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation di = new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation();
                    _deviceType = (di.SystemSku == "Microsoft Virtual") ? DeviceType.Emulator : Devices.DeviceType.Device;

#elif WINDOWS_PHONE
                    _deviceType = Microsoft.Devices.Environment.DeviceType == Microsoft.Devices.DeviceType.Device ? DeviceType.Device : DeviceType.Emulator;

#else
                    throw new PlatformNotSupportedException();
#endif
                }

                return _deviceType.Value;
            }
        }
    }

    /// <summary>
    /// Defines the device type values used by the <see cref="Environment.DeviceType"/> property.
    /// </summary>
    internal enum DeviceType
    {
        /// <summary>
        /// The device type is an actual device. 
        /// </summary>
        Device,

        /// <summary>
        /// The device type is a device emulator. 
        /// </summary>
        Emulator,
    }
}
