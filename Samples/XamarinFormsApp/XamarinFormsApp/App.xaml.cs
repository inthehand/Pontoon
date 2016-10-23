using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Xamarin.Forms;
using Windows.Storage;
using System.Threading.Tasks;

namespace XamarinFormsApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new XamarinFormsApp.MainPage();

            
        }
        
        protected async override void OnStart()
        {
            // Handle when your app starts
            Windows.Foundation.Point p = new Windows.Foundation.Point() { X = 10, Y = 20 };
            var t = typeof(Windows.Foundation.Metadata.ApiInformation);

            //var t = typeof(Windows.System.LauncherOptions);

            //Windows.System.Launcher.LaunchUriAsync(new Uri("http://bing.com"));
            //var folder = InTheHand.ApplicationModel.Package.Current.InstalledLocation;
            //StorageFolder folder = null;
            //IStorageFolder isf = null;

            //var folder = InTheHand.ApplicationModel.Package.Current.InstalledLocation;
            //var file = await folder.CreateFileAsync("test.txt", CreationCollisionOption.OpenIfExists);
            //var file = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("test.txt", CreationCollisionOption.OpenIfExists);
            
            //var s = Windows.ApplicationModel.Package.Current.PublisherDisplayName;
            //System.Diagnostics.Debug.WriteLine(Windows.Storage.ApplicationData.Current.ToString());

            //InTheHand.UI.Notifications.ToastNotificationCreator.CreateToastNotification("content", "title");
            /*var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("testfile.txt", CreationCollisionOption.OpenIfExists);
            using (var stream = await file.OpenStreamForWriteAsync())
            {
                using (var sw = new StreamWriter(stream))
                {
                    sw.WriteLine("Hello world");
                }
            }
            using (var stream = await file.OpenStreamForReadAsync())
            {
                using (var sr = new StreamReader(stream))
                {
                    System.Diagnostics.Debug.WriteLine(sr.ReadToEnd());
                }
            }*/
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
