// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneCallManager.cs" company="In The Hand Ltd">
//   Copyright (c) 2014-2016 In The Hand Ltd, All rights reserved.
// </copyright>
// <summary>
//   Provides methods for launching the built-in phone call UI.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_PHONE_APP
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.ApplicationModel.Calls.PhoneCallManager))]
#else
using System;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Foundation;

#if __ANDROID__
using Android.App;
using Android.Content;
#elif WINDOWS_APP
using Windows.UI.Popups;
#elif WINDOWS_PHONE
using Microsoft.Phone.Tasks;
#endif

namespace Windows.ApplicationModel.Calls
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
        public static IAsyncOperation<PhoneCallStore> RequestStoreAsync()
        {
#if __ANDROID__
            return Task.FromResult<PhoneCallStore>(new PhoneCallStore()).AsAsyncOperation<PhoneCallStore>();
#elif __IOS__
            return Task.Run<PhoneCallStore>(()=>
            {
                if (UIKit.UIDevice.CurrentDevice.Model == "iPhone")
                {
                    return new PhoneCallStore();
                }
                return null;
            }).AsAsyncOperation<PhoneCallStore>();
            /*#elif WINDOWS_PHONE_APP
                        if (_type10 != null)
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
            return Task.FromResult<PhoneCallStore>(null).AsAsyncOperation<PhoneCallStore>();
        }

        /*internal static void CompletionHandler(IAsyncInfo operation, AsyncStatus status)
        {
            global::System.Diagnostics.Debug.WriteLine(operation.ErrorCode);
            
            Type storeType = Type.GetType("Windows.ApplicationModel.Calls.PhoneCallStore, Windows, ContentType=WindowsRuntime");
            Type template = typeof(Windows.Foundation.IAsyncOperation<>);
            Type genericType = template.MakeGenericType(storeType);
            var nativeStore = genericType.GetRuntimeMethod("GetResults", new Type[0]).Invoke(operation, new object[0]);
        }

        public static Task<object> AsTask(Type type, object operation)
        {
            var tcs = new TaskCompletionSource<object>();
            Type tch = typeof(AsyncOperationCompletedHandler<>).MakeGenericType(type);
            
            var handler = Activator.CreateInstance(tch, (IAsyncOperation<T> i, AsyncStatus s) =>
            {
                switch (i.Status)
                {
                    case AsyncStatus.Completed:
                        tcs.SetResult(i.GetResults());
                        break;
                    case AsyncStatus.Error:
                        tcs.SetException(i.ErrorCode);
                        break;
                    case AsyncStatus.Canceled:
                        tcs.SetCanceled();
                        break;
                }
            });

            typeof(IAsyncOperation<>).MakeGenericType(type).GetRuntimeProperty("Completed").SetValue(operation,null);
            return tcs.Task;
        }*/

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
#endif