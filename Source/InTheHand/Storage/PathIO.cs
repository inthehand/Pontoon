//-----------------------------------------------------------------------
// <copyright file="PathIO.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
[assembly: TypeForwardedTo(typeof(Windows.Storage.PathIO))]
#else

namespace Windows.Storage
{
    [CLSCompliant(false)]
    /// <summary>
    /// Provides helper methods for reading and writing a file using the absolute path or URI of the file.
    /// </summary>
    public static class PathIO
    {
        /// <summary>
        /// Appends lines of text to the file at the specified path or URI.
        /// </summary>
        /// <param name="absolutePath">The path or URI of the file that the lines are appended to.</param>
        /// <param name="lines">The list of text strings to append as lines.</param>
        /// <returns>No object or value is returned when this method completes.</returns>
        public static IAsyncAction AppendLinesAsync(string absolutePath, IEnumerable<string> lines)
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
        public static IAsyncAction AppendLinesAsync(string absolutePath, IEnumerable<string> lines, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__
            return Task.Run(() => { File.AppendAllLines(absolutePath, lines,UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)); }).AsAsyncAction();
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
            }).AsAsyncAction();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Appends text to the file at the specified path or URI.
        /// </summary>
        /// <param name="absolutePath">The path of the file that the text is appended to.</param>
        /// <param name="contents">The text to append.</param>
        /// <param name="encoding">The character encoding of the file.</param>
        /// <returns>No object or value is returned when this method completes.</returns>
        public static IAsyncAction AppendTextAsync(string absolutePath, string contents)
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
        public static IAsyncAction AppendTextAsync(string absolutePath, string contents, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__
            return Task.Run(() => { File.AppendAllText(absolutePath, contents, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)); }).AsAsyncAction();
#elif WINDOWS_PHONE
            return Task.Run(() =>
            {
                Stream s = File.OpenWrite(absolutePath);
                s.Position = s.Length;

                using (StreamWriter sw = new StreamWriter(s, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)))
                    sw.Write(contents);
            }).AsAsyncAction();
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
        public static IAsyncOperation<IList> ReadLinesAsync(string absolutePath)
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
        public static IAsyncOperation<IList> ReadLinesAsync(string absolutePath, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__
            return Task.Run<IList>(() => { return File.ReadAllLines(absolutePath, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)); }).AsAsyncOperation<IList>();
#elif WINDOWS_PHONE
            return Task.Run<IList>(() =>
            {
                String line;
                List<String> lines = new List<String>();

                Stream s = File.OpenRead(absolutePath);
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
        /// Reads the contents of the file at the specified path or URI and returns text.
        /// </summary>
        /// <param name="absolutePath">The path of the file to read.</param>
        /// <returns>When this method completes successfully, it returns the contents of the file as a text string.</returns>
        public static IAsyncOperation<string> ReadTextAsync(string absolutePath)
        {
            return ReadTextAsync(absolutePath, UnicodeEncoding.Utf8);
        }

        /// <summary>
        /// Reads the contents of the file at the specified path or URI using the specified character encoding and returns text.
        /// </summary>
        /// <param name="absolutePath">The path of the file to read.</param>
        /// <param name="encoding">The character encoding of the file.</param>
        /// <returns>When this method completes successfully, it returns the contents of the file as a text string.</returns>
        public static IAsyncOperation<string> ReadTextAsync(string absolutePath, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__
            return Task.Run<string>(() => { return File.ReadAllText(absolutePath, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)); }).AsAsyncOperation<string>();
#elif WINDOWS_PHONE
            Stream s = File.OpenRead(absolutePath);
            using (StreamReader sr = new StreamReader(s, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)))
                return sr.ReadToEndAsync().AsAsyncOperation<string>();
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
        public static IAsyncAction WriteLinesAsync(string absolutePath, IEnumerable<string> lines)
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
        public static IAsyncAction WriteLinesAsync(string absolutePath, IEnumerable<string> lines, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__
            return Task.Run(() => { File.WriteAllLines(absolutePath, lines, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)); }).AsAsyncAction();
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
            }).AsAsyncAction();
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
        public static IAsyncAction WriteTextAsync(string absolutePath, string contents)
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
        public static IAsyncAction WriteTextAsync(string absolutePath, string contents, UnicodeEncoding encoding)
        {
#if __ANDROID__ || __IOS__
            return Task.Run(() => { File.WriteAllText(absolutePath, contents, UnicodeEncodingHelper.EncodingFromUnicodeEncoding(encoding)); }).AsAsyncAction();
#elif WINDOWS_PHONE
            return Task.Run(() =>
            {
                Stream s = File.OpenWrite(absolutePath);
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