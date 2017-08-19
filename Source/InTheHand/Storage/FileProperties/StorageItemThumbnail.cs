//-----------------------------------------------------------------------
// <copyright file="StorageItemThumbnail.cs" company="In The Hand Ltd">
//     Copyright © 2017 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;

namespace InTheHand.Storage.FileProperties
{
    public sealed partial class StorageItemThumbnail : Stream
    {
        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        public override long Length => GetLength();

        public override long Position { get => GetPosition(); set => SetPosition(value); }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return DoRead(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return DoSeek(offset, origin);
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}