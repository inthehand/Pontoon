//-----------------------------------------------------------------------
// <copyright file="DisplayInformation.cs" company="In The Hand Ltd">
//     Copyright © 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.Graphics.Display.DisplayInformation))]
//#else
#if __IOS__
using UIKit;
#endif

using System;
using System.Windows;

namespace InTheHand.Graphics.Display
{
    /// <summary>
    /// Monitors and controls physical display information.
    /// The class provides events to allow clients to monitor for changes in the display.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item></list>
    /// </remarks>
    public sealed class DisplayInformation
    {
        private static DisplayInformation current;

        /// <summary>
        /// Gets the current physical display information.
        /// </summary>
        /// <returns></returns>
        public static DisplayInformation GetForCurrentView()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return new Display.DisplayInformation(Windows.Graphics.Display.DisplayInformation.GetForCurrentView());
#else
            if (current == null)
            {
                current = new DisplayInformation();
            }

            return current;
#endif
        }

#if __ANDROID__
        private Android.Util.DisplayMetrics _metrics;
#elif __IOS__
        private UIScreen _screen;
        
        public static implicit operator UIScreen(DisplayInformation d)
        {
            return d._screen;
        }
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        private Windows.Graphics.Display.DisplayInformation _information;

        private DisplayInformation(Windows.Graphics.Display.DisplayInformation information)
        {
            _information = information;
        }

        public static implicit operator Windows.Graphics.Display.DisplayInformation(DisplayInformation d)
        {
            return d._information;
        }
#endif

        private DisplayInformation()
        {
#if __ANDROID__
            _metrics = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.Resources.DisplayMetrics;
#elif __IOS__
            _screen = UIApplication.SharedApplication.KeyWindow.Screen;
#endif
        }

        /// <summary>
        /// Gets the current orientation of a rectangular monitor.
        /// </summary>
        public DisplayOrientations CurrentOrientation
        {
            get
            {
#if __ANDROID__
                return _metrics.WidthPixels > _metrics.HeightPixels ? DisplayOrientations.Landscape : DisplayOrientations.Portrait;
#elif __IOS__
                return _screen.Bounds.Width > _screen.Bounds.Height ? DisplayOrientations.Landscape : DisplayOrientations.Portrait;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return (DisplayOrientations)((int)_information.CurrentOrientation);
#elif WINDOWS_PHONE
                return Application.Current.Host.Content.ActualWidth > Application.Current.Host.Content.ActualHeight ? DisplayOrientations.Landscape : DisplayOrientations.Portrait;
#else
                return DisplayOrientations.None;
#endif
            }
        }

        private float? rawDpiX;

        /// <summary>
        /// Gets the raw dots per inch (DPI) along the x axis of the display monitor.
        /// </summary>
        public float RawDpiX
        {
            get
            {
                if(!rawDpiX.HasValue)
                {
#if __ANDROID__
                    rawDpiX = _metrics.Xdpi;
#elif __IOS__
                    rawDpiX = (float?)_screen.NativeScale;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                    return _information.RawDpiX;
#elif WINDOWS_PHONE
                    object temp;
                    if(Microsoft.Phone.Info.DeviceExtendedProperties.TryGetValue("RawDpiX", out temp))
                    {
                        rawDpiX = (float)((double)temp);
                    }
                    else
                    {
                        rawDpiX = 0f;
                    }
#else
                    rawDpiX = 0f;
#endif
                }

                return rawDpiX.Value;
            }
        }

        private float? rawDpiY;
        /// <summary>
        /// Gets the raw dots per inch (DPI) along the y axis of the display monitor.
        /// </summary>
        public float RawDpiY
        {
            get
            {

                if(!rawDpiY.HasValue)
                {
#if __ANDROID__
                    rawDpiY = _metrics.Ydpi;
#elif __IOS__
                    rawDpiY = (float?)_screen.NativeScale;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                    return _information.RawDpiY;
#elif WINDOWS_PHONE
                    object temp;
                    if (Microsoft.Phone.Info.DeviceExtendedProperties.TryGetValue("RawDpiY", out temp))
                    {
                        rawDpiY = (float)((double)temp);
                    }
                    else
                    {
                        rawDpiY = 0f;
                    }
#else
                    rawDpiY = 0f;
#endif
                }

                return rawDpiY.Value;
            }
        }

        private double? _rawPixelsPerViewPixel;
        /// <summary>
        /// Gets a value representing the number of raw (physical) pixels for each view (layout) pixel.
        /// </summary>
        public double RawPixelsPerViewPixel
        {
            get
            {
                if(!_rawPixelsPerViewPixel.HasValue)
                {
#if __ANDROID__
                    _rawPixelsPerViewPixel = _metrics.Density;
#elif __IOS__
                    _rawPixelsPerViewPixel = (float?)_screen.Scale;
#elif WINDOWS_UWP || WINDOWS_PHONE_APP
                    return _information.RawPixelsPerViewPixel;
#elif WINDOWS_APP
                    return (int)_information.ResolutionScale / 100;
#elif WINDOWS_PHONE
                    int scaleFactor = global::System.Windows.Application.Current.Host.Content.ScaleFactor;

                    object temp;
                    if (Microsoft.Phone.Info.DeviceExtendedProperties.TryGetValue("PhysicalScreenResolution", out temp))
                    {
                        global::System.Windows.Size screenResolution = (global::System.Windows.Size)temp;
                        _rawPixelsPerViewPixel = screenResolution.Width / 480d;
                    }
#else
                    _rawPixelsPerViewPixel = 1;
#endif
                }

                return _rawPixelsPerViewPixel.Value;
            }
        }
    }
}
//#endif