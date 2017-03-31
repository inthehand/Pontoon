//-----------------------------------------------------------------------
// <copyright file="FileOpenPicker.WinRT.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace InTheHand.Storage.Pickers
{
    public sealed partial class FileOpenPicker
    {
        private Windows.Storage.Pickers.FileOpenPicker _picker;

        public FileOpenPicker()
        {
            _picker = new Windows.Storage.Pickers.FileOpenPicker();
        }

        private async Task<StorageFile> PickSingleFileAsyncImpl()
        {
#if WINDOWS_PHONE_APP
            if (InTheHand.Environment.OSVersion.Version.Major > 8)
            {
#endif
#pragma warning disable CS0618 // Type or member is obsolete
                return await _picker.PickSingleFileAsync();
#pragma warning restore CS0618 // Type or member is obsolete

#if WINDOWS_PHONE_APP
            }

            return null;
#endif
        }

        private IList<string> GetFileTypeFilter()
        {
            return _picker.FileTypeFilter;
        }
    }
}