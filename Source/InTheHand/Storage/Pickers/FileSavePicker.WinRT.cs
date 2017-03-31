//-----------------------------------------------------------------------
// <copyright file="FileSavePicker.WinRT.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace InTheHand.Storage.Pickers
{
    public sealed partial class FileSavePicker
    {
        private Windows.Storage.Pickers.FileSavePicker _picker;

        public FileSavePicker()
        {
            _picker = new Windows.Storage.Pickers.FileSavePicker();
        }

        private async Task<StorageFile> PickSaveFileAsyncImpl()
        {
            _picker.DefaultFileExtension = DefaultFileExtension;
            return await _picker.PickSaveFileAsync();
        }

        private IDictionary<string, IList<string>> GetFileTypeChoices()
        {
            return _picker.FileTypeChoices;
        }
    }
}