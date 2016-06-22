// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraCaptureUI.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#if __IOS__
using UIKit;
#endif
using System;
using System.Threading.Tasks;

namespace InTheHand.Media.Capture
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CameraCaptureUI
    {
        private UIImagePickerController pc = new UIImagePickerController();

        public CameraCaptureUI()
        {
            pc.ModalPresentationStyle = UIModalPresentationStyle.CurrentContext;
            pc.FinishedPickingMedia += Pc_FinishedPickingMedia;
            pc.Canceled += Pc_Canceled;
        }

        private void Pc_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            /*if (e.EditedImage != null)
            {
                e.EditedImage.SaveToPhotosAlbum((i, err) => { });
            }
            else
            {
                e.OriginalImage.SaveToPhotosAlbum((i, err) => { });
            }*/
            pc.DismissViewController(true, null);
        }

        private void Pc_Canceled(object sender, EventArgs e)
        {
            pc.DismissViewController(true, null);
        }

        public async Task<string> CaptureFileAsync(CameraCaptureUIMode mode)
        {
            pc.SourceType = UIImagePickerControllerSourceType.Camera;
            switch(mode)
            {
                case CameraCaptureUIMode.Photo:
                    pc.CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Photo;
                    break;

                case CameraCaptureUIMode.Video:
                    pc.CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Video;
                    break;
            }

            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(pc, true,null);
            
            return "";
        }
    }

    public enum CameraCaptureUIMode
    {
        PhotoOrVideo = 0,
        Photo = 1,
        Video = 2,
    }
}