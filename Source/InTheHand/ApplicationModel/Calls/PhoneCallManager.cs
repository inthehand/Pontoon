// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneCallManager.cs" company="In The Hand Ltd">
//   Copyright (c) 2014-2016 In The Hand Ltd, All rights reserved.
// </copyright>
// <summary>
//   Provides methods for launching the built-in phone call UI.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;
using System.Threading.Tasks;

#if __ANDROID__
using Android.App;
using Android.Content;
#elif WINDOWS_APP
using Windows.UI.Popups;
#elif WINDOWS_PHONE_APP
using Windows.ApplicationModel.Calls;
#elif WINDOWS_PHONE
using Microsoft.Phone.Tasks;
#endif

namespace InTheHand.ApplicationModel.Calls
{
    /// <summary>
    /// Provides methods for launching the built-in phone call UI.
    /// </summary>
    public static class PhoneCallManager
    {
#if WINDOWS_APP || WINDOWS_PHONE_APP
        private static Type _type10;

        static PhoneCallManager()
        {
            _type10 = Type.GetType("Windows.ApplicationModel.Calls.PhoneCallManager, Windows, ContentType=WindowsRuntime");
        }
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<PhoneCallStore> RequestStoreAsync()
        {
#if __ANDROID__ || __IOS__
            return new PhoneCallStore();
#elif WINDOWS_UWP
            if (Windows.Foundation.Metadata.ApiInformation.IsMethodPresent("Windows.ApplicationModel.Calls.PhoneCallManager", "RequestStoreAsync"))
            {
                return new PhoneCallStore(await Windows.ApplicationModel.Calls.PhoneCallManager.RequestStoreAsync());
            }
#elif WINDOWS_APP || WINDOWS_PHONE_APP
            if (_type10 != null)
            {
                Type storeType = Type.GetType("Windows.ApplicationModel.Calls.PhoneCallStore, Windows, ContentType=WindowsRuntime");
                Type template = typeof(Windows.Foundation.IAsyncOperation<>);
                Type genericType = template.MakeGenericType(storeType);
                object nativeStoreTask = _type10.GetRuntimeMethod("RequestStoreAsync", new Type[0]).Invoke(null, new object[0]);
                Windows.Foundation.IAsyncOperation<object> op = nativeStoreTask as Windows.Foundation.IAsyncOperation<object>;
                
                var nativeStore = genericType.GetRuntimeMethod("GetResults", new Type[0]).Invoke(nativeStoreTask, new object[0]);
                //object nativeStore = nativeStoreTask.GetType().GetRuntimeMethod("GetResults",new Type[0]).Invoke(nativeStoreTask, new object[0]);
                return new PhoneCallStore(await (Windows.Foundation.IAsyncOperation<object>)nativeStoreTask);
            }
#endif
            return null;
        }
        
        /// <summary>
        /// Launches the built-in phone call UI with the specified phone number and display name.
        /// </summary>
        /// <param name="phoneNumber">A phone number.
        /// This should be in international format e.g. +12345678901</param>
        /// <param name="displayName">A display name.</param>
        public static void ShowPhoneCallUI(string phoneNumber, string displayName)
        {
#if __ANDROID__
            string action = Intent.ActionDial; //promptUser ? Intent.ActionDial : Intent.ActionCall;
            Intent callIntent = new Intent(action, Android.Net.Uri.FromParts("tel", CleanPhoneNumber(phoneNumber), null));
            callIntent.AddFlags(ActivityFlags.ClearWhenTaskReset);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivity(callIntent);
            //Platform.Android.ContextManager.Context.StartActivity(callIntent);
#elif __IOS__
            if (UIKit.UIDevice.CurrentDevice.Model != "iPhone")
            {
                UIKit.UIAlertView av = new UIKit.UIAlertView("Phone", "Dial " + phoneNumber, null, "Done");
                av.Show();
            }
            else
            {
                global::Foundation.NSUrl url = new global::Foundation.NSUrl("telprompt:" + CleanPhoneNumber(phoneNumber));        
                UIKit.UIApplication.SharedApplication.OpenUrl(url);
            } 
#elif WINDOWS_APP
            MessageDialog prompt = new MessageDialog(string.Format("Dial {0} at {1}?", displayName, phoneNumber), "Phone");
            prompt.Commands.Add(new UICommand("Call", async (c) =>
                {
                        // Windows may prompt the user for an app e.g. Skype, Lync etc
                        await Windows.System.Launcher.LaunchUriAsync(new Uri("tel:" + CleanPhoneNumber(phoneNumber)));
                }));
            prompt.Commands.Add(new UICommand("Cancel", null));
            prompt.ShowAsync();

#elif WINDOWS_PHONE_APP || WINDOWS_UWP
            PhoneCallManager.ShowPhoneCallUI(phoneNumber, displayName);

#elif WINDOWS_PHONE
            PhoneCallTask pct = new PhoneCallTask();
            pct.PhoneNumber = phoneNumber;
            pct.DisplayName = displayName;
            pct.Show();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        internal static string CleanPhoneNumber(string phoneNumber)
        {
            return phoneNumber.Replace(" ", "");
        }
    }
}
