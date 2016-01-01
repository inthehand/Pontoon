// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneCallManager.cs" company="In The Hand Ltd">
//   Copyright (c) 2014-2015 In The Hand Ltd, All rights reserved.
// </copyright>
// <summary>
//   Provides methods for launching the built-in phone call UI.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

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
        /// <summary>
        /// Launches the built-in phone call UI with the specified phone number and display name.
        /// </summary>
        /// <param name="phoneNumber">A phone number.
        /// This should be in international format e.g. +12345678901</param>
        /// <param name="displayName">A display name.</param>
        public static void ShowPhoneCallUI(string phoneNumber, string displayName)
        {
            ShowPhoneCallUI(phoneNumber, displayName, true);
        }
        
        /// <summary>
        /// Launches the built-in phone call UI with the specified phone number and display name.
        /// </summary>
        /// <param name="phoneNumber">A phone number.
        /// This should be in international format e.g. +12345678901</param>
        /// <param name="displayName">A display name.</param>
        /// <param name="promptUser">A value indicating whether to prompt the user first.
        /// Not supported on Windows platforms.</param>
        public static void ShowPhoneCallUI(string phoneNumber, string displayName, bool promptUser)
        {
#if __ANDROID__
            string action = promptUser ? Intent.ActionDial : Intent.ActionCall;
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
                Foundation.NSUrl url = null;
                if (promptUser)
                {
                    url = new Foundation.NSUrl("telprompt:" + CleanPhoneNumber(phoneNumber));
                }
                else
                {
                    url = new Foundation.NSUrl("tel:" + CleanPhoneNumber(phoneNumber));
                }
                
                UIKit.UIApplication.SharedApplication.OpenUrl(url);
            } 
#elif WINDOWS_APP
            if (promptUser)
            {
                MessageDialog prompt = new MessageDialog(string.Format("Dial {0} at {1}?", displayName, phoneNumber), "Phone");
                prompt.Commands.Add(new UICommand("Call", async (c) =>
                    {
                        // Windows may prompt the user for an app e.g. Skype, Lync etc
                        await Windows.System.Launcher.LaunchUriAsync(new Uri("tel:" + CleanPhoneNumber(phoneNumber)));
                    }));
                prompt.Commands.Add(new UICommand("Cancel", null));
                prompt.ShowAsync();
            }
            else
            {
                // Windows may prompt the user for an app e.g. Skype, Lync etc
                Windows.System.Launcher.LaunchUriAsync(new Uri("tel:" + CleanPhoneNumber(phoneNumber)));
            }

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

        private static string CleanPhoneNumber(string phoneNumber)
        {
            return phoneNumber.Replace(" ", "");
        }
    }
}
