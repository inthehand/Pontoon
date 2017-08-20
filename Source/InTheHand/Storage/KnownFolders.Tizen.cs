//-----------------------------------------------------------------------
// <copyright file="KnownFolders.Tizen.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Tizen.System;

namespace InTheHand.Storage
{
    partial class KnownFolders
    {
        private static StorageFolder GetStorageFolderForDirectoryType(DirectoryType type)
        {
            foreach (Tizen.System.Storage s in StorageManager.Storages)
            {
                if (s.State == StorageState.Mounted)
                {
                    return new StorageFolder(s.GetAbsolutePath(type));
                }
            }

            return null;
        }
    }
}