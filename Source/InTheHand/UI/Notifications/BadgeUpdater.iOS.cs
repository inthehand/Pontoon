//-----------------------------------------------------------------------
// <copyright file="BadgeUpdater.iOS.cs" company="In The Hand Ltd">
//     Copyright © 2017-18 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Foundation;
using System;
using UserNotifications;
using ObjCRuntime;

namespace InTheHand.UI.Notifications
{
    partial class BadgeUpdater
    {
        private static bool s_granted;

        static BadgeUpdater()
        {
            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Badge, RequestAuthorizationComplete);
            if(UNUserNotificationCenter.Current.Delegate == null)
            {
                UNUserNotificationCenter.Current.Delegate = new NotificationDelegate();
            }
        }

        private static void RequestAuthorizationComplete(bool success, NSError error)
        {
            s_granted = success;
        }
        
        private void ClearImpl()
        {
            if (s_granted)
            {
                var content = new UNMutableNotificationContent();
                content.Badge = -1;

                UNUserNotificationCenter.Current.AddNotificationRequest(UNNotificationRequest.FromIdentifier(Guid.NewGuid().ToString(), content, null), null);
            }
        }
        
        private void UpdateImpl(BadgeNotification notification)
        {
            if (s_granted)
            {

                var request = UNNotificationRequest.FromIdentifier(Guid.NewGuid().ToString(), notification._content, null);
                UNUserNotificationCenter.Current.AddNotificationRequest(request, AddCompleted);
            }
        }

        private void AddCompleted(NSError error)
        {

        }
    }

    internal sealed class NotificationDelegate : UNUserNotificationCenterDelegate   
    {
        // Allows the app to display notifications when added when app is in the foreground
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            completionHandler.Invoke(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Badge | UNNotificationPresentationOptions.Sound);
        }
    }
}