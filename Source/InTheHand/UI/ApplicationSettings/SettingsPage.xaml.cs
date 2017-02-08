using System;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using Windows.UI;
using Windows.ApplicationModel;
using InTheHand.ApplicationModel;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using InTheHand.UI.ApplicationSettings;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace InTheHandUI.ApplicationSettings
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    internal sealed partial class SettingsPage : Page
    {
        private IList<SettingsCommand> _commands;
        private bool _backRegistered = true;
#if WINDOWS_UWP
        private Windows.UI.Color? _previousBackground;
        private Windows.UI.Color? _previousForeground;
        private double _previousBackgroundOpacity;
        
#endif
        
        public SettingsPage()
        {
            InitializeComponent();


            /*Color bgColor = InTheHand.ApplicationModel.Package.Current.BackgroundColor;
            if (bgColor != Colors.Transparent)
            {
                Color modColor = Color.FromArgb((byte)0xff, (byte)(bgColor.R / 3), (byte)(bgColor.G / 3), (byte)(bgColor.B / 3));
                Background = new SolidColorBrush(modColor);
            }*/

#if WINDOWS_UWP

            Transitions.Add(new NavigationThemeTransition() { DefaultNavigationTransitionInfo = new CommonNavigationTransitionInfo() { IsStaggeringEnabled = true } });

            Grid LayoutRoot = new Grid();
            LayoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new Windows.UI.Xaml.GridLength(48) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new Windows.UI.Xaml.GridLength(1, GridUnitType.Star) });
            Rectangle r = new Rectangle() { Stroke = new SolidColorBrush(Color.FromArgb(0xff, 0x80, 0x80, 0x80)), StrokeThickness = 0.5, Margin = new Windows.UI.Xaml.Thickness(-4, 0, -4, 0), Fill= new SolidColorBrush(Color.FromArgb(0xff,0xf1,0xf1,0xf1)), HorizontalAlignment = HorizontalAlignment.Stretch };
            Grid.SetRow(r, 0);
            LayoutRoot.Children.Add(r);

            SettingsList = new ListView();
            Grid.SetRow(SettingsList, 1);
            LayoutRoot.Children.Add(SettingsList);
            Content = LayoutRoot;

            SystemNavigationManager.GetForCurrentView().BackRequested += SettingsPage_BackRequested;
            /*Windows.UI.ViewManagement.StatusBar sb = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            if(sb != null)
            {
                _previousBackground = sb.BackgroundColor;
                _previousForeground = sb.ForegroundColor;
                _previousBackgroundOpacity = sb.BackgroundOpacity;
                sb.BackgroundColor = Windows.ApplicationModel.Package.Current.BackgroundColor;
                sb.BackgroundOpacity = 1.0;
                sb.ForegroundColor = Colors.White;
            }*/

#elif WINDOWS_PHONE_APP

            AppNameText.Text = InTheHand.ApplicationModel.Package.Current.DisplayName;
            PublisherText.Text = InTheHand.ApplicationModel.Package.Current.PublisherDisplayName;
            VersionText.Text = "Version " + InTheHand.ApplicationModel.Package.Current.Id.Version.ToString(4);

            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
#endif
            Unloaded += SettingsPage_Unloaded;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            UnregisterBack();
#if WINDOWS_UWP
            Windows.UI.ViewManagement.StatusBar sb = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            if (sb != null)
            {
                sb.BackgroundColor = _previousBackground;
                sb.BackgroundOpacity = _previousBackgroundOpacity;
                sb.ForegroundColor = _previousForeground;
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
            if (_backRegistered)
            {
#if WINDOWS_UWP
                SystemNavigationManager.GetForCurrentView().BackRequested -= SettingsPage_BackRequested;
#elif WINDOWS_PHONE_APP
                Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
#endif
                _backRegistered = false;
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

            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _commands = SettingsPane.GetForCurrentView().OnCommandsRequested();

            if (_commands == null)
            {
                _commands = new List<SettingsCommand>();
            }

            // for store distribution include rate and review
#if !DEBUG
            if (!InTheHand.ApplicationModel.Package.Current.IsDevelopmentMode)
            {
#endif
                _commands.Add(new SettingsCommand("RateAndReview", "Rate and review", async (c) =>
                {
                    await InTheHand.ApplicationModel.Store.CurrentApp.RequestReviewAsync();
                }));

                _commands.Add(new SettingsCommand("PrivacyPolicy", "Privacy policy", async (c) =>
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

            SettingsList.ItemsSource = _commands;

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
