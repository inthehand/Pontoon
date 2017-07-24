//-----------------------------------------------------------------------
// <copyright file="KnownFolders.Win32.cs" company="In The Hand Ltd">
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
            return GetKnownFolderFromGuid(CameraRollGuid);
        }

        private static readonly Guid CameraRollGuid = new Guid("{AB5FB87B-7CE2-4F83-915D-550846C9537B}");

        private static StorageFolder GetKnownFolderFromGuid(Guid id)
        {
            IntPtr pathPtr;
            // to match UWP create the file if not present
            int hresult = NativeMethods.SHGetKnownFolderPath(id, 0x8000, IntPtr.Zero, out pathPtr);
            if (hresult == 0)
            {
                string path = Marshal.PtrToStringUni(pathPtr);
                Marshal.FreeCoTaskMem(pathPtr);
                return new StorageFolder(path);
            }

            return null;
        }

        private static class NativeMethods
        {
            [DllImport("Shell32")]
            internal static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out IntPtr ppszPath);
        }
    }
}