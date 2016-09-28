using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Provider;
using Windows.UI.Popups;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Notifications;
using InTheHand.UI.ApplicationSettings;

namespace DroidApp
{
    [Activity(Label = "DroidApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {            
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Windows.ApplicationModel.DataTransfer.DataTransferManager.GetForCurrentView().DataRequested += MainActivity_DataRequested;
            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            ApplicationData.Current.LocalSettings.Values.MapChanged += Values_MapChanged;
            ApplicationData.Current.LocalSettings.Values.Add("MyNewTest", "cheese");
            ApplicationData.Current.LocalSettings.Values["MyNewTest"] = "bread";
            ApplicationData.Current.LocalSettings.Values.Remove("MyNewTest");

            button.Click += new EventHandler((s,e)=> { button.Text = string.Format("{0} clicks!", count++);
                SettingsPane.Show();
                /*System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.DisplayName);
                System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.Id.FullName);
                System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.Id.Name);
                System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.Id.Version);
                System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.InstalledDate);
                System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.IsDevelopmentMode);
                System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.PublisherDisplayName);*/

                /*Windows.UI.Popups.MessageDialog md = new MessageDialog("message","title");
                md.Commands.Add(new UICommand("One", (c) => { System.Diagnostics.Debug.WriteLine("One"); }));
                md.Commands.Add(new UICommand("Two", (c) => { System.Diagnostics.Debug.WriteLine("Two"); }));
                await md.ShowAsync();*/

                /*Windows.Media.Capture.CameraCaptureUI ccu = new Windows.Media.Capture.CameraCaptureUI();
                StorageFile sf = await ccu.CaptureFileAsync(Windows.Media.Capture.CameraCaptureUIMode.Photo);
                System.Diagnostics.Debug.WriteLine(sf);*/

                // InTheHand.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
                //Task.Run(async () =>
                //{
                    /*MessageDialog md = new MessageDialog("Hello Android");
                    MessageDialog.Context = this;
                     md.ShowAsync();*/

                //System.Diagnostics.Debug.WriteLine(InTheHand.g)

                /*InTheHand.ApplicationModel.Email.EmailMessage msg = new InTheHand.ApplicationModel.Email.EmailMessage();
                msg.Subject = "Cheesy Subject";
                msg.Body = "Body text";
                msg.To.Add(new InTheHand.ApplicationModel.Email.EmailRecipient("peter@inthehand.com"));
                InTheHand.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(msg);*/
                    //System.Diagnostics.Debug.WriteLine("Hello Android Dismissed");
                //});
            });

            ToastNotifier n = ToastNotificationManager.CreateToastNotifierForApplication();
            var notification = InTheHand.UI.Notifications.ToastNotificationCreator.CreateToastNotification("content", "title");
            n.Show(notification);
        }

        private void Values_MapChanged(IObservableMap<string, object> sender, IMapChangedEventArgs<string> eventArgs)
        {
            System.Diagnostics.Debug.WriteLine(eventArgs.CollectionChange.ToString() + " " + eventArgs.Key);
        }

        private void MainActivity_DataRequested(object sender, Windows.ApplicationModel.DataTransfer.DataRequestedEventArgs e)
        {
            e.Request.Data.SetWebLink(new Uri("http://peterfoot.net"));
            e.Request.Data.Properties.Title = "Pete's Website";
        }
    }
}

