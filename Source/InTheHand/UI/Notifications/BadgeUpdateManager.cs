//-----------------------------------------------------------------------
// <copyright file="BadgeUpdateManager.cs" company="In The Hand Ltd">
//     Copyright © 2014-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.UI.Notifications.BadgeUpdateManager))]
//#else

namespace InTheHand.UI.Notifications
{
    /// <summary>
    /// Creates BadgeUpdater objects that you use to manipulate a tile's badge overlay.
    /// </summary>
    public static class BadgeUpdateManager
    {
        private static BadgeUpdater _updater = null;

        /// <summary>
        /// Creates and initializes a new instance of the BadgeUpdater, which lets you change the appearance or content of the badge on the calling app's tile.
        /// </summary>
        /// <returns></returns>
        public static BadgeUpdater CreateBadgeUpdaterForApplication()
        {
            if(_updater == null)
            {
#if __IOS__
                _updater = new BadgeUpdater();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                _updater = new BadgeUpdater(Windows.UI.Notifications.BadgeUpdateManager.CreateBadgeUpdaterForApplication());
#endif
            }

            return _updater;
        }
    }
}
//#endif