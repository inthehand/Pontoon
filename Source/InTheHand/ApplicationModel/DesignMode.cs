// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesignMode.cs" company="In The Hand Ltd">
//   Copyright (c) 2013-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                    designModeEnabled = Windows.ApplicationModel.DesignMode.DesignModeEnabled;
#elif WINDOWS_PHONE
                    designModeEnabled = (null == global::System.Windows.Application.Current) || global::System.Windows.Application.Current.GetType() == typeof(global::System.Windows.Application);
#else
                    designModeEnabled = false;
#endif
                }

                return designModeEnabled.Value;
            }
        }
    }
}