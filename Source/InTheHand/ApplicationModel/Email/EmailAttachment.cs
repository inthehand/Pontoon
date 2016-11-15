//-----------------------------------------------------------------------
// <copyright file="EmailAttachment.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Storage;
using System.Collections.Generic;

namespace InTheHand.ApplicationModel.Email
{
    /// <summary>
    /// Represents an email attachment.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item></list>
    /// </remarks>
    public sealed class EmailAttachment
    {
        /// <summary>
        /// Creates an instance of the <see cref="EmailAttachment"/> class with the specified data.
        /// </summary>
        /// <param name="filename">The filename of the attachment.</param>
        /// <param name="data">The stream to use to download the attachment.</param>
        public EmailAttachment(string filename, StorageFile data)
        {
            FileName = filename;
            Data = data;
            MimeType = data.ContentType;
        }

        /// <summary>
        /// Initializes a new instance of the see cref="EmailAttachment"/> class.
        /// </summary>
        /// <param name="filename">The filename of the attachment.</param>
        /// <param name="data">The stream to use to download the attachment.</param>
        /// <param name="mimeType">The MIME type of the attachment.</param>
        public EmailAttachment(string filename, StorageFile data, string mimeType) : this(filename, data)
        {
            MimeType = mimeType;
        }

        /// <summary>
        /// Gets or sets the email attachment's data.
        /// </summary>
        public StorageFile Data { set; get; }

        /// <summary>
        /// Gets or sets the displayed file name for the email attachment. 
        /// </summary>
        /// <value>The displayed file name for the email attachment.</value>
        public string FileName { set; get; }

        /// <summary>
        /// Gets or sets the MIME type of the attachment.
        /// </summary>
        /// <value>The MIME type of the attachment.</value>
        public string MimeType { set; get; }
    }
}