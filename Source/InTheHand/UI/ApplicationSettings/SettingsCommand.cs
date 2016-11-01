//-----------------------------------------------------------------------
// <copyright file="SettingsCommand.cs" company="In The Hand Ltd">
//     Copyright © 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.UI.ApplicationSettings.SettingsCommand))]
//#else
using System;
using InTheHand.UI.Popups;

namespace InTheHand.UI.ApplicationSettings
{
    /// <summary>
    /// Creates a settings command object that represents a settings entry.
    /// This settings command can be appended to the ApplicationCommands collection.
    /// </summary>
    public sealed class SettingsCommand : IUICommand 
    {
        /// <summary>
        /// Creates a new settings command.
        /// </summary>
        /// <param name="settingsCommandId">The ID of the command.</param>
        /// <param name="label">The label for the command, which is displayed in the settings pane.</param>
        /// <param name="handler">The event handler that is called when the user selects this command in the settings pane.</param>
        public SettingsCommand(object settingsCommandId, string label, UICommandInvokedHandler handler)
        {
            Id = settingsCommandId;
            Invoked = handler;
            Label = label;
        }

        /// <summary>
        /// Gets or sets the command ID.
        /// </summary>
        public object Id { set; get; }

        /// <summary>
        /// Gets or sets the handler for the event that is raised when the user selects the command.
        /// </summary>
        public UICommandInvokedHandler Invoked { set; get; }

        /// <summary>
        /// Gets or sets the label for the command.
        /// </summary>
        public string Label { set; get; }
    }
}
//#endif