//-----------------------------------------------------------------------
// <copyright file="StatusBarProgressIndicator.cs" company="In The Hand Ltd">
//     Copyright © 2015-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_PHONE_APP
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.UI.ViewManagement.StatusBarProgressIndicator))]
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
#else
        internal StatusBarProgressIndicator()
        {
        }
#endif
        /// <summary>
        /// Hides the progress indicator.
        /// </summary>
        /// <returns></returns>
        public IAsyncAction HideAsync()
        {
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
            }).AsAsyncAction();
        }

        /// <summary>
        /// Shows the progress indicator.
        /// </summary>
        /// <returns></returns>
        public IAsyncAction ShowAsync()
        {
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
            }).AsAsyncAction();
        }
            
    }
}
//#endif