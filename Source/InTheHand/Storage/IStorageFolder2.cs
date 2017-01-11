//-----------------------------------------------------------------------
// <copyright file="IStorageFolder2.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Threading.Tasks;

namespace InTheHand.Storage
{
    /// <summary>
    /// Manipulates folders and their contents, and provides information about them.
    /// </summary>
    public interface IStorageFolder2
    {
        /// <summary>
        /// Try to get a single file or sub-folder from the current folder by using the name of the item.
        /// </summary>
        /// <param name="name">The name (or path relative to the current folder) of the file or sub-folder to try to retrieve.</param>
        /// <returns>When this method completes successfully, it returns the file or folder (type <see cref="IStorageItem"/>).</returns>
        Task<IStorageItem> TryGetItemAsync(string name);
    }
}