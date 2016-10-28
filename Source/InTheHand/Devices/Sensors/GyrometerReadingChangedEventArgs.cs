// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GyrometerReadingChangedEventArgs.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Sensors
{
    /// <summary>
    /// Provides data for the gyrometer reading–changed event.
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
    public sealed class GyrometerReadingChangedEventArgs
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        private Windows.Devices.Sensors.GyrometerReadingChangedEventArgs _args;

        private GyrometerReadingChangedEventArgs(Windows.Devices.Sensors.GyrometerReadingChangedEventArgs e)
        {
            _args = e;
        }

        public static implicit operator Windows.Devices.Sensors.GyrometerReadingChangedEventArgs(GyrometerReadingChangedEventArgs e)
        {
            return e._args;
        }

        public static implicit operator GyrometerReadingChangedEventArgs(Windows.Devices.Sensors.GyrometerReadingChangedEventArgs e)
        {
            return new GyrometerReadingChangedEventArgs(e);
        }
#else
        private GyrometerReading _reading;
        internal GyrometerReadingChangedEventArgs(GyrometerReading reading)
        {
            _reading = reading;
        }
#endif

        /// <summary>
        /// Gets the most recent gyrometer reading.
        /// </summary>
        public GyrometerReading Reading
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
