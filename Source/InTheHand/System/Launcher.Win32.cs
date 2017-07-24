//-----------------------------------------------------------------------
// <copyright file="Launcher.Win32.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
//     This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Storage;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace InTheHand.System
{
    /// <summary>
    /// Starts the default app associated with the specified file or URI.
    /// </summary>
    partial class Launcher
    {
        private static Task<bool> LaunchFileAsyncImpl(IStorageFile file)
        {
            return Task.FromResult<bool>(Launch(file.Path, null));
        }

        private static Task<bool> LaunchFolderAsyncImpl(IStorageFolder folder)
        {
            return Task.FromResult<bool>(Launch("explorer.exe", folder.Path));
        }

        private static Task<bool> LaunchUriAsyncImpl(Uri uri, LauncherOptions options)
        {
            return Task.FromResult<bool>(Launch(uri.ToString(), null));
        }

        private static Task<LaunchQuerySupportStatus> QueryUriSupportAsyncImpl(Uri uri, LaunchQuerySupportType launchQuerySupportType)
        {
            return Task.Run<LaunchQuerySupportStatus>(() =>
            {
                using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(uri.Scheme))
                {
                    if (rk != null)
                    {
                        if (rk.GetValue("URL Protocol") != null)
                        {
                            return LaunchQuerySupportStatus.Available;
                        }
                    }
                }

                return LaunchQuerySupportStatus.NotSupported;
            });
        }

        private static bool Launch(string applicationName, string commandLine)
        {
            return NativeMethods.CreateProcess(applicationName, commandLine, IntPtr.Zero, IntPtr.Zero, false, 0, IntPtr.Zero, null, IntPtr.Zero, IntPtr.Zero);
        }

        private static class NativeMethods
        {
            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            internal static extern bool CreateProcess(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, IntPtr lpStartupInfo, IntPtr lpProcessInformation);
        }
    }
}