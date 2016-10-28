// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GyrometerReading.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Sensors
{
    /// <summary>
    /// Represents a gyrometer reading.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item></list>
    /// </remarks>
    public sealed class GyrometerReading
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        private Windows.Devices.Sensors.GyrometerReading _gyrometerReading;

        private GyrometerReading(Windows.Devices.Sensors.GyrometerReading gyrometerReading)
        {
            _gyrometerReading = gyrometerReading;
        }

        public static implicit operator Windows.Devices.Sensors.GyrometerReading(GyrometerReading gr)
        {
            return gr._gyrometerReading;
        }

        public static implicit operator GyrometerReading(Windows.Devices.Sensors.GyrometerReading gr)
        {
            return new GyrometerReading(gr);
        }
#elif __IOS__
        private CoreMotion.CMGyroData _gyrometerData;

        private GyrometerReading(CoreMotion.CMGyroData gyrometerData)
        {
            _gyrometerData = gyrometerData;
        }

        public static implicit operator CoreMotion.CMGyroData(GyrometerReading gr)
        {
            return gr._gyrometerData;
        }

        public static implicit operator GyrometerReading(CoreMotion.CMGyroData gd)
        {
            return new GyrometerReading(gd);
        }
#endif

        /// <summary>
        /// Gets the angular velocity, in degrees per second, about the x-axis.
        /// </summary>
        public double AngularVelocityX
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _gyrometerReading.AngularVelocityX;
#elif __IOS__
                return _gyrometerData.RotationRate.x;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets the angular velocity, in degrees per second, about the y-axis.
        /// </summary>
        public double AngularVelocityY
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _gyrometerReading.AngularVelocityY;
#elif __IOS__
                return _gyrometerData.RotationRate.y;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets the angular velocity, in degrees per second, about the z-axis.
        /// </summary>
        public double AngularVelocityZ
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _gyrometerReading.AngularVelocityZ;
#elif __IOS__
                return _gyrometerData.RotationRate.z;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets the time at which the sensor reported the reading.
        /// </summary>
        public DateTimeOffset Timestamp
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _gyrometerReading.Timestamp;
#elif __IOS__
                //TODO: convert time since boot to actual timestamp
                return Gyrometer._timestampOffset.AddSeconds(_gyrometerData.Timestamp);
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }
    }
}
