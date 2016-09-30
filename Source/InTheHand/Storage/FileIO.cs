//-----------------------------------------------------------------------
// <copyright file="FileIO.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Storage.FileIO))]
#else

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;
#if WINDOWS_PHONE
using InTheHand.Storage.Streams;
#endif

namespace Windows.Storage
{
    [CLSCompliant(false)]
    /// <summary>
    /// Provides helper methods for reading and writing files that are represented by objects of type <see cref="IStorageFile"/>.
    /// </summary>
    public static class FileIO
    {
        /// <summary>
        /// Appends lines of text to the specified file.
        /// </summary>
        /// <param name="file">The file that the lines are appended to.</param>
        /// <param name="lines">The list of text strings to append as lines.</param>
        /// <returns>No object or value is returned when this method completes.</returns>
        public static IAsyncAction AppendLinesAsync(IStorageFile file, IEnumerable<string> lines)
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
        public static IAsyncAction AppendLinesAsync(IStorageFile file, IEnumerable<string> lines, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__ || WIN32
            return PathIO.AppendLinesAsync(file.Path, lines, encoding);
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
            }).AsAsyncAction();
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
        public static IAsyncAction AppendTextAsync(IStorageFile file, string contents)
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
        public static IAsyncAction AppendTextAsync(IStorageFile file, string contents, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__ || WIN32
            return PathIO.AppendTextAsync(file.Path, contents, encoding);
#elif WINDOWS_PHONE
            return Task.Run(async () =>
            {
                Stream s = await file.OpenStreamForWriteAsync();
                s.Position = s.Length;

                using (StreamWriter sw = new StreamWriter(s, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)))
                    sw.Write(contents);
            }).AsAsyncAction();
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
        public static IAsyncOperation<IList> ReadLinesAsync(IStorageFile file)
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
        public static IAsyncOperation<IList> ReadLinesAsync(IStorageFile file, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__ || WIN32
            return PathIO.ReadLinesAsync(file.Path, encoding);
#elif WINDOWS_PHONE
            return Task.Run<IList>(async () =>
            {
                String line;
                List<String> lines = new List<String>();

                Stream s = await file.OpenStreamForReadAsync();
                using (StreamReader sr = new StreamReader(s, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)))
                    while ((line = sr.ReadLine()) != null)
                        lines.Add(line);

                return lines.ToArray();
            }).AsAsyncOperation<IList>();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Reads the contents of the specified file and returns text.
        /// </summary>
        /// <param name="file">The file to read.</param>
        /// <param name="encoding">The character encoding of the file.</param>
        /// <returns>When this method completes successfully, it returns the contents of the file as a text string.</returns>
        public static IAsyncOperation<string> ReadTextAsync(IStorageFile file)
        {
            return ReadTextAsync(file, UnicodeEncoding.Utf8);
        }

        /// <summary>
        /// Reads the contents of the specified file using the specified character encoding and returns text.
        /// </summary>
        /// <param name="file">The file to read.</param>
        /// <param name="encoding">The character encoding of the file.</param>
        /// <returns>When this method completes successfully, it returns the contents of the file as a text string.</returns>
        public static IAsyncOperation<string> ReadTextAsync(IStorageFile file, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__ || WIN32
            return PathIO.ReadTextAsync(file.Path, encoding);
#elif WINDOWS_PHONE
            return Task.Run<string>(async () =>
            {
                Stream s = await file.OpenStreamForReadAsync();
                using (StreamReader sr = new StreamReader(s, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)))
                    return await sr.ReadToEndAsync();
            }).AsAsyncOperation<string>();
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
        public static IAsyncAction WriteLinesAsync(IStorageFile file, IEnumerable<string> lines)
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
        public static IAsyncAction WriteLinesAsync(IStorageFile file, IEnumerable<string> lines, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__ || WIN32
            return PathIO.WriteLinesAsync(file.Path, lines, encoding);
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
            }).AsAsyncAction();
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
        public static IAsyncAction WriteTextAsync(IStorageFile file, string contents)
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
        public static IAsyncAction WriteTextAsync(IStorageFile file, string contents, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__ || WIN32
            return PathIO.WriteTextAsync(file.Path, contents, encoding);
#elif WINDOWS_PHONE
            return Task.Run(async () =>
            {
                Stream s = await file.OpenStreamForWriteAsync();
                using (StreamWriter sw = new StreamWriter(s, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)))
                    sw.Write(contents);
            }).AsAsyncAction();
#else
            throw new PlatformNotSupportedException();
#endif
        }
    }
}
#endif