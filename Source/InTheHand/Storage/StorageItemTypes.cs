//-----------------------------------------------------------------------
// <copyright file="StorageItemTypes.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.Storage.StorageItemTypes))]
//#else

namespace InTheHand.Storage
{
    /// <summary>
    /// Describes whether an item that implements the <see cref="IStorageItem"/> interface is a file or a folder.
    /// </summary>
    /// <seealso cref="IStorageItem.IsOfType(StorageItemTypes)"/>
    public enum StorageItemTypes
    {
        /// <summary>
        /// A storage item that is neither a file nor a folder.
        /// </summary>
        None = 0,

        /// <summary>
        /// A file that is represented as a <see cref="StorageFile"/> instance.
        /// </summary>
        File = 1,

        /// <summary>
        /// A folder that is represented as a <see cref="StorageFolder"/> instance.
        /// </summary>
        Folder = 2,
    }
}
//#endif