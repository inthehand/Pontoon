//-----------------------------------------------------------------------
// <copyright file="ToastNotifier.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if __IOS__
using UIKit;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
using Windows.UI.Notifications;
#endif

using System;

namespace InTheHand.UI.Notifications
{
    /// <summary>
    /// Updates a badge overlay on the specific tile that the updater is bound to.
    /// </summary>
    public sealed class ToastNotifier
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private Windows.UI.Notifications.ToastNotifier _notifier;

        internal ToastNotifier(Windows.UI.Notifications.ToastNotifier notifier)
        {
            _notifier = notifier;
        }
#else
        internal ToastNotifier()
        {
        }
#endif

        /// <summary>
        /// Shows a toast notification.
        /// </summary>
        /// <param name="notification">The object that supplies the new XML definition for the toast.</param>
        public void Show(ToastNotification notification)
        {
#if __ANDROID__
            notification._toast.Show();
#elif __IOS__
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.PresentLocalNotificationNow(notification._localNotification);
            });
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _notifier.Show(notification._notification);
#elif WINDOWS_PHONE
            notification._shellToast.Show();
#endif
        }

        /// <summary>
        /// Hides a toast notification.
        /// </summary>
        /// <param name="notification">The object that supplies the new XML definition for the toast.</param>
        public void Hide(ToastNotification notification)
        {
#if __ANDROID__
            throw new PlatformNotSupportedException();
#elif __IOS__
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.CancelLocalNotification(notification._localNotification);
            });
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _notifier.Hide(notification._notification);
#elif WINDOWS_PHONE
            throw new PlatformNotSupportedException();
#endif
        }

        public void AddToSchedule(ScheduledToastNotification scheduledToast)
        {
#if __ANDROID__
            throw new PlatformNotSupportedException();
#elif __IOS__
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.ScheduleLocalNotification(scheduledToast._localNotification);
            });
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _notifier.AddToSchedule(scheduledToast._notification);
#elif WINDOWS_PHONE
            throw new PlatformNotSupportedException();
#endif
        }

        public void RemoveFromSchedule(ScheduledToastNotification scheduledToast)
        {
#if __ANDROID__
            throw new PlatformNotSupportedException();
#elif __IOS__
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.CancelLocalNotification(scheduledToast._localNotification);
            });
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _notifier.RemoveFromSchedule(scheduledToast._notification);
#elif WINDOWS_PHONE
            throw new PlatformNotSupportedException();
#endif
        }
    }
}