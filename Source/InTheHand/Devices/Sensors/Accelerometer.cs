// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Accelerometer.cs" company="In The Hand Ltd">
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
    /// Represents an accelerometer sensor.
    /// <para>This sensor returns G-force values with respect to the x, y, and z axes.</para></summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item></list>
    /// </remarks>
    public sealed class Accelerometer
    {
        /// <summary>
        /// Returns the default accelerometer.
        /// </summary>
        /// <returns></returns>
        public static Accelerometer GetDefault()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return Windows.Devices.Sensors.Accelerometer.GetDefault();
#elif __IOS__
            if(_default == null)
            {
                if (_manager.AccelerometerAvailable)
                {
                    _default = new Sensors.Accelerometer();
                }
            }

            return _default;
#else
            throw new PlatformNotSupportedException();
#endif
        }

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        private Windows.Devices.Sensors.Accelerometer _accelerometer;

        private Accelerometer(Windows.Devices.Sensors.Accelerometer accelerometer)
        {
            _accelerometer = accelerometer;
        }

        public static implicit operator Windows.Devices.Sensors.Accelerometer(Accelerometer a)
        {
            return a._accelerometer;
        }

        public static implicit operator Accelerometer(Windows.Devices.Sensors.Accelerometer a)
        {
            return new Sensors.Accelerometer(a);
        }

        private void _accelerometer_ReadingChanged(Windows.Devices.Sensors.Accelerometer sender, Windows.Devices.Sensors.AccelerometerReadingChangedEventArgs args)
        {
            _readingChanged?.Invoke(this, args);
        }

#elif __IOS__
        private static CMMotionManager _manager = new CMMotionManager();
        private static Accelerometer _default;
        internal static DateTimeOffset _timestampOffset = DateTimeOffset.MinValue;

        private Accelerometer()
        { }


        private void AccelerometerHandler(CoreMotion.CMAccelerometerData data, NSError error)
        {
            if (_timestampOffset == DateTimeOffset.MinValue)
            {
                _timestampOffset = DateTimeOffset.Now.Subtract(TimeSpan.FromSeconds(data.Timestamp));
            }
            global::System.Diagnostics.Debug.WriteLine(_timestampOffset);

            _readingChanged?.Invoke(this, new Sensors.AccelerometerReadingChangedEventArgs(data));
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
                return _accelerometer.ReportInterval;
#elif __IOS__
                return Convert.ToUInt32(_manager.AccelerometerUpdateInterval * 1000);
#else
                throw new PlatformNotSupportedException();
#endif
            }
            set
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                _accelerometer.ReportInterval = value;
#elif __IOS__
                _manager.AccelerometerUpdateInterval = value / 1000f;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets the current accelerometer reading.
        /// </summary>
        /// <returns></returns>
        public AccelerometerReading GetCurrentReading()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return _accelerometer.GetCurrentReading();
#elif __IOS__
            return _manager.AccelerometerData;
#else
            throw new PlatformNotSupportedException();
#endif
        }

        private event TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs> _readingChanged;

        /// <summary>
        /// Occurs each time the accelerometer reports a new sensor reading.
        /// </summary>
        public event TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs> ReadingChanged
        {
            add
            {
                if(_readingChanged == null)
                {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                    _accelerometer.ReadingChanged += _accelerometer_ReadingChanged;
#elif __IOS__
                    _manager.StartAccelerometerUpdates(NSOperationQueue.CurrentQueue, AccelerometerHandler);
#else
                    throw new PlatformNotSupportedException();
#endif
                }

                _readingChanged += value;
            }
            remove
            {
                _readingChanged -= value;

                if(_readingChanged == null)
                {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                    _accelerometer.ReadingChanged -= _accelerometer_ReadingChanged;
#elif __IOS__
                    _manager.StopAccelerometerUpdates();
#endif
                }
            }
        }


        //public event TypedEventHandler<Accelerometer, AccelerometerShakenEventArgs> Shaken;
    }
}
