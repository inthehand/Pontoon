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
        public static async Task<StorageFile> GetFileFromPathAsync(string path)
        {
#if __ANDROID__ || __IOS__
            return new StorageFile(path);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return new Storage.StorageFile(await Windows.Storage.StorageFile.GetFileFromPathAsync(path));
#endif
        }

#if __ANDROID__ || __IOS__
        private string _path;
    
        internal StorageFile(string path)
        {
            _path = path;
        }
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private Windows.Storage.StorageFile _file;

        internal StorageFile(Windows.Storage.StorageFile file)
        {
            _file = file;
        }

        [CLSCompliant(false)]
        public static implicit operator Windows.Storage.StorageFile(StorageFile file)
        {
            return file._file;
        }
#endif


        public async Task DeleteAsync()
        {
#if __ANDROID__ || __IOS__
                global::System.IO.File.Delete(Path);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            await _file.DeleteAsync();
#endif
        }

        public FileAttributes Attributes
        {
            get
            {
#if __ANDROID__ || __IOS__
                return FileAttributesHelper.FromIOFileAttributes(global::System.IO.File.GetAttributes(Path));
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return (FileAttributes)((uint)_file.Attributes);
#endif
            }
        }

        public DateTimeOffset DateCreated
        {
            get
            {
#if __ANDROID__ || __IOS__
                var utc = global::System.IO.File.GetCreationTimeUtc(Path);
                var local = global::System.IO.File.GetCreationTime(Path);
                var offset = local - utc;
                return new DateTimeOffset(local, offset);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _file.DateCreated;
#endif
            }
        }

        public string FileType
        {
            get
            {
#if __ANDROID__ || __IOS__
                return global::System.IO.Path.GetExtension(Path);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _file.FileType;
#endif
            }
        }

        public string Name
        {
            get
            {
#if __ANDROID__ || __IOS__
                return global::System.IO.Path.GetFileName(Path);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _file.Name;
#endif
            }
        }

        public string Path
        {
            get
            {
#if __ANDROID__ || __IOS__
                return _path;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _file.Path;
#endif
            }
        }

        public async Task<Stream> OpenStreamForReadAsync()
        {
#if __ANDROID__ || __IOS__
                return global::System.IO.File.OpenRead(Path);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return await _file.OpenStreamForReadAsync();
#endif
        }

        public async Task<Stream> OpenStreamForWriteAsync()
        {
#if __ANDROID__ || __IOS__
                return global::System.IO.File.OpenWrite(Path);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return await _file.OpenStreamForWriteAsync();
#endif
        }
    }
}