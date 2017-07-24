// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PowerManager.Tizen.cs" company="In The Hand Ltd">
//   Copyright (c) 2016-17 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Tizen.System;

namespace InTheHand.System.Power
{
    partial class PowerManager
    {
        private static BatteryStatus GetBatteryStatus()
        {
            return Battery.IsCharging ? BatteryStatus.Charging : BatteryStatus.Discharging;
        }

        private static PowerSupplyStatus GetPowerSupplyStatus()
        {
            return Battery.IsCharging ? PowerSupplyStatus.Adequate : PowerSupplyStatus.NotPresent;
        }

        private static int GetRemainingChargePercent()
        {
            return Battery.Percent;
        }

        private static void Battery_PercentChanged(object sender, BatteryPercentChangedEventArgs e)
        {
            _remainingChargePercentChanged?.Invoke(null, null);
        }

        private static void RemainingChargePercentChangedAdd()
        {
            Battery.PercentChanged += Battery_PercentChanged;
        }

        private static void RemainingChargePercentChangedRemove()
        {
            Battery.PercentChanged -= Battery_PercentChanged;
        }
    }
}