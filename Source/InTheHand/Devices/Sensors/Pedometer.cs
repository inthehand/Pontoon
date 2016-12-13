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
            if(CMPedometer.IsStepCountingAvailable)
            {
                _default = new Pedometer();
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
        private static Pedometer _default;

        private CMPedometer _pedometer;
        private global::System.Threading.EventWaitHandle _handle = new global::System.Threading.EventWaitHandle(false, global::System.Threading.EventResetMode.AutoReset);
        private CMPedometerData _lastData;

        private Pedometer()
        {
            _pedometer = new CMPedometer();
        }

        private void PedometerDataHandler(CMPedometerData data, NSError err)
        {
            _lastData = data;
            _handle.Set();
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
            Dictionary<PedometerStepKind, PedometerReading> readings = new Dictionary<PedometerStepKind, PedometerReading>();
            _lastData = null;
            _pedometer.QueryPedometerData(NSDate.FromTimeIntervalSince1970(DateTimeOffset.Now.Subtract(DateTimeOffset.Now.TimeOfDay).ToUnixTimeSeconds()), NSDate.FromTimeIntervalSince1970(DateTimeOffset.Now.ToUnixTimeSeconds()), PedometerDataHandler);
            _handle.WaitOne();
            if(_lastData != null)
            {
                readings.Add(PedometerStepKind.Unknown, new PedometerReading(_lastData.NumberOfSteps.Int32Value, TimeSpan.FromSeconds(_lastData.EndDate.SecondsSinceReferenceDate - _lastData.StartDate.SecondsSinceReferenceDate), PedometerStepKind.Unknown, DateTimeOffsetHelper.FromNSDate(_lastData.EndDate)));
            }

            return readings;
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
                    _pedometer.StartPedometerUpdates(DateTimeOffset.Now.ToNSDate(), PedometerDataHandler);
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
                    _pedometer.StopPedometerUpdates();
#elif TIZEN
                    _pedometer.DataUpdated -= _pedometer_DataUpdated;
#endif
                }
            }
        }
    }
}
