using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
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
        InTheHand.Devices.Sensors.Pedometer p;

        public MainPage()
        {
            this.InitializeComponent();

            DataTransferManager.GetForCurrentView().DataRequested += MainPage_DataRequested;
            System.Diagnostics.Debug.WriteLine(Package.Current.DisplayName);
            System.Diagnostics.Debug.WriteLine(Package.Current.Id.Name);
            System.Diagnostics.Debug.WriteLine(Package.Current.Id.Version);
            System.Diagnostics.Debug.WriteLine(Package.Current.InstalledDate);
            System.Diagnostics.Debug.WriteLine(Package.Current.IsDevelopmentMode);
            System.Diagnostics.Debug.WriteLine(Package.Current.PublisherDisplayName);

            System.Diagnostics.Debug.WriteLine(Windows.ApplicationModel.Package.Current);
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagment.StatusBar"))
            {
                StatusBar.GetForCurrentView()?.ProgressIndicator.ShowAsync();
            }

            this.Loaded += MainPage_Loaded;
           
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

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            p = await InTheHand.Devices.Sensors.Pedometer.GetDefaultAsync();
            if (p != null)
            {
                var r = p.GetCurrentReadings();
                foreach (KeyValuePair<InTheHand.Devices.Sensors.PedometerStepKind, InTheHand.Devices.Sensors.PedometerReading> readingentry in r)
                {
                    System.Diagnostics.Debug.WriteLine(readingentry.Value.Timestamp.ToString() + " " + readingentry.Value.CumulativeSteps.ToString() + " " + readingentry.Value.StepKind);
                }
            }
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
           MessageDialog md = new MessageDialog("Message", "Title");
            md.Commands.Add(new UICommand("One", (c) => { System.Diagnostics.Debug.WriteLine("One"); }));
            md.Commands.Add(new UICommand("Two", (c) => { System.Diagnostics.Debug.WriteLine("Two"); }));

            IUICommand uc = await md.ShowAsync();
            //uc.Invoked.Invoke(uc);

            DataTransferManager.ShowShareUI();
        }

        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            //Windows.Media.Capture.CameraCaptureUI ccu = new Windows.Media.Capture.CameraCaptureUI();
            //StorageFile sf = await ccu.CaptureFileAsync(Windows.Media.Capture.CameraCaptureUIMode.Photo);
           // InTheHand.UI.ApplicationSettings.SettingsPane.Show();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            InTheHand.Storage.Pickers.FileOpenPicker fop = new InTheHand.Storage.Pickers.FileOpenPicker();
            fop.FileTypeFilter.Add(".txt");
            fop.FileTypeFilter.Add(".cs");
            var file = await fop.PickSingleFileAsync();
        }
    }
}
