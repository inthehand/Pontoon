//-----------------------------------------------------------------------
// <copyright file="SettingsPaneCommandsRequest.cs" company="In The Hand Ltd">
//     Copyright © 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.UI.ApplicationSettings.SettingsPaneCommandsRequest))]
#else
using System.Collections.Generic;

namespace Windows.UI.ApplicationSettings
{
    /// <summary>
    /// Contains properties that are only available during the <see cref="SettingsPane.CommandsRequested"/> event.
    /// </summary>
    public sealed class SettingsPaneCommandsRequest
    {
        internal SettingsPaneCommandsRequest()
        {
            ApplicationCommands = new List<SettingsCommand>();
        }

        /// <summary>
        /// A vector that is available during the <see cref="SettingsPane.CommandsRequested"/> event.
        /// Append SettingsCommand objects to it to make them available to the SettingsPane UI.
        /// </summary>
        public IList<SettingsCommand> ApplicationCommands { get; internal set; }
    }
}
#endif