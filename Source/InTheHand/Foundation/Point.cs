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
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        public static implicit operator Windows.Foundation.Point(Point p)
        {
            return new Windows.Foundation.Point(p.X, p.Y);
        }

        public static implicit operator Point(Windows.Foundation.Point p)
        {
            return new Point() { X = p.X, Y = p.Y };
        }
#endif
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