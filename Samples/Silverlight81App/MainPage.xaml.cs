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
using Windows.ApplicationModel;

namespace Silverlight81App
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();


            //System.Diagnostics.Debug.WriteLine(Windows.);

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            //System.Diagnostics.Debug.WriteLine(Package.Current.DisplayName);
            System.Diagnostics.Debug.WriteLine(Package.Current.Id.Name);
            System.Diagnostics.Debug.WriteLine(Package.Current.Id.Version);
            //System.Diagnostics.Debug.WriteLine(Package.Current.InstalledDate);
            //System.Diagnostics.Debug.WriteLine(Package.Current.IsDevelopmentMode);
            Windows.Devices.Input.KeyboardCapabilities kc = new Windows.Devices.Input.KeyboardCapabilities();
            System.Diagnostics.Debug.WriteLine(kc.KeyboardPresent);


            System.Diagnostics.Debug.WriteLine(Windows.ApplicationModel.Package.Current);
            InTheHand.UI.ApplicationSettings.SettingsPane.GetForCurrentView().CommandsRequested += MainPage_CommandsRequested;
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_CommandsRequested(InTheHand.UI.ApplicationSettings.SettingsPane sender, InTheHand.UI.ApplicationSettings.SettingsPaneCommandsRequestedEventArgs args)
        {
            args.Request.ApplicationCommands.Add(new Windows.UI.ApplicationSettings.SettingsCommand("one", "One", (c) => { System.Diagnostics.Debug.WriteLine("s"); }));
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var p = await Windows.Storage.ApplicationData.Current.LocalFolder.GetBasicPropertiesAsync();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            InTheHand.UI.ApplicationSettings.SettingsPane.Show();
           /* MessageDialog md1 = new MessageDialog("hello1");
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
            md2.ShowAsync();*/

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