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