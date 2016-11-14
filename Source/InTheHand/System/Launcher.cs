//-----------------------------------------------------------------------
// <copyright file="Launcher.cs" company="In The Hand Ltd">
//     Copyright © 2015-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.System.Launcher))]
//#else

#if __ANDROID__
using Android.App;
using Android.Content;
#elif __IOS__
using UIKit;
#endif
using InTheHand.Storage;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace InTheHand.System
{
    /// <summary>
    /// Starts the default app associated with the specified file or URI.
    /// </summary>
    /// <remarks>    
    /// <list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list></remarks>
    public static partial class Launcher
    {
        /// <summary>
        /// Starts the app associated with the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>The launch operation.</returns>
        /// <remarks>    
        /// <list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
        /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
        /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list></remarks>
        public static Task<bool> LaunchFileAsync(IStorageFile file)
        {
#if __IOS__
            return Task.Run<bool>(() =>
            {
                bool success = false;
                UIKit.UIApplication.SharedApplication.InvokeOnMainThread(() =>
                {
                    UIDocumentInteractionController c = UIDocumentInteractionController.FromUrl(global::Foundation.NSUrl.FromFilename(file.Path));
                    c.ViewControllerForPreview = ViewControllerForPreview;
                    success = c.PresentPreview(true);
                });

                return success;
            });
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Windows.System.Launcher.LaunchFileAsync((Windows.Storage.StorageFile)((StorageFile)file)).AsTask();
#elif WIN32
            return Task.FromResult<bool>(Launch(file.Path, null));
#else
            throw new PlatformNotSupportedException();
#endif
        }

#if __IOS__

        private static UIViewController ViewControllerForPreview(UIDocumentInteractionController c)
        {
            return UIApplication.SharedApplication.KeyWindow.RootViewController;
        }
#endif

        /// <summary>
        /// Launches File Explorer and displays the contents of the specified folder.
        /// </summary>
        /// <param name="folder">The folder to display in File Explorer.</param>
        /// <returns>The result of the operation.</returns>
        /// <remarks>    
        /// <list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list></remarks>
        public static Task<bool> LaunchFolderAsync(IStorageFolder folder)
        {
#if WINDOWS_UWP
            return Windows.System.Launcher.LaunchFolderAsync((Windows.Storage.StorageFolder)((StorageFolder)folder)).AsTask();
#elif WIN32
            return Task.FromResult<bool>(Launch("explorer.exe", folder.Path));
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Starts the default app associated with the URI scheme name for the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="options">Ignored on Android and iOS.</param>
        /// <returns>The launch operation.</returns>
        internal static Task<bool> LaunchUriAsync(Uri uri, LauncherOptions options)
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
                return UIKit.UIApplication.SharedApplication.OpenUrl(new global::Foundation.NSUrl(uri.ToString()));
            });
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Windows.System.Launcher.LaunchUriAsync(uri).AsTask();
#elif WIN32
            return Task.FromResult<bool>(Launch(uri.ToString(), null));
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

        /// <summary>
        /// Asynchronously query whether an app can be activated for the specified URI and launch type.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="launchQuerySupportType">The type of launch for which to query support.</param>
        /// <returns>Returns true if the default app for the URI scheme was launched; false otherwise.</returns>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Android</term><description>-</description></item>
        /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows Store</term><description>-</description></item>
        /// <item><term>Windows Phone Store</term><description>-</description></item>
        /// <item><term>Windows Phone Silverlight</term><description>-</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>-</description></item></list>
        /// </remarks>
        public static Task<LaunchQuerySupportStatus> QueryUriSupportAsync(Uri uri, LaunchQuerySupportType launchQuerySupportType)
        {
#if __IOS__
            if(launchQuerySupportType == LaunchQuerySupportType.UriForResults)
            {
                return Task.FromResult<LaunchQuerySupportStatus>(LaunchQuerySupportStatus.AppNotInstalled);
            }
            return Task.FromResult<LaunchQuerySupportStatus>(UIKit.UIApplication.SharedApplication.CanOpenUrl(uri) ? LaunchQuerySupportStatus.Available : LaunchQuerySupportStatus.AppNotInstalled);
#elif WINDOWS_UWP
            return Task.Run<LaunchQuerySupportStatus>(async () =>
            {
                var s = await Windows.System.Launcher.QueryUriSupportAsync(uri, (Windows.System.LaunchQuerySupportType)((int)launchQuerySupportType)).AsTask();
                return (LaunchQuerySupportStatus)((int)s);
            });
#else
            return Task.FromResult<LaunchQuerySupportStatus>(LaunchQuerySupportStatus.Unknown);
#endif
        }
    }
}
//#endif