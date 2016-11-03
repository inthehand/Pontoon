//-----------------------------------------------------------------------
// <copyright file="BadgeNotificationCreator.cs" company="In The Hand Ltd">
//     Copyright © 2014-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
#endif

using System;

namespace InTheHand.UI.Notifications
{
    /// <summary>
    /// Simplifies creation of badges without the need to build XML documents.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.1 or later</description></item></list>
    /// </remarks>
    public static class BadgeNotificationCreator
    {
        /// <summary>
        /// Creates a badge notification with the required numerical value.
        /// </summary>
        /// <param name="value">Value to show on the badge. Zero will hide the badge.</param>
        /// <returns></returns>
        public static BadgeNotification CreateBadgeNotification(int value)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            XmlDocument doc = Windows.UI.Notifications.BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);
            var badgeElements = doc.GetElementsByTagName("badge");
            badgeElements[0].Attributes[0].InnerText = value.ToString();
            return new BadgeNotification(new Windows.UI.Notifications.BadgeNotification(doc));
#elif WINDOWS_PHONE
            throw new PlatformNotSupportedException();
#else
            return new BadgeNotification(value);
#endif
        }
    }
}