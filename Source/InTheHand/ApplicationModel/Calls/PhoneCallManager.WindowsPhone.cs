// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneCallManager.WindowsPhone.cs" company="In The Hand Ltd">
//     Copyright (c) 2014-2017 In The Hand Ltd, All rights reserved.
//     This source code is licensed under the MIT License - see License.txt
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.Phone.Tasks;

namespace InTheHand.ApplicationModel.Calls
{
    static partial class PhoneCallManager
    {
        private static void DoShowPhoneCallUI(string phoneNumber, string displayName, bool suppressPrompt)
        {
            PhoneCallTask pct = new PhoneCallTask();
            pct.PhoneNumber = phoneNumber;
            pct.DisplayName = displayName;
            pct.Show();
        }
    }
}