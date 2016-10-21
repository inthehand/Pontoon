//-----------------------------------------------------------------------
// <copyright file="DisplayRequest.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.System.Display.DisplayRequest))]
//#else

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
    /// Represents a display request.
    /// </summary>
    /// <remarks>
    /// To conserve power and extend battery life, the system reduces power to the computer if it does not detect any user activity for a certain amount of time.
    /// Depending on system power settings, the display may first be dimmed, a screen saver may be displayed, and eventually the display may be turned off as the system enters a low-power sleep state.
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list>
    /// </remarks>
    public sealed partial class DisplayRequest
    {
#if __ANDROID__
        private bool requested = false;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        private Windows.System.Display.DisplayRequest _request = new DisplayRequest();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public static implicit operator Windows.System.Display.DisplayRequest(DisplayRequest r)
        {
            return r._request;
        }
#endif

        /// <summary>
        /// Activates a display request. 
        /// </summary>
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
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            _request.RequestActive();
#elif WINDOWS_PHONE
            Microsoft.Phone.Shell.PhoneApplicationService.Current.ApplicationIdleDetectionMode = Microsoft.Phone.Shell.IdleDetectionMode.Disabled;
#elif WIN32
            NativeMethods.SetThreadExecutionState(NativeMethods.EXECUTION_STATE.DISPLAY_REQUIRED | NativeMethods.EXECUTION_STATE.CONTINUOUS);
#endif

        }

        /// <summary>
        /// Deactivates a display request.
        /// </summary>
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
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            _request.RequestRelease();
#elif WINDOWS_PHONE
            Microsoft.Phone.Shell.PhoneApplicationService.Current.ApplicationIdleDetectionMode = Microsoft.Phone.Shell.IdleDetectionMode.Enabled;
#elif WIN32
            NativeMethods.SetThreadExecutionState(NativeMethods.EXECUTION_STATE.CONTINUOUS);
#endif
        }
    }
}
//#endif