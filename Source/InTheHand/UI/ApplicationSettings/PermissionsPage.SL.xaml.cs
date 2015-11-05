using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using InTheHand.ApplicationModel;

namespace InTheHand.UI.ApplicationSettings
{
    internal partial class PermissionsPage : PhoneApplicationPage
    {
        public PermissionsPage()
        {
            InitializeComponent();

            this.Loaded += PermissionsPage_Loaded;
        }

        void PermissionsPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.AppNameHeader.Text = InTheHand.ApplicationModel.Package.Current.DisplayName.ToUpper();
            this.PermissionsHeader.Text = InTheHand.UI.ApplicationSettings.Resources.Resources.Permissions.ToLower();

            this.ProductName.Text = InTheHand.ApplicationModel.Package.Current.DisplayName;

            if (SettingsPane.GetForCurrentView().showPublisher)
            {
                this.AuthorName.Text = string.Format(InTheHand.UI.ApplicationSettings.Resources.Resources.ByAuthor, InTheHand.ApplicationModel.Package.Current.PublisherDisplayName);
            }
            else
            {
                this.AuthorName.Visibility = System.Windows.Visibility.Collapsed;
            }

            this.Version.Text = string.Format(InTheHand.UI.ApplicationSettings.Resources.Resources.Version, InTheHand.ApplicationModel.Package.Current.Id.Version.ToString());
            this.PermissionsSubHeading.Text = InTheHand.UI.ApplicationSettings.Resources.Resources.PermissionsSubHeading;
            this.NotificationToggle.IsChecked = UserPermission.Instance.AllowToastNotifications;

            foreach(InTheHand.ApplicationModel.Capability cap in Enum.GetValues(typeof(InTheHand.ApplicationModel.Capability)))
            {
                if (InTheHand.ApplicationModel.Package.Current.Capabilities.HasFlag(cap))
                {
                    if(cap == Capability.PushNotification)
                    {
                        // show required toast notification toggle
                        NotificationSubHeading.Text = InTheHand.UI.ApplicationSettings.Resources.Resources.Notifications;
                        NotificationSubHeading.Visibility = System.Windows.Visibility.Visible;
                        NotificationToggle.Header = InTheHand.UI.ApplicationSettings.Resources.Resources.ToastNotifications;
                        NotificationToggle.Visibility = System.Windows.Visibility.Visible;
                        NotificationToggle.Checked += NotificationToggle_Checked;
                        NotificationToggle.Unchecked += NotificationToggle_Unchecked;
                    }
                    else
                    {
                        System.Reflection.PropertyInfo pi = typeof(InTheHand.UI.ApplicationSettings.Resources.Resources).GetProperty("Capability" + cap.ToString(), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
                        if (pi != null)
                        {
                            CapabilitiesList.Items.Add(pi.GetValue(null).ToString());
                        }
                    }
                }
            }
        }

        void NotificationToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            UserPermission.Instance.AllowToastNotifications = false;
        }

        void NotificationToggle_Checked(object sender, RoutedEventArgs e)
        {
            UserPermission.Instance.AllowToastNotifications = true;
        }
    }
}