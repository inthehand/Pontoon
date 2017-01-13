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

            var sel = InTheHand.Devices.Bluetooth.BluetoothLEDevice.GetDeviceSelector();
                //var sel = Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService.GetDeviceSelectorFromUuid(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.DeviceInformation);
                var devs = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(sel);
            foreach (DeviceInformation di in devs)
            {
                //var s = await Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService.FromIdAsync(di.Id);

                //var d = await InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService.FromIdAsync(di.Id);
                var d = await InTheHand.Devices.Bluetooth.BluetoothLEDevice.FromIdAsync(di.Id);
                foreach (InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService serv in d.GattServices)
                {
                    System.Diagnostics.Debug.WriteLine(serv.Uuid.ToString());
                    if (serv.Uuid == GattServiceUuids.DeviceInformation)
                    {
                        foreach(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic ch in serv.GetCharacteristics(GattCharacteristicUuids.ManufacturerNameString))
                        {
                            InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattReadResult rr = await ch.ReadValueAsync();
                            var vl = rr.Value;
                            System.Diagnostics.Debug.WriteLine(System.Text.Encoding.UTF8.GetString(vl));
                        }
                    }
                    foreach (InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic c in serv.GetAllCharacteristics())
                    {
                        System.Diagnostics.Debug.WriteLine(c.Uuid.ToString());

                        
                        /*foreach(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattDescriptor ds in c.GetAllDescriptors())
                        {
                            InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattReadResult r = await ds.ReadValueAsync();
                            object raw = r.Value;
                        }*/

                    }
                }
                System.Diagnostics.Debug.WriteLine(di.Name);
            }

                

                    if (ApiInformation.IsTypePresent("Windows.UI.ViewManagment.StatusBar"))
                    {
                        StatusBar.GetForCurrentView()?.ProgressIndicator.HideAsync();
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
