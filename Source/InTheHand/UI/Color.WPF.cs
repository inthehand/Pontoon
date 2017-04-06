//-----------------------------------------------------------------------
// <copyright file="ColorExtensions.WPF.cs" company="In The Hand Ltd">
//     Copyright © 2017 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.UI
{
    public static class ColorExtensions
    {
        public static global::System.Windows.Media.Color ToNative(this Color c)
        {
            return global::System.Windows.Media.Color.FromArgb(c.A,c.R,c.G,c.B);
        }
    }
}