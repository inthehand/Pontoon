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
            if(string.IsNullOrEmpty(path))
            {
                return null;
            }

            return new StorageFile(path);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return StorageFile.FromWindowsStorageFile(await Windows.Storage.StorageFile.GetFileFromPathAsync(path));
#else
            return null;
#endif
        }

#if __ANDROID__ || __IOS__
        private string _path;
    
        internal StorageFile(string path)
        {
            _path = path;
        }
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.Storage.StorageFile _file;

        internal StorageFile(Windows.Storage.StorageFile file)
        {
            _file = file;
        }

        public static StorageFile FromWindowsStorageFile(Windows.Storage.StorageFile file)
        {
            return file == null ? null : new Storage.StorageFile(file);
        }
       
        [CLSCompliant(false)]
        public static implicit operator Windows.Storage.StorageFile(StorageFile file)
        {
            return file._file;
        }
#endif

        /// <summary>
        /// Creates a copy of the file in the specified folder.
        /// </summary>
        /// <param name="destinationFolder">The destination folder where the copy of the file is created.</param>
        /// <returns></returns>
        public async Task<StorageFile> CopyAsync(StorageFolder destinationFolder)
        {
#if __ANDROID__ || __IOS__
            return await CopyAsync(destinationFolder, global::System.IO.Path.GetFileName(Path));
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            var file = await _file.CopyAsync((Windows.Storage.StorageFolder)destinationFolder);
            return file == null ? null : new StorageFile(file);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Creates a copy of the file in the specified folder and renames the copy.
        /// </summary>
        /// <param name="destinationFolder">The destination folder where the copy of the file is created.</param>
        /// <param name="desiredNewName">The new name for the copy of the file created in the destinationFolder.</param>
        /// <returns></returns>
        public async Task<StorageFile> CopyAsync(StorageFolder destinationFolder, string desiredNewName)
        {
#if __ANDROID__ || __IOS__
            string newPath = global::System.IO.Path.Combine(destinationFolder.Path, desiredNewName);
            global::System.IO.File.Copy(Path, newPath);
            return new Storage.StorageFile(newPath);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            var file = await _file.CopyAsync((Windows.Storage.StorageFolder)destinationFolder, desiredNewName);
            return file == null ? null : new StorageFile(file);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Deletes the current file.
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAsync()
        {
#if __ANDROID__ || __IOS__
                global::System.IO.File.Delete(Path);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            await _file.DeleteAsync();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the parent folder of the current file.
        /// </summary>
        /// <returns></returns>
        public async Task<StorageFolder> GetParentAsync()
        {
#if __ANDROID__ || __IOS__
            var parent = global::System.IO.Directory.GetParent(Path);
            return parent == null ? null : new StorageFolder(parent.FullName);
#elif WINDOWS_UWP || WINDOWS_APP
            var parent = await _file.GetParentAsync();
            return parent == null ? null : new StorageFolder(parent);
#elif WINDOWS_PHONE_APP || WINDOWS_PHONE
            var parentPath = global::System.IO.Path.GetPathRoot(Path.TrimEnd('\\'));
            Windows.Storage.StorageFolder parent = null;
            if (!string.IsNullOrEmpty(parentPath))
            {
                parent = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(parentPath);
            }
            return parent == null ? null : new StorageFolder(parent);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Moves the current file to the specified folder and renames the file according to the desired name.
        /// </summary>
        /// <param name="destinationFolder">The destination folder where the file is moved.</param>
        /// <returns></returns>
        public async Task MoveAsync(StorageFolder destinationFolder)
        {
#if __ANDROID__ || __IOS__
            await MoveAsync(destinationFolder, global::System.IO.Path.GetFileName(Path));
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            await _file.MoveAsync((Windows.Storage.StorageFolder)destinationFolder);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Moves the current file to the specified folder and renames the file according to the desired name.
        /// </summary>
        /// <param name="destinationFolder">The destination folder where the file is moved.</param>
        /// <param name="desiredNewName">The desired name of the file after it is moved.</param>
        /// <returns></returns>
        public async Task MoveAsync(StorageFolder destinationFolder, string desiredNewName)
        {
#if __ANDROID__ || __IOS__
            string newPath = global::System.IO.Path.Combine(destinationFolder.Path, desiredNewName);
            global::System.IO.File.Move(Path, newPath);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            await _file.MoveAsync((Windows.Storage.StorageFolder)destinationFolder, desiredNewName);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the attributes of a file.
        /// </summary>
        public FileAttributes Attributes
        {
            get
            {
#if __ANDROID__ || __IOS__
                return FileAttributesHelper.FromIOFileAttributes(global::System.IO.File.GetAttributes(Path));
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return (FileAttributes)((uint)_file.Attributes);
#else
                return FileAttributes.Normal;
#endif
            }
        }

        /// <summary>
        /// Gets the date and time when the current file was created. 
        /// </summary>
        public DateTimeOffset DateCreated
        {
            get
            {
#if __ANDROID__ || __IOS__
                var utc = global::System.IO.File.GetCreationTimeUtc(Path);
                var local = global::System.IO.File.GetCreationTime(Path);
                var offset = local - utc;
                return new DateTimeOffset(local, offset);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _file.DateCreated;
#else
                return DateTimeOffset.MinValue;
#endif
            }
        }

        /// <summary>
        /// Gets the type (file name extension) of the file.
        /// </summary>
        public string FileType
        {
            get
            {
#if __ANDROID__ || __IOS__
                return global::System.IO.Path.GetExtension(Path);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _file.FileType;
#else
                return string.Empty;
#endif
            }
        }

        /// <summary>
        /// Gets the name of the file including the file name extension.
        /// </summary>
        public string Name
        {
            get
            {
#if __ANDROID__ || __IOS__
                return global::System.IO.Path.GetFileName(Path);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _file.Name;
#else
                return string.Empty;
#endif
            }
        }

        /// <summary>
        /// Gets the full file-system path of the current file, if the file has a path.
        /// </summary>
        public string Path
        {
            get
            {
#if __ANDROID__ || __IOS__
                return _path;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _file.Path;
#else
                return string.Empty;
#endif
            }
        }

        public async Task<Stream> OpenStreamForReadAsync()
        {
#if __ANDROID__ || __IOS__
                return global::System.IO.File.OpenRead(Path);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return await _file.OpenStreamForReadAsync();
#else
                throw new PlatformNotSupportedException();
#endif
        }

        public async Task<Stream> OpenStreamForWriteAsync()
        {
#if __ANDROID__ || __IOS__
                return global::System.IO.File.OpenWrite(Path);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return await _file.OpenStreamForWriteAsync();
#else
                throw new PlatformNotSupportedException();
#endif
        }
    }
}