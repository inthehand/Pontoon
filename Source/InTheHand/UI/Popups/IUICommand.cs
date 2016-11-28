// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUICommand.cs" company="In The Hand Ltd">
//   Copyright (c) 2012-16 In The Hand Ltd, All rights reserved.
// </copyright>
// <summary>
//   Represents a command in a context menu.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.UI.Popups
{
    /// <summary>
    /// Represents a command in a context menu or message dialog box.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list>
    /// </remarks>
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