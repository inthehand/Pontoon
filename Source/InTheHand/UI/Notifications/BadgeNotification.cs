//-----------------------------------------------------------------------
// <copyright file="BadgeNotification.cs" company="In The Hand Ltd">
//     Copyright © 2014-15 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.UI.Notifications.BadgeNotification))]
//#else

namespace InTheHand.UI.Notifications
{
    /// <summary>
    /// Defines the content, associated metadata, and expiration time of an update to a tile's badge overlay.
    /// A badge can display a number from 1 to 999 (beyond this truncation will occur)
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.1 or later</description></item></list></remarks>
    public sealed class BadgeNotification
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        internal Windows.UI.Notifications.BadgeNotification _notification;

        internal BadgeNotification(Windows.UI.Notifications.BadgeNotification notification)
        {
            _notification = notification;
        }

        public static implicit operator Windows.UI.Notifications.BadgeNotification(BadgeNotification b)
        {
            return b._notification;
        }
#else
        internal BadgeNotification(int value)
        {
            _value = value;
        }

        private int _value;

        internal int Value
        {
            get
            {
                return _value;
            }
        }
#endif
    }
}
//#endif