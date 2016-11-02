//-----------------------------------------------------------------------
// <copyright file="EmailMessage.cs" company="In The Hand Ltd">
//     Copyright © 2014-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.ApplicationModel.Email.EmailMessage))]
//#else
using System.Collections.Generic;

namespace InTheHand.ApplicationModel.Email
{
    /// <summary>
    /// Represents an email message.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list>
    /// </remarks>
    public sealed class EmailMessage
    {
        /// <summary>
        /// Creates an instance of the <see cref="EmailMessage"/> class.
        /// </summary>
        public EmailMessage()
        {
            To = new List<EmailRecipient>();
            CC = new List<EmailRecipient>();
            Bcc = new List<EmailRecipient>();
        }

#if __IOS__ || WINDOWS_UWP || WINDOWS_PHONE_APP
        private List<EmailAttachment> _attachments = new List<EmailAttachment>();
        public IList<EmailAttachment> Attachments
        {
            get
            {
                return _attachments;
            }
        }
#endif

        /// <summary>
        /// Gets or sets the subject of the email message.
        /// </summary>
        public string Subject { set; get; }

        /// <summary>
        /// Gets or sets the body of the email message.
        /// </summary>
        public string Body { set; get; }

        /// <summary>
        /// Gets the direct recipients of the email message.
        /// </summary>
        public IList<EmailRecipient> To { get; private set; }

        /// <summary>
        /// Gets the recipients CC'd to the email message.
        /// </summary>
        public IList<EmailRecipient> CC { get; private set; }

        /// <summary>
        /// Gets the recipients BCC'd to the email message.
        /// </summary>
        public IList<EmailRecipient> Bcc { get; private set; }
    }
}
//#endif