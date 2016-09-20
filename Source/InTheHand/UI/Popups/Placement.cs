// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Placement.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// <summary>
//   Represents a command in a context menu.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.UI.Popups.Placement))]
#else

#if __IOS__
using UIKit;
#endif

namespace Windows.UI.Popups
{
    /// <summary>
    /// Specifies where the context menu should be positioned relative to the selection rectangle
    /// </summary>
    public enum Placement
    {
        /// <summary>
        /// 
        /// </summary>
        Default = 0,
        /// <summary>
        /// Place the context menu above the selection rectangle.
        /// </summary>
        Above = 1,
        /// <summary>
        /// Place the context menu below the selection rectangle.
        /// </summary>
        Below = 2,
        /// <summary>
        /// Place the context menu to the left of the selection rectangle.
        /// </summary>
        Left = 3,
        /// <summary>
        /// Place the context menu to the right of the selection rectangle.
        /// </summary>
        Right = 4,
    }


}
#endif

#if __IOS__

namespace InTheHand.UI.Popups
{
    internal static class PlacementHelper
    {
        public static UIPopoverArrowDirection ToArrowDirection(Windows.UI.Popups.Placement placement)
        {
            switch(placement)
            {
                case Windows.UI.Popups.Placement.Above:
                    return UIPopoverArrowDirection.Down;

                case Windows.UI.Popups.Placement.Below:
                    return UIPopoverArrowDirection.Up;

                case Windows.UI.Popups.Placement.Left:
                    return UIPopoverArrowDirection.Right;

                case Windows.UI.Popups.Placement.Right:
                    return UIPopoverArrowDirection.Left;

                default:
                    return UIPopoverArrowDirection.Any;
            }
        }
    }
}
#endif