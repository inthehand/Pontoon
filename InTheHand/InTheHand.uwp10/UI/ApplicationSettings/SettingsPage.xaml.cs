using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace InTheHand.UI.ApplicationSettings
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private IList<SettingsCommand> commands;
        private bool backRegistered = true;

        public SettingsPage()
        {
            this.InitializeComponent();

            SystemNavigationManager.GetForCurrentView().BackRequested += SettingsPage_BackRequested;
            this.Loaded += SettingsPage_Loaded;
            this.Unloaded += SettingsPage_Unloaded;
        }

        private void SettingsPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;

            if (backRegistered)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= SettingsPage_BackRequested;
                backRegistered = false;
            }

            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (backRegistered)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= SettingsPage_BackRequested;
                backRegistered = false;
            }
            base.OnNavigatingFrom(e);
        }

        void SettingsPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (backRegistered)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= SettingsPage_BackRequested;
                backRegistered = false;
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

            commands.Add(new SettingsCommand("Permissions", "Permissions", PermissionsSelected));

            // for store distribution include rate and review
#if !DEBUG
            if (!InTheHand.ApplicationModel.Package.Current.IsDevelopmentMode)
            {
#endif
            commands.Add(new SettingsCommand("RateAndReview", "Rate and review", RateAndReviewSelected));

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

        void PermissionsSelected(IUICommand command)
        {
            //Frame.Navigate(typeof(PermissionsPage));
        }

        async void RateAndReviewSelected(IUICommand command)
        {
            await InTheHand.ApplicationModel.Store.CurrentApp.RequestReviewAsync();
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
