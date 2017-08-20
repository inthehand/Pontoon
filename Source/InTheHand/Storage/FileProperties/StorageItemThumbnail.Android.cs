//-----------------------------------------------------------------------
// <copyright file="StorageItemThumbnail.Android.cs" company="In The Hand Ltd">
//     Copyright © 2017 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Android.Graphics;
using Android.Media;
using Java.Nio;
using System;
using System.IO;
using System.Threading.Tasks;

namespace InTheHand.Storage.FileProperties
{
    partial class StorageItemThumbnail
    {
        private global::System.IO.Stream _stream;

        private static ThumbnailUtils s_utils = new ThumbnailUtils();
        internal static async Task<StorageItemThumbnail> CreateVideoThumbnailAsync(StorageFile file)
        {
            var bmp = await ThumbnailUtils.CreateVideoThumbnailAsync(file.Path, Android.Provider.ThumbnailKind.MiniKind);
            MemoryStream stream = new MemoryStream();
            await bmp.CompressAsync(Bitmap.CompressFormat.Jpeg, 90, stream);
            stream.Seek(0, SeekOrigin.Begin);
            return new StorageItemThumbnail(stream);
        }

        internal static async Task<StorageItemThumbnail> CreatePhotoThumbnailAsync(StorageFile file)
        {
            var bmp = await ThumbnailUtils.ExtractThumbnailAsync(await BitmapFactory.DecodeFileAsync(file.Path), 240, 240, ThumnailExtractOptions.None);
            MemoryStream stream = new MemoryStream();
            await bmp.CompressAsync(Bitmap.CompressFormat.Jpeg, 90, stream);
            stream.Seek(0, SeekOrigin.Begin);
            return new StorageItemThumbnail(stream);
        }

        internal StorageItemThumbnail(global::System.IO.Stream stream)
        {
            _stream = stream;
        }

        protected override void Dispose(bool disposing)
        {
            if(_stream != null)
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