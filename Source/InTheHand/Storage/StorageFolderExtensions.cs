//-----------------------------------------------------------------------
// <copyright file="StorageFolderExtensions.cs" company="In The Hand Ltd">
//     Copyright © 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Storage.FileProperties;
using System;
#if WINDOWS_PHONE
using System.IO;
using System.IO.IsolatedStorage;
#endif
using System.Collections.Generic;
using System.Threading.Tasks;

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
        internal static string GetIsoStorePath(this Windows.Storage.StorageFolder folder)
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
        public static Task DeleteAllItems(this StorageFolder folder)
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
            });
        }
        
        /// <summary>
        /// Returns the size, in bytes, of the folder and all of its contents.
        /// </summary>
        /// <param name="folder">The folder to measure</param>
        /// <returns>The size, in bytes, of the folder and all of its contents.</returns>
        public static async Task<ulong> GetFolderSizeAsync(this IStorageFolder folder)
        {
            ulong size = 0;

            foreach (var thisFolder in await folder.GetFoldersAsync())
            {
                size += await GetFolderSizeAsync(thisFolder);
            }

            foreach (StorageFile thisFile in await folder.GetFilesAsync())
            {
                BasicProperties props = await thisFile.GetBasicPropertiesAsync();
                size += props.Size;
            }

            return size;
        }
    }
}
