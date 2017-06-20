// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneCallManager.Android.cs" company="In The Hand Ltd">
//     Copyright (c) 2014-2017 In The Hand Ltd, All rights reserved.
//     This source code is licensed under the MIT License - see License.txt
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Android.Content;

namespace InTheHand.ApplicationModel.Calls
{
    static partial class PhoneCallManager
    {
        private static void DoShowPhoneCallUI(string phoneNumber, string displayName, bool suppressPrompt)
        {
            string action = suppressPrompt ? Intent.ActionCall : Intent.ActionDial;
            Intent callIntent = new Intent(action, Android.Net.Uri.FromParts("tel", CleanPhoneNumber(phoneNumber), null));
            callIntent.AddFlags(ActivityFlags.ClearWhenTaskReset);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivity(callIntent);
            //Platform.Android.ContextManager.Context.StartActivity(callIntent);
        }
    }
}