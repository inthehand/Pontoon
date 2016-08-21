using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using InTheHand.UI.Popups;
using System.Threading.Tasks;
using InTheHand.Storage;
using InTheHand.Devices.Enumeration;

namespace ApplicationModel.iOS
{
    [Register("UniversalView")]
    public class UniversalView : UIView
    {
        public UniversalView()
        {
            Initialize();
        }

        public UniversalView(RectangleF bounds) : base(bounds)
        {
            Initialize();
        }

        void Initialize()
        {
            BackgroundColor = UIColor.FromRGB(0x0, 0x66, 0x66);

            Task.Run(async () =>
            {
                await Task.Delay(3000);
                BeginInvokeOnMainThread(async () =>
                {
                    InTheHand.Storage.ApplicationData.Current.LocalSettings.Values.MapChanged += Values_MapChanged;
                    InTheHand.Storage.ApplicationData.Current.LocalSettings.Values.Add("MyNewTest", "cheese");
                    InTheHand.Storage.ApplicationData.Current.LocalSettings.Values["MyNewTest"] = "bread";
                    InTheHand.Storage.ApplicationData.Current.LocalSettings.Values.Remove("MyNewTest");

                    InTheHand.Media.Capture.CameraCaptureUI ccu = new InTheHand.Media.Capture.CameraCaptureUI();
                    StorageFile sf = await ccu.CaptureFileAsync(InTheHand.Media.Capture.CameraCaptureUIMode.Photo);
                    /*
                    string q = InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService.GetDeviceSelectorFromUuid(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.Battery);
                    var devs = await InTheHand.Devices.Enumeration.DeviceInformation.FindAllAsync(q);
                    MessageDialog md = new MessageDialog("test", "Title");
                    UICommand oneC = new UICommand("One", null);
                    UICommand twoC = new UICommand("Two", null);
                    md.Commands.Add(oneC);
                    md.Commands.Add(twoC);
                    IUICommand chosen = await md.ShowAsync();

                    bool success = chosen == oneC;*/
                });
            });
        }

        private void Values_MapChanged(InTheHand.Foundation.Collections.IObservableMap<string, object> sender, InTheHand.Foundation.Collections.IMapChangedEventArgs<string> eventArgs)
        {
            System.Diagnostics.Debug.WriteLine(eventArgs.CollectionChange.ToString() + " " + eventArgs.Key);
        }
    }

    [Register("UIViewController1")]
    public class UIViewController1 : UIViewController
    {
        public UIViewController1()
        {
            InTheHand.ApplicationModel.DataTransfer.DataTransferManager.GetForCurrentView().DataRequested += UIViewController1_DataRequested;
        }

        private void UIViewController1_DataRequested(object sender, InTheHand.ApplicationModel.DataTransfer.DataRequestedEventArgs e)
        {
            e.Request.Data.SetText("Hello World!");
            e.Request.Data.Properties.Title = "New Share";
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }
        public async override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            await Task.Delay(2000);
            foreach(DeviceInformation di in await InTheHand.Devices.Enumeration.DeviceInformation.FindAllAsync(""))
            {
                System.Diagnostics.Debug.WriteLine(di.Name);
            }


          
            /*InTheHand.UI.Popups.MessageDialog md = new InTheHand.UI.Popups.MessageDialog("Content", "Title");
            md.Commands.Add(new UICommand("One", (c) => { System.Diagnostics.Debug.WriteLine("One"); }));
            md.Commands.Add(new UICommand("Two", (c) => { System.Diagnostics.Debug.WriteLine("Two"); }));
            //md.Commands.Add(new UICommand("Three", (c) => { System.Diagnostics.Debug.WriteLine("Three"); }));
            await md.ShowAsync();*/

            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.DisplayName);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.Id.Name);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.Id.Version);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.InstalledDate);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.IsDevelopmentMode);
            //InTheHand.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();

            /*var store = await InTheHand.ApplicationModel.Calls.PhoneCallManager.RequestStoreAsync();
            if(store != null)
            {
                var g = await store.GetDefaultLineAsync();
                var l = await InTheHand.ApplicationModel.Calls.PhoneLine.FromIdAsync(g);
                l.Dial("07968449031", "Pete");
            }*/
        }

        public override void ViewDidLoad()
        {
            View = new UniversalView();

            base.ViewDidLoad();

            
            // Perform any additional setup after loading the view
        }
    }
}