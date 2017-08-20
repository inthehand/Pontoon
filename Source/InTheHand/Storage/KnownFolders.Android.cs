//-----------------------------------------------------------------------
// <copyright file="KnownFolders.Android.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Storage
{
    partial class KnownFolders
    {
        private static StorageFolder GetCameraRoll()
        {
            var t = StorageFolder.GetFolderFromPathAsync(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim).AbsolutePath);
            t.Wait();
            return t.Result;
        }
    }
}