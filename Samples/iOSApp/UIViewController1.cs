using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using InTheHand.UI.Popups;
using System.Threading.Tasks;

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

            foreach(NSObject o in NSBundle.MainBundle.InfoDictionary.Keys)
            {
                System.Diagnostics.Debug.WriteLine(o.ToString());
                System.Diagnostics.Debug.WriteLine(NSBundle.MainBundle.InfoDictionary[o].ToString());
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
            InTheHand.Media.Capture.CameraCaptureUI ccu = new InTheHand.Media.Capture.CameraCaptureUI();
            ccu.CaptureFileAsync(InTheHand.Media.Capture.CameraCaptureUIMode.Photo);

        }

        public override void ViewDidLoad()
        {
            View = new UniversalView();

            base.ViewDidLoad();

            
            // Perform any additional setup after loading the view
        }
    }
}