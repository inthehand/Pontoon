//-----------------------------------------------------------------------
// <copyright file="StatusBarProgressIndicator.uwp.cs" company="In The Hand Ltd">
//     Copyright © 2017 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.System;

namespace InTheHand.UI.ViewManagement
{
    partial class StatusBarProgressIndicator
    {
        private Windows.UI.ViewManagement.StatusBarProgressIndicator _indicator;

        // mobile
        internal StatusBarProgressIndicator(Windows.UI.ViewManagement.StatusBarProgressIndicator indicator)
        {
            _indicator = indicator;
        }

        public static implicit operator Windows.UI.ViewManagement.StatusBarProgressIndicator(StatusBarProgressIndicator pi)
        {
            return pi._indicator;
        }

        private Task HideAsyncImpl()
        {
            if (_indicator != null)
            {
                return _indicator.HideAsync().AsTask();
            }

            _isVisible = false;
            return new Task(() => { SetTaskbarProgress(-2); });
        }

        private Task ShowAsyncImpl()
        {
            if (_indicator != null)
            {
                return _indicator.ShowAsync().AsTask();
            }
            var h = Handle;

            return Task.Run(() => 
            {
                _isVisible = true;
                if (ProgressValue.HasValue)
                {
                    SetTaskbarProgress(ProgressValue.Value);
                }
                else
                {
                    // indeterminate
                    SetTaskbarProgress(-1);
                }
            });
        }

        private double? GetProgressValue()
        {
            if (_indicator != null)
            {
                return _indicator.ProgressValue;
            }

            return _progressValue;
        }

        private void SetProgressValue(double? value)
        {
            if (_indicator != null)
            {
                _indicator.ProgressValue = value;
            }
            else
            {
                _progressValue = value;
                if (_isVisible)
                {
                    if (_progressValue.HasValue)
                    {
                        SetTaskbarProgress(_progressValue.Value);
                    }
                    else
                    {
                        SetTaskbarProgress(0);
                    }
                }
            }
        }

        private string GetText()
        {
            if (_indicator != null)
            {
                return _indicator.Text;
            }

            return string.Empty;
        }

        private void SetText(string value)
        {
            if (_indicator != null)
            {
                _indicator.Text = value;
            }
        }

        internal void SetTaskbarProgress(double progress)
        {
            if(progress < 0)
            {
                CallDesktopTaskbarHelper(-1);
            }

            CallDesktopTaskbarHelper((int)(progress * short.MaxValue));
        }

        internal void CallDesktopTaskbarHelper(int value)
        {
            if(Handle != IntPtr.Zero)
            {
                var t = Launcher.LaunchUriAsync(new Uri("taskbarprogress:///?h=" + Handle.ToString() + "&p=" + value.ToString())).AsTask();
                t.Wait();
                Debug.WriteLine(t.Result);
            }
        }

        private IntPtr _handle;
        internal IntPtr Handle
        {
            get
            {
                if (_handle == IntPtr.Zero)
                {
                    dynamic corewin = Windows.UI.Core.CoreWindow.GetForCurrentThread();
                    var interop = (ICoreWindowInterop)corewin;
                    _handle = interop.WindowHandle;
                }
                return _handle;
            }
        }

    }

    

    [ComImport, Guid("45D64A29-A63E-4CB6-B498-5781D298CB4F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface ICoreWindowInterop
    {
        IntPtr WindowHandle { get; }
        bool MessageHandled { set; }
    }
}