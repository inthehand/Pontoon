using System;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using InTheHand.UI.ApplicationSettings;
using Windows.UI.Core;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace InTheHand.UI.ApplicationSettings
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    internal sealed partial class AboutPage : Page
    {
        private bool backRegistered = true;
#if WINDOWS_UWP || WINDOWS_PHONE_APP
        private Windows.UI.Color? previousBackground;
        private Windows.UI.Color? previousForeground;
        private double previousBackgroundOpacity;
#endif

        public AboutPage()
        {
            this.InitializeComponent();

            /*Color bgColor = InTheHand.ApplicationModel.Package.Current.BackgroundColor;
            if (bgColor != Colors.Transparent)
            {
                Color modColor = Color.FromArgb((byte)0xff, (byte)(bgColor.R / 3), (byte)(bgColor.G / 3), (byte)(bgColor.B / 3));
                this.Background = new SolidColorBrush(modColor);
            }*/
#if WINDOWS_UWP || WINDOWS_PHONE_APP
            InTheHand.UI.ViewManagement.StatusBar sb = InTheHand.UI.ViewManagement.StatusBar.GetForCurrentView();
            if (sb != null)
            {
                previousBackground = sb.BackgroundColor;
                previousForeground = sb.ForegroundColor;
                previousBackgroundOpacity = sb.BackgroundOpacity;
                sb.BackgroundColor = InTheHand.ApplicationModel.Package.Current.BackgroundColor;
                sb.BackgroundOpacity = 1.0;
                sb.ForegroundColor = Colors.White;
            }
#endif

#if WINDOWS_UWP
            SystemNavigationManager.GetForCurrentView().BackRequested += AboutPage_BackRequested;    
#elif WINDOWS_PHONE_APP
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
#endif
            this.Loaded += AboutPage_Loaded;
            this.Unloaded += AboutPage_Unloaded;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            UnregisterBack();
#if WINDOWS_UWP || WINDOWS_PHONE_APP
            InTheHand.UI.ViewManagement.StatusBar sb = InTheHand.UI.ViewManagement.StatusBar.GetForCurrentView();
            if (sb != null)
            {
                sb.BackgroundColor = previousBackground;
                sb.BackgroundOpacity = previousBackgroundOpacity;
                sb.ForegroundColor = previousForeground;
            }
#endif

            base.OnNavigatingFrom(e);
        }

        void AboutPage_Unloaded(object sender, RoutedEventArgs e)
        {
            UnregisterBack();
        }

        private void UnregisterBack()
        {
            if (backRegistered)
            {
#if WINDOWS_UWP
                SystemNavigationManager.GetForCurrentView().BackRequested -= AboutPage_BackRequested;
#elif WINDOWS_PHONE_APP
                Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
#endif
                backRegistered = false;
            }
        }

#if WINDOWS_UWP
        private void AboutPage_BackRequested(object sender, BackRequestedEventArgs e)
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

        void AboutPage_Loaded(object sender, RoutedEventArgs e)
        {
            AppIcon.Source = new BitmapImage(InTheHand.ApplicationModel.Package.Current.Logo);
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

            Description.Text = InTheHand.ApplicationModel.Package.Current.Description;
        }


        
    }
}
