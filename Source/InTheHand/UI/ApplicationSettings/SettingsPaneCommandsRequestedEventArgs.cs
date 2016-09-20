//-----------------------------------------------------------------------
// <copyright file="SettingsPaneCommandsRequestedEventArgs.cs" company="In The Hand Ltd">
//     Copyright © 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.UI.ApplicationSettings.SettingsPaneCommandsRequestedEventArgs))]
#else
namespace Windows.UI.ApplicationSettings
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
#endif