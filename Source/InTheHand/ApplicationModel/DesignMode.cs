// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesignMode.cs" company="In The Hand Ltd">
//   Copyright (c) 2013-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#if WIN32
using System.Reflection;
#endif

namespace InTheHand.ApplicationModel
{
    /// <summary>
    /// Enables you to detect whether your app is in design mode in a visual designer.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public static class DesignMode
    {
        private static bool? designModeEnabled;

        /// <summary>
        /// Gets a value that indicates whether the process is running in design mode.
        /// </summary>
        /// <value>True if the process is running in design mode; otherwise false.</value>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Android</term><description>Android 4.4 and later</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
        /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
        /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
        /// </remarks>
        public static bool DesignModeEnabled
        {
            get
            {
                if (!designModeEnabled.HasValue)
                {
#if __ANDROID__
                    Android.Views.View v = new Android.Views.View(Android.App.Application.Context);
                    designModeEnabled = v.IsInEditMode;

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                    designModeEnabled = Windows.ApplicationModel.DesignMode.DesignModeEnabled;

#elif WINDOWS_PHONE
                    designModeEnabled = (null == global::System.Windows.Application.Current) || global::System.Windows.Application.Current.GetType() == typeof(global::System.Windows.Application);

#elif WIN32
                    // test for WPF
                    var t = global::System.Type.GetType("System.ComponentModel.DesignerProperties, PresentationFramework, Version = 4.0.0.0, Culture = neutral, PublicKeyToken = 31bf3856ad364e35");
                    if(t != null)
                    {
                        var mi = t.GetMethod("GetIsInDesignMode", BindingFlags.Public | BindingFlags.Static);
                        if(mi != null)
                        {
                            var tdo = global::System.Type.GetType("System.Windows.DependencyObject, WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
                            var arg = global::System.Activator.CreateInstance(tdo);
                            var val = mi.Invoke(null, new object[] { arg });
                            designModeEnabled = (bool)val;
                        }
                    }
                    else
                    {
                        using (var process = global::System.Diagnostics.Process.GetCurrentProcess())
                        {
                            // in the absence of a better solution check if running under devenv process
                            designModeEnabled = process.ProcessName.ToLower().Contains("devenv");
                        }
                    }
#else
                    designModeEnabled = false;
#endif
                }

                return designModeEnabled.Value;
            }
        }
    }
}