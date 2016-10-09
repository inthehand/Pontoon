﻿//-----------------------------------------------------------------------
// <copyright file="ScheduledToastNotification.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.UI.Notifications.ScheduledToastNotification))]
#else

using System;
#if __IOS__
using Foundation;
using UIKit;
#endif

namespace Windows.UI.Notifications
{
    /// <summary>
    /// Contains the definition of the toast notification that will display at the scheduled time.
    /// </summary>
    public sealed class ScheduledToastNotification
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        internal Windows.UI.Notifications.ScheduledToastNotification _notification;

        internal ScheduledToastNotification(Windows.UI.Notifications.ScheduledToastNotification notification)
        {
            _notification = notification;
        }
#elif __IOS__
        internal UILocalNotification _localNotification;

        internal ScheduledToastNotification(UILocalNotification localNotification)
        {
            _localNotification = localNotification;
        }
#else


        internal ScheduledToastNotification(string content, string title, DateTimeOffset deliveryTime)
        {
            Title = title;
            Content = content;
            DeliveryTime = deliveryTime;
        }

        internal string Title
        {
            get;
            private set;
        }

        internal string Content
        {
            get;
            private set;
        }


#endif

        /// <summary>
        /// Gets the time that this toast notification is scheduled to be displayed.
        /// </summary>
        public DateTimeOffset DeliveryTime
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            get
            {
                return _notification.DeliveryTime;
            }
#elif __IOS__
            get
            {
                return InTheHand.DateTimeOffsetHelper.FromNSDate(_localNotification.FireDate);
            }
#else
            get;
            private set;
#endif
        }

        public string Group
        {

            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _notification.Group;
#elif __IOS__
                return _localNotification.Category;
#else
                return string.Empty;
#endif
            }
            set
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                _notification.Group = value;
#elif __IOS__
                _localNotification.Category = value;
#endif
            }
        }

        public bool SuppressPopup
        {

            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _notification.SuppressPopup;
#elif __IOS__
                if (_localNotification.UserInfo.ContainsKey(new NSString("SuppressPopup")))
                {
                    return ((NSNumber)_localNotification.UserInfo["SuppressPopup"]).BoolValue;
                }
                return false;
#else
                return false;
#endif
            }
            set
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                _notification.SuppressPopup = value;
#elif __IOS__
                _localNotification.UserInfo.SetValueForKey(NSNumber.FromBoolean(value), new NSString("SuppressPopup"));
#endif
            }
        }

        public string Tag
        {

            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _notification.Tag;
#elif __IOS__
                if (_localNotification.UserInfo.ContainsKey(new NSString("Tag")))
                {
                    return ((NSString)_localNotification.UserInfo["Tag"]).ToString();
                }
                return string.Empty;
#else
                return string.Empty;
#endif
            }
            set
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                _notification.Tag = value;
#elif __IOS__
                _localNotification.UserInfo.SetValueForKey(new NSString(value), new NSString("Tag"));
#endif
            }
        }
    }
}
#endif