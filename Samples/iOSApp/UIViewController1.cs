using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;

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
            BackgroundColor = UIColor.Red;
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

            InTheHand.UI.Popups.MessageDialog md = new InTheHand.UI.Popups.MessageDialog("content", "title");
            await md.ShowAsync();

            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.DisplayName);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.Id.Name);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.Id.Version);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.InstalledDate);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.IsDevelopmentMode);
            //InTheHand.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();

        }

        public override void ViewDidLoad()
        {
            View = new UniversalView();

            base.ViewDidLoad();

            
            // Perform any additional setup after loading the view
        }
    }
}