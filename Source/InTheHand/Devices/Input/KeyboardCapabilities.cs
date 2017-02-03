// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyboardCapabilities.cs" company="In The Hand Ltd">
//   Copyright (c) 2016-17 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;

namespace InTheHand.Devices.Input
{
    /// <summary>
    /// Supports the ability to determine the capabilities of any connected hardware keyboards.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item></list>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 8 or later</description></item></list></remarks>
    /// </remarks>
    /// <seealso cref="global::System.Environment"/>
    public sealed class KeyboardCapabilities
    {

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        private Windows.Devices.Input.KeyboardCapabilities _keyboardCapabilities;

        public static implicit operator Windows.Devices.Input.KeyboardCapabilities(KeyboardCapabilities kc)
        {
            return kc._keyboardCapabilities;
        }
#elif WIN32
        private global::System.Type _type10 = global::System.Type.GetType("Windows.Devices.Input.KeyboardCapabilities, Windows, ContentType=WindowsRuntime");
#endif
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardCapabilities"/> class.
        /// </summary>
        public KeyboardCapabilities()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            _keyboardCapabilities = new Windows.Devices.Input.KeyboardCapabilities();
#endif
        }

        /// <summary>
        /// Gets a value that indicates whether a device identifying itself as a keyboard is detected.
        /// </summary>
        /// <value>Returns 1 if a device identifying itself as a keyboard is detected. Otherwise, returns 0.</value>
        public int KeyboardPresent
        {
            get
            {
#if __ANDROID__
                return Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.Resources.Configuration.Keyboard == Android.Content.Res.KeyboardType.Nokeys ? 0 : 1;
#elif __MAC__
                // this is a hack, but all Macs have a keyboard so return 1 until an API can be identified
                return 1;
#elif WINDOWS_PHONE
                return Microsoft.Phone.Info.DeviceStatus.IsKeyboardPresent ? 1 : 0;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _keyboardCapabilities.KeyboardPresent;
#elif TIZEN
                bool keyb = false;
                Tizen.System.SystemInfo.TryGetValue("http://tizen.org/feature/input.keyboard", out keyb);
                return keyb ? 1 : 0;
#elif WIN32
                if(_type10 != null)
                {
                    object kc = global::System.Activator.CreateInstance(_type10);
                    return (int)_type10.GetRuntimeProperty("KeyboardPresent").GetValue(kc);
                }

                return 0;
#else
                return 0;
#endif
            }
        }
    }
}