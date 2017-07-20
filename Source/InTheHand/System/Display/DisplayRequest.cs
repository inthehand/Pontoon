//-----------------------------------------------------------------------
// <copyright file="DisplayRequest.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.System.Display
{
    /// <summary>
    /// Represents a display request.
    /// </summary>
    /// <remarks>
    /// To conserve power and extend battery life, the system reduces power to the computer if it does not detect any user activity for a certain amount of time.
    /// Depending on system power settings, the display may first be dimmed, a screen saver may be displayed, and eventually the display may be turned off as the system enters a low-power sleep state.
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public sealed partial class DisplayRequest
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        private Windows.System.Display.DisplayRequest _request = new DisplayRequest();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public static implicit operator Windows.System.Display.DisplayRequest(DisplayRequest r)
        {
            return r._request;
        }
#else
        private static int s_refCount = 0;
#endif

        /// <summary>
        /// Activates a display request. 
        /// </summary>
        public void RequestActive()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            _request.RequestActive();
#else
            if(s_refCount == 0)
            {
#if __ANDROID__ || __UNIFIED || WINDOWS_PHONE || WIN32
                RequestActiveImpl();
#endif
            }

            s_refCount++;
#endif
            }

        /// <summary>
        /// Deactivates a display request.
        /// </summary>
        public void RequestRelease()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            _request.RequestRelease();
#else
            s_refCount--;

            if (s_refCount == 0)
            {
#if __ANDROID__ || __UNIFIED || WINDOWS_PHONE || WIN32
                RequestReleaseImpl();
#endif
            }
#endif
        }
    }
}