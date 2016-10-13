using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using InTheHand.UI.Popups;
using System.Threading.Tasks;
using InTheHand.Devices.Enumeration;
using InTheHand.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using InTheHand.ApplicationModel;
using Windows.ApplicationModel;

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
                    var l = Windows.Globalization.ApplicationLanguages.Languages;
                    var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("test.txt", CreationCollisionOption.OpenIfExists);
                    await FileIO.WriteTextAsync(file, "Here is a small amount of text.");
                    var bp = await file.GetBasicPropertiesAsync();
                    System.Diagnostics.Debug.WriteLine(bp.Size);
                    ApplicationData.Current.LocalSettings.Values.MapChanged += Values_MapChanged;
                    ApplicationData.Current.LocalSettings.Values.Add("MyNewTest", "cheese");
                    ApplicationData.Current.LocalSettings.Values["MyNewTest"] = "bread";
                    ApplicationData.Current.LocalSettings.Values.Remove("MyNewTest");
                    
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

        private void Values_MapChanged(IObservableMap<string, object> sender, IMapChangedEventArgs<string> eventArgs)
        {
            System.Diagnostics.Debug.WriteLine(eventArgs.CollectionChange.ToString() + " " + eventArgs.Key);
        }
    }

    [Register("UIViewController1")]
    public class UIViewController1 : UIViewController
    {
        public UIViewController1()
        {
            Windows.ApplicationModel.DataTransfer.DataTransferManager.GetForCurrentView().DataRequested += UIViewController1_DataRequested;
        }

        private void UIViewController1_DataRequested(object sender, Windows.ApplicationModel.DataTransfer.DataRequestedEventArgs e)
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

        private async void B_TouchUpInside(object sender, EventArgs e)
        {
            Windows.Media.Capture.CameraCaptureUI ccu = new Windows.Media.Capture.CameraCaptureUI();
            StorageFile sf = await ccu.CaptureFileAsync(Windows.Media.Capture.CameraCaptureUIMode.Photo);
            var p = await sf.GetBasicPropertiesAsync();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            //var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("test.txt", CreationCollisionOption.OpenIfExists);
            //System.Diagnostics.Debug.WriteLine(file.ContentType);

            /* Windows.UI.Popups.PopupMenu pm = new Windows.UI.Popups.PopupMenu();
             pm.Commands.Add(new UICommand("First", (c) => { }));
             pm.Commands.Add(new UICommand("Second", (c) => { }));
             pm.Commands.Add(new UICommand("Third", (c) => { }));
             pm.Commands.Add(new UICommand("Fourth", (c) => { }));
             pm.Commands.Add(new UICommand("Fifth", (c) => { }));
             pm.Commands.Add(new UICommand("Sixth", (c) => { }));
             await pm.ShowAsync(new Windows.Foundation.Point() { X = 20, Y = 20 });*/



          
            /*InTheHand.UI.Popups.MessageDialog md = new InTheHand.UI.Popups.MessageDialog("Content", "Title");
            md.Commands.Add(new UICommand("One", (c) => { System.Diagnostics.Debug.WriteLine("One"); }));
            md.Commands.Add(new UICommand("Two", (c) => { System.Diagnostics.Debug.WriteLine("Two"); }));
            //md.Commands.Add(new UICommand("Three", (c) => { System.Diagnostics.Debug.WriteLine("Three"); }));
            await md.ShowAsync();*/

            System.Diagnostics.Debug.WriteLine(Package.Current.DisplayName);
            System.Diagnostics.Debug.WriteLine(Package.Current.Id.Name);
            System.Diagnostics.Debug.WriteLine(Package.Current.Id.Version.ToString(4));
            System.Diagnostics.Debug.WriteLine(Package.Current.InstalledDate);
            System.Diagnostics.Debug.WriteLine(Package.Current.IsDevelopmentMode);
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
            var b = new UIButton(UIButtonType.System);
            b.Center = new CoreGraphics.CGPoint(200, 200);
            b.Bounds = new CoreGraphics.CGRect(0, 0, 400, 200);
            b.SetTitle("Camera", UIControlState.Normal);
            b.TouchUpInside += B_TouchUpInside;
            View.Add(b);

            base.ViewDidLoad();

            
            // Perform any additional setup after loading the view
        }
    }
}