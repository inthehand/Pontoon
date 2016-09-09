//-----------------------------------------------------------------------
// <copyright file="CreationCollisionOption.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
using System.Runtime.CompilerServices;

[assembly: TypeForwardedTo(typeof(Windows.Storage.CreationCollisionOption))]
#else

namespace Windows.Storage
{
    /// <summary>
    /// Represents a file.
    /// Provides information about the file and its content, and ways to manipulate them.
    /// </summary>
    public enum CreationCollisionOption
    {
        /// <summary>
        /// Automatically append a number to the base of the specified name if the file or folder already exists.
        /// For example, if MyFile.txt already exists, then the new file is named MyFile(2).txt.If MyFolder already exists, then the new folder is named MyFolder(2).
        /// </summary>
        GenerateUniqueName = 0,

        /// <summary>
        /// Replace the existing item if the file or folder already exists.
        /// </summary>
        ReplaceExisting = 1,

        /// <summary>
        /// Raise an exception of type System.Exception if the file or folder already exists.
        /// Methods that don't explicitly pass a value from the CreationCollisionOption enumeration use the FailIfExists value as the default when you try to create, rename, copy, or move a file or folder.
        /// </summary>
        FailIfExists = 2,

        /// <summary>
        /// Return the existing item if the file or folder already exists.
        /// </summary>
        OpenIfExists = 3,
    }
}
#endif