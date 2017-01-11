//-----------------------------------------------------------------------
// <copyright file="IStorageFile.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Threading.Tasks;

namespace InTheHand.Storage
{
    /// <summary>
    /// Represents a file.
    /// Provides information about the file and its contents, and ways to manipulate them.
    /// </summary>
    public interface IStorageFile : IStorageItem
    {
        /// <summary>
        /// Replaces the specified file with a copy of the current file.
        /// </summary>
        /// <param name="fileToReplace">The file to replace.</param>
        /// <returns>No object or value is returned when this method completes.</returns>
        Task CopyAndReplaceAsync(IStorageFile fileToReplace);

        /// <summary>
        /// Creates a copy of the file in the specified folder.
        /// </summary>
        /// <param name="destinationFolder"></param>
        /// <returns></returns>
        Task<StorageFile> CopyAsync(IStorageFolder destinationFolder);

        /// <summary>
        /// Creates a copy of the file in the specified folder, using the desired name.
        /// </summary>
        /// <param name="destinationFolder"></param>
        /// <param name="desiredNewName"></param>
        /// <returns></returns>
        Task<StorageFile> CopyAsync(IStorageFolder destinationFolder, string desiredNewName);

        /// <summary>
        /// Moves the current file to the location of the specified file and replaces the specified file in that location.
        /// </summary>
        /// <param name="fileToReplace"></param>
        /// <returns></returns>
        Task MoveAndReplaceAsync(IStorageFile fileToReplace);

        /// <summary>
        /// Moves the current file to the specified folder.
        /// </summary>
        /// <param name="destinationFolder"></param>
        /// <returns></returns>
        Task MoveAsync(IStorageFolder destinationFolder);

        /// <summary>
        /// Moves the current file to the specified folder and renames the file according to the desired name.
        /// </summary>
        /// <param name="destinationFolder"></param>
        /// <param name="desiredNewName"></param>
        /// <returns></returns>
        Task MoveAsync(IStorageFolder destinationFolder, string desiredNewName);
        
        /// <summary>
        /// Gets the MIME type of the contents of the file.
        /// </summary>
        /// <value>The MIME type of the file contents.
        /// For example, a music file might have the "audio/mpeg" MIME type.</value>
        string ContentType { get; }

        /// <summary>
        /// Gets the type (file name extension) of the file.
        /// </summary>
        string FileType { get; }
    }
}