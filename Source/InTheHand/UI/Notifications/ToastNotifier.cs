//-----------------------------------------------------------------------
// <copyright file="ToastNotifier.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.UI.Notifications
{
    /// <summary>
    /// Raises a toast notification to the specific app to which the ToastNotifier is bound.
    /// This class also lets you schedule and remove toast notifications.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>watchOS</term><description>watchOS 2.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.1 or later</description></item></list>
    /// </remarks>
    public sealed partial class ToastNotifier
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private Windows.UI.Notifications.ToastNotifier _notifier;

        internal ToastNotifier(Windows.UI.Notifications.ToastNotifier notifier)
        {
            _notifier = notifier;
        }

        public static implicit operator Windows.UI.Notifications.ToastNotifier(ToastNotifier tn)
        {
            return tn._notifier;
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

#elif __UNIFIED__
            ShowImpl(notification);

#elif TIZEN
            Tizen.Applications.Notifications.NotificationManager.PostToastMessage(notification.Title + "\r\n" + notification.Content);

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
#if __UNIFIED__
            HideImpl(notification);

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _notifier.Hide(notification._notification);

#else
            throw new PlatformNotSupportedException();
#endif
        }

#if !TIZEN
        /// <summary>
        /// Adds a <see cref="ScheduledToastNotification"/> for later display.
        /// </summary>
        /// <param name="scheduledToast">The scheduled toast notification, which includes its content and timing instructions.</param>
        public void AddToSchedule(ScheduledToastNotification scheduledToast)
        {
#if __UNIFIED__
            AddToScheduleImpl(scheduledToast);

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _notifier.AddToSchedule(scheduledToast._notification);

#else
            throw new PlatformNotSupportedException();
#endif
        }


        /// <summary>
        /// Cancels the scheduled display of a specified <see cref="ScheduledToastNotification"/>.
        /// </summary>
        /// <param name="scheduledToast">The scheduled toast notification, which includes its content and timing instructions.</param>
        public void RemoveFromSchedule(ScheduledToastNotification scheduledToast)
        {
#if __UNIFIED__
            RemoveFromScheduleImpl(scheduledToast);

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _notifier.RemoveFromSchedule(scheduledToast._notification);

#else
            throw new PlatformNotSupportedException();
#endif
        }
#endif
    }
}