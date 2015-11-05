// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUICommand.cs" company="In The Hand Ltd">
//   Copyright (c) 2012-15 In The Hand Ltd, All rights reserved.
// </copyright>
// <summary>
//   Represents a command in a context menu.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.UI.Popups
{
    /// <summary>
    /// ts a command in a context menu or message dialog box.
    /// </summary>
    public interface IUICommand
    {
        /// <summary>
        /// Gets or sets the identifier of the command.
        /// </summary>
        object Id { set; get; }

        /// <summary>
        /// Ges or sets the handler for the event that is fired when the user invokes the command.
        /// </summary>
        UICommandInvokedHandler Invoked { set; get; }

        /// <summary>
        /// Gets or sets the label for the command.
        /// </summary>
        string Label { set; get; }
    }

    /// <summary>
    /// Represents a callback function that handles the event that is fired when the user invokes a context menu command.
    /// </summary>
    /// <param name="command">Represents the invoked command.</param>
    public delegate void UICommandInvokedHandler(IUICommand command);
}
