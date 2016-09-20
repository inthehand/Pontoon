// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PositionAccuracy.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Devices.Geolocation.PositionAccuracy))]
#else

namespace Windows.Devices.Geolocation
{
    /// <summary>
    /// Indicates the requested accuracy level for the location data that the application uses.
    /// </summary>
    public enum PositionAccuracy
    {
        /// <summary>
        /// Optimize for power, performance, and other cost considerations. 
        /// </summary>
        Default = 0,

        /// <summary>
        /// Deliver the most accurate report possible.
        /// This includes using services that might charge money, or consuming higher levels of battery power or connection bandwidth.
        /// An accuracy level of High may degrade system performance and should be used only when necessary. 
        /// </summary>
        High = 1,
    }
}
#endif