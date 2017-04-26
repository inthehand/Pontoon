// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Placement.cs" company="In The Hand Ltd">
//   Copyright (c) 2016-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#if __IOS__
using UIKit;
#endif

namespace InTheHand.UI.Popups
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

#if __IOS__
    internal static class PlacementHelper
    {
        public static UIPopoverArrowDirection ToArrowDirection(Placement placement)
        {
            switch(placement)
            {
                case Placement.Above:
                    return UIPopoverArrowDirection.Down;

                case Placement.Below:
                    return UIPopoverArrowDirection.Up;

                case Placement.Left:
                    return UIPopoverArrowDirection.Right;

                case Placement.Right:
                    return UIPopoverArrowDirection.Left;

                default:
                    return UIPopoverArrowDirection.Any;
            }
        }
    }

#endif
}