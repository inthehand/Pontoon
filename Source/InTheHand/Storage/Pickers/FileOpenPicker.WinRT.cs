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
                return new StorageFile(await _picker.PickSingleFileAsync());
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