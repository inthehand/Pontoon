using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Calls;
using Windows.ApplicationModel.DataTransfer;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
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

            DataTransferManager.GetForCurrentView().DataRequested += MainPage_DataRequested;
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.DisplayName);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.Id.Name);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.Id.Version);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.InstalledDate);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.IsDevelopmentMode);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.PublisherDisplayName);

            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagment.StatusBar"))
            {
                StatusBar.GetForCurrentView()?.ProgressIndicator.ShowAsync();
            }
           
            Windows.Devices.Enumeration.DeviceInformationPairing dip;
            Task.Run(async () => {
                await Task.Delay(1000);

                //var sel = Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService.GetDeviceSelectorFromUuid(Windows.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.DeviceInformation);

                var sel = Windows.Devices.Bluetooth.BluetoothLEDevice.GetDeviceSelector();
                 var devs =   await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(sel);
                foreach(DeviceInformation di in devs)
                {
                    //var s = await Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService.FromIdAsync(di.Id);

                    var d = await Windows.Devices.Bluetooth.BluetoothLEDevice.FromIdAsync(di.Id);
                    foreach(GattDeviceService serv in d.GattServices)
                    {
                        System.Diagnostics.Debug.WriteLine(serv);
                    }
                    System.Diagnostics.Debug.WriteLine(di.Name);
                }

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {

                    if (ApiInformation.IsTypePresent("Windows.UI.ViewManagment.StatusBar"))
                    {
                        StatusBar.GetForCurrentView()?.ProgressIndicator.HideAsync();
                    }
                });
            });
        }

        private void MainPage_DataRequested(object sender, DataRequestedEventArgs e)
        {
            e.Request.Data.SetText("Hello Windows World");
            e.Request.Data.SetWebLink(new Uri("http://peterfoot.net"));
            e.Request.Data.Properties.Title = "Pete Stuff";
            e.Request.Data.Properties.Description = "A load of pete stuff";
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var store = await PhoneCallManager.RequestStoreAsync();
           MessageDialog md = new MessageDialog("Message", "Title");
            md.Commands.Add(new UICommand("One", (c) => { System.Diagnostics.Debug.WriteLine("One"); }));
            md.Commands.Add(new UICommand("Two", (c) => { System.Diagnostics.Debug.WriteLine("Two"); }));

            IUICommand uc = await md.ShowAsync();
            uc.Invoked.Invoke(uc);

            DataTransferManager.ShowShareUI();
        }

        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            //Windows.Media.Capture.CameraCaptureUI ccu = new Windows.Media.Capture.CameraCaptureUI();
            //StorageFile sf = await ccu.CaptureFileAsync(Windows.Media.Capture.CameraCaptureUIMode.Photo);
            InTheHand.UI.ApplicationSettings.SettingsPane.Show();
        }
    }
}
