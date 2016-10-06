// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemProtection.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_PHONE_APP || WINDOWS_PHONE
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Phone.System.SystemProtection))]
#else

#if __ANDROID__
using Android.App;
using Android.Content;
using Android.OS;
#elif __IOS__
using UIKit;
#endif

using System;

namespace Windows.Phone.System
{
    /// <summary>
    /// Provides information related to system protection.
    /// <para><b>Supported on Windows Phone, Android and iOS only.</b></para>
    /// </summary>
    public static class SystemProtection
    {
        /// <summary>
        /// Gets a value that indicates whether the screen is locked.
        /// </summary>
        public static bool ScreenLocked
        {
            get
            {
#if __ANDROID__
                KeyguardManager km = (KeyguardManager) Application.Context.GetSystemService(Application.KeyguardService);
                return km.InKeyguardRestrictedInputMode();
#elif __IOS__
                return !UIApplication.SharedApplication.ProtectedDataAvailable;
#else
                return false;
#endif
            }
        }
    }
}
#endif