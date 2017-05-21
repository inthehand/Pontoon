//-----------------------------------------------------------------------
// <copyright file="StatusBar.Win32.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace InTheHand.UI.ViewManagement
{
    partial class StatusBar
    {
        private IntPtr _handle;

        private static class NativeMethods
        {
            [DllImport("User32")]
            internal static extern IntPtr GetForegroundWindow();
        }
    }
}