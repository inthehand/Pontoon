//-----------------------------------------------------------------------
// <copyright file="FileIO.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.Storage.FileIO))]
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
    /// Provides helper methods for reading and writing files that are represented by objects of type <see cref="IStorageFile"/>.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list>
    /// </remarks>
    public static class FileIO
    {
        /// <summary>
        /// Appends lines of text to the specified file.
        /// </summary>
        /// <param name="file">The file that the lines are appended to.</param>
        /// <param name="lines">The list of text strings to append as lines.</param>
        /// <returns>No object or value is returned when this method completes.</returns>
        public static Task AppendLinesAsync(IStorageFile file, IEnumerable<string> lines)
        {
            return AppendLinesAsync(file, lines);
        }

        /// <summary>
        /// Appends lines of text to the specified file using the specified character encoding.
        /// </summary>
        /// <param name="file">The file that the lines are appended to.</param>
        /// <param name="lines">The list of text strings to append as lines.</param>
        /// <param name="encoding">The character encoding of the file.</param>
        /// <returns>No object or value is returned when this method completes.</returns>
        public static Task AppendLinesAsync(IStorageFile file, IEnumerable<string> lines, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__ || __TVOS__ || WIN32 || TIZEN
            return PathIO.AppendLinesAsync(file.Path, lines, encoding);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return Windows.Storage.FileIO.AppendLinesAsync((Windows.Storage.StorageFile)((StorageFile)file), lines, (Windows.Storage.Streams.UnicodeEncoding)((int)encoding)).AsTask();
#elif WINDOWS_PHONE
            return Task.Run(async () =>
            {
                Stream s = await file.OpenStreamForWriteAsync();
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
        /// Appends text to the specified file.
        /// </summary>
        /// <param name="file">The file that the text is appended to.</param>
        /// <param name="contents">The text to append.</param>
        /// <returns>No object or value is returned when this method completes.</returns>
        public static Task AppendTextAsync(IStorageFile file, string contents)
        {
            return AppendTextAsync(file, contents, UnicodeEncoding.Utf8);
        }

        /// <summary>
        /// Appends text to the specified file using the specified character encoding.
        /// </summary>
        /// <param name="file">The file that the text is appended to.</param>
        /// <param name="contents">The text to append.</param>
        /// <param name="encoding">The character encoding of the file.</param>
        /// <returns>No object or value is returned when this method completes.</returns>
        public static Task AppendTextAsync(IStorageFile file, string contents, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__ || __TVOS__ || WIN32 || TIZEN
            return PathIO.AppendTextAsync(file.Path, contents, encoding);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return Windows.Storage.FileIO.AppendTextAsync((Windows.Storage.StorageFile)((StorageFile)file), contents, (Windows.Storage.Streams.UnicodeEncoding)((int)encoding)).AsTask();
#elif WINDOWS_PHONE
            return Task.Run(async () =>
            {
                Stream s = await file.OpenStreamForWriteAsync();
                s.Position = s.Length;

                using (StreamWriter sw = new StreamWriter(s, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)))
                    sw.Write(contents);
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Reads the contents of the specified file and returns lines of text.
        /// </summary>
        /// <param name="file">The file to read.</param>
        /// <returns>When this method completes successfully, it returns the contents of the file as a list (type IVector) of lines of text.
        /// Each line of text in the list is represented by a String object.</returns>
        public static Task<IList<string>> ReadLinesAsync(IStorageFile file)
        {
            return ReadLinesAsync(file, UnicodeEncoding.Utf8);
        }

        /// <summary>
        /// Reads the contents of the specified file using the specified character encoding and returns lines of text.
        /// </summary>
        /// <param name="file">The file to read.</param>
        /// <param name="encoding">The character encoding of the file.</param>
        /// <returns>When this method completes successfully, it returns the contents of the file as a list (type IVector) of lines of text.
        /// Each line of text in the list is represented by a String object.</returns>
        public static Task<IList<string>> ReadLinesAsync(IStorageFile file, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__ || __TVOS__ || WIN32 || TIZEN
            return PathIO.ReadLinesAsync(file.Path, encoding);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return Windows.Storage.FileIO.ReadLinesAsync((Windows.Storage.StorageFile)((StorageFile)file), (Windows.Storage.Streams.UnicodeEncoding)((int)encoding)).AsTask();
#elif WINDOWS_PHONE
            return Task.Run<IList<string>>(async () =>
            {
                String line;
                List<String> lines = new List<String>();

                Stream s = await file.OpenStreamForReadAsync();
                using (StreamReader sr = new StreamReader(s, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)))
                    while ((line = sr.ReadLine()) != null)
                        lines.Add(line);

                return lines.ToArray();
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Reads the contents of the specified file and returns text.
        /// </summary>
        /// <param name="file">The file to read.</param>
        /// <returns>When this method completes successfully, it returns the contents of the file as a text string.</returns>
        public static Task<string> ReadTextAsync(IStorageFile file)
        {
            return ReadTextAsync(file, UnicodeEncoding.Utf8);
        }

        /// <summary>
        /// Reads the contents of the specified file using the specified character encoding and returns text.
        /// </summary>
        /// <param name="file">The file to read.</param>
        /// <param name="encoding">The character encoding of the file.</param>
        /// <returns>When this method completes successfully, it returns the contents of the file as a text string.</returns>
        public static Task<string> ReadTextAsync(IStorageFile file, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__ || __TVOS__ || WIN32 || TIZEN
            return PathIO.ReadTextAsync(file.Path, encoding);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return Windows.Storage.FileIO.ReadTextAsync((Windows.Storage.StorageFile)((StorageFile)file), (Windows.Storage.Streams.UnicodeEncoding)((int)encoding)).AsTask();
#elif WINDOWS_PHONE
            return Task.Run<string>(async () =>
            {
                Stream s = await ((StorageFile)file).OpenStreamForReadAsync();
                using (StreamReader sr = new StreamReader(s, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)))
                    return await sr.ReadToEndAsync();
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Writes lines of text to the specified file.
        /// </summary>
        /// <param name="file">The file that the lines are written to.</param>
        /// <param name="lines">The list of text strings to append as lines.</param>
        /// <returns>No object or value is returned when this method completes.</returns>
        public static Task WriteLinesAsync(IStorageFile file, IEnumerable<string> lines)
        {
            return WriteLinesAsync(file, lines, UnicodeEncoding.Utf8);
        }

        /// <summary>
        /// Writes lines of text to the specified file using the specified character encoding.
        /// </summary>
        /// <param name="file">The file that the lines are written to.</param>
        /// <param name="lines">The list of text strings to append as lines.</param>
        /// <param name="encoding">The character encoding of the file.</param>
        /// <returns>No object or value is returned when this method completes.</returns>
        public static Task WriteLinesAsync(IStorageFile file, IEnumerable<string> lines, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__ || __TVOS__ || WIN32 || TIZEN
            return PathIO.WriteLinesAsync(file.Path, lines, encoding);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return Windows.Storage.FileIO.WriteLinesAsync((Windows.Storage.StorageFile)((StorageFile)file), lines, (Windows.Storage.Streams.UnicodeEncoding)((int)encoding)).AsTask();
#elif WINDOWS_PHONE
            return Task.Run(async () =>
            {
                Stream s = await file.OpenStreamForWriteAsync();
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
        /// Writes text to the specified file.
        /// </summary>
        /// <param name="file">The file that the text is written to.</param>
        /// <param name="contents">The text to write.</param>
        /// <returns>No object or value is returned when this method completes.</returns>
        public static Task WriteTextAsync(IStorageFile file, string contents)
        {
            return WriteTextAsync(file, contents, UnicodeEncoding.Utf8);
        }

        /// <summary>
        /// Writes text to the specified file using the specified character encoding.
        /// </summary>
        /// <param name="file">The file that the text is written to.</param>
        /// <param name="contents">The text to write.</param>
        /// <param name="encoding">The character encoding of the file.</param>
        /// <returns>No object or value is returned when this method completes.</returns>
        public static Task WriteTextAsync(IStorageFile file, string contents, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__ || __TVOS__ || WIN32 || TIZEN
            return PathIO.WriteTextAsync(file.Path, contents, encoding);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return Windows.Storage.FileIO.WriteTextAsync((Windows.Storage.StorageFile)((StorageFile)file), contents, (Windows.Storage.Streams.UnicodeEncoding)((int)encoding)).AsTask();
#elif WINDOWS_PHONE
            return Task.Run(async () =>
            {
                Stream s = await file.OpenStreamForWriteAsync();
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