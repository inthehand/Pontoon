//-----------------------------------------------------------------------
// <copyright file="DisplayInformation.Win32.cs" company="In The Hand Ltd">
//     Copyright © 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


using System;
using System.Runtime.InteropServices;

namespace InTheHand.Graphics.Display
{
    public sealed partial class DisplayInformation
    {
        private static DisplayInformation GetForCurrentViewImpl()
        {
            var hMonitor = NativeMethods.MonitorFromWindow(NativeMethods.GetActiveWindow(), NativeMethods.MONITOR_DEFAULTTO.NEAREST);
            if(hMonitor != IntPtr.Zero)
            {
                NativeMethods.MONITORINFOEX mi = new NativeMethods.MONITORINFOEX();
                mi.cbSize = Marshal.SizeOf(mi);

                if(NativeMethods.GetMonitorInfo(hMonitor, ref mi))
                {
                    DisplayInformation di = new DisplayInformation();

                    var dc = NativeMethods.CreateDC(mi.szDevice, null, null, IntPtr.Zero);

                    int xpixels = NativeMethods.GetDeviceCaps(dc, NativeMethods.DeviceCap.HORZRES);
                    int xsize = NativeMethods.GetDeviceCaps(dc, NativeMethods.DeviceCap.HORZSIZE);
                    di.rawDpiX = xpixels / (xsize * mmToInch);

                    int ypixels = NativeMethods.GetDeviceCaps(dc, NativeMethods.DeviceCap.VERTRES);
                    int ysize = NativeMethods.GetDeviceCaps(dc, NativeMethods.DeviceCap.VERTSIZE);
                    di.rawDpiY = ypixels / (ysize * mmToInch);

                    di._orientation = xpixels > ypixels ? DisplayOrientations.Landscape : DisplayOrientations.Portrait;
                    NativeMethods.DeleteDC(dc);

                    return di;
                }
            }

            return null;
        }

        private const float mmToInch = 0.03937008f;

        private DisplayOrientations _orientation;


        private static class NativeMethods
        {
            [DllImport("user32")]
            internal static extern IntPtr GetActiveWindow();

            [DllImport("user32")]
            internal static extern IntPtr MonitorFromWindow(IntPtr hwnd, MONITOR_DEFAULTTO dwFlags);

            internal enum MONITOR_DEFAULTTO : int
            {
                NULL = 0,
                PRIMARY = 1,
                NEAREST = 2,
            }

            [DllImport("user32")]
            internal static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFOEX lpmi);


            [StructLayout(LayoutKind.Sequential, CharSet =CharSet.Ansi)]
            internal struct MONITORINFOEX
            {
                internal int cbSize;
                internal RECT rcMonitor;
                internal RECT rcWork;
                internal uint dwFlags;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst =32)]
                internal string szDevice;
            }

            internal struct RECT
            {
                internal int left;
                internal int top;
                internal int right;
                internal int bottom;
            }

            [DllImport("gdi32")]
            internal static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);

            [DllImport("gdi32")]
            internal static extern bool DeleteDC(IntPtr hdc);

            [DllImport("gdi32")]
            internal static extern int GetDeviceCaps(IntPtr hdc, DeviceCap nIndex);

            internal enum DeviceCap : int
            {
                /// <summary>
                /// Horizontal size in millimeters
                /// </summary>
                HORZSIZE = 4,
                /// <summary>
                /// Vertical size in millimeters
                /// </summary>
                VERTSIZE = 6,
                /// <summary>
                /// Horizontal width in pixels
                /// </summary>
                HORZRES = 8,
                /// <summary>
                /// Vertical height in pixels
                /// </summary>
                VERTRES = 10,
            }
        }
    }
}