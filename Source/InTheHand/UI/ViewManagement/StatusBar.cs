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
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10 Mobile</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list></remarks>
    public sealed partial class StatusBar
    {
#if WINDOWS_UWP || WINDOWS_PHONE_APP
        private Windows.UI.ViewManagement.StatusBar _statusBar;

        internal StatusBar(Windows.UI.ViewManagement.StatusBar statusBar)
        {
            _statusBar = statusBar;
        }

        public static implicit operator Windows.UI.ViewManagement.StatusBar(StatusBar sb)
        {
            return sb._statusBar;
        }
#else
        private static StatusBar _statusBar;

        private StatusBar()
        {
#if WIN32
            _handle = NativeMethods.GetForegroundWindow();
#endif
        }
#endif

        /// <summary>
        /// Gets the status bar for the current window (app view).
        /// </summary>
        /// <returns></returns>
        public static StatusBar GetForCurrentView()
        {
#if WINDOWS_UWP
            if(Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                return new StatusBar(Windows.UI.ViewManagement.StatusBar.GetForCurrentView());
            }

            return null;
#elif WINDOWS_PHONE_APP
            return new StatusBar(Windows.UI.ViewManagement.StatusBar.GetForCurrentView());
#else
            if (_statusBar == null)
            {
                _statusBar = new StatusBar();
            }

            return _statusBar;
#endif
        }

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
#elif WIN32
                return new StatusBarProgressIndicator(_handle);
#else
                return new StatusBarProgressIndicator();
#endif
            }
        }
    }
}
//#endif