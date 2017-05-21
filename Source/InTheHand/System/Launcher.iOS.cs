//-----------------------------------------------------------------------
// <copyright file="Launcher.iOS.cs" company="In The Hand Ltd">
//     Copyright © 2017 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Foundation;
using InTheHand.Storage;
using System;
using System.Threading.Tasks;
using UIKit;

namespace InTheHand.System
{
    static partial class Launcher
    {
#if !__TVOS__
        private static Task<bool> LaunchFileAsyncImpl(IStorageFile file)
        {
            return Task.Run<bool>(() =>
            {
                bool success = false;
                UIApplication.SharedApplication.InvokeOnMainThread(() =>
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
            bool success = false;

            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                success = UIApplication.SharedApplication.OpenUrl(new NSUrl(uri.ToString()));
            });

            return Task.FromResult(success);
        }

        private static Task<LaunchQuerySupportStatus> QueryUriSupportAsyncImpl(Uri uri, LaunchQuerySupportType launchQuerySupportType)
        {
            if(launchQuerySupportType == LaunchQuerySupportType.UriForResults)
            {
                return Task.FromResult(LaunchQuerySupportStatus.AppNotInstalled);
            }

            return Task.FromResult(UIApplication.SharedApplication.CanOpenUrl(uri) ? LaunchQuerySupportStatus.Available : LaunchQuerySupportStatus.AppNotInstalled);
        }
    }
}