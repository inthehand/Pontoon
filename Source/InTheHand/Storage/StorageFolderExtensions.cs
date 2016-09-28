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

namespace Windows.Storage
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

        /// <summary>
        /// Try to get a single file or sub-folder from the current folder by using the name of the item.
        /// </summary>
        /// <param name="folder">The <see cref="StorageFolder"/>.</param>
        /// <param name="name">The name (or path relative to the current folder) of the file or sub-folder to try to retrieve.</param>
        /// <returns>When this method completes successfully, it returns the file or folder.</returns>
        /// <remarks>Use the <see cref="TryGetItemAsync"/> method to try to get a file or folder by name, without the need to add error-catching logic to your code (like you would if you used <see cref="StorageFolder.GetItemAsync"/>).
        /// If the file or folder can't be found, <see cref="TryGetItemAsync"/> returns null and doesn't raise an exception.
        /// Because the method returns null, you can use it to check if the specified fie or folder exists.</remarks>
        [CLSCompliant(false)]
        public static IAsyncOperation<Windows.Storage.IStorageItem> TryGetItemAsync(this Windows.Storage.StorageFolder folder, string name)
        {
            // trim leading \ if present
            if(name.StartsWith("\\"))
            {
                name = name.Substring(1);
            }

            TryGetItemState state = new TryGetItemState() { Folder = folder, Name = name };
            Task<Windows.Storage.IStorageItem> t = new Task<Windows.Storage.IStorageItem>(TryGetItem, state);
            t.Start();
            return t.AsAsyncOperation<Windows.Storage.IStorageItem>();
        }

        /// <exclude/>
        private static Windows.Storage.IStorageItem TryGetItem(object state)
        {
            TryGetItemState tgiState = (TryGetItemState)state;

            string isoPath = tgiState.Folder.GetIsoStorePath();
            if(isoPath == null)
            {
                // On 8.1 defer to try/catch if outside of localstate/isostore
                try
                {
                    Task<Windows.Storage.IStorageItem> t = tgiState.Folder.GetItemAsync(tgiState.Name).AsTask<Windows.Storage.IStorageItem>();
                    t.Wait();
                    return t.Result;
                }
                catch
                {
                    return null;
                }
            }
            
            if (tgiState.Name.Contains("."))
            {
                // file
                if (IsolatedStorageFile.GetUserStoreForApplication().FileExists(Path.Combine(isoPath, tgiState.Name)))
                {
                    Task<Windows.Storage.StorageFile> t = tgiState.Folder.GetFileAsync(tgiState.Name).AsTask<Windows.Storage.StorageFile>();
                    t.Wait();
                    return t.Result;
                }
            }
            else
            {
                // folder
                if (IsolatedStorageFile.GetUserStoreForApplication().DirectoryExists(Path.Combine(isoPath, tgiState.Name)))
                {
                    Task<Windows.Storage.StorageFolder> t = tgiState.Folder.GetFolderAsync(tgiState.Name).AsTask<Windows.Storage.StorageFolder>();
                    t.Wait();
                    return t.Result;
                }
                else if (IsolatedStorageFile.GetUserStoreForApplication().FileExists(Path.Combine(isoPath, tgiState.Name)))
                {
                    // file without extension
                    Task<Windows.Storage.StorageFile> t = tgiState.Folder.GetFileAsync(tgiState.Name).AsTask<Windows.Storage.StorageFile>();
                    t.Wait();
                    return t.Result;
                }
            }

            return null;
        }
       
        /// <exclude/>
        private struct TryGetItemState
        {
            public Windows.Storage.StorageFolder Folder;
            public string Name;
        }
#endif

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

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Try to get a single file or sub-folder from the current folder by using the name of the item.
        /// </summary>
        /// <param name="folder">The <see cref="StorageFolder"/>.</param>
        /// <param name="name">The name (or path relative to the current folder) of the file or sub-folder to try to retrieve.</param>
        /// <returns>When this method completes successfully, it returns the file or folder.</returns>
        /// <remarks>Use the <see cref="TryGetItemAsync"/> method to try to get a file or folder by name, without the need to add error-catching logic to your code (like you would if you used <see cref="StorageFolder.GetItemAsync"/>).
        /// If the file or folder can't be found, <see cref="TryGetItemAsync"/> returns null and doesn't raise an exception.
        /// Because the method returns null, you can use it to check if the specified fie or folder exists.</remarks>
        [CLSCompliant(false)]
        public static IAsyncOperation<IStorageItem> TryGetItemAsync(this StorageFolder folder, string name)
        {
            Task<Windows.Storage.IStorageItem> t = Task.Run<Windows.Storage.IStorageItem>(async () =>
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
            });

            return t.AsAsyncOperation<Windows.Storage.IStorageItem>();
        }
#endif

        /// <summary>
        /// Returns the size, in bytes, of the folder and all of its contents.
        /// </summary>
        /// <param name="folder">The folder to measure</param>
        /// <returns>The size, in bytes, of the folder and all of its contents.</returns>
        [CLSCompliant(false)]
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
