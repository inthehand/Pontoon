//-----------------------------------------------------------------------
// <copyright file="StorageFolder.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Storage
{
    /// <summary>
    /// Manipulates folders and their contents, and provides information about them.
    /// </summary>
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