//-----------------------------------------------------------------------
// <copyright file="BadgeUpdater.cs" company="In The Hand Ltd">
//     Copyright © 2014-15 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.UI.Notifications.BadgeUpdater))]
#else

#if __IOS__
using UIKit;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
using Windows.UI.Notifications;
#endif

namespace Windows.UI.Notifications
{
    /// <summary>
    /// Updates a badge overlay on the specific tile that the updater is bound to.
    /// </summary>
    public sealed class BadgeUpdater
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
#if __IOS__
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
            });
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
#if __IOS__
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.ApplicationIconBadgeNumber = notification.Value;
            });
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _updater.Update(notification._notification);
#endif
        }
    }
}
#endif