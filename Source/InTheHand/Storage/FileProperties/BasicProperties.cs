//-----------------------------------------------------------------------
// <copyright file="BasicProperties.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;

namespace InTheHand.Storage.FileProperties
{
    /// <summary>
    /// Provides access to the basic properties, like the size of the item or the date the item was last modified, of the item (like a file or folder).
    /// </summary>
    /// <seealso cref="IStorageItem.GetBasicPropertiesAsync"/>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list>
    /// </remarks>
    public sealed class BasicProperties
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.Storage.FileProperties.BasicProperties _properties;

        internal BasicProperties(Windows.Storage.FileProperties.BasicProperties properties)
        {
            _properties = properties;
        }

        public static implicit operator Windows.Storage.FileProperties.BasicProperties(BasicProperties p)
        {
            return p._properties;
        }
#else
        private IStorageItem _item;

        internal BasicProperties(IStorageItem item)
        {
            _item = item;
        }
#endif
        /// <summary>
        /// Gets the timestamp of the last time the file was modified.
        /// </summary>
        public DateTimeOffset DateModified
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _properties.DateModified;
#elif __ANDROID__ || __IOS__ || __TVOS__ || WIN32
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
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets the size of the file in bytes.
        /// </summary>
        public ulong Size
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _properties.Size;
#elif __ANDROID__ || __IOS__ || __TVOS__ || WIN32
                if (_item.IsOfType(StorageItemTypes.File))
                {
                    FileInfo fi = new FileInfo(_item.Path);
                    return (ulong)fi.Length;
                }

                return 0;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }
    }
}