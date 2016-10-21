// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PositionSource.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.Devices.Geolocation.PositionSource))]
//#else

namespace InTheHand.Devices.Geolocation
{
    public enum PositionSource
    {
        Cellular,
        Satellite,
        WiFi,
        IPAddress,
        Unknown,
    }
}
//#endif