//-----------------------------------------------------------------------
// <copyright file="MessageDialog.Win32.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace InTheHand.UI.Popups
{
    partial class MessageDialog
    {
        private IUICommand ShowTaskDialog()
        {
            NativeMethods.TASKDIALOGCONFIG tdc = new NativeMethods.TASKDIALOGCONFIG();
            tdc.cbSize = Marshal.SizeOf(tdc);
            tdc.hwndParent = NativeMethods.GetForegroundWindow();
            tdc.pszWindowTitle = " ";
            tdc.pszMainInstruction = Title;
            tdc.dwFlags = NativeMethods.TASKDIALOG_FLAGS.SIZE_TO_CONTENT;
            tdc.pszContent = Content;
            if (Commands.Count == 0)
            {
                tdc.dwCommonButtons = NativeMethods.TASKDIALOG_COMMON_BUTTON_FLAGS.CLOSE_BUTTON;
            }
            else
            {
                if (CancelCommandIndex != uint.MaxValue)
                {
                    tdc.dwFlags |= NativeMethods.TASKDIALOG_FLAGS.ALLOW_DIALOG_CANCELLATION;
                }
            }

            IntPtr bptr = Marshal.AllocHGlobal(Commands.Count * 8);
            for(int ibut = 0; ibut < Commands.Count; ibut++)
            {
                var but = new NativeMethods.TASKDIALOG_BUTTON { nButtonID = ibut+11, pszButtonText = Commands[ibut].Label };
                Marshal.StructureToPtr(but, IntPtr.Add(bptr, ibut * 8), false);
            }

            if (DefaultCommandIndex != uint.MaxValue)
            {
                tdc.nDefaultButton = DefaultCommandIndex + 11;
            }

            tdc.pButtons = bptr;
            tdc.cButtons = Commands.Count;
            int ibtn;
            int hresult = NativeMethods.TaskDialogIndirect(ref tdc, out ibtn, IntPtr.Zero, IntPtr.Zero);
            Marshal.FreeHGlobal(bptr);

            if(ibtn > 10)
            {
                return Commands[ibtn - 11];
            }
            if(ibtn == 2 && CancelCommandIndex != uint.MaxValue)
            {
                return Commands[(int)CancelCommandIndex];
            }

            return null;
        }

        private static class NativeMethods
        {
            [DllImport("user32", PreserveSig = true)]
            internal static extern uint GetForegroundWindow();


            [DllImport("comctl32", PreserveSig =true)]
            internal static extern int TaskDialogIndirect(ref TASKDIALOGCONFIG pTaskConfig, out int pnButton, IntPtr pnRadioButton, IntPtr pfVerificationFlagChecked);

            [StructLayout(LayoutKind.Sequential, Size =96)]
            internal struct TASKDIALOGCONFIG
            {
                public int cbSize;
                public uint hwndParent;
                uint hInstance;
                public TASKDIALOG_FLAGS dwFlags;
                public TASKDIALOG_COMMON_BUTTON_FLAGS dwCommonButtons;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string pszWindowTitle;
                uint hMainIcon;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string pszMainInstruction;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string pszContent;
                public int cButtons;
                //[MarshalAs(UnmanagedType.LPArray)]
                public IntPtr pButtons;
                public uint nDefaultButton;
                uint cRadioButtons;
                uint pRadioButtons;
                uint nDefaultRadioButton;
                uint pszVerificationText;
                uint pszExpandedInformation;
                uint pszExpandedControlText;
                uint pszCollapsedControlText;
                uint hFooterIcon;
                uint pszFooter;
                uint pfCallback;
                uint lpCallbackData;
                uint cxWidth;
            }

            [StructLayout(LayoutKind.Sequential, Size =8)]
            internal struct TASKDIALOG_BUTTON
            {
                public int nButtonID;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string pszButtonText;
            }

            [Flags]
            internal enum TASKDIALOG_COMMON_BUTTON_FLAGS : uint
            {
                OK_BUTTON = 0x0001, // selected control return value IDOK
                YES_BUTTON = 0x0002, // selected control return value IDYES
                NO_BUTTON = 0x0004, // selected control return value IDNO
                CANCEL_BUTTON = 0x0008, // selected control return value IDCANCEL
                RETRY_BUTTON = 0x0010, // selected control return value IDRETRY
                CLOSE_BUTTON = 0x0020  // selected control return value IDCLOSE
            }

            [Flags]
            internal enum TASKDIALOG_FLAGS : uint
            {
                ENABLE_HYPERLINKS = 0x0001,
                USE_HICON_MAIN = 0x0002,
                USE_HICON_FOOTER = 0x0004,
                ALLOW_DIALOG_CANCELLATION = 0x0008,
                USE_COMMAND_LINKS = 0x0010,
                USE_COMMAND_LINKS_NO_ICON = 0x0020,
                EXPAND_FOOTER_AREA = 0x0040,
                EXPANDED_BY_DEFAULT = 0x0080,
                VERIFICATION_FLAG_CHECKED = 0x0100,
                SHOW_PROGRESS_BAR = 0x0200,
                SHOW_MARQUEE_PROGRESS_BAR = 0x0400,
                CALLBACK_TIMER = 0x0800,
                POSITION_RELATIVE_TO_WINDOW = 0x1000,
                RTL_LAYOUT = 0x2000,
                NO_DEFAULT_RADIO_BUTTON = 0x4000,
                CAN_BE_MINIMIZED = 0x8000,
//#if (NTDDI_VERSION >= NTDDI_WIN8)
//    TDF_NO_SET_FOREGROUND               = 0x00010000, // Don't call SetForegroundWindow() when activating the dialog
//#endif // (NTDDI_VERSION >= NTDDI_WIN8)
                SIZE_TO_CONTENT = 0x01000000  // used by ShellMessageBox to emulate MessageBox sizing behavior
            }
        }
    }
}