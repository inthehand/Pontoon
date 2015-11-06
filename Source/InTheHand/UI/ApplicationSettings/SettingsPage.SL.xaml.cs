using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using Microsoft.Phone.Controls;
using InTheHand.ApplicationModel;
using InTheHand.UI.Popups;
using InTheHand.UI.ApplicationSettings;

namespace InTheHandUI.ApplicationSettings
{
    internal partial class SettingsPage : PhoneApplicationPage
    {
        private IList<SettingsCommand> commands;

        public SettingsPage()
        {
            InitializeComponent();

            StreamResourceInfo sri = Application.GetResourceStream(Package.Current.Logo);

            BitmapImage img = new BitmapImage();
            img.SetSource(sri.Stream);

            WriteableBitmap wb = new WriteableBitmap(img);
            int rgb = wb.Pixels[0];

            byte r, g, b;
            r = (byte)((rgb & 0xFF0000) >> 16);
            g = (byte)((rgb & 0xFF00) >> 8);
            b = (byte)(rgb & 0xFF);

            Color chrome = (Color)Application.Current.Resources["PhoneBackgroundColor"];
            r = (byte)((int)(r + (3 * chrome.R)) / 4);
            g = (byte)((int)(g + (3 * chrome.G)) / 4);
            b = (byte)((int)(b + (3 * chrome.B)) / 4);
            Color c = Color.FromArgb(0xff, r, g, b);
            SolidColorBrush brush = new SolidColorBrush(c);
            LayoutRoot.Background = brush;
            Microsoft.Phone.Shell.SystemTray.BackgroundColor = c;

            this.Loaded += SettingsPage_Loaded;
        }

        void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            SettingsHeader.Text = InTheHandUI.Strings.Resources.Settings.ToLower();

            AppNameText.Text = InTheHand.ApplicationModel.Package.Current.DisplayName.ToUpper();

            if (SettingsPane.GetForCurrentView().showPublisher)
            {
                AuthorText.Text = string.Format(InTheHandUI.Strings.Resources.ByAuthor, InTheHand.ApplicationModel.Package.Current.PublisherDisplayName);
            }
            else
            {
                AuthorText.Visibility = Visibility.Collapsed;
            }

            this.Version.Text = string.Format(InTheHandUI.Strings.Resources.Version, InTheHand.ApplicationModel.Package.Current.Id.Version.ToString());
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            commands = InTheHand.UI.ApplicationSettings.SettingsPane.GetForCurrentView().OnCommandsRequested();

            if (commands == null)
            {
                commands = new List<SettingsCommand>();
            }

            //commands.Add(new SettingsCommand(null, InTheHand.UI.ApplicationSettings.Resources.Resources.Permissions, PermissionsSelected));

            // for store distribution include rate and review
#if DEBUG
            if(true)
#else
            if (!InTheHand.ApplicationModel.Package.Current.IsDevelopmentMode)
#endif
            {
                commands.Add(new SettingsCommand("RateAndReview", InTheHandUI.Strings.Resources.RateAndReview, RateAndReviewSelected));

                if (SettingsPane.GetForCurrentView().showPublisher)
                {
                    commands.Add(new SettingsCommand("PrivacyPolicy", InTheHandUI.Strings.Resources.PrivacyPolicy, async (c) =>
                        {
                            await InTheHand.ApplicationModel.Store.CurrentApp.RequestDetailsAsync();
                        }));
                }
            }

            SettingsList.ItemsSource = commands;

            base.OnNavigatedTo(e);
        }

        /*void PermissionsSelected(IUICommand command)
        {
            NavigationService.Navigate(new Uri("/InTheHand.UI.ApplicationSettings;component/PermissionsPage.SL.xaml", UriKind.Relative));
        }*/

        void RateAndReviewSelected(IUICommand command)
        {
            Microsoft.Phone.Tasks.MarketplaceReviewTask reviewTask = new Microsoft.Phone.Tasks.MarketplaceReviewTask();
            reviewTask.Show();
        }

        private void SettingsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SettingsList.SelectedItem != null)
            {
                ((SettingsCommand)SettingsList.SelectedItem).Invoked((SettingsCommand)SettingsList.SelectedItem);
            }
        }
    }
}