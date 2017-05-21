//-----------------------------------------------------------------------
// <copyright file="BadgeUpdater.cs" company="In The Hand Ltd">
//     Copyright © 2014-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if __MAC__
using AppKit;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
using Windows.UI.Notifications;
#endif

namespace InTheHand.UI.Notifications
{
    /// <summary>
    /// Updates a badge overlay on the specific tile that the updater is bound to.
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
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.1 or later</description></item></list>
    /// </remarks>
    public sealed partial class BadgeUpdater
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private Windows.UI.Notifications.BadgeUpdater _updater;

        internal BadgeUpdater(Windows.UI.Notifications.BadgeUpdater updater)
        {
            _updater = updater;
        }
#else
        internal BadgeUpdater()
        {
        }
#endif
        /// <summary>
        /// Removes the badge from the tile that the updater is bound to.
        /// </summary>
        public void Clear()
        {
#if __MAC__
            NSApplication.SharedApplication.DockTile.BadgeLabel = string.Empty;
#elif __UNIFIED__
            ClearImpl();

#elif TIZEN
            //Tizen.Applications.BadgeControl.Remove(Tizen.Applications.Application.Current.ApplicationInfo.ApplicationId);

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _updater.Clear();
#endif
        }

        /// <summary>
        /// Applies a change to the badge's number.
        /// </summary>
        /// <param name="notification">The object that supplies the new XML definition for the badge.</param>
        public void Update(BadgeNotification notification)
        {
#if __MAC__
            if (notification.Glyph != char.MinValue)
            {
                NSApplication.SharedApplication.DockTile.BadgeLabel = notification.Glyph.ToString();
            }
            else
            {
                if (notification.Value < 1)
                {
                    Clear();
                    return;
                }

                NSApplication.SharedApplication.DockTile.BadgeLabel = notification.Value.ToString();
            }

#elif __UNIFIED__
            UpdateImpl(notification);

#elif TIZEN
            //Tizen.Applications.Update(Tizen.Applications.Application.Current.ApplicationInfo.ApplicationId, (int)notification.Value);

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _updater.Update(notification);
#endif
        }
    }
}