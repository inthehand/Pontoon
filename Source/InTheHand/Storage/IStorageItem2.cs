//-----------------------------------------------------------------------
// <copyright file="IStorageItem2.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace InTheHand.Storage
{
    /// <summary>
    /// Manipulates storage items (files and folders) and their contents, and provides information about them.
    /// </summary>
    [Obsolete("Use IStorageItem", true)]
    public interface IStorageItem2 : IStorageItem
    {
        /// <summary>
        /// Gets the parent folder of the current storage item.
        /// </summary>
        /// <returns></returns>
        Task<StorageFolder> GetParentAsync();

        /// <summary>
        /// Indicates whether the current item is the same as the specified item.
        /// </summary>
        /// <param name="item">The <see cref="IStorageItem"/> object that represents a storage item to compare against.</param>
        /// <returns>Returns true if the current storage item is the same as the specified storage item; otherwise false.</returns>
        bool IsEqual(IStorageItem item);
    }
}