//-----------------------------------------------------------------------
// <copyright file="Launcher.iOS.cs" company="In The Hand Ltd">
//     Copyright © 2017 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Storage;
using System;
using System.Threading.Tasks;
using UIKit;

namespace InTheHand.System
{
    public static partial class Launcher
    {
#if !__TVOS__
        private static Task<bool> LaunchFileAsyncImpl(IStorageFile file)
        {
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
        }


        private static UIViewController ViewControllerForPreview(UIDocumentInteractionController c)
        {
            return UIApplication.SharedApplication.KeyWindow.RootViewController;
        }
#endif

        private static Task<bool> LaunchUriAsyncImpl(Uri uri, LauncherOptions options)
        {
            return Task.Run<bool>(() =>
            {
#if __MAC__
                return NSWorkspace.SharedWorkspace.OpenUrl(new global::Foundation.NSUrl(uri.ToString()));
#else
                return UIApplication.SharedApplication.OpenUrl(new global::Foundation.NSUrl(uri.ToString()));
#endif
            });
        }

        private static Task<LaunchQuerySupportStatus> QueryUriSupportAsyncImpl(Uri uri, LaunchQuerySupportType launchQuerySupportType)
        {
            if(launchQuerySupportType == LaunchQuerySupportType.UriForResults)
            {
                return Task.FromResult<LaunchQuerySupportStatus>(LaunchQuerySupportStatus.AppNotInstalled);
            }

            return Task.FromResult<LaunchQuerySupportStatus>(UIKit.UIApplication.SharedApplication.CanOpenUrl(uri) ? LaunchQuerySupportStatus.Available : LaunchQuerySupportStatus.AppNotInstalled);
        }
    }
}