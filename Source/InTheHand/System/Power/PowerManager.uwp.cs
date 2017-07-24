// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PowerManager.uwp.cs" company="In The Hand Ltd">
//   Copyright (c) 2016-17 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace InTheHand.System.Power
{
    partial class PowerManager
    {
        private static BatteryStatus GetBatteryStatus()
        {
            return (BatteryStatus)((int)Windows.System.Power.PowerManager.BatteryStatus);
        }

        private static EnergySaverStatus GetEnergySaverStatus()
        {
            return (EnergySaverStatus)((int)Windows.System.Power.PowerManager.EnergySaverStatus);
        }

        private static PowerSupplyStatus GetPowerSupplyStatus()
        {
            return (PowerSupplyStatus)((int)Windows.System.Power.PowerManager.PowerSupplyStatus);
        }

        private static int GetRemainingChargePercent()
        {
            return Windows.System.Power.PowerManager.RemainingChargePercent;
        }

        private static void PowerManager_RemainingChargePercentChanged(object sender, object e)
        {
            _remainingChargePercentChanged?.Invoke(null, null);
        }

        private static void RemainingChargePercentChangedAdd()
        {
            Windows.System.Power.PowerManager.RemainingChargePercentChanged += PowerManager_RemainingChargePercentChanged;
        }

        private static void RemainingChargePercentChangedRemove()
        {
            Windows.System.Power.PowerManager.RemainingChargePercentChanged -= PowerManager_RemainingChargePercentChanged;
        }

        private static TimeSpan GetRemainingDischargeTime()
        {
            return Windows.System.Power.PowerManager.RemainingDischargeTime;
        }
        
    }
}