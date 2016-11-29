//-----------------------------------------------------------------------
// <copyright file="BadgeGlyph.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.UI.Notifications
{
    /// <summary>
    /// Specifies a set of possible badge glyphs.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item></list></remarks>
    public enum BadgeGlyph
    {
        /// <summary>
        /// No badge shown.
        /// </summary>
        None,

        /// <summary>
        /// *
        /// </summary>
        Alert,

        /// <summary>
        /// !
        /// </summary>
        Attention,
    }
}