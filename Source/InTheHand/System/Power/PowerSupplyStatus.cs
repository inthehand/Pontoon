// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PowerSupplyStatus.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
//#if WINDOWS_UWP
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.System.Power.PowerSupplyStatus))]
//#else

namespace InTheHand.System.Power
{
    /// <summary>
    /// Represents the device's power supply status.
    /// </summary>
    /// <remarks>An Inadequate status occurs when the power supply is present, but the charge rate is negative.
    /// For example, the device is plugged in, but it’s losing charge.</remarks>
    public enum PowerSupplyStatus
    {
        /// <summary>
        /// The device has no power supply. 
        /// </summary>
        NotPresent = 0,

        /// <summary>
        /// The device has an inadequate power supply. 
        /// </summary>
        Inadequate = 1,

        /// <summary>
        /// The device has an adequate power supply. 
        /// </summary>
        Adequate = 2,
    }
}
//#endif