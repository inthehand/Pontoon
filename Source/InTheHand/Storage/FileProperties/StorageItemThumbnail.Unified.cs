//-----------------------------------------------------------------------
// <copyright file="StorageItemThumbnail.Unified.cs" company="In The Hand Ltd">
//     Copyright © 2017 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using AVFoundation;
using CoreGraphics;
using CoreMedia;
using Foundation;
using System;
using System.IO;
using System.Threading.Tasks;
using CoreImage;
using ImageIO;
using MobileCoreServices;
#if __MAC__
using AppKit;
#else
using UIKit;
#endif

namespace InTheHand.Storage.FileProperties
{
    partial class StorageItemThumbnail
    {
        private Stream _stream;
        
        internal static async Task<StorageItemThumbnail> CreateVideoThumbnailAsync(StorageFile file)
        {
            AVAsset asset = AVUrlAsset.FromUrl(NSUrl.FromString(file.Path));
            AVAssetImageGenerator generator = AVAssetImageGenerator.FromAsset(asset);
            NSError error;
            CMTime actualTime;
            CMTime time = CMTime.FromSeconds(asset.Duration.Seconds / 2, asset.Duration.TimeScale);
            CGImage image = generator.CopyCGImageAtTime(time, out actualTime, out error);
#if __MAC__
            NSMutableData buffer = new NSMutableData();
            CGImageDestination dest = CGImageDestination.Create(buffer, UTType.JPEG, 1, null);
            dest.AddImage(image);
            return new StorageItemThumbnail(buffer.AsStream());
#else
            UIImage image2 = UIImage.FromImage(image);
            image.Dispose();

            UIImage image3 = image2.Scale(new CGSize(240, 240));            
            image2.Dispose();

            return new StorageItemThumbnail(image3.AsJPEG().AsStream());
#endif
        }

        internal static async Task<StorageItemThumbnail> CreatePhotoThumbnailAsync(StorageFile file)
        {
#if __MAC__
            NSImage image = NSImage.FromStream(await file.OpenStreamForReadAsync());
            double ratio = image.Size.Width / image.Size.Height;

            NSImage newImage = new NSImage(new CGSize(240, 240 * ratio));
            newImage.LockFocus();
            image.Size = newImage.Size;

            image.Draw(new CGPoint(0, 0), new CGRect(0, 0, newImage.Size.Width, newImage.Size.Height), NSCompositingOperation.Copy, 1.0f);
            newImage.UnlockFocus();

            NSMutableData buffer = new NSMutableData();
            CGImageDestination dest = CGImageDestination.Create(buffer, UTType.JPEG, 1);
            dest.AddImage(newImage.CGImage);
            
            return new StorageItemThumbnail(buffer.AsStream());
#else
            UIImage image = UIImage.FromFile(file.Path);

            UIImage image2 = image.Scale(new CGSize(240, 240));
            image.Dispose();

            return new StorageItemThumbnail(image2.AsJPEG().AsStream());
#endif
        }

        internal StorageItemThumbnail(Stream stream)
        {
            _stream = stream;
        }

        protected override void Dispose(bool disposing)
        {
            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null;
            }

            base.Dispose(disposing);
        }

        private long GetLength()
        {
            return _stream.Length;
        }
        
        private long GetPosition()
        {
            return _stream.Position;
        }

        private void SetPosition(long value)
        {
            _stream.Position = value;
        }

        private int DoRead(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        private long DoSeek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }
    }
}