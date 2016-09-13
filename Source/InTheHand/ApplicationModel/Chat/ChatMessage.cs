//-----------------------------------------------------------------------
// <copyright file="ChatMessage.cs" company="In The Hand Ltd">
//     Copyright © 2014-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.ApplicationModel.Chat.ChatMessage))]
#else

using System.Collections.Generic;

namespace Windows.ApplicationModel.Chat
{
    /// <summary>
    /// Represents a chat message.
    /// </summary>
    public sealed class ChatMessage
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ChatMessage"/> class.
        /// </summary>
        public ChatMessage()
        {

        }

        /// <summary>
        /// Gets or sets the body of the chat message.
        /// </summary>
        public string Body { get; set; }

        private List<string> _recipients = new List<string>();
        /// <summary>
        /// Gets the list of recipients of the message.
        /// </summary>
        public IList<string> Recipients
        {
            get
            {
                return _recipients;
            }
        }
    }
}
#endif