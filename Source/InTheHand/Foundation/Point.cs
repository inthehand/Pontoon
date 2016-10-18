// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Point.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
[assembly:global::System.Runtime.CompilerServices.TypeForwardedTo(typeof(Windows.Foundation.Point))]
#else

using Windows.Foundation.Metadata;

namespace Windows.Foundation
{
    /// <summary>
    /// Represents x- and y-coordinate values that define a point in a two-dimensional plane.
    /// </summary>
    [ContractVersion(typeof(FoundationContract), 65536U)]
    public struct Point
    {
        /// <summary>
        /// The horizontal position of the point.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// The vertical position of the point.
        /// </summary>
        public double Y { get; set; }
    }
}
#endif