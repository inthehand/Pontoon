//-----------------------------------------------------------------------
// <copyright file="DisplayRequest.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if __ANDROID__
using Android.App;
using Android.Views;
#elif __IOS__
using UIKit;
#endif
using System;


namespace InTheHand.System.Display
{
    /// <summary>
    /// Starts the default app associated with the specified file or URI.
    /// </summary>
    public sealed class DisplayRequest
    {
#if __ANDROID__
        private bool requested = false;
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
        private Windows.System.Display.DisplayRequest _request = new Windows.System.Display.DisplayRequest();

        [CLSCompliant(false)]
        public static implicit operator Windows.System.Display.DisplayRequest(DisplayRequest dr)
        {
            return dr._request;
        }
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
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
            _request.RequestActive();
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
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
            _request.RequestRelease();
#elif WINDOWS_PHONE
            Microsoft.Phone.Shell.PhoneApplicationService.Current.ApplicationIdleDetectionMode = Microsoft.Phone.Shell.IdleDetectionMode.Enabled;
#endif
        }
    }
}
