extern alias InTheHand;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Silverlight80App.Resources;

namespace Silverlight80App
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            InTheHand::Windows.ApplicationModel.DataTransfer.DataTransferManager.GetForCurrentView().DataRequested += MainPage_DataRequested;
            InTheHand::Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
            System.Diagnostics.Debug.WriteLine(Windows.ApplicationModel.Package.Current);
        }

        private void MainPage_DataRequested(InTheHand::Windows.ApplicationModel.DataTransfer.DataTransferManager sender, InTheHand::Windows.ApplicationModel.DataTransfer.DataRequestedEventArgs args)
        {
            args.Request.Data.SetText("Hello world");
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var p = await Windows.Storage.ApplicationData.Current.LocalFolder.GetBasicPropertiesAsync();
            
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