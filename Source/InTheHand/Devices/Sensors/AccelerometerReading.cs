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
                return DateTimeOffset.Now;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }
    }
}
