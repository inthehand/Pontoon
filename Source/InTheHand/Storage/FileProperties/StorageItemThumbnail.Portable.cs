//-----------------------------------------------------------------------
// <copyright file="StorageItemThumbnail.Portable.cs" company="In The Hand Ltd">
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
        internal static async Task<StorageItemThumbnail> CreateVideoThumbnailAsync(StorageFile file)
        {
            return new StorageItemThumbnail();
        }

        internal static async Task<StorageItemThumbnail> CreatePhotoThumbnailAsync(StorageFile file)
        {
            return new StorageItemThumbnail();
        }

        internal StorageItemThumbnail()
        {
        }

        private long GetLength()
        {
            return 0;
        }
        
        private long GetPosition()
        {
            return 0;
        }

        private void SetPosition(long value)
        {
        }

        private int DoRead(byte[] buffer, int offset, int count)
        {
            return 0;
        }

        private long DoSeek(long offset, SeekOrigin origin)
        {
            return 0;
        }
    }
}