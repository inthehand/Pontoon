// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesignMode.cs" company="In The Hand Ltd">
//   Copyright (c) 2013-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.ApplicationModel.DesignMode))]
#else
using System;

#if WINDOWS_PHONE
using System.Windows;
#endif

namespace Windows.ApplicationModel
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
#endif