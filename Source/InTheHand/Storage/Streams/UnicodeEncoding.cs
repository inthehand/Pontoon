//-----------------------------------------------------------------------
// <copyright file="UnicodeEncoding.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Text;

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
[assembly: TypeForwardedTo(typeof(Windows.Storage.Streams.UnicodeEncoding))]
#endif

namespace Windows.Storage.Streams
{
    #if !WINDOWS_UWP && !WINDOWS_APP && !WINDOWS_PHONE_APP && !WINDOWS_PHONE
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
#endif

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE
    internal static class UnicodeEncodingHelper
    {
        public static Encoding EncodingFromUnicodeEncoding(UnicodeEncoding e)
        {
            switch (e)
            {
                case UnicodeEncoding.Utf16LE:
                    return Encoding.Unicode;

                case UnicodeEncoding.Utf16BE:
                    return Encoding.BigEndianUnicode;

                default:
                    return Encoding.UTF8;
            }
        }
    }
#endif
}