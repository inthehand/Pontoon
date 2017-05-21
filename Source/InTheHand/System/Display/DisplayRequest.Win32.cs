//-----------------------------------------------------------------------
// <copyright file="DisplayRequest.Win32.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
//     This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace InTheHand.System.Display
{
    partial class DisplayRequest
    {
        private void RequestActiveImpl()
        {
            NativeMethods.SetThreadExecutionState(NativeMethods.EXECUTION_STATE.DISPLAY_REQUIRED | NativeMethods.EXECUTION_STATE.CONTINUOUS);
        }

        private void RequestReleaseImpl()
        {
            NativeMethods.SetThreadExecutionState(NativeMethods.EXECUTION_STATE.CONTINUOUS);
        }

        private static class NativeMethods
        {
            [DllImport("Kernel32", SetLastError = true)]
            internal static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

            internal enum EXECUTION_STATE : uint
            {
                // <summary>
                // Enables away mode.This value must be specified with ES_CONTINUOUS.
                // Away mode should be used only by media-recording and media-distribution applications that must perform critical background processing on desktop computers while the computer appears to be sleeping.See Remarks.
                // </summary>
                //AWAYMODE_REQUIRED = 0x00000040,

                /// <summary>
                /// Informs the system that the state being set should remain in effect until the next call that uses ES_CONTINUOUS and one of the other state flags is cleared.
                /// </summary>
                CONTINUOUS = 0x80000000,

                /// <summary>
                /// Forces the display to be on by resetting the display idle timer.
                /// </summary>
                DISPLAY_REQUIRED = 0x00000002,

                // <summary>
                // Forces the system to be in the working state by resetting the system idle timer.
                // </summary>
                //SYSTEM_REQUIRED = 0x00000001,
            }
        }
    }
}