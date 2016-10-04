//-----------------------------------------------------------------------
// <copyright file="BasicProperties.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Storage.FileProperties.BasicProperties))]
#else

using System;
using System.IO;

namespace Windows.Storage.FileProperties
{
    /// <summary>
    /// Provides access to the basic properties, like the size of the item or the date the item was last modified, of the item (like a file or folder).
    /// </summary>
    public sealed class BasicProperties
    {
        private IStorageItem _item;

        internal BasicProperties(IStorageItem item)
        {
            _item = item;
        }

        /// <summary>
        /// Gets the timestamp of the last time the file was modified.
        /// </summary>
        public DateTimeOffset DateModified
        {
            get
            {
#if __ANDROID__ || __IOS__ || WIN32
                DateTime time;
                TimeSpan offset;
                if (_item.IsOfType(StorageItemTypes.File))
                {
                    time = File.GetLastWriteTime(_item.Path);
                    offset = time.Subtract(File.GetLastWriteTimeUtc(_item.Path));
                }
                else
                {
                    time = Directory.GetLastWriteTime(_item.Path);
                    offset = time.Subtract(Directory.GetLastWriteTimeUtc(_item.Path));
                }
                return new DateTimeOffset(time, offset);
#endif
                return DateTimeOffset.MinValue;
            }
        }

        /// <summary>
        /// Gets the size of the file in bytes.
        /// </summary>
        public ulong Size
        {
            get
            {
#if __ANDROID__ || __IOS__ || WIN32
                if (_item.IsOfType(StorageItemTypes.File))
                {
                    FileInfo fi = new FileInfo(_item.Path);
                    return (ulong)fi.Length;
                }
#endif
                return 0;
            }
        }
    }
}
#endif