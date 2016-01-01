//-----------------------------------------------------------------------
// <copyright file="StatusBarProgressIndicator.cs" company="In The Hand Ltd">
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
    /// <summary>
    /// Provides methods and properties for interacting with the progress indicator on the status bar on a window (app view).
    /// </summary>
    public sealed class StatusBarProgressIndicator
    {
#if WINDOWS_PHONE
        private Microsoft.Phone.Shell.ProgressIndicator _progressIndicator;

        internal StatusBarProgressIndicator(Microsoft.Phone.Shell.ProgressIndicator progressIndicator)
        {
            _progressIndicator = progressIndicator;
        }
#elif WINDOWS_PHONE_APP || WINDOWS_UWP
        private Windows.UI.ViewManagement.StatusBarProgressIndicator _progressIndicator;

        internal StatusBarProgressIndicator(Windows.UI.ViewManagement.StatusBarProgressIndicator progressIndicator)
        {
            _progressIndicator = progressIndicator;
        }
#else
        internal StatusBarProgressIndicator()
        {
        }
#endif
        /// <summary>
        /// Hides the progress indicator.
        /// </summary>
        /// <returns></returns>
        public Task HideAsync()
        {
#if WINDOWS_PHONE_APP || WINDOWS_UWP
            return _progressIndicator.HideAsync().AsTask();
#else
            return Task.Run(()=>{
#if __ANDROID__
                Activity a = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
                if (a != null)
                {
                    a.SetProgressBarIndeterminateVisibility(false);
                }         
#elif __IOS__
                UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
#elif WINDOWS_PHONE
                _progressIndicator.IsVisible = false;
#endif
            });
#endif
        }

        /// <summary>
        /// Shows the progress indicator.
        /// </summary>
        /// <returns></returns>
        public Task ShowAsync()
        {
#if WINDOWS_PHONE_APP || WINDOWS_UWP
            return _progressIndicator.ShowAsync().AsTask();
#else
            return Task.Run(() =>
            {
#if __ANDROID__
                Activity a = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
                if (a != null)
                {
                    a.SetProgressBarIndeterminateVisibility(true);
                }         
#elif __IOS__
                UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
#elif WINDOWS_PHONE
                _progressIndicator.IsVisible = true;
#endif
            });
#endif
        }
            
    }
}
