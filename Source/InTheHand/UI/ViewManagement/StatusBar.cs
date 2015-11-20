//-----------------------------------------------------------------------
// <copyright file="StatusBar.cs" company="In The Hand Ltd">
//     Copyright © 2015 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.UI.ViewManagement
{

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

#if __ANDROID__
    using Android.App;
    using Android.Content;
#elif __IOS__
    using UIKit;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
#elif WINDOWS_PHONE
    using Windows.Foundation;
#endif
    
    public sealed class StatusBar
    {
        
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

#if WINDOWS_PHONE_APP || WINDOWS_UWP
        private Windows.UI.ViewManagement.StatusBar _statusBar;

        internal StatusBar(Windows.UI.ViewManagement.StatusBar statusBar)
        {
            _statusBar = statusBar;
        }
#else
        private static StatusBar _statusBar;

        internal StatusBar()
        {
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
#if WINDOWS_PHONE   
                return new StatusBarProgressIndicator(Microsoft.Phone.Shell.SystemTray.ProgressIndicator);     
#elif WINDOWS_PHONE_APP || WINDOWS_UWP
                return new StatusBarProgressIndicator(_statusBar.ProgressIndicator);
#else
                return new StatusBarProgressIndicator();
#endif
            }
        }
    }
}
