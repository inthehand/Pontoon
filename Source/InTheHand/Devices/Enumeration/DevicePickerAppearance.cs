//-----------------------------------------------------------------------
// <copyright file="DevicePickerAppearance.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Windows.UI;
using Windows.UI.Xaml.Media;

namespace InTheHand.Devices.Enumeration
{
    /// <summary>
    /// Represents the appearance of a device picker.
    /// </summary>
    public sealed class DevicePickerAppearance
    {
#if WINDOWS_UWP
        private Windows.Devices.Enumeration.DevicePickerAppearance _appearance;

        private DevicePickerAppearance(Windows.Devices.Enumeration.DevicePickerAppearance appearance)
        {
            _appearance = appearance;
        }

        public static implicit operator Windows.Devices.Enumeration.DevicePickerAppearance(DevicePickerAppearance a)
        {
            return a._appearance;
        }

        public static implicit operator DevicePickerAppearance(Windows.Devices.Enumeration.DevicePickerAppearance a)
        {
            return new DevicePickerAppearance(a);
        }
#else
        public DevicePickerAppearance()
        {
#if WINDOWS_PHONE_APP
            AccentColor = ((SolidColorBrush)Windows.UI.Xaml.Application.Current.Resources["PhoneAccentBrush"]).Color;
            BackgroundColor = (Color)Windows.UI.Xaml.Application.Current.Resources["PhoneBackgroundColor"];
            ForegroundColor = (Color)Windows.UI.Xaml.Application.Current.Resources["PhoneForegroundColor"];
#elif WINDOWS_APP
            AccentColor = (Color)Windows.UI.Xaml.Application.Current.Resources["SystemColorControlAccentColor"];
            BackgroundColor = (Color)Windows.UI.Xaml.Application.Current.Resources["SystemColorWindowColor"];
            ForegroundColor = (Color)Windows.UI.Xaml.Application.Current.Resources["SystemColorWindowTextColor"];
#endif
        }
#endif

        /// <summary>
        /// Gets and sets the accent color of the picker UI.
        /// </summary>
        public Color AccentColor { get; set; }

        /// <summary>
        /// Gets and sets the background color of the picker UI.
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets and sets the foreground color of the picker UI.
        /// </summary>
        public Color ForegroundColor { get; set; }

        /// <summary>
        /// Gets and sets the accent color of the picker UI.
        /// </summary>
        public Color SelectedAccentColor { get; set; }

        /// <summary>
        /// Gets and sets the background color of the picker UI.
        /// </summary>
        public Color SelectedBackgroundColor { get; set; }

        /// <summary>
        /// Gets and sets the foreground color of the picker UI.
        /// </summary>
        public Color SelectedForegroundColor { get; set; }
    }
}