//-----------------------------------------------------------------------
// <copyright file="StorageFolder.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Storage.IStorageFolder))]
#else

using System;
using System.Collections.Generic;

using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Storage;

namespace Windows.Storage
{
    /// <summary>
    /// Manipulates folders and their contents, and provides information about them.
    /// </summary>
    //[ContractVersion(typeof(UniversalApiContract), 65536U)]
    //[Guid(1926351736U, 46063, 20341, 168, 11, 111, 217, 218, 226, 148, 75)]
    public interface IStorageFolder : IStorageItem
    {
        IAsyncOperation<StorageFile> CreateFileAsync(string desiredName);
        IAsyncOperation<StorageFile> CreateFileAsync(string desiredName, CreationCollisionOption options);
        IAsyncOperation<StorageFolder> CreateFolderAsync(string desiredName);
        IAsyncOperation<StorageFolder> CreateFolderAsync(string desiredName, CreationCollisionOption options);
        IAsyncOperation<StorageFile> GetFileAsync(string filename);
        IAsyncOperation<IReadOnlyList<StorageFile>> GetFilesAsync();
        IAsyncOperation<StorageFolder> GetFolderAsync(string name);
        IAsyncOperation<IReadOnlyList<StorageFolder>> GetFoldersAsync();
        IAsyncOperation<IReadOnlyList<IStorageItem>> GetItemsAsync();
    }
}
#endif