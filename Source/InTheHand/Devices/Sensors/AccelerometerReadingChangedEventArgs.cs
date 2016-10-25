// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccelerometerReadingChangedEventArgs.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Sensors
{
    /// <summary>
    /// Provides data for the accelerometer reading–changed event.
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
    public sealed class AccelerometerReadingChangedEventArgs
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        private Windows.Devices.Sensors.AccelerometerReadingChangedEventArgs _args;

        private AccelerometerReadingChangedEventArgs(Windows.Devices.Sensors.AccelerometerReadingChangedEventArgs e)
        {
            _args = e;
        }

        public static implicit operator Windows.Devices.Sensors.AccelerometerReadingChangedEventArgs(AccelerometerReadingChangedEventArgs e)
        {
            return e._args;
        }

        public static implicit operator AccelerometerReadingChangedEventArgs(Windows.Devices.Sensors.AccelerometerReadingChangedEventArgs e)
        {
            return new AccelerometerReadingChangedEventArgs(e);
        }
#else
        private AccelerometerReading _reading;
        internal AccelerometerReadingChangedEventArgs(AccelerometerReading reading)
        {
            _reading = reading;
        }
#endif

        /// <summary>
        /// Gets the most recent accelerometer reading.
        /// </summary>
        public AccelerometerReading Reading
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
