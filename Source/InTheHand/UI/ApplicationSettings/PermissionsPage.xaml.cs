using InTheHand.ApplicationModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace InTheHand.UI.ApplicationSettings
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    internal sealed partial class PermissionsPage : Page
    {
        public PermissionsPage()
        {
            this.InitializeComponent();

            /*Color bgColor = InTheHand.ApplicationModel.Package.Current.BackgroundColor;
            if (bgColor != Colors.Transparent)
            {
                Color modColor = Color.FromArgb((byte)0xff, (byte)(bgColor.R / 3), (byte)(bgColor.G / 3), (byte)(bgColor.B / 3));
                this.Background = new SolidColorBrush(modColor);
            }*/

            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            this.Loaded += PermissionsPage_Loaded;
            this.Unloaded += PermissionsPage_Unloaded;
        }
        void PermissionsPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            e.Handled = true;
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            this.Frame.GoBack();
        }

        void PermissionsPage_Loaded(object sender, RoutedEventArgs e)
        {
            ResourceLoader res = ResourceLoader.GetForCurrentView("InTheHand.UI.ApplicationSettings/Resources");

            this.AppNameText.Text = InTheHand.ApplicationModel.Package.Current.DisplayName;
            
            if (SettingsPane.GetForCurrentView().showPublisher)
            {
                this.AuthorName.Text = string.Format(res.GetString("ByAuthor"), InTheHand.ApplicationModel.Package.Current.PublisherDisplayName);
            }
            else
            {
                this.AuthorName.Visibility = Visibility.Collapsed;
            }

            PackageVersion ver = Windows.ApplicationModel.Package.Current.Id.Version;
            this.Version.Text = string.Format(res.GetString("Version"), ver.Major, ver.Minor, ver.Build, ver.Revision);
            //this.PermissionsSubHeading.Text = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView().GetString("PermissionsSubHeading");
            // show required toast notification toggle
            //NotificationSubHeading.Text = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView().GetString("Notifications");
            //NotificationButton.Header = "Allow toast notifications";// InTheHand.UI.ApplicationSettings.Resources.Resources.ToastNotifications;

            if(InTheHand.ApplicationModel.Package.Current.Capabilities.HasFlag(InTheHand.ApplicationModel.Capability.PushNotification))
            {
                NotificationPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

            foreach (InTheHand.ApplicationModel.Capability cap in Enum.GetValues(typeof(InTheHand.ApplicationModel.Capability)))
            {
                if (InTheHand.ApplicationModel.Package.Current.Capabilities.HasFlag(cap))
                {
                    string s = res.GetString("Capability" + cap.ToString());
                    if (!string.IsNullOrEmpty(s))
                    {
                        CapabilitiesList.Items.Add(s);
                    }
                    else
                    {
                        CapabilitiesList.Items.Add(cap.ToString());
                    }
                }
            }
        }

        private void NotificationSettings_Click(object sender, RoutedEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-notifications:"));
        }

    }
}
