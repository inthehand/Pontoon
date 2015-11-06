using System;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using InTheHand.UI.ApplicationSettings;
using Windows.UI.Core;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace InTheHand.UI.ApplicationSettings
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    internal sealed partial class SettingsPage : Page
    {
        private IList<SettingsCommand> commands;
        private bool backRegistered = true;

        public SettingsPage()
        {
            this.InitializeComponent();

            /*Color bgColor = InTheHand.ApplicationModel.Package.Current.BackgroundColor;
            if (bgColor != Colors.Transparent)
            {
                Color modColor = Color.FromArgb((byte)0xff, (byte)(bgColor.R / 3), (byte)(bgColor.G / 3), (byte)(bgColor.B / 3));
                this.Background = new SolidColorBrush(modColor);
            }*/
#if WINDOWS_UWP
            SystemNavigationManager.GetForCurrentView().BackRequested += SettingsPage_BackRequested;
#elif WINDOWS_PHONE_APP
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
#endif
            this.Loaded += SettingsPage_Loaded;
            this.Unloaded += SettingsPage_Unloaded;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            UnregisterBack();
            base.OnNavigatingFrom(e);
        }

        void SettingsPage_Unloaded(object sender, RoutedEventArgs e)
        {
            UnregisterBack();
        }

        private void UnregisterBack()
        {
            if (backRegistered)
            {
#if WINDOWS_UWP
                SystemNavigationManager.GetForCurrentView().BackRequested -= SettingsPage_BackRequested;
#elif WINDOWS_PHONE_APP
                Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
#endif
                backRegistered = false;
            }
        }

#if WINDOWS_UWP
        private void SettingsPage_BackRequested(object sender, BackRequestedEventArgs e)
#elif WINDOWS_PHONE_APP
        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
#endif
        {
            e.Handled = true;

            UnregisterBack();

            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            AppNameText.Text = InTheHand.ApplicationModel.Package.Current.DisplayName;
            Version.Text = "Version " + InTheHand.ApplicationModel.Package.Current.Id.Version.ToString();
            if (SettingsPane.GetForCurrentView().showPublisher)
            {
                AuthorText.Text = string.Format("By {0}", InTheHand.ApplicationModel.Package.Current.PublisherDisplayName);
            }
            else
            {
                AuthorText.Visibility = Visibility.Collapsed;
            }
        }




        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            commands = InTheHand.UI.ApplicationSettings.SettingsPane.GetForCurrentView().OnCommandsRequested();

            if (commands == null)
            {
                commands = new List<SettingsCommand>();
            }

            //commands.Add(new SettingsCommand("Permissions", "Permissions", PermissionsSelected));

            // for store distribution include rate and review
#if !DEBUG
            if (!InTheHand.ApplicationModel.Package.Current.IsDevelopmentMode)
            {
#endif
                commands.Add(new SettingsCommand("RateAndReview", "Rate and review", async (c) =>
                {
                    await InTheHand.ApplicationModel.Store.CurrentApp.RequestReviewAsync();
                }));

                commands.Add(new SettingsCommand("PrivacyPolicy", "Privacy policy", async (c) =>
                    {
                        await InTheHand.ApplicationModel.Store.CurrentApp.RequestDetailsAsync();
                    }));
#if !DEBUG
            }
#endif

            SettingsList.ItemsSource = commands;

            base.OnNavigatedTo(e);
        }

        private void SettingsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SettingsList.SelectedItem != null)
            {
                ((SettingsCommand)SettingsList.SelectedItem).Invoked((SettingsCommand)SettingsList.SelectedItem);
                SettingsList.SelectedIndex = -1;
            }
        }
    }
}
