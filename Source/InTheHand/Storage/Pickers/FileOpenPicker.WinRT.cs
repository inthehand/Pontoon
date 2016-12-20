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
            return new StorageFile(await _picker.PickSingleFileAsync());
        }

        private IList<string> GetFileTypeFilter()
        {
            return _picker.FileTypeFilter;
        }
    }
}