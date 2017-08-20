//-----------------------------------------------------------------------
// <copyright file="FileOpenPicker.Android.cs" company="In The Hand Ltd">
//     Copyright © 2017 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Content.PM;
using Android.OS;

namespace InTheHand.Storage.Pickers
{
    partial class FileOpenPicker
    {
        private static EventWaitHandle _handle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private static string _path = string.Empty;

        [Activity(NoHistory = false, LaunchMode = LaunchMode.Multiple)]
        private sealed class FileOpenActivity : Activity
        {
            protected override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);

                Intent intent = new Intent(Intent.ActionGetContent);
                intent.SetType("*/*");
                intent.AddCategory(Intent.CategoryOpenable);
                Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivityForResult(intent, 0);
            }

            protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
            {
                base.OnActivityResult(requestCode, resultCode, data);

                // TODO: get true path from content: uri
                if (resultCode == Result.Ok)
                {
                    _path = data.Data.ToString();
                }

                _handle.Set();

                Finish();
            }
        }

        public FileOpenPicker()
        {
            
        }

        private Task<StorageFile> DoPickSingleFileAsync()
        {
            Intent i = new Intent(Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity, typeof(FileOpenActivity));
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivity(i);
            return Task.Run<StorageFile>(async () =>
            {
                _handle.WaitOne();
                if (!string.IsNullOrEmpty(_path))
                {
                    return await StorageFile.GetFileFromPathAsync(_path);
                }

                return null;
            });
        }

        private List<string> _fileTypes = new List<string>();
        private IList<string> GetFileTypeFilter()
        {
            return _fileTypes;
        }
    }
}