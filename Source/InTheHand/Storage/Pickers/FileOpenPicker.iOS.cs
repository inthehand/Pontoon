//-----------------------------------------------------------------------
// <copyright file="FileOpenPicker.iOS.cs" company="In The Hand Ltd">
//     Copyright © 2017 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Threading.Tasks;
using System.Collections.Generic;
using UIKit;
using System.Threading;
using Foundation;
using System;

namespace InTheHand.Storage.Pickers
{
    partial class FileOpenPicker
    {
        private UIDocumentPickerViewController _picker;

        public FileOpenPicker()
        {
            
        }

        EventWaitHandle _handle = new EventWaitHandle(false, EventResetMode.AutoReset);

        private async Task<StorageFile> DoPickSingleFileAsync()
        {
            _picker = new UIDocumentPickerViewController(_fileTypes.ToArray(), UIDocumentPickerMode.Open);
            _picker.PresentViewController(_picker, true, () => {
                _handle.Set();
            });
            _handle.WaitOne();

            return new StorageFile(_uri);
        }

        private string _uri;

        private void SetUri(string uri)
        {
            _uri = uri;
            _handle.Set();
        }

        private List<string> _fileTypes = new List<string>();
        private IList<string> GetFileTypeFilter()
        {
            return _fileTypes;
        }

        internal sealed class PickerDelegate : UIDocumentPickerDelegate
        {
            private FileOpenPicker _picker;

            internal PickerDelegate(FileOpenPicker parent)
            {
                _picker = parent;
            }

            public override void DidPickDocument(UIDocumentPickerViewController controller, NSUrl url)
            {
                _picker.SetUri(url.ToString());
            }
        }
    }
}