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

namespace Silverlight81App
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.DisplayName);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.Id.Name);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.Id.Version);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.InstalledDate);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.IsDevelopmentMode);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InTheHand.UI.Popups.MessageDialog md1 = new InTheHand.UI.Popups.MessageDialog("hello1");
            md1.Commands.Add(new InTheHand.UI.Popups.UICommand("one", async (c) =>
            {
                InTheHand.UI.Popups.MessageDialog md3 = new InTheHand.UI.Popups.MessageDialog("hello3");
                await md3.ShowAsync();
            }));
            md1.ShowAsync();

            InTheHand.UI.Popups.MessageDialog md2 = new InTheHand.UI.Popups.MessageDialog("hello2");
            md2.Commands.Add(new InTheHand.UI.Popups.UICommand("two", async (c) =>
            {
                InTheHand.UI.Popups.MessageDialog md4 = new InTheHand.UI.Popups.MessageDialog("hello4");
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