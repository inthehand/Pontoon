//-----------------------------------------------------------------------
// <copyright file="SettingsPane.cs" company="In The Hand Ltd">
//     Copyright © 2013-15 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net;
#if __ANDROID__
using InTheHand.Storage;
#elif WINDOWS_PHONE
#if WINDOWS_PHONE_81
using Windows.Storage;
#else
using InTheHand.Storage;
#endif
using System.Windows;
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif

namespace InTheHand.UI.ApplicationSettings
{
    /// <summary>
    /// A static class that enables the app to control the Settings page.
    /// The app can add or remove commands, receive a notification when the user opens the pane, or open the page programmatically.
    /// </summary>
    public sealed class SettingsPane
    {
        private static SettingsPane instance;

        /// <summary>
        /// Gets a <see cref="SettingsPane"/> object that is associated with the current app.
        /// </summary>
        /// <returns></returns>
        public static SettingsPane GetForCurrentView()
        {
            if (instance == null)
            {
                instance = new SettingsPane();
            }

            return instance;
        }

        internal bool showPublisher = true;

        private SettingsPane()
        {
            
        }

        /// <summary>
        /// Displays the Settings page to the user.
        /// </summary>
        public static void Show()
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("InTheHand.UI.ApplicationSettings.ShowPublisher"))
            {
                if (!(bool)ApplicationData.Current.LocalSettings.Values["InTheHand.UI.ApplicationSettings.ShowPublisher"])
                {
                    GetForCurrentView().showPublisher = false;
                }
            }
#if WINDOWS_UWP
            if(Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent("Windows.UI.ApplicationSettings.ApplicationsSettingsContract",1))
            {
#pragma warning disable 618
                Windows.UI.ApplicationSettings.SettingsPane.Show();
            }
            else
            {
                Frame f = Window.Current.Content as Frame;
                if (f != null)
                {
                    f.Navigate(typeof(SettingsPage));
                }
            }
#elif WINDOWS_APP
            Windows.UI.ApplicationSettings.SettingsPane.Show();
#elif WINDOWS_PHONE_APP
            Frame f = Window.Current.Content as Frame;
            if (f != null)
            {
                f.Navigate(typeof(SettingsPage));
            }
#elif WINDOWS_PHONE
            ((Microsoft.Phone.Controls.PhoneApplicationFrame)Application.Current.RootVisual).Navigate(new Uri("/InTheHand;component/UI/ApplicationSettings/SettingsPage.SL.xaml", UriKind.Relative));
#else
            throw new PlatformNotSupportedException();
#endif
        }

        internal IList<SettingsCommand> OnCommandsRequested()
        {
            SettingsPaneCommandsRequestedEventArgs e = new SettingsPaneCommandsRequestedEventArgs();
            try
            {
                if (_commandsRequested != null)
                {
                    _commandsRequested(this, e);
                    return e.Request.ApplicationCommands;
                }
            }
            catch { }

            return null;
        }

        private event EventHandler<SettingsPaneCommandsRequestedEventArgs> _commandsRequested;
        /// <summary>
        /// Occurs when the user opens the settings pane.
        /// Listening for this event lets the app initialize the setting commands and pause its UI until the user closes the pane.
        /// During this event, append your SettingsCommand objects to the available ApplicationCommands vector to make them available to the SettingsPane UI.
        /// </summary>
        [CLSCompliant(false)]
        public event EventHandler<SettingsPaneCommandsRequestedEventArgs> CommandsRequested
        {
            add
            {
#if WINDOWS_UWP
                if (Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent("Windows.UI.ApplicationSettings.ApplicationsSettingsContract", 1))
                {
#endif
#if WINDOWS_UWP || WINDOWS_APP
                    if (_commandsRequested == null)
                    {
#pragma warning disable 618
                        Windows.UI.ApplicationSettings.SettingsPane.GetForCurrentView().CommandsRequested += SettingsPane_CommandsRequested;
                    }
#endif
#if WINDOWS_UWP
                }
#endif
                _commandsRequested += value;
            }
            remove
            {
                _commandsRequested -= value;
#if WINDOWS_UWP
                if (Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent("Windows.UI.ApplicationSettings.ApplicationsSettingsContract", 1))
                {
#endif
#if WINDOWS_UWP || WINDOWS_APP
                    if (_commandsRequested == null)
                    {
                        Windows.UI.ApplicationSettings.SettingsPane.GetForCurrentView().CommandsRequested -= SettingsPane_CommandsRequested;
                    }
#endif
#if WINDOWS_UWP
                }
#endif
            }
        }

#if WINDOWS_UWP || WINDOWS_APP


        private void SettingsPane_CommandsRequested(Windows.UI.ApplicationSettings.SettingsPane sender, Windows.UI.ApplicationSettings.SettingsPaneCommandsRequestedEventArgs args)
        {
            foreach(SettingsCommand cmd in OnCommandsRequested())
            {
#pragma warning disable 618
                args.Request.ApplicationCommands.Add(new Windows.UI.ApplicationSettings.SettingsCommand(cmd.Id, cmd.Label, new Windows.UI.Popups.UICommandInvokedHandler(async (c)=> { InTheHand.UI.ApplicationSettings.SettingsCommand sc = new SettingsCommand(c.Id, c.Label, cmd.Invoked ); sc.Invoked.Invoke(sc); })));
            }

#if WINDOWS_UWP
            // add missing 8.1 commands
#if DEBUG
            if(true)
#else
            if(!InTheHand.ApplicationModel.Package.Current.IsDevelopmentMode)
#endif
            {

#pragma warning disable 618
                args.Request.ApplicationCommands.Add(new Windows.UI.ApplicationSettings.SettingsCommand("RateAndReview", "Rate and review", async (c) =>
                {
                    await InTheHand.ApplicationModel.Store.CurrentApp.RequestReviewAsync();
                }));
#pragma warning disable 618
                args.Request.ApplicationCommands.Add(new Windows.UI.ApplicationSettings.SettingsCommand("PrivacyPolicy", "Privacy policy", async (c) =>
                {
                    await InTheHand.ApplicationModel.Store.CurrentApp.RequestDetailsAsync();
                }));
            }
#endif
        }
#endif
    }
}
