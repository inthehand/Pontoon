// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneCallManager.cs" company="In The Hand Ltd">
//     Copyright (c) 2014-2017 In The Hand Ltd, All rights reserved.
//     This source code is licensed under the MIT License - see License.txt
// </copyright>
// <summary>
//   Provides methods for launching the built-in phone call UI.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using InTheHand.UI.Popups;
using InTheHand.System;
using System;
using System.Reflection;
using System.Threading.Tasks;

#if __ANDROID__
using Android.App;
using Android.Content;
#elif WINDOWS_PHONE
using Microsoft.Phone.Tasks;
#endif

namespace InTheHand.ApplicationModel.Calls
{
    /// <summary>
    /// Provides methods for launching the built-in phone call UI.
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list>
    /// </remarks>
    public static class PhoneCallManager
    {
#if WINDOWS_APP
        private static Type _type10;

        static PhoneCallManager()
        {
            _type10 = Type.GetType("Windows.ApplicationModel.Calls.PhoneCallManager, Windows, ContentType=WindowsRuntime");
        }
#endif

#if !WIN32 && !__MAC__
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Task<PhoneCallStore> RequestStoreAsync()
        {
#if __ANDROID__
            return Task.FromResult<PhoneCallStore>(new PhoneCallStore());
#elif __IOS__
            return Task.Run<PhoneCallStore>(()=>
            {
                if (UIKit.UIDevice.CurrentDevice.Model == "iPhone")
                {
                    return new PhoneCallStore();
                }
                return null;
            });
#elif WINDOWS_UWP
            return Task.Run<PhoneCallStore>(async () => {
                if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.ApplicationModel.Calls.PhoneCallStore"))
                {
                    return await Windows.ApplicationModel.Calls.PhoneCallManager.RequestStoreAsync();
                }

                return null;
            });
#elif WINDOWS_PHONE_APP
                        /*if (_type10 != null)
                        {
                            Type storeType = Type.GetType("Windows.ApplicationModel.Calls.PhoneCallStore, Windows, ContentType=WindowsRuntime");
                            object act = _type10.GetRuntimeProperty("IsCallActive").GetValue(null);

                            Type template = typeof(Windows.Foundation.IAsyncOperation<>);
                            Type genericType = template.MakeGenericType(storeType);
                            Type deltype = typeof(AsyncOperationCompletedHandler<>).MakeGenericType(storeType);
                            object nativeStoreTask = _type10.GetRuntimeMethod("RequestStoreAsync", new Type[0]).Invoke(null, new object[0]);
                            Type pcsType = typeof(PhoneCallManager);
                            MethodInfo completionInfo = pcsType.GetTypeInfo().GetDeclaredMethod("CompletionHandler");
                            Delegate del = completionInfo.CreateDelegate(deltype);

                            genericType.GetRuntimeProperty("Completed").SetValue(nativeStoreTask, del);

                            //object nativeStore = nativeStoreTask.GetType().GetRuntimeMethod("GetResults",new Type[0]).Invoke(nativeStoreTask, new object[0]);
                            //return new PhoneCallStore(await (Windows.Foundation.IAsyncOperation<object>)nativeStoreTask);
                        }*/
#endif
            return Task.FromResult<PhoneCallStore>(null);
        }
#endif
        /*internal static void CompletionHandler(IAsyncInfo operation, AsyncStatus status)
        {
            global::System.Diagnostics.Debug.WriteLine(operation.ErrorCode);
            
            Type storeType = Type.GetType("Windows.ApplicationModel.Calls.PhoneCallStore, Windows, ContentType=WindowsRuntime");
            Type template = typeof(Windows.Foundation.IAsyncOperation<>);
            Type genericType = template.MakeGenericType(storeType);
            var nativeStore = genericType.GetRuntimeMethod("GetResults", new Type[0]).Invoke(operation, new object[0]);
        }
*/

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
#elif WINDOWS_UWP || WINDOWS_PHONE_APP
#if WINDOWS_UWP
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.ApplicationModel.Calls.PhoneCallManager"))
            {
#endif
                Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(phoneNumber, displayName);
#if WINDOWS_UWP
            }
            else
            {
                Windows.System.Launcher.LaunchUriAsync(new Uri("tel:" + phoneNumber));
            }
#endif
#elif WINDOWS_APP || WIN32 || __MAC__
            MessageDialog prompt = new MessageDialog(string.Format("Dial {0} at {1}?", displayName, phoneNumber), "Phone");
            prompt.Commands.Add(new UICommand("Call", async (c) =>
                {
                        // OS may prompt the user for an app e.g. Skype, Lync etc
                        await Launcher.LaunchUriAsync(new Uri("tel:" + CleanPhoneNumber(phoneNumber)));
                }));
            prompt.Commands.Add(new UICommand("Cancel", null));
            prompt.ShowAsync();

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