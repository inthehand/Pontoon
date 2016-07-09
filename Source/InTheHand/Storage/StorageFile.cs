//-----------------------------------------------------------------------
// <copyright file="StorageFile.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Storage;
using System;
using System.IO;
using System.Threading.Tasks;

namespace InTheHand.Storage
{
    /// <summary>
    /// Represents a file.
    /// Provides information about the file and its content, and ways to manipulate them.
    /// </summary>
    public sealed class StorageFile
    {
        /// <summary>
        /// Gets a StorageFile object to represent the file at the specified path.
        /// </summary>
        /// <param name="path">The path of the file to get a StorageFile to represent.
        /// If your path uses slashes, make sure you use backslashes(\).
        /// Forward slashes(/) are not accepted by this method.</param>
        /// <returns>When this method completes, it returns the file as a StorageFile.</returns>
        public static Task<StorageFile> GetFileFromPathAsync(string path)
        {
            return Task.Run<StorageFile>(() =>
            {
                return new StorageFile(path);
            });
        }

        private string _path;

        private StorageFile(string path)
        {
            _path = path;
        }

        public Task DeleteAsync()
        {
#if __ANDROID__ || __IOS__
            return Task.Run(() =>
            {
                global::System.IO.File.Delete(_path);
            });
#endif
        }

        public DateTimeOffset DateCreated
        {
            get
            {
                var utc = global::System.IO.File.GetCreationTimeUtc(_path);
                var local = global::System.IO.File.GetCreationTime(_path);
                var offset = local - utc;
                return new DateTimeOffset(local, offset);
            }
        }

        public string FileType
        {
            get
            {
                return global::System.IO.Path.GetExtension(_path);
            }
        }

        public string Name
        {
            get
            {
                return global::System.IO.Path.GetFileName(_path);
            }
        }

        public string Path
        {
            get
            {
                return _path;
            }
        }

        public Task<Stream> OpenStreamForReadAsync()
        {
            return Task.Run<Stream>(() =>
            {
                return global::System.IO.File.OpenRead(_path);
            });
        }

        public Task<Stream> OpenStreamForWriteAsync()
        {
            return Task.Run<Stream>(() =>
            {
                return global::System.IO.File.OpenWrite(_path);
            });
        }
    }
}