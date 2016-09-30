//-----------------------------------------------------------------------
// <copyright file="UnicodeEncodingHelper.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Text;

namespace InTheHand.Storage.Streams
{ 
    internal static class UnicodeEncodingHelper
    {
        public static Encoding EncodingFromUnicodeEncoding(Windows.Storage.Streams.UnicodeEncoding e)
        {
            switch (e)
            {
                case Windows.Storage.Streams.UnicodeEncoding.Utf16LE:
                    return Encoding.Unicode;

                case Windows.Storage.Streams.UnicodeEncoding.Utf16BE:
                    return Encoding.BigEndianUnicode;

                default:
                    return Encoding.UTF8;
            }
        }
    }
}