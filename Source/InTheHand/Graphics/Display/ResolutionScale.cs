//-----------------------------------------------------------------------
// <copyright file="ResolutionScale.cs" company="In The Hand Ltd">
//     Copyright © 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
using System.Runtime.CompilerServices;
#pragma warning disable 618
[assembly: TypeForwardedTo(typeof(Windows.Graphics.Display.ResolutionScale))]
#else

namespace Windows.Graphics.Display
{
    /// <summary>
    /// Describes the scale factor of the immersive environment.
    /// The scale factor is determined by the operating system in response to high pixel density screens.
    /// </summary>
    public enum ResolutionScale
    {
        /// <summary>
        /// Specifies the scale of a display is invalid.
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// Specifies the scale of a display as 100 percent. 
        /// This percentage indicates a screen resolution of 480x800.
        /// </summary>
        Scale100Percent = 100,
        /// <summary>
        /// Specifies the scale of a display as 150 percent. 
        /// This percentage indicates a screen resolution of 720x1280.
        /// </summary>
        Scale150Percent = 150,
        /// <summary>
        /// Specifies the scale of a display as 160 percent. 
        /// This percentage indicates a screen resolution of 768x1280.
        /// </summary>
        Scale160Percent = 160,
        /// <summary>
        /// Specifies the scale of a display as 225 percent. 
        /// This percentage indicates a screen resolution of 1080x1920.
        /// </summary>
        Scale225Percent = 225,
    }
}
#endif