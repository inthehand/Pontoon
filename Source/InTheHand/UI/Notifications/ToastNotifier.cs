//-----------------------------------------------------------------------
// <copyright file="ToastNotifier.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.UI.Notifications.ToastNotifier))]
#else

#if __IOS__
using UIKit;
#endif

using System;

namespace Windows.UI.Notifications
{
    /// <summary>
    /// Updates a badge overlay on the specific tile that the updater is bound to.
    /// </summary>
    public sealed class ToastNotifier
    {
        internal ToastNotifier()
        {
        }

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
#elif WINDOWS_PHONE
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Adds a <see cref="ScheduledToastNotification"/> for later display.
        /// </summary>
        /// <param name="scheduledToast"></param>
        public void AddToSchedule(ScheduledToastNotification scheduledToast)
        {
#if __ANDROID__
            throw new PlatformNotSupportedException();
#elif __IOS__
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.ScheduleLocalNotification(scheduledToast._localNotification);
            });
#elif WINDOWS_PHONE
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Cancels the scheduled display of a specified <see cref="ScheduledToastNotification"/>.
        /// </summary>
        /// <param name="scheduledToast"></param>
        public void RemoveFromSchedule(ScheduledToastNotification scheduledToast)
        {
#if __ANDROID__
            throw new PlatformNotSupportedException();
#elif __IOS__
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.CancelLocalNotification(scheduledToast._localNotification);
            });
#elif WINDOWS_PHONE
            throw new PlatformNotSupportedException();
#endif
        }
    }
}
#endif