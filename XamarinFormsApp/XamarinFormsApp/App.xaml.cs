using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Xamarin.Forms;
using Windows.Storage;

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
