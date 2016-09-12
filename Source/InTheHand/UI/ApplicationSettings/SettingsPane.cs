//-----------------------------------------------------------------------
// <copyright file="SettingsPane.cs" company="In The Hand Ltd">
//     Copyright © 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net;
using Windows.Storage;
#if WINDOWS_PHONE
using System.Windows;
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
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
#if WINDOWS_UWP
        private static bool hasSettingsPane;
#endif
        /// <summary>
        /// Gets a <see cref="SettingsPane"/> object that is associated with the current app.
        /// </summary>
        /// <returns></returns>
        public static SettingsPane GetForCurrentView()
        {
            if (instance == null)
            {
#if WINDOWS_UWP
                hasSettingsPane = Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent("Windows.UI.ApplicationSettings.ApplicationsSettingsContract", 1);
#endif
                instance = new SettingsPane();
            }

            return instance;
        }

        internal bool showPublisher = true;
        internal bool showAbout = false;

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
            object objAbout = null;
            if(ApplicationData.Current.LocalSettings.Values.TryGetValue("InTheHand.UI.ApplicationSettings.ShowAbout", out objAbout))
            {
                GetForCurrentView().showAbout = (bool)objAbout;
            }
#if WINDOWS_UWP
            if(hasSettingsPane)
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
            ((Microsoft.Phone.Controls.PhoneApplicationFrame)Application.Current.RootVisual).Navigate(new Uri("/InTheHand.UI;component/UI/ApplicationSettings/SettingsPage.SL.xaml", UriKind.Relative));
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
                if (hasSettingsPane)
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
                if (hasSettingsPane)
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
                args.Request.ApplicationCommands.Add(new Windows.UI.ApplicationSettings.SettingsCommand(cmd.Id, cmd.Label, new Windows.UI.Popups.UICommandInvokedHandler((c)=> { InTheHand.UI.ApplicationSettings.SettingsCommand sc = new SettingsCommand(c.Id, c.Label, cmd.Invoked ); sc.Invoked.Invoke(sc); })));
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
                args.Request.ApplicationCommands.Add(new Windows.UI.ApplicationSettings.SettingsCommand("rateAndReview", "Rate and review", async (c) =>
                {
                    await InTheHand.ApplicationModel.Store.CurrentApp.RequestReviewAsync();
                }));
#pragma warning disable 618
                args.Request.ApplicationCommands.Add(new Windows.UI.ApplicationSettings.SettingsCommand("privacyPolicy", "Privacy policy", async (c) =>
                {
                    await InTheHand.ApplicationModel.Store.CurrentApp.RequestDetailsAsync();
                }));
            }


#endif
            if (showAbout)
            {
                args.Request.ApplicationCommands.Add(new Windows.UI.ApplicationSettings.SettingsCommand("about", "About", (c) =>
                {
                    SettingsFlyout flyout = new SettingsFlyout();
                    flyout.Title = "About";
                    //flyout.IconSource = new BitmapImage(InTheHand.ApplicationModel.Package.Current.Logo);
                    flyout.Content = new InTheHandUI.AboutView();
                    flyout.Show();
                }));
            }
        }
#endif
    }
}
