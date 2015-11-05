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

        private DisplayInformation()
        {          
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
#if __ANDROID__
                if(!rawDpiY.HasValue)
                {
                    rawDpiY = Platform.Android.ContextManager.Context.Resources.DisplayMetrics.Ydpi;
                }

                return rawDpiY.Value;
#else
                return RawDpiX;
#endif
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
#elif WINDOWS_PHONE
                    int scaleFactor = System.Windows.Application.Current.Host.Content.ScaleFactor;

                    object temp;
                    if (Microsoft.Phone.Info.DeviceExtendedProperties.TryGetValue("PhysicalScreenResolution", out temp))
                    {
                        System.Windows.Size screenResolution = (System.Windows.Size)temp;
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
