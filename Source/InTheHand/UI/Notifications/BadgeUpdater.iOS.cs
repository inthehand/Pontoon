//-----------------------------------------------------------------------
// <copyright file="BadgeUpdater.iOS.cs" company="In The Hand Ltd">
//     Copyright © 2017 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Foundation;
using System;
using UserNotifications;

namespace InTheHand.UI.Notifications
{
    public sealed partial class BadgeUpdater
    {
        private static bool s_granted;

        static BadgeUpdater()
        {
            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Badge, RequestAuthorizationComplete);
        }

        private static void RequestAuthorizationComplete(bool success, NSError error)
        {
            s_granted = success;
        }
        
        private void ClearImpl()
        {
            if (s_granted)
            {
                UNUserNotificationCenter.Current.AddNotificationRequest(UNNotificationRequest.FromIdentifier(Guid.NewGuid().ToString(), new BadgeNotification(0)._content, null), null);
            }
        }
        
        private void UpdateImpl(BadgeNotification notification)
        {
            if (s_granted)
            {
                var request = UNNotificationRequest.FromIdentifier(Guid.NewGuid().ToString(), notification._content, null);
                UNUserNotificationCenter.Current.AddNotificationRequest(request, null);
            }
        }
    }
}