// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PedometerReadingChangedEventArgs.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Sensors
{
    /// <summary>
    /// Provides data for the pedometer reading–changed event.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item></list>
    /// </remarks>
    public sealed class PedometerReadingChangedEventArgs
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        private Windows.Devices.Sensors.PedometerReadingChangedEventArgs _args;

        private PedometerReadingChangedEventArgs(Windows.Devices.Sensors.PedometerReadingChangedEventArgs e)
        {
            _args = e;
        }

        public static implicit operator Windows.Devices.Sensors.PedometerReadingChangedEventArgs(PedometerReadingChangedEventArgs e)
        {
            return e._args;
        }

        public static implicit operator PedometerReadingChangedEventArgs(Windows.Devices.Sensors.PedometerReadingChangedEventArgs e)
        {
            return new PedometerReadingChangedEventArgs(e);
        }
#else
        private PedometerReading _reading;
        internal PedometerReadingChangedEventArgs(PedometerReading reading)
        {
            _reading = reading;
        }
#endif

        /// <summary>
        /// Gets the most recent pedometer reading.
        /// </summary>
        public PedometerReading Reading
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _args.Reading;
#else
                return _reading;
#endif
            }
        }
    }
}
