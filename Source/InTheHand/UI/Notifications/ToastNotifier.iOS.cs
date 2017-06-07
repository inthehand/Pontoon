//-----------------------------------------------------------------------
// <copyright file="ToastNotifier.iOS.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using UserNotifications;
using System;
using Foundation;

namespace InTheHand.UI.Notifications
{
    partial class ToastNotifier
    {
        private static bool s_granted;

        static ToastNotifier()
        {
            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound, RequestAuthorizationComplete);

            if (UNUserNotificationCenter.Current.Delegate == null)
            {
                UNUserNotificationCenter.Current.Delegate = new NotificationDelegate();
            }
        }

        private static void RequestAuthorizationComplete(bool success, NSError error)
        {
            s_granted = success;
        }
        
        private void ShowImpl(ToastNotification notification)
        {
            if (s_granted)
            {
                notification._request = UNNotificationRequest.FromIdentifier(Guid.NewGuid().ToString(), notification._content, null);
                UNUserNotificationCenter.Current.AddNotificationRequest(notification._request, null);
            }
        }
        
        private void HideImpl(ToastNotification notification)
        {
            if (s_granted)
            {
                if (notification._request != null)
                {
                    UNUserNotificationCenter.Current.RemoveDeliveredNotifications(new string[] { notification._request.Identifier });
                }
            }
        }
        
        private void AddToScheduleImpl(ScheduledToastNotification scheduledToast)
        {
            if (s_granted)
            {
                scheduledToast._request = UNNotificationRequest.FromIdentifier(Guid.NewGuid().ToString(), scheduledToast._content, scheduledToast._trigger);
                UNUserNotificationCenter.Current.AddNotificationRequest(scheduledToast._request, null);
            }
        }
        
        private void RemoveFromScheduleImpl(ScheduledToastNotification scheduledToast)
        {
            if (s_granted)
            {
                if (scheduledToast._request != null)
                {
                    UNUserNotificationCenter.Current.RemovePendingNotificationRequests(new string[] { scheduledToast._request.Identifier });
                }
            }
        }
    }
}