// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EasClientDeviceInformation.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#if __ANDROID__
using Android.OS;
#elif __IOS__
using Foundation;
using UIKit;
#elif WINDOWS_PHONE
using Microsoft.Phone.Info;
#endif
using System;

namespace InTheHand.Security.ExchangeActivesyncProvisioning
{
    /// <summary>
    /// Provides the app the ability to retrieve device information from the local device.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item></list>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// </remarks>
    public sealed class EasClientDeviceInformation
    {
#if __IOS__
        private UIDevice _device;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        private Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation _deviceInformation;

        public static implicit operator Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation(EasClientDeviceInformation i)
        {
            return i._deviceInformation;
        }
#endif

        /// <summary>
        /// Creates an instance of an object that allows the caller app to retrieve device information from the local device.
        /// </summary>
        public EasClientDeviceInformation()
        {
#if __IOS__
            _device = UIDevice.CurrentDevice;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            _deviceInformation = new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation();
#endif
        }

        /// <summary>
        /// Returns the friendly name of the local device.
        /// </summary>
        public string FriendlyName
        {
            get
            {
#if __IOS__
                return _device.Name;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _deviceInformation.FriendlyName;
#elif WINDOWS_PHONE
                return Windows.Networking.Proximity.PeerFinder.DisplayName;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Returns the identifier of the local device.
        /// </summary>
        /// <remarks>For Windows Phone (Silverlight) apps you must declare the ID_CAP_IDENTITY_DEVICE capability in your WMAppManifest.</remarks>
        public Guid Id
        {
            get
            {
#if __IOS__
                return new Guid(_device.IdentifierForVendor.GetBytes());
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _deviceInformation.Id;
#elif WINDOWS_PHONE
                byte[] raw = (byte[])Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceUniqueId");
                byte[] trimmed = new byte[16];
                Buffer.BlockCopy(raw, 0, trimmed, 0, 16);
                return new Guid(trimmed);
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Returns the operating system of the local device.
        /// </summary>
        public string OperatingSystem
        {
            get
            {
#if __IOS__
                return "Apple iOS";
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _deviceInformation.OperatingSystem;
#elif WINDOWS_PHONE
                return "Windows Phone";
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Returns the system manufacturer of the local device. Use this only if the SystemSku is empty. 
        /// </summary>
        public string SystemManufacturer
        {
            get
            {
#if __IOS__
                return "Apple";
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _deviceInformation.SystemManufacturer;
#elif WINDOWS_PHONE
                return DeviceStatus.DeviceManufacturer;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }


        /// <summary>
        /// Returns the system product name of the local device. 
        /// </summary>
        public string SystemProductName
        {
            get
            {
#if __IOS__
                return _device.Model;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _deviceInformation.SystemProductName;
#elif WINDOWS_PHONE
                return DeviceStatus.DeviceName;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

    }
}