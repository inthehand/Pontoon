//-----------------------------------------------------------------------
// <copyright file="PathIO.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.Storage.PathIO))]
//#else

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using System.Threading.Tasks;
using InTheHand.Storage.Streams;

namespace InTheHand.Storage
{
    /// <summary>
    /// Provides helper methods for reading and writing a file using the absolute path or URI of the file.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list>
    /// </remarks>
    public static class PathIO
    {
        /// <summary>
        /// Appends lines of text to the file at the specified path or URI.
        /// </summary>
        /// <param name="absolutePath">The path or URI of the file that the lines are appended to.</param>
        /// <param name="lines">The list of text strings to append as lines.</param>
        /// <returns>No object or value is returned when this method completes.</returns>
        public static Task AppendLinesAsync(string absolutePath, IEnumerable<string> lines)
        {
            return AppendLinesAsync(absolutePath, lines, UnicodeEncoding.Utf8);
        }

        /// <summary>
        /// Appends lines of text to the file at the specified path or URI using the specified character encoding.
        /// </summary>
        /// <param name="absolutePath">The path or URI of the file that the lines are appended to.</param>
        /// <param name="lines">The list of text strings to append as lines.</param>
        /// <param name="encoding">The character encoding of the file.</param>
        /// <returns>No object or value is returned when this method completes.</returns>
        public static Task AppendLinesAsync(string absolutePath, IEnumerable<string> lines, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__ || WIN32 || TIZEN
            return Task.Run(() => { File.AppendAllLines(absolutePath, lines,UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)); });
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return Windows.Storage.PathIO.AppendLinesAsync(absolutePath, lines, (Windows.Storage.Streams.UnicodeEncoding)((int)encoding)).AsTask();
#elif WINDOWS_PHONE
            return Task.Run(() =>
            {
                Stream s = File.OpenWrite(absolutePath);
                s.Position = s.Length;

                using (StreamWriter sw = new StreamWriter(s, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)))
                {
                    foreach (string line in lines)
                    {
                        sw.WriteLine(line);
                    }
                }
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Appends text to the file at the specified path or URI.
        /// </summary>
        /// <param name="absolutePath">The path of the file that the text is appended to.</param>
        /// <param name="contents">The text to append.</param>
        /// <returns>No object or value is returned when this method completes.</returns>
        public static Task AppendTextAsync(string absolutePath, string contents)
        {
            return AppendTextAsync(absolutePath, contents, UnicodeEncoding.Utf8);
        }

        /// <summary>
        /// Appends text to the file at the specified path or URI using the specified character encoding.
        /// </summary>
        /// <param name="absolutePath">The path of the file that the text is appended to.</param>
        /// <param name="contents">The text to append.</param>
        /// <param name="encoding">The character encoding of the file.</param>
        /// <returns>No object or value is returned when this method completes.</returns>
        public static Task AppendTextAsync(string absolutePath, string contents, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__ || WIN32 || TIZEN
            return Task.Run(() => { File.AppendAllText(absolutePath, contents, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)); });
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return Windows.Storage.PathIO.AppendTextAsync(absolutePath, contents, (Windows.Storage.Streams.UnicodeEncoding)((int)encoding)).AsTask();
#elif WINDOWS_PHONE
            return Task.Run(() =>
            {
                Stream s = File.OpenWrite(absolutePath);
                s.Position = s.Length;

                using (StreamWriter sw = new StreamWriter(s, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)))
                    sw.Write(contents);
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Reads the contents of the file at the specified path or URI and returns lines of text.
        /// </summary>
        /// <param name="absolutePath">The path of the file to read.</param>
        /// <returns>When this method completes successfully, it returns the contents of the file as a list (type IVector) of lines of text.
        /// Each line of text in the list is represented by a String object.</returns>
        public static Task<IList<string>> ReadLinesAsync(string absolutePath)
        {
            return ReadLinesAsync(absolutePath, UnicodeEncoding.Utf8);
        }

        /// <summary>
        /// Reads the contents of the file at the specified path or URI using the specified character encoding and returns lines of text.
        /// </summary>
        /// <param name="absolutePath">The path of the file to read.</param>
        /// <param name="encoding">The character encoding of the file.</param>
        /// <returns>When this method completes successfully, it returns the contents of the file as a list (type IVector) of lines of text.
        /// Each line of text in the list is represented by a String object.</returns>
        public static Task<IList<string>> ReadLinesAsync(string absolutePath, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__ || WIN32 || TIZEN
            return Task.Run<IList<string>>(() => { return File.ReadAllLines(absolutePath, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)); });
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return Windows.Storage.PathIO.ReadLinesAsync(absolutePath, (Windows.Storage.Streams.UnicodeEncoding)((int)encoding)).AsTask();
#elif WINDOWS_PHONE
            return Task.Run<IList<string>>(() =>
            {
                String line;
                List<String> lines = new List<String>();

                Stream s = File.OpenRead(absolutePath);
                using (StreamReader sr = new StreamReader(s, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)))
                    while ((line = sr.ReadLine()) != null)
                        lines.Add(line);

                return lines;
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Reads the contents of the file at the specified path or URI and returns text.
        /// </summary>
        /// <param name="absolutePath">The path of the file to read.</param>
        /// <returns>When this method completes successfully, it returns the contents of the file as a text string.</returns>
        public static Task<string> ReadTextAsync(string absolutePath)
        {
            return ReadTextAsync(absolutePath, UnicodeEncoding.Utf8);
        }

        /// <summary>
        /// Reads the contents of the file at the specified path or URI using the specified character encoding and returns text.
        /// </summary>
        /// <param name="absolutePath">The path of the file to read.</param>
        /// <param name="encoding">The character encoding of the file.</param>
        /// <returns>When this method completes successfully, it returns the contents of the file as a text string.</returns>
        public static Task<string> ReadTextAsync(string absolutePath, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__ || WIN32 || TIZEN
            return Task.Run<string>(() => { return File.ReadAllText(absolutePath, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)); });
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return Windows.Storage.PathIO.ReadTextAsync(absolutePath, (Windows.Storage.Streams.UnicodeEncoding)((int)encoding)).AsTask();
#elif WINDOWS_PHONE
            Stream s = File.OpenRead(absolutePath);
            using (StreamReader sr = new StreamReader(s, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)))
                return sr.ReadToEndAsync();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Writes lines of text to the file at the specified path or URI.
        /// </summary>
        /// <param name="absolutePath">The path of the file that the lines are written to.</param>
        /// <param name="lines">The list of text strings to append as lines.</param>
        /// <returns>No object or value is returned when this method completes.</returns>
        public static Task WriteLinesAsync(string absolutePath, IEnumerable<string> lines)
        {
            return WriteLinesAsync(absolutePath, lines, UnicodeEncoding.Utf8);
        }

        /// <summary>
        /// Writes lines of text to the file at the specified path or URI using the specified character encoding.
        /// </summary>
        /// <param name="absolutePath">The path of the file that the lines are written to.</param>
        /// <param name="lines">The list of text strings to append as lines.</param>
        /// <param name="encoding">The character encoding of the file.</param>
        /// <returns>No object or value is returned when this method completes.</returns>
        public static Task WriteLinesAsync(string absolutePath, IEnumerable<string> lines, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__ || WIN32 || TIZEN
            return Task.Run(() => { File.WriteAllLines(absolutePath, lines, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)); });
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return Windows.Storage.PathIO.WriteLinesAsync(absolutePath, lines, (Windows.Storage.Streams.UnicodeEncoding)((int)encoding)).AsTask();
#elif WINDOWS_PHONE
            return Task.Run(() =>
            {
                Stream s = File.OpenWrite(absolutePath);
                using (StreamWriter sw = new StreamWriter(s, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)))
                {
                    foreach (string line in lines)
                    {
                        sw.WriteLine(line);
                    }
                }
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Writes text to the file at the specified path or URI.
        /// </summary>
        /// <param name="absolutePath">The path of the file that the text is written to.</param>
        /// <param name="contents">The text to write.</param>
        /// <returns>No object or value is returned when this method completes.</returns>
        public static Task WriteTextAsync(string absolutePath, string contents)
        {
            return WriteTextAsync(absolutePath, contents, UnicodeEncoding.Utf8);
        }

        /// <summary>
        /// Writes text to the file at the specified path or URI using the specified character encoding.
        /// </summary>
        /// <param name="absolutePath">The path of the file that the text is written to.</param>
        /// <param name="contents">The text to write.</param>
        /// <param name="encoding">The character encoding of the file.</param>
        /// <returns>No object or value is returned when this method completes.</returns>
        public static Task WriteTextAsync(string absolutePath, string contents, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__ || WIN32 || TIZEN
            return Task.Run(() => { File.WriteAllText(absolutePath, contents, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)); });
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return Windows.Storage.PathIO.WriteTextAsync(absolutePath, contents, (Windows.Storage.Streams.UnicodeEncoding)((int)encoding)).AsTask();
#elif WINDOWS_PHONE
            return Task.Run(() =>
            {
                Stream s = File.OpenWrite(absolutePath);
                using (StreamWriter sw = new StreamWriter(s, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)))
                    sw.Write(contents);
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }
    }
}
//#endif