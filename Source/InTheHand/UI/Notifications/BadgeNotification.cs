//-----------------------------------------------------------------------
// <copyright file="BadgeNotification.cs" company="In The Hand Ltd">
//     Copyright © 2014-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

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
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>watchOS</term><description>watchOS 2.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.1 or later</description></item></list></remarks>
    public sealed partial class BadgeNotification
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private Windows.UI.Notifications.BadgeNotification _notification;

        private BadgeNotification(Windows.UI.Notifications.BadgeNotification notification)
        {
            _notification = notification;
        }

        public static implicit operator BadgeNotification(Windows.UI.Notifications.BadgeNotification b)
        {
            return new BadgeNotification(b);
        }

        public static implicit operator Windows.UI.Notifications.BadgeNotification(BadgeNotification b)
        {
            return b._notification;
        }

#elif __IOS__ || __TVOS__ || __WATCHOS__
#else
        internal BadgeNotification(uint value)
        {
            _value = value;
        }

        internal BadgeNotification(char glyph)
        {
            _glyph = glyph;
        }

        private uint _value;

        internal uint Value
        {
            get
            {
                return _value;
            }
        }

        private char _glyph;

        internal char Glyph
        {
            get
            {
                return _glyph;
            }
        }
#endif
    }
}