//-----------------------------------------------------------------------
// <copyright file="DisplayRequest.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.System.Display.DisplayRequest))]
#else

#if __ANDROID__
using Android.App;
using Android.Views;
#elif __IOS__
using UIKit;
#endif
using System;


namespace Windows.System.Display
{
    /// <summary>
    /// Starts the default app associated with the specified file or URI.
    /// </summary>
    public sealed class DisplayRequest
    {
#if __ANDROID__
        private bool requested = false;
#endif

        public void RequestActive()
        {
#if __ANDROID__
            if (!requested)
            {
                Activity a = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
                if (a != null)
                {
                    a.Window.AddFlags(WindowManagerFlags.KeepScreenOn);
                    requested = true;
                }
            }
#elif __IOS__
            UIApplication.SharedApplication.IdleTimerDisabled = true;
#elif WINDOWS_PHONE
            Microsoft.Phone.Shell.PhoneApplicationService.Current.ApplicationIdleDetectionMode = Microsoft.Phone.Shell.IdleDetectionMode.Disabled;
#endif

        }

        public void RequestRelease()
        {
#if __ANDROID__
            if (requested)
            {
                Activity a = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
                if (a != null)
                {
                    a.Window.ClearFlags(WindowManagerFlags.KeepScreenOn);
                    requested = false;
                }
            }
#elif __IOS__
            UIApplication.SharedApplication.IdleTimerDisabled = false;
#elif WINDOWS_PHONE
            Microsoft.Phone.Shell.PhoneApplicationService.Current.ApplicationIdleDetectionMode = Microsoft.Phone.Shell.IdleDetectionMode.Enabled;
#endif
        }
    }
}
#endif