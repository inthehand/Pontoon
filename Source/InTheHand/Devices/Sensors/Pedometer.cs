// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Pedometer.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using InTheHand.Foundation;
using System.Threading.Tasks;
using System.Collections.Generic;
#if __IOS__
using CoreMotion;
using Foundation;
#elif TIZEN
using Tizen.Sensor;
#endif

namespace InTheHand.Devices.Sensors
{
    /// <summary>
    /// Provides an interface for a pedometer to measure the number of steps taken.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item></list>
    /// </remarks>
    public sealed class Pedometer
    {
        /// <summary>
        /// Returns the default accelerometer.
        /// </summary>
        /// <returns></returns>
        public static async Task<Pedometer> GetDefaultAsync()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            var p = await Windows.Devices.Sensors.Pedometer.GetDefaultAsync();
            return p == null ? null : new Pedometer(p);
#elif __IOS__
            if(_default == null)
            {
                if (_manager.AccelerometerAvailable)
                {
                    _default = new Sensors.Accelerometer();
                }
            }

            return _default;
#elif TIZEN
            if (_default == null)
            {
                if (Tizen.Sensor.Pedometer.IsSupported)
                {
                    _default = new Pedometer(new Tizen.Sensor.Pedometer(0));
                }
            }

            return _default;
#else
            throw new PlatformNotSupportedException();
#endif
        }

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        private Windows.Devices.Sensors.Pedometer _pedometer;

        private Pedometer(Windows.Devices.Sensors.Pedometer pedometer)
        {
            _pedometer = pedometer;
        }

        public static implicit operator Windows.Devices.Sensors.Pedometer(Pedometer p)
        {
            return p._pedometer;
        }

        public static implicit operator Pedometer(Windows.Devices.Sensors.Pedometer p)
        {
            return new Pedometer(p);
        }

        private void _pedometer_ReadingChanged(Windows.Devices.Sensors.Pedometer sender, Windows.Devices.Sensors.PedometerReadingChangedEventArgs args)
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
#elif TIZEN
        private static Pedometer _default;

        private Tizen.Sensor.Pedometer _pedometer;

        private Pedometer(Tizen.Sensor.Pedometer pedometer)
        {
            _pedometer = pedometer;
        }
        
        private void _pedometer_DataUpdated(object sender, PedometerDataUpdatedEventArgs e)
        {
            _readingChanged?.Invoke(this, new PedometerReadingChangedEventArgs(new PedometerReading( (int)e.StepCount, TimeSpan.Zero, PedometerStepKindHelper.FromState(e.LastStepStatus), DateTimeOffset.Now)));
        }
#endif

        /// <summary>
        /// Gets or sets the current report interval for the pedometer.
        /// </summary>
        /// <value>The current report interval.</value>
        /// <remarks>The report interval is specified in milliseconds.</remarks>
        public uint ReportInterval
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _pedometer.ReportInterval;
#elif __IOS__
                return Convert.ToUInt32(_manager.AccelerometerUpdateInterval * 1000);
#elif TIZEN
                return _pedometer.Interval;
#else
                throw new PlatformNotSupportedException();
#endif
            }
            set
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                _pedometer.ReportInterval = value;
#elif __IOS__
                _manager.AccelerometerUpdateInterval = value / 1000f;
#elif TIZEN
                if(value < _pedometer.MinInterval)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _pedometer.Interval = value;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets the current step information from the pedometer sensor.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyDictionary<PedometerStepKind, PedometerReading> GetCurrentReadings()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            var rawReadings = _pedometer.GetCurrentReadings();
            Dictionary<PedometerStepKind, PedometerReading> readings = new Dictionary<Sensors.PedometerStepKind, Sensors.PedometerReading>();
            foreach(KeyValuePair<Windows.Devices.Sensors.PedometerStepKind, Windows.Devices.Sensors.PedometerReading> r in rawReadings)
            {
                readings.Add((PedometerStepKind)((int)r.Key), r.Value);
            }

            return readings;
#elif __IOS__
            return _manager.AccelerometerData;
#elif TIZEN
            Dictionary<PedometerStepKind, PedometerReading> readings = new Dictionary<Sensors.PedometerStepKind, Sensors.PedometerReading>();
            readings.Add(PedometerStepKind.Running, new PedometerReading((int)_pedometer.RunStepCount, TimeSpan.Zero, PedometerStepKind.Running, DateTimeOffset.Now));
            readings.Add(PedometerStepKind.Walking, new PedometerReading((int)_pedometer.WalkStepCount, TimeSpan.Zero, PedometerStepKind.Walking, DateTimeOffset.Now));
            return readings;
#else
            throw new PlatformNotSupportedException();
#endif
        }

        private event TypedEventHandler<Pedometer, PedometerReadingChangedEventArgs> _readingChanged;

        /// <summary>
        /// Occurs each time the pedometer reports a new value.
        /// </summary>
        public event TypedEventHandler<Pedometer, PedometerReadingChangedEventArgs> ReadingChanged
        {
            add
            {
                if(_readingChanged == null)
                {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                    _pedometer.ReadingChanged += _pedometer_ReadingChanged;
#elif __IOS__
                    _manager.StartAccelerometerUpdates(NSOperationQueue.CurrentQueue, AccelerometerHandler);
#elif TIZEN
                    _pedometer.DataUpdated += _pedometer_DataUpdated;
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
                    _pedometer.ReadingChanged -= _pedometer_ReadingChanged;
#elif __IOS__
                    _manager.StopAccelerometerUpdates();
#elif TIZEN
                    _pedometer.DataUpdated -= _pedometer_DataUpdated;
#endif
                }
            }
        }
    }
}
