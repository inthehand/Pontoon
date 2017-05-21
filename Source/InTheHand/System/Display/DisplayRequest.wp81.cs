//-----------------------------------------------------------------------
// <copyright file="DisplayRequest.wp81.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
//     This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.Phone.Shell;

namespace InTheHand.System.Display
{
    partial class DisplayRequest
    {
        private void RequestActiveImpl()
        {
            PhoneApplicationService.Current.ApplicationIdleDetectionMode = IdleDetectionMode.Disabled;
        }

        private void RequestReleaseImpl()
        {
            PhoneApplicationService.Current.ApplicationIdleDetectionMode = IdleDetectionMode.Enabled;
        }
    }
}