//-----------------------------------------------------------------------
// <copyright file="ToastNotificationCreator.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
#elif __ANDROID__
using Android.Widget;
#elif __IOS__
using UIKit;
#endif

namespace InTheHand.UI.Notifications
{
    /// <summary>
    /// Simplifies creation of toasts without the need to build XML documents.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.1 or later</description></item></list>
    /// </remarks>
    public static class ToastNotificationCreator
    {
        /// <summary>
        /// Creates a toast notification with the required values.
        /// </summary>
        /// <param name="content">Text content.</param>
        /// <param name="title">Toast title.</param>
        /// <returns></returns>
        public static ToastNotification CreateToastNotification(string content, string title)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            XmlDocument doc = Windows.UI.Notifications.ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var textElements = doc.GetElementsByTagName("text");
            textElements[0].InnerText = title;
            textElements[1].InnerText = content;
            return new ToastNotification(new Windows.UI.Notifications.ToastNotification(doc));
#elif WINDOWS_PHONE
            return new ToastNotification(new Microsoft.Phone.Shell.ShellToast() { Title = title, Content = content });
#elif __ANDROID__
            Toast toast = Toast.MakeText(Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity, title + "\r\n" + content, ToastLength.Long);
            return new ToastNotification(toast);
#elif __IOS__
            UILocalNotification localNotification = new UILocalNotification();
            localNotification.AlertTitle = title;
            localNotification.AlertBody = content;
            localNotification.SoundName = UILocalNotification.DefaultSoundName;
            //localNotification.RepeatCalendar = global::Foundation.NSCalendar.CurrentCalendar;
            //localNotification.RepeatInterval = global::Foundation.NSCalendarUnit.Minute;

            return new ToastNotification(localNotification);
#else
            return new ToastNotification(content, title);
#endif
        }

        /// <summary>
        /// Creates a scheduled toast notification with the required values.
        /// </summary>
        /// <param name="content">Text content.</param>
        /// <param name="title">Toast title.</param>
        /// <param name="deliveryTime">When to display the toast.</param>
        /// <returns></returns>
        public static ScheduledToastNotification CreateScheduledToastNotification(string content, string title, DateTimeOffset deliveryTime)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            XmlDocument doc = Windows.UI.Notifications.ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var textElements = doc.GetElementsByTagName("text");
            textElements[0].InnerText = title;
            textElements[1].InnerText = content;
            return new ScheduledToastNotification(new Windows.UI.Notifications.ScheduledToastNotification(doc, deliveryTime));
#elif WINDOWS_PHONE
            throw new PlatformNotSupportedException();
#elif __ANDROID__
            throw new PlatformNotSupportedException();
#elif __IOS__
            UILocalNotification localNotification = new UILocalNotification();
            localNotification.AlertTitle = title;
            localNotification.AlertBody = content;
            localNotification.FireDate = deliveryTime.ToNSDate();
            localNotification.SoundName = UILocalNotification.DefaultSoundName;
            localNotification.RepeatCalendar = global::Foundation.NSCalendar.CurrentCalendar;
            localNotification.RepeatInterval = global::Foundation.NSCalendarUnit.Minute;

            return new ScheduledToastNotification(localNotification);
#else
            return new ScheduledToastNotification(content, title, deliveryTime);
#endif
        }
    }
}