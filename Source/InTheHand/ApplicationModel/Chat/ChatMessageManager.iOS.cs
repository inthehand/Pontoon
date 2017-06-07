//-----------------------------------------------------------------------
// <copyright file="ChatMessageManager.iOS.cs" company="In The Hand Ltd">
//     Copyright © 2014-17 In The Hand Ltd. All rights reserved.
//     This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using MessageUI;
using UIKit;

namespace InTheHand.ApplicationModel.Chat
{
    static partial class ChatMessageManager
    {
        private static Task ShowComposeSmsMessageAsyncImpl(ChatMessage message)
        {
            return Task.Run(() =>
            {
                try
                {
                    string[] recipients = new string[message.Recipients.Count];
                    message.Recipients.CopyTo(recipients, 0);

                    UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
                    {
                        MFMessageComposeViewController mcontroller = new MFMessageComposeViewController();
                        mcontroller.Finished += mcontroller_Finished;
                        
                        mcontroller.Recipients = recipients;
                        mcontroller.Body = message.Body;

                        UIViewController currentController = UIApplication.SharedApplication.KeyWindow.RootViewController;
                        while (currentController.PresentedViewController != null)
                            currentController = currentController.PresentedViewController;

                        currentController.PresentViewController(mcontroller, true, null);
                    });

                }
                catch(Exception ex)
                {
                    global::System.Diagnostics.Debug.WriteLine(ex);
                    // probably an iPod/iPad
                    throw new PlatformNotSupportedException();
                }
            });
        }
        
        private static void mcontroller_Finished(object sender, MFMessageComposeResultEventArgs e)
        {
            e.Controller.DismissViewController(true, null);
        }
    }
}