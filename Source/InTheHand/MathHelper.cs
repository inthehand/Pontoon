// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MathHelper.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace InTheHand
{
    /// <summary>
    /// Provides additional Math operations.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>watchOS</term><description>watchOS 2.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public static class MathHelper
    {
        /// <summary>
        /// Restricts a value to be within a specified range. 
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="minimum">The minimum value. 
        /// If value is less than min, min will be returned. </param>
        /// <param name="maximum">The maximum value. 
        /// If value is greater than max, max will be returned. </param>
        /// <returns></returns>
        public static T Clamp<T>(T value, T minimum, T maximum)
            where T : IComparable<T>
        {
            if (value.CompareTo(minimum) < 0) return minimum;
            else if (value.CompareTo(maximum) > 0) return maximum;
            else return value;
        }
    }
}