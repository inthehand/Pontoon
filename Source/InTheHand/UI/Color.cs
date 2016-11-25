//-----------------------------------------------------------------------
// <copyright file="Color.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.UI
{
    /// <summary>
    /// Describes a color in terms of alpha, red, green, and blue channels.
    /// </summary>
    public struct Color
    {
        /// <summary>
        /// Creates a new <see cref="Color"/> structure by using the specified sRGB alpha channel and color channel values.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color FromArgb(byte a, byte r, byte g, byte b)
        {
            return new Color { A = a, B = b, G = g, R = r };
        }
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        public static implicit operator Windows.UI.Color(Color c)
        {
            return Windows.UI.Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        public static implicit operator Color(Windows.UI.Color c)
        {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }
#elif WIN32
        public static Color FromCOLORREF(uint color)
        {
            return Color.FromArgb(0xff, (byte)(color & 0xff), (byte)(color & 0xff00 >> 8), (byte)(color & 0xff0000 >> 16));
        }
#endif

        /// <summary>
        /// Compares two <see cref="Color"/> structures for equality.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> structure to compare to this <see cref="Color"/>.</param>
        /// <returns></returns>
        public bool Equals(Color color)
        {
            return this.A == color.A && this.B == color.B && this.G == color.G && this.R == color.R;
        }

        /// <summary>
        /// Gets or sets the sRGB alpha channel value of the color.
        /// </summary>
        public byte A { get; set; }

        /// <summary>
        /// Gets or sets the sRGB blue channel value of the color.
        /// </summary>
        public byte B { get; set; }

        /// <summary>
        /// Gets or sets the sRGB green channel value of the color.
        /// </summary>
        public byte G { get; set; }

        /// <summary>
        /// Gets or sets the sRGB red channel value of the color.
        /// </summary>
        public byte R { get; set; }
    }
}