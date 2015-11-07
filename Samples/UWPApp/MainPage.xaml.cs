﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            InTheHand.ApplicationModel.DataTransfer.DataTransferManager.GetForCurrentView().DataRequested += MainPage_DataRequested;
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.DisplayName);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.Id.Name);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.Id.Version);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.InstalledDate);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.IsDevelopmentMode);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.PublisherDisplayName);

            Windows.UI.ViewManagement.StatusBar.GetForCurrentView().ProgressIndicator.ShowAsync();
        }

        private void MainPage_DataRequested(object sender, InTheHand.ApplicationModel.DataTransfer.DataRequestedEventArgs e)
        {
            e.Request.Data.SetText("Hello Windows World");
            e.Request.Data.SetWebLink(new Uri("http://peterfoot.net"));
            e.Request.Data.Properties.Title = "Pete Stuff";
            e.Request.Data.Properties.Description = "A load of pete stuff";
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            InTheHand.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
        }

        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            //Windows.Media.Capture.CameraCaptureUI ccu = new Windows.Media.Capture.CameraCaptureUI();
            //StorageFile sf = await ccu.CaptureFileAsync(Windows.Media.Capture.CameraCaptureUIMode.Photo);
            InTheHand.UI.ApplicationSettings.SettingsPane.Show();
        }
    }
}
