// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Accelerometer.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using InTheHand.Foundation;

namespace InTheHand.Devices.Sensors
{
    /// <summary>
    /// Gives applications access to information about the environment in which they are running.
    /// The only supported property of this class is <see cref="DeviceType"/>, which is used to determine if an application is running on an actual Windows Phone device or on the device emulator on a PC.
    /// </summary>
    public sealed class Accelerometer
    {
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
#endif

        public AccelerometerReading GetCurrentReading()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return _accelerometer.GetCurrentReading();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        //public event TypedEventHandler<Accelerometer, AccelerometerShakenEventArgs> Shaken;
    }
}
