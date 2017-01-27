//-----------------------------------------------------------------------
// <copyright file="EmailManager.cs" company="In The Hand Ltd">
//     Copyright © 2014-17 In The Hand Ltd. All rights reserved.
//     This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
#if __ANDROID__
using Android.Content;
using Android.App;
#elif __IOS__
using Foundation;
using MessageUI;
using UIKit;
#elif WINDOWS_PHONE
using Microsoft.Phone.Tasks;
#endif

namespace InTheHand.ApplicationModel.Email
{
    /// <summary>
    /// Allows an application to launch the email application with a new message displayed.
    /// Use this to allow users to send email from your application.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list>
    /// </remarks>
    public static class EmailManager
    {
#if WINDOWS_APP || WIN32
        private static Type _type10 = Type.GetType("Windows.ApplicationModel.Email.EmailManager, Windows, ContentType=WindowsRuntime");
#endif
        /// <summary>
        /// Launches the email application with a new message displayed.
        /// </summary>
        /// <param name="message">The email message that is displayed when the email application is launched.</param>
        /// <returns>An asynchronous action used to indicate when the operation has completed.</returns>
        public static Task ShowComposeNewEmailAsync(EmailMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException();
            }

            return Task.Run(() =>
            {
#if __ANDROID__
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
#elif __IOS__
                try
                {
                    UIApplication.SharedApplication.InvokeOnMainThread(() =>
                    {
                        MFMailComposeViewController mcontroller = new MFMailComposeViewController();
                        mcontroller.Finished += mcontroller_Finished;

                        mcontroller.SetToRecipients(BuildRecipientArray(message.To));
                        mcontroller.SetCcRecipients(BuildRecipientArray(message.CC));
                        mcontroller.SetBccRecipients(BuildRecipientArray(message.Bcc));
                        mcontroller.SetSubject(message.Subject);
                        mcontroller.SetMessageBody(message.Body, false);
                        foreach (EmailAttachment a in message.Attachments)
                        {
                            NSData dataBuffer = NSData.FromFile(a.Data.Path);
                            mcontroller.AddAttachmentData(dataBuffer, a.MimeType, a.FileName);
                        }

                        UIViewController currentController = UIApplication.SharedApplication.KeyWindow.RootViewController;
                        while (currentController.PresentedViewController != null)
                            currentController = currentController.PresentedViewController;

                        currentController.PresentViewController(mcontroller, true, null);
                    });
                }
                catch
                {
                    throw new PlatformNotSupportedException();
                }
#elif WINDOWS_UWP || WINDOWS_PHONE_APP
                Windows.ApplicationModel.Email.EmailMessage em = new Windows.ApplicationModel.Email.EmailMessage() { Subject = message.Subject, Body = message.Body };
                foreach(EmailRecipient r in message.To)
                {
                    em.To.Add(new Windows.ApplicationModel.Email.EmailRecipient(r.Address, r.Name));
                }
                foreach(EmailRecipient r in message.CC)
                {
                    em.CC.Add(new Windows.ApplicationModel.Email.EmailRecipient(r.Address, r.Name));
                }
                foreach (EmailRecipient r in message.Bcc)
                {
                    em.Bcc.Add(new Windows.ApplicationModel.Email.EmailRecipient(r.Address, r.Name));
                }
                foreach(EmailAttachment a in message.Attachments)
                {
                    em.Attachments.Add(new Windows.ApplicationModel.Email.EmailAttachment(a.FileName, (Windows.Storage.StorageFile)a.Data));
                }

                return Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(em).AsTask();
#elif WINDOWS_APP || WIN32 || __MAC__
                /*if (_type10 != null)
                {

                }
                else
                {*/
                    // build URI

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

                    InTheHand.System.Launcher.LaunchUriAsync(new Uri(sb.ToString()));
                //}
#elif WINDOWS_PHONE
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
#else
                throw new PlatformNotSupportedException();
#endif
            });
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

#if WINDOWS_APP || WINDOWS_PHONE || WIN32 || __MAC__
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