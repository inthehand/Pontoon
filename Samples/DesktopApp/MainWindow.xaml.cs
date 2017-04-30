using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InTheHand.ApplicationModel.DataTransfer;
using InTheHand.UI.Popups;
using System.Diagnostics;
using InTheHand.Storage;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace DesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            /*InTheHand.ApplicationModel.Email.EmailMessage msg = new InTheHand.ApplicationModel.Email.EmailMessage();
            msg.Subject = "Hello world";
            msg.Body = "Some text\r\nsome more";
            msg.To.Add(new InTheHand.ApplicationModel.Email.EmailRecipient("peter@inthehand.com", "Pete"));
            StorageFile f = await StorageFile.GetFileFromPathAsync("C:\\Users\\peter\\Documents\\cheese.txt");
            msg.Attachments.Add(new InTheHand.ApplicationModel.Email.EmailAttachment("cheese.txt", f));
            await InTheHand.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(msg);*/

            /*var radios = await InTheHand.Devices.Radios.Radio.GetRadiosAsync();
            foreach(Radio r in radios)
            {
                Debug.WriteLine(r.State);
                await r.SetStateAsync(RadioState.Off);
                Debug.WriteLine(r.State);
                await r.SetStateAsync(RadioState.On);
                Debug.WriteLine(r.State);
            }*/

            System.Diagnostics.Debug.WriteLine(new InTheHand.Devices.Input.KeyboardCapabilities().KeyboardPresent);
            /*System.Diagnostics.Debug.WriteLine(InTheHand.Environment.OSVersion.VersionString);

            MessageDialog md = new MessageDialog("Content goes here", "Title goes here");
            md.Commands.Add(new UICommand("One", (c) => { System.Diagnostics.Debug.WriteLine("One"); }, "one"));
            md.Commands.Add(new UICommand("Two", (c) => { System.Diagnostics.Debug.WriteLine("Two"); }, "two"));
            md.Commands.Add(new UICommand("Three", (c) => { System.Diagnostics.Debug.WriteLine("Three"); }, "three"));
            //md.DefaultCommandIndex = 2;
            //md.CancelCommandIndex = 0;
            var cmd = await md.ShowAsync();*/

            //MessageDialog md = new MessageDialog(InTheHand.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamilyVersion);
            //await md.ShowAsync();
            var di = InTheHand.Graphics.Display.DisplayInformation.GetForCurrentView();
            BrightnessHelper.SetBrightness(100);

            //await InTheHand.UI.ViewManagement.StatusBar.GetForCurrentView().ProgressIndicator.ShowAsync();
            //InTheHand.UI.ViewManagement.StatusBar.GetForCurrentView().ProgressIndicator.ProgressValue = 50;
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            //var devices = await InTheHand.Devices.Enumeration.DeviceInformation.FindAllAsync();

            InTheHand.Devices.Enumeration.DevicePicker p = new InTheHand.Devices.Enumeration.DevicePicker();
            p.Filter.SupportedDeviceSelectors.Add(InTheHand.Devices.Bluetooth.BluetoothDevice.GetDeviceSelectorFromPairingState(false));
            //p.Filter.SupportedDeviceSelectors.Add(InTheHand.Devices.Bluetooth.BluetoothDevice.GetDeviceSelectorFromClassOfDevice(InTheHand.Devices.Bluetooth.BluetoothClassOfDevice.FromRawValue(0x104)));
            var d = await p.PickSingleDeviceAsync(new InTheHand.Foundation.Rect());

            if (d != null)
            {
                InTheHand.Devices.Bluetooth.BluetoothDevice bd = await InTheHand.Devices.Bluetooth.BluetoothDevice.FromDeviceInformationAsync(d);
                Debug.WriteLine(d.Pairing.IsPaired);
                if(!d.Pairing.IsPaired)
                {
                    var pairresult = await d.Pairing.PairAsync();
                    /*d.Pairing.Custom.PairingRequested += Custom_PairingRequested;
                    await d.Pairing.Custom.PairAsync(InTheHand.Devices.Enumeration.DevicePairingKinds.ProvidePin);*/
                }
                Debug.WriteLine(bd.ClassOfDevice.MinorClass);
                Debug.WriteLine(bd.ClassOfDevice);
            }
            /*InTheHand.Storage.Pickers.FileOpenPicker fop = new InTheHand.Storage.Pickers.FileOpenPicker();
            fop.FileTypeFilter.Add(".txt");
            fop.FileTypeFilter.Add(".cs");
            var file = await fop.PickSingleFileAsync();*/

            /* InTheHand.Storage.Pickers.FileSavePicker fsp = new InTheHand.Storage.Pickers.FileSavePicker();
             fsp.FileTypeChoices.Add("Text file", new List<string> { ".txt", ".csv" });
             fsp.DefaultFileExtension = ".txt";
             var savefile = await fsp.PickSaveFileAsync();*/

            InTheHand.Storage.Pickers.FolderPicker fp = new InTheHand.Storage.Pickers.FolderPicker();


            var folder = await fp.PickSingleFolderAsync();

        }

        private void Custom_PairingRequested(object sender, InTheHand.Devices.Enumeration.DevicePairingRequestedEventArgs e)
        {
            Debug.WriteLine(e.Pin);
        }

        public static class BrightnessHelper
        {
            public static void SetBrightness(uint brightness)
            {
                if (brightness > 100)
                {
                    throw new ArgumentOutOfRangeException("brightness");
                }

                // get handle to primary display
                IntPtr hmon = NativeMethods.MonitorFromWindow(IntPtr.Zero, NativeMethods.MONITOR_DEFAULTTO.PRIMARY);

                // get number of physical displays (assume only one for simplicity)
                int num;
                bool success = NativeMethods.GetNumberOfPhysicalMonitorsFromHMONITOR(hmon, out num);
                NativeMethods.PHYSICAL_MONITOR pmon = new NativeMethods.PHYSICAL_MONITOR();

                success = NativeMethods.GetPhysicalMonitorsFromHMONITOR(hmon, num, ref pmon);

                for (int i = 0; i < num; i++)
                {
                    uint min, max, current;
                    // commonly min and max are 0-100 which represents a percentage brightness
                    success = NativeMethods.GetMonitorBrightness(pmon.hPhysicalMonitor, out min, out current, out max);

                    // set to full brightness
                    success = NativeMethods.SetMonitorBrightness(pmon.hPhysicalMonitor, brightness);
                }

                success = NativeMethods.DestroyPhysicalMonitors(num, ref pmon);
            }

            private static class NativeMethods
            {
                [DllImport("Dxva2")]
                [return: MarshalAs(UnmanagedType.Bool)]
                internal static extern bool GetMonitorBrightness(IntPtr hMonitor,
                    out uint pdwMinimumBrightness,
                    out uint pdwCurrentBrightness,
                    out uint pdwMaximumBrightness);

                [DllImport("Dxva2")]
                [return: MarshalAs(UnmanagedType.Bool)]
                internal static extern bool SetMonitorBrightness(IntPtr hMonitor, uint newBrightness);

                [DllImport("Dxva2")]
                [return: MarshalAs(UnmanagedType.Bool)]
                internal static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, out int numberOfPhysicalMonitors);

                [DllImport("Dxva2", CharSet = CharSet.Unicode)]
                [return: MarshalAs(UnmanagedType.Bool)]
                internal static extern bool GetPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, int physicalMonitorArraySize, ref PHYSICAL_MONITOR physicalMonitorArray);

                [DllImport("Dxva2", CharSet = CharSet.Unicode)]
                [return: MarshalAs(UnmanagedType.Bool)]
                internal static extern bool DestroyPhysicalMonitors(int physicalMonitorArraySize, ref PHYSICAL_MONITOR physicalMonitorArray);

                [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
                internal struct PHYSICAL_MONITOR
                {
                    internal IntPtr hPhysicalMonitor;
                    //[PHYSICAL_MONITOR_DESCRIPTION_SIZE]
                    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
                    internal string szPhysicalMonitorDescription;
                }

                [DllImport("User32")]
                internal static extern IntPtr MonitorFromWindow(IntPtr hwnd, MONITOR_DEFAULTTO dwFlags);

                internal enum MONITOR_DEFAULTTO
                {
                    NULL = 0x00000000,
                    PRIMARY = 0x00000001,
                    NEAREST = 0x00000002,
                }
            }
        }
    }
}
