//-----------------------------------------------------------------------
// <copyright file="EmailManager.cs" company="In The Hand Ltd">
//     Copyright © 2014-15 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
#if __ANDROID__
using Android.Content;
using Android.App;
#elif __IOS__
using MessageUI;
using UIKit;
#elif WINDOWS_APP
using Windows.System;
#elif WINDOWS_PHONE_APP
using Windows.ApplicationModel.Email;
#elif WINDOWS_PHONE
using Microsoft.Phone.Tasks;
#endif

namespace InTheHand.ApplicationModel.Email
{
    /// <summary>
    /// Allows an application to launch the email application with a new message displayed.
    /// Use this to allow users to send email from your application.
    /// </summary>
    public static class EmailManager
    {
        /// <summary>
        /// Launches the email application with a new message displayed.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Task ShowComposeNewEmailAsync(InTheHand.ApplicationModel.Email.EmailMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException();
            }
#if __ANDROID__
            return Task.Run(() =>
            {
                Intent emailIntent = new Intent(Intent.ActionSendto);
                //emailIntent.SetType("message/rfc822");
                emailIntent.SetData(Android.Net.Uri.Parse("mailto:"));
                emailIntent.PutExtra(Intent.ExtraEmail, RecipientsToStringArray(message.To));
                if (message.CC.Count > 0)
                {
                    emailIntent.PutExtra(Intent.ExtraCc, RecipientsToStringArray(message.CC));
                }

                if (message.Bcc.Count > 0)
                {
                    emailIntent.PutExtra(Intent.ExtraBcc, RecipientsToStringArray(message.Bcc));
                }
                emailIntent.PutExtra(Intent.ExtraSubject, message.Subject);
                emailIntent.PutExtra(Intent.ExtraText, message.Body);
                emailIntent.AddFlags(ActivityFlags.ClearWhenTaskReset);
                Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivity(emailIntent);
                //Platform.Android.ContextManager.Context.StartActivity(emailIntent);
            });
#elif __IOS__
            return Task.Run(() =>
            {
                try
                {
                    MFMailComposeViewController mcontroller = new MFMailComposeViewController();
                    mcontroller.Finished += mcontroller_Finished;

                    mcontroller.SetToRecipients(BuildRecipientArray(message.To));
                    mcontroller.SetCcRecipients(BuildRecipientArray(message.CC));
                    mcontroller.SetBccRecipients(BuildRecipientArray(message.Bcc));
                    mcontroller.SetSubject(message.Subject);
                    mcontroller.SetMessageBody(message.Body, false);

                    UIViewController currentController = UIApplication.SharedApplication.KeyWindow.RootViewController;
                    while (currentController.PresentedViewController != null)
                        currentController = currentController.PresentedViewController;

                    currentController.PresentViewController(mcontroller, true, null);

                }
                catch
                {
                    throw new PlatformNotSupportedException();
                }
            });
#elif WINDOWS_APP
// build URI for Windows Store platform

// build a uri
StringBuilder sb = new StringBuilder();
bool firstParameter = true;

            if (message.To.Count == 0)
            {
                throw new InvalidOperationException();
            }
            else
            {
                sb.Append("mailto:" + FormatRecipientCollection(message.To));
            }

            if (message.CC.Count > 0)
            {
                if (firstParameter)
                {
                    sb.Append("?");
                    firstParameter = false;
                }
                else
                {
                    sb.Append("&");
                }

                sb.Append("cc=" + FormatRecipientCollection(message.CC));
            }

            if (message.Bcc.Count > 0)
            {
                if (firstParameter)
                {
                    sb.Append("?");
                    firstParameter = false;
                }
                else
                {
                    sb.Append("&");
                }

                sb.Append("bbc=" + FormatRecipientCollection(message.Bcc));
            }

            if (!string.IsNullOrEmpty(message.Subject))
            {
                if (firstParameter)
                {
                    sb.Append("?");
                    firstParameter = false;
                }
                else
                {
                    sb.Append("&");
                }

                sb.Append("subject=" + Uri.EscapeDataString(message.Subject));
            }

            if (!string.IsNullOrEmpty(message.Body))
            {
                if (firstParameter)
                {
                    sb.Append("?");
                    firstParameter = false;
                }
                else
                {
                    sb.Append("&");
                }

                sb.Append("body=" + Uri.EscapeDataString(message.Body));
            }

            return Windows.System.Launcher.LaunchUriAsync(new Uri(sb.ToString())).AsTask<bool>(); 
#elif WINDOWS_PHONE_APP || WINDOWS_UWP
            Windows.ApplicationModel.Email.EmailMessage nativeMessage = new Windows.ApplicationModel.Email.EmailMessage();
            nativeMessage.Subject = message.Subject;
            nativeMessage.Body = message.Body;
            foreach (EmailRecipient toRecipient in message.To)
            {
                nativeMessage.To.Add(new Windows.ApplicationModel.Email.EmailRecipient(toRecipient.Address, toRecipient.Name));
            }
            foreach (EmailRecipient ccRecipient in message.CC)
            {
                nativeMessage.To.Add(new Windows.ApplicationModel.Email.EmailRecipient(ccRecipient.Address, ccRecipient.Name));
            }
            foreach (EmailRecipient bccRecipient in message.Bcc)
            {
                nativeMessage.To.Add(new Windows.ApplicationModel.Email.EmailRecipient(bccRecipient.Address, bccRecipient.Name));
            }

            return Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(nativeMessage).AsTask();
#elif WINDOWS_PHONE
            return Task.Run(() =>
            {
                // On Phone 8.0 use the email compose dialog
                EmailComposeTask task = new EmailComposeTask();

                if (!string.IsNullOrEmpty(message.Subject))
                {
                    task.Subject = message.Subject;
                }

                if (!string.IsNullOrEmpty(message.Body))
                {
                    task.Body = message.Body;
                }

                if (message.To.Count == 0)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    task.To = FormatRecipientCollection(message.To);
                }

                if (message.CC.Count > 0)
                {
                    task.Cc = FormatRecipientCollection(message.CC);
                }

                if (message.Bcc.Count > 0)
                {
                    task.Bcc = FormatRecipientCollection(message.Bcc);
                }

                task.Show();
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

#if __IOS__
        private static string[] BuildRecipientArray(IList<EmailRecipient> list)
        {
            string[] array = new string[list.Count];

            for(int i = 0; i < list.Count; i++)
            {
                array[i] = list[i].Address;
            }

            return array;
        }

        static void mcontroller_Finished(object sender, MFComposeResultEventArgs e)
        {
            e.Controller.DismissViewController(true, null);
        }
#endif

#if __ANDROID__
        private static string[] RecipientsToStringArray(IList<EmailRecipient> list)
        {
            List<string> recipients = new List<string>();
            foreach(EmailRecipient em in list)
            {
                recipients.Add(em.ToMailString());
            }

            return recipients.ToArray();
        }
#endif

#if WINDOWS_APP || WINDOWS_PHONE
        private static string FormatRecipientCollection(IList<EmailRecipient> list)
        {
            StringBuilder builder = new StringBuilder();

            foreach(EmailRecipient recipient in list)
            {
                if(!string.IsNullOrEmpty(recipient.Address))
                {
                    builder.Append(recipient.Address + ",");
                }
            }

            if(builder.Length > 0)
            {
                // trim final comma
                builder.Length--;
            }

            return builder.ToString();
        }
#endif
    }
}