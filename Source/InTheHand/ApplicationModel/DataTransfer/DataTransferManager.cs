//-----------------------------------------------------------------------
// <copyright file="DataTransferManager.cs" company="In The Hand Ltd">
//     Copyright © 2013-15 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


using global::System;
using global::System.Collections.Generic;
using global::System.Diagnostics;

#if __ANDROID__
using Android.Content;
using Android.App;
#elif __IOS__
using Foundation;
using UIKit;
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81 || WINDOWS_UWP
using Windows.Foundation;
using Windows.ApplicationModel.DataTransfer;
#endif
#if WINDOWS_PHONE
using System.Windows;
#endif
namespace InTheHand.ApplicationModel.DataTransfer
{
    /// <summary>
    /// Programmatically initiates an exchange of content with other apps.
    /// </summary>
    /// <remarks>The <see cref="DataTransferManager"/> class is a static class that you use to initiate sharing operations.
    /// To use the class, first call the <see cref="GetForCurrentView"/> method.
    /// This method returns the <see cref="DataTransferManager"/> object that is specific to the active window.
    /// Next, you need to add an event listener for the datarequested event to the object.
    /// This event is fired when your app starts a share operation programmatically.
    /// <para>The DataTransferManager class includes a <see cref="ShowShareUI"/> method, which you can use to programmatically start a share operation.
    /// In general, we recommend against using this method.
    /// Users expect to initiate share operations by using the Share charm—when you launch the operation programmatically, you can create an inconsistent user experience.
    /// We include the method because there are a few scenarios in which the user might not recognize opportunities to share.
    /// A good example is when the user achieves a high score in a game.</para>
    /// <para>The <see cref="DataTransferManager"/> class also has a <see cref="TargetApplicationChosen"/> event.
    /// Use this event when you want to capture what applications a user selects when sharing content from your app.</para></remarks>
    public class DataTransferManager
    {
        private static DataTransferManager instance;

        /// <summary>
        /// Returns the <see cref="DataTransferManager"/> object associated with the current window.
        /// </summary>
        /// <returns>The <see cref="DataTransferManager"/> object associated with the current window.</returns>
        public static DataTransferManager GetForCurrentView()
        {
            if (instance == null)
            {
                instance = new DataTransferManager();
            }

            return instance;
        }

        /// <summary>
        /// Programmatically initiates the user interface for sharing content with another app.
        /// </summary>
        public async static void ShowShareUI()
        {
#if __ANDROID__ || __IOS__ || WINDOWS_PHONE
            
                DataRequestedEventArgs e = new DataRequestedEventArgs();
                instance.OnDataRequested(e);

                if (e.Request.Data.data.Count > 0)
                {
                    instance.currentDataPackage = e.Request.Data;
                    DataPackageView view = instance.currentDataPackage.GetView();

                    foreach (string format in view.AvailableFormats)
                    {
                        Debug.WriteLine(format);
                    }
#if __ANDROID__
                    string text = null;
                    if(view.Contains(StandardDataFormats.Text))
                    {
                        text = await view.GetTextAsync();
                    }
                    else if(view.Contains(StandardDataFormats.WebLink ))
                    {
                        text = (await view.GetWebLinkAsync()).ToString();
                    }
                    Intent shareIntent = new Intent();
                    shareIntent.SetAction(Intent.ActionSend);
                    shareIntent.AddFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
                    shareIntent.PutExtra(Intent.ExtraText, text);
                    shareIntent.SetType("text/plain");
                    Intent shareChooserIntent = Intent.CreateChooser(shareIntent, "Share");
                    shareChooserIntent.AddFlags(ActivityFlags.ClearWhenTaskReset);
                Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivity(shareChooserIntent);
                //Platform.Android.ContextManager.Context.ApplicationContext.StartActivity(shareChooserIntent);
#elif __IOS__
                    List<NSObject> values = new List<NSObject>();
                    if (view.Contains(StandardDataFormats.WebLink))
                    {
                        values.Add(new NSUrl((await view.GetWebLinkAsync()).ToString()));
                    }
                    else if (view.Contains(StandardDataFormats.Text))
                    {
                        values.Add(new NSString(await view.GetTextAsync()));
                    }
                    else if (view.Contains(StandardDataFormats.ApplicationLink))
                    {
                        values.Add(new NSUrl((await view.GetApplicationLinkAsync()).ToString()));
                    }
                    UIActivityViewController activity = new UIActivityViewController(values.ToArray(), null);
                    activity.SetCompletionHandler((text,success,items,error)=> {
                        if(success)
                        {
                            instance.OnTargetApplicationChosen(text);
                        }
                        else
                        {
                            Debug.WriteLine(error);
                        }                        
                    });
                    UIViewController currentController = UIApplication.SharedApplication.KeyWindow.RootViewController;
                    while (currentController.PresentedViewController != null)
                    {
                        currentController = currentController.PresentedViewController;
                    }
                    currentController.PresentModalViewController(activity, true);
#else
                /*if (view.Contains(StandardDataFormats.Bitmap))
                {
                    string path = view.GetBitmapFilename();
                    if (string.IsNullOrEmpty(path))
                    {
                        System.IO.Stream /Windows.Storage.Streams.RandomAccessStreamReference/ strRef = await view.GetBitmapAsync();
                        Microsoft.Xna.Framework.Media.MediaLibrary ml = new Microsoft.Xna.Framework.Media.MediaLibrary();
                        Microsoft.Xna.Framework.Media.Picture pic = ml.SavePicture("share", strRef);
                        path = Microsoft.Xna.Framework.Media.PhoneExtensions.MediaLibraryExtensions.GetPath(pic);
                    }

                    Microsoft.Phone.Tasks.ShareMediaTask shareMediaTask = new Microsoft.Phone.Tasks.ShareMediaTask();
                    shareMediaTask.FilePath = path; // "isostore:///shared/shellcontent/share.temp." + filename + ".png";
                    shareMediaTask.Show();
                }
                else
                {*/

                // for 8.0 apps running on 8.1 provide "light-up" to use shell sharing feature
                if (Environment.OSVersion.Version >= new Version(8, 10))
                {
                    if (view.Contains(StandardDataFormats.WebLink))
                    {
                        Microsoft.Phone.Tasks.ShareLinkTask shareLinkTask = new Microsoft.Phone.Tasks.ShareLinkTask();
                        shareLinkTask.LinkUri = await view.GetWebLinkAsync();
                        shareLinkTask.Message = await view.GetTextAsync();
                        shareLinkTask.Title = view.Properties.Title;
                        shareLinkTask.Show();
                    }
                    else if (view.Contains(StandardDataFormats.ApplicationLink))
                    {
                        Microsoft.Phone.Tasks.ShareLinkTask shareLinkTask = new Microsoft.Phone.Tasks.ShareLinkTask();
                        shareLinkTask.LinkUri = await view.GetApplicationLinkAsync();
                        shareLinkTask.Message = await view.GetTextAsync();
                        shareLinkTask.Title = view.Properties.Title;
                        shareLinkTask.Show();
                    }
                    else if (view.Contains(StandardDataFormats.Text))
                    {
                        Microsoft.Phone.Tasks.ShareStatusTask shareStatusTask = new Microsoft.Phone.Tasks.ShareStatusTask();
                        shareStatusTask.Status = await view.GetTextAsync();
                        shareStatusTask.Show();
                    }
                }
                else
                {
                    // use "custom" page to match OS 8.0 support
                    ((Microsoft.Phone.Controls.PhoneApplicationFrame)Application.Current.RootVisual).Navigate(new Uri("/InTheHand.ApplicationModel;component/SharePage.xaml", UriKind.Relative));
                }
                    //}
#endif
            }


            else
            {
                // nothing to share
#if WINDOWS_PHONE
                //System.Windows.MessageBox.Show(Resources.Resources.NothingToShare, Resources.Resources.ShareHeader, MessageBoxButton.OK);
#endif

            }
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81 || WINDOWS_UWP
            Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        private DataTransferManager()
        { 
        }



#if __ANDROID__ || __IOS__ || WINDOWS_PHONE
        internal DataPackage currentDataPackage;

        private void OnDataRequested(DataRequestedEventArgs e)
        {
            if(_dataRequested != null)
            {
                _dataRequested(this, e);
            }
        }
#endif

        private event EventHandler<DataRequestedEventArgs> _dataRequested;
        /// <summary>
        /// Occurs when a share operation starts.
        /// </summary>
        /// <remarks>This event is fired when a sharing operation starts.
        /// To handle this event, you need to add an event listener to the <see cref="DataTransferManager"/> object for the active window.
        /// You can get this object through the <see cref="GetForCurrentView"/> method.
        /// <para>When handling a datarequested event, the most important property you need to be aware of is its request property.
        /// This property contains a <see cref="DataRequest"/> object.
        /// Your app uses this object to provide the data that the user wants to share with a selected target app.</para></remarks>
        public event EventHandler<DataRequestedEventArgs> DataRequested
        {
            add
            {
#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81 || WINDOWS_UWP
                if (_dataRequested == null)
                {
                    Windows.ApplicationModel.DataTransfer.DataTransferManager.GetForCurrentView().DataRequested += DataTransferManager_DataRequested;
                }
#endif
                _dataRequested += value;
            }
            remove
            {
                _dataRequested -= value;
#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81 || WINDOWS_UWP
                if (_dataRequested == null)
                {
                    Windows.ApplicationModel.DataTransfer.DataTransferManager.GetForCurrentView().DataRequested -= DataTransferManager_DataRequested;
                }
#endif
            }
        }
#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81 || WINDOWS_UWP
        private async void DataTransferManager_DataRequested(Windows.ApplicationModel.DataTransfer.DataTransferManager sender, Windows.ApplicationModel.DataTransfer.DataRequestedEventArgs args)
        {
            var deferral = args.Request.GetDeferral();
            if (_dataRequested != null)
            {
                DataRequestedEventArgs e = new DataRequestedEventArgs();
                _dataRequested(this, e);
                if (e.Request.Data != null)
                {
                    DataPackageView view = e.Request.Data.GetView();
                    if (!string.IsNullOrEmpty(view.Properties.Title))
                    {
                        args.Request.Data.Properties.Title = view.Properties.Title;
                    }
                    if (!string.IsNullOrEmpty(view.Properties.Description))
                    {
                        args.Request.Data.Properties.Description = view.Properties.Description;
                    }

                    foreach (string format in view.AvailableFormats)
                    {
                        args.Request.Data.SetData(format, await view.GetDataAsync(format));
                    }
                }
            }
            deferral.Complete();
        }
#endif

        private event EventHandler<TargetApplicationChosenEventArgs> _targetApplicationChosen;
        /// <summary>
        /// Occurs when the user chooses a target app in a Share operation.
        /// </summary>
        /// <remarks>When the user chooses a target app to share content with, the system fires a TargetApplicationChosen event.
        /// The app receiving the event can use this event to record information about the target app for business intelligence.
        /// A common use of this event is to record which applications are used to complete different sharing actions, which in turn can help the source app create better experiences for the user.</remarks>
        public event EventHandler<TargetApplicationChosenEventArgs> TargetApplicationChosen
        {
            add
            {
#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
                if(_targetApplicationChosen == null)
                {
                    Windows.ApplicationModel.DataTransfer.DataTransferManager.GetForCurrentView().TargetApplicationChosen += DataTransferManager_TargetApplicationChosen;
                }
#endif
                _targetApplicationChosen += value;

            }
            remove
            {
                _targetApplicationChosen -= value;
#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
                if (_targetApplicationChosen == null)
                {
                    Windows.ApplicationModel.DataTransfer.DataTransferManager.GetForCurrentView().TargetApplicationChosen -= DataTransferManager_TargetApplicationChosen;
                }
#endif
            }
        }

#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
        private void DataTransferManager_TargetApplicationChosen(Windows.ApplicationModel.DataTransfer.DataTransferManager sender, Windows.ApplicationModel.DataTransfer.TargetApplicationChosenEventArgs args)
        {
            OnTargetApplicationChosen(args.ApplicationName);
        }
#endif

        internal void OnTargetApplicationChosen(string applicationName)
        {
            if (this._targetApplicationChosen != null)
            {
                this._targetApplicationChosen(this, new TargetApplicationChosenEventArgs(applicationName));
            }
        }
    }
}
