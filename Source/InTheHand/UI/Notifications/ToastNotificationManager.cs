//-----------------------------------------------------------------------
// <copyright file="ToastNotificationManager.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.UI.Notifications.ToastNotificationManager))]
//#else

namespace InTheHand.UI.Notifications
{
    /// <summary>
    /// Creates ToastNotifier objects which let you display toast notifications.
    /// </summary>
    public static class ToastNotificationManager
    {
        private static ToastNotifier _notifier = null;

        /// <summary>
        /// Creates and initializes a new instance of the ToastNotifier, which lets you display toast notifications.
        /// </summary>
        /// <returns></returns>
        public static ToastNotifier CreateToastNotifierForApplication()
        {
            if(_notifier == null)
            {     
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                _notifier = new ToastNotifier(Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier());
#else
                _notifier = new ToastNotifier();
#endif
            }

            return _notifier;
        }
    }
}
//#endif