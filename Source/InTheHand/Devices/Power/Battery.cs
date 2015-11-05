// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Battery.cs" company="In The Hand Ltd">
//   Copyright (c) 2015 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;

namespace InTheHand.Devices.Power
{
    /// <summary>
    /// Provides information about the status of the device's battery.
    /// <para>Not supported for apps deployed via the public Windows Store.</para>
    /// </summary>
    public sealed class Battery
    {
        private static Battery _default;
        /// <summary>
        /// Gets the default Battery object for the phone.
        /// </summary>
        /// <returns></returns>
        public static Battery GetDefault()
        {
            if (_default == null)
            {
                _default = new Battery();
#if WINDOWS_PHONE_APP
                _default._battery = Windows.Phone.Devices.Power.Battery.GetDefault();
#endif
            }

            return _default;
        }

#if __IOS__
        private UIKit.UIDevice _device;
        private bool _isSimulator;
#elif WINDOWS_PHONE_APP
        private Windows.Phone.Devices.Power.Battery _battery;
#endif

        private Battery()
        {
#if __IOS__
            _device = UIKit.UIDevice.CurrentDevice;
            if (_device.Model == "iPhone Simulator")
            {
                _isSimulator = true;
            }
#endif
        }

        /// <summary>
        /// Gets a value that indicates the percentage of the charge remaining on the phone's battery.
        /// <para>Not supported for apps deployed via the public Windows Store.</para>
        /// </summary>
        /// <value>A value from 0 to 100 that indicates the percentage of the charge remaining on the phone's battery.</value>
        public int RemainingChargePercent
        {
            get
            {
#if __IOS__
                if (_isSimulator)
                {
                    return 100;
                }

                double percent = _device.BatteryLevel;
                return Convert.ToInt32(percent * 100f);
#elif WINDOWS_APP
                //TODO: add reflection support for Windows 10

                NativeMethods.SYSTEM_POWER_STATUS status;
                bool success = NativeMethods.GetSystemPowerStatus(out status);
                return status.BatteryLifePercent;
#elif WINDOWS_PHONE_APP
                return _battery.RemainingChargePercent;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

#if !WINDOWS_APP
        private event EventHandler<object> _remainingChargePercentChanged;
#endif

        /// <summary>
        /// Occurs when the value of RemainingChargePercent decreases by 1%.
        /// </summary>
        public event EventHandler<object> RemainingChargePercentChanged
#if WINDOWS_APP
            ;
#else
        {
            add
            {
                if (_remainingChargePercentChanged == null)
                {
#if __IOS__
                    if(!_isSimulator && !_device.BatteryMonitoringEnabled)
                    {
                        _device.BatteryMonitoringEnabled = true;
                        UIKit.UIDevice.Notifications.ObserveBatteryLevelDidChange(BatteryLevelDidChangeHandler);
                    }
#elif WINDOWS_PHONE_APP
                    _battery.RemainingChargePercentChanged += _battery_RemainingChargePercentChanged;
#endif
                }
                _remainingChargePercentChanged += value;
            }
            remove
            {
                _remainingChargePercentChanged -= value;

                if(_remainingChargePercentChanged == null)
                {
#if __IOS__
                    _device.BatteryMonitoringEnabled = false;
#elif WINDOWS_PHONE_APP
                    _battery.RemainingChargePercentChanged -= _battery_RemainingChargePercentChanged;
#endif
                }
            }
        }
#endif

#if WINDOWS_PHONE_APP
        private void _battery_RemainingChargePercentChanged(object sender, object e)
        {
            if(_remainingChargePercentChanged != null)
            {
                _remainingChargePercentChanged(this, e);
            }
        }
#elif __IOS__
        private void BatteryLevelDidChangeHandler(object sender, Foundation.NSNotificationEventArgs e)
        {
            if(_remainingChargePercentChanged != null)
            {
                _remainingChargePercentChanged(this, null);
            }
        }
#endif


#if WINDOWS_APP
        /// <summary>
        /// Gets a value that estimates how long is left until the device's battery is fully discharged.
        /// <para>Not supported for apps deployed via the public Windows Store.</para>
        /// </summary>
        public System.TimeSpan RemainingDischargeTime
        {
            get
            {
                NativeMethods.SYSTEM_POWER_STATUS status;
                bool success = NativeMethods.GetSystemPowerStatus(out status);
                int seconds = status.BatteryLifeTime;
                if(seconds == -1)
                {
                    return TimeSpan.Zero;
                }

                return TimeSpan.FromSeconds(seconds);
            }
        }

        private static class NativeMethods
        {
            [System.Runtime.InteropServices.DllImport("kernel32.dll")]
            internal static extern bool GetSystemPowerStatus(out SYSTEM_POWER_STATUS lpSystemPowerStatus);

            internal struct SYSTEM_POWER_STATUS
            {
                public byte ACLineStatus;
                public byte BatteryFlag;
                public byte BatteryLifePercent;
                public byte Reserved1;            // set to 0
                public int BatteryLifeTime;
                public int BatteryFullLifeTime;
            }
        }
#endif
    }
}
