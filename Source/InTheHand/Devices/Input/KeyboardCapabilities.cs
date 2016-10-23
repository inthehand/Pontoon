// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyboardCapabilities.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Devices.Input
{
    /// <summary>
    /// Supports the ability to determine the capabilities of any connected hardware keyboards.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item></list>
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
#if WINDOWS_PHONE
                return Microsoft.Phone.Info.DeviceStatus.IsKeyboardPresent ? 1 : 0;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _keyboardCapabilities.KeyboardPresent;
#else
                return 0;
#endif
            }
        }
    }
}