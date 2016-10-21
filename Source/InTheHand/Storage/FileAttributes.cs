//-----------------------------------------------------------------------
// <copyright file="FileAttributes.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.Storage.FileAttributes))]
//#else

using System;

namespace InTheHand.Storage
{
    /// <summary>
    /// Describes the attributes of a file or folder.
    /// </summary>
    [Flags]
    public enum FileAttributes : int
    {
        /// <summary>
        /// The item is normal. That is, the item doesn't have any of the other values in the enumeration.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// The item is read-only.
        /// </summary>
        ReadOnly = 1,

        /// <summary>
        /// The item is a directory.
        /// </summary>
        Directory = 16,

        /// <summary>
        /// The item is archived.
        /// </summary>
        Archive = 32,

        /// <summary>
        /// The item is a temporary file.
        /// </summary>
        Temporary = 256,
    }

#if __ANDROID__ || __IOS__ || WIN32
    internal static class FileAttributesHelper
    {
        public static FileAttributes FromIOFileAttributes(global::System.IO.FileAttributes attrs)
        {
            FileAttributes outvalue = FileAttributes.Normal;

            if(attrs.HasFlag(global::System.IO.FileAttributes.ReadOnly))
            {
                outvalue |= FileAttributes.ReadOnly;
            }
            if (attrs.HasFlag(global::System.IO.FileAttributes.Directory))
            {
                outvalue |= FileAttributes.Directory;
            }
            if (attrs.HasFlag(global::System.IO.FileAttributes.Archive))
            {
                outvalue |= FileAttributes.Archive;
            }
            if (attrs.HasFlag(global::System.IO.FileAttributes.Temporary))
            {
                outvalue |= FileAttributes.Temporary;
            }

            return outvalue;
        }

    }
#endif
}
//#endif