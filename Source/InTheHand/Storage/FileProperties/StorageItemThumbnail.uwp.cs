//-----------------------------------------------------------------------
// <copyright file="StorageItemThumbnail.Unified.cs" company="In The Hand Ltd">
//     Copyright © 2017 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;

namespace InTheHand.Storage.FileProperties
{
    partial class StorageItemThumbnail
    {
        private Windows.Storage.FileProperties.StorageItemThumbnail _thumbnail;
        private Stream _stream;

        public static implicit operator StorageItemThumbnail(Windows.Storage.FileProperties.StorageItemThumbnail tn)
        {
            return new StorageItemThumbnail(tn);
        }

        public static implicit operator Windows.Storage.FileProperties.StorageItemThumbnail(StorageItemThumbnail tn)
        {
            return tn._thumbnail;
        }

        private StorageItemThumbnail(Windows.Storage.FileProperties.StorageItemThumbnail thumbnail)
        {
            _thumbnail = thumbnail;
            _stream = _thumbnail.AsStreamForRead();
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