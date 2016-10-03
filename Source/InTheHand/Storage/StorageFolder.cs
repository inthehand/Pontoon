//-----------------------------------------------------------------------
// <copyright file="StorageFolder.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.FileProperties;

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
[assembly: TypeForwardedTo(typeof(Windows.Storage.IStorageFolder))]
[assembly: TypeForwardedTo(typeof(Windows.Storage.StorageFolder))]
#else

namespace Windows.Storage
{
    /// <summary>
    /// Manipulates folders and their contents, and provides information about them.
    /// </summary>
    public interface IStorageFolder : IStorageItem
    {
        IAsyncOperation<StorageFile> CreateFileAsync(string desiredName);
        IAsyncOperation<StorageFile> CreateFileAsync(string desiredName, CreationCollisionOption options);
        IAsyncOperation<StorageFolder> CreateFolderAsync(string desiredName);
        IAsyncOperation<StorageFolder> CreateFolderAsync(string desiredName, CreationCollisionOption options);
        IAsyncOperation<StorageFile> GetFileAsync(string filename);
        IAsyncOperation<IReadOnlyList<StorageFile>> GetFilesAsync();
        IAsyncOperation<StorageFolder> GetFolderAsync(string name);
        IAsyncOperation<IReadOnlyList<StorageFolder>> GetFoldersAsync();
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
        public static IAsyncOperation<StorageFolder> GetFolderFromPathAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

#if __ANDROID__ || __IOS__
            return Task.FromResult<StorageFolder>(new StorageFolder(path)).AsAsyncOperation<StorageFolder>();
#else
            throw new PlatformNotSupportedException();
#endif
        }
        
        private string _path;

        internal StorageFolder(string path)
        {
            _path = path;
        }

        /// <summary>
        /// Creates a new file with the specified name in the current folder.
        /// </summary>
        /// <param name="desiredName">The name of the new file to create in the current folder.</param>
        /// <returns>When this method completes, it returns a StorageFile that represents the new file.</returns>
        public IAsyncOperation<StorageFile> CreateFileAsync(string desiredName)
        {
            return CreateFileAsync(desiredName, CreationCollisionOption.FailIfExists);
        }

        /// <summary>
        /// Creates a new file with the specified name in the current folder.
        /// </summary>
        /// <param name="desiredName">The name of the new file to create in the current folder.</param>
        /// <param name="options">One of the enumeration values that determines how to handle the collision if a file with the specified desiredName already exists in the current folder.</param>
        /// <returns>When this method completes, it returns a StorageFile that represents the new file.</returns>
        public IAsyncOperation<StorageFile> CreateFileAsync(string desiredName, CreationCollisionOption options)
        {
#if __ANDROID__ || __IOS__
            return Task.Run<StorageFile>(() => {
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
                            for (int i = 1; i < 100; i++)
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

                return new StorageFile(filepath);

            }).AsAsyncOperation<StorageFile>();
#else
            throw new PlatformNotSupportedException();
#endif
            }

        /// <summary>
        /// Creates a new subfolder with the specified name in the current folder.
        /// </summary>
        /// <param name="desiredName">The name of the new subfolder to create in the current folder.</param>
        /// <returns>When this method completes, it returns a StorageFolder that represents the new subfolder.</returns>
        public IAsyncOperation<StorageFolder> CreateFolderAsync(string desiredName)
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
        public IAsyncOperation<StorageFolder> CreateFolderAsync(string desiredName, CreationCollisionOption options)
        {
#if __ANDROID__ || __IOS__
            return Task.Run(() =>
            {
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
            }).AsAsyncOperation<StorageFolder>();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Deletes the current folder.
        /// </summary>
        /// <returns></returns>
        public IAsyncAction DeleteAsync()
        {
            return DeleteAsync(StorageDeleteOption.Default);
        }

        /// <summary>
        /// Deletes the current folder.
        /// This method also specifies whether to delete the folder permanently.
        /// </summary>
        /// <returns></returns>
        public IAsyncAction DeleteAsync(StorageDeleteOption option)
        {
#if __ANDROID__ || __IOS__
            if (!Directory.Exists(Path))
            {
                throw new FileNotFoundException();
            }
            return Task.Run(() =>
            {
                global::System.IO.Directory.Delete(Path,true);
            }).AsAsyncAction();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the basic properties of the current folder.
        /// </summary>
        /// <returns></returns>
        public IAsyncOperation<BasicProperties> GetBasicPropertiesAsync()
        {
            return Task.FromResult<BasicProperties>(new BasicProperties(this)).AsAsyncOperation<BasicProperties>();
        }

        /// <summary>
        /// Gets the file with the specified name from the current folder. 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public IAsyncOperation<StorageFile> GetFileAsync(string filename)
        {
#if __ANDROID__ || __IOS__
            return Task.Run<StorageFile>(() =>
            {
                string filepath = global::System.IO.Path.Combine(Path, filename);

                if (!global::System.IO.File.Exists(filepath))
                {
                    throw new FileNotFoundException();
                }

                return new StorageFile(filepath);
            }).AsAsyncOperation<StorageFile>();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the files in the current folder.
        /// </summary>
        /// <returns></returns>
        public IAsyncOperation<IReadOnlyList<StorageFile>> GetFilesAsync()
        {
#if __ANDROID__ || __IOS__
            return Task.Run<IReadOnlyList<StorageFile>>(() =>
            {
                List<StorageFile> files = new List<StorageFile>();

                foreach (string filename in global::System.IO.Directory.GetFiles(Path))
                {
                    files.Add(new StorageFile(global::System.IO.Path.Combine(Path, filename)));
                }

                return files;
            }).AsAsyncOperation<IReadOnlyList<StorageFile>>();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the specified folder from the current folder. 
        /// </summary>
        /// <param name="name">The name of the child folder to retrieve.</param>
        /// <returns>When this method completes successfully, it returns a StorageFolder that represents the child folder.</returns>
        public IAsyncOperation<StorageFolder> GetFolderAsync(string name)
        {
#if __ANDROID__ || __IOS__
            return Task.Run<StorageFolder>(() =>
            {
                string folderpath = global::System.IO.Path.Combine(Path, name);

                if (!global::System.IO.Directory.Exists(folderpath))
                {
                    throw new FileNotFoundException();
                }

                return new StorageFolder(folderpath);
            }).AsAsyncOperation<StorageFolder>();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the folders in the current folder.
        /// </summary>
        /// <returns></returns>
        public IAsyncOperation<IReadOnlyList<StorageFolder>> GetFoldersAsync()
        {
            List<StorageFolder> folders = new List<StorageFolder>();
#if __ANDROID__ || __IOS__
            return Task.Run<IReadOnlyList<StorageFolder>>(() =>
            {
                foreach (string foldername in global::System.IO.Directory.GetDirectories(Path))
                {
                    folders.Add(new StorageFolder(global::System.IO.Path.Combine(Path, foldername)));
                }

                return folders;
            }).AsAsyncOperation<IReadOnlyList<StorageFolder>>();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the parent folder of the current folder.
        /// </summary>
        /// <returns>When this method completes, it returns the parent folder as a StorageFolder.</returns>
        public IAsyncOperation<StorageFolder> GetParentAsync()
        {
#if __ANDROID__ || __IOS__
            return Task.Run<StorageFolder>(() =>
            {
                var parent = global::System.IO.Directory.GetParent(Path);
                return parent == null ? null : new StorageFolder(parent.FullName);
            }).AsAsyncOperation<StorageFolder>();
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

        public IAsyncOperation<IStorageItem> TryGetItemAsync(string name)
        {
#if __ANDROID__ || __IOS__
            return Task.Run<IStorageItem>(() =>
            {
                string itempath = global::System.IO.Path.Combine(Path, name);
                if (File.Exists(itempath))
                {
                    return new StorageFile(itempath);
                }
                else if (Directory.Exists(itempath))
                {
                    return new StorageFolder(itempath);
                }

                return null;
            }).AsAsyncOperation<IStorageItem>();
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
#endif