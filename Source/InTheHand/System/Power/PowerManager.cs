// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PowerManager.cs" company="In The Hand Ltd">
//   Copyright (c) 2016-17 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;

namespace InTheHand.System.Power
{
    /// <summary>
    /// Provides information about the status of the device's battery.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 10 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public static partial class PowerManager
    {

#if WINDOWS_APP
        private static Type _type10 = null;
#elif WINDOWS_PHONE_APP || WINDOWS_PHONE
        private static Windows.Phone.Devices.Power.Battery _battery = Windows.Phone.Devices.Power.Battery.GetDefault();
#endif

#if WINDOWS_APP
        static PowerManager()
        {
            //check for 10
           _type10 = Type.GetType("Windows.System.Power.PowerManager, Windows, ContentType=WindowsRuntime");
        }
#endif
        /// <summary>
        /// Gets the device's battery status.
        /// </summary>
        /// <remarks>The device's battery status.</remarks>
        public static BatteryStatus BatteryStatus
        {
            get
            {
#if __ANDROID__ || __IOS__ || TIZEN || WIN32 || WINDOWS_UWP
                return GetBatteryStatus();

#elif WINDOWS_APP
                if(_type10 != null)
                {
                    object val = _type10.GetRuntimeProperty("BatteryStatus")?.GetValue(null);
                    if(val != null)
                    {
                        return (BatteryStatus)((int)val);
                    }
                }

                return BatteryStatus.NotPresent;
#elif WINDOWS_PHONE
                switch (Microsoft.Phone.Info.DeviceStatus.PowerSource)
                {
                    case Microsoft.Phone.Info.PowerSource.External:
                        return BatteryStatus.Charging;

                    default:
                        return BatteryStatus.Discharging;
                }

#else
                return BatteryStatus.Idle;
#endif
            }
        }

        /// <summary>
        /// Gets battery saver status, indicating when to save energy.
        /// </summary>
        public static EnergySaverStatus EnergySaverStatus
        {
            get
            {
#if __IOS__ || __ANDROID__ || WIN32 || WINDOWS_UWP
                return GetEnergySaverStatus();
                
#elif WINDOWS_APP
                if (_type10 != null)
                {
                    object val = _type10.GetRuntimeProperty("EnergySaverStatus")?.GetValue(null);
                    if (val != null)
                    {
                        return (EnergySaverStatus)((int)val);
                    }
                }

#elif WINDOWS_PHONE_81 || WINDOWS_PHONE_APP
                bool saverEnabled = Windows.Phone.System.Power.PowerManager.PowerSavingModeEnabled;
                if (saverEnabled)
                {
                    bool saverOn = Windows.Phone.System.Power.PowerManager.PowerSavingMode == Windows.Phone.System.Power.PowerSavingMode.On;
                    return saverOn ? EnergySaverStatus.On : EnergySaverStatus.Off;
                }  
                
#endif
                return EnergySaverStatus.Disabled;
            }
        }

        /// <summary>
        /// Gets the device's power supply status.
        /// </summary>
        public static PowerSupplyStatus PowerSupplyStatus
        {
            get
            {
#if __ANDROID__ || __IOS__ || TIZEN || WIN32 || WINDOWS_UWP
                return GetPowerSupplyStatus();

#elif WINDOWS_APP
                if (_type10 != null)
                {
                    object val = _type10.GetRuntimeProperty("PowerSupplyStatus")?.GetValue(null);
                    if (val != null)
                    {
                        return (PowerSupplyStatus)((int)val);
                    }
                }

#elif WINDOWS_PHONE
                if (Microsoft.Phone.Info.DeviceStatus.PowerSource == Microsoft.Phone.Info.PowerSource.External)
                    return PowerSupplyStatus.Adequate;
#endif
                return PowerSupplyStatus.NotPresent;
            }
        }

 
        /// <summary>
        /// Gets the total percentage of charge remaining from all batteries connected to the device.
        /// <para>Not supported for Windows 8.1 apps deployed via the public Windows Store.</para>
        /// </summary>
        /// <value>The total percentage of charge remaining from all batteries connected to the device.</value>
        public static int RemainingChargePercent
        {
            get
            {
#if __ANDROID__ || __IOS__ || TIZEN || WIN32 || WINDOWS_UWP
                return GetRemainingChargePercent();
                                
#elif WINDOWS_APP
                if (_type10 != null)
                {
                    return (int)_type10.GetRuntimeProperty("RemainingChargePercent").GetValue(null);
                }
                else
                {
                    return 0;
                }

#elif WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _battery.RemainingChargePercent;

#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

#if !WINDOWS_APP
        private static event EventHandler<object> _remainingChargePercentChanged;
#endif

        /// <summary>
        /// Occurs when <see cref="RemainingChargePercent"/>changes.
        /// </summary>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
        /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
        /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item></list>
        /// </remarks>
        public static event EventHandler<object> RemainingChargePercentChanged
#if WINDOWS_APP
            ;
#else
        {
            add
            {
                if (_remainingChargePercentChanged == null)
                {
#if __IOS__ || WINDOWS_UWP || TIZEN
                    RemainingChargePercentChangedAdd();

#elif WINDOWS_PHONE_APP || WINDOWS_PHONE
                    _battery.RemainingChargePercentChanged += _battery_RemainingChargePercentChanged;
#endif
                }
                _remainingChargePercentChanged += value;
            }
            remove
            {
                _remainingChargePercentChanged -= value;

                if (_remainingChargePercentChanged == null)
                {
#if TIZEN || WINDOWS_UWP
                    RemainingChargePercentChangedRemove();

#elif WINDOWS_PHONE_APP || WINDOWS_PHONE
                    _battery.RemainingChargePercentChanged -= _battery_RemainingChargePercentChanged;
#endif
                }
            }
        }
        
#endif

#if WINDOWS_PHONE_APP || WINDOWS_PHONE
        private static void _battery_RemainingChargePercentChanged(object sender, object e)
        {
            if (_remainingChargePercentChanged != null)
            {
                _remainingChargePercentChanged(null, e);
            }
        }
#endif


        /// <summary>
        /// Gets a value that estimates how long is left until the device's battery is fully discharged.
        /// <para>Not supported for apps deployed on Windows 8.1 from the public Windows Store.</para>
        /// </summary>
        public static TimeSpan RemainingDischargeTime
        {
            get
            {
               
#if WINDOWS_APP
                if (_type10 != null)
                {
                    return (TimeSpan)_type10.GetRuntimeProperty("RemainingDischargeTime").GetValue(null);
                }

#elif WINDOWS_PHONE
                return _battery.RemainingDischargeTime;

#elif WIN32
                return GetRemainingDischargeTime();  
                
#endif

                return TimeSpan.Zero;
            }
        }
    }
}