//-----------------------------------------------------------------------
// <copyright file="FolderPicker.Android.cs" company="In The Hand Ltd">
//     Copyright © 2018 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Storage.Pickers
{
    partial class FolderPicker
    {
        private static EventWaitHandle _handle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private static string _path = string.Empty;

        private Task<StorageFolder> DoPickSingleFolderAsync()
        {
            Intent i = new Intent(Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity, typeof(FolderPickerActivity));
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivity(i);
            return Task.Run<StorageFolder>(async () =>
            {
                _handle.WaitOne();
                if (!string.IsNullOrEmpty(_path))
                {
                    return await StorageFolder.GetFolderFromPathAsync(_path);
                }

                return null;
            });
        }

       
        [Activity(NoHistory = false, LaunchMode = LaunchMode.Multiple)]
        private sealed class FolderPickerActivity : Activity
        {
            protected override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);

                Intent intent = new Intent(Intent.ActionOpenDocumentTree);
                intent.AddCategory(Intent.CategoryDefault);
                Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivityForResult(intent, 0);
            }

            protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
            {
                base.OnActivityResult(requestCode, resultCode, data);

                // TODO: get true path from content: uri
                if (resultCode == Result.Ok)
                {
                    Android.Net.Uri uri = data.Data;
                    _path = uri.ToString();                    
                }

                _handle.Set();

                Finish();
            }
        }
    }
}