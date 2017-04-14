//-----------------------------------------------------------------------
// <copyright file="ScheduledToastNotification.iOS.cs" company="In The Hand Ltd">
//     Copyright © 2017 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using Foundation;
using UserNotifications;

namespace InTheHand.UI.Notifications
{
    public sealed partial class ScheduledToastNotification
    {
        internal UNMutableNotificationContent _content;
        internal UNCalendarNotificationTrigger _trigger;
        internal UNNotificationRequest _request;

        internal ScheduledToastNotification(UNMutableNotificationContent content, UNCalendarNotificationTrigger trigger)
        {
            _content = content;
            _trigger = trigger;
        }

        private DateTimeOffset GetDeliveryTime()
        {
            return InTheHand.DateTimeOffsetHelper.FromNSDate(_trigger.DateComponents.Date);
        }

        private string GetGroup()
        {
            return _content.CategoryIdentifier;
        }

        private void SetGroup(string value)
        {
            _content.CategoryIdentifier = value;
        }

        private bool GetSuppressPopup()
        {
            if (_content.UserInfo.ContainsKey(new NSString("SuppressPopup")))
            {
                return ((NSNumber)_content.UserInfo["SuppressPopup"]).BoolValue;
            }
            return false;
        }

        private void SetSuppressPopup(bool value)
        {
            _content.UserInfo.SetValueForKey(NSNumber.FromBoolean(value), new NSString("SuppressPopup"));
        }


        private string GetTag()
        {
            if (_content.UserInfo.ContainsKey(new NSString("Tag")))
            {
                return ((NSString)_content.UserInfo["Tag"]).ToString();
            }
            return string.Empty;
        }

        private void SetTag(string value)
        {
            _content.UserInfo.SetValueForKey(new NSString(value), new NSString("Tag"));
        }
    }
}