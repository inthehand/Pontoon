using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI.Popups;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace WindowsPhone81App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.DisplayName);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.Id.Name);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.Id.Version);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.InstalledDate);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.IsDevelopmentMode);
            System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.PublisherDisplayName);

            System.Diagnostics.Debug.WriteLine(InTheHand.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamilyVersion);
            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.

            InTheHand.ApplicationModel.DataTransfer.Clipboard.Clear();

        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var store = await InTheHand.ApplicationModel.Calls.PhoneCallManager.RequestStoreAsync();
            Guid g = await store.GetDefaultLineAsync();
            var line = await InTheHand.ApplicationModel.Calls.PhoneLine.FromIdAsync(g);
            line.Dial("1234", "Testing");

            InTheHand.UI.Popups.MessageDialog md = new InTheHand.UI.Popups.MessageDialog("message", "title");
            md.Commands.Add(new InTheHand.UI.Popups.UICommand("One", (c) => { System.Diagnostics.Debug.WriteLine("One"); }));
            md.Commands.Add(new InTheHand.UI.Popups.UICommand("Two", (c) => { System.Diagnostics.Debug.WriteLine("Two"); }));
            
                await md.ShowAsync();
            InTheHand.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            //Windows.Storage.StorageFile sf = await CaptureFileAsync();

            //System.Diagnostics.Debug.WriteLine(sf);
            InTheHand.UI.ApplicationSettings.SettingsPane.Show();
        }

        public Task<Windows.Storage.StorageFile> CaptureFileAsync()
        {
            Type t = Type.GetType("Windows.Media.Capture.CameraCaptureUI, Windows, ContentType=WindowsRuntime");
           
            if (t != null)
            {
                Type tp = Type.GetType("Windows.Media.Capture.CameraCaptureUIMode, Windows, ContentType=WindowsRuntime");
                FieldInfo fi = tp.GetRuntimeField("Photo");
                object parameter = fi.GetValue(null);
                MethodInfo mi = t.GetRuntimeMethod("CaptureFileAsync", new Type[] { tp });
                object instance = Activator.CreateInstance(t);
                Windows.Foundation.IAsyncOperation<Windows.Storage.StorageFile> result = mi.Invoke(instance, new object[] { parameter }) as IAsyncOperation<Windows.Storage.StorageFile>;
                return result.AsTask();
            }

            return null;
        }
    }
}
