// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesignMode.cs" company="In The Hand Ltd">
//   Copyright (c) 2013-15 In The Hand Ltd, All rights reserved.
// </copyright>
// <summary>
//   Provides package identification info, such as name, version, and publisher.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

#if WINDOWS_PHONE
    using System.Windows;
#endif

namespace InTheHand.ApplicationModel
{
    /// <summary>
    /// Enables you to detect whether your app is in design mode in a visual designer.
    /// </summary>
    public static class DesignMode
    {
        private static bool? designModeEnabled;

        /// <summary>
        /// Gets a value that indicates whether the process is running in design mode.
        /// </summary>
        /// <value>True if the process is running in design mode; otherwise false.</value>
        public static bool DesignModeEnabled
        {
            get
            {
                if (!designModeEnabled.HasValue)
                {
#if __ANDROID__ || __IOS__
                    designModeEnabled = false;

#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
                    designModeEnabled = Windows.ApplicationModel.DesignMode.DesignModeEnabled;
#elif WINDOWS_PHONE
                    designModeEnabled = (null == Application.Current) || Application.Current.GetType() == typeof(Application);
#else
                    throw new PlatformNotSupportedException();
#endif
                }

                return designModeEnabled.Value;
            }
        }
    }
}