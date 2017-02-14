//-----------------------------------------------------------------------
// <copyright file="Launcher.macOS.cs" company="In The Hand Ltd">
//     Copyright © 2017 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using AppKit;
using Foundation;
using InTheHand.Storage;
using System;
using System.Threading.Tasks;

namespace InTheHand.System
{
    public static partial class Launcher
    {
        private static Task<bool> LaunchFileAsyncImpl(IStorageFile file)
        {
            return Task.Run<bool>(() =>
            {
                bool success = NSWorkspace.SharedWorkspace.OpenFile(file.Path);
                return success;
            });
        }

        private static Task<bool> LaunchFolderAsyncImpl(IStorageFolder folder)
        {
            return Task.Run<bool>(() =>
            {
                bool success = NSWorkspace.SharedWorkspace.OpenUrl(NSUrl.FromFilename(folder.Path));
                return success;
            });
        }

        private static Task<bool> LaunchUriAsyncImpl(Uri uri, LauncherOptions options)
        {
            return Task.Run<bool>(() =>
            {
                return NSWorkspace.SharedWorkspace.OpenUrl(new NSUrl(uri.ToString()));
            });
        }
    }
}