
namespace InTheHand.Devices
{
    /// <summary>
    /// Gives applications access to information about the environment in which they are running.
    /// The only supported property of this class is <see cref="DeviceType"/>, which is used to determine if an application is running on an actual Windows Phone device or on the device emulator on a PC.
    /// </summary>
    internal static class Environment
    {
        private static DeviceType? _deviceType;

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
#if __IOS__
                    _deviceType = UIKit.UIDevice.CurrentDevice.Name.IndexOf("Simulator") > -1 ? DeviceType.Emulator : DeviceType.Device;
#elif WINDOWS_PHONE_APP
                    Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation di = new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation();
                    _deviceType = (di.SystemSku == "Microsoft Virtual") ? DeviceType.Emulator : Devices.DeviceType.Device;
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
