//-----------------------------------------------------------------------
// <copyright file="DisplayRequest.Android.cs" company="In The Hand Ltd">
//     Copyright © 2017 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Android.App;
using Android.Views;

namespace InTheHand.System.Display
{
    public sealed partial class DisplayRequest
    {
        private void RequestActiveImpl()
        {
            Activity a = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;

            if (a != null)
            {
                a.Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            }
        }
        
        private void RequestReleaseImpl()
        {
            Activity a = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;

            if (a != null)
            {
                a.Window.ClearFlags(WindowManagerFlags.KeepScreenOn);
            }
        }
    }
}