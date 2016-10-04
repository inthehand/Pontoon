//-----------------------------------------------------------------------
// <copyright file="SettingsPaneCommandsRequestedEventArgs.cs" company="In The Hand Ltd">
//     Copyright © 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.UI.ApplicationSettings
{
    /// <summary>
    /// Contains arguments that are available from the event object during the <see cref="SettingsPane.CommandsRequested"/> event.
    /// </summary>
    public sealed class SettingsPaneCommandsRequestedEventArgs
    {
        internal SettingsPaneCommandsRequestedEventArgs()
        {
            Request = new SettingsPaneCommandsRequest();
        }

        /// <summary>
        /// An instance of <see cref="SettingsPaneCommandsRequest"/> that is made available during the <see cref="SettingsPane.CommandsRequested"/> event.
        /// </summary>
        public SettingsPaneCommandsRequest Request { get; private set; }
    }
}