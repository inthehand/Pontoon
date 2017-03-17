//-----------------------------------------------------------------------
// <copyright file="DisplayRequest.iOS.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
//     This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using UIKit;

namespace InTheHand.System.Display
{
    public sealed partial class DisplayRequest
    {
        private void RequestActiveImpl()
        {
            UIApplication.SharedApplication.IdleTimerDisabled = true;
        }

        private void RequestReleaseImpl()
        {
            UIApplication.SharedApplication.IdleTimerDisabled = false;
        }
    }
}