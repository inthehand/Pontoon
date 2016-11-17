// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccelerometerReading.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Sensors
{
    /// <summary>
    /// Represents an accelerometer reading.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item></list>
    /// </remarks>
    public sealed class AccelerometerReading
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        private Windows.Devices.Sensors.AccelerometerReading _accelerometerReading;

        private AccelerometerReading(Windows.Devices.Sensors.AccelerometerReading accelerometerReading)
        {
            _accelerometerReading = accelerometerReading;
        }

        public static implicit operator Windows.Devices.Sensors.AccelerometerReading(AccelerometerReading ar)
        {
            return ar._accelerometerReading;
        }

        public static implicit operator AccelerometerReading(Windows.Devices.Sensors.AccelerometerReading ar)
        {
            return new AccelerometerReading(ar);
        }
#elif __IOS__
        private CoreMotion.CMAccelerometerData _accelerometerData;

        private AccelerometerReading(CoreMotion.CMAccelerometerData accelerometerData)
        {
            _accelerometerData = accelerometerData;
        }

        public static implicit operator CoreMotion.CMAccelerometerData(AccelerometerReading ar)
        {
            return ar._accelerometerData;
        }

        public static implicit operator AccelerometerReading(CoreMotion.CMAccelerometerData ad)
        {
            return new AccelerometerReading(ad);
        }
#elif TIZEN
        private float _x;
        private float _y;
        private float _z;
        private DateTimeOffset _timestamp;

        internal AccelerometerReading(float x, float y, float z, DateTimeOffset timestamp)
        {
            _x = x;
            _y = y;
            _z = z;
            _timestamp = timestamp;
        }
#endif

        /// <summary>
        /// Gets the g-force acceleration along the x-axis.
        /// </summary>
        public double AccelerationX
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _accelerometerReading.AccelerationX;
#elif __IOS__
                return _accelerometerData.Acceleration.X;
#elif TIZEN
                return _x;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets the g-force acceleration along the y-axis.
        /// </summary>
        public double AccelerationY
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _accelerometerReading.AccelerationY;
#elif __IOS__
                return _accelerometerData.Acceleration.Y;
#elif TIZEN
                return _y;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets the g-force acceleration along the z-axis.
        /// </summary>
        public double AccelerationZ
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _accelerometerReading.AccelerationZ;
#elif __IOS__
                return _accelerometerData.Acceleration.Z;
#elif TIZEN
                return _z;
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
                return _accelerometerReading.Timestamp;
#elif __IOS__
                //TODO: convert time since boot to actual timestamp
                return Accelerometer._timestampOffset.AddSeconds(_accelerometerData.Timestamp);
#elif TIZEN
                return _timestamp;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }
    }
}
