using System;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using Windows.UI;
using Windows.UI.ApplicationSettings;

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
#if WINDOWS_UWP
        private Windows.UI.Color? previousBackground;
        private Windows.UI.Color? previousForeground;
        private double previousBackgroundOpacity;
#endif

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
            Windows.UI.ViewManagement.StatusBar sb = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            if(sb != null)
            {
                previousBackground = sb.BackgroundColor;
                previousForeground = sb.ForegroundColor;
                previousBackgroundOpacity = sb.BackgroundOpacity;
                sb.BackgroundColor = InTheHand.ApplicationModel.Package.Current.BackgroundColor;
                sb.BackgroundOpacity = 1.0;
                sb.ForegroundColor = Colors.White;
            }

#elif WINDOWS_PHONE_APP
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            AppNameText.Text = InTheHand.ApplicationModel.Package.Current.DisplayName;
            Version.Text = string.Format("Version {0}", InTheHand.ApplicationModel.Package.Current.Id.Version);
#endif
            this.Unloaded += SettingsPage_Unloaded;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            UnregisterBack();
#if WINDOWS_UWP
            Windows.UI.ViewManagement.StatusBar sb = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            if (sb != null)
            {
                sb.BackgroundColor = previousBackground;
                sb.BackgroundOpacity = previousBackgroundOpacity;
                sb.ForegroundColor = previousForeground;
            }
#endif

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


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            commands = SettingsPane.GetForCurrentView().OnCommandsRequested();

            if (commands == null)
            {
                commands = new List<SettingsCommand>();
            }

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
            /*if (SettingsPane.GetForCurrentView().showAbout)
            {
                commands.Add(new SettingsCommand("About", "About", (c) =>
                {
                    Frame.Navigate(typeof(AboutPage));
                }));
            }*/

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
