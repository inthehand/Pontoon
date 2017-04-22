// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Rect.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Foundation
{
    /// <summary>
    /// Contains number values that represent the location and size of a rectangle.
    /// </summary>
    public struct Rect
    {
        /// <summary>
        /// Initializes a Rect structure that has the specified x-coordinate, y-coordinate, width, and height. 
        /// </summary>
        /// <param name="x">The x-coordinate of the top-left corner of the rectangle.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        public Rect(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        public static implicit operator Windows.Foundation.Rect(Rect r)
        {
            return new Windows.Foundation.Rect(r.X, r.Y, r.Width, r.Height);
        }

        public static implicit operator Rect(Windows.Foundation.Rect r)
        {
            return new Rect(r.X, r.Y, r.Width, r.Height);
        }
#endif

        /// <summary>
        /// The x-coordinate location of the left side of the rectangle.
        /// </summary>
        public double X
        { get; set; }

        /// <summary>
        /// The y-coordinate location of the top side of the rectangle.
        /// </summary>
        public double Y
        { get; set; }

        /// <summary>
        /// A value that represents the Width of the rectangle.
        /// </summary>
        public double Width
        { get; set; }

        /// <summary>
        /// A value that represents the Height of the rectangle.
        /// </summary>
        public double Height
        { get; set; }
    }
}