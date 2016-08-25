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
    /// Manipulates folders and their contents, and provides information about them.
    /// </summary>
    public interface IStorageFolder : IStorageItem
    {
        Task<StorageFile> CreateFileAsync(string desiredName);
        Task<StorageFile> CreateFileAsync(string desiredName, CreationCollisionOption options);
        Task<StorageFolder> CreateFolderAsync(string desiredName);
        Task<StorageFolder> CreateFolderAsync(string desiredName, CreationCollisionOption options);
        Task<StorageFile> GetFileAsync(string filename);
        Task<IReadOnlyList<StorageFile>> GetFilesAsync();
        Task<StorageFolder> GetFolderAsync(string name);
        Task<IReadOnlyList<StorageFolder>> GetFoldersAsync();
    }

    /// <summary>
    /// Manages folders and their contents and provides information about them.
    /// </summary>
    public sealed class StorageFolder : IStorageFolder
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
            if(string.IsNullOrEmpty(path))
            {
                return null;
            }

            return new StorageFolder(path);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Storage.StorageFolder.FromWindowsStorageFolder(await Windows.Storage.StorageFolder.GetFolderFromPathAsync(path));
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

        public static StorageFolder FromWindowsStorageFolder(Windows.Storage.StorageFolder folder)
        {
            return folder == null ? null : new Storage.StorageFolder(folder);
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
        public Task<StorageFile> CreateFileAsync(string desiredName)
        {
            return CreateFileAsync(desiredName, CreationCollisionOption.FailIfExists);
        }

        /// <summary>
        /// Creates a new file with the specified name in the current folder.
        /// </summary>
        /// <param name="desiredName">The name of the new file to create in the current folder.</param>
        /// <param name="options">One of the enumeration values that determines how to handle the collision if a file with the specified desiredName already exists in the current folder.</param>
        /// <returns>When this method completes, it returns a StorageFile that represents the new file.</returns>
        public async Task<StorageFile> CreateFileAsync(string desiredName, CreationCollisionOption options)
        {
#if __ANDROID__ || __IOS__
            string filepath = global::System.IO.Path.Combine(Path, desiredName);

            if (global::System.IO.File.Exists(filepath))
            {
                switch (options)
                {
                    case CreationCollisionOption.OpenIfExists:
                        return new Storage.StorageFile(filepath);

                    case CreationCollisionOption.ReplaceExisting:
                        File.Delete(filepath);
                        break;

                    case CreationCollisionOption.GenerateUniqueName:
                        for(int i = 1; i < 100; i++)
                        {
                            string newPath = string.Format(filepath.Substring(0, filepath.LastIndexOf('.')) + " ({0})" + filepath.Substring(filepath.LastIndexOf('.')), i);
                            if (!File.Exists(newPath))
                            {
                                filepath = newPath;
                                break;
                            }
                        }
                        break;

                    default:
                        throw new IOException();
                }
            }

            File.Create(filepath).Close();

            return new Storage.StorageFile(filepath);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            var file = await _folder.CreateFileAsync(desiredName, (Windows.Storage.CreationCollisionOption)((int)options));
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
        public Task<StorageFolder> CreateFolderAsync(string desiredName)
        {
            return CreateFolderAsync(desiredName);
        }

        /// <summary>
        /// Creates a new subfolder with the specified name in the current folder.
        /// This method also specifies what to do if a subfolder with the same name already exists in the current folder. 
        /// </summary>
        /// <param name="desiredName">The name of the new subfolder to create in the current folder.</param>
        /// <param name="options">One of the enumeration values that determines how to handle the collision if a subfolder with the specified desiredName already exists in the current folder.</param>
        /// <returns>When this method completes, it returns a StorageFolder that represents the new subfolder.</returns>
        public async Task<StorageFolder> CreateFolderAsync(string desiredName, CreationCollisionOption options)
        {
#if __ANDROID__ || __IOS__
            string newpath = global::System.IO.Path.Combine(Path, desiredName);

            if (global::System.IO.Directory.Exists(newpath))
            {
                switch (options)
                {
                    case CreationCollisionOption.OpenIfExists:
                        return new Storage.StorageFolder(newpath);

                    case CreationCollisionOption.ReplaceExisting:
                        Directory.Delete(newpath);
                        break;

                    case CreationCollisionOption.GenerateUniqueName:
                        for (int i = 1; i < 100; i++)
                        {
                            string uniquePath = string.Format(newpath.Substring(0, newpath.LastIndexOf('.')) + " ({0})" + newpath.Substring(newpath.LastIndexOf('.')), i);
                            if (!File.Exists(uniquePath))
                            {
                                newpath = uniquePath;
                                break;
                            }
                        }
                        break;

                    default:
                        throw new IOException();
                }
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
        /// Gets the specified folder from the current folder. 
        /// </summary>
        /// <param name="name">The name of the child folder to retrieve.</param>
        /// <returns>When this method completes successfully, it returns a StorageFolder that represents the child folder.</returns>
        public async Task<StorageFolder> GetFolderAsync(string name)
        {
#if __ANDROID__ || __IOS__
            string folderpath = global::System.IO.Path.Combine(Path, name);

            if (!global::System.IO.Directory.Exists(folderpath))
            {
                throw new FileNotFoundException();
            }

            return new StorageFolder(folderpath);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            var folder = await _folder.GetFolderAsync(name);
            return folder == null ? null : new Storage.StorageFolder(folder);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the folders in the current folder.
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<StorageFolder>> GetFoldersAsync()
        {
            List<StorageFolder> folders = new List<StorageFolder>();
#if __ANDROID__ || __IOS__
            foreach (string foldername in global::System.IO.Directory.GetDirectories(Path))
            {
                folders.Add(new StorageFolder(global::System.IO.Path.Combine(Path, foldername)));
            }
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            foreach (Windows.Storage.StorageFolder folder in await _folder.GetFoldersAsync())
            {
                folders.Add(new StorageFolder(folder));
            }
#endif
            return folders;
        }

        /// <summary>
        /// Gets the parent folder of the current folder.
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

        public async Task<IStorageItem> TryGetItemAsync(string name)
        {
#if __ANDROID__ || __IOS__
            string itempath = global::System.IO.Path.Combine(Path, name);
            if(File.Exists(itempath))
            {
                return new StorageFile(itempath);
            }
            else if(Directory.Exists(itempath))
            {
                return new StorageFolder(itempath);
            }

            return null;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            Windows.Storage.IStorageItem item = await _folder.TryGetItemAsync(name);
            if(item.IsOfType(Windows.Storage.StorageItemTypes.File))
            {
                return new StorageFile(item as Windows.Storage.StorageFile);
            }
            else if(item.IsOfType(Windows.Storage.StorageItemTypes.Folder))
            {
                return new StorageFolder(item as Windows.Storage.StorageFolder);
            }

            return null;
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

        public bool IsOfType(StorageItemTypes type)
        {
            return type == StorageItemTypes.Folder;
        }
    }
}