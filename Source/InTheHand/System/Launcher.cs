//-----------------------------------------------------------------------
// <copyright file="Launcher.cs" company="In The Hand Ltd">
//     Copyright © 2015 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if __ANDROID__
using Android.App;
using Android.Content;
#endif
using System;
using System.Threading.Tasks;

namespace InTheHand.System
{
    /// <summary>
    /// Starts the default app associated with the specified file or URI.
    /// </summary>
    public static class Launcher
    {
        /// <summary>
        /// Starts the default app associated with the URI scheme name for the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="options">Ignored on Android and iOS.</param>
        /// <returns>The launch operation.</returns>
        public static Task<bool> LaunchUriAsync(Uri uri, LauncherOptions options)
        {
#if __ANDROID__
            return Task.Run<bool>(() =>
            {
                try {
                    Intent uriIntent = new Intent(Intent.ActionView);
                    uriIntent.SetData(Android.Net.Uri.Parse(uri.ToString()));
                    Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivity(uriIntent);
                    return true;
                }
                catch { return false; }
            });
#elif __IOS__
            return Task.Run<bool>(() =>
            {
                return UIKit.UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(uri.ToString()));
            });
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return Windows.System.Launcher.LaunchUriAsync(uri).AsTask<bool>();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Starts the default app associated with the URI scheme name for the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>The launch operation.</returns>
        public static Task<bool> LaunchUriAsync(Uri uri)
        {
            return LaunchUriAsync(uri, new LauncherOptions());
        }
    }
}
