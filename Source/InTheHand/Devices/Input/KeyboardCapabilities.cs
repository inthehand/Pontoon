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
    public sealed class KeyboardCapabilities
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardCapabilities"/> class.
        /// </summary>
        public KeyboardCapabilities() { }

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
#else
                return 0;
#endif
            }
        }
    }
}