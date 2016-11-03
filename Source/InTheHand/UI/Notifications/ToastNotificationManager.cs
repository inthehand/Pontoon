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
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Android</term><description>Android 4.4 and later</description></item>
        /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
        /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
        /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.1 or later</description></item></list>
        /// </remarks>
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