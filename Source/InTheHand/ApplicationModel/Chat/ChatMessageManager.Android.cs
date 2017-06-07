//-----------------------------------------------------------------------
// <copyright file="ChatMessageManager.Android.cs" company="In The Hand Ltd">
//     Copyright © 2014-17 In The Hand Ltd. All rights reserved.
//     This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Android.Content;
using Android.App;

namespace InTheHand.ApplicationModel.Chat
{
    static partial class ChatMessageManager
    {
        private static Task ShowComposeSmsMessageAsyncImpl(ChatMessage message)
        {
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
        }
        
        private static void ShowSmsSettingsImpl()
        {
            Intent settingsIntent = new Intent(Android.Provider.Settings.ActionDataRoamingSettings);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivity(settingsIntent);
        }
    }
}