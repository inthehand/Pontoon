// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraCaptureUI.iOS.cs" company="In The Hand Ltd">
//   Copyright (c) 2016-17 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using InTheHand.Storage;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UIKit;

namespace InTheHand.Media.Capture
{
    public sealed partial class CameraCaptureUI
    {
        private UIImagePickerController _pc = new UIImagePickerController();
        private EventWaitHandle _handle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private string _filename;

        private void Init()
        {
            _pc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            _pc.FinishedPickingMedia += Pc_FinishedPickingMedia;
            _pc.Canceled += Pc_Canceled;
        }

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
            _handle.Set();

            UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
            {
                _pc.DismissViewController(true, null);
            });
        }

        private Task<StorageFile> CaptureFileAsyncImpl(CameraCaptureUIMode mode)
        {
            return Task.Run<StorageFile>(async () =>
            {
                UIApplication.SharedApplication.InvokeOnMainThread(() =>
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
            });
        }
    }
}