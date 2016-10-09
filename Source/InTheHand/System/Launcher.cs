//-----------------------------------------------------------------------
// <copyright file="Launcher.cs" company="In The Hand Ltd">
//     Copyright © 2015-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.System.Launcher))]
#else

#if __ANDROID__
using Android.App;
using Android.Content;
#endif
using System;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Windows.System
{
    /// <summary>
    /// Starts the default app associated with the specified file or URI.
    /// </summary>
    public static partial class Launcher
    {
        /// <summary>
        /// Starts the default app associated with the URI scheme name for the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="options">Ignored on Android and iOS.</param>
        /// <returns>The launch operation.</returns>
        public static IAsyncOperation<bool> LaunchUriAsync(Uri uri, LauncherOptions options)
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
            }).AsAsyncOperation<bool>();
#elif __IOS__
            return Task.Run<bool>(() =>
            {
                return UIKit.UIApplication.SharedApplication.OpenUrl(new global::Foundation.NSUrl(uri.ToString()));
            }).AsAsyncOperation<bool>();
#elif WIN32
            return Task.FromResult<bool>(LaunchUri(uri)).AsAsyncOperation<bool>();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Starts the default app associated with the URI scheme name for the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>The launch operation.</returns>
        public static IAsyncOperation<bool> LaunchUriAsync(Uri uri)
        {
            return LaunchUriAsync(uri, new LauncherOptions());
        }

        /// <summary>
        /// Asynchronously query whether an app can be activated for the specified URI and launch type.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>Returns true if the default app for the URI scheme was launched; false otherwise.</returns>
        public static IAsyncOperation<LaunchQuerySupportStatus> QueryUriSupportAsync(Uri uri, LaunchQuerySupportType launchQuerySupportType)
        {
#if __IOS__
            if(launchQuerySupportType == LaunchQuerySupportType.UriForResults)
            {
                return Task.FromResult<LaunchQuerySupportStatus>(LaunchQuerySupportStatus.AppNotInstalled).AsAsyncOperation<LaunchQuerySupportStatus>();
            }
            return Task.FromResult<LaunchQuerySupportStatus>(UIKit.UIApplication.SharedApplication.CanOpenUrl(uri) ? LaunchQuerySupportStatus.Available : LaunchQuerySupportStatus.AppNotInstalled).AsAsyncOperation<LaunchQuerySupportStatus>();
#else
            return Task.FromResult<LaunchQuerySupportStatus>(LaunchQuerySupportStatus.Unknown).AsAsyncOperation<LaunchQuerySupportStatus>();
#endif
        }
    }
}
#endif