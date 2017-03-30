//-----------------------------------------------------------------------
// <copyright file="IStorageItem.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InTheHand.Storage.FileProperties;

namespace InTheHand.Storage
{
    /// <summary>
    /// Manipulates storage items (files and folders) and their contents, and provides information about them.
    /// </summary>
    public interface IStorageItem
    {
        /// <summary>
        /// Deletes the current item. 
        /// </summary>
        /// <returns></returns>
        Task DeleteAsync();

        /// <summary>
        /// Deletes the current item, optionally deleting it permanently. 
        /// </summary>
        /// <returns></returns>
        Task DeleteAsync(StorageDeleteOption option);

        /// <summary>
        /// Gets the basic properties of the current item (like a file or folder).
        /// </summary>
        /// <returns></returns>
        Task<BasicProperties> GetBasicPropertiesAsync();

        /// <summary>
        /// Determines whether the current IStorageItem matches the specified StorageItemTypes value.
        /// </summary>
        /// <param name="type">The value to match against.</param>
        /// <returns></returns>
        /// <seealso cref="StorageItemTypes"/>
        bool IsOfType(StorageItemTypes type);

        /// <summary>
        /// Gets the attributes of a storage item.
        /// </summary>
        FileAttributes Attributes { get; }

        /// <summary>
        /// Gets the date and time when the current item was created. 
        /// </summary>
        DateTimeOffset DateCreated { get; }

        /// <summary>
        /// Gets the name of the item including the file name extension if there is one.
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Gets the full file-system path of the item, if the item has a path.
        /// </summary>
        string Path { get; }
    }
}