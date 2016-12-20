//-----------------------------------------------------------------------
// <copyright file="FileSavePicker.Win32.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace InTheHand.Storage.Pickers
{
    public sealed partial class FileSavePicker
    {
        private async Task<StorageFile> PickSaveFileAsyncImpl()
        {
            NativeMethods.OPENFILENAME ofn = new FileSavePicker.NativeMethods.OPENFILENAME();
            StringBuilder sb = new StringBuilder();
            foreach(KeyValuePair<string,IList<string>> item in FileTypeChoices)
            {
                sb.Append(item.Key + "\0");
                foreach (string filetype in item.Value)
                {
                    sb.Append("*" + filetype + ";");
                }

                if(sb[sb.Length-1] == ';')
                {
                    sb.Length -= 1;
                }

                sb.Append('\0');
            }

            sb.Append("\0");
            ofn.lpstrFilter = sb.ToString();
            ofn.lStructSize = Marshal.SizeOf(ofn);
            ofn.nMaxFile = 256;
            ofn.lpstrFile = new string('\0', ofn.nMaxFile);
            ofn.Flags = 0x02001000;
            if(!string.IsNullOrEmpty(DefaultFileExtension))
            {
                ofn.lpstrDefExt = DefaultFileExtension.Substring(DefaultFileExtension.StartsWith(".") ? 1 : 0);
            }

            bool success = NativeMethods.GetSaveFileName(ref ofn);
            if(success)
            {
                File.Create(ofn.lpstrFile).Close();
                return new StorageFile(ofn.lpstrFile);
            }

            return null;
        }

        private IDictionary<string, IList<string>> _filter = new Dictionary<string, IList<string>>();
        private IDictionary<string,IList<string>> GetFileTypeChoices()
        {
            return _filter;
        }

        private static class NativeMethods
        {
            [DllImport("comdlg32")]
            internal static extern bool GetSaveFileName(ref OPENFILENAME lpofn);

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
                internal string lpstrDefExt;
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