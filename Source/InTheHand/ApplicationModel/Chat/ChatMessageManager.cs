//-----------------------------------------------------------------------
// <copyright file="ChatMessageManager.cs" company="In The Hand Ltd">
//     Copyright © 2014-17 In The Hand Ltd. All rights reserved.
//     This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
#if __ANDROID__
using Android.Content;
using Android.App;
#elif __IOS__
using MessageUI;
using UIKit;
#endif

namespace InTheHand.ApplicationModel.Chat
{
    /// <summary>
    /// Provides methods for managing chat messages.
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
    public static class ChatMessageManager
    {
#if WINDOWS_APP || WIN32
        private static Type _type10;

        static ChatMessageManager()
        {
            _type10 = Type.GetType("Windows.ApplicationModel.Chat.ChatMessageManager, Windows, ContentType=WindowsRuntime");
        }
#endif

        /// <summary>
        /// Shows the compose SMS dialog, pre-populated with data from the supplied ChatMessage object, allowing the user to send an SMS message.
        /// </summary>
        /// <param name="message">The chat message.</param>
        /// <returns>An asynchronous action.</returns>
        public static Task ShowComposeSmsMessageAsync(ChatMessage message)
        {
#if __ANDROID__
            return Task.Run(() =>
            {
                StringBuilder addresses = new StringBuilder();
                foreach (string recipient in message.Recipients)
                {
                    addresses.Append(recipient + ";");
                }
                if (addresses.Length > 0)
                {
                    // trim final semicolon
                    addresses.Length--;
                }
                Intent smsIntent = new Intent(Intent.ActionSendto, global::Android.Net.Uri.Parse("smsto:" + addresses.ToString()));
                smsIntent.PutExtra("sms_body", message.Body);
                smsIntent.AddFlags(ActivityFlags.ClearWhenTaskReset);
                Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivity(smsIntent);
                //Platform.Android.ContextManager.Context.StartActivity(smsIntent);
            });
#elif __IOS__
            return Task.Run(() =>
            {
                try
                {
                    string[] recipients = new string[message.Recipients.Count];
                    message.Recipients.CopyTo(recipients, 0);

                    UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
                    {
                        MFMessageComposeViewController mcontroller = new MFMessageComposeViewController();
                        mcontroller.Finished += mcontroller_Finished;
                        
                        mcontroller.Recipients = recipients;
                        mcontroller.Body = message.Body;

                        UIViewController currentController = UIApplication.SharedApplication.KeyWindow.RootViewController;
                        while (currentController.PresentedViewController != null)
                            currentController = currentController.PresentedViewController;

                        currentController.PresentViewController(mcontroller, true, null);
                    });

                }
                catch(Exception ex)
                {
                    global::System.Diagnostics.Debug.WriteLine(ex);
                    // probably an iPod/iPad
                    throw new PlatformNotSupportedException();
                }
            });
#elif WINDOWS_UWP || WINDOWS_PHONE_APP
            Windows.ApplicationModel.Chat.ChatMessage m = new Windows.ApplicationModel.Chat.ChatMessage();
            foreach (string r in message.Recipients)
            {
                m.Recipients.Add(r);
            }
            m.Body = message.Body;

            return Windows.ApplicationModel.Chat.ChatMessageManager.ShowComposeSmsMessageAsync(m).AsTask();
#elif WINDOWS_APP || WIN32 || __MAC__
            return Task.Run(async () =>
            {
                // build uri
                StringBuilder sb = new StringBuilder();

                if (message.Recipients.Count == 0)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    sb.Append("sms:");

                    foreach (string recipient in message.Recipients)
                    {
                        sb.Append(recipient + ";");
                    }

                    // Remove last semi-colon
                    if (sb.Length > 4)
                    {
                        sb.Length -= 1;
                    }
                }

                // add body if present
                if (!string.IsNullOrEmpty(message.Body))
                {
                    sb.Append("?");
                    sb.Append("body=" + Uri.EscapeDataString(message.Body));
                }

                await InTheHand.System.Launcher.LaunchUriAsync(new Uri(sb.ToString()));
            });
#elif WINDOWS_PHONE
            return Task.Run(() =>
            {
                Microsoft.Phone.Tasks.SmsComposeTask composeTask = new Microsoft.Phone.Tasks.SmsComposeTask();

                composeTask.Body = message.Body;

                StringBuilder recipients = new StringBuilder();
                foreach (string recipient in message.Recipients)
                {
                    recipients.Append(recipient + ";");
                }

                // Remove last ;
                if (recipients.Length > 0)
                {
                    recipients.Length -= 1;
                }

                composeTask.To = recipients.ToString();
                composeTask.Show();
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

#if __IOS__
        static void mcontroller_Finished(object sender, MFMessageComposeResultEventArgs e)
        {
            e.Controller.DismissViewController(true, null);
        }
#endif
        /// <summary>
        /// Launches the device's SMS settings app.
        /// </summary>
        public static void ShowSmsSettings()
        {
#if __ANDROID__
            Intent settingsIntent = new Intent(Android.Provider.Settings.ActionDataRoamingSettings);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivity(settingsIntent);

#elif WINDOWS_UWP || WINDOWS_PHONE_APP
            Windows.ApplicationModel.Chat.ChatMessageManager.ShowSmsSettings();

#elif WINDOWS_APP || WIN32
            if(_type10 != null)
            {
                _type10.GetRuntimeMethod("ShowSmsSettings", new Type[0]).Invoke(null, new object[0]);
            }

#elif WINDOWS_PHONE
            Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-cellular:"));
#endif
        }
    }
}