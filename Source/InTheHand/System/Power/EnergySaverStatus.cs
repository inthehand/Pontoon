// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnergySaverStatus.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
//#if WINDOWS_UWP
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.System.Power.EnergySaverStatus))]
//#else

namespace InTheHand.System.Power
{
    /// <summary>
    /// Specifies the status of battery saver.
    /// </summary>
    public enum EnergySaverStatus
    {
        /// <summary>
        /// Battery saver is off permanently or the device is plugged in.
        /// </summary>
        Disabled = 0,

        /// <summary>
        /// Battery saver is off now, but ready to turn on automatically. 
        /// </summary>
        Off = 1,

        /// <summary>
        /// Battery saver is on. Save energy where possible. 
        /// </summary>
        On = 2,
    }
}
//#endif