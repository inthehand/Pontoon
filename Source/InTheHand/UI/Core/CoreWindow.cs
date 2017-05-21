//-----------------------------------------------------------------------
// <copyright file="CoreWindow.cs" company="In The Hand Ltd">
//     Copyright © 2017 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace InTheHand.UI.Core
{
    public sealed class CoreWindow
    {
        public static CoreWindow GetForCurrentThread()
        {
            IntPtr hwnd = global::System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            
            if(hwnd != IntPtr.Zero)
            {
                return new CoreWindow(hwnd);
            }

            return null;
        }

        private IntPtr _hwnd;

        private CoreWindow(IntPtr hwnd)
        {
            _hwnd = hwnd;
        }

        internal IntPtr Handle
        {
            get
            {
                return _hwnd;
            }
        }

        internal static class NativeMethods
        {
        }
    }
}