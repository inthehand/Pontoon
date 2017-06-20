// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneCallManager.Android.cs" company="In The Hand Ltd">
//     Copyright (c) 2014-2017 In The Hand Ltd, All rights reserved.
//     This source code is licensed under the MIT License - see License.txt
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using InTheHand.UI.Popups;

using Foundation;
using UIKit;

namespace InTheHand.ApplicationModel.Calls
{
    static partial class PhoneCallManager
    {
        private static void DoShowPhoneCallUI(string phoneNumber, string displayName, bool suppressPrompt)
        {
            if (UIDevice.CurrentDevice.Model != "iPhone")
            {
                MessageDialog dialog = new MessageDialog("Dial " + phoneNumber, "Phone");
                dialog.ShowAsync();
            }
            else
            {
                NSUrl url = new NSUrl("tel:" + CleanPhoneNumber(phoneNumber));
                UIApplication.SharedApplication.OpenUrl(url);
            }
        }
    }
}