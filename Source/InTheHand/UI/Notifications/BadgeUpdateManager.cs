//-----------------------------------------------------------------------
// <copyright file="BadgeUpdateManager.cs" company="In The Hand Ltd">
//     Copyright © 2014-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.UI.Notifications
{
    /// <summary>
    /// Creates <see cref="BadgeUpdater"/> objects that you use to manipulate a tile's badge overlay.
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
    public static class BadgeUpdateManager
    {
        private static BadgeUpdater _updater = null;

        /// <summary>
        /// Creates and initializes a new instance of the <see cref="BadgeUpdater"/>, which lets you change the appearance or content of the badge on the calling app's tile.
        /// </summary>
        /// <returns>The object you will use to send changes to the app tile's badge.</returns>
        public static BadgeUpdater CreateBadgeUpdaterForApplication()
        {
            if(_updater == null)
            {
#if __UNIFIED__ || TIZEN
                _updater = new BadgeUpdater();

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                _updater = new BadgeUpdater(Windows.UI.Notifications.BadgeUpdateManager.CreateBadgeUpdaterForApplication());
#endif
            }

            return _updater;
        }
    }
}