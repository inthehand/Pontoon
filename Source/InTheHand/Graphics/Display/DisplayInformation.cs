//-----------------------------------------------------------------------
// <copyright file="DisplayInformation.cs" company="In The Hand Ltd">
//     Copyright © 2013-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if __IOS__ || __TVOS__
using UIKit;
#endif

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
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public sealed partial class DisplayInformation
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
#elif __MAC__ || WIN32
            return GetForCurrentViewImpl();
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
#elif __IOS__ || __TVOS__
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
#elif __IOS__ || __TVOS__
            _screen = UIApplication.SharedApplication.KeyWindow.Screen;
#endif
        }


        /// <summary>
        /// Gets the current orientation of a rectangular monitor.
        /// </summary>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Android</term><description>Android 4.4 and later</description></item>
        /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
        /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
        /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
        /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
        /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
        /// </remarks>
        public DisplayOrientations CurrentOrientation
        {
            get
            {
#if __ANDROID__
                return _metrics.WidthPixels > _metrics.HeightPixels ? DisplayOrientations.Landscape : DisplayOrientations.Portrait;
#elif __IOS__ || __TVOS__
                return _screen.Bounds.Width > _screen.Bounds.Height ? DisplayOrientations.Landscape : DisplayOrientations.Portrait;
#elif __MAC__
                return GetCurrentOrientation();
#elif TIZEN
                int orientation;

                if(Tizen.System.SystemInfo.TryGetValue("DEVICE_ORIENTATION", out orientation))
                {
                    switch(orientation)
                    {
                        case 0:
                            return DisplayOrientations.Portrait;

                        case 1:
                            return DisplayOrientations.PortraitFlipped;

                        case 2:
                            return DisplayOrientations.Landscape;

                        case 3:
                            return DisplayOrientations.LandscapeFlipped;
                    }
                }

                return DisplayOrientations.None;

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return (DisplayOrientations)((int)_information.CurrentOrientation);
#elif WINDOWS_PHONE
                return global::System.Windows.Application.Current.Host.Content.ActualWidth > global::System.Windows.Application.Current.Host.Content.ActualHeight ? DisplayOrientations.Landscape : DisplayOrientations.Portrait;
#elif WIN32
                return _orientation;
#else
                return DisplayOrientations.None;
#endif
            }
        }

        private float? rawDpiX;

        /// <summary>
        /// Gets the raw dots per inch (DPI) along the x axis of the display monitor.
        /// </summary>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Android</term><description>Android 4.4 and later</description></item>
        /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
        /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
        /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
        /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
        /// </remarks>
        public float RawDpiX
        {
            get
            {
                if(!rawDpiX.HasValue)
                {
#if __ANDROID__
                    rawDpiX = _metrics.Xdpi;
#elif __IOS__ || __TVOS__
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
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Android</term><description>Android 4.4 and later</description></item>
        /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
        /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
        /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
        /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
        /// </remarks>
        public float RawDpiY
        {
            get
            {

                if(!rawDpiY.HasValue)
                {
#if __ANDROID__
                    rawDpiY = _metrics.Ydpi;
#elif __IOS__ || __TVOS__
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
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Android</term><description>Android 4.4 and later</description></item>
        /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
        /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
        /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
        /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item></list>
        /// </remarks>
        public double RawPixelsPerViewPixel
        {
            get
            {
                if(!_rawPixelsPerViewPixel.HasValue)
                {
#if __ANDROID__
                    _rawPixelsPerViewPixel = _metrics.Density;
#elif __IOS__ || __TVOS__
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