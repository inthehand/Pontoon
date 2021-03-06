﻿//-----------------------------------------------------------------------
// <copyright file="UISettings.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
#if WIN32
using System.Runtime.InteropServices;
#elif __TVOS__
using UIKit;
#endif

namespace InTheHand.UI.ViewManagement
{
    /// <summary>
    /// Contains a set of common app user interface settings and operations.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10 Mobile</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list></remarks>
    public sealed class UISettings
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.UI.ViewManagement.UISettings _settings = new Windows.UI.ViewManagement.UISettings();
#endif

        /// <summary>
        /// Returns the color value of the specified color type.
        /// </summary>
        /// <param name="desiredColor">An enumeration value that specifies the type of color to get a value for.</param>
        /// <returns>The color value of the specified color type.</returns>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10 Mobile</description></item>
        /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
        /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
        /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list></remarks>
        public Color GetColorValue(UIColorType desiredColor)
        {
#if WINDOWS_UWP
            return _settings.GetColorValue((Windows.UI.ViewManagement.UIColorType)((int)desiredColor));
#elif WINDOWS_APP
            string key = null;
            switch (desiredColor)
            {
                case UIColorType.Foreground:
                    key = "ApplicationForegroundThemeBrush";
                    break;

                case UIColorType.Background:
                    key = "ApplicationPageBackgroundThemeBrush";
                    break;

                case UIColorType.Accent:
                    key = "ListBoxItemSelectedBackgroundThemeBrush";
                    break;
            }

            if (key != null)
            {
                Windows.UI.Color c = ((Windows.UI.Xaml.Media.SolidColorBrush)Windows.UI.Xaml.Application.Current.Resources[key]).Color;
                return c;
            }

            return Color.FromArgb(0, 0, 0, 0);

#elif WINDOWS_PHONE_APP
            string key = null;
            switch(desiredColor)
            {
                case UIColorType.Foreground:
                    key = "PhoneForegroundColor";
                    break;

                case UIColorType.Background:
                    key = "PhoneBackgroundColor";
                    break;

                case UIColorType.Accent:
                    key = "SystemColorControlAccentColor";
                    break;
            }

            if(key!=null)
            {
                Windows.UI.Color c =  (Windows.UI.Color)Windows.UI.Xaml.Application.Current.Resources[key];
                return c;
            }

            return Color.FromArgb(0, 0, 0, 0);

#elif WINDOWS_PHONE
            string key = null;
            switch(desiredColor)
            {
                case UIColorType.Foreground:
                    key = "PhoneForegroundColor";
                    break;

                case UIColorType.Background:
                    key = "PhoneBackgroundColor";
                    break;

                case UIColorType.Accent:
                    key = "PhoneAccentColor";
                    break;
            }

            if(key!=null)
            {
                global::System.Windows.Media.Color c =  (global::System.Windows.Media.Color)global::System.Windows.Application.Current.Resources[key];
                return c;
            }

            return Color.FromArgb(0, 0, 0, 0);

#elif WIN32
            uint color = NativeMethods.GetSysColor(UIColorTypeToCOLOR(desiredColor));
            return Color.FromCOLORREF(color);

#elif __TVOS__
            bool isDark = UIScreen.MainScreen.TraitCollection.UserInterfaceStyle == UIUserInterfaceStyle.Dark;
            switch(desiredColor)
            {
                case UIColorType.Foreground:
                    return isDark ? Color.FromArgb(0xff, 0xff, 0xff) : Color.FromArgb(0,0,0);

                case UIColorType.Background:
                    return isDark ? Color.FromArgb(0, 0, 0) : Color.FromArgb(0xff, 0xff, 0xff);

                case UIColorType.Accent:
                    return UIScreen.MainScreen.FocusedView?.TintColor;
            }

            return Color.FromArgb(0, 0, 0, 0);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the color used for a specific user interface element type, such as a button face or window text.
        /// </summary>
        /// <param name="desiredElement">The type of element for which the color will be obtained.</param>
        /// <returns>The color of the element type, expressed as a 32-bit color value.</returns>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Windows UWP</term><description>Windows 10 Mobile</description></item>
        /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
        /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
        /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list></remarks>
        public Color UIElementColor(UIElementType desiredElement)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return _settings.UIElementColor((Windows.UI.ViewManagement.UIElementType)((int)desiredElement));
#elif WIN32
            uint color = NativeMethods.GetSysColor(UIElementTypeToCOLOR(desiredElement));
            return Color.FromCOLORREF(color);
#else
            throw new PlatformNotSupportedException();
#endif
        }

#if WIN32

        private static int UIColorTypeToCOLOR(UIColorType t)
        {
            switch(t)
            {
                case UIColorType.Background:
                    return 5;

                case UIColorType.Foreground:
                    return 8;

                case UIColorType.Accent:
                    return 13;
            }

            return 0;
        }

        private static int UIElementTypeToCOLOR(UIElementType t)
        {
            switch (t)
            {
                case UIElementType.ActiveCaption:
                    return 2;

                case UIElementType.Background:
                    return 1;

                case UIElementType.ButtonFace:
                    return 15;

                case UIElementType.ButtonText:
                    return 18;

                case UIElementType.CaptionText:
                    return 9;

                case UIElementType.GrayText:
                    return 17;

                case UIElementType.Highlight:
                    return 13;

                case UIElementType.HighlightText:
                    return 14;

                case UIElementType.Hotlight:
                    return 26;

                case UIElementType.InactiveCaption:
                    return 3;

                case UIElementType.InactiveCaptionText:
                    return 19;

                case UIElementType.Window:
                    return 5;

                case UIElementType.WindowText:
                    return 8;
            }

            return 0;
        }

        private static class NativeMethods
        {
            [DllImport("User32")]
            internal static extern uint GetSysColor(int nIndex);

            [DllImport("User32")]
            internal static extern uint GetSystemMetrics(int nIndex);
        }
#endif
    }
}