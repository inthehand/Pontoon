//-----------------------------------------------------------------------
// <copyright file="StorageItemThumbnail.Android.cs" company="In The Hand Ltd">
//     Copyright © 2017 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Android.Media;
using Java.Nio;
using System;
using System.IO;
using System.Threading.Tasks;

namespace InTheHand.Storage.FileProperties
{
    partial class StorageItemThumbnail
    {
        private MemoryStream _stream;

        private static ThumbnailUtils s_utils = new ThumbnailUtils();
        internal static async Task<StorageItemThumbnail> CreateVideoAsync(StorageFile file)
        {
            var bmp = await ThumbnailUtils.CreateVideoThumbnailAsync(file.Path, Android.Provider.ThumbnailKind.MiniKind);
            var rawBuffer = new byte[bmp.ByteCount];
            var buffer = ByteBuffer.Wrap(rawBuffer);
            await bmp.CopyPixelsToBufferAsync(buffer);
            return new StorageItemThumbnail(rawBuffer);
        }

        internal StorageItemThumbnail(byte[] data)
        {
            _stream = new MemoryStream(data);
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