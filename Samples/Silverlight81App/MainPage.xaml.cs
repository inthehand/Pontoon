using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Silverlight81App.Resources;
using Windows.UI.Popups;

namespace Silverlight81App
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();


            System.Diagnostics.Debug.WriteLine(InTheHand.Environment.OSVersion.Version);

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.DisplayName);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.Id.Name);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.Id.Version);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.InstalledDate);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.IsDevelopmentMode);
            Windows.Devices.Input.KeyboardCapabilities kc = new Windows.Devices.Input.KeyboardCapabilities();
            System.Diagnostics.Debug.WriteLine(kc.KeyboardPresent);


            System.Diagnostics.Debug.WriteLine(Windows.ApplicationModel.Package.Current);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog md1 = new MessageDialog("hello1");
            md1.Commands.Add(new Windows.UI.Popups.UICommand("one", async (c) =>
            {
                MessageDialog md3 = new MessageDialog("hello3");
                await md3.ShowAsync();
            }));
            md1.ShowAsync();

            MessageDialog md2 = new MessageDialog("hello2");
            md2.Commands.Add(new UICommand("two", async (c) =>
            {
                MessageDialog md4 = new MessageDialog("hello4");
                await md4.ShowAsync();
            }));
            md2.ShowAsync();

        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}