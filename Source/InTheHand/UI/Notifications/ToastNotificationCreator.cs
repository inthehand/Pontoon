//-----------------------------------------------------------------------
// <copyright file="ToastNotificationCreator.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
#elif __ANDROID__
using Android.Widget;

#elif __UNIFIED__
using Foundation;
#if !__MAC__
using UserNotifications;
#endif
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
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>watchOS</term><description>watchOS 2.0 and later</description></item>
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
            return new Windows.UI.Notifications.ToastNotification(doc);

#elif WINDOWS_PHONE
            return new ToastNotification(new Microsoft.Phone.Shell.ShellToast() { Title = title, Content = content });

#elif __ANDROID__
            return Toast.MakeText(Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity, title + "\r\n" + content, ToastLength.Long);

#elif __MAC__
			NSUserNotification notification = new NSUserNotification();
			notification.Title = title;
			notification.InformativeText = content;
            notification.SoundName = NSUserNotification.NSUserNotificationDefaultSoundName;
			return notification;

#elif __UNIFIED__
            UNMutableNotificationContent notificationContent = new UNMutableNotificationContent();
            notificationContent.Title = title;
            notificationContent.Body = content;
            notificationContent.Sound = UNNotificationSound.Default;
            return new ToastNotification(notificationContent);

#else
            return new ToastNotification(content, title);
#endif
        }

#if !TIZEN
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
#elif __MAC__
			NSUserNotification notification = new NSUserNotification();
			notification.Title = title;
			notification.InformativeText = content;
            notification.SoundName = NSUserNotification.NSUserNotificationDefaultSoundName;
            notification.DeliveryDate = deliveryTime.ToNSDate();
			return notification;

#elif __UNIFIED__

            UNMutableNotificationContent notificationContent = new UNMutableNotificationContent();
            notificationContent.Title = title;
            notificationContent.Body = content;
            notificationContent.Sound = UNNotificationSound.Default;
            NSDateComponents dc = NSCalendar.CurrentCalendar.Components(NSCalendarUnit.Year | NSCalendarUnit.Month | NSCalendarUnit.Day | NSCalendarUnit.Hour | NSCalendarUnit.Minute | NSCalendarUnit.Second, deliveryTime.ToNSDate());
            UNCalendarNotificationTrigger trigger = UNCalendarNotificationTrigger.CreateTrigger(dc, false);

            return new ScheduledToastNotification(notificationContent, trigger);

#else
            return new ScheduledToastNotification(content, title, deliveryTime);
#endif
        }
#endif
    }
}