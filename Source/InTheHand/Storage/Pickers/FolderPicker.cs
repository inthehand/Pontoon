//-----------------------------------------------------------------------
// <copyright file="FolderPicker.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Threading.Tasks;

namespace InTheHand.Storage.Pickers
{
    /// <summary>
    /// Represents a UI element that lets the user choose folders.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public sealed partial class FolderPicker
    {
        /// <summary>
        /// Shows the folderPicker object so that the user can pick a folder.
        /// </summary>
        /// <returns>When the call to this method completes successfully, it returns a <see cref="StorageFolder"/> object that represents the folder that the user picked.</returns>
        public Task<StorageFolder> PickSingleFolderAsync()
        {
            return PickSingleFolderAsyncImpl();
        }

#if PCL
        private Task<StorageFolder> PickSingleFolderAsyncImpl()
        {
            return null;
        }
#endif
    }
}