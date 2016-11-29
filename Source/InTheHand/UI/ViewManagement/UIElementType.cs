//-----------------------------------------------------------------------
// <copyright file="UIElementType.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.UI.ViewManagement
{
    /// <summary>
    /// Defines the set of user interface element types.
    /// </summary>
    public enum UIElementType
    {
        /// <summary>
        /// An active caption element.
        /// </summary>
        ActiveCaption = 0,
        
        /// <summary>
        /// A background element.
        /// </summary>
        Background = 1,
        
        /// <summary>
        /// A button face element.
        /// </summary>
        ButtonFace = 2,
        
        /// <summary>
        /// The text displayed on a button.
        /// </summary>
        ButtonText = 3,
        
        /// <summary>
        /// The text displayed in a caption.
        /// </summary>
        CaptionText = 4,
        
        /// <summary>
        /// Greyed text.
        /// </summary>
        GrayText = 5,
        
        /// <summary>
        /// A highlighted user interface (UI) element.
        /// </summary>
        Highlight = 6,
        
        /// <summary>
        /// Highlighted text.
        /// </summary>
        HighlightText = 7,
        
        /// <summary>
        /// A hotlighted UI element.
        /// </summary>
        Hotlight = 8,
        
        /// <summary>
        /// An inactive caption element.
        /// </summary>
        InactiveCaption = 9,
        
        /// <summary>
        /// The text displayed in an inactive caption element.
        /// </summary>
        InactiveCaptionText = 10,
        
        /// <summary>
        /// A window.
        /// </summary>
        Window = 11,

        /// <summary>
        /// The text displayed in a window's UI decoration.
        /// </summary>
        WindowText = 12,
    }
}