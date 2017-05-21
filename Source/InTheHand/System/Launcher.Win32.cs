//-----------------------------------------------------------------------
// <copyright file="Launcher.Win32.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
//     This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace InTheHand.System
{
    /// <summary>
    /// Starts the default app associated with the specified file or URI.
    /// </summary>
    partial class Launcher
    {
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