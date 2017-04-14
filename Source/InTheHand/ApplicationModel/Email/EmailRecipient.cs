//-----------------------------------------------------------------------
// <copyright file="EmailRecipient.cs" company="In The Hand Ltd">
//     Copyright © 2014-17 In The Hand Ltd. All rights reserved.
//     This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Text;

namespace InTheHand.ApplicationModel.Email
{
    /// <summary>
    /// Represents an email recipient.
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
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public sealed class EmailRecipient
    {
        /// <summary>
        /// Initializes an instance of the EmailRecipient class.
        /// </summary>
        public EmailRecipient() { }

        /// <summary>
        /// Initializes an instance of the EmailRecipient class.
        /// </summary>
        /// <param name="address">The address of the recipient.</param>
        public EmailRecipient(string address) 
        { 
            Address = address; 
        }

        /// <summary>
        /// Initializes an instance of the EmailRecipient class.
        /// </summary>
        /// <param name="address">The address of the recipient.</param>
        /// <param name="name">The name of the recipient.</param>
        public EmailRecipient(string address, string name) : this(address)
        {
            Name = name;
        }

        /// <summary>
        /// Gets or sets the address of the email recipient.
        /// </summary>
        public string Address { set; get; }

        /// <summary>
        /// Gets or sets the name of the email recipient.
        /// </summary>
        public string Name { set; get; }

        internal string ToMailString()
        {
            StringBuilder builder = new StringBuilder();

            if(!string.IsNullOrEmpty(Name))
            {
                builder.Append(Name + " <");
            }

            builder.Append(Address);

            if (!string.IsNullOrEmpty(Name))
            {
                builder.Append(">");
            }

            return builder.ToString();
        }
    }
}