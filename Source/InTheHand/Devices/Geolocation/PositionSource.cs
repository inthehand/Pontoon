// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PositionSource.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Devices.Geolocation
{
    /// <summary>
    /// Indicates the source used to obtain a <see cref="Geocoordinate"/>. 
    /// </summary>
    public enum PositionSource
    {
        /// <summary>
        /// The position was obtained from cellular network data.
        /// </summary>
        Cellular = 0,

        /// <summary>
        /// The position was obtained from satellite data.
        /// </summary>
        Satellite = 1,

        /// <summary>
        /// The position was obtained from Wi-Fi network data.
        /// </summary>
        WiFi = 2,

        /// <summary>
        /// The position was obtained from an IP address.
        /// </summary>
        IPAddress = 3,

        /// <summary>
        /// The position was obtained from an unknown source.
        /// </summary>
        Unknown = 4,

        /// <summary>
        /// The position was obtained from the user's manually-set location.
        /// </summary>
        Default = 5,

        /// <summary>
        /// The position was obtained via the Consentless Location feature and was therefore intentionally made inaccurate to a degree.
        /// </summary>
        Obfuscated = 6,
    }
}