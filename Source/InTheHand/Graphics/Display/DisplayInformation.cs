//-----------------------------------------------------------------------
// <copyright file="DisplayInformation.cs" company="In The Hand Ltd">
//     Copyright © 2013-15 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Graphics.Display
{
    /// <summary>
    /// Monitors and controls physical display information.
    /// The class provides events to allow clients to monitor for changes in the display.
    /// </summary>
    public sealed class DisplayInformation
    {
        private static DisplayInformation current;

        /// <summary>
        /// Gets the current physical display information.
        /// </summary>
        /// <returns></returns>
        public static DisplayInformation GetForCurrentView()
        {
            if(current == null)
            {
                current = new DisplayInformation();
            }

            return current;
        }

#if WINDOWS_APP || WINDOWS_UWP || WINDOWS_PHONE_APP
        private Windows.Graphics.Display.DisplayInformation _displayInformation;
#endif

        private DisplayInformation()
        {
#if WINDOWS_APP || WINDOWS_UWP || WINDOWS_PHONE_APP
            _displayInformation = Windows.Graphics.Display.DisplayInformation.GetForCurrentView();
#endif
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
                    rawDpiX = Platform.Android.ContextManager.Context.Resources.DisplayMetrics.Xdpi;
#elif WINDOWS_APP || WINDOWS_UWP || WINDOWS_PHONE_APP
                    rawDpiX = _displayInformation.RawDpiX;
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
                    rawDpiY = Platform.Android.ContextManager.Context.Resources.DisplayMetrics.Ydpi;
               

                return rawDpiY.Value;
#elif WINDOWS_APP || WINDOWS_UWP || WINDOWS_PHONE_APP
                    rawDpiY = _displayInformation.RawDpiY;
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
                    _rawPixelsPerViewPixel = Platform.Android.ContextManager.Context.Resources.DisplayMetrics.Density;
#elif WINDOWS_UWP || WINDOWS_PHONE_APP
                    _rawPixelsPerViewPixel = _displayInformation.RawPixelsPerViewPixel;
#elif WINDOWS_APP || WINDOWS_PHONE
                    _rawPixelsPerViewPixel = ((int)ResolutionScale) / 100.0;
#endif
                }

                return _rawPixelsPerViewPixel.Value;
            }
        }

        private ResolutionScale? resolutionScale;
        /// <summary>
        /// Gets the scale factor of the immersive environment.
        /// </summary>
        public ResolutionScale ResolutionScale
        {
            get
            {
                if (!resolutionScale.HasValue)
                {
#if __ANDROID__
                    float scale = Platform.Android.ContextManager.Context.Resources.DisplayMetrics.Density;
                    resolutionScale = (ResolutionScale)((int)(scale * 100));
#elif WINDOWS_APP || WINDOWS_UWP || WINDOWS_PHONE_APP
                    resolutionScale = (ResolutionScale)((int)_displayInformation.ResolutionScale);
#elif WINDOWS_PHONE
                    int scaleFactor = global::System.Windows.Application.Current.Host.Content.ScaleFactor;

                    object temp;
                    if (Microsoft.Phone.Info.DeviceExtendedProperties.TryGetValue("PhysicalScreenResolution", out temp))
                    {
                        global::System.Windows.Size screenResolution = (global::System.Windows.Size)temp;
                        scaleFactor = (int)((screenResolution.Width / 480d) * 100d);
                    }

                    resolutionScale = (ResolutionScale)scaleFactor;
#else
                    resolutionScale = ResolutionScale.Invalid;
#endif
                }

                return resolutionScale.Value;
            }
        }
    }
}
