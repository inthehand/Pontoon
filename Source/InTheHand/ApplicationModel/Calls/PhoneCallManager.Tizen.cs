// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneCallManager.Tizen.cs" company="In The Hand Ltd">
//     Copyright (c) 2014-2017 In The Hand Ltd, All rights reserved.
//     This source code is licensed under the MIT License - see License.txt
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Tizen.Applications;

namespace InTheHand.ApplicationModel.Calls
{
    static partial class PhoneCallManager
    {
        private static void DoShowPhoneCallUI(string phoneNumber, string displayName, bool suppressPrompt)
        {
            var appControl = new AppControl()
            {
                ApplicationId =
                    suppressPrompt ? "http://tizen.org/appcontrol/operation/call" : "http://tizen.org/appcontrol/operation/dial",
                Uri =
                    "tel:" + CleanPhoneNumber(phoneNumber)
            };

            AppControl.SendLaunchRequest(appControl);
        }
    }
}