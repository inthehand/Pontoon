//-----------------------------------------------------------------------
// <copyright file="Launcher.Android.cs" company="In The Hand Ltd">
//     Copyright © 2017 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Android.Content;
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
                Android.Net.Uri uri = Android.Net.Uri.Parse(file.Path);
                Intent viewIntent = new Intent(Intent.ActionView);
                viewIntent.SetFlags(ActivityFlags.ClearTop);
                viewIntent.SetDataAndType(uri, "*/*");
                try
                {
                    Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivity(viewIntent);
                    return true;
                }
                catch (ActivityNotFoundException e)
                {
                    return false;
                }
            });
        }

        

        private static Task<bool> LaunchUriAsyncImpl(Uri uri, LauncherOptions options)
        {
            return Task.Run<bool>(() =>
            {
                try
                {
                    Intent uriIntent = new Intent(Intent.ActionView);
                    uriIntent.SetData(Android.Net.Uri.Parse(uri.ToString()));
                    Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivity(uriIntent);
                    return true;
                }
                catch { return false; }
            });
        }

        private static Task<LaunchQuerySupportStatus> QueryUriSupportAsyncImpl(Uri uri, LaunchQuerySupportType launchQuerySupportType)
        {
            return Task.Run<LaunchQuerySupportStatus>(() =>
            {
                Android.Net.Uri u = Android.Net.Uri.Parse(uri.ToString());
                Intent viewIntent = new Intent(Intent.ActionRun);
                viewIntent.SetData(u);
                try
                {
                    if(viewIntent.ResolveActivity(Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.PackageManager) != null)
                    {
                        return LaunchQuerySupportStatus.Available;
                    }

                    return LaunchQuerySupportStatus.AppNotInstalled;
                }
                catch (ActivityNotFoundException e)
                {
                    return LaunchQuerySupportStatus.NotSupported;
                }
            });
        }
    }
}