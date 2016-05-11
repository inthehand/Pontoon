//-----------------------------------------------------------------------
// <copyright file="DevicePickerAppearance.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
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
        public DevicePickerAppearance()
        {
#if WINDOWS_PHONE_APP
            AccentColor = ((SolidColorBrush)Windows.UI.Xaml.Application.Current.Resources["PhoneAccentBrush"]).Color;
            BackgroundColor = (Color)Windows.UI.Xaml.Application.Current.Resources["PhoneBackgroundColor"];
            ForegroundColor = (Color)Windows.UI.Xaml.Application.Current.Resources["PhoneForegroundColor"];
#else
            AccentColor = (Color)Windows.UI.Xaml.Application.Current.Resources["SystemColorControlAccentColor"];
            BackgroundColor = (Color)Windows.UI.Xaml.Application.Current.Resources["SystemColorWindowColor"];
            ForegroundColor = (Color)Windows.UI.Xaml.Application.Current.Resources["SystemColorWindowTextColor"];
#endif
        }
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