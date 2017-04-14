//-----------------------------------------------------------------------
// <copyright file="ToastNotification.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if __ANDROID__
using Android.Widget;
#elif __MAC__
using Foundation;
#elif WINDOWS_PHONE
using Microsoft.Phone.Shell;
#endif

namespace InTheHand.UI.Notifications
{
    /// <summary>
    /// Defines the content, associated metadata and events, and expiration time of a toast notification.
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
    /// <seealso cref="ToastNotificationCreator"/>
    public sealed partial class ToastNotification
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private Windows.UI.Notifications.ToastNotification _notification;

        private ToastNotification(Windows.UI.Notifications.ToastNotification notification)
        {
            _notification = notification;
        }

        public static implicit operator ToastNotification(Windows.UI.Notifications.ToastNotification n)
        {
            return new ToastNotification(n);
        }

        public static implicit operator Windows.UI.Notifications.ToastNotification(ToastNotification n)
        {
            return n._notification;
        }
#elif WINDOWS_PHONE
        internal ShellToast _shellToast;

        internal ToastNotification(ShellToast shellToast)
        {
            _shellToast = shellToast;
        }
#elif __ANDROID__
        private Toast _toast;

        private ToastNotification(Toast toast)
        {
            _toast = toast;
        }

        public static implicit operator ToastNotification(Toast t)
        {
            return new ToastNotification(t);
        }

        public static implicit operator Toast(ToastNotification n)
        {
            return n._toast;
        }

#elif __MAC__
		private NSUserNotification _notification;

		private ToastNotification(NSUserNotification notification)
		{
			_notification = notification;
		}

		public static implicit operator NSUserNotification(ToastNotification tn)
		{
			return tn._notification;
		}

		public static implicit operator ToastNotification(NSUserNotification un)
		{
			return new ToastNotification(un);
		}

#elif __UNIFIED__
#else
        internal ToastNotification(string content, string title)
        {
            Title = title;
            Content = content;
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
        /// Gets or sets the group identifier for the notification.
        /// </summary>
        /// <value>The group identifier for the notification.</value>
        /// <remarks>Not used on macOS. On iOS this maps to the CategoryIdentifier property.</remarks>
        public string Group
        {

            get
            {
#if WINDOWS_UWP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _notification.Group;
#elif __MAC__
				return string.Empty;

#elif __UNIFIED__
                return GetGroup();
#else
                return string.Empty;
#endif
            }
            set
            {
#if WINDOWS_UWP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                _notification.Group = value;
#elif __MAC__

#elif __UNIFIED__
                SetGroup(value);
#endif
            }
        }

        /// <summary>
        /// Gets or sets whether a toast's pop-up UI is displayed on the user's screen.
        /// </summary>
        public bool SuppressPopup
        {

            get
            {
#if WINDOWS_UWP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _notification.SuppressPopup;

#elif __MAC__
                return false;

#elif __UNIFIED__
                return GetSuppressPopup();
#else
                return false;
#endif
            }
            set
            {
#if WINDOWS_UWP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                _notification.SuppressPopup = value;

#elif __MAC__

#elif __UNIFIED__
                SetSuppressPopup(value);
#endif
            }
        }

        /// <summary>
        /// Gets or sets the unique identifier of this notification within the notification Group.
        /// </summary>
        /// <remarks>
        /// On iOS this maps to the ThreadIdentifier property and on macOS the Identifier property.</remarks>
        public string Tag
        {

            get
            {
#if WINDOWS_UWP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _notification.Tag;
#elif __MAC__
				return _notification.Identifier;

#elif __UNIFIED__
                return GetTag();
#else
                return string.Empty;
#endif
            }
            set
            {
#if WINDOWS_UWP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                _notification.Tag = value;

#elif __MAC__
				_notification.Identifier = value;

#elif __UNIFIED__
                SetTag(value);
#endif
            }
        }
    }
}