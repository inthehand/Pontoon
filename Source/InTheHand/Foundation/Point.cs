// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Point.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Foundation
{
    /// <summary>
    /// Represents x- and y-coordinate values that define a point in a two-dimensional plane.
    /// </summary>
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