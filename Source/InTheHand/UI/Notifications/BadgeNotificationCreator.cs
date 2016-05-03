//-----------------------------------------------------------------------
// <copyright file="BadgeNotificationCreator.cs" company="In The Hand Ltd">
//     Copyright © 2014-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using Windows.Data.Xml.Dom;
#endif

using System;

namespace InTheHand.UI.Notifications
{
    /// <summary>
    /// Simplifies creation of badges without the need to build XML documents.
    /// </summary>
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
            XmlDocument doc = Windows.UI.Notifications.BadgeUpdateManager.GetTemplateContent(Windows.UI.Notifications.BadgeTemplateType.BadgeNumber);
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