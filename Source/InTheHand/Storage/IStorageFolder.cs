//-----------------------------------------------------------------------
// <copyright file="StorageFolder.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.Storage.IStorageFolder))]
//#else

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Storage
{
    /// <summary>
    /// Manipulates folders and their contents, and provides information about them.
    /// </summary>
    //[ContractVersion(typeof(UniversalApiContract), 65536U)]
    //[Guid(1926351736U, 46063, 20341, 168, 11, 111, 217, 218, 226, 148, 75)]
    public interface IStorageFolder : IStorageItem
    {
        Task<StorageFile> CreateFileAsync(string desiredName);
        Task<StorageFile> CreateFileAsync(string desiredName, CreationCollisionOption options);
        Task<StorageFolder> CreateFolderAsync(string desiredName);
        Task<StorageFolder> CreateFolderAsync(string desiredName, CreationCollisionOption options);
        Task<StorageFile> GetFileAsync(string filename);
        Task<IReadOnlyList<StorageFile>> GetFilesAsync();
        Task<StorageFolder> GetFolderAsync(string name);
        Task<IReadOnlyList<StorageFolder>> GetFoldersAsync();
        Task<IReadOnlyList<IStorageItem>> GetItemsAsync();
    }
}
//#endif