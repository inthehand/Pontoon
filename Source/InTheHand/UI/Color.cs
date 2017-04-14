//-----------------------------------------------------------------------
// <copyright file="Color.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.UI
{
    /// <summary>
    /// Describes a color in terms of alpha, red, green, and blue channels.
    /// Can be implicitly converted into a variety of platform-specific types.
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader><term>Platform</term><description>Platform Type</description></listheader>
    /// <item><term>Android</term><description>Android.Graphics.Color</description></item>
    /// <item><term>iOS, tvOS, macOS, watchOS</term><description>CoreGraphics.CGColor</description></item>
    /// <item><term>iOS, tvOS, macOS</term><description>CoreImage.CIColor</description></item>
    /// <item><term>iOS, tvOS</term><description>UIKit.UIColor</description></item>
    /// <item><term>macOS</term><description>AppKit.NSColor</description></item>
    /// <item><term>Windows UWP, Windows Store, Windows Phone Store</term><description>Windows.UI.Color</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>System.Windows.Media.Color</description></item></list>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>watchOS</term><description>watchOS 2.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public struct Color
    {
        /// <summary>
        /// Creates a new <see cref="Color"/> structure by using the specified sRGB color channel values.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color FromArgb(byte r, byte g, byte b)
        {
            return FromArgb(0xff, r, g, b);
        }

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
#if __ANDROID__
        public static implicit operator Android.Graphics.Color(Color c)
        {
            return new Android.Graphics.Color(c.R, c.G, c.B, c.A);
        }

        public static implicit operator Color(Android.Graphics.Color c)
        {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }
#elif __UNIFIED__

        public static implicit operator CoreGraphics.CGColor(Color c)
        {
            return new CoreGraphics.CGColor(CoreGraphics.CGColorSpace.CreateGenericRgb(), new nfloat[] { c.R / 255, c.G / 255, c.B / 255, c.A / 255 });
        }

        public static implicit operator Color(CoreGraphics.CGColor c)
        {
            // TODO: test against colors created for other color spaces
            return Color.FromArgb(FloatComponentToByte(c.Alpha), FloatComponentToByte(c.Components[0]), FloatComponentToByte(c.Components[1]), FloatComponentToByte(c.Components[2]));
        }
#if !__WATCHOS__
        public static implicit operator CoreImage.CIColor(Color c)
        {
            return new CoreImage.CIColor(c.R / 255, c.G / 255, c.B / 255, c.A / 255);
        }


        public static implicit operator Color(CoreImage.CIColor c)
        {
            return Color.FromArgb(FloatComponentToByte(c.Alpha), FloatComponentToByte(c.Red), FloatComponentToByte(c.Green), FloatComponentToByte(c.Blue));
        }
#endif

#if __IOS__ || __TVOS__
        public static implicit operator UIKit.UIColor(Color c)
        {
            return new UIKit.UIColor(c.R / 255, c.G / 255, c.B / 255, c.A / 255);
        }

        public static implicit operator Color(UIKit.UIColor c)
        {
            nfloat r,g,b,a;
            c.GetRGBA(out r, out g, out b, out a);
            return Color.FromArgb(FloatComponentToByte(a), FloatComponentToByte(r), FloatComponentToByte(g), FloatComponentToByte(b));
        }
#elif __MAC__
        public static implicit operator AppKit.NSColor(Color c)
        {
            return AppKit.NSColor.FromCalibratedRgba(c.R, c.G, c.B, c.A);
        }

        public static implicit operator Color(AppKit.NSColor c)
        {
            nfloat r,g,b,a;
            c.GetRgba(out r, out g, out b, out a);
            return Color.FromArgb(FloatComponentToByte(a), FloatComponentToByte(r), FloatComponentToByte(g), FloatComponentToByte(b));
        }
#endif

        private static byte FloatComponentToByte(nfloat value)
        {
            return (byte)(MathHelper.Clamp(value, 0.0, 1.0) * 255.0);
        }

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        public static implicit operator Windows.UI.Color(Color c)
        {
            return Windows.UI.Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        public static implicit operator Color(Windows.UI.Color c)
        {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }

#if WINDOWS_PHONE
        public static implicit operator global::System.Windows.Media.Color(Color c)
        {
            return global::System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        public static implicit operator Color(global::System.Windows.Media.Color c)
        {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }
#endif

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
            return A == color.A && B == color.B && G == color.G && R == color.R;
        }

        /// <summary>
        /// Returns the color value as a UInt32 of the form 0xBBGGRRAA.
        /// </summary>
        /// <returns></returns>
        public uint ToArgb()
        {
            return (uint)(A << 24 | R << 16 | G << 8 | B);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "#" + A.ToString("X2") + R.ToString("X2") + G.ToString("X2") + B.ToString("X2");
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