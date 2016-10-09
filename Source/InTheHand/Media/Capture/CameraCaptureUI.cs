// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraCaptureUI.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Media.Capture.CameraCaptureUI))]
[assembly: TypeForwardedTo(typeof(Windows.Media.Capture.CameraCaptureUIMode))]
#else

#if __ANDROID__
using Android.App;
using Android.OS;
using Android.Content;
using Android.Provider;
using Android.Content.PM;
#elif __IOS__
using UIKit;
#elif WINDOWS_PHONE
using Microsoft.Phone.Tasks;
#endif

using System;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;
using System.IO;
using Windows.Foundation;
using Windows.Storage;

using System.Collections.Generic;


namespace Windows.Media.Capture
{
    /// <summary>
    /// Provides a full window UI for capturing audio, video, and photos from a camera. 
    /// </summary>
    public sealed class CameraCaptureUI
    {
#if __IOS__
        private UIImagePickerController _pc = new UIImagePickerController();
        private EventWaitHandle _handle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private string _filename;

#elif WINDOWS_PHONE_APP
        private static Type _type10;
        private object ccu = null;

        static CameraCaptureUI()
        {
            _type10 = Type.GetType("Windows.Media.Capture.CameraCaptureUI, Windows, ContentType=WindowsRuntime");
        }
#elif WINDOWS_PHONE
        private CameraCaptureTask _task;
        private EventWaitHandle _handle = new EventWaitHandle(false, EventResetMode.AutoReset);

        private void _task_Completed(object sender, PhotoResult e)
        {
            _handle.Set();
        }
#endif

        /// <summary>
        /// Create a new CameraCaptureUI object.
        /// </summary>
        public CameraCaptureUI()
        {
#if __IOS__
            _pc.ModalPresentationStyle = UIModalPresentationStyle.CurrentContext;
            _pc.FinishedPickingMedia += Pc_FinishedPickingMedia;
            _pc.Canceled += Pc_Canceled;
#elif WINDOWS_PHONE_APP
            if (_type10 != null)
            {
                ccu = Activator.CreateInstance(_type10);
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
#elif WINDOWS_PHONE
            _task = new CameraCaptureTask();
            _task.Completed += _task_Completed;
#endif
        }


#if __ANDROID__

        private static EventWaitHandle _handle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private static string _path = string.Empty;

        private static bool IsIntentAvailable()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = Android.App.Application.Context.PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        [Activity(NoHistory =false, LaunchMode =LaunchMode.Multiple)]
        private sealed class CameraActivity : Activity
        {
            private Java.IO.File _file;

            protected override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);

                //var dir = Android.OS.Environment.DirectoryPictures;
                var kf = KnownFolders.CameraRoll.Path;
                
                Intent intent = new Intent(MediaStore.ActionImageCapture);
                _file = new Java.IO.File(kf, String.Format("{0}.jpg", DateTime.Now.ToString("yyyyMMdd_HHmmss")));
                intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_file));
                Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivityForResult(intent, 0);

            }
            protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
            {
                base.OnActivityResult(requestCode, resultCode, data);
                this.Finish();

                if(resultCode == Result.Ok)
                {
                    _path = _file.AbsolutePath;
                }

                _handle.Set();
            }
        }

#elif __IOS__
        private void Pc_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            Stream gfxStream = null;
            if (e.EditedImage != null)
            {
                gfxStream = e.EditedImage.AsJPEG().AsStream();
            }
            else if (e.OriginalImage != null)
            {
                gfxStream = e.OriginalImage.AsJPEG().AsStream();
            }

            if (gfxStream != null)
            {
                _filename = Path.Combine(global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.Personal), DateTime.UtcNow.ToString("yyyy-mm-dd HH:mm:ss") + ".jpg");
                Stream file = File.Create(_filename);
                gfxStream.CopyTo(file);
                file.Close();
            }

            _pc.DismissViewController(true, null);
            _handle.Set();
        }

        private void Pc_Canceled(object sender, EventArgs e)
        {
            _pc.DismissViewController(true, null);
            _handle.Set();
        }
#endif

        /// <summary>
        /// Launches the CameraCaptureUI user interface. 
        /// </summary>
        /// <param name="mode">Specifies whether the user interface that will be shown allows the user to capture a photo, capture a video, or capture both photos and videos.</param>
        /// <returns>When this operation completes, a StorageFile object is returned.</returns>
        public IAsyncOperation<StorageFile> CaptureFileAsync(CameraCaptureUIMode mode)
        {
#if __ANDROID__
            
            if(IsIntentAvailable())
            {
                Intent i = new Intent(Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity, typeof(CameraActivity));
                Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivity(i);
                return Task.Run<StorageFile>(async () =>
                {
                    _handle.WaitOne();
                    if (!string.IsNullOrEmpty(_path))
                    {
                        return await StorageFile.GetFileFromPathAsync(_path);
                    }

                    return null;
                }).AsAsyncOperation<StorageFile>();
            }
            return Task.FromResult<StorageFile>(null).AsAsyncOperation<StorageFile>();

#elif __IOS__
            return Task.Run<StorageFile>(async () =>
                        {
                            UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
                                {
                                    _pc.SourceType = UIImagePickerControllerSourceType.Camera;
                                    switch (mode)
                                    {
                                        case CameraCaptureUIMode.Photo:
                                            _pc.CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Photo;
                                            break;

                                        case CameraCaptureUIMode.Video:
                                            _pc.CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Video;
                                            break;
                                    }

                                    UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(_pc, true, null);
                                });

                            _handle.WaitOne();

                            if (!string.IsNullOrEmpty(_filename))
                            {
                                return await StorageFile.GetFileFromPathAsync(_filename);
                            }
                            return null;
                        }).AsAsyncOperation<StorageFile>();
#elif WINDOWS_PHONE_APP
                if (_type10 != null)
                {
                    Type modeType = Type.GetType("Windows.Media.Capture.CameraCaptureUIMode, Windows, ContentType=WindowsRuntime");
                    object modeVal = Enum.ToObject(modeType, mode);
                    return (IAsyncOperation<StorageFile>)_type10.GetRuntimeMethod("CaptureFileAsync", new Type[] { modeType }).Invoke(ccu, new object[] { modeVal });
                }

                return Task.FromResult<StorageFile>(null).AsAsyncOperation<StorageFile>();
#elif WINDOWS_PHONE
            _task.Show();
            return Task.Run<StorageFile>(() =>
            {
                _handle.WaitOne();
                return (StorageFile)null;
            }).AsAsyncOperation<StorageFile>();
#else
            return Task.FromResult<StorageFile>(null).AsAsyncOperation<StorageFile>();
#endif
        }
    }

    /// <summary>
    /// Determines whether the user interface for capturing from the attached camera allows capture of photos, videos, or both photos and videos.
    /// </summary>
    public enum CameraCaptureUIMode
    {
        /// <summary>
        /// Either a photo or video can be captured.
        /// </summary>
        PhotoOrVideo = 0,

        /// <summary>
        /// The user can only capture a photo.
        /// </summary>
        Photo = 1,

        /// <summary>
        /// The user can only capture a video. 
        /// </summary>
        Video = 2,
    }
}
#endif