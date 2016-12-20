//-----------------------------------------------------------------------
// <copyright file="FileSavePicker.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Storage.Pickers
{
    /// <summary>
    /// Represents a file picker that lets the user choose the file name, extension, and storage location for a file.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list>
    /// </remarks>
    public sealed partial class FileSavePicker
    {
        /// <summary>
        /// Shows the file picker so that the user can save a file and set the file name, extension, and location of the file to be saved.
        /// </summary>
        /// <returns>When the call to this method completes successfully, it returns a storageFile object that was created to represent the saved file.
        /// The file name, extension, and location of this storageFile match those specified by the user, but the file has no content.</returns>
        public Task<StorageFile> PickSaveFileAsync()
        {
            return PickSaveFileAsyncImpl();
        }

        /// <summary>
        /// Gets or sets the default file name extension that the <see cref="FileSavePicker"/> gives to files to be saved. 
        /// </summary>
        public string DefaultFileExtension { get; set; }

        /// <summary>
        /// Gets the collection of valid file types that the user can choose to assign to a file.
        /// </summary>
        public IDictionary<String, IList<String>> FileTypeChoices
        {
            get
            {
                return GetFileTypeChoices();
            }
        }
    }
}