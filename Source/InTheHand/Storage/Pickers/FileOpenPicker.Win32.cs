//-----------------------------------------------------------------------
// <copyright file="FileOpenPicker.Win32.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace InTheHand.Storage.Pickers
{
    public sealed partial class FileOpenPicker
    {
        private async Task<StorageFile> PickSingleFileAsyncImpl()
        {
            if(_filter.Count == 0)
            {
                throw new InvalidOperationException("The FileTypeFilters property must have at least one file type filter specified.");
            }

            NativeMethods.OPENFILENAME ofn = new Pickers.FileOpenPicker.NativeMethods.OPENFILENAME();
            
            StringBuilder sb = new StringBuilder();
            foreach(string ext in _filter)
            {
                sb.Append("*" + ext + "\0*" + ext + "\0");
            }

            sb.Append("\0");
            ofn.lpstrFilter = sb.ToString();
            ofn.lStructSize = Marshal.SizeOf(ofn);
            ofn.nMaxFile = 256;
            ofn.lpstrFile = new string('\0', ofn.nMaxFile);
            ofn.Flags = 0x02001000;
            bool success = NativeMethods.GetOpenFileName(ref ofn);

            if(success)
            {
                return new StorageFile(ofn.lpstrFile);
            }

            return null;
        }

        private List<string> _filter = new List<string>();
        private IList<string> GetFileTypeFilter()
        {
            return _filter;
        }

        private static class NativeMethods
        {
            [DllImport("comdlg32")]
            internal static extern bool GetOpenFileName(ref OPENFILENAME lpofn);

            internal struct OPENFILENAME
            {
                internal int lStructSize;
                internal IntPtr hwndOwner;
                IntPtr hInstance;
                internal string lpstrFilter;
                IntPtr lpstrCustomFilter;
                uint nMaxCustFilter;
                uint nFilterIndex;
                internal string lpstrFile;
                internal int nMaxFile;
                IntPtr lpstrFileTitle;
                int nMaxFileTitle;
                internal string lpstrInitialDir;
                internal string lpstrTitle;
                internal uint Flags;
                internal ushort nFileOffset;
                internal ushort nFileExtension;
                internal IntPtr lpstrDefExt;
                IntPtr lCustData;
                IntPtr lpfnHook;
                IntPtr lpTemplateName;
                IntPtr pvReserved;
                int dwReserved;
                int FlagsEx;
            }
        }
    }
}