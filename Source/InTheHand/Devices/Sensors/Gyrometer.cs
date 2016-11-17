// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Gyrometer.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using InTheHand.Foundation;
#if __IOS__
using CoreMotion;
using Foundation;
#endif

namespace InTheHand.Devices.Sensors
{
    /// <summary>
    /// Represents a gyrometer sensor.
    /// <para>This sensor returns angular velocity values with respect to the x, y, and z axes.</para>
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
    public sealed class Gyrometer
    {
        /// <summary>
        /// Returns the default gyrometer.
        /// </summary>
        /// <returns></returns>
        public static Gyrometer GetDefault()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return Windows.Devices.Sensors.Gyrometer.GetDefault();
#elif __IOS__
            if(_default == null)
            {
                if (_manager.AccelerometerAvailable)
                {
                    _default = new Sensors.Gyrometer();
                }
            }

            return _default;
#elif TIZEN
            if (_default == null)
            {
                if (Tizen.Sensor.Gyroscope.IsSupported)
                {
                    _default = new Sensors.Gyrometer(new Tizen.Sensor.Gyroscope(0));
                }
            }

            return _default;
#else
            throw new PlatformNotSupportedException();
#endif
        }

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        private Windows.Devices.Sensors.Gyrometer _gyrometer;

        private Gyrometer(Windows.Devices.Sensors.Gyrometer gyrometer)
        {
            _gyrometer = gyrometer;
        }

        public static implicit operator Windows.Devices.Sensors.Gyrometer(Gyrometer g)
        {
            return g._gyrometer;
        }

        public static implicit operator Gyrometer(Windows.Devices.Sensors.Gyrometer g)
        {
            return new Sensors.Gyrometer(g);
        }

        private void _gyrometer_ReadingChanged(Windows.Devices.Sensors.Gyrometer sender, Windows.Devices.Sensors.GyrometerReadingChangedEventArgs args)
        {
            _readingChanged?.Invoke(this, args);
        }

#elif __IOS__
        private static CMMotionManager _manager = new CMMotionManager();
        private static Gyrometer _default;
        internal static DateTimeOffset _timestampOffset = DateTimeOffset.MinValue;

        private Gyrometer()
        { }


        private void GyrometerHandler(CoreMotion.CMGyroData data, NSError error)
        {
            if (_timestampOffset == DateTimeOffset.MinValue)
            {
                _timestampOffset = DateTimeOffset.Now.Subtract(TimeSpan.FromSeconds(data.Timestamp));
            }
            global::System.Diagnostics.Debug.WriteLine(_timestampOffset);

            _readingChanged?.Invoke(this, new Sensors.GyrometerReadingChangedEventArgs(data));
        }
#elif TIZEN
        private static Gyrometer _default;

        private Tizen.Sensor.Gyroscope _gyroscope;

        private Gyrometer(Tizen.Sensor.Gyroscope gyroscope)
        {
            _gyroscope = gyroscope;
        }


        private void _gyroscope_DataUpdated(object sender, Tizen.Sensor.GyroscopeDataUpdatedEventArgs e)
        {
            _readingChanged?.Invoke(this, new Sensors.GyrometerReadingChangedEventArgs(new Sensors.GyrometerReading(e.X, e.Y, e.Z, DateTimeOffset.Now)));
        }
#endif

        /// <summary>
        /// Gets or sets the current report interval for the accelerometer.
        /// </summary>
        /// <value>The current report interval.</value>
        /// <remarks>The report interval is specified in milliseconds.</remarks>
        public uint ReportInterval
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _gyrometer.ReportInterval;
#elif __IOS__
                return Convert.ToUInt32(_manager.GyroUpdateInterval * 1000);
#elif TIZEN
                return _gyroscope.Interval;
#else
                throw new PlatformNotSupportedException();
#endif
            }
            set
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                _gyrometer.ReportInterval = value;
#elif __IOS__
                _manager.GyroUpdateInterval = value / 1000f;
#elif TIZEN
                if (value < _gyroscope.MinInterval)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _gyroscope.Interval = value;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets the current gyrometer reading.
        /// </summary>
        /// <returns></returns>
        public GyrometerReading GetCurrentReading()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return _gyrometer.GetCurrentReading();
#elif __IOS__
            return _manager.GyroData;
#elif TIZEN
            return new GyrometerReading(_gyroscope.X, _gyroscope.Y, _gyroscope.Z, DateTimeOffset.Now);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        private event TypedEventHandler<Gyrometer, GyrometerReadingChangedEventArgs> _readingChanged;

        /// <summary>
        /// Occurs each time the gyrometer reports a new sensor reading.
        /// </summary>
        public event TypedEventHandler<Gyrometer, GyrometerReadingChangedEventArgs> ReadingChanged
        {
            add
            {
                if (_readingChanged == null)
                {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                    _gyrometer.ReadingChanged += _gyrometer_ReadingChanged;
#elif __IOS__
                    _manager.StartGyroUpdates(NSOperationQueue.CurrentQueue, GyrometerHandler);
#elif TIZEN
                    _gyroscope.DataUpdated += _gyroscope_DataUpdated;
#else
                    throw new PlatformNotSupportedException();
#endif
                }

                _readingChanged += value;
            }
            remove
            {
                _readingChanged -= value;

                if (_readingChanged == null)
                {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                    _gyrometer.ReadingChanged -= _gyrometer_ReadingChanged;
#elif __IOS__
                    _manager.StopGyroUpdates();
#elif TIZEN
                    _gyroscope.DataUpdated -= _gyroscope_DataUpdated;
#endif
                }
            }
        }

    }
}
