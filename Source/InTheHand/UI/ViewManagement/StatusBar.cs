//-----------------------------------------------------------------------
// <copyright file="StatusBar.cs" company="In The Hand Ltd">
//     Copyright © 2015-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

//#if WINDOWS_UWP || WINDOWS_PHONE_APP
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.UI.ViewManagement.StatusBar))]
//#else
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
#if __ANDROID__
    using Android.App;
    using Android.Content;
#elif __IOS__
    using UIKit;
#elif WINDOWS_APP
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
#endif

namespace InTheHand.UI.ViewManagement
{
    /// <summary>
    /// Provides methods and properties for interacting with the status bar on a window (app view).
    /// </summary>
    public sealed class StatusBar
    {
        /// <summary>
        /// Gets the status bar for the current window (app view).
        /// </summary>
        /// <returns></returns>
        public static StatusBar GetForCurrentView()
        {
            if (_statusBar == null)
            {
                _statusBar = new StatusBar();
            }

            return _statusBar;
        }

        private static StatusBar _statusBar;

        private StatusBar()
        {
        }

#if WINDOWS_PHONE_APP || WINDOWS_UWP
        public Windows.UI.Color? BackgroundColor {
            get
            {
                return ((Windows.UI.ViewManagement.StatusBar)_statusBar).BackgroundColor;
            }
            set
            {
                ((Windows.UI.ViewManagement.StatusBar)_statusBar).BackgroundColor = value;
            }
        }

        public double BackgroundOpacity
        {
            get
            {
                return ((Windows.UI.ViewManagement.StatusBar)_statusBar).BackgroundOpacity;
            }
            set
            {
                ((Windows.UI.ViewManagement.StatusBar)_statusBar).BackgroundOpacity = value;
            }
        }

        public Windows.UI.Color? ForegroundColor
        {
            get
            {
                return ((Windows.UI.ViewManagement.StatusBar)_statusBar).ForegroundColor;
            }
            set
            {
                ((Windows.UI.ViewManagement.StatusBar)_statusBar).ForegroundColor = value;
            }
        }
#endif

        /// <summary>
        /// Gets the progress indicator for the status bar.
        /// </summary>
        /// <value>The progress indicator for the status bar.</value>
        public StatusBarProgressIndicator ProgressIndicator
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_PHONE_APP
                return new StatusBarProgressIndicator(_statusBar.ProgressIndicator);
#elif WINDOWS_PHONE
                return new StatusBarProgressIndicator(Microsoft.Phone.Shell.SystemTray.ProgressIndicator);
#else
                return new StatusBarProgressIndicator();
#endif
            }
        }
    }
}
//#endif