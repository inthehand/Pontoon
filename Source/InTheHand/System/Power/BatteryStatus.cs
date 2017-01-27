// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BatteryStatus.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.System.Power
{
    /// <summary>
    /// Indicates the status of the battery.
    /// </summary>
    public enum BatteryStatus
    {
        /// <summary>
        /// The battery or battery controller is not present.
        /// </summary>
        NotPresent = 0,

        /// <summary>
        /// The battery is discharging. 
        /// </summary>
        Discharging = 1,

        /// <summary>
        /// The battery is idle.
        /// </summary>
        Idle = 2,

        /// <summary>
        /// The battery is charging.
        /// </summary>
        Charging = 3,
    }
}