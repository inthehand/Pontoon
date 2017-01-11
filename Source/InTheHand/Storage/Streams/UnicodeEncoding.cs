//-----------------------------------------------------------------------
// <copyright file="UnicodeEncoding.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Storage.Streams
{
    /// <summary>
    /// Specifies the type of character encoding for a stream.
    /// </summary>
    public enum UnicodeEncoding : int
    {
        /// <summary>
        /// The encoding is UTF-8.
        /// </summary>
        Utf8 = 0,

        /// <summary>
        /// The encoding is UTF-16, with the least significant byte first in the two eight-bit bytes.
        /// </summary>
        Utf16LE = 1,

        /// <summary>
        /// The encoding is UTF-16, with the most significant byte first in the two eight-bit bytes.
        /// </summary>
        Utf16BE = 2,
    }
}