//-----------------------------------------------------------------------
// <copyright file="StorageFolderExtensions.cs" company="In The Hand Ltd">
//     Copyright © 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
#if WINDOWS_PHONE
using System.IO;
using System.IO.IsolatedStorage;
#endif
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace InTheHand.Storage
{
    /// <summary>
    /// Provides additional methods for <see cref="Windows.Storage.StorageFolder"/>.
    /// </summary>
    public static class StorageFolderExtensions
    {
#if WINDOWS_PHONE
        // <summary>
        // Gets a relative path from a StorageFolder so that IsolatedStorage APIs can be used on it.
        // </summary>
        // <param name="folder">The folder.</param>
        // <returns>An IsolatedStorage path.</returns>
        private static string GetIsoStorePath(this Windows.Storage.StorageFolder folder)
        {
            string fullPath = folder.Path;
#if WINDOWS_PHONE_81
            if(!fullPath.ToLower().Contains(Windows.ApplicationModel.Package.Current.Id.Name.ToLower()))
            {
                return null;
            }

            string isoPath = fullPath.Substring(fullPath.ToLowerInvariant().IndexOf("\\localstate") + 11);  
#else
            string isoPath = fullPath.Substring(fullPath.ToLowerInvariant().IndexOf("\\local") + 6);
#endif
            return isoPath;
        }
#endif

        /// <summary>
        /// Deletes all contents of a folder without deleting the folder itself.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static IAsyncAction DeleteAllItems(this StorageFolder folder)
        {
            return Task.Run(async () =>
            {
                foreach(StorageFolder subfolder in await folder.GetFoldersAsync())
                {
                    await subfolder.DeleteAsync();
                }

                foreach(StorageFile file in await folder.GetFilesAsync())
                {
                    await file.DeleteAsync();
                }
            }).AsAsyncAction();
        }
        /// <summary>
        /// Gets the parent folder of the current folder.
        /// </summary>
        /// <param name="folder">The StorageFolder.</param>
        /// <returns>When this method completes, it returns the parent folder as a StorageFolder.</returns>
        public static IAsyncOperation<StorageFolder> GetParentAsync(this StorageFolder folder)
        {
#if WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Task.Run<StorageFolder>(async () =>
            {
                var parentPath = folder.Path.Substring(0, folder.Path.TrimEnd('\\').LastIndexOf('\\'));
                StorageFolder parent = null;
                if (!string.IsNullOrEmpty(parentPath))
                {
                    parent = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(parentPath);
                }

                return parent;
            }).AsAsyncOperation<StorageFolder>();
#else
            return folder.GetParentAsync();
#endif
        }


        /// <summary>
        /// Try to get a single file or sub-folder from the current folder by using the name of the item.
        /// </summary>
        /// <param name="folder">The <see cref="StorageFolder"/>.</param>
        /// <param name="name">The name (or path relative to the current folder) of the file or sub-folder to try to retrieve.</param>
        /// <returns>When this method completes successfully, it returns the file or folder.</returns>
        /// <remarks>Use the <see cref="TryGetItemAsync"/> method to try to get a file or folder by name, without the need to add error-catching logic to your code (like you would if you used <see cref="StorageFolder.GetItemAsync"/>).
        /// If the file or folder can't be found, <see cref="TryGetItemAsync"/> returns null and doesn't raise an exception.
        /// Because the method returns null, you can use it to check if the specified fie or folder exists.</remarks>
        public static IAsyncOperation<IStorageItem> TryGetItemAsync(this StorageFolder folder, string name)
        {
#if WINDOWS_PHONE_APP
            return Task.Run<Windows.Storage.IStorageItem>(async () =>
            {
                if (name.Contains("."))
                {
                    // names containing a . are files so do this faster files-only approach
                    foreach (Windows.Storage.StorageFile file in await folder.GetFilesAsync())
                    {
                        if (file.Name == name)
                        {
                            return file;
                        }
                    }
                }
                else
                {
                    // items with no extension could be either file or folder so check all items
                    foreach (Windows.Storage.IStorageItem item in await folder.GetItemsAsync())
                    {
                        if (item.Name == name)
                        {
                            return item;
                        }
                    }
                }

                return null;
            }).AsAsyncOperation<Windows.Storage.IStorageItem>();
#elif WINDOWS_PHONE
            return Task.Run<Windows.Storage.IStorageItem>(async () =>
            {
                string isoPath = folder.GetIsoStorePath();
                if (isoPath == null)
                {
                    // On 8.1 defer to try/catch if outside of localstate/isostore
                    try
                    {
                        return await folder.GetItemAsync(name);
                    }
                    catch
                    {
                        return null;
                    }
                }

                if (name.Contains("."))
                {
                    // file
                    if (IsolatedStorageFile.GetUserStoreForApplication().FileExists(Path.Combine(isoPath, name)))
                    {
                        return await folder.GetFileAsync(name);
                    }
                }
                else
                {
                    // folder
                    if (IsolatedStorageFile.GetUserStoreForApplication().DirectoryExists(Path.Combine(isoPath, name)))
                    {
                        return await folder.GetFolderAsync(name);
                    }
                    else if (IsolatedStorageFile.GetUserStoreForApplication().FileExists(Path.Combine(isoPath, name)))
                    {
                        // file without extension
                        return await folder.GetFileAsync(name);
                    }
                }

                return null;
            }).AsAsyncOperation<IStorageItem>();
#else
            return folder.TryGetItemAsync(name);
#endif
        }

        /// <summary>
        /// Returns the size, in bytes, of the folder and all of its contents.
        /// </summary>
        /// <param name="folder">The folder to measure</param>
        /// <returns>The size, in bytes, of the folder and all of its contents.</returns>
        public static async Task<ulong> GetFolderSizeAsync(this Windows.Storage.IStorageFolder folder)
        {
            ulong size = 0;

            foreach (var thisFolder in await folder.GetFoldersAsync())
            {
                size += await GetFolderSizeAsync(thisFolder);
            }

            foreach (Windows.Storage.StorageFile thisFile in await folder.GetFilesAsync())
            {
                Windows.Storage.FileProperties.BasicProperties props = await thisFile.GetBasicPropertiesAsync();
                size += props.Size;
            }

            return size;
        }
    }
}
