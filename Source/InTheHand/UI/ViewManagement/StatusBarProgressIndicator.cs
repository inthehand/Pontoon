//-----------------------------------------------------------------------
// <copyright file="StatusBarProgressIndicator.cs" company="In The Hand Ltd">
//     Copyright © 2015-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

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
    /// Provides methods and properties for interacting with the progress indicator on the status bar on a window (app view).
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10 Mobile</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list></remarks>
    public sealed partial class StatusBarProgressIndicator
    {
        private bool _isVisible = false;
        private double? _progressValue = null;
        
#if WINDOWS_UWP || WINDOWS_PHONE_APP
        private Windows.UI.ViewManagement.StatusBarProgressIndicator _indicator;

        internal StatusBarProgressIndicator(Windows.UI.ViewManagement.StatusBarProgressIndicator indicator)
        {
            _indicator = indicator;
        }

        public static implicit operator Windows.UI.ViewManagement.StatusBarProgressIndicator(StatusBarProgressIndicator pi)
        {
            return pi._indicator;
        }

#elif WINDOWS_PHONE
        private Microsoft.Phone.Shell.ProgressIndicator _progressIndicator;

        internal StatusBarProgressIndicator(Microsoft.Phone.Shell.ProgressIndicator progressIndicator)
        {
            _progressIndicator = progressIndicator;
        } 
#elif WIN32
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
            return Task.Run(()=>{
#if __ANDROID__
                Activity a = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
                if (a != null)
                {
                    a.SetProgressBarVisibility(false);
                    a.SetProgressBarIndeterminateVisibility(false);
                }         
#elif __IOS__
                UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
#elif WINDOWS_UWP || WINDOWS_PHONE_APP
                return _indicator.HideAsync().AsTask();
#elif WINDOWS_PHONE
                _progressIndicator.IsVisible = false;
#elif WIN32
                _isVisible = false;
                _taskbarList.SetProgressState(_handle, TaskbarStates.NoProgress);
#endif
            });
        }

        /// <summary>
        /// Shows the progress indicator.
        /// </summary>
        /// <returns></returns>
        public Task ShowAsync()
        {
            return Task.Run(() =>
            {
#if __ANDROID__
                Activity a = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
                if (a != null)
                {
                    if (_progressValue.HasValue)
                    {
                        a.SetProgressBarVisibility(true);
                    }
                    else
                    {
                        a.SetProgressBarIndeterminateVisibility(true);
                    }
                }         
#elif __IOS__
                UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
#elif WINDOWS_UWP || WINDOWS_PHONE_APP
                return _indicator.ShowAsync().AsTask();
#elif WINDOWS_PHONE
                _progressIndicator.IsVisible = true;
#elif WIN32
                _isVisible = true;
                if (ProgressValue.HasValue)
                {
                    _taskbarList.SetProgressState(_handle, TaskbarStates.Normal);
                }
                else
                {
                    _taskbarList.SetProgressState(_handle, TaskbarStates.Indeterminate);
                }
#endif
            });
        }

        /// <summary>
        /// Gets or sets a value representing progress in the range 0 to 1.
        /// </summary>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Windows UWP</term><description>Windows 10 Mobile</description></item>
        /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
        /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list></remarks>
        public double? ProgressValue
        {
            get
            { 
#if WINDOWS_UWP || WINDOWS_PHONE_APP
                return _indicator.ProgressValue;
#elif WINDOWS_PHONE
                if(_progressIndicator.IsIndeterminate)
                {
                    return null;
                }

                return _progressIndicator.Value;
#else
                return _progressValue;
#endif
            }

            set
            {
#if __ANDROID__
                _progressValue = value;

#elif WINDOWS_UWP
                _indicator.ProgressValue = value;
#elif WINDOWS_PHONE
                if (value.HasValue)
                {
                    _progressIndicator.IsIndeterminate = false;
                    _progressIndicator.Value = value.Value;
                }
                else
                {
                    _progressIndicator.IsIndeterminate = true;
                }
#elif WIN32
                _progressValue = value;

                if (value.HasValue)
                {
                    _taskbarList.SetProgressState(_handle, _isVisible ? TaskbarStates.Normal : TaskbarStates.NoProgress);
                    _taskbarList.SetProgressValue(_handle, (ulong)(value.Value*100), 100);
                }
                else
                {
                    _taskbarList.SetProgressState(_handle, _isVisible ? TaskbarStates.Indeterminate : TaskbarStates.NoProgress);
                }
#endif
            }
        }

        /// <summary>
        /// Gets or sets the text label displayed on the progress indicator.
        /// </summary>
        public string Text
        {
            get
            {        
#if WINDOWS_UWP || WINDOWS_PHONE_APP
                return _indicator.Text;
#elif WINDOWS_PHONE
                return _progressIndicator.Text;
#else
                return string.Empty;
#endif
            }

            set
            {
#if WINDOWS_UWP || WINDOWS_PHONE_APP
                _indicator.Text = value;
#elif WINDOWS_PHONE
                _progressIndicator.Text = value;
#endif
            }
        }

    }
}