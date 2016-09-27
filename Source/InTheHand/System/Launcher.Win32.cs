//-----------------------------------------------------------------------
// <copyright file="Launcher.Win32.cs" company="In The Hand Ltd">
//     Copyright © 2015-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Windows.System
{
    /// <summary>
    /// Starts the default app associated with the specified file or URI.
    /// </summary>
    public static partial class Launcher
    {
        private static bool LaunchUri(Uri uri)
        {
            return NativeMethods.CreateProcess(uri.ToString(), null, IntPtr.Zero, IntPtr.Zero, false, 0, IntPtr.Zero, null, IntPtr.Zero, IntPtr.Zero);
        }

        private static class NativeMethods
        {
            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            internal static extern bool CreateProcess(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, IntPtr lpStartupInfo, IntPtr lpProcessInformation);
        }
    }
}