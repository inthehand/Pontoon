//-----------------------------------------------------------------------
// <copyright file="ChatMessageManager.cs" company="In The Hand Ltd">
//     Copyright © 2014-15 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Text;
using System.Threading.Tasks;
#if __ANDROID__
using Android.Content;
using Android.App;
#elif __IOS__
using MessageUI;
using UIKit;
#elif WINDOWS_APP || WINDOWS_PHONE_APP
using Windows.Foundation;
#else
#endif

namespace InTheHand.ApplicationModel.Chat
{
    /// <summary>
    /// Provides methods for managing chat messages.
    /// </summary>
    public static class ChatMessageManager
    {
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
                    MFMessageComposeViewController mcontroller = new MFMessageComposeViewController();
                    mcontroller.Finished += mcontroller_Finished;

                    string[] recipients = new string[message.Recipients.Count];
                    message.Recipients.CopyTo(recipients, 0);
                    mcontroller.Recipients = recipients;
                    mcontroller.Body = message.Body;

                    UIViewController currentController = UIApplication.SharedApplication.KeyWindow.RootViewController;
                    while (currentController.PresentedViewController != null)
                        currentController = currentController.PresentedViewController;

                    currentController.PresentViewController(mcontroller, true, null);

                }
                catch
                {
                    // probably an iPod/iPad
                    throw new PlatformNotSupportedException();
                }
            });
#elif WINDOWS_APP
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

            return Windows.System.Launcher.LaunchUriAsync(new Uri(sb.ToString())).AsTask<bool>();
#elif WINDOWS_PHONE_APP || WINDOWS_UWP
            Windows.ApplicationModel.Chat.ChatMessage nativeMessage = new Windows.ApplicationModel.Chat.ChatMessage() { Body = message.Body };
            foreach(string recipient in message.Recipients)
            {
                nativeMessage.Recipients.Add(recipient);
            }

            return Windows.ApplicationModel.Chat.ChatMessageManager.ShowComposeSmsMessageAsync(nativeMessage).AsTask();
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
    }
}
