//-----------------------------------------------------------------------
// <copyright file="BadgeNotification.cs" company="In The Hand Ltd">
//     Copyright © 2014-15 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.UI.Notifications
{
    /// <summary>
    /// Defines the content, associated metadata, and expiration time of an update to a tile's badge overlay.
    /// A badge can display a number from 1 to 999 (beyond this truncation will occur)
    /// </summary>
    public sealed class BadgeNotification
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        internal Windows.UI.Notifications.BadgeNotification _notification;

        internal BadgeNotification(Windows.UI.Notifications.BadgeNotification notification)
        {
            _notification = notification;
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
