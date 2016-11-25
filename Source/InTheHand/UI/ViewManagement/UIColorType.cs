//-----------------------------------------------------------------------
// <copyright file="UIColorType.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.UI.ViewManagement
{
    /// <summary>
    /// Defines constants that specify known system color values.
    /// </summary>
    public enum UIColorType
    {
        /// <summary>
        /// The background color.
        /// </summary>
        Background = 0,
        /// <summary>
        /// The foreground color.
        /// </summary>
        Foreground = 1,

        //AccentDark3 | accentDark3
        //2
        //The darkest accent color.
        //AccentDark2 | accentDark2
        //3
        //The darker accent color.
        //AccentDark1 | accentDark1
        //4
        //The dark accent color.

        /// <summary>
        /// The accent color.
        /// </summary>
        Accent = 5,

        //AccentLight1 | accentLight1
        //6
        //The light accent color.
        //AccentLight2 | accentLight2
        //7
        //The lighter accent color.
        //AccentLight3 | accentLight3
        //8
        //The lightest accent color.
    }
}