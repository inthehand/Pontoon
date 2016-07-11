//-----------------------------------------------------------------------
// <copyright file="StorageFolder.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace InTheHand.Storage
{
    /// <summary>
    /// Manages folders and their contents and provides information about them.
    /// </summary>
    public sealed class StorageFolder
    {
        /// <summary>
        /// Gets a StorageFile object to represent the file at the specified path.
        /// </summary>
        /// <param name="path">The path of the file to get a StorageFile to represent.
        /// If your path uses slashes, make sure you use backslashes(\).
        /// Forward slashes(/) are not accepted by this method.</param>
        /// <returns>When this method completes, it returns the file as a StorageFile.</returns>
        public static async Task<StorageFolder> GetFolderFromPathAsync(string path)
        {
#if __ANDROID__ || __IOS__
            return new StorageFolder(path);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return new Storage.StorageFolder(await Windows.Storage.StorageFolder.GetFolderFromPathAsync(path));
#else
            throw new PlatformNotSupportedException();
#endif
        }

#if __ANDROID__ || __IOS__
        private string _path;

        internal StorageFolder(string path)
        {
            _path = path;
        }
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.Storage.StorageFolder _folder;

        internal StorageFolder(Windows.Storage.StorageFolder folder)
        {
            _folder = folder;
        }

        [CLSCompliant(false)]
        public static implicit operator Windows.Storage.StorageFolder(StorageFolder folder)
        {
            return folder._folder;
        }
#endif

        /// <summary>
        /// Creates a new file with the specified name in the current folder.
        /// </summary>
        /// <param name="desiredName">The name of the new file to create in the current folder.</param>
        /// <returns>When this method completes, it returns a StorageFile that represents the new file.</returns>
        public async Task<StorageFile> CreateFileAsync(string desiredName)
        {
#if __ANDROID__ || __IOS__
            string filepath = global::System.IO.Path.Combine(Path, desiredName);

            if (global::System.IO.File.Exists(filepath))
            {
                throw new IOException();
            }

            File.Create(filepath).Close();

            return new Storage.StorageFile(filepath);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            var file = await _folder.CreateFileAsync(desiredName);
            return file == null ? null : new StorageFile(file);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Creates a new subfolder with the specified name in the current folder.
        /// </summary>
        /// <param name="desiredName">The name of the new subfolder to create in the current folder.</param>
        /// <returns>When this method completes, it returns a StorageFolder that represents the new subfolder.</returns>
        public async Task<StorageFolder> CreateFolderAsync(string desiredName)
        {
#if __ANDROID__ || __IOS__
            string newpath = global::System.IO.Path.Combine(Path, desiredName);

            if (global::System.IO.Directory.Exists(newpath))
            {
                throw new IOException();
            }

            Directory.CreateDirectory(newpath);

            return new Storage.StorageFolder(newpath);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            var folder = await _folder.CreateFolderAsync(desiredName);
            return folder == null ? null : new StorageFolder(folder);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Deletes the current folder.
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAsync()
        {
#if __ANDROID__ || __IOS__
            if (!Directory.Exists(Path))
            {
                throw new FileNotFoundException();
            }

            global::System.IO.Directory.Delete(Path);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            await _folder.DeleteAsync();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the file with the specified name from the current folder. 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public async Task<StorageFile> GetFileAsync(string filename)
        {
#if __ANDROID__ || __IOS__
            string filepath = global::System.IO.Path.Combine(Path, filename);

            if (!global::System.IO.File.Exists(filepath))
            {
                throw new FileNotFoundException();
            }

            return new StorageFile(filepath);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            var file = await _folder.GetFileAsync(filename);
            return file == null ? null : new Storage.StorageFile(file);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the files in the current folder.
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<StorageFile>> GetFilesAsync()
        {
            List<StorageFile> files = new List<StorageFile>();
#if __ANDROID__ || __IOS__
            foreach (string filename in global::System.IO.Directory.GetFiles(Path))
            {
                files.Add(new StorageFile(global::System.IO.Path.Combine(Path, filename)));
            }
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            foreach(Windows.Storage.StorageFile file in await _folder.GetFilesAsync())
            {
                files.Add(new StorageFile(file));
            }
#endif

            return files;
        }

        /// <summary>
        /// Gets the parent folder of the current file.
        /// </summary>
        /// <returns>When this method completes, it returns the parent folder as a StorageFolder.</returns>
        public async Task<StorageFolder> GetParentAsync()
        {
#if __ANDROID__ || __IOS__
            var parent = global::System.IO.Directory.GetParent(Path);
            return parent == null ? null : new StorageFolder(parent.FullName);
#elif WINDOWS_UWP || WINDOWS_APP
            var parent = await _folder.GetParentAsync();
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
        /// Gets the attributes of the current folder.
        /// </summary>
        public FileAttributes Attributes
        {
            get
            {
#if __ANDROID__ || __IOS__
                return FileAttributesHelper.FromIOFileAttributes(global::System.IO.File.GetAttributes(Path));
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return (FileAttributes)((uint)_folder.Attributes);
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets the date and time that the current folder was created. 
        /// </summary>
        public DateTimeOffset DateCreated
        {
            get
            {
#if __ANDROID__ || __IOS__
                var utc = global::System.IO.Directory.GetCreationTimeUtc(Path);
                var local = global::System.IO.Directory.GetCreationTime(Path);
                var offset = local - utc;
                return new DateTimeOffset(local, offset);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _folder.DateCreated;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets the name of the current folder. 
        /// </summary>
        public string Name
        {
            get
            {
#if __ANDROID__ || __IOS__
                return global::System.IO.Path.GetDirectoryName(Path);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _folder.Name;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets the full path of the current folder in the file system, if the path is available.
        /// </summary>
        public string Path
        {
            get
            {
#if __ANDROID__ || __IOS__
                return _path;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _folder.Path;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }
    }
}