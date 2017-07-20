// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PowerManager.Android.cs" company="In The Hand Ltd">
//   Copyright (c) 2016-17 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Android.Content;
using Android.OS;

namespace InTheHand.System.Power
{
    partial class PowerManager
    {
        private static BatteryManager _batteryManager;
        private static Android.OS.PowerManager _powerManager;

        private static BatteryStatus GetBatteryStatus()
        {
            return _batteryManager.IsCharging ? BatteryStatus.Charging : BatteryStatus.Discharging;
        }

        private static EnergySaverStatus GetEnergySaverStatus()
        {
            return _powerManager.IsPowerSaveMode ? EnergySaverStatus.On : EnergySaverStatus.Off;
        }

        private static PowerSupplyStatus GetPowerSupplyStatus()
        {
            // partial implementation
            return _batteryManager.IsCharging ? PowerSupplyStatus.Adequate : PowerSupplyStatus.NotPresent;
        }

        private static int GetRemainingChargePercent()
        {
            return _batteryManager.GetIntProperty((int)BatteryProperty.Capacity);
        }
    }
}