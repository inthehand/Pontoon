//-----------------------------------------------------------------------
// <copyright file="Clipboard.Win32.cs" company="In The Hand Ltd">
//     Copyright © 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace InTheHand.ApplicationModel.DataTransfer
{
    partial class Clipboard
    {
        private static void EmptyClipboard()
        {
            NativeMethods.OpenClipboard(IntPtr.Zero);
            NativeMethods.EmptyClipboard();
            NativeMethods.CloseClipboard();
        }

        private static void SetText(string text)
        {
            NativeMethods.OpenClipboard(IntPtr.Zero);
            IntPtr ptr = NativeMethods.SetClipboardData(NativeMethods.CF_UNICODETEXT, text);
            NativeMethods.CloseClipboard();
        }

        private static string GetText()
        {
            string value = string.Empty;

            NativeMethods.OpenClipboard(IntPtr.Zero);
            IntPtr ptr = NativeMethods.GetClipboardData(NativeMethods.CF_UNICODETEXT);
            if(ptr != IntPtr.Zero)
            {
                value = Marshal.PtrToStringUni(ptr);
            }

            NativeMethods.CloseClipboard();

            return value;
        }

        private static class NativeMethods
        {
            internal const int CF_UNICODETEXT = 13;

            [DllImport("User32")]
            internal static extern bool OpenClipboard(IntPtr hWndNewOwner);

            [DllImport("User32")]
            internal static extern bool CloseClipboard();

            [DllImport("User32")]
            internal static extern bool EmptyClipboard();

            [DllImport("User32")]
            internal static extern bool IsClipboardFormatAvailable(int format);

            [DllImport("User32")]
            internal static extern IntPtr GetClipboardData(int uFormat);

            [DllImport("User32", CharSet=CharSet.Unicode)]
            internal static extern IntPtr SetClipboardData(int uFormat, string hMem);
        }

    }
}